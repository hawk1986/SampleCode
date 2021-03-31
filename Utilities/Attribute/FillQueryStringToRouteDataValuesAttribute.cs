using System.Web.Mvc;

namespace Utilities.Attribute
{
    public class FillQueryStringToRouteDataValuesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //store querystring to routedata
            var querystring = filterContext.HttpContext.Request.QueryString;
            var routeValueDictionary = filterContext.Controller.ControllerContext.RouteData.Values;

            for (int i = 0; i < querystring.Count; i++)
            {
                var value = querystring[i];
                var key = querystring.Keys[i];
                if (string.IsNullOrEmpty(value))
                {
                    routeValueDictionary.Remove(key);
                    continue;
                }
                if (key != "page" && key != "X-Requested-With" && key != "Random")
                {
                    routeValueDictionary[querystring.Keys[i]] = querystring[i];
                }
            }

            var form = filterContext.HttpContext.Request.Form;

            for (int i = 0; i < form.Count; i++)
            {
                try
                {
                    var value = form[i];
                    var key = form.Keys[i];
                    if (string.IsNullOrEmpty(value))
                    {
                        routeValueDictionary.Remove(key);
                        continue;
                    }
                    routeValueDictionary[form.Keys[i]] = form[i];
                }
                catch { /* do nothing */ }
            }

            routeValueDictionary.Remove("X-Requested-With");
        }
    }
}
