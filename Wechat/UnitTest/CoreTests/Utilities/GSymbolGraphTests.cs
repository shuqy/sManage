using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Tests {
    [TestClass()]
    public class GSymbolGraphTests {
        private class User {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        List<List<User>> testList;
        User user1;
        User user2;
        public GSymbolGraphTests() {
            user1 = new User { Name = "Luce", Age = 12 };
            user2 = new User { Name = "Lili", Age = 12 };
            testList = new List<List<User>>();
            var userl = new List<User>();
            testList.Add(userl);
            userl.Add(user1);
            userl.Add(user2);
        }
        [TestMethod()]
        public void GSymbolGraphTest() {
        }

        [TestMethod()]
        public void ConatinsTest() {
            GSymbolGraph<User> gsg = new GSymbolGraph<User>(testList);
            Assert.AreEqual(gsg.Conatins(user1), true);
        }

        [TestMethod()]
        public void IndexTest() {
            GSymbolGraph<User> gsg = new GSymbolGraph<User>(testList);
            Assert.AreEqual(gsg.Index(user1), 0);
        }

        [TestMethod()]
        public void NameTest() {
            GSymbolGraph<User> gsg = new GSymbolGraph<User>(testList);
            Assert.AreEqual(gsg.Name(0).Name, "Luce");
        }

        [TestMethod()]
        public void GraphTest() {
            GSymbolGraph<User> gsg = new GSymbolGraph<User>(testList);
            Assert.AreEqual(gsg.Graph().V, 2);
        }
    }
}