using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface ITradeRqmtConfirmBlobDal
    {
        Int32 GetCount(Int32 pTradeRqmtConfirmId);
        List<TradeRqmtConfirmBlobDto> GetAll();
        TradeRqmtConfirmBlobDto Get(Int32 pTradeRqmtConfirmId);
        Int32 Insert(TradeRqmtConfirmBlobDto pData);
        void Update(TradeRqmtConfirmBlobDto pData);
        Int32 Delete(Int32 pTradeRqmtConfirmId);
    }
}
