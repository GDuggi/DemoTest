using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IInboundDocsDal
    {
        List<InboundDocsView> GetAllStub();
        List<InboundDocsView> GetAll();
        Int32 Insert(InboundDocsDto pData);
        Int32 Update(InboundDocsDto pData);
        Dictionary<Int32, Int32> Update(List<InboundDocsDto> pInboundDocsList);
        Dictionary<Int32, Int32> UpdateStatus(Dictionary<Int32, string> pInboundDocsList);
        Int32 UpdateStatus(Int32 pId, string pStatus);
        Int32 Delete(Int32 pId);
    }
}
