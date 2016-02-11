// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Revised for CS 3500 by Joe Zachary, January 29, 2016

using System;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> GraphKeyDependee;

        private Dictionary<string, HashSet<string>> GraphKeyDependent;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            GraphKeyDependee = new Dictionary<string, HashSet<string>>();

            GraphKeyDependent = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return GetSize(); }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null and throws exception if it is.
        /// </summary>
        public bool HasDependents(string s)
        {
            if(s == null)
            {
                throw new ArgumentNullException("Input s is null");
            }

            if (GraphKeyDependee.ContainsKey(s))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null and throwns exception if it is.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("Input s is null");
            }

            if (GraphKeyDependent.ContainsKey(s))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null and throws exception if it is.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("Input s is null");
            }

            if (GraphKeyDependee.ContainsKey(s))
            {
                foreach (string dependent in GraphKeyDependee[s])
                {
                    yield return dependent;
                }
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null and throws exception if it is.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("Input s is null");
            }

            if (GraphKeyDependent.ContainsKey(s))
            {
                foreach (string dependee in GraphKeyDependent[s])
                {
                    yield return dependee;
                }
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null and throws exception if either one is.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if ((s == null) || (t == null))
            {
                throw new ArgumentNullException("Input s or t is null");
            }

            if (GraphKeyDependee.ContainsKey(s))
            {
                GraphKeyDependee[s].Add(t);
            }
            else
            {
                HashSet<string> tSet = new HashSet<string>();
                tSet.Add(t);
                GraphKeyDependee.Add(s, tSet);
            }

            if (GraphKeyDependent.ContainsKey(t))
            {
                GraphKeyDependent[t].Add(s);
            }
            else
            {
                HashSet<string> sSet = new HashSet<string>();
                sSet.Add(s);
                GraphKeyDependent.Add(t, sSet);
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null and throws exception if either one is.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if ((s == null) || (t == null))
            {
                throw new ArgumentNullException("Input s or t is null");
            }

            if (GraphKeyDependee.ContainsKey(s))
            {
                if (GraphKeyDependee[s].Count > 1) // Removes 1 element t from list
                {
                    GraphKeyDependee[s].Remove(t);
                }
                else
                {
                    GraphKeyDependee.Remove(s); // Removes the dependee s
                }
            }



            if (GraphKeyDependent.ContainsKey(t))
            {
                if (GraphKeyDependent[t].Count > 1) // Removes 1 element s from list
                {
                    GraphKeyDependent[t].Remove(s);
                }
                else
                {
                    GraphKeyDependent.Remove(t); // Removes the dependee t
                }

            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null and throws excpetion if either one is.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if ((s == null) || (newDependents == null))
            {
                throw new ArgumentNullException("Input s or new depenndents is null");
            }

            if (GraphKeyDependee.ContainsKey(s))
            {
                GraphKeyDependee[s] = new HashSet<string>(); // Creates new list of dependents and adds new dependents
                foreach (string Dependent in newDependents)
                {
                    GraphKeyDependee[s].Add(Dependent);
                }
            }


            List<string> DependentsToReplace = new List<string>();

            foreach (KeyValuePair<string, HashSet<string>> Dep in GraphKeyDependent)
            {
                if (GraphKeyDependent[Dep.Key].Contains(s)) // Finds keys that contain s and adds to dependendentsToReplace
                {
                    DependentsToReplace.Add(Dep.Key);
                }
            }

            foreach (string dependent in DependentsToReplace) // Removes the Dependents
            {
                if (GraphKeyDependent.ContainsKey(dependent))
                {
                    if (GraphKeyDependent[dependent].Count > 1) // Removes 1 element s from list
                    {
                        GraphKeyDependent[dependent].Remove(s);
                    }
                    else
                    {
                        GraphKeyDependent.Remove(dependent); // Removes the dependent t

                    }
                }
            }

            foreach (string newDependee in newDependents) // Adds new dependents based of s and new dependents
            {
                if (GraphKeyDependent.ContainsKey(newDependee))
                {
                    GraphKeyDependent[newDependee].Add(s);
                }
                else
                {
                    HashSet<string> sSet = new HashSet<string>();
                    sSet.Add(s);
                    GraphKeyDependent.Add(newDependee, sSet);
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null and throws exception if either one is.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if ((newDependees == null) || (t == null))
            {
                throw new ArgumentNullException("Input newDependess or t is null");
            }

            if (GraphKeyDependent.ContainsKey(t))
            {
                GraphKeyDependent[t] = new HashSet<string>(); // Creates new list of dependees and adds new dependees
                foreach (string Dependent in newDependees)
                {
                    GraphKeyDependent[t].Add(Dependent);
                }

            }

            List<string> DependessToReplace = new List<string>();

            foreach (KeyValuePair<string, HashSet<string>> Dep in GraphKeyDependee)
            {
                if (GraphKeyDependee[Dep.Key].Contains(t)) // Finds keys that contain t and adds to dependessToReplace
                {
                    DependessToReplace.Add(Dep.Key);
                }
            }

            foreach (string dependee in DependessToReplace) // Removes the Dependecies
            {
                if (GraphKeyDependee.ContainsKey(dependee))
                {
                    if (GraphKeyDependee[dependee].Count > 1) // Removes 1 element t from list
                    {
                        GraphKeyDependee[dependee].Remove(t);
                    }
                    else
                    {
                        GraphKeyDependee.Remove(dependee); // Removes the dependee
                    }
                }
            }

            foreach (string newDependee in newDependees) // Adds new dependecies based of t and new dependees
            {
                if (GraphKeyDependee.ContainsKey(newDependee))
                {
                    GraphKeyDependee[newDependee].Add(t);
                }
                else
                {
                    HashSet<string> tSet = new HashSet<string>();
                    tSet.Add(t);
                    GraphKeyDependee.Add(newDependee, tSet);
                }
            }
        }

        /// <summary>
        /// Goes through each key value pair in graph and counts the number of dependecies
        /// by looking at each list of dependents and adding the count to the total.
        /// </summary>
        private int GetSize()
        {
            int NumOfDep = 0;
            foreach (KeyValuePair<string, HashSet<string>> Dep in GraphKeyDependee)
            {
                NumOfDep += GraphKeyDependee[Dep.Key].Count;
            }
            return NumOfDep;
        }
    }
}
