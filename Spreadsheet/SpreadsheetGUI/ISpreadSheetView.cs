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

        event Action<int, int, string> ChangeContents;

        event Action<int, int> ChangeSelection;

        event Action loadSpreadSheet;

        event Action<string> saveAsEvent;

        event Action saveEvent;

        string Title { set; }

        string Message { set; }

        string NameOfCell { set; }

        string ContentsOfCell { set; }

        string ValueOfCell { set; }

        void DoClose();

        void OpenNew();

        void setPanelValue(int col, int row, string value);
    }
}
