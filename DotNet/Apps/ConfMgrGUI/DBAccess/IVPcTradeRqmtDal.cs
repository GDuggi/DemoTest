using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IVPcTradeRqmtDal
    {
        List<RqmtData> GetAllStub();
        List<RqmtData> GetAll();
        List<RqmtData> GetAll(string pTradeIdList);
    }
}
