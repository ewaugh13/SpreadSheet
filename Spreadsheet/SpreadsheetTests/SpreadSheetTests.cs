using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using Formulas;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadSheetTests
    {

        Regex regex = new Regex(@"^[a-zA-Z]+[1-9]\d*$");

        /// <summary>
        /// Tests adding 2 elements with doubles
        /// </summary>
        [TestMethod]
        public void TestSetCell()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("a17", 5.ToString());
            sheet.SetContentsOfCell("B5", 6.ToString());
        }

        /// <summary>
        /// Tests invalid name type that throws invalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("a07", 5.ToString());
        }

        /// <summary>
        /// Tests invalid name type
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("Z", 5.ToString());
        }

        /// <summary>
        /// Test invalid name type and throws invalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("hello", 5.ToString());
        }

        /// <summary>
        /// Tests inserting formula
        /// </summary>
        [TestMethod]
        public void TestSetCell5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("B5", 6.ToString());
            sheet.SetContentsOfCell("C9", "B5");
            Formula form = new Formula("8 / 2");
            sheet.SetContentsOfCell("D7", "=" + form.ToString());
        }

        /// <summary>
        /// Tests if set with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetTextNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("Z", null);
        }

        /// <summary>
        /// Tests with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetFormNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            Formula form = new Formula();
            sheet.SetContentsOfCell("Z", "=" + form.ToString());
        }

        /// <summary>
        /// Tests the amount of GetNamesOfAllNonEmptyCells which is 3
        /// </summary>
        [TestMethod]
        public void TestGetNames()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("B5", 6.ToString());
            sheet.SetContentsOfCell("C9", "B5");
            Formula form = new Formula("8 / 2");
            sheet.SetContentsOfCell("D7", "=" + form.ToString());
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
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("C9", "B5");
            Formula form = new Formula("8 / 2");
            sheet.SetContentsOfCell("D7", "=" + form.ToString());
            sheet.SetContentsOfCell("E4", "");
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
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("B5", 6.ToString());
            sheet.SetContentsOfCell("C9", "B5");
            Formula form = new Formula("8 / 2");
            sheet.SetContentsOfCell("D7", "=" + form.ToString());
            Assert.AreEqual(6.0, sheet.GetCellContents("B5"));
            Assert.AreEqual("B5", sheet.GetCellContents("C9"));
            Assert.AreEqual(form.ToString(), sheet.GetCellContents("D7").ToString());
        }

        /// <summary>
        /// Tests invalid get cell contents
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetContents2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("B5", 6.ToString());
            sheet.GetCellContents("B05");
        }

        /// <summary>
        /// Test get cell contents with null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetContents3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("B5", 6.ToString());
            sheet.GetCellContents(null);
        }

        /// <summary>
        /// Tests seting name with null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell(null, "A5");
        }

        /// <summary>
        /// Tests set using number and text
        /// </summary>
        [TestMethod]
        public void TestSetUsingNumberandText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("B5", 6.ToString());
            HashSet<string> dependents = new HashSet<string>();
            Formula form = new Formula("B5 * 2");
            foreach (string s in sheet.SetContentsOfCell("C1", "=" + form.ToString()))
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
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("C1", "B5");
            HashSet<string> dependents = new HashSet<string>();
            foreach (string s in sheet.SetContentsOfCell("B5", 6.ToString()))
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
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("B5", 6.ToString());
            HashSet<string> elementsWithC1 = new HashSet<string>();
            Formula form = new Formula("B5 * 2");
            Formula form2 = new Formula("C1");
            sheet.SetContentsOfCell("D1", "=" + form2.ToString());
            foreach (string s in sheet.SetContentsOfCell("C1", "=" + form.ToString()))
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
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("B5", 6.ToString());
            HashSet<string> elementsWithC1 = new HashSet<string>();
            Formula form = new Formula("B5 * 2");
            Formula form2 = new Formula("C1");
            Formula form3 = new Formula("D1");
            sheet.SetContentsOfCell("D1", "=" + form2.ToString());
            sheet.SetContentsOfCell("E1", "=" + form3.ToString());
            foreach (string s in sheet.SetContentsOfCell("C1", "=" + form.ToString()))
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
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("C1", "B5");
            sheet.SetContentsOfCell("c1", "A5");
            sheet.SetContentsOfCell("D7", 7.ToString());
            sheet.SetContentsOfCell("d7", 8.ToString());
        }

        /// <summary>
        /// Tests getting empty cell
        /// </summary>
        [TestMethod]
        public void TestEmptyCell()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("C1", "");
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
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            Formula form = new Formula("B1 + C1");
            sheet.SetContentsOfCell("A1", "=" + form.ToString());
            form = new Formula("D1 + E1");
            sheet.SetContentsOfCell("A1", "=" + form.ToString());
            HashSet<string> elements = new HashSet<string>();
            foreach (string item in sheet.SetContentsOfCell("E1", 2.ToString()))
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
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            Formula form = new Formula("B1 + C1");
            sheet.SetContentsOfCell("A1", 2.ToString());
            sheet.SetContentsOfCell("A1", "=" + form.ToString());
        }

        /// <summary>
        /// Tests creating a ciruclar expression
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularException()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            Formula form = new Formula("A5");
            Formula form2 = new Formula("B5");
            Formula form3 = new Formula("C5");
            sheet.SetContentsOfCell("A5", "=" + form2.ToString());
            sheet.SetContentsOfCell("B5", "=" + form3.ToString());
            sheet.SetContentsOfCell("C5", "=" + form.ToString());
        }

        /// <summary>
        /// Does a strees test of lots of elements with formulas
        /// </summary>
        [TestMethod]
        public void TestStress()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            for (int i = 1; i < 10000; i++)
            {
                sheet.SetContentsOfCell("A" + i.ToString(), "=" + (new Formula("B" + i.ToString())).ToString());
            }
        }

        /// <summary>
        /// Does a stress test of lots of elements with doubles
        /// </summary>
        [TestMethod]
        public void TestStress2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            for (int i = 1; i < 10000; i++)
            {
                sheet.SetContentsOfCell("A" + i.ToString(), 6.ToString());
            }
        }

        /// <summary>
        /// // SETTING CELL TO A STRING
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test7()
        {
            AbstractSpreadsheet s = new Spreadsheet(regex);
            s.SetContentsOfCell("A8", (string)null);
        }

        /// <summary>
        /// Tests the vailidator for a formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaException()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("A2", "=" + new Formula("Z"));
        }

        /// <summary>
        /// // Getting value of cell
        /// </summary>
        [TestMethod()]
        public void TestGettingValue()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("A2", 6.ToString());
            sheet.SetContentsOfCell("A3", "=" + new Formula("B3"));
            Assert.AreEqual(6.0, sheet.GetCellValue("A2"));       
        }

        /// <summary>
        /// // Getting value of cell with formula
        /// </summary>
        [TestMethod()]
        public void TestGettingValue2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("A2", 6.ToString());
            sheet.SetContentsOfCell("A3", "=" + new Formula("A2 * 6"));
            Assert.AreEqual(36.0, sheet.GetCellValue("A3"));
        }

        /// <summary>
        /// // Getting value of cell with string
        /// </summary>
        [TestMethod()]
        public void TestGettingValue3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("A2", "name");
            Assert.AreEqual("name", sheet.GetCellValue("A2"));
        }

        /// <summary>
        /// Test get cell value with null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGettingValu4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("B5", 6.ToString());
            sheet.GetCellValue(null);
        }

        /// <summary>
        /// Test get cell value with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGettingValu5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("B5", 6.ToString());
            sheet.GetCellValue("Z");
        }

        /// <summary>
        /// // Getting value of cell with formula that is not defined
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaError))]
        public void TestGettingValue6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(regex);
            sheet.SetContentsOfCell("A3", "=" + new Formula("A4 * 6"));
            sheet.GetCellValue("A3");
        }

        /// <summary>
        /// // Testing a spreadsheet with out regex
        /// </summary>
        [TestMethod()]
        public void TestSpreedWithoutRegex()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("Z", 3.ToString());
            sheet.SetContentsOfCell("hello", 3.ToString());
            sheet.SetContentsOfCell("?", 3.ToString());
            sheet.SetContentsOfCell("7", 3.ToString());
            sheet.SetContentsOfCell("5", 3.ToString());
            sheet.SetContentsOfCell("    ", 3.ToString());
            sheet.SetContentsOfCell("1dsa6f 6a4d d", 3.ToString());
        }
    }
}
