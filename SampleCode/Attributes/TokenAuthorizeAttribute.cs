using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Utilities;
using SampleCode.Manager;

namespace SampleCode.Attributes
{
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 驗證代碼
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 授權
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var user = TokenManager.GetUser();
            if (user == null)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            //權限驗證
            if (Code != "Pass")
            {
                if (user.Auth == null || user.Auth.Count == 0)
                {
                    HandleUnauthorizedRequest(actionContext);
                    return;
                }

                IEnumerable<Auth> auth;
                if (string.IsNullOrWhiteSpace(Code))
                {
                    //取得控制器及動作的名稱
                    string controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName.ToLower();
                    string actionName = actionContext.ActionDescriptor.ActionName.ToLower();
                    auth = user.Auth.Where(x => x.ControllerName.ToLower() == controllerName && x.ActionName.ToLower() == actionName);
                }
                else
                {
                    var tempCode = Code.Split(',');
                    auth = user.Auth.Where(x => tempCode.Contains(x.Code));
                }

                if (!auth.Any())
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
        }

        /// <summary>
        /// 未授權的處理
        /// </summary>
        /// <param name="actionContext"></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
        }
    }
}