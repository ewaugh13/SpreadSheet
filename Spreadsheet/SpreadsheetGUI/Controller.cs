using System;
using System.Collections.Generic;
using SS;
using System.Windows.Forms;
using SSGui;
using System.IO;
using Formulas;

namespace SpreadsheetGUI
{
    class Controller
    {
        // The window being controlled
        private ISpreadSheetView window;

        // The model being used
        private Spreadsheet sheet;

        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(ISpreadSheetView window)
        {
            this.window = window;
            this.sheet = new Spreadsheet();
            window.CloseEvent += HandleClose;
            window.NewEvent += HandleNew;
            window.ChangeContents += ContentsChanged;
            window.ChangeSelection += SelectionChanged;
            window.FileChosenEvent += FileSelected;
            window.saveAsEvent += SaveAs;
        }

        public Controller(ISpreadSheetView window, string fileName)
        {
            this.window = window;
            TextReader file = File.OpenText(fileName);
            sheet = new Spreadsheet(file);

            window.Title = "hello";

            window.CloseEvent += HandleClose;
            window.NewEvent += HandleNew;
            window.ChangeContents += ContentsChanged;
            window.ChangeSelection += SelectionChanged;
            window.FileChosenEvent += FileSelected;
            window.loadSpreadSheet += SpreadSheetLoad;
            window.saveAsEvent += SaveAs;
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            if(sheet.Changed == true)
            {
                MessageBox.Show("The Contents have not been saved. Do you want to save?", "File not saved");
            }
            window.DoClose();
        }

        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {
            window.OpenNew();
        }

        private void ContentsChanged(SpreadsheetPanel sender, string value)
        {
            int col;
            int row;
            sender.GetSelection(out col, out row);

            string name = getNameOfCell(col, row);

            try
            {
                foreach (string cell in sheet.SetContentsOfCell(name, value))
                {
                    sender.SetValue((int)(cell[0]) - 65, int.Parse((cell.Substring(1))) - 1, sheet.GetCellValue(cell).ToString());
                }

                window.ValueOfCell = sheet.GetCellValue(name).ToString();
            }

            catch (CircularException)
            {
                window.Message = "Can't set a cell to this value because it forms a circular expression.";
                window.ContentsOfCell = sheet.GetCellContents(name).ToString();
            }
            catch (FormulaFormatException)
            {               
                window.Message = "Can't set a cell to this value because it's not a proper formula.";
                window.ContentsOfCell = sheet.GetCellContents(name).ToString();
            }
        }

        private void SelectionChanged(SpreadsheetPanel sender)
        {
            int col;
            int row;
            sender.GetSelection(out col, out row);

            string name = getNameOfCell(col, row);
            window.NameOfCell = name;
            window.ContentsOfCell = sheet.GetCellContents(name).ToString();
            window.ValueOfCell = sheet.GetCellValue(name).ToString();          
        }

        private void SpreadSheetLoad(SpreadsheetPanel sender)
        {
            setSpreadsheetCells(sender);
        }

        private void FileSelected(string fileName)
        {
            SpreadSheetContext.GetContext().RunNew(fileName);
        }

        private void SaveAs(string filname)
        {
            using (TextWriter file = File.CreateText(filname))
            {
                sheet.Save(file);
            }
        }

        private string getNameOfCell(int col, int row)
        {
            return char.ConvertFromUtf32(col + 65) + (row + 1);
        }

        private void setSpreadsheetCells(SpreadsheetPanel sender)
        {
            foreach (string cellName in sheet.GetNamesOfAllNonemptyCells())
            {
                sender.SetValue((int)(cellName[0]) - 65, int.Parse((cellName.Substring(1))) - 1, sheet.GetCellValue(cellName).ToString());
            }
        }
    }
}
