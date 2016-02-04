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
        private Dictionary<string, List<string>> Graph; 

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            Graph = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return Graph.Count; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            if(Graph.ContainsKey(s))
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            foreach (KeyValuePair<string, List<string>> Dep in Graph)
            {
                if(Graph[Dep.Key].Contains(s)) // If the current key has s
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            foreach (KeyValuePair<string, List<string>> Dep in Graph)
            {
                if (Dep.Key == s)
                {
                    foreach (string value in Graph[Dep.Key]) // Yields all values in Graph at the current key if it was equal to s
                    {
                        yield return value;
                    }
                }
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            foreach (KeyValuePair<string, List<string>> Dep in Graph)
            {
                if (Graph[Dep.Key].Contains(s)) // If any graph at a certain key contains s it returns the key
                {
                    yield return Dep.Key;
                }
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (Graph.ContainsKey(s))
            {
                if (!(Graph[s].Contains(t)))
                {
                    Graph[s].Add(t);
                }
            }
            else
            {
                List<string> tList = new List<string>();
                tList.Add(t);
                Graph.Add(s, tList);
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            foreach(KeyValuePair<string, List<string>> Dep in Graph)
            {
                if(Dep.Key == s)
                {
                    if(Dep.Value.Count > 1) // Removes 1 element t from list
                    {
                        Dep.Value.Remove(t);
                    }
                    else
                    {
                        Graph.Remove(s); // Removes the dependee s
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            foreach (KeyValuePair<string, List<string>> Dep in Graph)
            {
                if (Dep.Key == s)
                {
                    Graph[Dep.Key] = new List<string>(); // Creates new list of dependents and adds new dependents
                    foreach (string Dependent in newDependents)
                    {
                        Graph[Dep.Key].Add(Dependent);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            List<string> DependessToReplace = new List<string>();

            foreach (KeyValuePair<string, List<string>> Dep in Graph)
            {
                if (Graph[Dep.Key].Contains(t)) // Finds keys that contain t and adds to dependessToReplace
                {
                    DependessToReplace.Add(Dep.Key);
                }
            }

            foreach (string dependee in DependessToReplace) // Removes the Dependecies
            {
                RemoveDependency(dependee, t);
            }

            foreach (string newDependee in newDependees) // Adds new dependecies based of t and new dependees
            {
                AddDependency(newDependee, t);
            }

        }
    }
}
