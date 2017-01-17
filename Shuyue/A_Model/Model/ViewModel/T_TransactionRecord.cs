using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    /// <summary>
    /// 交易历史几率
    /// </summary>
    public class T_TransactionRecord
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public string StockCode { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime TradingDate { get; set; }
        /// <summary>
        /// 开盘价
        /// </summary>
        public decimal Open { get; set; }
        /// <summary>
        /// 最高价
        /// </summary>
        public decimal High { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public decimal Low { get; set; }
        /// <summary>
        /// 收盘价
        /// </summary>
        public decimal Close { get; set; }
        /// <summary>
        /// 涨幅
        /// </summary>
        public decimal Rose { get; set; }
        /// <summary>
        /// 振幅
        /// </summary>
        public decimal Amplitude { get; set; }
        /// <summary>
        /// 手数
        /// </summary>
        public int Hands { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 换手率
        /// </summary>
        public decimal Turnover { get; set; }
        /// <summary>
        /// 成交次数
        /// </summary>
        public int VOLAMOUNT { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Deleted { get; set; }
    }
}
