using Core.Entities;
using Core.Util;
using Core.Utilities;
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
        public ActionResult UpBaseData()
        {
            return View();
        }

        /// <summary>
        /// 更新数据信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpBaseData(string path)
        {
            var db = Core.AppContext.Current.StockDbContext;
            //读取Excel中的数据
            DataTable dt = NPOIHelper.ImportExceltoDt(Server.MapPath(path));
            List<T_Stock> stockList = ModelHelper.ExcelFillModelList<T_Stock>(dt).ToList();
            //行业数据/新增行业列表
            List<T_Industry> industryList = db.T_Industry.ToList(), newIndustryList = new List<T_Industry>();
            //股票信息列表/新增股票列表
            List<T_Stock> currentStockList = db.T_Stock.ToList(), newStockList = new List<T_Stock>();
            foreach (var item in stockList)
            {
                if (!industryList.Any(i => i.Name.Equals(item.IndustryName)) && !newIndustryList.Any(i => i.Name.Equals(item.IndustryName)))
                    newIndustryList.Add(new T_Industry { Name = item.IndustryName });
                if (!currentStockList.Any(s => s.FullCode == item.FullCode))
                {
                    T_Stock stock = new T_Stock
                    {
                        FullCode = item.FullCode,
                        StockName = item.StockName,
                        TotalAmount = item.TotalAmount,
                        TotalMarketValue = item.TotalMarketValue,
                        CirculationMarketValue = item.CirculationMarketValue,
                        CreatedOn = DateTime.Now,
                        Deleted = false,
                        StockCode = item.FullCode.Substring(2),
                        PyAbbre = PyHelper.GetFirst(item.StockName),
                        PyFullName = PyHelper.Get(item.StockName),
                        IndustryName = item.IndustryName,
                    };
                    newStockList.Add(stock);
                }
            }
            int count1 = newIndustryList.Count(), count2 = newStockList.Count();
            if (newIndustryList.Count() > 0)
            {
                db.T_Industry.AddRange(newIndustryList);
                db.SaveChanges();
            }
            if (newStockList.Count() > 0)
            {
                var currentIndustryList = db.T_Industry.ToList();
                //获取行业id
                newStockList.ForEach(s => s.Industry = (currentIndustryList.FirstOrDefault(i => i.Name.Equals(s.IndustryName)) ?? new T_Industry()).Id);
                db.T_Stock.AddRange(newStockList);
                db.SaveChanges();
            }
            return Json(new JsonData { Code = Core.Enum.ResultCode.OK, Data = new { count1 = count1, count2 = count2 } }, JsonRequestBehavior.DenyGet);
        }
    }
}