using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using VaultUploader.Data.DbAccess;
using VaultUploader.WSAccess;

namespace VaultUploader.Service
{
    class Program
    {
        public static void Main(String[] args)
        {
            InitializeCommon();
            if (args.Count() == 0)
            {
                Log.Info("Starting service");
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new Service() };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                Log.Info("Starting");
                VaultScanner _vaultScanner = new VaultScanner();
                _vaultScanner.StartScan();
            }
        }

        private static void InitializeCommon()
        {
           // log4net.Config.XmlConfigurator.Configure();//configured in AssemblyInfo.

            string connectionString = System.Configuration.ConfigurationManager.AppSettings["DBConnectionString"];
            Log.Info("Initializing DbContext with connection string :" + connectionString);
            DbContext.Instance(connectionString);

            string serviceURL = System.Configuration.ConfigurationManager.AppSettings["RouterServiceURL"];
            Log.Info("Initializing ServiceContext with RouterServiceURL :" + serviceURL);
            ServiceContext.Instance(serviceURL);
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(Program)); }
        }

    }
}
