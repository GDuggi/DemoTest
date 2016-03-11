using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface ITradeRqmtConfirmDal
    {
        List<TradeRqmtConfirm> GetAllStub();
        List<TradeRqmtConfirm> GetAll();
        List<TradeRqmtConfirm> GetAll(string pTradeIdList);
        //List<TradeRqmtConfirm> GetAllSelectedTrades(string pTrdSysCode, string pSeCptySn, string pCptySn,
        //            string pCdtyCode, DateTime pBeginTradeDt, DateTime pEndTradeDt, string pTrdSysTicket);
        Int32 Insert(TradeRqmtConfirm pData);
        Int32 Update(TradeRqmtConfirm pData);
        //int UpdateCreator(Int32 pRqmtConfirmId, string pUserId);
        //string GetCreator(Int32 pRqmtConfirmId);
    }
}
