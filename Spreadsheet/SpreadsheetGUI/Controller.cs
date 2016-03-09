using System;
using System.Collections.Generic;
using SS;
using System.Windows.Forms;
using System.IO;
using Formulas;
using System.Text.RegularExpressions;

namespace SpreadsheetGUI
{
    public class Controller
    {
        // The window being controlled
        private ISpreadSheetView window;

        // The model being used
        private Spreadsheet sheet;

        // The string to access the filename
        private string file = "";

        /// <summary>
        /// Begins controlling window. Will create the spreadsheet and add the methods from the controller to the events
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

        /// <summary>
        /// Handles the contents of a cell being changed in the GUI and the spreadsheet
        /// </summary>
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
                window.Message = "Can't set a cell to this value because it's not a proper formula or is not in range.";
                string oldContents = sheet.GetCellContents(name).ToString();

                if (oldContents == "System.Object")
                {
                    oldContents = "";
                }

                window.ContentsOfCell = oldContents;
            }
        }

        /// <summary>
        /// Handles selecting another cell
        /// </summary>
        private void SelectionChanged(int col, int row)
        {
            string name = getNameOfCell(col, row);
            window.NameOfCell = name;
            window.ContentsOfCell = sheet.GetCellContents(name).ToString();
            window.ValueOfCell = sheet.GetCellValue(name).ToString();
        }

        /// <summary>
        /// Loads the spreadsheet
        /// </summary>
        private void SpreadSheetLoad()
        {
            setSpreadsheetCells();
        }

        /// <summary>
        /// Handles the selection of the file
        /// </summary>
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

        /// <summary>
        /// Handles saving a spreadsheet to a location as a .ss file
        /// </summary>
        private void SaveAs(string filename)
        {
            file = filename;
            using (TextWriter file = File.CreateText(filename))
            {
                sheet.Save(file);
            }
        }

        /// <summary>
        /// Handles saving to the current filename
        /// </summary>
        private void Save()
        {
            string fileName = file;
            if (fileName != "")
            {
                using (TextWriter file = File.CreateText(fileName))
                {
                    sheet.Save(file);
                }
            }
            else
            {
                window.Message = "Can't save because there is no file location. Call save as a file location.";
            }
        }

        /// <summary>
        /// Gets the name of the cell based on the col and row
        /// </summary>
        private string getNameOfCell(int col, int row)
        {
            return char.ConvertFromUtf32(col + 65) + (row + 1);
        }

        /// <summary>
        /// Will set the contents of the GUI.
        /// </summary>
        private void setSpreadsheetCells()
        {
            foreach (string cellName in sheet.GetNamesOfAllNonemptyCells())
            {
                window.setPanelValue((int)(cellName[0]) - 65, int.Parse((cellName.Substring(1))) - 1, sheet.GetCellValue(cellName).ToString());
            }
        }

        /// <summary>
        /// Handles showing the help box
        /// </summary>
        private void ShowHelp()
        {
            window.Message = "This is a spreadsheet that acts similar to excel. You have cells that you can click on and it will display thier name, contents, and value in" +
            " the top bar. To change the contents of the curently clicked cell you put the value into the contents (either a number, formula, or a word/string) and then press enter to update it." + 
            " Clicking on file will drop down letting you select from multiple options. Clicking on new will open up a new spreadsheet, clicking on open will allow you to open"+
            " a saved a saed spreadsheet file. Clicking on save as will let you save the the file as a .ss file. Clicking on save will save the current file as long as it has a file location" +
            " Clicking on close will close the program. Clicking on help will show this message box. Clicking on the red X will also close the program";
        }
    }
}
