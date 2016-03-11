using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using log4net;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.IO;
using System.Reflection;


namespace ConfirmationsManager
{
    public class ConfirmationsManager : IConfirmationsManager
    {
        public GetPermissionKeysResponse getPermissionKeys(GetPermissionKeysRequest getPermissionKeysRequest)
        {
            try
            {
                Log.Info("ConfirmationsManager GetPermissionKeysRequest: " + (getPermissionKeysRequest == null ? "null" : getPermissionKeysRequest.ToString()));
                var permissionKeyCodes = new String[] { "permissionKeyCode1", "permissionKeyCode2" };

                var response = new GetPermissionKeysResponse(true, permissionKeyCodes);

                Log.Info("ConfirmationsManager getPermissionKeys: " + (response == null ? "null" : response.ToString()));
                return response;
            }
            catch (Exception e)
            {
                Log.Error("Failed to process GetPermissionKeysRequest() service call", e);
                throw e;
            }
        }


        public GetConfirmationTemplatesResponse getConfirmationTemplates(GetConfirmationTemplatesRequest getConfirmationTemplatesRequest)
        {
            try
            {
                Log.Info("ConfirmationsManager GetConfirmationTemplatesRequest: " + (getConfirmationTemplatesRequest == null ? "null" : getConfirmationTemplatesRequest.ToString()));
                var confirmationTemplateNames = new String[] { "template1", "template2" };

                var attributes1 = new Attribute[] { new Attribute("Group", "ISDA"), new Attribute("Category", "OIL") };
                var confirmationTemplate1 = new ConfirmationTemplate("OIL.BONITO.EXCH", attributes1);

                var attributes2 = new Attribute[] { new Attribute("Group", "ISDA"), new Attribute("Category", "OIL") };
                var confirmationTemplate2 = new ConfirmationTemplate("OIL.BONITO.EXCH", attributes2);


                XmlDocument xml = new XmlDocument();
                xml.Load( getFilePath( "TemplateList.xml") );
                XmlNode root = xml.DocumentElement;

                var confirmationTemplates = new List<ConfirmationTemplate>();
                foreach ( XmlNode node in xml.DocumentElement.ChildNodes )
                {                    
                    String templateName =null;
                    var attributes = new List<Attribute>();

                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Name.Equals("TemplateName") )
                            templateName = node2.InnerText;
                        else
                        {
                            var name = node2.Name;
                            var value = node2.InnerText;
                            attributes.Add(new Attribute(name, value));
                        }                        
                    }

                    if (templateName != null)
                    {
                        var confirmationTemplate = new ConfirmationTemplate(templateName, attributes.ToArray() );
                        confirmationTemplates.Add(confirmationTemplate);
                    } 

                }

                var response = new GetConfirmationTemplatesResponse(confirmationTemplates.ToArray() );

                Log.Info("ConfirmationsManager getConfirmationTemplatesResponse: " + (response == null ? "null" : response.ToString()));
                return response;
            }
            catch (Exception e)
            {
                Log.Error("Failed to process getConfirmationTemplates() service call", e);
                throw e;
            }
        }

        public TradeConfirmationStatusChangeResponse tradeConfirmationStatusChange(TradeConfirmationStatusChangeRequest tradeConfirmationStatusChangeRequest)
        {
            try
            {
                Log.Info("ConfirmationsManager TradeConfirmationStatusChangeRequest: " + (tradeConfirmationStatusChangeRequest == null ? "null" : tradeConfirmationStatusChangeRequest.ToString()));

                //th real process like update DB etcc.. since it is stub we want kepp this info in separate log file ..                
                string procees = String.Format("\n{0} {1} {2} {3} {4}", DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                                                 tradeConfirmationStatusChangeRequest.tradingSystemCode, tradeConfirmationStatusChangeRequest.tradingSystemKey,  
                                                 tradeConfirmationStatusChangeRequest.workflowInd, tradeConfirmationStatusChangeRequest.confirmationStatusCode);

                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory+"/ConfStatusChange.txt", procees);
                var response = new TradeConfirmationStatusChangeResponse(true);

                Log.Info("ConfirmationsManager TradeConfirmationStatusChangeResponse: " + (response == null ? "null" : response.ToString()));
                return response;
            }
            catch (Exception e)
            {
                Log.Error("Failed to process tradeConfirmationStatusChange() service call", e);
                throw e;
            }
        }

        String getFilePath(String fileName)
        {
            return Path.Combine(AssemblyDirectory, fileName);
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(ConfirmationsManager)); }
        }

    }
}
