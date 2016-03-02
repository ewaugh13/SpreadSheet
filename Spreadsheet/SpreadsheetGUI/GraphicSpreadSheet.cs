using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;

namespace SpreadsheetGUI
{
    public partial class GraphicSpreadSheet : Form, ISpreadSheetView
    {

        public GraphicSpreadSheet()
        {
            InitializeComponent();
        }

        public string Title
        {
            set { Text = value; }
        }

        public string Message
        {
            set { MessageBox.Show(value); }
        }

        public event Action<string> FileChosenEvent;
        public event Action CloseEvent;
        public event Action NewEvent;
        public event Action<getRowAndCol, setRowAndCol, string> ChangeContents;

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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        public void DoClose()
        {
            Close();
        }

        public void OpenNew()
        {
            throw new NotImplementedException();
        }

        private void Contents_KeyPress(object sender, KeyPressEventArgs e)

        {
            if (ChangeContents != null)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    ChangeContents(spreadsheetPanel1.GetSelection, spreadsheetPanel1.SetValue, Contents.Text);
                }
            }
        }
    }
}
