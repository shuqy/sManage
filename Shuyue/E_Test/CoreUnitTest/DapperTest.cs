using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Util;
using Core.Enum;
using Model.ViewModel.Report;
using System.Collections.Generic;
using Core;
using Core.Application;

namespace CoreUnitTest
{
    [TestClass]
    public class DapperTest
    {
        [TestMethod]
        public void QueryTest()
        {
            AppContext.Start(new ServiceApplication());
            DapperHelper dh = new DapperHelper(DbConnEnum.Monitor);
            var list = dh.Query<T_ZhouBangConfig>("SELECT * FROM T_ZhouBangConfig where Id<=@TOPN", new { TOPN = 10 });
            int count = 0;
            foreach (var item in list)
            {
                count++;
            }
            Assert.AreEqual(10, count);
        }
    }
}
