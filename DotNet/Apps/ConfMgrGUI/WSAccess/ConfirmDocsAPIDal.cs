using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using WSAccess.SvcRef_GetDocument;
using WSAccess.SvcRef_ConfirmationsManager;

namespace WSAccess
{
    public class ConfirmDocsAPIDal : IConfirmDocsAPIDal
    {
        private const string FORM_NAME = "ConfirmDocsAPIDal";
        const string DOCX_TESTDOC = @"\\cnf01file01\CNF01\Apps\Tools\TestDocs\StubSamples\SampleConfirm.docx";
        const string TEMPLATE_LIST_STUB = @"\\CNF01INF01\cnf01\Apps\Tools\TestDocs\TemplateList.xml";
        const string DOCX_TESTDOC_1 = @"\\CNF01INF01\cnf01\Apps\Tools\TestDocs\0009_NGAS.PHYS.LONG.FORM.docx";
        const string DOCX_TESTDOC_2 = @"\\CNF01INF01\cnf01\Apps\Tools\TestDocs\0039_OIL.SWAP.FLO.FLO.ISDA.PARTY.B.docx";

        private const string GET_DOCUMENT_ENDPOINT = "BasicHttpBinding_GetDocument";
        private const string GET_DOCUMENT_URL_EXT = "GetDocument";
        private const string CONFIRMATIONS_MGR_ENDPOINT = "BasicHttpBinding_ConfirmationsManager";
        private const string CONFIRMATIONS_MGR_URL_EXT = "ConfirmationsManager";
        
        private string baseUrlStr = ""; 
        private string svcUserName = ""; 
        private string svcPassword = "";

        public ConfirmDocsAPIDal(string pBaseUrl, string pSvcUserName, string pSvcPassword)
        {
            baseUrlStr = pBaseUrl;
            svcUserName = pSvcUserName;
            svcPassword = pSvcPassword;
        }

        private static ConfirmDocsAPIDal _confirmDocsAPIDal;
        public static ConfirmDocsAPIDal Instance(string pBaseUrl, string pSvcUserName, string pSvcPassword)
        {
            if (_confirmDocsAPIDal==null)
            {
                _confirmDocsAPIDal = new ConfirmDocsAPIDal(pBaseUrl, pSvcUserName, pSvcPassword);
            }
            return _confirmDocsAPIDal;
        }

        #region Stubbed Data

        public string GetStubTemplateList()
        {
            //string filename = @"\\CNF01INF01\cnf01\Apps\Tools\TestDocs\TemplateList.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(TEMPLATE_LIST_STUB);

            //string innerXmlText = xmlDoc.nodenodes[0].InnerText;
            //return innerXmlText;

            return xmlDoc.InnerXml;
        }

        public byte[] GetStubDocxAuto()
        {
            byte[] resultDoc;
            resultDoc = File.ReadAllBytes(DOCX_TESTDOC);
            return resultDoc;
        }

        public byte[] GetStubDocxManual()
        {
            byte[] resultDoc;
            resultDoc = File.ReadAllBytes(DOCX_TESTDOC);
            return resultDoc;
        }

        #endregion

        private Dictionary<string, string> _templatesDict = null;
        public string GetTemplateList(string pTradingSysCode, ref bool fromCache, string dataDir, bool needRefresh)
        {
            if (_templatesDict != null && _templatesDict.ContainsKey(pTradingSysCode) && !needRefresh)
            {
                return _templatesDict[pTradingSysCode];
            }

            string templateXml = GetTemplateList(pTradingSysCode,ref fromCache, dataDir);
            if (_templatesDict == null)
                _templatesDict = new Dictionary<string, string>();

            _templatesDict[pTradingSysCode] = templateXml;//add to dictionary
            return templateXml;
        }

        
        private string GetTemplateList(string pTradingSysCode,ref bool fromCache,string dataDir)
        {
            string xmlResult = "";            
            try
            {
                GetConfirmationTemplatesResponse response = GetTemplates(pTradingSysCode);
                // confirmationTemplate resultList = new confirmationTemplate();
                //resultList = response.confirmationTemplates
                xmlResult= WriteIntoXml(response.confirmationTemplates);
                PushXMlToCache(xmlResult, pTradingSysCode, dataDir);//save this data user level folder so that user can use that next instance when service is down.
                return xmlResult;
            }
            catch (Exception ex)
            {
                //TODO: on specific exception where service not run stuff like that 
                try
                {
                    fromCache = true;
                    return GetTemplateListFromCache(pTradingSysCode, dataDir);
                }
                catch (Exception exL)
                {
                    throw new Exception("The Services are down. Please contact the administrator." + Environment.NewLine +
                        "Error CNF-545 in " + FORM_NAME + ".GetTradeData(): " + ex.Message);
                }

            }
        }

