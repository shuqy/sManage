using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Tests {
    [TestClass()]
    public class LogHelperTests {
        [TestMethod()]
        public void WriteTest() {
            LogHelper lh = new LogHelper();
            Msg msg = new Msg {
                Datetime = DateTime.Now,
                Text = "testtttttttt",
                Type = MsgType.Information
            };
            lh.Write(msg);
        }
    }
}