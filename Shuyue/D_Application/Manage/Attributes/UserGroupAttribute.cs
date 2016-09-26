using ManageService.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manage.Attributes
{
    /// <summary>
    /// 用户拥有管理组是否有权操作判断
    /// </summary>
    public class UserGroupAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
            var action = filterContext.RouteData.Values["action"].ToString().ToLower();
            bool flag = false;
            var r = filterContext.HttpContext.Request;
            if (Core.AppContext.Current.CurrentUser.UserCode == "sa") return;
            if (r.IsAjaxRequest()) return;
            if (Core.AppContext.Current.UserMenu == null)
            {
                MenuBLL menuBLL = new MenuBLL();
                menuBLL.GetUserMenu();
            }
            if (Core.AppContext.Current.UserMenu == null) flag = false;
            else flag = Core.AppContext.Current.UserMenu.Any(m => m.ControllName == controller && m.ActionName == action);
            if (!flag)
            {
                filterContext.RequestContext.HttpContext.Response.Redirect("/Error/?msg=您没有操作权限", true);
                filterContext.RequestContext.HttpContext.Response.End();
                filterContext.Result = new EmptyResult();
            }
        }
    }
}