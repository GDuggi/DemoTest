using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSAccess
{
    public interface IDealsheetAPIDal
    {
        byte[] GetDealsheetStub();
        byte[] GetDealsheet(string pTradingSysCode, string pTradingSysKey, out string pDocType);
    }
}
