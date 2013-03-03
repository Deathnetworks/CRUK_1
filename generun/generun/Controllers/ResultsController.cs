using generun.EngineLoader;
using generun.Helper;
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
        
        ///api/results?userMean=0.79&floatList[]=0.765432&floatList[]=0.8456789&floatList[]=0.712345678&floatList[]=0.765432345&floatList[]=0.75987655432&difficulty=0.35
        public string getMedian([FromUri]float[] floatList, float userMean, decimal difficulty = 1)
        {
            List<decimal> results = new List<decimal>();

            floatList.ToList().ForEach(x => results.Add((decimal)x));

            decimal userCompared = MathFunctions.Sd(new List<decimal>() { MathFunctions.Mean(results), (decimal)userMean });

            decimal testCV = MathFunctions.Cv(MathFunctions.Mean(results), userCompared);

            if (testCV <= (3 * difficulty))
                return "Perfect!!";
            else if (testCV <= (6 * difficulty))
                return "Good";
            else if (testCV <= (9 * difficulty))
                return "Almost Missed that one";
            else
                return "Are you fucking blind?!";
        }
    }
}
