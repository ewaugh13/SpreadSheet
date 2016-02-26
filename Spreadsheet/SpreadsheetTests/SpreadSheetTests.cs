using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using Formulas;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;



namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadSheetTests
    {
        /// <summary>
        /// Tests adding 2 elements with doubles
        /// </summary>
        [TestMethod]
        public void TestSetCell1()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A2", 3.ToString());
        }

        /// <summary>
        /// Tests invalid name type that throws invalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a07", 5.ToString());
        }

        /// <summary>
        /// Tests invalid name type
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("Z", 5.ToString());
        }

        /// <summary>
        /// Test invalid name type and throws invalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("hello", 5.ToString());
        }

        /// <summary>
        /// Tests inserting formula
        /// </summary>
        [TestMethod]
        public void TestSetCell5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("Z", null);
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
            sheet.SetContentsOfCell("Z", "=" + form.ToString());
        }

        /// <summary>
        /// Tests the amount of GetNamesOfAllNonEmptyCells which is 3
        /// </summary>
        [TestMethod]
        public void TestGetNames()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "A5");
        }

        /// <summary>
        /// Tests set using number and text
        /// </summary>
        [TestMethod]
        public void TestSetUsingNumberandText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A8", (string)null);
        }

        /// <summary>
        /// Tests the vailidator for a formula format exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaException()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A2", "=" + new Formula("Z"));
        }

        /// <summary>
        /// // Getting value of cell
        /// </summary>
        [TestMethod()]
        public void TestGettingValue()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
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
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B5", 6.ToString());
            sheet.GetCellValue("Z");
        }

        /// <summary>
        /// Test get cell value with formula that isn't defined
        /// </summary>
        [TestMethod]
        public void TestGettingValu6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B5", 6.ToString());
            sheet.SetContentsOfCell("A5", "=" + new Formula("B1 * 2"));
            Assert.AreEqual(sheet.GetCellValue("A5"), new FormulaError("The formula contents and not defined in the spreadsheet"));
        }

        /// <summary>
        /// Test get cell value with invalid name
        /// </summary>
        [TestMethod]
        public void TestGettingValue7()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B5", "");
            sheet.SetContentsOfCell("A5", "=" + new Formula("B5 * 2"));
            Assert.AreEqual(sheet.GetCellValue("A5"), new FormulaError("The formula contents and not defined in the spreadsheet"));
        }

        /// <summary>
        /// // Testing a spreadsheet with out regex
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSpreedWithoutRegex()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("Z", 3.ToString());
        }

        /// <summary>
        /// // Tests for circular exception and that the value was reset
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void Test16()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            try
            {
                s.SetContentsOfCell("A1", "=" + new Formula("A2+A3").ToString());
                s.SetContentsOfCell("A2", 15.ToString());
                s.SetContentsOfCell("A3", 30.ToString());
                s.SetContentsOfCell("A2", "=" + new Formula("A3*A1").ToString());
            }
            catch (CircularException e)
            {
                Assert.AreEqual(15, (double)s.GetCellContents("A2"), 1e-9);
                throw e;
            }
        }

        /// <summary>
        /// Tests getting cell value
        /// </summary>
        [TestMethod()]
        public void TestCellValue()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A4", "=" + new Formula("5").ToString());
            sheet.SetContentsOfCell("A3", "=" + new Formula("A4*2").ToString());
            sheet.SetContentsOfCell("A2", "=" + new Formula("A3*2").ToString());
            Assert.AreEqual(20.0, sheet.GetCellValue("A2"));
            sheet.SetContentsOfCell("A4", "=" + new Formula("6").ToString());
            Assert.AreEqual(24.0, sheet.GetCellValue("A2"));
        }

        /// <summary>
        /// Tests getting cell value
        /// </summary>
        [TestMethod()]
        public void TestCellValue2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A3", "=" + new Formula("A4*2").ToString());
            sheet.SetContentsOfCell("A2", "=" + new Formula("A3*2").ToString());
            Assert.AreEqual(new FormulaError().GetType(), sheet.GetCellValue("A2").GetType());
        }

        /// <summary>
        /// Tests with extra regex
        /// </summary>
        [TestMethod()]
        public void TestSetWithExtraRegex()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(new Regex(@"^[A-M]+[1-9]\d*$"));
            sheet.SetContentsOfCell("M2", 5.ToString());
        }

        /// <summary>
        /// Tests with extra regex
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetWithExtraRegex2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(new Regex(@"^[A-M]+[1-9]\d*$"));
            sheet.SetContentsOfCell("N2", 5.ToString());
        }

        /// <summary>
        /// Tests xml save
        /// </summary>
        [TestMethod()]
        public void TestXmlSave()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("A2", 5.ToString());
            s.SetContentsOfCell("A3", "Hello");
            s.SetContentsOfCell("A4", "=A2 * 2");

            using (TextWriter test = File.CreateText("../../text.xml"))
            {
                s.Save(test);
            }
        }

        /// <summary>
        /// Tests xml save
        /// </summary>
        [TestMethod()]
        public void TestXmlSave2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A3", "Hello");
            s.SetContentsOfCell("A4", "=A2 * 2");

            using (TextWriter test = File.CreateText("../../text2.xml"))
            {
                s.Save(test);
            }
        }

        /// <summary>
        /// Tests xml save and that the value is changed
        /// </summary>
        [TestMethod()]
        public void TestXml()
        {
            AbstractSpreadsheet sheet;
            using (TextReader test = File.OpenText("../../text.xml"))
            {
                sheet = new Spreadsheet(test);
            }

            List<string> names = new List<string>();

            foreach (string cell in sheet.GetNamesOfAllNonemptyCells())
            {
                names.Add(cell);
            }

            Assert.AreEqual(true, sheet.Changed);

        }

        /// <summary>
        /// Tests creating spreadsheet of source
        /// </summary>
        [TestMethod()]
        public void TestXml2()
        {
            AbstractSpreadsheet sheet;
            using (TextReader test = File.OpenText("../../text2.xml"))
            {
                sheet = new Spreadsheet(test);
            }
        }

        /// <summary>
        /// Tests for null source
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestXml3()
        {
            AbstractSpreadsheet sheet;
            using (TextReader test = null)
            {
                sheet = new Spreadsheet(test);
            }
        }

        /// <summary>
        /// Tests for spreadsheetReadException
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestXml4()
        {
            AbstractSpreadsheet sheet;
            using (TextReader test = File.OpenText("../../text3.xml"))
            {
                sheet = new Spreadsheet(test);
            }
        }

        /// <summary>
        /// Tests for spreadsheetReadException
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestXml5()
        {
            AbstractSpreadsheet sheet;
            using (TextReader test = File.OpenText("../../text4.xml"))
            {
                sheet = new Spreadsheet(test);
            }
        }

        /// <summary>
        /// Tests for spreadsheetReadException
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestXml6()
        {
            AbstractSpreadsheet sheet;
            using (TextReader test = File.OpenText("../../text5.xml"))
            {
                sheet = new Spreadsheet(test);
            }
        }

        /// <summary>
        /// Tests for spreadsheetReadException
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void TestXml7()
        {
            AbstractSpreadsheet sheet;
            using (TextReader test = File.OpenText("../../text6.xml"))
            {
                sheet = new Spreadsheet(test);
            }
        }
    }
}
