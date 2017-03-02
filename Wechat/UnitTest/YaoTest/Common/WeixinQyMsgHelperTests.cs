using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeixinService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.QY.Entities;

namespace WeixinService.Common.Tests {
    [TestClass()]
    public class WeixinQyMsgHelperTests {
        [TestMethod()]
        public void SendNewsTest() {
            WeixinQyMsgHelper.SendNews("@all", "1", new List<Article> { new Article { Title = "test", Description = "okokok" } });
        }
    }
}