using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.UserSkins;
using NSRiskManager.Properties;
using NSRMCommon;
using System.Threading;
using NSRMLogging;
using System.Configuration;


namespace NSRiskManager {

    public static class RiskManagerProgram {
        const string LISTENER_NAME = "myName";
        [STAThread()]
        public static void Main(string[] args) {

            TextWriterTraceListener twtl = null;
            string path,file,name,dir;

            dir = Path.Combine(
                path = Environment.GetEnvironmentVariable("TEMP"),
                name = Assembly.GetEntryAssembly().GetName().Name);
            file = Path.Combine(dir,name + ".log");
            Trace.WriteLine("TRACE-FILE: " + file);
            try {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                Trace.Listeners.Add(twtl = new TextWriterTraceListener(file,LISTENER_NAME));
                Trace.WriteLine(name + " starts: " + DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fffff"));
                Trace.IndentLevel++;

                try {
                    if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                        Thread.CurrentThread.Name = Assembly.GetEntryAssembly().GetName().Name + "-thread";
                } 
                catch (Exception ex)
                {
                    Util.show(MethodBase.GetCurrentMethod(),ex);
                }
                doIt(args);
            } 
            catch (Exception ex) 
            {
                Trace.WriteLine("[" + ex.GetType().FullName + "] " + ex.Message);
            } finally {
                Trace.Flush();
                if (twtl != null) {
                    Trace.IndentLevel--;
                    Trace.WriteLine(name + " ends: " + DateTime.Now.ToString("dd-MMM-yy hh:mm:ss.fffff"));
                    twtl.Flush();
                    Trace.Listeners.Remove(LISTENER_NAME);
                    twtl.Close();
                    twtl = null;
                }
            }
        }

        static void doIt(string[] args) {
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "Office 2010 Blue"; // For Silver Theme use "Office 2007 Silver"
            DevExpress.LookAndFeel.UserLookAndFeel.Default.UseWindowsXPTheme = false;

            ConnectionUtil.setDotNetConnection(ConfigurationSettings.AppSettings["riskMgrConnection"]);
            ConnectionUtil.setJavaConnection(ConfigurationSettings.AppSettings["javaConnection"]);

            //Application.Run(new RiskMgrMenu());
            Application.Run(new RiskManagerWindowManager());
           

        }
    }
}