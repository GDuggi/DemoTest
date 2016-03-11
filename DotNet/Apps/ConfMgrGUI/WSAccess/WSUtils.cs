using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DevExpress.XtraRichEdit;
using System.Net.Http;
using System.Xml;
//using DevExpress.XtraPdfViewer;


namespace WSAccess
{
    public static class WSUtils
    {
        /* External Service vs Internal Implementation Cross-Reference
         * Web API Architecture:
         *      ConfirmationsManager.svc.getConfirmTemplates = ConfirmDocsAPIDal.GetTemplates
         *      ConfirmationsManager.svc.getPermissionKeys = ConfirmMgrAPIDal.GetPermissionKeys
         *      ConfirmationsManager.svc.tradeConfirmationStatusChange = ConfirmMgrAPIDal...
         *      
         *      GetDocument.svc.getConfirmation = ConfirmDocsAPIDal.GetConfirm
         *      GetDocument.svc.getDealsheet = DealsheetAPIDal.GetDealsheet
         *  **********************************************************************************    
         *      CounterParty.svc.getAgreementList = CptyInfoAPIDal...
         *      CounterParty.svc.getDocumentSendTo = CptyInfoAPIDal...
         *      CounterParty.svc.storeDocumentSendTo = CptyInfoAPIDal...
         * 
         * ConfirmManager Architecture:
         *      ConfirmDocsAPIDal.GetTemplates = ConfirmationsManager.svc.getConfirmTemplates
         *      ConfirmDocsAPIDal.GetConfirm = GetDocument.svc.getConfirmation  
         *
         *      ConfirmMgrAPIDal.GetPermissionKeys = ConfirmationsManager.svc.getPermissionKeys
         *      ConfirmMgrAPIDal... = ConfirmationsManager.svc.tradeConfirmationStatusChange
         *      
         *      DealsheetAPIDal.GetDealsheet = GetDocument.svc.getDealsheet
         *  **********************************************************************************          
         *      CptyInfoAPIDal... = CounterParty.svc.getAgreementList 
         *      CptyInfoAPIDal... = CounterParty.svc.getDocumentSendTo 
         *      CptyInfoAPIDal... = CounterParty.svc.storeDocumentSendTo
         * 
        */

        public static bool SaveByteArrayAsPdfFile(byte[] pByteArray, DocumentFormat pDocFormat, string pFileName)
        {
            bool isConversionOk = false;
            RichEditControl richEdit = new RichEditControl();
            try
            {
                using (MemoryStream memStream = new MemoryStream(pByteArray))
                {
                    richEdit.LoadDocument(memStream, pDocFormat);
                    richEdit.ExportToPdf(pFileName);
                }

                isConversionOk = true;
            }
            catch (Exception e)
            {
                isConversionOk = false;
            }
            return isConversionOk;
        }

        public static byte[] GetByteArrayFromDocument(RichEditControl pRichEdit, DocumentFormat pDocFormat)
        {
            byte[] resultByteArray = null;
            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    pRichEdit.SaveDocument(memStream, pDocFormat);
                    resultByteArray = memStream.ToArray();
                }
            }
            catch (Exception e)
            {
            }
            return resultByteArray;
        }

        public static string getUrlStr(string pUrl, string pMethod, string[] pParmList)
        {
            string resultStr = pUrl;
            resultStr += !pMethod.EndsWith("/") ? "/" + pMethod : pMethod;

            if (pParmList != null)
                foreach (string parm in pParmList)
                {
                    if (!resultStr.EndsWith("/"))
                        resultStr += "/";

                    resultStr += parm;
                }

            return resultStr;
        }

        public static string getWebServiceUrlResult(string pUrl)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage wcfResponse = client.GetAsync(pUrl).Result;
            HttpContent stream = wcfResponse.Content;
            var data = stream.ReadAsStringAsync();
            string xmlText = data.Result;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlText);
            XmlNodeList nodes = doc.GetElementsByTagName("string");
            string innerXmlText = nodes[0].InnerText;
            return innerXmlText;
        }

    }
}
