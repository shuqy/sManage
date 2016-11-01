using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockApp.Controllers
{
    public class SummaryController : ControllerBase
    {
        // GET: Summary
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 增加转入转出记录
        /// </summary>
        /// <returns></returns>
        public ActionResult TurnInAndOut()
        {
            return View();
        }
    }
}