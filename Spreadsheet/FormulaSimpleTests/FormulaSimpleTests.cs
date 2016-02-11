// Written by Joe Zachary for CS 3500, January 2016.
// Repaired error in Evaluate5.  Added TestMethod Attribute
//    for Evaluate4 and Evaluate5 - JLZ January 25, 2016
// Corrected comment for Evaluate3 - JLZ January 29, 2016

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        /// <summary>
        /// syntax error with wrong characters.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test1()
        {
            Formula f = new Formula("(? ! $");
        }

        /// <summary>
        /// syntax error with no tokens.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test2()
        {
            Formula f = new Formula("");
        }

        /// <summary>
        /// Too many closing parenthesis after opening parenthesis.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test3()
        {
            Formula f = new Formula("())(");
        }

        /// <summary>
        /// Another syntax error not equal parenthesis counts.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test4()
        {
            Formula f = new Formula("((2 + 2) + 4))");
        }

        /// <summary>
        /// Another syntax error leading with a operator.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test5()
        {
            Formula f = new Formula("+ 2");
        }

        /// <summary>
        /// Another syntax error not equal parenthesis.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test6()
        {
            Formula f = new Formula("(2 +");
        }

        /// <summary>
        /// Another syntax error of no variables or numbers inputer.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test7()
        {
            Formula f = new Formula("( )");
        }

        /// <summary>
        /// Another syntax error with no operator inbetween number and variable.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test8()
        {
            Formula f = new Formula("(2 x3");
        }

        /// <summary>
        /// Another syntax error with wrong variable format
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test9()
        {
            Formula f = new Formula("x2?");
        }

        /// <summary>
        /// Another syntax error with more closing before opening
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test10()
        {
            Formula f = new Formula("(2))(");
        }

        /// <summary>
        /// Another syntax error with operator at end
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test11()
        {
            Formula f = new Formula("2x + 3 +");
        }


        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            f.Evaluate(Lookup4);
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        /// <summary>
        /// Testing dividing by variable
        /// </summary>
        [TestMethod]
        public void Evaluate6()
        {
            Formula f = new Formula("(x / x) / (y / y)");
            f.Evaluate(Lookup4);
            Assert.AreEqual(f.Evaluate(Lookup4), 1, 1e-6);
        }

        /// <summary>
        /// Testing dividing by variable that is 0
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate7()
        {
            Formula f = new Formula("4 / x");
            f.Evaluate(Lookup5);
        }

        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        /// <summary>
        /// A Lookup method that maps x to 0.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup5(String v)
        {
            switch (v)
            {
                case "x": return 0.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        /// <summary>
        /// Test division being in correct order.
        /// </summary>
        [TestMethod]
        public void TestEvaluate1()
        {
            Formula f = new Formula("4/2");
            Assert.AreEqual(f.Evaluate(v => 0), 2.0, 1e-6);
        }

        /// <summary>
        /// Tests multiple +'s and -'s.
        /// </summary>
        [TestMethod]
        public void TestEvaluate2()
        {
            Formula f = new Formula("2+3+7-2-4");
            Assert.AreEqual(f.Evaluate(v => 0), 6.0, 1e-6);
        }

        /// <summary>
        /// Test parenthesis order.
        /// </summary>
        [TestMethod]
        public void TestEvaluate3()
        {
            Formula f = new Formula("7-(2+3)");
            Assert.AreEqual(f.Evaluate(v => 0), 2.0, 1e-6);
        }

        /// <summary>
        /// Does same test as TestEvaluate3 without parenthesis.
        /// </summary>
        [TestMethod]
        public void TestEvaluate4()
        {
            Formula f = new Formula("7-2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 8.0, 1e-6);
        }

        /// <summary>
        /// Test multiple ordering things like parenthesis and multiplication.
        /// </summary>
        [TestMethod]
        public void TestEvaluate5()
        {
            Formula f = new Formula("(4+6)*(8/4)*1");
            Assert.AreEqual(f.Evaluate(v => 0), 20.0, 1e-6);
        }

        /// <summary>
        /// Tests multiplication and division.
        /// </summary>
        [TestMethod]
        public void TestEvaluate6()
        {
            Formula f = new Formula("2*2/4");
            Assert.AreEqual(f.Evaluate(v => 0), 1.0, 1e-6);
        }


        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestDelegate()
        {
            Formula f = new Formula("x2 + y + z4", ToCaps, charAndDigit);
        }

        [TestMethod()]
        public void TestDelegate2()
        {
            Formula f = new Formula("x2 + y3 + z4", ToCaps, charAndDigit);
            f.ToString();
        }

        [TestMethod()]
        public void TestToString()
        {
            Formula f1 = new Formula("x2 + y3 + z4", ToCaps, charAndDigit);
            Formula f2 = new Formula(f1.ToString(), s => s, s => true);
            Assert.AreEqual(f1.Evaluate(LookupForTestString), f2.Evaluate(LookupForTestString));
        }

        [TestMethod()]
        public void TestDelegate4()
        {
            Formula f1 = new Formula("x2 + y3 + z4", ToCaps, charAndDigit);
            Formula f2 = new Formula("x2 + y3 + z4", s => s, s => true);
            Assert.AreEqual(f1.Evaluate(LookupForTestString), 18.0);
            Assert.AreEqual(f2.Evaluate(LookupForTestString), 9.0);
            Assert.AreNotEqual(f1.Evaluate(LookupForTestString), f2.Evaluate(LookupForTestString));
        }

        [TestMethod()]
        public void TestGetVariables()
        {
            Formula f1 = new Formula("x2 + y3 + z4", ToCaps, charAndDigit);
            HashSet<string> myVariables = new HashSet<string>();
            foreach (string var in f1.GetVariables())
            {
                myVariables.Add(var);
            }
            Assert.AreEqual(myVariables.Count, 3);
        }

        [TestMethod()]
        public void TestGetVariables2()
        {
            Formula f1 = new Formula("(1 + 5) / 2", ToCaps, charAndDigit);
            HashSet<string> myVariables = new HashSet<string>();
            foreach (string var in f1.GetVariables())
            {
                myVariables.Add(var);
            }
            Assert.AreEqual(myVariables.Count, 0);
        }

        [TestMethod()]
        public void TestGetVariables3()
        {
            Formula f1 = new Formula("x3 + y4", ToCaps, charAndDigit);
            HashSet<string> myVariables = new HashSet<string>();
            foreach (string var in f1.GetVariables())
            {
                myVariables.Add(var);
            }
            Assert.AreEqual(true, myVariables.Contains("X3"));
            Assert.AreEqual(true, myVariables.Contains("Y4"));
        }

        [TestMethod()]
        public void TestGetVariables4()
        {
            Formula f1 = new Formula("x3 + y4 + x3", ToCaps, charAndDigit);
            HashSet<string> myVariables = new HashSet<string>();
            foreach (string var in f1.GetVariables())
            {
                myVariables.Add(var);
            }
            Assert.AreEqual(myVariables.Count, 2);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestDelegate5()
        {
            Formula f1 = new Formula("x2 + y3 + z4", ChangeToHashTag, charAndDigit);
        }


        public string ToCaps(string formula)
        {
            return formula.ToUpper();
        }

        public string ChangeToHashTag(string formula)
        {
            return "#";
        }

        public bool charAndDigit(string formula)
        {
            char[] chars = formula.ToCharArray();
            if (chars.Length == 2)
            {
                if (Char.IsLetter(chars[0]) && Char.IsDigit(chars[1]))
                {
                    return true;
                }
            }
            return false;
        }

        public double LookupForTestString(String v)
        {
            switch (v)
            {
                case "x2": return 2.0;
                case "y3": return 3.0;
                case "z4": return 4.0;

                case "X2": return 4.0;
                case "Y3": return 6.0;
                case "Z4": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }
    }
}