using generun.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Net.Http;

namespace generun.EngineLoader
{
    public static class DB
    {
        public static Dictionary<string, Dictionary<string, LevelResult>> LevelDB = new Dictionary<string, Dictionary<string, LevelResult>>();

        public static void ClearResults()
        {
            LevelDB.Clear();
        }

        public static void ClearResults(string username)
        {
            if (LevelDB.ContainsKey(username))
                LevelDB.Remove(username);
        }

        public static void ClearResults(string username, string levelName)
        {
            if (LevelDB.ContainsKey(username))
            {
                Dictionary<string, LevelResult> userDB = LevelDB[username];
                if (userDB.ContainsKey(levelName))
                {
                    userDB.Remove(levelName);
                }
            }

        }

        

        public static LevelResult GetBestLevelResult(string LevelName)
        {
            var test = from x in LevelDB
                       where x.Value.ContainsKey(LevelName)
                       select (x.Value as Dictionary<string, LevelResult>);

            var testing = (from x in test
                           select x[LevelName]).OrderByDescending(x => (x.score)).First();

            return testing;
        }
    }

    public static class Engine
    {
        public static HttpResponseMessage HTTPResponseFromBitmap(Bitmap canvas, HttpRequestMessage Request)
        {
            var ms = new MemoryStream();
            canvas.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            HttpResponseMessage r = Request.CreateResponse();
            r.Content = new ByteArrayContent(ms.ToArray());
            ms.Close();
            ms.Dispose();
            r.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            return r;
        }

        public static byte[] GetLevel(string LevelName, int Height, int Width)
        {
            string newString = string.Empty;
            byte[] bits = new byte[(int)(Width * (double)Height / 8 + 0.5)];

            string filePath = HttpContext.Current.Server.MapPath(string.Format("~/CRUK_data/{0}.txt", LevelName)),
                line = string.Empty;

            int minX = int.MaxValue,
                       maxX = int.MinValue;

            if (File.Exists(filePath))
                using (StreamReader reader = new StreamReader(filePath))
                {
                    line = reader.ReadLine(); //skip firstline

                    while ((line = reader.ReadLine()) != null)
                    {

                        string[] data = line.Split('\t');


                        int chromosome = Convert.ToInt32(data[0]),
                        x = Convert.ToInt32(data[1]);

                        double y = Convert.ToDouble(data[2]);

                        if (y >= 0.5)
                            continue;

                        minX = Math.Min(minX, x);
                        maxX = Math.Max(maxX, x);

                    }

                    reader.BaseStream.Position = 0;
                    reader.DiscardBufferedData();

                    line = reader.ReadLine(); //skip firstline

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split('\t');


                        int chromosome = Convert.ToInt32(data[0]),
                        x = Convert.ToInt32(data[1]);

                        double y = Convert.ToDouble(data[2]);

                        if (y >= 0.5)
                            continue;

                        int xBitmap = (int)((double)x / (maxX - minX) * Width),
                        yBitmap = (int)(Height * 2 * y),
                        index = (xBitmap + yBitmap * Width) / 8;

                        var bit = (xBitmap + yBitmap * Width) % 8;
                        bits[index] |= Convert.ToByte(1 << bit);
                    }
                }

            //Process Data
            return bits; //return data
        }

        
    }
}