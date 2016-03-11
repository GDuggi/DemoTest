using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraRichEdit;
using WSAccess.SvcRef_GetDocument;

namespace WSAccess
{
    public class DealsheetAPIDal : IDealsheetAPIDal
    {
        private const string GET_DOCUMENT_ENDPOINT = "BasicHttpBinding_GetDocument";
       // private const string GET_DOCUMENT_URL_EXT = "GetDocument";
        private const string GET_DOCUMENT_URL_EXT = "GetDocument";
        private string urlStr = ""; 
        private string svcUserName = ""; 
        private string svcPassword = ""; 

        public DealsheetAPIDal(string pUrl, string pSvcUserName, string pSvcPassword)
        {
            urlStr = pUrl + @"/" + GET_DOCUMENT_URL_EXT; ;
            svcUserName = pSvcUserName;
            svcPassword = pSvcPassword;
        }

        public byte[] GetDealsheetStub()
        {
            string SAMPLE_DEALSHEET_FILENAME = @"\\cnf01file01\CNF01\Apps\Tools\TestDocs\StubSamples\SampleDealsheet.html";
            byte[] dealsheetBytes = System.IO.File.ReadAllBytes(SAMPLE_DEALSHEET_FILENAME);
            return dealsheetBytes;
        }

        public byte[] GetDealsheet(string pTradingSysCode, string pTradingSysKey, out string pDocType)
        {
            byte[] resultDoc = null;
            GetDocumentClient client = new GetDocumentClient(GET_DOCUMENT_ENDPOINT, urlStr);
            client.ClientCredentials.UserName.UserName = svcUserName;
            client.ClientCredentials.UserName.Password = svcPassword;

            GetDealSheetResponse response = new GetDealSheetResponse();
            GetDealSheetRequest request = new GetDealSheetRequest();
            request.tradingSystemCode = pTradingSysCode;
            request.tradingSystemKey = pTradingSysKey;
            response = client.getDealSheet(request);

            pDocType = response.objectFormatInd.ToString();
            resultDoc = response.objectStream;

            return resultDoc;
        }

    }
}
