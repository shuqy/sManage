using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaoService.LeShare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.LeShare.Tests {
    [TestClass()]
    public class AutoSignTests {
        [TestMethod()]
        public void GetMansUserTest() {
            AutoSign.GetMansUser();
        }
    }
}