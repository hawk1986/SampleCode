using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using PRS_New;
using PRS_New.Models.Public;

namespace PRS_New.Controllers.Api.Public
{
    [RoutePrefix("api/option")]
    public class OptionController : ApiController
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string id)
        {
            //初始化回傳物件
            var resp = new HttpResponseMessage();
            resp.StatusCode = HttpStatusCode.OK;
            resp.Content = new ObjectContent<List<Option>>(Global.GetOptions(id), new JsonMediaTypeFormatter(), "application/json");
            return resp;
        }
    }
}