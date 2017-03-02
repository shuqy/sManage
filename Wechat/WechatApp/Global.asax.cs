using Core;
using Core.Application;
using Core.Entities;
using Core.Utilities;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using YaoService.LeShare;

namespace WechatApp {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //注册Appid，全局一次即可
            AccessTokenContainer.Register(ConfigHelper.Get(AppSettingsKey.AppId),
                ConfigHelper.Get(AppSettingsKey.AppSecret));
            //初始化
            AppContext.Start(new WebApplication());

        }


    }
}
