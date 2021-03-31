using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SampleCode.Interface;
using SampleCode.ViewModel;

namespace SampleCode.Controllers.Api.Public
{
    [RoutePrefix("api/resetPassword")]
    public class ResetPasswordController : ApiController
    {
        readonly IUserManager _userManager;

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="userManager"></param>
        public ResetPasswordController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public HttpResponseMessage Post(ResetPasswordViewModel viewModel)
        {
            //若資料驗證未過，傳回錯誤訊息
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            //初始化回傳物件
            var resp = new HttpResponseMessage();
            if (_userManager.ResetPassword(viewModel))
                resp.StatusCode = HttpStatusCode.OK;
            else
                resp.StatusCode = HttpStatusCode.NotFound;
            return resp;
        }
    }
}