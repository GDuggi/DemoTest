using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface ITradeRqmtDal
    {
        Int32 AddTradeRqmt(Int32 pTradeId, string pRqmtCode, string pReference, string pCmt);
        Int32 UpdateTradeRqmt(Int32 pId, DateTime pCompletedDt, string pSecondChk, string pStatus, string pReference, string pCmt);
        Int32 UpdateTradeRqmts(List<TradeRqmtDto> pData);
    }
}
