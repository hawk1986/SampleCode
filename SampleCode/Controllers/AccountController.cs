using SampleCode.Interface;
using SampleCode.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using ResourceLibrary;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Utilities;
using Utilities.Attribute;
using Utilities.Extensions;
using Utilities.Utility;
using SampleCode.Manager;

namespace SampleCode.Controllers
{
    public class AccountController : BaseController
    {
        readonly ICommonManager _commonManager;
        readonly IUserManager _userManager;
        protected IAuthenticationManager Authentication => HttpContext.GetOwinContext().Authentication;

        public AccountController(
            ICommonManager commonManager,
            IUserManager userManager)
        {
            _commonManager = commonManager;
            _userManager = userManager;
            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <returns></returns>
        [WebAuthorize(Code = "Pass")]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            var user = GetUser();
            return View(new ChangePasswordViewModel { Account = user.Account });
        }

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [WebAuthorize(Code = "Pass")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel.NewPassword == viewModel.ConfirmPassword)
            {
                var user = GetUser();
                // 只能變更自己的密碼
                viewModel.Account = viewModel.Account.ToLower();
                if (user.Account.ToLower() == viewModel.Account && _userManager.ChangePassword(viewModel))
                {
                    viewModel.Password = string.Empty;
                    viewModel.NewPassword = string.Empty;
                    viewModel.ConfirmPassword = string.Empty;
                    // 完成
                    TempData["ErrorMsg"] = new JsonMessage { Style = "success", Message = Resource.ChangePasswordComplete }.ToString();
                    return View(viewModel);
                }
                else
                {
                    TempData["ErrorMsg"] = new JsonMessage { Style = "danger", Message = Resource.ChangePasswordFailed }.ToString();
                }
            }
            return View(viewModel);
        }

