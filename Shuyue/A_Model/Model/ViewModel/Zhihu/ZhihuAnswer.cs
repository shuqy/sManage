using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Zhihu
{
    /// <summary>
    /// 知乎回答
    /// </summary>
    public partial class ZhihuAnswer
    {
        public int Id { get; set; }
        /// <summary>
        /// 问题id
        /// </summary>
        public string QuestionId { get; set; }
        /// <summary>
        /// 答案id
        /// </summary>
        public string AnswerId { get; set; }
        /// <summary>
        /// 问题标题
        /// </summary>
        public string Question { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 作者介绍
        /// </summary>
        public string Bio { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 赞数
        /// </summary>
        public Nullable<int> ZanCount { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool Recommended { get; set; }
        /// <summary>
        /// 浏览数
        /// </summary>
        public Nullable<int> ViewCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> CreatedOn { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Deleted { get; set; }
    }
}
