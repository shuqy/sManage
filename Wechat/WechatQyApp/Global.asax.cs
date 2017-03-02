using Core;
using Core.Application;
using Core.Entities;
using Core.Utilities;
using Senparc.Weixin.QY.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using YaoService.LeShare;
using YaoService.Zhihu;

namespace WechatQyApp {
    public class MvcApplication : System.Web.HttpApplication {
        private System.Timers.Timer _timer;
        private System.Timers.Timer _timer2;
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //初始化
            AppContext.Start(new WebApplication());

            //定时任务
            _timer = new System.Timers.Timer(60 * 1000);
            _timer.Enabled = true;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(run);
            _timer.Start();

            _timer2 = new System.Timers.Timer(30 * 60 * 1000);
            _timer2.Enabled = true;
            _timer2.Elapsed += new System.Timers.ElapsedEventHandler(grawl);
            _timer2.Start();
        }

        /// <summary>
        /// 定时要执行的程序
        /// </summary>
        private void run(object sender, System.Timers.ElapsedEventArgs args) {
            AutoSign.AutoSignInHandle();
        }
        //定时抓取知乎
        private void grawl(object sender, System.Timers.ElapsedEventArgs args) {
            AutoGrawl.GetAnswer();
        }

        private void FalseVisit() {
            HttpHelper httpHelper = new HttpHelper();
            HttpItem item = new HttpItem {
                URL = "http://www.wmylife.com/autosign/index/"
            };
            HttpResult result = httpHelper.GetHtml(item);
        }

        protected void Application_End(object sender, EventArgs e) {
        }
    }
}
