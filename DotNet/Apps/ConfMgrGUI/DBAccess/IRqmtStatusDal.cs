

using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IRqmtStatusDal
    {
        List<RqmtStatusView> GetAllStub();
        List<RqmtStatusView> GetAll();
    }
}
