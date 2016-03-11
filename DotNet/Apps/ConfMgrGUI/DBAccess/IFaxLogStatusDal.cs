using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IFaxLogStatusDal
    {
        List<FaxLogStatusDto> GetStub();
        List<FaxLogStatusDto> Get(Int32 pTradeRqmtConfirmId);
    }
}
