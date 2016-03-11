using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GetDocument
{
    [ServiceContract(Name = "GetDocument", Namespace = "http://cnf/GetDocument")]
    public interface IGetDocument
    {
        [OperationContract]
        GetDealSheetResponse getDealSheet(GetDealSheetRequest getDealSheetRequest);

        [OperationContract]
        GetConfirmationResponse getConfirmation(GetConfirmationRequest getConfirmationRequest);
    }
 
}
