using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VaultService.DataContracts;

namespace VaultService
{
    public class VaultService : IVaultService
    {

        public UploadDocumentResponse UploadDocument(UploadDocumentRequest request)
        {
            //TODO store these at loaacl folders side by exe.
            return new UploadDocumentResponse { Status = "Uploaded", URL = String.Format("{0}/{1}/{2}", request.FeedType, request.DocumentType, request.FileName) };
        }

        public GetVersionInfoForResponse GetVersionInfoForDocument(GetVersionInfoForDocumentRequest request)
        {
            ContractInfo DocInfo = new ContractInfo();
            if (request.DocInfo == null)
            {
                DocInfo.BookingCompShortName = "Merc GVA";
                DocInfo.CmdtyGroup = "CRUDE";
                DocInfo.DocName = request.DocumentKey;
                DocInfo.DocType = request.FeedType;
                DocInfo.Source = request.TradingSystemCode;
                DocInfo.FileType="docx";
            }
            DocInfo = request.DocInfo;

            ContractInfo[] contracts = new ContractInfo[5];
            contracts[0] = new ContractInfo
            {
                BookingCompShortName = DocInfo.BookingCompShortName,
                CmdtyGroup = DocInfo.CmdtyGroup,
                DocName = request.DocumentKey,
                DocType = request.FeedType,
                FileType = DocInfo.FileType,
                IsLatestVersion = false,
                TradeNum = ConvertToInt (request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt (request.DocumentKey)+1,
                VersionNum="1",
                URL = DocInfo.CmdtyGroup + "/" + request.FeedType + "/" + DocInfo.FileType + "/" + request.DocumentKey + "/" + request.DocumentKey + 
                "1" + "." + DocInfo.FileType            
            };
            contracts[1] = new ContractInfo
            {
                BookingCompShortName = DocInfo.BookingCompShortName,
                CmdtyGroup = DocInfo.CmdtyGroup,
                DocName = request.DocumentKey,
                DocType = request.FeedType,
                FileType = DocInfo.FileType,
                IsLatestVersion = false,
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey) + 2,
                VersionNum = "2",
                URL = DocInfo.CmdtyGroup + "/" + request.FeedType + "/" + DocInfo.FileType + "/" + request.DocumentKey + "/" + request.DocumentKey +
                "2" + "." + DocInfo.FileType     
            };
            contracts[2] = new ContractInfo
            {
                BookingCompShortName = DocInfo.BookingCompShortName,
                CmdtyGroup = DocInfo.CmdtyGroup,
                DocName = request.DocumentKey,
                DocType = request.FeedType,
                FileType = DocInfo.FileType,
                IsLatestVersion = false,
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey) + 3,
                VersionNum = "3",
                URL = DocInfo.CmdtyGroup + "/" + request.FeedType + "/" + DocInfo.FileType + "/" + request.DocumentKey + "/" + request.DocumentKey +
                "3" + "." + DocInfo.FileType     

            };
            contracts[3] = new ContractInfo
            {
                BookingCompShortName = DocInfo.BookingCompShortName,
                CmdtyGroup = DocInfo.CmdtyGroup,
                DocName = request.DocumentKey,
                DocType = request.FeedType,
                FileType = DocInfo.FileType,
                IsLatestVersion = false,
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey) + 4,
                VersionNum = "4",
                URL = DocInfo.CmdtyGroup + "/" + request.FeedType + "/" + DocInfo.FileType + "/" + request.DocumentKey + "/" + request.DocumentKey +
                "4" + "." + DocInfo.FileType     
            };
            contracts[4] = new ContractInfo
            {
                BookingCompShortName = DocInfo.BookingCompShortName,
                CmdtyGroup = DocInfo.CmdtyGroup,
                DocName = request.DocumentKey,
                DocType = request.FeedType,
                FileType = DocInfo.FileType,
                IsLatestVersion = false,
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey) + 5,
                VersionNum = "5",
                URL = DocInfo.CmdtyGroup + "/" + request.FeedType + "/" + DocInfo.FileType + "/" + request.DocumentKey + "/" + request.DocumentKey +
                "5" + "." + DocInfo.FileType 
            };

            return new GetVersionInfoForResponse { QueryResult=contracts };
        }

        public GetDocInfoForQueryResponse GetDocInfoForQuery(GetDocInfoForQueryRequest request)
        {
            ContractInfo[] contracts = new ContractInfo[8];      
            contracts[0] = new ContractInfo
            {
                BookingCompShortName = "Merc GVA",
                CmdtyGroup = "BRENT",
                DocName = "SampleConfirm.docx",
                DocType = "OURPAPER_OUTBOUND",
                FileType = "docx",
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey)+2 ,
                URL = "BRENT" + "/" + request.FeedType + "/" + "docx" + "/" + ConvertToInt(request.DocumentKey) + 2 + ".docx" 
            };
            contracts[1] = new ContractInfo
            {
                BookingCompShortName = "Merc USA",
                CmdtyGroup = "WTI",
                DocName = "Template_01.docx",
                DocType = "OURPAPER_OUTBOUND",
                FileType = "docx",
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey) +3,
                URL = "WTI" + "/" + request.FeedType + "/" + "docx" + "/" + ConvertToInt(request.DocumentKey) + 3 + ".docx" 
            };
            contracts[2] = new ContractInfo
            {
                BookingCompShortName = "Merc Sing",
                CmdtyGroup = "CRUDE",
                DocName = "BillOfLading_01.tif",
                DocType = "BROKER_INBOUND",
                FileType = "tif",
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey) + 4,
                URL = "CRUDE" + "/" + request.FeedType + "/" + "tif" + "/" + ConvertToInt(request.DocumentKey) + 4 + ".tif" 
            };
            contracts[3] = new ContractInfo
            {
                BookingCompShortName = "Merc GVA",
                CmdtyGroup = "BRENT",
                DocName = "Contract_01.pdf",
                DocType = "OURPAPER_INBOUND",
                FileType = "pdf",
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey) + 4,
                URL = "BRENT" + "/" + request.FeedType + "/" + "pdf" + "/" + ConvertToInt(request.DocumentKey) + 2 + ".pdf"
            };
            contracts[4] = new ContractInfo
            {
                BookingCompShortName = "Merc GVA",
                CmdtyGroup = "BRENT",
                DocName = "BillOfLading_02.rtf",
                DocType = "BROKER_INBOUND",
                FileType = "rtf",
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey) + 2,
                URL = "BRENT" + "/" + request.FeedType + "/" + "rtf" + "/" + ConvertToInt(request.DocumentKey) + 2 + ".rtf"
            };
            contracts[5] = new ContractInfo
            {
                BookingCompShortName = "Merc USA",
                CmdtyGroup = "WTI",
                DocName = "BillOfLading_03.doc",
                DocType = "BROKER_OUTBOUND",
                FileType = "doc",
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey) + 3,
                URL = "WTI" + "/" + request.FeedType + "/" + "doc" + "/" + ConvertToInt(request.DocumentKey) + 2 + ".doc"
            };
            contracts[6] = new ContractInfo
            {
                BookingCompShortName = "Merc Sing",
                CmdtyGroup = "CRUDE",
                DocName = "CptyPaper_01.html",
                DocType = "CPTYPAPER_INBOUND",
                FileType = "html",
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey) + 1,
                URL = "CRUDE" + "/" + request.FeedType + "/" + "html" + "/" + ConvertToInt(request.DocumentKey) + 2 + ".html"
            };
            contracts[7] = new ContractInfo
            {
                BookingCompShortName = "Merc GVA",
                CmdtyGroup = "BRENT",
                DocName = "MiscDoc_01.txt",
                DocType = "CPTYPAPER_OUTBOUND",
                FileType = "txt",
                TradeNum = ConvertToInt(request.DocumentKey),
                Source = request.TradingSystemCode,
                DocID = ConvertToInt(request.DocumentKey) + 2,
                URL = "BRENT" + "/" + request.FeedType + "/" + "txt" + "/" + ConvertToInt(request.DocumentKey) + 2 + ".txt"
            };
            return new GetDocInfoForQueryResponse {QueryResult=contracts };
        }

        public GetDocumentForURLResponse GetDocumentForURL(GetDocumentForURLRequest request)
        {            
            //TODO:load the documents from local folders
            Log.Info("GetDocumentForURL request...");
            string fileName = @"TestData\SampleConfirm.docx"; 
            if (!String.IsNullOrEmpty(request.FileName))
                fileName = @"TestData\" + request.FileName;
            return new GetDocumentForURLResponse
                {
                    //ObjectStream = getFile("TestData\\SampleConfirm.docx")
                    ObjectStream = getFile(fileName)
                };
        }

        byte[] getFile(String fileName)
        {
            String path = Path.Combine(AssemblyDirectory, fileName);
            Log.InfoFormat("Loading file {0}", path);
            byte[] b = File.ReadAllBytes(path);
            return b;
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

        private int ConvertToInt(string value)
        {
            int rtn = 0;
            try
            {
                rtn = int.Parse(value);
            }
            catch (Exception ex)
            {
            }
            return rtn;
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(VaultService)); }
        }
    }
}
