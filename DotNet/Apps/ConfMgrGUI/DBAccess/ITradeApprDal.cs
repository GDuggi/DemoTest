using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface ITradeApprDal
    {
        Int32 SetFinalApprovalFlag(Int32 pTradeId, string pFinalApprovalFlag, string pOnlyIfReadyFlag, string pUserName);
        Int32 SetFinalApprovalFlag(List<TradeApprDto> pData);
    }
}
