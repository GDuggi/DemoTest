using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IAssociatedDocsDal
    {
        List<AssociatedDoc> GetAllStub();
        List<AssociatedDoc> GetAll();
        List<AssociatedDoc> GetAll(string pTradeIdList);
        Int32 GetCount(Int32 pInboundDocsId);
        Int32 GetCount(Int32 pInboundDocsId, string pDocStatusCode);
        Int32 GetCurrentIndexValue(Int32 pInboundDocsId);
    }
}
