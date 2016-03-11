using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.ServiceProcess;

/*
 * A program to launch the Router 
 * When debugging add any agument and it will launch as a ruugular console app
 */

namespace WebServiceRouter
{
    class Program
    {
        public static void Main(String[] args)
        {            
            if (args.Count() == 0) {
                Log.Info("Starting service");
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new Service() };
                ServiceBase.Run(ServicesToRun);  
            }
            else {
                Log.Info("Starting");
                Router wsr = new Router();
                wsr.Start();
            } 
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(Program)); }
        }

    }
}
