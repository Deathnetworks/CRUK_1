using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Linq;
using generun.EngineLoader;
using System.IO;
using System.Web;

namespace generun.Controllers
{
    public class LevelController : ApiController
    {
        // GET api/level
        public IEnumerable<string> Get()
        {
            IEnumerable<string> files = Directory.EnumerateFiles(HttpContext.Current.Server.MapPath("~/CRUK_data"));

            IEnumerable<string> testList = (from x in files
                         where x.Contains("BAF") & x.Contains("Chrom")
                         select Path.GetFileNameWithoutExtension(x));

            return testList; // return all level names
        }

        // GET api/level/5
        public byte[] Get(string LevelName, int Height, int Width)
        {
            byte[] returnVal;

            returnVal = new Engine().GetLevel(LevelName,Height,Width);
            
            return returnVal;
        }
    }
}
