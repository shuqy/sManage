using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockApp.Controllers
{
    public class ProphesyController : ControllerBase
    {
        // GET: Prophesy
        public ActionResult Index()
        {
            return View();
        }
    }
}