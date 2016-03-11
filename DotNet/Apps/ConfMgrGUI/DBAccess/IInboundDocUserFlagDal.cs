using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IInboundDocUserFlagDal
    {
        List<InboundDocUserFlagDto> Get(string pInboundUser);
        Dictionary<Int32, Int32> UpdateFlags(List<InboundDocUserFlagDto> pUserFlagList);
        Int32 UpdateFlag(InboundDocUserFlagDto pUserFlagData);
    }
}
