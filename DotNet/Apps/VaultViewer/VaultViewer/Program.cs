using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VaultViewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new frmVaultViewer());
        //}

        [STAThread]
        public static void Main(string[] args)
        {
            if (System.Environment.GetCommandLineArgs().Length == 2)
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmVaultViewer());
            }
            else
            {
                SingleInstanceManager manager = new SingleInstanceManager();
                manager.Run(args);
            }
        }
    }
}
