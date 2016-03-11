using System;
using System.Collections.Generic;
using System.Text;
using OpsTrackingModel;
using System.IO;

namespace VaultUtils
{
    public class InboundConfirmsDoc
    {
        private const string PROJ_FILE_NAME = "InboundConfirmsDoc";
        private string tradeSysCode = "";
        private string tradeSydID = "";
        private string senderTypeInd = "";
        private string senderShortName = "";
        private string senderRef = "";
        private string cptyShortName = "";
        private string seCptyShortName = "";

        private string fieldValues = "";
        private string vaultUrl = "";
        private string documentPath = "";
        private string employee = "";
        private string completedFileLocation = "";
        private string vaultFolderFieldNames = "";
        private string vaultFolder = "";

        private AssociatedDoc doc = null;

        public InboundConfirmsDoc(string user)
        {
            employee = user;
        }

        public string TradeSysCode
        {
            get { return tradeSysCode; }
            set { tradeSysCode = value; }
        }

        public string VaultUrl
        {
            get { return vaultUrl; }
            set { vaultUrl = value; }
        }

        public string TradeSysID
        {
            get { return tradeSydID; }
            set { tradeSydID = value; }
        }

        public string SenderTypeInd
        {
            get { return senderTypeInd; }
            set { senderTypeInd = value; }
        }

        public string SenderShortName
        {
            get { return senderShortName; }
            set { senderShortName = value; }
        }

        public string SenderRef
        {
            get { return senderRef; }
            set { senderRef = value; }
        }

        public string CptyShortName
        {
            get { return cptyShortName; }
            set { cptyShortName = value; }
        }

        public string SeCptyShortName
        {
            get { return seCptyShortName; }
            set { seCptyShortName = value; }
        }

        public string FieldValues
        {
            get { return fieldValues; }
            set { fieldValues = value; }
        }

        public string DocumentPath
        {
            get { return documentPath; }
            set { documentPath = value; }
        }

        public virtual string CompletedFileLocation
        {
            get { return completedFileLocation; }
            set { completedFileLocation = value; }
        }

        public virtual string VaultFolderFieldNames
        {
            get { return vaultFolderFieldNames; }
            set { vaultFolderFieldNames = value; }
        }

        public virtual string VaultFolder
        {
            get { return vaultFolder; }
            set { vaultFolder = value; }
        }

        public AssociatedDoc Doc
        {
            get { return doc; }
            set { doc = value; }
        }

        public bool VaultDocument()
        {
            bool success = false;
            try
            {
                if (doc == null)
                {
                    throw new Exception("Associated Document is not assigned." + Environment.NewLine +
                         "Error CNF-413 in " + PROJ_FILE_NAME + ".VaultDocument().");
                }
                BuildFieldValues();
                SempraDocWs ws = new SempraDocWs();
                ws.Url = VaultUrl;
                ws.Credentials = System.Net.CredentialCache.DefaultCredentials;

                FileStream fs = new FileStream(documentPath, FileMode.Open);
                Int32 initialLength = (Int32)fs.Length;
                byte[] dataStream = new byte[initialLength];
                fs.Read(dataStream, 0, initialLength);
                fs.Close();



                string RetXML = ws.ImportDocIntoVault(VaultFolder, documentPath, VaultFolderFieldNames, fieldValues, employee, false, "");
//                string RetXML = ws.ImportDocIntoVault(_lib, "\\\\STConfirm1\\InboundDocuments\\temp\\20121025_1133365641.TIF", _fieldNames, _fieldValues, "", false, "");
                //      string RetXML = ws.ImportDocByStream(VaultFolder, dataStream, "", VaultFolderFieldNames, fieldValues, employee, "");

                ws.Dispose();

                ws = null;

                System.Xml.XmlDocument RetXMLNode = new System.Xml.XmlDocument();
                RetXMLNode.LoadXml(RetXML);
                string ResultCode = RetXMLNode.DocumentElement.SelectSingleNode("//CallResult/ResultCode").InnerText;
                string ResultValue = RetXMLNode.DocumentElement.SelectSingleNode("//CallResult/ResultValue").InnerText;
                if (ResultCode.Equals("0"))
                {
                    //MessageBox.Show("Inbound Document Vaulted Successfully!");
                    success =  true;
                }
                else
                {
                    throw new Exception("An error occurred while vaulting the document: " + ResultValue + "." + Environment.NewLine +
                         "Error CNF-414 in " + PROJ_FILE_NAME + ".VaultDocument().");
                }
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while setting up the category display list." + Environment.NewLine +
                     "Error CNF-415 in " + PROJ_FILE_NAME + ".VaultDocument(): " + e.Message);
            }
            return success;
        }

        private void BuildFieldValues()
        {
            tradeSysCode = "A";

            tradeSydID = doc.TradeId.ToString();
            senderTypeInd = GetSenderTypeCode();

            senderShortName = doc.DocTypeCode;
            senderRef = employee;
            cptyShortName = doc.CptyShortName;
            seCptyShortName = "";
            fieldValues = tradeSysCode + "|" + tradeSydID + "|" + senderTypeInd + "|" + senderShortName + "|" + senderRef + "|" + cptyShortName + "|" + seCptyShortName;
        }

        private string GetSenderTypeCode()
        {
            switch (doc.DocTypeCode.ToString())
            {
                case "XQBBP": return "B";
                case "XQCCP": return "CC";
                case "XQCSP": return "CS";
                default: return "";
            }
        }

        internal string GetDealsheetFromVault(int tradeID, int version, string tradeSysCode)
        {
            string fieldNames = "TRD_SYS_ID|TRD_SYS_VER";
            string fieldValues = tradeID + "|" + version;
            string dealSheet = "";
            string docType = "";
            byte[] docStream = new byte[32768];
            bool latestVersionOnly = false;

            try
            {
                if (version == 0)
                {
                    fieldNames = "TRD_SYS_ID";
                    fieldValues = tradeID.ToString();
                    latestVersionOnly = true;
                }
                SempraDocWs ws = new SempraDocWs();


                ws.Url = VaultUrl;
                ws.Credentials = System.Net.CredentialCache.DefaultCredentials;

                string RetXML = ws.GetDocumentList("DEALSHEETS", fieldNames, fieldValues, latestVersionOnly, "");

                System.Xml.XmlDocument RetXMLNode = new System.Xml.XmlDocument();
                RetXMLNode.LoadXml(RetXML);

                if (RetXMLNode.DocumentElement.SelectSingleNode("//Documents/Doc/ResourceString") == null)
                {
                    return "No Document list found in vault: " + VaultUrl + " for Trade ID: " + tradeID.ToString() + " Version:" + version.ToString();
                }
                else
                {
                    string ResultValue = RetXMLNode.DocumentElement.SelectSingleNode("//Documents/Doc/ResourceString").InnerText;
                    RetXML = ws.GetLatestDocString("DEALSHEETS", ResultValue, "", out dealSheet, out docType);
                }

                if (dealSheet == "")
                {
                    return "Dealsheet not found  for trade and version: " + tradeID.ToString() + ":" + version.ToString();
                }
                else
                {
                    return dealSheet;
                }

            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while getting the Dealsheet from the vault for the following values:" + Environment.NewLine +
                    "Trade Id: " + tradeID + ", Version: " + version + ", Trading System Code: " + tradeSysCode + Environment.NewLine +
                     "Error CNF-416 in " + PROJ_FILE_NAME + ".GetDealsheetFromVault(): " + e.Message);
            }
        }
    }
}
