using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;
using Dependencies;
using Formulas;
using SSGui;

namespace SpreadsheetGUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Get the application context and run one form inside it
            var context = SpreadSheetContext.GetContext();
            SpreadSheetContext.GetContext().RunNew();
            Application.Run(context);
        }
    }
}
