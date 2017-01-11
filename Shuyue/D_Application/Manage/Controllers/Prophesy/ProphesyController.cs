using Core.Entities;
using Core.Util;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manage.Controllers.Prophesy
{
    public class ProphesyController : ControllerBase
    {
        // GET: Prophesy
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 更新行业信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateIndustry()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UpdateIndustry(string path)
        {
            DataTable dt = NPOIHelper.ImportExceltoDt(Server.MapPath(path));
            List<T_Stock> stockList = ModelHelper.ExcelFillModelList<T_Stock>(dt).ToList();
            foreach(var item in stockList)
            {

            }
            return Json(new JsonData { Code = Core.Enum.ResultCode.OK }, JsonRequestBehavior.DenyGet);
        }
    }
}