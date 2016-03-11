using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonUtils
{
    public class WebUtils
    {
        private const string PROJ_FILE_NAME = "WebUtils";
        public static SettingsReader settingsReaderInbound = new SettingsReader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\ConfirmInbound.dll.config");
        public static SettingsReader settingsReaderOpsManager = new SettingsReader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\ConfirmManager.dll.config");
        public string logonUser = "";
        public JBossVersion currentJBossVersion = 0;
        public string jbossWSUrlConfirmation = "";
        public string jbossWSUrlConfirmUtil = "";
        public string jbossWSUrlInbound = "";
        public string jbossWSUrlTradeConfirm = "";

        public enum JBossVersion
        {
            JBoss423 = 0, JBossEAP6
        };

        public WebUtils()
        {
            initVariables();
        }

        private void initVariables()
        {
            if (settingsReaderOpsManager.GetSettingValue("JBossCurrentVersion") == "423")
                currentJBossVersion = JBossVersion.JBoss423;
            else if (settingsReaderOpsManager.GetSettingValue("JBossCurrentVersion") == "EAP6")
                currentJBossVersion = JBossVersion.JBossEAP6;
            else
                throw new Exception("An invalid JBoss version was entered in the config file." + Environment.NewLine +
                    "The JBossCurrentVersion is set to:" + settingsReaderOpsManager.GetSettingValue("JBossCurrentVersion") + Environment.NewLine +
                    "Error CNF-338 in " + PROJ_FILE_NAME + ".initVariables()");

            switch (currentJBossVersion)
            {
                case JBossVersion.JBoss423:
                    jbossWSUrlConfirmation = @"http://" + settingsReaderOpsManager.GetSettingValue("JBoss423WebServiceLoc") + @"/" +
                        settingsReaderOpsManager.GetSettingValue("JBoss423WSAddrConfirmation");
                    jbossWSUrlConfirmUtil = @"http://" + settingsReaderOpsManager.GetSettingValue("JBoss423WebServiceLoc") + @"/" +
                        settingsReaderOpsManager.GetSettingValue("JBoss423WSAddrConfirmUtil");
                    jbossWSUrlInbound = @"http://" + settingsReaderOpsManager.GetSettingValue("JBoss423WebServiceLoc") + @"/" +
                        settingsReaderOpsManager.GetSettingValue("JBoss423WSAddrInbound");
                    jbossWSUrlTradeConfirm = @"http://" + settingsReaderOpsManager.GetSettingValue("JBoss423WebServiceLoc") + @"/" +
                        settingsReaderOpsManager.GetSettingValue("JBoss423WSAddrTradeConfirm");
                    break;

                case JBossVersion.JBossEAP6:
                    jbossWSUrlConfirmation = @"http://" + settingsReaderOpsManager.GetSettingValue("JBossEAP6WebServiceLoc") + @"/" +
                        settingsReaderOpsManager.GetSettingValue("JBossEAP6WSAddrConfirmation");
                    jbossWSUrlConfirmUtil = @"http://" + settingsReaderOpsManager.GetSettingValue("JBossEAP6WebServiceLoc") + @"/" +
                        settingsReaderOpsManager.GetSettingValue("JBossEAP6WSAddrConfirmUtil");
                    jbossWSUrlInbound = @"http://" + settingsReaderOpsManager.GetSettingValue("JBossEAP6WebServiceLoc") + @"/" +
                        settingsReaderOpsManager.GetSettingValue("JBossEAP6WSAddrInbound");
                    jbossWSUrlTradeConfirm = @"http://" + settingsReaderOpsManager.GetSettingValue("JBossEAP6WebServiceLoc") + @"/" +
                        settingsReaderOpsManager.GetSettingValue("JBossEAP6WSAddrTradeConfirm");
                    break;
            }
        }

    }
}
