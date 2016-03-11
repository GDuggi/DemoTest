using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IUserCompanyDal
    {
        List<UserCompanyDto> GetAllStub();
        List<UserCompanyDto> GetAll(string pPermissionKeyInClause);
        //List<UserCompanyDto> Fetch();
        //UserCompanyDto Fetch(int id);
        //UserCompanyDto Fetch(string shortName);
        //int Insert(UserCompanyDto data);
        //void Update(UserCompanyDto data);
        //void Delete(int id);
    }
}
