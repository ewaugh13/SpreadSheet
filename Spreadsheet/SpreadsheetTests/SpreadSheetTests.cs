using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using Formulas;
using System.Collections.Generic;
using Dependencies;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadSheetTests
    {

        /// <summary>
        /// Tests adding 2 elements with doubles
        /// </summary>
        [TestMethod]
        public void TestSetCell()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a17", 5);
            sheet.SetCellContents("B5", 6);
        }

        /// <summary>
        /// Tests invalid name type that throws invalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a07", 5);
        }

        /// <summary>
        /// Tests invalid name type
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("Z", 5);
        }

        /// <summary>
        /// Test invalid name type and throws invalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("hello", 5);
        }

        /// <summary>
        /// Tests inserting formula
        /// </summary>
        [TestMethod]
        public void TestSetCell5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", 6);
            sheet.SetCellContents("C9", "B5");
            Formula form = new Formula("8 / 2");
            sheet.SetCellContents("D7", form);
        }

        /// <summary>
        /// Tests if set with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetTextNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();            
            sheet.SetCellContents("Z", null);
        }

        /// <summary>
        /// Tests with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetFormNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form = new Formula();
            sheet.SetCellContents("Z", form);
        }

        /// <summary>
        /// Tests the amount of GetNamesOfAllNonEmptyCells which is 3
        /// </summary>
        [TestMethod]
        public void TestGetNames()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", 6);
            sheet.SetCellContents("C9", "B5");
            Formula form = new Formula("8 / 2");
            sheet.SetCellContents("D7", form);
            List<string> cellsWithName = new List<string>();
            foreach (string cellName in sheet.GetNamesOfAllNonemptyCells())
            {
                cellsWithName.Add(cellName);
            }
            Assert.AreEqual(3, cellsWithName.Count);
        }

        /// <summary>
        /// Test get names of all non empty cells
        /// </summary>
        [TestMethod]
        public void TestGetNames2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", null);
            sheet.SetCellContents("C9", "B5");
            Formula form = new Formula("8 / 2");
            sheet.SetCellContents("D7", form);
            sheet.SetCellContents("E4", "");
            List<string> cellsWithName = new List<string>();
            foreach (string cellName in sheet.GetNamesOfAllNonemptyCells())
            {
                cellsWithName.Add(cellName);
            }
            Assert.AreEqual(2, cellsWithName.Count);
        }

        /// <summary>
        /// Test get contents of certain cells
        /// </summary>
        [TestMethod]
        public void TestGetContents()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", 6);
            sheet.SetCellContents("C9", "B5");
            Formula form = new Formula("8 / 2");
            sheet.SetCellContents("D7", form);
            Assert.AreEqual(6.0, sheet.GetCellContents("B5"));
            Assert.AreEqual("B5", sheet.GetCellContents("C9"));
            Assert.AreEqual(form, sheet.GetCellContents("D7"));
        }

        /// <summary>
        /// Tests invalid get cell contents
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetContents2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", 6);
            sheet.GetCellContents("B05");
        }

        /// <summary>
        /// Test get cell contents with null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetContents3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", 6);
            sheet.GetCellContents(null);
        }

        /// <summary>
        /// Tests seting name with null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null, "A5");
        }

        /// <summary>
        /// Tests set using number and text
        /// </summary>
        [TestMethod]
        public void TestSetUsingNumberandText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", 6);
            HashSet<string> dependents = new HashSet<string>();
            Formula form = new Formula("B5 * 2");
            foreach (string s in sheet.SetCellContents("C1", form))
            {
                dependents.Add(s);
            }
            Assert.AreEqual(1, dependents.Count);
        }

        /// <summary>
        /// Test sets using number and text
        /// </summary>
        [TestMethod]
        public void TestSetUsingNumberandText2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("C1", "B5");
            HashSet<string> dependents = new HashSet<string>();
            foreach (string s in sheet.SetCellContents("B5", 6))
            {
                dependents.Add(s);
            }
            Assert.AreEqual(1, dependents.Count);
        }

        /// <summary>
        /// Test the contents that contain c1 being 2
        /// </summary>
        [TestMethod]
        public void TestSetUsingNumberandFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", 6);
            HashSet<string> elementsWithC1 = new HashSet<string>();
            Formula form = new Formula("B5 * 2");
            Formula form2 = new Formula("C1");
            sheet.SetCellContents("D1", form2);
            foreach (string s in sheet.SetCellContents("C1", form))
            {
                elementsWithC1.Add(s);
            }
            Assert.AreEqual(2, elementsWithC1.Count);
        }

        /// <summary>
        /// Test the contents that contain c1 being 3
        /// </summary>
        [TestMethod]
        public void TestSetUsingNumberandFormula2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", 6);
            HashSet<string> elementsWithC1 = new HashSet<string>();
            Formula form = new Formula("B5 * 2");
            Formula form2 = new Formula("C1");
            Formula form3 = new Formula("D1");
            sheet.SetCellContents("D1", form2);
            sheet.SetCellContents("E1", form3);
            foreach (string s in sheet.SetCellContents("C1", form))
            {
                elementsWithC1.Add(s);
            }
            Assert.AreEqual(3, elementsWithC1.Count);
        }

        /// <summary>
        /// Tests replacing cells
        /// </summary>
        [TestMethod]
        public void TestSetReplacing()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("C1", "B5");
            sheet.SetCellContents("c1", "A5");
            sheet.SetCellContents("D7", 7);
            sheet.SetCellContents("d7", 8);
        }

        /// <summary>
        /// Tests getting empty cell
        /// </summary>
        [TestMethod]
        public void TestEmptyCell()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("C1", "");
            Assert.AreEqual("", sheet.GetCellContents("c1"));
            Assert.AreEqual("", sheet.GetCellContents("C1"));
            Assert.AreEqual("", sheet.GetCellContents("D1"));
        }

        /// <summary>
        /// Tests replacing formulas
        /// </summary>
        [TestMethod]
        public void TestSwitchFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form = new Formula("B1 + C1");
            sheet.SetCellContents("A1", form);
            form = new Formula("D1 + E1");
            sheet.SetCellContents("A1", form);
            HashSet<string> elements = new HashSet<string>();
            foreach (string item in sheet.SetCellContents("E1", 2))
            {
                elements.Add(item);
            }
            Assert.AreEqual(2, elements.Count);
        }

        /// <summary>
        /// Tests replacing with a formula
        /// </summary>
        [TestMethod]
        public void TestSwitchFormula2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form = new Formula("B1 + C1");
            sheet.SetCellContents("A1", 2);
            sheet.SetCellContents("A1", form);
        }

        /// <summary>
        /// Tests creating a ciruclar expression
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularException()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form = new Formula("A5");
            Formula form2 = new Formula("B5");
            Formula form3 = new Formula("C5");
            sheet.SetCellContents("A5", form2);
            sheet.SetCellContents("B5", form3);
            sheet.SetCellContents("C5", form);
        }

        /// <summary>
        /// Does a strees test of lots of elements with formulas
        /// </summary>
        [TestMethod]
        public void TestStress()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            for (int i = 1; i < 100000; i++)
            {
                sheet.SetCellContents("A" + i.ToString(), new Formula("B" + i.ToString()));
            }
        }

        /// <summary>
        /// Does a stress test of lots of elements with doubles
        /// </summary>
        [TestMethod]
        public void TestStress2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            for (int i = 1; i < 100000; i++)
            {
                sheet.SetCellContents("A" + i.ToString(), 6);
            }
        }
    }
}
