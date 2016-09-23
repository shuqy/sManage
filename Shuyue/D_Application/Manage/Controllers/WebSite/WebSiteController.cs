using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manage.Controllers.WebSite
{
    public class WebSiteController : ControllerBase
    {
        // GET: WebSite
        public ActionResult Index()
        {
            return View();
        }

        [Description("按钮")]
        public ActionResult Button()
        {
            return View();
        }

        [Description("文本框")]
        public ActionResult TextBox()
        {
            return View();
        }

        [Description("字体图标")]
        public ActionResult FontIcon()
        {
            return View();
        }

        [Description("时间控件")]
        public ActionResult TimeControl()
        {
            return View();
        }

        [Description("上传控件")]
        public ActionResult UploadControl()
        {
            return View();
        }

        [Description("提示框")]
        public ActionResult Dialog()
        {
            return View();
        }
    }
}