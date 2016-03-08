using System;

using System.Windows.Forms;
using SSGui;
using SpreadsheetGUI;

namespace SpreadsheetGUI
{
    public partial class GraphicSpreadSheet : Form, ISpreadSheetView
    {

        public GraphicSpreadSheet()
        {
            InitializeComponent();
            spreadsheetPanel1.SelectionChanged += selection;
        }

        public string Title
        {
            set { Text = value; }
        }

        public string Message
        {
            set { MessageBox.Show(value); }
        }
        
        public string NameOfCell
        {
            set { CellName.Text = value; }
        }

        public string ValueOfCell
        {
            set { Value.Text = value; }
        }

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

        public void DoClose()
        {
            Close();
        }

        public void OpenNew()
        {
            SpreadSheetContext.GetContext().RunNew("");
        }

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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

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

        private void saveToolStripMenuItem_Save(object sender, EventArgs e)
        {
            if(saveEvent != null)
            {
                saveEvent();
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(helpEvent != null)
            {
                helpEvent();
            }
        }

        public void setPanelValue(int col, int row, string value)
        {
            spreadsheetPanel1.SetValue(col, row, value);
        }
    }
}
