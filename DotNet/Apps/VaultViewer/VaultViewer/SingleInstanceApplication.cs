using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
namespace VaultViewer
{
    class SingleInstanceApplication : System.Windows.Application
    {
        public static frmVaultViewer fm;

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            // Call the OnStartup event on our base class 
            base.OnStartup(e);

            // Create our MainWindow and show it 
            //fm = new TabForm(System.Environment.GetCommandLineArgs()[1], System.Environment.GetCommandLineArgs()[2]);
            // System.Windows.Forms.Application.EnableVisualStyles();
            // System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            if (e.Args == null || e.Args.Count() == 0)
            {
                fm = new frmVaultViewer();
            }
            else
            {
                fm = new frmVaultViewer(e.Args[0], e.Args[1]);

            }
            fm.Show();
        }

        public void Activate(string tradingSysCode, string tokenNum)
        {
            // Reactivate the main window 
            if (fm != null)
            {
                fm.LoadVaultViewer(tradingSysCode, tokenNum);
                fm.Activate();
            }
            else
            {
                fm = new frmVaultViewer(tradingSysCode, tokenNum);
                fm.Show();
            }
            //fm.AddTabPage(xmlPath, infoPath);
            //fm.AddTabPage(xmlPath, infoPath);
        } 
    }
}
