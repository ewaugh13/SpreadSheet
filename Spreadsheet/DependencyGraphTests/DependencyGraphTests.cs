using System;
using System.Collections.Generic;
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
            d.AddDependency("Marry", "Bob");
            Assert.AreEqual(d.Size, 2);
        }

        [TestMethod]
        public void TestMethod3()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "Bob");
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

        [TestMethod]
        public void TestHasDependees1()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "Bob");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "Bob");
            Assert.AreEqual(true, d.HasDependees("Bob"));
        }

        [TestMethod]
        public void TestHasDependees2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "Bob");
            Assert.AreEqual(false, d.HasDependees("Marry"));
        }

        [TestMethod]
        public void TestHasGetDependents()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "Bob");
            List<string> Dependents = new List<string>();
            foreach (string value in d.GetDependents("Sally"))
            {
                Dependents.Add(value);
            } 
            Assert.AreEqual(Dependents.Count, 2);
        }

        [TestMethod]
        public void TestHasGetDependents2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "Bob");
            List<string> Dependents = new List<string>();
            foreach (string value in d.GetDependents("Jim"))
            {
                Dependents.Add(value);
            }
            Assert.AreEqual(Dependents.Count, 0);
        }

        [TestMethod]
        public void TestHasGetDependees()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "John");
            List<string> Dependees = new List<string>();
            foreach (string value in d.GetDependees("John"))
            {
                Dependees.Add(value);
            }
            Assert.AreEqual(Dependees.Count, 2);
        }

        [TestMethod]
        public void TestHasGetDependees2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "Bob");
            List<string> Dependees = new List<string>();
            foreach (string value in d.GetDependees("Sally"))
            {
                Dependees.Add(value);
            }
            Assert.AreEqual(Dependees.Count, 0);
        }

        [TestMethod]
        public void TestRemoveDependency()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "Bob");
            d.RemoveDependency("Sally", "Jim");
            d.RemoveDependency("Sally", "Bob");
            Assert.AreEqual(d.Size, 1);
        }

        [TestMethod]
        public void TestRepleaceDependents()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "Bob");
            List<string> newDependents = new List<string>();
            newDependents.Add("Alex");
            newDependents.Add("Jake");
            d.ReplaceDependents("Sally", newDependents);
            Assert.AreEqual(d.Size, 2);
        }

        [TestMethod]
        public void TestRepleaceDependees()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Kim", "Brock");
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "John");
            d.AddDependency("Sally", "Bob");
            List<string> newDependees = new List<string>();
            newDependees.Add("Jessica");
            newDependees.Add("Kelly");
            d.ReplaceDependees("John", newDependees);
            Assert.AreEqual(d.Size, 4);
        }
    }
}
