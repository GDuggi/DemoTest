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
        bool IsUserIdValid(string pUserId);
        int AddRolesToUser(List<string> roles, string pUserId);
        int RemoveRolesFromUser(List<string> roles, string pUserId);
    }
}
