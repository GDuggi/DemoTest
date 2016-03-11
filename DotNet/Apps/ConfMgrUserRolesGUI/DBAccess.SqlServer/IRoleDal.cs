using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IRoleDal
    {
        List<RoleView> GetAllStub();
        List<RoleView> GetAll();
    }
}
