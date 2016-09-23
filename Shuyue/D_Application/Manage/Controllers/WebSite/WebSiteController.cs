using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manage.Controllers.WebSite
{
    public class WebSiteController : Controller
    {
        // GET: WebSite
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Button()
        {
            return View();
        }
        public ActionResult TextBox()
        {
            return View();
        }
    }
}