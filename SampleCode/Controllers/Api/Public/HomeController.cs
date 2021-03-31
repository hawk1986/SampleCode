using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using SampleCode;
using SampleCode.Models.Public;

namespace SampleCode.Controllers.Api.Public
{
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string culture = "tw")
        {
            //初始化回傳物件
            var resp = new HttpResponseMessage();
            //判斷語系是否支援
            if (Enum.TryParse(culture, out Culture cs))
            {
                resp.StatusCode = HttpStatusCode.OK;
                resp.Content = new ObjectContent<HomeInfo>(Global.GetHomeInfo(cs), new JsonMediaTypeFormatter(), "application/json");
            }
            else
            {
                resp.StatusCode = HttpStatusCode.NotFound;
            }
            return resp;
        }
    }
}