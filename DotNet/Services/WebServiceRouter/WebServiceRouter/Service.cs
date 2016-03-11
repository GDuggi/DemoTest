using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Threading;

namespace WebServiceRouter
{
    partial class Service : ServiceBase
    {
        Router wsr = null;
        Thread workerThread = null;
        static readonly int stopTimeout = 10000;

        public Service()
        {
            try
            {
                InitializeComponent();
                wsr = new Router();
                workerThread = new Thread(wsr.Start);
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
                workerThread.Start();
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
                wsr.Stop();
                workerThread.Join();
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
