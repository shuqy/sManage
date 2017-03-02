using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaoService.Zhihu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.Zhihu.Tests {
    [TestClass()]
    public class CrawlPageTests {
        [TestMethod()]
        public void CrawlHomePageTest() {
            new CrawlPage().CrawlHomePage();
        }

        [TestMethod()]
        public void RQTest() {
            new CrawlPage().RQ("https://www.zhihu.com/");
        }
    }
}