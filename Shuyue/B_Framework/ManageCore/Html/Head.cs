using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Html
{
    /// <summary>
    /// html头部信息
    /// </summary>
    public class Head
    {
        public Head()
        {
            Meta = new Dictionary<string, string>();
        }
        public string Title { get; set; }
        public string Charset { get; set; }
        public Dictionary<string, string> Meta { get; set; }
    }
}
