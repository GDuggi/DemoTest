using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace WebServicePrototypeService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(String[] args)
        {
            if (args.Count() == 0)
            {
                Log.Info("Starting service");
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new Service() };
                ServiceBase.Run(ServicesToRun);
            } 
            else
            {
                // I couldn't get this to work, so to debug:
                // To start all the WebServices in Solution Explorer:
                //    Solution > Properties > Startup > Multiple Startup projects
                Log.Info("Starting");
                Service.debug(args);
                Console.WriteLine("Hit Enter to exit");
                Console.ReadLine();
            }
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(Program)); }
        }
    }
}
