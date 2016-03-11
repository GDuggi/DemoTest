using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IRqmtStatusColorsDal
    {
        List<RqmtStatusColor> GetAllStub();
        List<RqmtStatusColor> GetAll();
    }
}
