using System;
using System.Collections.Generic;
using SpreadsheetGUI;


namespace ControllerTester
{
    class SpreadsheetStub : ISpreadSheetView
    {
        public bool CalledDoClose
        {
            get; private set;
        }

        public bool CalledOpenNew
        {
            get; private set;
        }

        public bool CalledLoad
        {
            get; private set;
        }

        public bool ContentsChanged
        {
            get; private set;
        }

        public void FireCloseEvent()
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        public void FireNewEvent()
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }

        public void FireLoadEvent()
        {
            if (ChangeSelection != null)
            {
                ChangeSelection(0, 0);
            }

            if (loadSpreadSheet != null)
            {
                loadSpreadSheet();
            }
        }

        public void FireChangeContents(int col, int row, string contents)
        {
            if(ChangeContents != null)
            {
                ChangeContents(col, row, contents);
            }
        }

        public void FireChangeSelection(int col, int row)
        {
            if(ChangeSelection != null)
            {
                ChangeSelection(col, row);
            }
        }

        public void FireSaveAs(string FileName)
        {
            if(saveAsEvent != null)
            {
                saveAsEvent(FileName);
            }
        }

        public void FireSave()
        {
            if (saveEvent != null)
            {
                saveEvent();
            }
        }

        public void FireFileChosen(string FileName)
        {
            if(FileChosenEvent != null)
            {
                FileChosenEvent(FileName);
            }
        }

        public string ContentsOfCell
        {
            set; get;
        }

        public string Message
        {
            set; get;
        }

        public string NameOfCell
        {
            set; get;
        }

        public string Title
        {
            set; get;
        }

        public string ValueOfCell
        {
            set; get;
        }

        public event Action<int, int, string> ChangeContents;
        public event Action<int, int> ChangeSelection;
        public event Action CloseEvent;
        public event Action<string> FileChosenEvent;
        public event Action loadSpreadSheet;
        public event Action NewEvent;
        public event Action<string> saveAsEvent;
        public event Action saveEvent;
        public event Action helpEvent;

        public void DoClose()
        {
            CalledDoClose = true;
        }

        public void OpenNew()
        {
            CalledOpenNew = true;
        }

        public void setPanelValue(int col, int row, string value)
        {
            ContentsChanged = true;
        }
    }
}
