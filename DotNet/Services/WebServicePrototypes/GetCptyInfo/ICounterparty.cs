using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Counterparty
{    
    [ServiceContract( Name="Counterparty", Namespace="http://cnf/Integration")]
    public interface ICounterparty
    {

        // TODO: Add your service operations here
        [OperationContract]
        GetAgreementListResponse getAgreementList(GetAgreementListRequest getAgreementListRequest);

        [OperationContract]
        GetDocumentSendToResponse getDocumentSendTo(GetDocumentSendToRequest getDocumentSendToRequest);

        [OperationContract]
        StoreDocumentSendToResponse storeDocumentSendTo(StoreDocumentSendToRequest storeDocumentSendToRequest);
    }

}
