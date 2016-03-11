using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface ITradeAuditDal
    {
        List<TradeAuditDto> GetTradeAudit(Int32 pTradeId);
        bool HasConfirmBeenSent(Int32 pTradeId);
    }
}
