using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities;

namespace DotNet.Utilities.Tests {
    [TestClass()]
    public class FileOperateTests {
        [TestMethod()]
        public void WriteFileTest() {
            FileOperate.WriteFile(ConfigHelper.Get("TxtPath") + "teste" + ".txt", "dfsadfsadfsadfsdafasd");
        }
    }
}