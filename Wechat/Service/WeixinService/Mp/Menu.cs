using Core.Entities;
using Core.Utilities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeixinService.Mp {
    public class Menu {

        public static bool Create(GetMenuResultFull resultFull) {
            try {
                var bg = CommonApi.GetMenuFromJsonResult(resultFull, new ButtonGroup()).menu;
                string token = GetAccessToken();
                //创建菜单
                var result = CommonApi.CreateMenu(token, bg);
                return true;
            } catch (Exception ex) {
                return false;
            }
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <returns></returns>
        private static string GetAccessToken() {
            return AccessTokenContainer.TryGetAccessToken(ConfigHelper.Get(AppSettingsKey.AppId), ConfigHelper.Get(AppSettingsKey.AppSecret));
        }

    }
}
