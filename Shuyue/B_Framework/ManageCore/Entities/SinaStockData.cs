using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SinaStockData
    {
        /// <summary>
        /// 股票名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 今日开盘价
        /// </summary>
        public string openningPrice { get; set; }
        /// <summary>
        /// 昨日收盘价
        /// </summary>
        public string closingPrice { get; set; }
        /// <summary>
        /// 当前价格
        /// </summary>
        public string currentPrice { get; set; }
        /// <summary>
        /// 最高价
        /// </summary>
        public string hPrice { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public string lPrice { get; set; }
        /// <summary>
        /// 涨幅
        /// </summary>
        public string increase { get; set; }
        /// <summary>
        /// 成交量，需要除100
        /// </summary>
        public string totalNumber { get; set; }
        /// <summary>
        /// 成交额
        /// </summary>
        public string turnover { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 分时图
        /// </summary>
        public string chatMin { get; set; }
        /// <summary>
        /// 日k
        /// </summary>
        public string chatDaily { get; set; }
        /// <summary>
        /// 周k
        /// </summary>
        public string chatWeekly { get; set; }
        /// <summary>
        /// 月k
        /// </summary>
        public string chatMonthly { get; set; }
    }
}
