using Core.Entities;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Authorization {
    public class WeixinAuthorizationModule : IHttpModule {
        public void Dispose() { }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context) {
            //context.PostAcquireRequestState += context_PostAcquireRequestState;
            context.PreRequestHandlerExecute += context_PostAcquireRequestState;
        }

        /// <summary>
        /// 获取完会话状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void context_PostAcquireRequestState(object sender, EventArgs e) {
            HttpApplication application = (HttpApplication)sender;
            //判断当前url是否能忽略校验
            if (this.CanIgnoreVerify(application)) return;
            //如果会话当中保存了用户状态，则不需要继续后续校验步骤
            if (AppContext.Current.CurrentUser != null) return;

            string loginUrl;

            //是否要跳转到登录页
            if (this.IsNeedRedirectToLoginpage(application, out loginUrl)) {
                application.Response.Redirect(loginUrl);
                return;
            }
            WeixinLogin.Login(application.Context);

            //绑定手机跳转
            if (WeixinServiceType.QY.ToString() == ConfigHelper.Get(AppSettingsKey.WeixinServiceType)
                && string.IsNullOrEmpty(AppContext.Current.CurrentUser.Customer.Mobile)) {
                application.Response.Redirect("/Home/BindMobile?redirect_url=" + application.Request.Url);
                return;
            }
        }

        /// <summary>
        /// 是否需要跳转到登陆页面
        /// 可在此处做是否微信登陆判断
        /// </summary>
        /// <param name="application"></param>
        /// <param name="loginUrl"></param>
        /// <returns></returns>
        private bool IsNeedRedirectToLoginpage(HttpApplication application, out string loginUrl) {
            loginUrl = "";
            if (WeixinServiceType.QY.ToString() == ConfigHelper.Get(AppSettingsKey.WeixinServiceType)) return false;
            loginUrl = string.Format("{0}?ReturnUrl={1}", "/User/Login",
                HttpUtility.UrlEncode(application.Request.Url.ToString()));
            return ConfigHelper.Get(AppSettingsKey.IsNeedRedirectToLoginpage) == "true";


            //电脑登陆时，根据配置判断是否需要跳转到登录页面（为了方便在电脑上根据debug_openid获取微信信息）
            if (ConfigHelper.Get(AppSettingsKey.IsNeedRedirectToLoginpage) != "true") return false;
            //非微信浏览器登录，跳转到登陆页面
            string userAgent = application.Request.UserAgent;
            string weixinAgent = "micromessenger";
            bool isWeixinBrowser = userAgent.ToLower().Contains(weixinAgent);
            return !isWeixinBrowser;
        }

        /// <summary>
        /// 是否可以跳过验证
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        private bool CanIgnoreVerify(HttpApplication application) {
            string routeController = (string)(application.Request.RequestContext.RouteData.Values["controller"] ?? "");
            string routeAction = (string)(application.Request.RequestContext.RouteData.Values["action"] ?? "");

            return (application.Request.RequestContext.RouteData.Route == null
                && !application.Request.Url.AbsolutePath.ToLower().EndsWith(".html"))
                || (routeController.ToLower().Equals("user") && routeAction.ToLower().Equals("login"))
                || routeAction == WeixinCallbackServiceName
                || routeController == WeixinController
                || routeController.ToLower() == "home"
                || routeAction.ToLower() == "index";
        }


        //微信回调service名称
        private string WeixinCallbackServiceName {
            get { return ConfigHelper.Get("WeixinCallbackServiceName"); }
        }
        //微信Controller名称
        private string WeixinController {
            get { return ConfigHelper.Get("WeixinController"); }
        }
    }
}
