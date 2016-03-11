using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.Remoting;
using DataManager;
using System.Diagnostics;
using DevExpress.XtraEditors;
using System.Threading;

namespace ConfirmManager
{
   static class Program
   {
      public static frmSplash splashForm;         

      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main(string[] args)
      {
// Israel 10/2/2013 - Unimplemented single instance enforcement on behalf of BTGP
//         if (isAppRunningAlready())
//         {
//            XtraMessageBox.Show("OpsManager is already running.");
//            Application.Exit();
//         }
//         else
//         {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Skin registration.
            DevExpress.UserSkins.OfficeSkins.Register();
            DevExpress.UserSkins.BonusSkins.Register();
            //Unhandled exception handler
            AppDomain.CurrentDomain.UnhandledException +=
               new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //Mike 1/21/09 - Supports remoting service
            //Israel 6/9/14 - Remove support for remoting service.
            //Type dataManagerType = typeof(DAOManager);
            //RemotingConfiguration.RegisterWellKnownClientType(dataManagerType, Properties.Settings.Default.GSRemotingURL);

            //Israel 12/21/2015 -- Causes all invocations of the XtraMessageBox to automatically wrap to an easily-readable size.
            XtraMessageBox.SmartTextWrap = true;

            splashForm = new frmSplash();
            splashForm.Show();
            splashForm.ShowLoadProgress("Loading application libraries...");
            Thread.Sleep(500);

            Application.Run(new frmMain(args));
//         }
      }

      private static bool isAppRunningAlready()
      {
         bool isRunning = false;
         Process process = Process.GetCurrentProcess();
         int id = process.Id;
         String processName = process.ProcessName;
         foreach (Process p in Process.GetProcesses())
         {
            if (p.ProcessName == processName && p.Id != id)
            {
               isRunning = true;
               break;
            }
         }
         return isRunning;
      }

      static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
      {
         try
         {
            Exception ex = (Exception)e.ExceptionObject;
            XtraMessageBox.Show("Unhandled Exception: " + Environment.NewLine
                  + "Please contact the Support staff with the following information:" + Environment.NewLine
                  + ex.Message + Environment.NewLine + ex.StackTrace, "Fatal Error",
                  MessageBoxButtons.OK, MessageBoxIcon.Stop);
         }
#pragma warning disable 0168
         //Disable warning...
         catch (Exception excep) { }
#pragma warning restore 0168
         finally
         {
            Environment.ExitCode = -5;
            Application.Exit();
         }
      }
   }
}