using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Html
{
    public class LinkCollection
    {
        public LinkCollection()
        {
            Links = new List<Link>();
            SitelinkCount = 0;
            OffSitelinkCount = 0;
        }
        public IList<Link> Links { get; set; }
        public int TotalLinkCount { get { return SitelinkCount + OffSitelinkCount; } }
        public int SitelinkCount { get; set; }
        public int OffSitelinkCount { get; set; }
    }

    public class Link
    {
        public string Name { get; set; }
        public string Href { get; set; }
        public string Alt { get; set; }
        public string FullLink { get; set; }
        public bool IsSiteLink { get; set; }
    }
}
