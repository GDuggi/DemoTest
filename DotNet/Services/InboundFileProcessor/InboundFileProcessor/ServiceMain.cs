using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace InboundFileProcessor
{
    public partial class ServiceMain : ServiceBase
    {
        private Logging _logFile;
        private int _timerIntervalMilliseconds;
        private DirectoryProcessor _dirProcessor;

        public ServiceMain()
        {
            InitializeComponent();
            _logFile = new Logging();
            _logFile.logFileName = "EventLog";
            _logFile.WriteToLog("EventLog has been created.");

            string minuteDisplay;
            int timerIntervalMinutes = Properties.Settings.Default.TimerIntervalMinutes;
            minuteDisplay = timerIntervalMinutes == 1 ? " minute." : " minutes.";

            _timerIntervalMilliseconds = 1000 * 60 * timerIntervalMinutes;
            _logFile.WriteToLog("Timer interval set to " + timerIntervalMinutes.ToString() + minuteDisplay);

            System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
            sc = Properties.Settings.Default.ScanDirectories;
            string[] scanDir = new string[sc.Count];

            //Copies all rows from string collection
            sc.CopyTo(scanDir, 0);

            string dirDisplay;
            dirDisplay = sc.Count == 1 ? "directory" : "directories";
            _logFile.WriteToLog("The following source " + dirDisplay + " will be scanned:");
            foreach (string dirName in scanDir)
            {
                _logFile.WriteToLog("--> " + dirName);
            }

            //jvc
            string failedDir = Properties.Settings.Default.FailedDirectory;
            _logFile.WriteToLog("FailedDirectory=" + failedDir);

            string outputDir = Properties.Settings.Default.OutputDirectory;
            string processedDir = Properties.Settings.Default.ProcessedDirectory;
            string validFileExt = Properties.Settings.Default.ValidFileExt;
            bool isDebugLogEnabled = Properties.Settings.Default.DebugLogEnabled;

            //_logFile.WriteToLog("OutputDirectory=" + outputDir);
            _logFile.WriteToLog("ProcessedDirectory=" + processedDir);
            _logFile.WriteToLog("ValidFileExt=" + validFileExt);
            _logFile.WriteToLog("DebugLogEnabled=" + isDebugLogEnabled);

            _logFile.WriteToLog("Creating DirectoryPoller...");
            _dirProcessor = new DirectoryProcessor(scanDir, validFileExt, outputDir, processedDir, failedDir, _logFile);
            _dirProcessor._isDebugLogEnabled = isDebugLogEnabled;
            _dirProcessor._EventLog = _logFile;
            _logFile.WriteToLog("DirectoryPoller was successfully created.");

            this.timerMain.Interval = _timerIntervalMilliseconds;
            //this.timerMain.Elapsed += new System.Timers.ElapsedEventHandler(timerMain_Elapsed);

            // If the timer is declared in a long-running method, use
            // KeepAlive to prevent garbage collection from occurring
            // before the method ends.
            GC.KeepAlive(timerMain);
        }

        protected override void OnStart(string[] args)
        {
            _logFile.WriteToLog("Service Started successfully");
            this.timerMain.Enabled = true;
            _logFile.WriteToLog("Timer has been started.");
        }

        protected override void OnStop()
        {
            this.timerMain.Enabled = false;
            _logFile.WriteToLog("Service Stopped successfully");
        }

        private void timerMain_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerMain.Enabled = false; 
                _dirProcessor.ProcessDirectories();
                _logFile.WriteToLog("Timer event executed.");
                timerMain.Enabled = true; 
            }
            catch (Exception ex)
            {
                timerMain.Enabled = true; 
                _logFile.WriteToLog("Exception occured :" + ex.Message.ToString());
                _logFile.WriteToLog("Timer has been stopped and started");
            }
        }
    }
}
