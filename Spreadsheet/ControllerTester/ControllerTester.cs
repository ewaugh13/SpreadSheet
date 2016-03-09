using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;
using SS;

namespace ControllerTester
{
    [TestClass]
    public class ControllerTester
    {
        /// <summary>
        /// Opens and closes a window
        /// </summary>
        [TestMethod]
        public void TestOpenAndNew()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "");
            stub.FireCloseEvent();
            stub.FireNewEvent();
            Assert.IsTrue(stub.CalledDoClose);
            Assert.IsTrue(stub.CalledOpenNew);
        }

        /// <summary>
        /// Tests changing contents of the cells
        /// </summary>
        [TestMethod]
        public void TestChangeContents()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "");
            stub.FireChangeContents(0, 0, "Hello");
            Assert.IsTrue(stub.ContentsChanged);
            Assert.AreEqual("Hello", stub.ValueOfCell);
            stub.FireChangeContents(1, 1, "5");
            Assert.AreEqual("5", stub.ValueOfCell);
            stub.FireChangeSelection(0, 0);
            Assert.AreEqual("Hello", stub.ValueOfCell);
            Assert.AreEqual("A1", stub.NameOfCell);
            Assert.AreEqual("Hello", stub.ContentsOfCell);
        }

        /// <summary>
        /// Tests circular exceptions and formula format error
        /// </summary>
        [TestMethod]
        public void TestChangeContentsForCircularandFormulaFormatError()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "");
            stub.FireChangeContents(0, 0, "5");
            stub.FireChangeContents(0, 0, "=A1");
            Assert.AreEqual("5", stub.ValueOfCell);
            stub.FireChangeContents(0, 0, "=?");
            Assert.AreEqual("5", stub.ValueOfCell);
        }

        /// <summary>
        /// Test circular exception and formula format error
        /// </summary>
        [TestMethod]
        public void TestChangeContentsForCircularandFormulaFormatError2()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "");
            stub.FireLoadEvent();
            Assert.AreEqual("A1", stub.NameOfCell);
            stub.FireChangeContents(0, 0, "=A1");
            Assert.AreEqual("", stub.ValueOfCell);
            stub.FireChangeContents(0, 0, "=?");
            Assert.AreEqual("", stub.ValueOfCell);
            Assert.AreEqual("A1", stub.NameOfCell);
            stub.FireChangeSelection(0, 1);
            Assert.AreEqual("A2", stub.NameOfCell);
        }

        /// <summary>
        /// Saves a ss file
        /// </summary>
        [TestMethod]
        public void TestSaveAs()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "");
            stub.FireChangeContents(0, 0, "5");
            stub.FireChangeContents(0, 1, "7");
            stub.FireSaveAs("../../sheet.ss");
        }

        /// <summary>
        /// Opens up a ss file
        /// </summary>
        [TestMethod]
        public void OpenFile()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "../../sheet.ss");
            stub.FireLoadEvent();
            stub.FireChangeSelection(0, 1);
            Assert.AreEqual("7", stub.ValueOfCell);
            stub.FireChangeSelection(0, 0);
            Assert.AreEqual("5", stub.ValueOfCell);
            stub.FireCloseEvent();
        }

        /// <summary>
        /// Opens a saved ss file
        /// </summary>
        [TestMethod]
        public void OpenFile2()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "");
            stub.FireFileChosen("../../sheet.ss");
            stub.FireCloseEvent();
        }

        /// <summary>
        /// Saves a current file
        /// </summary>
        [TestMethod]
        public void SaveFile()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "");
            stub.FireChangeContents(0, 0, "5");
            stub.FireChangeContents(0, 1, "7");
            stub.FireSaveAs("../../sheet2.ss");
            stub.FireChangeContents(0, 0, "10");
            stub.FireSave();
        }

        /// <summary>
        /// Calls the help menu
        /// </summary>
        [TestMethod]
        public void CallHelp()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "");
            stub.FireHelp();
        }

        /// <summary>
        /// Check a file open error
        /// </summary>
        [TestMethod]
        public void FileOpenError()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "");
            stub.FireFileChosen("../sheet2.ss");
            Assert.IsTrue(stub.Message.Contains("Unable to open"));
        }

        /// <summary>
        /// Checks that you can't save a file that doesn't have a location
        /// </summary>
        [TestMethod]
        public void FileSaveMessage()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "");
            stub.FireSave();
            Assert.IsTrue(stub.Message.Contains("Can't save because"));
        }
    }
}
