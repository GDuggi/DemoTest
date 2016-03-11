using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IUserFiltersOpsmgrDal
    {
        List<UserFiltersOpsmgrDto> GetAllStub();
        List<UserFiltersOpsmgrDto> GetAll(string pUserId);
        UserFiltersOpsmgrDto Get(Int32 pId);
        Int32 Insert(UserFiltersOpsmgrDto pData);
        Int32 Update(UserFiltersOpsmgrDto pData);
        Int32 Delete(Int32 pId);
    }
}