        /// <summary>
        /// 檢查電子郵件是否存在
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Initial"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EmailBeUse(string Email)
        {
            var result = false;
            Email = Email.Trim().ToLower();
            try
            {
                result = _userManager.EmailBeUse(Email);
            }
            catch (Exception ex)
            {
                logger.Error(ex, string.Format(Resource.CheckBeUsedError, Resource.User, Resource.Email));
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            var viewModel = new ForgotPasswordViewModel();

            return View(viewModel);
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.Email = viewModel.Email.Trim().ToLower();
                    viewModel.NowTime = NowTime;
                    var hashToken = _userManager.ForgotPassword(viewModel);
                    sendForgotPasswordMail(viewModel.Email, hashToken);
                    TempData["ErrorMsg"] = new JsonMessage { Style = "success", Message = Resource.ForgotPasswordProcessCompleted }.ToString();
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, Resource.ForgotPasswordProcessFailed);
                    TempData["ErrorMsg"] = new JsonMessage { Style = "danger", Message = Resource.ForgotPasswordProcessFailed }.ToString();
                }
            }

            return View(viewModel);
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(ValidateUserViewModel viewModel)
        {
            bool isLockAccount = false;
            if (ModelState.IsValid)
            {
                //判斷是否鎖定帳號
                isLockAccount = LoginLockManager.IsLockAccount(viewModel.Account);
                //驗證碼檢查
                if (!isLockAccount && CaptchaManager.RemoveCaptcha(viewModel.CaptchaID, viewModel.CaptchaCode))
                {
                    viewModel.NowTime = NowTime;
                    var user = _userManager.ValidateUser(viewModel);
                    if (null != user)
                    {
                        // 登入成功
                        LoginLockManager.LoginSuccess(viewModel.Account);
                        var auth = user.Auth;
                        UnobtrusiveSession.Session["User"] = user;
                        UnobtrusiveSession.Session["Auth"] = auth;
                        // 設定驗證
                        var identity = new ClaimsIdentity(
                            new[] {
                            new Claim(ClaimTypes.Name, user.Account),
                            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Account),
                            new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", user.Account)
                            },
                            DefaultAuthenticationTypes.ApplicationCookie,
                            ClaimTypes.Name,
                            ClaimTypes.Role);
                        // 取得 web.config 設定的系統 timmeout 時間
                        var systemTimeout = Tools.GetConfigValue("SystemTimeout", 60);
                        // 設定 session timeout
                        Authentication.SignIn(new AuthenticationProperties { ExpiresUtc = DateTime.SpecifyKind(NowTime.AddMinutes(systemTimeout), DateTimeKind.Local) }, identity);
                        // 如果 ReturnUrl 不為空且不等於 / 則進行轉址
                        if (!string.IsNullOrWhiteSpace(viewModel.ReturnUrl) && viewModel.ReturnUrl != "/")
                        {
                            return Redirect(viewModel.ReturnUrl);
                        }
                        else
                        {
                            // 尋找 user 預設首頁
                            var function = user.Auth.OrderBy(x => x.FunctionID).FirstOrDefault(x => x.FunctionID == user.DefaultIndex);
                            if (null != function)
                            {
                                return RedirectToAction(function.ActionName, function.ControllerName);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }

                //紀錄登錄失敗
                LoginLockManager.LoginFailed(viewModel.Account, Request.UserHostAddress);
            }
            ModelState.AddModelError("", isLockAccount ? Resource.AccountLock : Resource.LoginFailed);
            setDropDownList(ref viewModel);

            //更驗驗證碼
            viewModel.CaptchaID = Guid.NewGuid();
            string code = new CaptchaImage().GetCaptchaImage(out byte[] image);
            CaptchaManager.InsertCaptcha(viewModel.CaptchaID, code);
            viewModel.CaptchaData = Convert.ToBase64String(image);

            return View(viewModel);
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var viewModel = new ValidateUserViewModel();
            viewModel.ReturnUrl = returnUrl;
            setDropDownList(ref viewModel);

            //取得驗證碼
            viewModel.CaptchaID = Guid.NewGuid();
            string code = new CaptchaImage().GetCaptchaImage(out byte[] image);
            CaptchaManager.InsertCaptcha(viewModel.CaptchaID, code);
            viewModel.CaptchaData = Convert.ToBase64String(image);

            return View(viewModel);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            UnobtrusiveSession.Session["User"] = null;
            UnobtrusiveSession.Session["Auth"] = null;
            Authentication.SignOut();

            return RedirectToAction("Login");
        }

        /// <summary>
        /// 忘記密碼重置作業
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.NowTime = NowTime;
                    var result = _userManager.ResetPassword(viewModel);
                    TempData["ErrorMsg"] = result ?
                        new JsonMessage { Style = "success", Message = Resource.ResetPasswordProcessCompleted }.ToString() :
                        new JsonMessage { Style = "danger", Message = Resource.ForgotPasswordProcessFailed }.ToString();
                    if (result)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, Resource.ForgotPasswordProcessFailed);
                    TempData["ErrorMsg"] = new JsonMessage { Style = "danger", Message = Resource.ForgotPasswordProcessFailed }.ToString();
                }
            }

            return View(viewModel);
        }

        /// <summary>
        /// 忘記密碼重置作業
        /// </summary>
        /// <param name="hashToken"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResetPassword(string hashToken)
        {
            var viewModel = new ResetPasswordViewModel();
            viewModel.HashToken = hashToken;

            return View(viewModel);
        }

        /// <summary>
        /// 發送忘記密碼郵件
        /// </summary>
        /// <param name="userMail"></param>
        /// <param name="hashToken"></param>
        void sendForgotPasswordMail(string userMail, string hashToken)
        {
            var fromName = Tools.GetConfigValue("AdminName", "admin");
            var fromAddress = Tools.GetConfigValue("AdminMail", "admin@net.com.tw");
            var toAddress = new string[] { userMail };
            var subject = string.Format(Resource.ForgotPasswordMailSubject, Resource.SystemName);
            var body = string.Format(
                Resource.ForgotPasswordMailBody,
                Resource.SystemName,
                Tools.GetConfigValue("DomainName", "") + Url.Action("ResetPassword", "Account", new { hashToken = HttpUtility.UrlEncode(hashToken) })).Replace("[", "<").Replace("]", ">");
            var mail = new SendMail(fromName, fromAddress, toAddress, subject, body);
            mail.Send();
        }

        /// <summary>
        /// 設定頁面所需要的下拉選單資料
        /// </summary>
        /// <param name="viewModel"></param>
        void setDropDownList(ref ValidateUserViewModel viewModel)
        {
            /* Do Nothing */
        }
    }
}