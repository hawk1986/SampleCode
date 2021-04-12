using System.Net;
using System.Net.Http;
using System.Web.Http;
using SampleCode.Attributes;
using SampleCode.Interface;
using SampleCode.Manager;
using SampleCode.ViewModel;

namespace SampleCode.Controllers.Api.Authorization
{
    [RoutePrefix("api/mail")]
    public class MailController : ApiController
    {
        readonly IUserManager _userManager;

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