using System;

using System.Windows.Forms;
using SSGui;
using SpreadsheetGUI;

namespace SpreadsheetGUI
{
    public partial class GraphicSpreadSheet : Form, ISpreadSheetView
    {
        /// <summary>
        /// Is the contstructor for the graphic spreadsheet
        /// </summary>
        public GraphicSpreadSheet()
        {
            InitializeComponent();
            spreadsheetPanel1.SelectionChanged += selection;
        }

        /// <summary>
        /// The title shown at the top
        /// </summary>
        public string Title
        {
            set { Text = value; }
        }

        /// <summary>
        /// The string to determine the message
        /// </summary>
        public string Message
        {
            set { MessageBox.Show(value); }
        }

        /// <summary>
        /// The name of the current selected cell
        /// </summary>
        public string NameOfCell
        {
            set { CellName.Text = value; }
        }

        /// <summary>
        /// The value of the current selected cell
        /// </summary>
        public string ValueOfCell
        {
            set { Value.Text = value; }
        }

        /// <summary>
        /// The contents of the currently selected cell
        /// </summary>
        public string ContentsOfCell
        {
            set { Contents.Text = value; }
        }

        public event Action<string> FileChosenEvent;
        public event Action CloseEvent;
        public event Action NewEvent;
        public event Action<int, int, string> ChangeContents;
        public event Action<int, int> ChangeSelection;
        public event Action loadSpreadSheet;
        public event Action<string> saveAsEvent;
        public event Action saveEvent;
        public event Action helpEvent;

        /// <summary>
        /// Handles the Click event of the closeItem control.
        /// </summary>
        private void CloseItem_Click(object sender, EventArgs e)
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        /// <summary>
        /// Handles closing the window
        /// </summary>
        public void DoClose()
        {
            Close();
        }

        /// <summary>
        /// Handles opening a new window
        /// </summary>
        public void OpenNew()
        {
            SpreadSheetContext.GetContext().RunNew("");
        }

        /// <summary>
        /// If enter is pressed in contents it will update the cell
        /// </summary>
        private void Contents_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ChangeContents != null)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    int col;
                    int row;

                    spreadsheetPanel1.GetSelection(out col, out row);
                    ChangeContents(col, row, Contents.Text);
                }
            }
        }

        /// <summary>
        /// loads the GUI
        /// </summary>
        private void SpreadsheetGUI_load(object sender, EventArgs e)
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
        /// Handles cells being selected in the spreadsheet panel
        /// </summary>
        private void selection(SpreadsheetPanel panel)
        {
            if (ChangeSelection != null)
            {
                int col;
                int row;

                spreadsheetPanel1.GetSelection(out col, out row);
                ChangeSelection(col, row);
            }
        }

        /// <summary>
        /// Handles opening a new window
        /// </summary>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }

        /// <summary>
        /// Handles closing a window
        /// </summary>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        /// <summary>
        /// Handles opening a file to load
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    FileChosenEvent(openFileDialog1.FileName);
                }
            }
        }

        /// <summary>
        /// Handles doing a save as
        /// </summary>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (saveAsEvent != null)
                {
                    saveAsEvent(saveFileDialog1.FileName);
                }
            }
        }

        /// <summary>
        /// Handles saving the current file
        /// </summary>
        private void saveToolStripMenuItem_Save(object sender, EventArgs e)
        {
            if(saveEvent != null)
            {
                saveEvent();
            }
        }

        /// <summary>
        /// Opens the help menu
        /// </summary>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(helpEvent != null)
            {
                helpEvent();
            }
        }

        /// <summary>
        /// Sets the panel values
        /// </summary>
        public void setPanelValue(int col, int row, string value)
        {
            spreadsheetPanel1.SetValue(col, row, value);
        }
    }
}
