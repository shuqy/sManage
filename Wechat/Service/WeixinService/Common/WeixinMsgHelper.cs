using Core.Entities;
using Core.Utilities;
using Senparc.Weixin;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeixinService.Common {
    /// <summary>
    /// 微信推送消息助手
    /// </summary>
    public sealed class WeixinMsgHelper {

        static WeixinMsgHelper() {
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="toOpenId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SendMessage(string toOpenId, string msg) {
            try {
                var accessToken = GetAccessToken();
                var result = Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(accessToken, toOpenId, msg);
                return result != null && result.errcode == ReturnCode.请求成功;
            } catch (Exception ex) {
            }
            return false;
        }

        /// <summary>
        /// 发送新闻模版
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="articles"></param>
        /// <returns></returns>
        public static bool SendNews(string openId, List<Article> articles) {
            try {
                var accessToken = GetAccessToken();
                var result = Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendNews(accessToken, openId, articles);
                return result != null && result.errcode == ReturnCode.请求成功;
            } catch (Exception ex) {
            }
            return false;
        }
        
        /// <summary>
        /// 获取accessToken
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken() {
            return Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.TryGetAccessToken(ConfigHelper.Get(AppSettingsKey.AppId), ConfigHelper.Get(AppSettingsKey.AppSecret));
        }

    }
}
