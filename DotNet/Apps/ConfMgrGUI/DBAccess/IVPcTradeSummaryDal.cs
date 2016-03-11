using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IVPcTradeSummaryDal
    {
        List<SummaryData> GetAllStub();
        List<string> GetAllTradingSysCodes(string pPermissionKeyInClause);
        List<SummaryData> GetAll(string pPermissionKeyInClause);
        //List<SummaryData> GetAll(List<Int32> pTradeIdList);
        Int32 GetAllTradeIdCount(string pTrdSysCode, string pSeCptySn, string pCptySn, string pCdtyCode, 
            DateTime pBeginTradeDt, DateTime pEndTradeDt, string pTrdSysTicket, string pCptyTradeId, string pPermissionKeyInClause);
        List<SummaryData> GetAllSelectedTrades(string pTrdSysCode, string pSeCptySn, string pCptySn, string pCdtyCode, 
            DateTime pBeginTradeDt, DateTime pEndTradeDt, string pTrdSysTicket, string pCptyTradeId, string pPermissionKeyInClause);
        bool IsValidTradeId(Int32 pTradeId);
    }
}
