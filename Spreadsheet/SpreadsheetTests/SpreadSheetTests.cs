﻿using System;
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
        [TestMethod]
        public void TestSetCell()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            //sheet.SetCellContents("a17", 5);
            sheet.SetCellContents("B5", 6);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a07", 5);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("Z", 5);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCell4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("hello", 5);
        }

        [TestMethod]
        public void TestSetCell5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", 6);
            sheet.SetCellContents("C9", "B5");
            Formula form = new Formula("8 / 2");
            sheet.SetCellContents("D7", form);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetTextNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();            
            sheet.SetCellContents("Z", null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetFormNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form = new Formula();
            sheet.SetCellContents("Z", form);
        }

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

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetContents2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", 6);
            sheet.GetCellContents("B05");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetContents3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("B5", 6);
            sheet.GetCellContents(null);
        }

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
            Assert.AreEqual(2, dependents.Count);
        }

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

        [TestMethod]
        public void TestSetReplacing()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("C1", "B5");
            sheet.SetCellContents("c1", "A5");
            sheet.SetCellContents("D7", 7);
            sheet.SetCellContents("d7", 8);
        }
    }
}
