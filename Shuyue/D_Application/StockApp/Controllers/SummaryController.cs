using Core;
using Core.Entities;
using Core.Enum;
using Core.Util;
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
        /// <summary>
        /// 综合统计首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var db = AppContext.Current.DbContext;
            //转入转出
            List<turn_in_out_record> turnInOutList = db.turn_in_out_record.Where(t => t.UserId == AppContext.Current.CurrentUser.Id).ToList();
            ViewBag.total = turnInOutList.Sum(t => t.Money);
            List<delivery_order> deliveryOrderList = db.delivery_order.Where(t => t.UserId == AppContext.Current.CurrentUser.Id).ToList();
            List<IGrouping<string, delivery_order>> gdList = deliveryOrderList.GroupBy(d => d.SecurityCode).ToList();
            Dictionary<string, decimal> sDic = new Dictionary<string, decimal>();
            foreach (var item in gdList.Where(a => a.Sum(i => i.Volume) != 0))
            {
                var stock = StockHelper.GetCurStockData(item.Key);
                decimal gmoney = Convert.ToInt32(item.Sum(i => i.Volume)) * Convert.ToDecimal(stock.currentPrice);
                sDic.Add(item.Key, gmoney);
            }
            ViewBag.sDic = sDic;
            return View(gdList);
        }

        /// <summary>
        /// 交割单
        /// </summary>
        /// <returns></returns>
        public ActionResult DeliveryOrder()
        {
            var db = AppContext.Current.DbContext;
            var dlist = db.delivery_order.Where(d => d.UserId == AppContext.Current.CurrentUser.Id).ToList();
            List<IGrouping<string, delivery_order>> glist = dlist.GroupBy(d => d.SecurityCode).ToList();
            return View(glist);
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
                decimal inOutMoney = tlist == null || !tlist.Any() ? 0 : tlist.Sum(t => t.Money);
                decimal profile = Convert.ToDecimal(elist == null || !elist.Any() ? 0 : elist.Sum(e => e.Profit));
                decimal inMoney = tlist.Any(t => t.Money > 0) ? !tlist.Any(t => t.Money > 0) ? 0 : tlist.Where(t => t.Money > 0).Sum(t => t.Money) : 0m;
                decimal outMoney = tlist.Any(t => t.Money < 0) ? !tlist.Any(t => t.Money < 0) ? 0 : tlist.Where(t => t.Money < 0).Sum(t => t.Money) : 0m;
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

        /// <summary>
        /// 交易纪律
        /// </summary>
        /// <returns></returns>
        public ActionResult StockExchangeList()
        {
            var db = AppContext.Current.DbContext;
            List<stock_exchange_record> stockExchangeList = db.stock_exchange_record.Where(t => t.UserId == AppContext.Current.CurrentUser.Id).ToList();
            return View(stockExchangeList);
        }

        /// <summary>
        /// 流水
        /// </summary>
        /// <returns></returns>
        public ActionResult WaterBill()
        {
            var db = AppContext.Current.DbContext;
            List<water_bill> waterBillList = db.water_bill.Where(t => t.UserId == AppContext.Current.CurrentUser.Id).ToList();
            return View(waterBillList);
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Statistics()
        {
            var db = AppContext.Current.DbContext;
            List<water_bill> waterBillList = db.water_bill.Where(t => t.UserId == AppContext.Current.CurrentUser.Id).ToList();
            ViewBag.stockExchangeList = db.stock_exchange_record.Where(t => t.UserId == AppContext.Current.CurrentUser.Id).GroupBy(a => a.StockCode).ToList();
            return View(waterBillList);
        }

        /// <summary>
        /// 每月统计
        /// </summary>
        /// <returns></returns>
        public ActionResult MonthlyStatistics()
        {
            return View();
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportData()
        {
            var table = NPOIHelper.ImportExceltoDt("D:\\workspace\\Shuyue\\G_DB\\jgd.xlsx");
            var list = ModelHelper.ExcelFillModelList<delivery_order>(table);
            list = list.Where(l => l.OccurrenceAmount != 0).ToList();
            foreach (var item in list)
            {
                item.UserId = AppContext.Current.CurrentUser.Id;
                item.SecurityCode = item.SecurityCode.PadLeft(6, '0');
                item.OperationType = item.Operation == "买入" ? 0 : 1;
            }
            var db = Core.AppContext.Current.DbContext;
            db.delivery_order.AddRange(list);
            ViewBag.IsSuccess = db.SaveChanges() > 0;
            return View();
        }
    }
}