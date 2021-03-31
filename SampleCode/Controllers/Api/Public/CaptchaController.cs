using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Utilities.Utility;
using SampleCode.Manager;
using SampleCode.ViewModel;

namespace SampleCode.Controllers.Api.Public
{
    [RoutePrefix("api/captcha")]
    public class CaptchaController : ApiController
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            //取得驗證碼內容資訊
            string strDraw = new CaptchaImage().GetCaptchaImage(out byte[] image);

            //初始新的驗證編號
            Guid id = Guid.NewGuid();
            CaptchaManager.InsertCaptcha(id, strDraw);

            //初始化回傳物件
            var resp = new HttpResponseMessage();
            resp.StatusCode = HttpStatusCode.OK;
            resp.Content = new ObjectContent<SimpleDataViewModel>(
                new SimpleDataViewModel { ID = id, Data = Convert.ToBase64String(image) },
                new JsonMediaTypeFormatter(), "application/json");
            return resp;
        }
    }
}