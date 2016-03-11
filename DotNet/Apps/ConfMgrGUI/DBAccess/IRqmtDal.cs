using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IRqmtDal
    {
        List<RqmtView> GetAllStub();
        List<RqmtView> GetAll();
    }
}
