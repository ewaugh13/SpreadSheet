using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;

namespace SpreadsheetGUI
{
    public delegate void getRowAndCol(out int col, out int row);

    public delegate bool setRowAndCol(int col, int row, string text);
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
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            window.DoClose();
        }

        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {
            window.OpenNew();
        }

        private void ContentsChanged(getRowAndCol colRowMethod, setRowAndCol colRowSet, string value)
        {
            int col;
            int row;
            colRowMethod(out col, out row);

            string name = getNameOfCell(col, row);
            sheet.SetContentsOfCell(name, value);

            colRowSet(col, row, sheet.GetCellValue(name).ToString());        
        }

        private string getNameOfCell(int col, int row)
        {
            return char.ConvertFromUtf32(col + 65) + (row + 1);
        }
    }
}
