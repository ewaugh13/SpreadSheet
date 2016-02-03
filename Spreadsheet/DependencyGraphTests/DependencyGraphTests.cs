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

        [TestMethod]
        public void TestMethod3()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.RemoveDependency("Sally", "Jim");
        }

        [TestMethod]
        public void TestHasDependents()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            Assert.AreEqual(true, d.HasDependents("Sally"));
        }

        [TestMethod]
        public void TestHasDependents2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            Assert.AreEqual(false, d.HasDependents("Bob"));
        }
    }
}
