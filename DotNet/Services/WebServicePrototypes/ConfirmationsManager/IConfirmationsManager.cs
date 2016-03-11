using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ConfirmationsManager
{
    
    [ServiceContract( Name="ConfirmationsManager", Namespace="http://cnf/ConfirmationsManager")]
    public interface IConfirmationsManager
    {
        [OperationContract]
        GetPermissionKeysResponse getPermissionKeys(GetPermissionKeysRequest getPermissionKeysRequest);

        [OperationContract]
        GetConfirmationTemplatesResponse getConfirmationTemplates(GetConfirmationTemplatesRequest getConfirmationTemplatesRequest);

        [OperationContract]
        TradeConfirmationStatusChangeResponse tradeConfirmationStatusChange(TradeConfirmationStatusChangeRequest tradeConfirmationStatusChangeRequest);
    }


}
