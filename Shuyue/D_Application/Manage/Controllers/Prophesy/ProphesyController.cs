using Core.Entities;
using Core.Util;
using Core.Utilities;
using ManageService.Stock;
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
                if (!string.IsNullOrEmpty(item.IndustryName) && !industryList.Any(i => i.Name.Equals(item.IndustryName)) && !newIndustryList.Any(i => i.Name.Equals(item.IndustryName)))
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

        /// <summary>
        /// 更新股票信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpSingleStockData(string path)
        {
            try
            {
                DateTime dtn = DateTime.Now;
                string stockCode = path.Substring(path.LastIndexOf('.') - 6, 6);
                var db = Core.AppContext.Current.StockDbContext;
                if (!db.T_Stock.Any(a => a.StockCode == stockCode))
                    return Json(new JsonData { Code = Core.Enum.ResultCode.Fail, Message = "操作失败，证券代码不存在！" }, JsonRequestBehavior.DenyGet);
                //读取Excel中的数据
                DataTable dt = NPOIHelper.ImportExceltoDt(Server.MapPath(path));
                StockBLL stockBLL = new StockBLL();
                int count = stockBLL.UpSingleStockData(stockCode, dt);
                return Json(new JsonData
                {
                    Code = count == 0 ? Core.Enum.ResultCode.Fail : Core.Enum.ResultCode.OK,
                    Data = new { count = count, time = Convert.ToInt32((DateTime.Now - dtn).TotalSeconds) }
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new JsonData { Code = Core.Enum.ResultCode.Fail, Message = "操作失败，非正确的文件！" }, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult UpAllStockData(string path)
        {
            try
            {
                DateTime dtn = DateTime.Now;
                string data = path.Substring(path.LastIndexOf('.') - 10, 10);
                //读取Excel中的数据
                DataTable dt = NPOIHelper.ImportExceltoDt(Server.MapPath(path));
                StockBLL stockBLL = new StockBLL();
                stockBLL.UpAllStockData(dt, Convert.ToDateTime(data));
                return Json(new JsonData { Code = Core.Enum.ResultCode.OK }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new JsonData { Code = Core.Enum.ResultCode.Fail, Message = "操作失败，请稍后重试！" }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}