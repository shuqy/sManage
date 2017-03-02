using Core.Authorization;
using Core.Entities;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace WechatApp.Controllers {
    public class WeixinOAuthController : Controller {
        public ActionResult Index() {
            return View();
        }


        /// <summary>
        /// 微信验证后继续执行Action
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult WeixinCallback(string code, string state) {
            if (string.IsNullOrEmpty(code)) // 未获得合法的验证码
            {
                throw new InvalidOperationException("您拒绝了授权！");
            }
            if (string.IsNullOrWhiteSpace(state)) // 默认跳转
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }

            var rawUrl = (string)Session[state];

            if (ConfigHelper.Get(AppSettingsKey.WeixinServiceType) == "MP")
                WeixinMPAuthenticated(code, state);
            //else
            //    WeixinQYAuthenticated(code, state);

            return Redirect(rawUrl);
        }


        private void WeixinMPAuthenticated(string code, string state) {
            var result = (Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthAccessTokenResult)Session[SessionKey.OAuthAccessTokenResult];

            if (result == null) //首次验证或者未从缓存获得
            {
                //通过，用code换取access_token
                result = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(ConfigHelper.Get(AppSettingsKey.AppId),
                    ConfigHelper.Get(AppSettingsKey.AppSecret), code);
                if (result.errcode != Senparc.Weixin.ReturnCode.请求成功) {
                    throw new InvalidOperationException("错误：" + result.errmsg);
                }

                Session[SessionKey.OAuthAccessTokenResult] = result; // 存储到缓存
            }

            Session[SessionKey.OpenId] = result.openid; // 存储OpenId，用于后续的验证
        }
    }
}