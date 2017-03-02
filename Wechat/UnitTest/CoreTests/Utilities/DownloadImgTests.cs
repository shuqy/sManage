using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Tests {
    [TestClass()]
    public class DownloadImgTests {
        [TestMethod()]
        public void LoadTest() {
            DownloadImg.Load("https://pic3.zhimg.com/188e47a6e022253cd1974753ba47bd7a_b.jpg", "test.jpg");
        }
    }
}