        private GetConfirmationTemplatesResponse GetTemplates(string pTradingSysCode)
        {
            string confirmMgrUrl = baseUrlStr + @"/" + CONFIRMATIONS_MGR_URL_EXT;
            ConfirmationsManagerClient client = new ConfirmationsManagerClient(CONFIRMATIONS_MGR_ENDPOINT, confirmMgrUrl);
            client.ClientCredentials.UserName.UserName = svcUserName;
            client.ClientCredentials.UserName.Password = svcPassword;

            GetConfirmationTemplatesRequest request = new GetConfirmationTemplatesRequest();
            GetConfirmationTemplatesResponse response = new GetConfirmationTemplatesResponse();

            request.tradingSystemCode = pTradingSysCode;
            response = client.getConfirmationTemplates(request);
            //throw new Exception(); //for test bad scenario
            return response;
        }

        private void PushXMlToCache(string xmlResult, string pTradingSysCode,string dataDir)
        {            
            if(!Directory.Exists(Path.Combine(dataDir, pTradingSysCode)))
            {
                Directory.CreateDirectory(Path.Combine(dataDir, pTradingSysCode));
            }
            File.WriteAllText(Path.Combine(dataDir, pTradingSysCode, _fileNameOfTemapltesCache), xmlResult);           
        }

        private const string  _fileNameOfTemapltesCache="Templates.xml";
        private string GetTemplateListFromCache(string pTradingSysCode, string dataDir)
        {
            return File.ReadAllText(Path.Combine(dataDir, pTradingSysCode, _fileNameOfTemapltesCache));            
        }

        private static string WriteIntoXml(confirmationTemplate[] templates)
        {
            string xmlResult = "";
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmltextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };

            // Start document
            xmltextWriter.WriteStartDocument();
            xmltextWriter.WriteStartElement("Templates");

            foreach (confirmationTemplate templateItem in templates)
            {
                xmltextWriter.WriteStartElement("Template");
                xmltextWriter.WriteElementString("TemplateName", templateItem.templateName);
                //Israel 9/30/2015 -- Uncomment when implementing Group and Category (and any other attributes)
                //Also be sure to uncomment/add these attributes as columns in the frmTemplateList.
                foreach (attribute templateItemAttribute in templateItem.attributes)
                {
                    xmltextWriter.WriteElementString(templateItemAttribute.name, templateItemAttribute.value);
                }
                xmltextWriter.WriteEndElement();
            }
            xmltextWriter.WriteEndElement();
            xmltextWriter.Flush();
            xmltextWriter.Close();
            stringWriter.Flush();

            xmlResult = stringWriter.ToString();
            
            return xmlResult;
        }

        public byte[] GetConfirm(string pTradingSysCode, string pTradingSysKey, string pTemplateName, out string pDocType)
        {
            byte[] resultDoc = null;
            string getDocumentUrl = baseUrlStr + @"/" + GET_DOCUMENT_URL_EXT;

            GetDocumentClient client = new GetDocumentClient(GET_DOCUMENT_ENDPOINT, getDocumentUrl);
            client.ClientCredentials.UserName.UserName = svcUserName;
            client.ClientCredentials.UserName.Password = svcPassword;

            GetConfirmationRequest request = new GetConfirmationRequest();
            GetConfirmationResponse response = new GetConfirmationResponse();

            request.tradingSystemCode = pTradingSysCode;
            request.tradingSystemKey = pTradingSysKey;
            request.templateName = pTemplateName;
            response = client.getConfirmation(request);
            
            pDocType = response.objectFormatInd.ToString();
            resultDoc = response.objectStream;

            return resultDoc;
        }



    }
}
