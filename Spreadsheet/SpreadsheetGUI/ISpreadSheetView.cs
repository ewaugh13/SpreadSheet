using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public interface ISpreadSheetView
    {
        event Action<string> FileChosenEvent;

        event Action CloseEvent;

        event Action NewEvent;

        event Action<getRowAndCol, setRowAndCol, string> ChangeContents;

        string Title { set; }

        string Message { set; }

        void DoClose();

        void OpenNew();
    }
}
