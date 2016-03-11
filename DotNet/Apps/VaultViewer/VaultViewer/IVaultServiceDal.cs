using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaultViewer
{
    public interface IVaultServiceDal
    {
        List<Dictionary<string, string>> GetDocumentInfoList(string pTradingSysCode, string pTicketNo, Dictionary<string, string> pQueryValues);
        byte[] GetDocumentForURL(string pUrl);
        byte[] GetDocumentForFileName(string pFileName, string pMetadata, string pDocURL);
    }
}
