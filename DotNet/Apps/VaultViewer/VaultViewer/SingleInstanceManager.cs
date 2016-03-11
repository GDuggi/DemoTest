using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VaultViewer
{
    public class SingleInstanceManager : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {

        private SingleInstanceApplication _application;
        protected System.Collections.ObjectModel.ReadOnlyCollection<string> _commandLine;
        public SingleInstanceManager()
        {
            IsSingleInstance = true;
        }
        protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs eventArgs)
        {

            // First time _application is launched 
            _commandLine = eventArgs.CommandLine;


            _application = new SingleInstanceApplication();
            _application.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            // Subsequent launches 
            //eventArgs.CommandLine.

            if (eventArgs.CommandLine.Count == 2)
            {
                base.OnStartupNextInstance(eventArgs);
                _commandLine = eventArgs.CommandLine;

                string tradingSysCode = _commandLine[0];
                string tokenNum = _commandLine[1];
                _application.Activate(tradingSysCode, tokenNum);
            }
            else
            {
                MessageBox.Show("Vault Viewer arguments are not proper");
            }
        }
    }
}
