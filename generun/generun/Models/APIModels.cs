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
        public string userName { get; set; }
        public int score { get; set; }
        public string level
        {
            get
            {
                return String.Format("{0}/{1}.txt", HttpContext.Current.Server.MapPath("~/CRUK_data"), _level);
            }
            set
            {
                _level = value;
            }
        }
        private string _level { get; set; }
        public int[] classificationResult { get; set; }
    }
}