using Core;
using Core.Entities;
using Core.Enum;
using ManageEF;
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
            ViewBag.UserId = AppContext.Current.CurrentUser.Id;
            return View();
        }
        [HttpPost]
        public JsonResult TurnInAndOut(turn_in_out_record record)
        {
            var db = AppContext.Current.DbContext;
            record.Deleted = false;
            record.CreatedOn = DateTime.Now;
            db.turn_in_out_record.Add(record);
            db.SaveChanges();
            return Json(new JsonData { Code = record.Id > 0 ? ResultCode.OK : ResultCode.Fail, }, JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 添加股票交易记录
        /// </summary>
        /// <returns></returns>
        public ActionResult AddStockExchange()
        {
            ViewBag.UserId = AppContext.Current.CurrentUser.Id;
            return View();
        }
        [HttpPost]
        public ActionResult AddStockExchange(stock_exchange_record record)
        {
            var db = AppContext.Current.DbContext;
            record.CreatedOn = DateTime.Now;
            record.Deleted = false;
            record.StockName = db.stock.FirstOrDefault(a => a.StockCode == record.StockCode).StockName;
            record.Profit = (record.SellPrice - record.BuyPrice) * record.Quantity;
            db.stock_exchange_record.Add(record);
            db.SaveChanges();
            return Json(new JsonData { Code = record.Id > 0 ? ResultCode.OK : ResultCode.Fail, }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult UpdateWaterBill()
        {
            DateTime dtn = DateTime.Now;
            int userId = AppContext.Current.CurrentUser.Id;
            var db = AppContext.Current.DbContext;
            //获取转入转出数据
            var turnInOutList = db.turn_in_out_record.Where(r => r.UserId == userId && !r.Deleted).OrderBy(a => a.OperationDate);
            var stockExchangeList = db.stock_exchange_record.Where(r => r.UserId == userId && !r.Deleted).OrderBy(a => a.SellDate);
            List<water_bill> wlist = new List<water_bill>();
            decimal bmoney = 0.00m;
            decimal emoney = 0.00m;
            DateTime fd = turnInOutList.First().OperationDate;
            DateTime bfd = Convert.ToDateTime(string.Format("{0}-{1}-{2}", fd.Year, fd.Month, 1));
            DateTime efd = Convert.ToDateTime(string.Format("{0}-{1}-{2}", fd.Year, fd.Month, bfd.AddMonths(1).AddDays(-1).Day));
            //生成月账单流水
            for (; efd < Convert.ToDateTime(DateTime.Now.ToShortDateString()); efd = efd.AddMonths(1), bfd = bfd.AddMonths(1))
            {
                var tlist = turnInOutList.Where(e => e.OperationDate <= efd && e.OperationDate >= bfd);
                var elist = stockExchangeList.Where(e => e.SellDate <= efd && e.SellDate >= bfd);
                bmoney = emoney;
                decimal inOutMoney = tlist.Sum(t => t.Money);
                decimal profile = Convert.ToDecimal(elist.Sum(e => e.Profit));
                decimal inMoney = tlist.Any(t => t.Money > 0) ? tlist.Where(t => t.Money > 0).Sum(t => t.Money) : 0m;
                decimal outMoney = tlist.Any(t => t.Money < 0) ? tlist.Where(t => t.Money > 0).Sum(t => t.Money) : 0m;
                emoney = bmoney + inOutMoney + profile;
                wlist.Add(new water_bill
                {
                    UserId = userId,
                    BMonthMoney = bmoney,
                    TurnInMoney = inMoney,
                    TurnOutMoney = outMoney,
                    ProfitMoney = profile,
                    EMonthMoney = emoney,
                    Date = bfd,
                    UpdatedOn = dtn,
                });
            }
            //保存数据
            db.water_bill.RemoveRange(db.water_bill);
            db.water_bill.AddRange(wlist);
            db.SaveChanges();
            return View();
        }
        /// <summary>
        /// 转入转出记录
        /// </summary>
        /// <returns></returns>
        public ActionResult TurnInOutList()
        {
            var db = AppContext.Current.DbContext;
            List<turn_in_out_record> turnInOutList = db.turn_in_out_record.Where(t => t.UserId == AppContext.Current.CurrentUser.Id).ToList();
            return View(turnInOutList);
        }
        public ActionResult StockExchangeList()
        {
            var db = AppContext.Current.DbContext;
            List<stock_exchange_record> stockExchangeList = db.stock_exchange_record.Where(t => t.UserId == AppContext.Current.CurrentUser.Id).ToList();
            return View(stockExchangeList);
        }
    }
}