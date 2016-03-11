using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VaultUploader.Service
{
    partial class Service : ServiceBase
    {
        private VaultScanner _vaultScanner = null;
        private  Thread _workerThread = null;      

        public Service()
        {
            try
            {
                InitializeComponent();
                _vaultScanner = new VaultScanner();
                _workerThread = new Thread(_vaultScanner.StartScan);
            }
            catch (Exception e)
            {
                Log.Fatal("Failed to start service", e);
                throw e;
            }
        }

        protected override void OnStart(String[] args)
        {
            try
            {
                Log.Info("Starting");
                _workerThread.Start();
            }
            catch (Exception e)
            {
                Log.Fatal("Failed to start service", e);
                throw e;
            }
        }

        protected override void OnStop()
        {
            try
            {
                Log.Info("Stopping");
                _vaultScanner.StopScan();
                _workerThread.Join();
                Log.Info("Stopped");
            }
            catch (Exception e)
            {
                Log.Error("Failed to start service", e);
                throw e;
            }
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(Service)); }
        }
    }
}
