using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Sample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread] //default behaviour - we must disable it
        [MTAThread] //IMPORTANT: we need to use MTA for main thread because NSOCR COM is created in main thread
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
