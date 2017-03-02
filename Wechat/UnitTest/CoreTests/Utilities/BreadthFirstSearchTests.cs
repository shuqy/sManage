using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Tests {
    [TestClass()]
    public class BreadthFirstSearchTests {
        Graph g;
        SymbolGraph sg;
        BreadthFirstSearch bfs;

        public BreadthFirstSearchTests() {
            g = new Graph(3);
            g.AddEdge(0, 1);
            g.AddEdge(0, 2);

            sg = new SymbolGraph(new List<string> { "gege|jiejie" }, '|');

            //bfs = new BreadthFirstSearch(g, 0);
            bfs = new BreadthFirstSearch(sg.Graph(), 0);
        }

        [TestMethod()]
        public void BreadthFirstSearchTest() {
        }

        [TestMethod()]
        public void HasPathToTest() {
            Assert.AreEqual(bfs.HasPathTo(sg.Index("jiejie")), true);
            //Assert.AreEqual(bfs.HasPathTo(1), true);
        }

        [TestMethod()]
        public void PathToTest() {
            Assert.AreEqual(bfs.PathTo(1).Count(), 2);
        }
    }
}