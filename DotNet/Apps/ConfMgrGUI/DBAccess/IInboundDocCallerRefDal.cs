using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IInboundDocCallerRefDal
    {
        Dictionary<string, Int32> MapCallerRef(List<InboundDocCallerRefDto> pCallerRefList);
        Int32 MapCallerRef(string pCallerRef, string pCptySn, string pRefType);
        Dictionary<string, Int32> UnmapCallerRef(List<InboundDocCallerRefDto> pCallerRefList);
        Int32 UnmapCallerRef(string pCallerRef);
    }
}
