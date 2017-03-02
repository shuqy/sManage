using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaoService.Zhihu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.Zhihu.Tests {
    [TestClass()]
    public class LoginTests {
        [TestMethod()]
        public void TryEmailLoginTest() {
            Login login = new Login();
            login.TryEmailLogin("617086902@qq.com", "t2131402780tyf");
        }
    }
}