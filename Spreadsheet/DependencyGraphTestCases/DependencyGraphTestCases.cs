using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;

namespace DependencyGraphTests
{
    [TestClass]
    public class DependencyGraphTestCases
    {

        /// <summary>
        /// Creates an emepty Dependency Graph
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            DependencyGraph d = new DependencyGraph();
            Assert.AreEqual(d.Size, 0);
        }

        /// <summary>
        /// This is adding 3 dependecies but the size of the dictonary being 2 since marry contains 2 dependents
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Marry", "Bob");
            Assert.AreEqual(d.Size, 3);
        }

        /// <summary>
        /// This is asking if Sally has dependents which it does
        /// </summary>
        [TestMethod]
        public void TestHasDependents()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            Assert.AreEqual(true, d.HasDependents("Sally"));
        }

        /// <summary>
        /// This is asking if Jim (which is a dependent) has depenndents which is false
        /// </summary>
        [TestMethod]
        public void TestHasDependents2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            Assert.AreEqual(false, d.HasDependents("Jim"));
        }

        /// <summary>
        /// This is asking if a has dependents which it does b, c
        /// </summary>
        [TestMethod]
        public void TestHasDependents3()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            Assert.AreEqual(true, d.HasDependents("a"));
        }

        /// <summary>
        /// This is asking if the dependent c has dependents which it doesn't
        /// </summary>
        [TestMethod]
        public void TestHasDependents4()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            Assert.AreEqual(false, d.HasDependents("c"));
        }

        /// <summary>
        /// This is asking if Bob has dependees which it does
        /// </summary>
        [TestMethod]
        public void TestHasDependees1()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "Bob");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "Bob");
            Assert.AreEqual(true, d.HasDependees("Bob"));
        }

        /// <summary>
        /// This is asking if the dependee marry has dependees which it doesn't
        /// </summary>
        [TestMethod]
        public void TestHasDependees2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "Bob");
            Assert.AreEqual(false, d.HasDependees("Marry"));
        }

        /// <summary>
        /// This is asking if d has dependees which it does
        /// </summary>
        [TestMethod]
        public void TestHasDependees3()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            Assert.AreEqual(true, d.HasDependees("d"));
        }

        /// <summary>
        /// This is asking if a has dependess which it doesn't
        /// </summary>
        [TestMethod]
        public void TestHasDependees4()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            Assert.AreEqual(false, d.HasDependees("a"));
        }

        /// <summary>
        /// This is creates a list of sally's dependents which has the count of 2
        /// </summary>
        [TestMethod]
        public void TestGetDependents()
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

        /// <summary>
        /// This is asking the size of get dependents of jim and since jim is a dependent and has not dependees it's count is 0
        /// </summary>
        [TestMethod]
        public void TestGetDependents2()
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

        /// <summary>
        /// This is getting the dependees of john which has the count of 2
        /// </summary>
        [TestMethod]
        public void TestGetDependees()
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

        /// <summary>
        /// This is asking for the dependees of sally which is a dependee and has no dependees
        /// </summary>
        [TestMethod]
        public void TestGetDependees2()
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

        /// <summary>
        /// This is removing (sally, jim) and (sally, bob) which just leaves (marry, john) which has a count of 1
        /// </summary>
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

        /// <summary>
        /// This is removing (sally, jim) so it has the size of 2
        /// </summary>
        [TestMethod]
        public void TestRemoveDependency2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("Marry", "John");
            d.AddDependency("Sally", "Jim");
            d.AddDependency("Sally", "Bob");
            d.RemoveDependency("Sally", "Jim");
            Assert.AreEqual(d.Size, 2);
        }

        /// <summary>
        /// This is removing (b, d) so it has the size of 3
        /// </summary>
        [TestMethod]
        public void TestRemoveDependency3()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "d");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            d.RemoveDependency("b", "d");
            Assert.AreEqual(d.Size, 3);
        }

        /// <summary>
        /// This replaces (sally, jim) and (sally, bob) to (sally, alex) and (sally, jake) with the size of 2
        /// </summary>
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
            Assert.AreEqual(d.Size, 3);
        }

        /// <summary>
        /// This replaces (a, b) and (a, d) to (a, e) and (a, f) which has the size of 3
        /// </summary>
        [TestMethod]
        public void TestRepleaceDependents2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "d");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            List<string> newDependents = new List<string>();
            newDependents.Add("e");
            newDependents.Add("f");
            d.ReplaceDependents("a", newDependents);
            Assert.AreEqual(d.Size, 4);
        }

        /// <summary>
        /// This replaces (marry, john) and (sally, john) to (jessica, john) and (kelly, john) with the size of 4
        /// </summary>
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

        /// <summary>
        /// This is replacing (b, d) and (d, d) to (e, d) and (f, d) which makes the size 3
        /// </summary>
        [TestMethod]
        public void TestRepleaceDependees2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            List<string> newDependees = new List<string>();
            newDependees.Add("e");
            newDependees.Add("f");
            d.ReplaceDependees("d", newDependees);
            Assert.AreEqual(d.Size, 4);
        }

        /// <summary>
        /// This is replacing (b, d) and (d, d) to (e, d) and (f, d) with the size of 5
        /// </summary>
        [TestMethod]
        public void TestRepleaceDependees3()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            d.AddDependency("e", "c");
            List<string> newDependees = new List<string>();
            newDependees.Add("e");
            newDependees.Add("f");
            d.ReplaceDependees("d", newDependees);
            Assert.AreEqual(d.Size, 5);
        }

        /// <summary>
        /// This is replacing (b, d) and (d, d) to (e, d) and (f, d) with the size of 5
        /// </summary>
        [TestMethod]
        public void TestRepleaceDependees4()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            d.AddDependency("e", "c");
            List<string> newDependees = new List<string>();
            d.ReplaceDependees("d", newDependees);
            Assert.AreEqual(d.Size, 3);
        }

        /// <summary>
        /// This adds the same dependecies which will not add one if it already contains it
        /// </summary>
        [TestMethod]
        public void TestAddingMultiples()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "b");
            Assert.AreEqual(d.Size, 1);
        }

        /// <summary>
        /// This is a test to remove something not in the dependency graph
        /// </summary>
        [TestMethod]
        public void TestRemovingItemNotInGraph()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.RemoveDependency("j", "k");
            Assert.AreEqual(d.Size, 2);
        }

        /// <summary>
        /// This is a test to remove something after it is already removed
        /// </summary>
        [TestMethod]
        public void TestRemovingSameOne()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "b");
            d.RemoveDependency("a", "b");
            d.RemoveDependency("a", "b");
            Assert.AreEqual(d.Size, 0);
        }

        /// <summary>
        /// This is asking what the dependents of a are which are b, c
        /// </summary>
        [TestMethod]
        public void TestEquationFromSummary()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            List<string> Dependents = new List<string>();
            foreach (string dependent in d.GetDependents("a"))
            {
                Dependents.Add(dependent);
            }
            Assert.AreEqual(Dependents[0], "b");
            Assert.AreEqual(Dependents[1], "c");
        }

        /// <summary>
        /// This is asking what the dependents of b are which is d
        /// </summary>
        [TestMethod]
        public void TestEquationFromSummary2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            List<string> Dependents = new List<string>();
            foreach (string dependent in d.GetDependents("b"))
            {
                Dependents.Add(dependent);
            }
            Assert.AreEqual(Dependents[0], "d");
        }

        /// <summary>
        /// This is asking the dependents of c which c has none so the count is 0
        /// </summary>
        [TestMethod]
        public void TestEquationFromSummary3()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            List<string> Dependents = new List<string>();
            foreach (string dependent in d.GetDependents("c"))
            {
                Dependents.Add(dependent);
            }
            Assert.AreEqual(Dependents.Count, 0);
        }

        /// <summary>
        /// This asking the dependents of d which is d
        /// </summary>
        [TestMethod]
        public void TestEquationFromSummary4()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            List<string> Dependents = new List<string>();
            foreach (string dependent in d.GetDependents("d"))
            {
                Dependents.Add(dependent);
            }
            Assert.AreEqual(Dependents[0], "d");
        }

        /// <summary>
        /// This is aksing the dependees of a which is 0 cause a is a dependee
        /// </summary>
        [TestMethod]
        public void TestEquationFromSummary5()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            List<string> Dependess = new List<string>();
            foreach (string dependent in d.GetDependees("a"))
            {
                Dependess.Add(dependent);
            }
            Assert.AreEqual(Dependess.Count, 0);
        }

        /// <summary>
        /// This asking the dependees of b which is a
        /// </summary>
        [TestMethod]
        public void TestEquationFromSummary6()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            List<string> Dependess = new List<string>();
            foreach (string dependent in d.GetDependees("b"))
            {
                Dependess.Add(dependent);
            }
            Assert.AreEqual(Dependess[0], "a");
        }

        /// <summary>
        /// This is asking the dependees of c which is a
        /// </summary>
        [TestMethod]
        public void TestEquationFromSummary7()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            List<string> Dependess = new List<string>();
            foreach (string dependent in d.GetDependees("c"))
            {
                Dependess.Add(dependent);
            }
            Assert.AreEqual(Dependess[0], "a");
        }

        /// <summary>
        /// This is asking the dependees of d which are b, d
        /// </summary>
        [TestMethod]
        public void TestEquationFromSummary8()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "b");
            d.AddDependency("a", "c");
            d.AddDependency("b", "d");
            d.AddDependency("d", "d");
            List<string> Dependess = new List<string>();
            foreach (string dependent in d.GetDependees("d"))
            {
                Dependess.Add(dependent);
            }
            Assert.AreEqual(Dependess[0], "b");
            Assert.AreEqual(Dependess[1], "d");
        }

        /// <summary>
        /// This is a test with 100,000 dependecies
        /// </summary>
        [TestMethod]
        public void TestUnique100000Dependcies()
        {
            DependencyGraph d = new DependencyGraph();

            string key = "A";
            string value = "B";

            for (int i = 0; i < 100000; i++)
            {
                d.AddDependency(key + i.ToString(), value + i.ToString());
            }
        }

        /// <summary>
        /// This is a test with 100,000 dependecies
        /// </summary>
        [TestMethod]
        public void Test100000Dependcies()
        {
            DependencyGraph d = new DependencyGraph();

            string key = "A";
            string value = "B";

            for (int i = 0; i < 100000; i++)
            {
                d.AddDependency(key, value + i.ToString());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("A", "B");
            foreach (string dependee in d.GetDependees(null))
            {

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull3()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("A", "B");
            foreach (string dependent in d.GetDependents(null))
            {

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull4()
        {
            DependencyGraph d = new DependencyGraph();
            d.HasDependees(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull5()
        {
            DependencyGraph d = new DependencyGraph();
            d.HasDependents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull6()
        {
            DependencyGraph d = new DependencyGraph();
            d.RemoveDependency(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull7()
        {
            DependencyGraph d = new DependencyGraph();
            d.ReplaceDependees(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull8()
        {
            DependencyGraph d = new DependencyGraph();
            d.ReplaceDependents(null, null);
        }

        [TestMethod]
        public void TestDependencyGraphUsingDependencyGraph()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("A", "B");
            d.AddDependency("A", "C");
            d.AddDependency("B", "A");
            d.AddDependency("B", "B");

            DependencyGraph d2 = new DependencyGraph(d);
            d2.AddDependency("D", "D");

            Assert.AreEqual(4, d.Size);
            Assert.AreEqual(5, d2.Size);
        }

        [TestMethod]
        public void TestDependencyGraphUsingDependencyGraph2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("A", "B");
            d.AddDependency("A", "C");
            d.AddDependency("B", "A");
            d.AddDependency("B", "B");

            DependencyGraph d2 = new DependencyGraph(d);
            d.AddDependency("D", "D");

            Assert.AreEqual(5, d.Size);
            Assert.AreEqual(4, d2.Size);
        }

        [TestMethod]
        public void TestDependencyGraphUsingDependencyGraph3()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("A", "B");
            d.AddDependency("A", "C");
            d.AddDependency("B", "A");
            d.AddDependency("B", "B");

            DependencyGraph d2 = new DependencyGraph(d);
            d.RemoveDependency("A", "B");

            Assert.AreEqual(3, d.Size);
            Assert.AreEqual(4, d2.Size);
        }

        [TestMethod]
        public void TestCopyLargeGraph()
        {
            DependencyGraph d = new DependencyGraph();

            string key = "A";
            string value = "B";

            for (int i = 0; i < 100000; i++)
            {
                d.AddDependency(key + i.ToString(), value + i.ToString());
            }

            DependencyGraph d2 = new DependencyGraph(d);

        }
    }
}
    