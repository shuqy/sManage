using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Report
{
    public class T_ZhouBangConfig
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 榜单类型（0：综合榜 ：业务线榜）
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 榜单名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 主榜描述 子榜维度
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 默认榜单 显示顺序 0综合榜 1新锐榜
        /// </summary>
        public int DefaultCode { get; set; }
        /// <summary>
        /// 榜单模板（0:模板一   1:模板二）
        /// </summary>
        public int Templste { get; set; }
        /// <summary>
        /// 上榜数量
        /// </summary>
        public int TopCount { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 统计语句
        /// </summary>
        public string SqlText { get; set; }
        /// <summary>
        /// 是否删除 1删除
        /// </summary>
        public int IsDel { get; set; }
        /// <summary>
        ///排名维度 1:综合榜 2:新锐榜 1,2
        /// </summary>
        public string OrderDim { get; set; }
        /// <summary>
        /// 是否开启 0关闭 1开启
        /// </summary>
        public int IsOpen { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime SysTime { get; set; }
        /// <summary>
        /// 类型说明
        /// </summary>
        public string TypeDesc { get; set; }
    }
}
