using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApacheNMS_Test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmTestSimple());
            //Application.Run(new frmTestMenu());
            Application.Run(new frmTestHornetQ());
            //Application.Run(new frmTestOpsMgrAPI());
        }
    }
}
