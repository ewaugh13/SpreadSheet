using System;
using System.Collections.Generic;
using SS;
using System.Windows.Forms;
using System.IO;
using Formulas;

namespace SpreadsheetGUI
{
    public class Controller
    {
        // The window being controlled
        private ISpreadSheetView window;

        // The model being used
        private Spreadsheet sheet;

        private string file;

        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(ISpreadSheetView window, string fileName)
        {
            this.window = window;
            if (fileName != "")
            {
                TextReader file = File.OpenText(fileName);
                sheet = new Spreadsheet(file);
            }
            else
            {
                sheet = new Spreadsheet();
            }
            window.CloseEvent += HandleClose;
            window.NewEvent += HandleNew;
            window.ChangeContents += ContentsChanged;
            window.ChangeSelection += SelectionChanged;
            window.FileChosenEvent += FileSelected;
            window.loadSpreadSheet += SpreadSheetLoad;
            window.saveAsEvent += SaveAs;
            window.saveEvent += Save;
            window.helpEvent += ShowHelp;
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            if (sheet.Changed == true)
            {
                DialogResult result = MessageBox.Show("The Contents have not been saved. Do you want to close the spreadsheet?", "File not saved", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    window.DoClose();
                }
            }
            else
            {
                window.DoClose();
            }
        }

        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {
            window.OpenNew();
        }

        private void ContentsChanged(int col, int row, string value)
        {
            string name = getNameOfCell(col, row);

            try
            {
                foreach (string cell in sheet.SetContentsOfCell(name, value))
                {

                    window.setPanelValue((int)(cell[0]) - 65, int.Parse((cell.Substring(1))) - 1, sheet.GetCellValue(cell).ToString());
                }

                window.ValueOfCell = sheet.GetCellValue(name).ToString();
            }

            catch (CircularException)
            {
                window.Message = "Can't set a cell to this value because it forms a circular expression.";
                string oldContents = sheet.GetCellContents(name).ToString();

                if (oldContents == "System.Object")
                {
                    oldContents = "";
                }

                window.ContentsOfCell = sheet.GetCellContents(name).ToString();
            }
            catch (FormulaFormatException)
            {
                window.Message = "Can't set a cell to this value because it's not a proper formula.";
                string oldContents = sheet.GetCellContents(name).ToString();

                if (oldContents == "System.Object")
                {
                    oldContents = "";
                }

                window.ContentsOfCell = oldContents;
            }
        }

        private void SelectionChanged(int col, int row)
        {
            string name = getNameOfCell(col, row);
            window.NameOfCell = name;
            window.ContentsOfCell = sheet.GetCellContents(name).ToString();
            window.ValueOfCell = sheet.GetCellValue(name).ToString();
        }

        private void SpreadSheetLoad()
        {
            setSpreadsheetCells();
        }

        private void FileSelected(string fileName)
        {
            try
            {
                file = fileName;
                SpreadSheetContext.GetContext().RunNew(fileName);
            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
        }

        private void SaveAs(string filename)
        {
            file = filename;
            using (TextWriter file = File.CreateText(filename))
            {
                sheet.Save(file);
            }
        }

        private void Save()
        {
            string fileName = file;
            using (TextWriter file = File.CreateText(fileName))
            {
                sheet.Save(file);
            }
        }

        private string getNameOfCell(int col, int row)
        {
            return char.ConvertFromUtf32(col + 65) + (row + 1);
        }

        private void setSpreadsheetCells()
        {
            foreach (string cellName in sheet.GetNamesOfAllNonemptyCells())
            {
                window.setPanelValue((int)(cellName[0]) - 65, int.Parse((cellName.Substring(1))) - 1, sheet.GetCellValue(cellName).ToString());
            }
        }

        private void ShowHelp()
        {
            window.Message = "";
        }
    }
}
