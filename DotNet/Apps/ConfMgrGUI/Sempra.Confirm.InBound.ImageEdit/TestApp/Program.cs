using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Sempra.TaskManager.WinApps;
using InboundDocuments;
namespace InboundDocuments
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //  SplashScreen.ShowSplashScreen(Application.ProductVersion, "", "");
                //  SplashScreen.SetStatus("Loading Task Manager...");
                Application.DoEvents();

                //SplashScreen.ShowSplashScreen("1.0", ".Net", "");
                // for (int i = 0; i < 500000; ++i)
                //{
                //}
                // Application.Run(new Sempra.TaskManager.WinApps.SplashScreen());


                // CallerRefList sp = new CallerRefList();
                // sp.SetOracleConnection("Data Source=Sempra.dev;user=srajaman;password=srajaman");
                //  sp.SrcFileName = "C:\\temp\\test.tif";
                /* 
                PageSelection sp = new PageSelection();
                sp.FileName = "C:\\temp\\test.tif";
                 */
                Form1 sp = new Form1();
                //  RefEditForm sp = new RefEditForm(RefEditForm.FaxDataType.SpamFax, RefEditForm.FormMode.Add, "Data Source=Sempra.dev;user=srajaman;password=srajaman", "", "");

                Application.Run(sp);
                //  DocumentEditor page = new DocumentEditor();
                // // page.FileName = @"C:\temp\tif\20061130_115816_Florida Power&Light_6600.tif";
                //    page.FileName = @"C:\temp\DISPUTE_63871720.RTF";
                //   Application.Run(page);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Main: " + ex.Message);
            }
        }
    }
}