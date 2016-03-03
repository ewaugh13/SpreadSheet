using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSGui;

namespace SpreadsheetGUI
{
    public interface ISpreadSheetView
    {
        event Action<string> FileChosenEvent;

        event Action CloseEvent;

        event Action NewEvent;

        event Action<SpreadsheetPanel, string> ChangeContents;

        event Action<SpreadsheetPanel> ChangeSelection;

        event Action<SpreadsheetPanel> loadSpreadSheet;

        event Action<string> saveAsEvent;

        string Title { set; }

        string Message { set; }

        string NameOfCell { set; }

        string ContentsOfCell { set; }

        string ValueOfCell { set; }

        void DoClose();

        void OpenNew();
    }
}
