using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface ITradeSummaryDal
    {
        Int32 UpdateCptyTradeId(Int32 pTradeId, string pCptyTradeId);
        Int32 UpdateCmt(Int32 pTradeId, string pCmt);
        Int32 UpdateCmts(List<TradeSummaryDto> pData);
        Int32 UpdateDetermineActions(Int32 pTradeId, string pOptsDetActFlag);
        Int32 UpdateDetermineActions(List<TradeSummaryDto> pData);
        Int32 UpdateFinalApproval(Int32 pTradeId, string pFinalApprovalStatus);
        Int32 UpdateFinalApproval(List<TradeSummaryDto> pData);
    }
}
