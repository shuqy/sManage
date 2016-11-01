using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockApp.Attributes
{
    public class SmAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = Core.AppContext.Current.CurrentUser;

            var r = filterContext.HttpContext.Request;
            var controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
            var action = filterContext.RouteData.Values["action"].ToString().ToLower();
            var rw = filterContext.HttpContext.Request.ServerVariables["X-Requested-With"];

            string url = "";
            if (!r.IsAjaxRequest())
            {
                url = r.Url.LocalPath;
                if (!string.IsNullOrEmpty(url))
                {
                    url = filterContext.HttpContext.Server.UrlEncode(url);
                }
            }

            if (user == null)
            {
                if (controller == "home" && action == "login")
                {

                }
                else
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        filterContext.RequestContext.HttpContext.Response.Redirect("/home/login?redirect=" + url, true);
                    }
                    else
                    {
                        filterContext.RequestContext.HttpContext.Response.Redirect("/home/login", true);
                    }
                    filterContext.RequestContext.HttpContext.Response.End();
                    filterContext.Result = new EmptyResult();
                }
            }
        }
    }
}