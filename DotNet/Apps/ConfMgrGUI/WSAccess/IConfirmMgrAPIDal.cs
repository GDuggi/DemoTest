using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSAccess
{
    public interface IConfirmMgrAPIDal
    {
        List<string> GetPermissionKeys(string pUserId, out bool pSuperUserFlag);
        string GetPermissionKeyDBInClause(List<string> pPermissionKeyList);
    }
}
