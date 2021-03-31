using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PRS_New.Interface;
using PRS_New.ViewModel;
using Utilities.Utility;

namespace PRS_New.Controllers.Api.Public
{
    [RoutePrefix("api/forgetPassword")]
    public class ForgetPasswordController : BaseApiController
    {
        readonly IUserManager _userManager;

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="userManager"></param>
        public ForgetPasswordController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public HttpResponseMessage Post(ForgotPasswordViewModel viewModel)
        {
            //若資料驗證未過，傳回錯誤訊息
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            //初始化回傳物件
            var resp = new HttpResponseMessage();
            viewModel.Email = viewModel.Email.Trim().ToLower();
            viewModel.NowTime = DateTime.Now;
            var hashToken = _userManager.ForgotPassword(viewModel);
            if (!string.IsNullOrEmpty(hashToken))
            {
                resp.StatusCode = HttpStatusCode.OK;
                //發送電子回函，取得電子信範本資料
                sendMail("~/App_Data/ForgetPassword.txt", viewModel.Email, "變更密碼確認信",
                    string.Concat(Tools.GetConfigValue("DomainName", string.Empty), "reset-password?token=", hashToken));
            }
            else
            {
                resp.StatusCode = HttpStatusCode.NotFound;
            }
            return resp;
        }
    }
}