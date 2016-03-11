using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IUserRoleDal
    {
        List<UserRoleView> GetAllStub();
        List<UserRoleView> GetAll(string pUserId);
    }
}
