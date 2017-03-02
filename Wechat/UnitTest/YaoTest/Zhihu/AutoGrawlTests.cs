using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaoService.Zhihu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.Zhihu.Tests {
    [TestClass()]
    public class AutoGrawlTests {
        [TestMethod()]
        public void GetAnswerTest() {
            AutoGrawl.GetAnswer();
        }
    }
}