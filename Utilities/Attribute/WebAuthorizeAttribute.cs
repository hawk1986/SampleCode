using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Utilities.Utility;

namespace Utilities.Attribute
{
    public class WebAuthorizeAttribute : AuthorizeAttribute
    {
        public string Code { get; set; }

        private readonly string _permissionsDenied = "權限不足，請更換登入帳號！";

        void ajaxResult(ref AuthorizationContext filterContext)
        {
            var cr = new ContentResult();
            cr.Content = _permissionsDenied;
            filterContext.Result = cr;
        }

        void notAjaxResult(ref AuthorizationContext filterContext)
        {
            if (null != UnobtrusiveSession.Session &&
                null != UnobtrusiveSession.Session["User"] &&
                null != UnobtrusiveSession.Session["Auth"])
            {
                filterContext.Controller.TempData["ErrorMsg"] = new JsonMessage { Style = "danger", Message = _permissionsDenied }.ToString();
                if (null != filterContext.HttpContext.Request.UrlReferrer)
                {
                    filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri);
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                            { "area", "" },
                            { "controller", "Account" },
                            { "action", "Login" },
                            { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
                        }
                    );
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "area", "" },
                        { "controller", "Account" },
                        { "action", "Login" },
                        { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
                    }
                );
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult ||
                null == UnobtrusiveSession.Session ||
                null == UnobtrusiveSession.Session["User"] ||
                null == UnobtrusiveSession.Session["Auth"])
            {
                noneAuthorizationProcess(ref filterContext);
            }
            else
            {
                if (Code != "Pass")
                {
                    var authList = UnobtrusiveSession.Session["Auth"] as List<Auth>;
                    if (null != authList && authList.Count > 0)
                    {
                        string controllerName = filterContext.RouteData.Values["controller"].ToString().Trim().ToLower();
                        string actionName = filterContext.RouteData.Values["action"].ToString().Trim().ToLower();

                        IEnumerable<Auth> auth;

                        if (string.IsNullOrWhiteSpace(Code))
                        {
                            auth = authList.Where(x =>
                                x.ControllerName.ToLower() == controllerName &&
                                x.ActionName.ToLower() == actionName);
                        }
                        else
                        {
                            var tempCode = Code.Split(',');
                            auth = authList.Where(x => tempCode.Contains(x.Code));
                        }

                        if (!auth.Any())
                        {
                            noneAuthorizationProcess(ref filterContext);
                        }
                    }
                    else
                    {
                        noneAuthorizationProcess(ref filterContext);
                    }
                }
            }
        }

        void noneAuthorizationProcess(ref AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ajaxResult(ref filterContext);
            }
            else if (!filterContext.IsChildAction)
            {
                notAjaxResult(ref filterContext);
            }
        }
    }
}
