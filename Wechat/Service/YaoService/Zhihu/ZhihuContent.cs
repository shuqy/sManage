using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.Zhihu {
    public class ZhihuContent {
        /// <summary>
        /// 问题标题
        /// </summary>
        public string Question { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 简介
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
        /// 赞数量
        /// </summary>
        public string ZanCount { get; set; }
    }
}
