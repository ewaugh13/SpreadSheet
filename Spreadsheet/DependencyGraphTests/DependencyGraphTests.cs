using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;

namespace DependencyGraphTests
{
    [TestClass]
    public class DependencyGraphTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            DependencyGraph d = new DependencyGraph();
        }

        [TestMethod]
        public void TestMethod2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            Assert.AreEqual(d.Size,2);
        }
    }
}
