using generun.EngineLoader;
using generun.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace generun.Controllers
{
    public class ResultsController : ApiController
    {
        public HttpResponseMessage Post(LevelResult result) //lines Data etc etc
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {

                if (!DB.LevelDB.ContainsKey(result.userName))
                {
                    DB.LevelDB.Add(result.userName, new Dictionary<string, LevelResult>() { { result.level, result } });
                }
                else
                {
                    Dictionary<string, LevelResult> userResults = DB.LevelDB[result.userName];

                    if (userResults.ContainsKey(result.level) && userResults[result.level].score < result.score)
                        userResults[result.level] = result;
                    else if (!userResults.ContainsKey(result.level))
                        userResults.Add(result.level, result);
                }

                //Store => DB
                response.StatusCode = HttpStatusCode.OK;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        public HttpResponseMessage 
    }
}
