using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;

namespace ControllerTester
{
    [TestClass]
    public class ControllerTester
    {
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
        }

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

        [TestMethod]
        public void TestSaveAs()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "");
            stub.FireChangeContents(0, 0, "5");
            stub.FireSaveAs("../../sheet.ss");
        }

        [TestMethod]
        public void OpenFile()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller2 = new Controller(stub, "");
            stub.FireLoadEvent();
            stub.FireFileChosen("../../sheet2.ss");
            Assert.AreEqual("5", stub.ValueOfCell);
        }
    }
}
