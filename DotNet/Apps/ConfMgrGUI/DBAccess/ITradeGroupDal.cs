using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface ITradeGroupDal
    {
        int Group(List<TradeGroupDto> pDataList);
        int Ungroup(List<Int32> pTradeIdList);
    }
}
