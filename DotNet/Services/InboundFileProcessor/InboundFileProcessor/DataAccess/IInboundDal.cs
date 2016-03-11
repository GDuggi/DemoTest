using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InboundFileProcessor.DataAccess
{
    interface IInboundDal
    {
        int Insert(string pOrigFileName, string pMarkupFileName, InboundDocsDto pDataDocs, InboundDocsBlobDto pDataBlob);
    }
}
