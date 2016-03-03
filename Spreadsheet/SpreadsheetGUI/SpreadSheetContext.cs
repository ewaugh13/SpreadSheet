using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SSGui;

namespace SpreadsheetGUI
{
    class SpreadSheetContext : ApplicationContext
    {
        // Number of open forms
        private int windowCount = 0;

        // Singleton ApplicationContext
        private static SpreadSheetContext context;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadSheetContext()
        {

        }

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static SpreadSheetContext GetContext()
        {
            if (context == null)
            {
                context = new SpreadSheetContext();
            }
            return context;
        }

        /// <summary>
        /// Runs a form in this application context
        /// </summary>
        public void RunNew()
        {
            // Create the window and the controller
            GraphicSpreadSheet window = new GraphicSpreadSheet();
            new Controller(window);

            // One more form is running
            windowCount++;

            // When this form closes, we want to find out
            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };

            // Run the form
            window.Show();
        }

        public void RunNew(string fileName)
        {
            // Create the window and the controller
            GraphicSpreadSheet window = new GraphicSpreadSheet();
            new Controller(window, fileName);

            // One more form is running
            windowCount++;

            // When this form closes, we want to find out
            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };

            // Run the form
            window.Show();          
        }
    }
}
