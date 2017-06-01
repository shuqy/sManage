using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Tests
{
    [TestClass()]
    public class GraphTests
    {
        [TestMethod()]
        public void GraphTest()
        {
            Graph g = new Graph(10);
            Assert.AreEqual(g.V, 10);
        }

        [TestMethod()]
        public void AddEdgeTest()
        {
            Graph g = new Graph(10);
            g.AddEdge(0, 1);
            Assert.AreEqual(g.Adj(0).FirstOrDefault(), 1);
        }

        [TestMethod()]
        public void AdjTest()
        {
            Graph g = new Graph(10);
            g.AddEdge(0, 1);
            Assert.AreEqual(g.Adj(0).Count(), 1);
        }
    }
}