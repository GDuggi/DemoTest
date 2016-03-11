using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CommonUtils;
using log4net;

namespace ConfirmInbound
{
    public class InboundSettings
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (InboundSettings));
        private static readonly SettingsReader settingsReader;        
        private static readonly string userName;
        private const string FORM_NAME = "InboundSettings";

        static InboundSettings()
        {            
            try
            {
                var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                settingsReader = new SettingsReader(directoryName + @"\ConfirmManager.exe.config");                
                userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
            catch (Exception e)
            {
                Logger.Error("Error CNF-529: Error initializing inbound settings:" + e.Message, e);
                //throw;
                throw new Exception("An error occurred while initializing the inbound settings." + Environment.NewLine +
                     "Error CNF-529 in " + FORM_NAME + ".InboundSettings(): " + e.Message);
            }
        }

        public static string ReceiptMethodType { get { return settingsReader.GetSettingValue("ReceiptMethodType"); }}
        public static string ReceiptMethodValue { get { return settingsReader.GetSettingValue("ReceiptMethodValue"); }}
        public static string Recipient { get { return settingsReader.GetSettingValue("Recipient"); }}
        public static string SuccessMethodType { get { return settingsReader.GetSettingValue("SuccessMethodType"); }}
        public static string SuccessMethodValue { get { return settingsReader.GetSettingValue("SuccessMethodValue"); }}
        public static string FailMethodType { get { return settingsReader.GetSettingValue("FailMethodType"); }}
        public static string FailMethodValue { get { return settingsReader.GetSettingValue("FailMethodValue"); }}
        public static string EmailHost { get { return settingsReader.GetSettingValue("EmailHost"); }}
        public static string TransmissionGatewayEmailToAddress { get { return settingsReader.GetSettingValue("TransmissionGatewayEmailToAddress"); } }
        public static string TransmissionGatewayEmailFromAddress { get { return settingsReader.GetSettingValue("TransmissionGatewayEmailFromAddress"); } }
        public static string TransmissionGatewayCallback { get { return settingsReader.GetSettingValue("TransmissionGatewayCallbackUrl"); }}
        public static string ContractServerPort { get { return settingsReader.GetSettingValue("ContractServerPort"); } }

        //Israel 12/16/2015 -- added new config file setting.
        public static string UserSettingsRootDir { get { return settingsReader.GetSettingValue("UserSettingsRootDir"); } }
        
        public static bool IsProductionSystem { get { return Convert.ToBoolean(settingsReader.GetSettingValue("IsProductionSystem")); } }
        public static IEnumerable<string> EmailDownloadDevsAllowSendTo { get
        {
            return settingsReader.GetSettingValues("EmailDomainsDevAllowSendTo"); 
        } }
        public static IEnumerable<string> FaxNumbersDevAllowSendTo { get
        {
            return settingsReader.GetSettingValues("FaxNumbersDevAllowSendTo");
        } }

        public static string FullUserName { get { return userName; } }

        public static string UserName
        {
            get
            {
                if (userName.Contains("\\"))
                {
                    return userName.Substring(userName.LastIndexOf("\\") + 1);
                }

                return userName;
            }
        }

        private static string appSettingsDir;

        public static string AppSettingsDir
        {
            get
            {
                if (String.IsNullOrWhiteSpace(appSettingsDir))
                {
                    throw new Exception("Configuration Error: The AppSettingsDir has not been initialized." + Environment.NewLine +
                        "Error CNF-542 in " + FORM_NAME + ".AppSettingsDir()");
                }
                return appSettingsDir;
            }
            set { appSettingsDir = value; }
        }
    }
}
