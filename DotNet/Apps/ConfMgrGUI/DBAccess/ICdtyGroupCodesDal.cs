using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface ICdtyGroupCodesDal
    {
        List<GetCdtyGroupCodesDto> GetAllStub();
        List<GetCdtyGroupCodesDto> GetAll();
    }
}
