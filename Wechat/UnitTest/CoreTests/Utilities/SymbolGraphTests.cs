using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Tests {
    [TestClass()]
    public class SymbolGraphTests {
        [TestMethod()]
        public void SymbolGraphTest() {
        }


        [TestMethod()]
        public void ContainsTest() {
            SymbolGraph sg = new SymbolGraph(new List<string> { "a|b" }, '|');
            Assert.AreEqual(sg.Contains("a"), true);
        }

        [TestMethod()]
        public void IndexTest() {
            SymbolGraph sg = new SymbolGraph(new List<string> { "a|b" }, '|');
            Assert.AreEqual(sg.Index("a"), 0);
        }

        [TestMethod()]
        public void NameTest() {
            SymbolGraph sg = new SymbolGraph(new List<string> { "a|b" }, '|');
            Assert.AreEqual(sg.Name(0), "a");
        }

        [TestMethod()]
        public void GraphTest() {
            SymbolGraph sg = new SymbolGraph(new List<string> { "a|b" }, '|');
            Assert.AreEqual(sg.Graph().V, 2);
        }
    }
}