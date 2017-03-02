using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities {
    public class ImgData {
        /// <summary>
        /// 原来img标签内容
        /// </summary>
        public string oldimg { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string src { get; set; }
        /// <summary>
        /// 新图片名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 站内图片地址链接
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 知乎图片真实路径
        /// </summary>
        public string realPath { get; set; }
    }
}
