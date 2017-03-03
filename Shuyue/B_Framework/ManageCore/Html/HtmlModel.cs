using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Html
{
    public class HtmlModel
    {
        public HtmlModel()
        {
            Head = new Head();
            LinkCollection = new LinkCollection();
            AllScripts = "";
            AllStyles = "";
            LineQueue = new Queue<HtmlLines>();
        }

        public string AllHtml { get; set; }
        public string AllHead { get; set; }
        public string AllBody { get; set; }
        public string AllStyles { get; set; }
        public string AllScripts { get; set; }
        public Head Head { get; set; }
        public LinkCollection LinkCollection { get; set; }
        public Queue<HtmlLines> LineQueue { get; set; }
    }
}
