using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSAccess.VaultSvcClient;

namespace WSAccess
{
   public class VaultSvcAccessor:IVaultSvcAccessor
    {
       private const string VAULT_SVC_ENDPOINT = "BasicHttpBinding_VaultSvc";
       private const string VAULT_SVC_URL_EXT = "VaultService";
      
       VaultSvcClient.VaultSvcClient _client = null;
       
        public VaultSvcAccessor(string pBaseUrl, string pSvcUserName, string pSvcPassword)
        {          
            _client = new VaultSvcClient.VaultSvcClient(VAULT_SVC_ENDPOINT, pBaseUrl + @"/" + VAULT_SVC_URL_EXT);
            //why user id pwd...
            _client.ClientCredentials.UserName.UserName = pSvcUserName;
            _client.ClientCredentials.UserName.Password = pSvcPassword;
        }

        public VaultSvcAccessor(string pBaseUrl)
        {      
            _client = new VaultSvcClient.VaultSvcClient(VAULT_SVC_ENDPOINT, pBaseUrl + @"/" + VAULT_SVC_URL_EXT);
        }

        public VaultSvcAccessor()            
        {
            string pBaseUrl = String.Empty;//TODO :get this from config local
            _client = new VaultSvcClient.VaultSvcClient(VAULT_SVC_ENDPOINT, pBaseUrl + @"/" + VAULT_SVC_URL_EXT);
        }

        private static VaultSvcAccessor _vaultSvcAccessor;
        public static VaultSvcAccessor Instance(string pBaseUrl =null)
        {
            if (_vaultSvcAccessor == null)
            {
                if (!String.IsNullOrEmpty(pBaseUrl))
                    _vaultSvcAccessor = new VaultSvcAccessor(pBaseUrl);
                else
                    _vaultSvcAccessor = new VaultSvcAccessor();

            }
            return _vaultSvcAccessor;
        }

       public string UploadDocument(string fileName, string docType, string tradeKey, string tradingSystem, byte[] stream)
       {
           UploadDocumentRequest request = new UploadDocumentRequest
           {
               DocumentKey=tradeKey,
               DocumentType=docType,
               FeedType="Manual",
               FileName=fileName,
               ObjectFormatInd="Bytes",
               ObjectStream=stream ,
               TradingSystemCode = tradingSystem               
           };

           UploadDocumentResponse response = _client.UploadDocument(request);
         
           return String.Format("Status:{0} ,URL :{1}",response.Status,response.URL);
       }

       public ContractInfo[] GetDocInfo(string feedType, string docKey, string tradingSystem, Dictionary<string,string> queries)
       {
           GetDocInfoForQueryRequest request = new GetDocInfoForQueryRequest
           {
               DocumentKey = docKey,
               FeedType=feedType,
               TradingSystemCode = tradingSystem,
               QueryValues = queries
           };       

           GetDocInfoForQueryResponse response = _client.GetDocInfoForQuery(request);

           return response.QueryResult;
       }

       public byte[] GetDocumentForURL(string feedType, string docKey, string tradingSystem, string fileName,string URL)
       {
           GetDocumentForURLRequest request = new GetDocumentForURLRequest
           {
               DocumentKey=docKey,
               FeedType=feedType,
               FileName=fileName,
               TradingSystemCode=tradingSystem,
               URL=URL
           };     

           GetDocumentForURLResponse response = _client.GetDocumentForURL(request);

           return response.ObjectStream;
       }

       public ContractInfo[] GetVersionInfo(string feedType, string docKey, string tradingSystem, string fileName, string URL)
       {
           GetVersionInfoForDocumentRequest request = new GetVersionInfoForDocumentRequest
           {
               DocumentKey = docKey,
               FeedType = feedType,               
               TradingSystemCode = tradingSystem                         
           };

           GetVersionInfoForResponse response = _client.GetVersionInfoForDocument(request);

           return response.QueryResult;
       }
    }
}
