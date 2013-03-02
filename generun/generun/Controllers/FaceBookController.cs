using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace generun.Controllers
{
    public class FaceBookController : ApiController
    {
        // GET api/facebook
        public string Get(string FBID)
        {
            string returnVal = string.Empty;

            //Access => DB
            //GetDBData
            //ReturnDBData

            return returnVal;
        }

        // GET api/facebook/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/facebook
        public void Post([FromBody]string value)
        {
        }

        // PUT api/facebook/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/facebook/5
        public void Delete(int id)
        {
        }
    }
}
