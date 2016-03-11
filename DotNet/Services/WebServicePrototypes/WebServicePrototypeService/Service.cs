using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using log4net;

namespace WebServicePrototypeService
{
    public partial class Service : ServiceBase
    {
        static Type[] types = { typeof( Counterparty.Counterparty ),
                              typeof( ConfirmationsManager.ConfirmationsManager) ,
                              typeof( GetDocument.GetDocument ),
                              typeof(VaultService.VaultService)};

        List<ServiceHost> hosts;
        
        public Service()
        {
            InitializeComponent();
            hosts = new List<ServiceHost>();
        }

        public static void debug(String[] args)
        {
            Service s = new Service();
            s.OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            hosts.Clear();

            Log.InfoFormat("Starting {0} services", types.Count());
            try
            {
                foreach (Type t in types)
                {

                    ServiceHost host = new ServiceHost(t);
                    hosts.Add(host);
                    host.Open();

                    foreach (var ea in host.Description.Endpoints)
                    {
                        Log.InfoFormat("\t{0}: {1}", ea.Address, host.Description.Name);
                    }

                }
            }
            catch (Exception e)
            {
                Log.Fatal("Failed to start service", e );
                throw e;
            }
        }

        protected override void OnStop()
        {
            try
            {
                Log.InfoFormat("Stopping services:");
                foreach (ServiceHost h in hosts)
                {
                    Log.InfoFormat("\t{0}", h.Description.Name);
                    h.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error("Failed to stop service", e);
                throw e;
            }
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(Service)); }
        }
    }
}
