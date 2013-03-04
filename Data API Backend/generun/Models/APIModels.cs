using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace generun.Models
{
    public class LevelResult
    {
        public string Access_Token { get; set; }
        public string userName { get; set; }
        public int score { get; set; }
        //public string levelServerLocation
        //{
        //    get
        //    {
        //        return String.Format("{0}/{1}.txt", HttpContext.Current.Server.MapPath("~/CRUK_data"), level);
        //    }
        //}
        public string level { get; set; }
        //public List<Achievement> achievements { get; set; }
        public int[] classificationResult { get; set; }
    }

    public enum HttpMethods
    {
        GET,
        POST,
        //PUT,
        DELETE
    }

    public enum Achievement
    {
        Just_Awesome = 100,
        Troll = 0,
        None = -1
    }
}