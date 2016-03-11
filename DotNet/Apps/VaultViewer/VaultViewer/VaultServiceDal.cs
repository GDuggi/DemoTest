using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VaultViewer.VaultSvc;

namespace VaultViewer
{
    public class VaultServiceDal : IVaultServiceDal
    {
        private const string VAULTVIEWER_ENDPOINT = "BasicHttpBinding_VaultSvc";
        private const string VAULTSERVICE_URL_EXT = "VaultService";
        private const string APPLICATION_NAME = "VaultViewer";

        private string urlStr = "";
        private string svcUserName = "";
        private string svcPassword = "";

        public VaultServiceDal(string pBaseUrl, string pSvcUserName, string pSvcPassword)
        {
            urlStr = pBaseUrl + @"/" + VAULTSERVICE_URL_EXT;
            svcUserName = pSvcUserName;
            svcPassword = pSvcPassword;
        }

        public List<Dictionary<string, string>> GetDocumentInfoList(string pTradingSysCode, string pTicketNo, Dictionary<string, string> pQueryValues)
        {
            List<Dictionary<string, string>> dicList = new List<Dictionary<string, string>>();

            VaultSvcClient client = new VaultSvcClient(VAULTVIEWER_ENDPOINT, urlStr);
            client.ClientCredentials.UserName.UserName = svcUserName;
            client.ClientCredentials.UserName.Password = svcPassword;

            GetDocInfoForQueryRequest request = new GetDocInfoForQueryRequest
            {
                TradingSystemCode = pTradingSysCode,
                DocumentKey = pTicketNo,
                FeedType = "CONTRACTS",
                QueryValues = pQueryValues
            };

            GetDocInfoForQueryResponse response = new GetDocInfoForQueryResponse();
            /*
             * response.QueryResult is a list of ContractInfo objects, i.e., documents
             * Iterate through them and for each document find every valid data field which contains data.
             * Add the field name and data value to a dictionary for the document.
             * Return the list of dictionary items, containing one dictionary item for each document
             * For now, display all boolean fields whether true or false.
             */
            response = client.GetDocInfoForQuery(request);
            if (response != null && response.QueryResult != null)
            {
                foreach (var docInfo in response.QueryResult)
                {
                    Type docInfoType = docInfo.GetType();
                    PropertyInfo[] propsForDocInfo = docInfoType.GetProperties();

                    Dictionary<string, string> dicItem = new Dictionary<string, string>();
                    foreach (PropertyInfo prop in propsForDocInfo)
                    {
                        var propType = prop.PropertyType;
                        bool getValueOk = false;

                        //Ignore all non-data-containing fields
                        if (propType.IsPrimitive || propType == typeof(Decimal) || propType == typeof(String))
                        {
                            string propValue = String.Empty;
                            if (prop.GetValue(docInfo, null) != null)
                                propValue = prop.GetValue(docInfo, null).ToString();

                            if (VVUtils.IsNumericPropertyType(propType.Name))
                            {
                                //propValue = prop.GetValue(docInfo, null).ToString();
                                if (propValue != "0")
                                    getValueOk = true;
                            }
                            else if (!String.IsNullOrEmpty(propValue))
                            {
                                //propValue = prop.GetValue(docInfo, null).ToString();
                                getValueOk = true;
                            }

                            if (getValueOk)
                            {
                                string propName = prop.Name;
                                dicItem.Add(propName, propValue);
                            }
                        }

                        //Test stub
                        //dicItem.Add("DocType", "CONFIRM-OUTBOUND");
                        //dicItem.Add("URL", "abd912732kjdyes932po95ahd883wsgf52");
                    }
                    dicList.Add(dicItem);
                }
            }

            return dicList;
        }

        public byte[] GetDocumentForURL(string pUrl)
        {
            VaultSvcClient client = new VaultSvcClient(VAULTVIEWER_ENDPOINT, urlStr);
            client.ClientCredentials.UserName.UserName = svcUserName;
            client.ClientCredentials.UserName.Password = svcPassword;

            GetDocumentForURLRequest request = new GetDocumentForURLRequest
            {
                TradingSystemCode = "",
                DocumentKey = "",
                FeedType = "",
                FileName = "",
                URL = pUrl 
            };

            GetDocumentForURLResponse response = client.GetDocumentForURL(request);

            return response.ObjectStream;
        }

        public byte[] GetDocumentForFileName(string pFileName, string pMetadata, string pDocURL)
        {
            VaultSvcClient client = new VaultSvcClient(VAULTVIEWER_ENDPOINT, urlStr);
            client.ClientCredentials.UserName.UserName = svcUserName;
            client.ClientCredentials.UserName.Password = svcPassword;           

            Dictionary<string,string> docDetails = extractDataFromMetaData(pMetadata);
            GetDocumentForURLRequest request = new GetDocumentForURLRequest();
            //{
            //    TradingSystemCode = "",
            //    DocumentKey = "",
            //    FeedType = "",
            //    FileName = pFileName,
            //    URL = ""
            //};
            request.FileName = pFileName;
            request.FeedType = "CONTRACTS";//(docDetails.ContainsKey("FileType") ==true ? docDetails["FileType"].ToString() : "");
            request.DocumentKey = (docDetails.ContainsKey("TradeNum") == true ? docDetails["TradeNum"].ToString() : "");
            request.TradingSystemCode = (docDetails.ContainsKey("Source") == true ? docDetails["Source"].ToString() : "");
            request.URL = pDocURL;

            GetDocumentForURLResponse response = client.GetDocumentForURL(request);

            return response.ObjectStream;
        }

        //TODO Make it generic
        public Dictionary<string,string> extractDataFromMetaData(string pMetadata)
        {
            Dictionary<string,string> detailData = new Dictionary<string,string>();
            if (!string.IsNullOrEmpty(pMetadata))
            {
                string[] docDetails = pMetadata.Split(';');
                if (docDetails != null && docDetails.Count() > 0 )
                {
                    foreach( string data in docDetails)
                    {
                        if (data != null && data.Length > 0)
                        {
                            string[] srtData = data.Split('=');
                            if (srtData != null && srtData.Count() > 0)
                                detailData.Add( srtData[0].Trim(), srtData[1].Trim());
                        }

                    }
                }

            }
            return detailData;
        }
    }
}
