using generun.EngineLoader;
using generun.Helper;
using generun.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace generun.Controllers
{
    public class ResultsController : ApiController
    {
        public HttpResponseMessage PostResult([FromBody]LevelResult result) //lines Data etc etc
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool fireFaceBookScore = false,
                fireAchievement = false;

            try
            {
                if (!DB.LevelDB.ContainsKey(result.userName))
                {
                    DB.LevelDB.Add(result.userName, new Dictionary<string, LevelResult>() { { result.level, result } });
                    
                    FaceBook.PostAchievement(result.Access_Token, result.userName);
                    //return FaceBook.PostScore(result.Access_Token, result.level, result.score); //Always fire on new score?
                }
                Dictionary<string, LevelResult> userResults = DB.LevelDB[result.userName];

                if (userResults.ContainsKey(result.level) && userResults[result.level].score < result.score)
                {
                    userResults[result.level] = result;
                    fireFaceBookScore = true;
                }
                else if (!userResults.ContainsKey(result.level))
                {
                    userResults.Add(result.level, result);
                    fireAchievement = true;
                }

                //if (fireFaceBookScore)
                //    FaceBook.PostScore(result.Access_Token, result.level, result.score);
                if (fireAchievement)
                    FaceBook.PostAchievement(result.Access_Token, result.userName);

                response.StatusCode = HttpStatusCode.OK;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        public HttpResponseMessage getImage(string user_id, string levelName)
        {
            LevelResult lr;
            try
            {
                lr = DB.LevelDB[user_id][levelName];

            }
            catch
            {
                HttpResponseMessage msg = new HttpResponseMessage();
                msg.StatusCode = HttpStatusCode.Unauthorized;
                return msg;
            }
            if (lr == null)
            {
                HttpResponseMessage msg = new HttpResponseMessage();
                msg.StatusCode = HttpStatusCode.ExpectationFailed;
                return msg;
            }
            int[] classification = lr.classificationResult;
            byte[] bits = Engine.GetLevel(lr.level, 1000, 325);
            Bitmap bmp = new Bitmap(1000, 650);

            int x = 0;
            int y = 0;
            for (int i=0; i<bits.Length; ++i)
            {
                byte value = bits[i];
                for (int j=0; j<8; ++j)
                {
                    bool isOn = ((value & (1 << j)) > 0);
                    bmp.SetPixel(x, y, isOn ? Color.Black : Color.White);
                    bmp.SetPixel(x, 649 - y, isOn ? Color.Black : Color.White);
                    if (classification[x] == y)
                    {
                        bmp.SetPixel(x, y, Color.Red);
                        bmp.SetPixel(x, 649 - y, Color.Red);
                    }
                }
                ++x;
                if (x>=1000)
                {
                    x = 0;
                    y++;
                }
            }

            return Engine.HTTPResponseFromBitmap(bmp, Request);
        }

        ///api/results?userMean=0.79&floatList[]=0.765432&floatList[]=0.8456789&floatList[]=0.712345678&floatList[]=0.765432345&floatList[]=0.75987655432&difficulty=0.35
        public float getMedian([FromUri]float[] floatList, float userMean, decimal difficulty = 1)
        {
            List<decimal> results = new List<decimal>();

            floatList.ToList().ForEach(x => results.Add((decimal)x));

            decimal userCompared = MathFunctions.Sd(new List<decimal>() { MathFunctions.Mean(results), (decimal)userMean });

            decimal testCV = MathFunctions.Cv(MathFunctions.Mean(results), userCompared);

            if (testCV <= (3 * difficulty))
                return 1.00f;
            else if (testCV <= (6 * difficulty))
                return 0.66f;
            else if (testCV <= (9 * difficulty))
                return 0.33f;
            else
                return 0.00f;
        }
    }
}
