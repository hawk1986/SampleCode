using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using SampleCode.Interface;
using SampleCode.Manager;
using SampleCode.Models;
using SampleCode.ViewModel;

namespace SampleCode.Controllers.Api.Public
{
    [RoutePrefix("api/login")]
    public class LoginController : BaseApiController
    {
        readonly IUserManager _userManager;

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="userManager"></param>
        public LoginController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public HttpResponseMessage Post(ValidateApiUserViewModel viewModel)
        {
            //初始化回傳物件
            var resp = new HttpResponseMessage();
            if (viewModel != null && ModelState.IsValid &&
                CaptchaManager.RemoveCaptcha(viewModel.CaptchaID, viewModel.CaptchaCode))
            {
                viewModel.NowTime = NowTime;
                var user = _userManager.ValidateUserByApi(viewModel);
                if (user != null)
                {
                    resp.StatusCode = HttpStatusCode.OK;
                    resp.Content = new ObjectContent<UserToken>(user, new JsonMediaTypeFormatter(), "application/json");
                    return resp;
                }
            }

            resp.StatusCode = HttpStatusCode.NotAcceptable;
            return resp;
        }
    }
}