using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace VaultService
{
    [ServiceContract(Name = "VaultSvc", Namespace = "http://dms/VaultService")]
    public interface IVaultService
    {
        [OperationContract]
        UploadDocumentResponse UploadDocument(UploadDocumentRequest request);

        [OperationContract]
        GetVersionInfoForResponse GetVersionInfoForDocument(GetVersionInfoForDocumentRequest request);

        [OperationContract]
        GetDocInfoForQueryResponse GetDocInfoForQuery(GetDocInfoForQueryRequest request);

        [OperationContract]
        GetDocumentForURLResponse GetDocumentForURL(GetDocumentForURLRequest request);
    }
   
}
