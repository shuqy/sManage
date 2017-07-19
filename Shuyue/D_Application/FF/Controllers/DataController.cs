using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FFService.Stock;

namespace FF.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult Index()
        {
            StockDataBLL sbll = new StockDataBLL();
            int count = sbll.UpdateStocks();
            return View();
        }
    }
}