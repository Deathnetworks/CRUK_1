using generun.EngineLoader;
using generun.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;

namespace generun.Helper
{
    public static class FaceBook
    {
        public static HttpResponseMessage PostScore(string Access_Token, string levelName, int score)
        {
            //return callFaceBook("Score", _HandleJson.ConvertJsonToParams(new { id = levelName, score = score, access_token = Access_Token }), HttpMethods.POST);

            HttpResponseMessage mockedMessage = new HttpResponseMessage();
            mockedMessage.StatusCode = HttpStatusCode.OK;
            return mockedMessage;
        }

        public static HttpResponseMessage PostAchievement(string Access_Token, string userID)
        {
            return callFaceBook("Achievements",
                _HandleJson.ConvertJsonToParams(new { id = 1, user_id = userID, access_token = Access_Token }),
                HttpMethods.POST);
        }

        public static HttpResponseMessage callFaceBook(string path, string parameters, HttpMethods Method)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(string.Format("http://crukhack.eu01.aws.af.cm/{0}{1}", path, Method == HttpMethods.POST ? "" : parameters));
                
                Request.Method = Method.ToString();

                if (Method == HttpMethods.POST)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(parameters);
                    Request.ContentType = "application/x-www-form-urlencoded";
                    Request.ContentLength = buffer.Length;

                    Request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    Request.GetRequestStream().Close();
                }

                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();

                response.StatusCode = Response.StatusCode;

            }
            catch (WebException ex)
            {
                if ((HttpWebResponse)ex.Response != null)
                    response.StatusCode = ((HttpWebResponse)ex.Response).StatusCode;
                else
                    response.StatusCode = HttpStatusCode.BadRequest;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }

        public static Achievement getLatestAchievement(string userID, string levelName)
        {
            LevelResult result = DB.LevelDB[userID][levelName];

            return Achievement.None;
        }
    }

    internal class _HandleJson
    {
        public static string ConvertJsonToParams(object Object)
        {
            string Json = JsonConvert.SerializeObject(Object);

            var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize<Dictionary<string, dynamic>>(Json);

            string paramSuffix = ParseDictionary(dict);

            if (paramSuffix.EndsWith("&"))
                paramSuffix = paramSuffix.Substring(0, paramSuffix.Length - 1);

            return Uri.EscapeUriString(paramSuffix);
        }

        internal static string ParseDictionary(Dictionary<string, dynamic> Dict, string ParentKey = null)
        {
            string stringparse = string.Empty;
            foreach (KeyValuePair<string, dynamic> key in Dict)
            {
                if (key.Value is Dictionary<string, dynamic>)
                    stringparse += ParseDictionary(key.Value, ParentKey != null ? String.Format("{0}[{1}]", ParentKey, key.Key) : key.Key);
                else if (key.Value is System.Collections.ArrayList)
                {
                    foreach (var item in key.Value)
                    {
                        stringparse += String.Format("{0}={1}&", ParentKey != null ? String.Format("{0}[{1}]", ParentKey, key.Key) : key.Key, Convert.ToString(item));
                    }
                }
                else
                    stringparse += String.Format("{0}={1}&", ParentKey != null ? String.Format("{0}[{1}]", ParentKey, key.Key) : key.Key, Convert.ToString(key.Value));
            }

            return stringparse;
        }
    }
}