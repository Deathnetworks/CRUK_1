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

            return new List<string>() { testList.First(x => x.EndsWith("Chrom2")), testList.First(x => x.EndsWith("Chrom8")), testList.First(x => x.EndsWith("Chrom4")), testList.First(x => x.EndsWith("Chrom12")) };
        }

        // GET api/level/5
        public byte[] Get(string LevelName, int Height, int Width)
        {
            byte[] returnVal;

            returnVal = Engine.GetLevel(LevelName, Height, Width);

            return returnVal;
        }
    }
}
