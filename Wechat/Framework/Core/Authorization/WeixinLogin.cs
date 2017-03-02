using Core.Entities;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Core.Authorization {
    /// <summary>
    /// 微信登陆
    /// </summary>
    public class WeixinLogin {
        public static void Login(HttpContext context) {
            bool ismp = ConfigHelper.Get("WeixinServiceType") == WeixinServiceType.MP.ToString();
            if (ismp) MpLogin(context);
            else QyLogin(context);
        }

        private static void MpLogin(HttpContext context) {
            BaseUser user = new BaseUser();
            string openId = "";
            openId = GetOpenId(context);
            if (string.IsNullOrWhiteSpace(openId)) {
                //跳转到微信认证页面
                RedirectToWeixinAuthPage(context);
            }
            //
            IWexinUserManagement weixinUser = new WexinUserManagement();
            user = weixinUser.Login(openId);
            if (user == null) {
                user = weixinUser.CreateAndRelevance(openId);
            }
            if (user == null) return;
            context.Session[SessionKey.CurrentUser] = user;
            FormsAuthentication.SetAuthCookie(user.Customer.Username, true);
        }

        private static void QyLogin(HttpContext context) {
            BaseUser user = new BaseUser();
            string userCode = "";
            userCode = GetUserCode(context);
            if (string.IsNullOrWhiteSpace(userCode)) {
                //跳转到微信认证页面
                RedirectToWeixinAuthPage(context);
            }
            //
            IWexinUserManagement weixinUser = new WexinQyUserManagement();
            user = weixinUser.Login(userCode);
            if (user == null) {  
                user = weixinUser.CreateAndRelevance(userCode);
            }
            if (user == null) return;
            context.Session[SessionKey.CurrentUser] = user;
            FormsAuthentication.SetAuthCookie(user.Customer.Username, true);
        }


        /// <summary>
        /// 跳转进行微信的验证
        /// </summary>
        /// <returns></returns>
        protected static void RedirectToWeixinAuthPage(HttpContext context) {
            var guid = Guid.NewGuid().ToString();
            context.Session[guid] = context.Request.RawUrl; //将url保存到参数中

            string url;
            //服务号
            if (ConfigHelper.Get(AppSettingsKey.WeixinServiceType) == "MP") {

                url = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAuthorizeUrl(
                   ConfigHelper.Get(AppSettingsKey.AppId), string.Format("http://{0}/{1}", context.Request.Url.Authority,
                   WeixinAuthorizationCallbackUrl),
                    guid, Senparc.Weixin.MP.OAuthScope.snsapi_userinfo);
            } else {
                url = Senparc.Weixin.QY.AdvancedAPIs.OAuth2Api.GetCode(
                    ConfigHelper.Get(AppSettingsKey.CorpId), string.Format("http://{0}/{1}", context.Request.Url.Authority,
                   WeixinAuthorizationCallbackUrl),
                    guid);
            }

            context.Response.Redirect(url, true);
        }


        //微信授权回调地址
        private static string WeixinAuthorizationCallbackUrl {
            get { return ConfigHelper.Get(AppSettingsKey.WeixinAuthorizationCallbackUrl); }
        }


        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetOpenId(HttpContext context) {
            string openId = context.Session[SessionKey.OpenId] as string;
            // 如果从Session中获取到的openid为空并且当前是debug状态并且配置文件中配置了调试用openid，则使用调试中的openid
#if DEBUG
            if (string.IsNullOrWhiteSpace(openId)
                          && context.IsDebuggingEnabled
                          && !string.IsNullOrWhiteSpace(ConfigHelper.Get(AppSettingsKey.DebugOpenId))) // TEST/DEBUG ONLY
            {
                openId = ConfigHelper.Get(AppSettingsKey.DebugOpenId);
                context.Session[SessionKey.OpenId] = openId;
            }
#endif
            return openId;
        }

        private static string GetUserCode(HttpContext context) {
            return context.Session[SessionKey.WeixinCode] == null ? "" : context.Session[SessionKey.WeixinCode].ToString();
        }
    }
}
