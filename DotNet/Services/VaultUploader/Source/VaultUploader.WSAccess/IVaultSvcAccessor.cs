using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VaultUploader.WSAccess.VaultServiceReference;

namespace VaultUploader.WSAccess
{
   public interface IVaultSvcAccessor
    {
       string UploadDocument(string fileName,string docType,string tradeKey,string tradingSystem,string formatInd,byte[] stream);
       ContractInfo[] GetDocInfo(string feedType, string docKey, string tradingSystem, Dictionary<string, string> queries);
       byte[] GetDocumentForURL(string feedType, string docKey, string tradingSystem, string fileName, string URL);
       ContractInfo[] GetVersionInfo(string feedType, string docKey, string tradingSystem, string fileName, string URL);
    }
}
