using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSAccess
{
    public interface IConfirmDocsAPIDal
    {
        byte[] GetStubDocxAuto();
        byte[] GetStubDocxManual();
        string GetStubTemplateList();
        string GetTemplateList(string pTradingSysCode, ref bool fromCache, string dataDir, bool needRefresh);
        byte[] GetConfirm(string pTradingSysCode, string pTradingSysKey, string pTemplateName, out string pDocType);
    }
}
