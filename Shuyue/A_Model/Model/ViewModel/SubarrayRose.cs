using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    /// <summary>
    /// 涨跌幅子矩阵
    /// </summary>
    public class SubarrayRose
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginData { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 集合列表
        /// </summary>
        public List<T_TransactionRecord> RecordList { get; set; }
    }
    /// <summary>
    /// 最大涨幅子矩阵
    /// </summary>
    public class MaximumSubarrayRose : SubarrayRose
    {

    }
    /// <summary>
    /// 最大跌幅子矩阵
    /// </summary>
    public class MinimumSubarrayRose : SubarrayRose
    {

    }
}
