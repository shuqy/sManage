using Core.Entities;
using Core.Utilities;
using Senparc.Weixin;
using Senparc.Weixin.QY.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeixinService.Common {
    public sealed class WeixinQyMsgHelper {
        static WeixinQyMsgHelper() {
        }
        public static bool SendMsg(string code, string agentId, string msg) {
            try {
                var accessToken = GetAccessToken();
                var result = Senparc.Weixin.QY.AdvancedAPIs.MassApi.SendText(accessToken, code, "", "", agentId, msg);
                return result != null && result.errcode == ReturnCode_QY.请求成功;
            } catch (Exception ex) {
            }
            return false;
        }

        public static bool SendNews(string code, string agentId, List<Article> articles) {
            try {
                var accessToken = GetAccessToken();
                var result = Senparc.Weixin.QY.AdvancedAPIs.MassApi.SendNews(accessToken, code, "", "", agentId, articles);
                return result != null && result.errcode == ReturnCode_QY.请求成功;
            } catch (Exception ex) {
            }
            return false;
        }

        /// <summary>
        /// 获取accessToken
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken() {
            if (Senparc.Weixin.QY.CommonAPIs.AccessTokenContainer.CheckRegistered(ConfigHelper.Get(AppSettingsKey.CorpId))) {
                Senparc.Weixin.QY.CommonAPIs.AccessTokenContainer.Register(ConfigHelper.Get(AppSettingsKey.CorpId),
                ConfigHelper.Get(AppSettingsKey.CorpSecret));
            }
            return Senparc.Weixin.QY.CommonAPIs.AccessTokenContainer.TryGetToken(ConfigHelper.Get(AppSettingsKey.CorpId), ConfigHelper.Get(AppSettingsKey.CorpSecret));
        }
    }
}
