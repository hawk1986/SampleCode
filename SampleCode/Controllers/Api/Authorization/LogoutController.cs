using System.Net;
using System.Net.Http;
using System.Web.Http;
using PRS_New.Attributes;
using PRS_New.Interface;
using PRS_New.Manager;
using PRS_New.ViewModel;

namespace PRS_New.Controllers.Api.Authorization
{
    [RoutePrefix("api/logout")]
    public class LogoutController : ApiController
    {
        readonly IUserManager _userManager;

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="userManager"></param>
        public LogoutController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [TokenAuthorize(Code = "Pass")]
        public HttpResponseMessage Get()
        {
            //初始化回傳物件
            var resp = new HttpResponseMessage();
            if (TokenManager.RemoveUser(out UserViewModel viewModel))
            {
                _userManager.ClearToken(viewModel);
                resp.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                resp.StatusCode = HttpStatusCode.NotAcceptable;
            }
            return resp;
        }
    }
}