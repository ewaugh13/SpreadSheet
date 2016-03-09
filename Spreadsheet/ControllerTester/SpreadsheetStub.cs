using System;
using System.Collections.Generic;
using SpreadsheetGUI;


namespace ControllerTester
{
    class SpreadsheetStub : ISpreadSheetView
    {
        /// <summary>
        /// Shows if do close was called
        /// </summary>
        public bool CalledDoClose
        {
            get; private set;
        }
        /// <summary>
        /// Shows if open was called
        /// </summary>
        public bool CalledOpenNew
        {
            get; private set;
        }
        /// <summary>
        /// Shows if load was called
        /// </summary>
        public bool CalledLoad
        {
            get; private set;
        }
        /// <summary>
        /// Shows if contents were changed
        /// </summary>
        public bool ContentsChanged
        {
            get; private set;
        }
        /// <summary>
        /// Calls close event
        /// </summary>
        public void FireCloseEvent()
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }
        /// <summary>
        /// Calls new event
        /// </summary>
        public void FireNewEvent()
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }
        /// <summary>
        /// Calls load event
        /// </summary>
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
        /// <summary>
        /// Calls change contents event
        /// </summary>
        public void FireChangeContents(int col, int row, string contents)
        {
            if(ChangeContents != null)
            {
                ChangeContents(col, row, contents);
            }
        }
        /// <summary>
        /// Calls change selection event
        /// </summary>
        public void FireChangeSelection(int col, int row)
        {
            if(ChangeSelection != null)
            {
                ChangeSelection(col, row);
            }
        }
        /// <summary>
        /// Calls save as event
        /// </summary>
        public void FireSaveAs(string FileName)
        {
            if(saveAsEvent != null)
            {
                saveAsEvent(FileName);
            }
        }
        /// <summary>
        /// Calls save event
        /// </summary>
        public void FireSave()
        {
            if (saveEvent != null)
            {
                saveEvent();
            }
        }
        /// <summary>
        /// Calls file chosen event
        /// </summary>
        public void FireFileChosen(string FileName)
        {
            if(FileChosenEvent != null)
            {
                FileChosenEvent(FileName);
            }
        }
        /// <summary>
        /// Calls help event
        /// </summary>
        public void FireHelp()
        {
            if(helpEvent != null)
            {
                helpEvent();
            }
        }
        /// <summary>
        /// The contents of the cell
        /// </summary>
        public string ContentsOfCell
        {
            set; get;
        }
        /// <summary>
        /// The current message
        /// </summary>
        public string Message
        {
            set; get;
        }
        /// <summary>
        /// Name in name of cell box
        /// </summary>
        public string NameOfCell
        {
            set; get;
        }
        /// <summary>
        /// Title of window
        /// </summary>
        public string Title
        {
            set; get;
        }
        /// <summary>
        /// The value of the cell
        /// </summary>
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

        /// <summary>
        /// Closes the window
        /// </summary>
        public void DoClose()
        {
            CalledDoClose = true;
        }
        /// <summary>
        /// Opens a new window
        /// </summary>
        public void OpenNew()
        {
            CalledOpenNew = true;
        }
        /// <summary>
        /// Sets the panel values
        /// </summary>
        public void setPanelValue(int col, int row, string value)
        {
            ContentsChanged = true;
        }
    }
}
