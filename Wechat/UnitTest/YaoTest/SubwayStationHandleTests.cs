using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaoService.SubwayStation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaoService.Tests {
    [TestClass()]
    public class SubwayStationHandleTests {
        [TestMethod()]
        public void CreateStationLinesTest() {
            SubwayStationHandle.CreateStationLines();
        }

        [TestMethod()]
        public void CreateAllStationFileTest() {
            SubwayStationHandle.CreateAllStationFile();
        }
    }
}