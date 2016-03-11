using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ConfirmationsManager
{
    [DataContract(Namespace="http://cnf/ConfirmationsManager")]
    public class TradeConfirmationStatusChangeRequest
    {
        [DataMember(IsRequired=true)]
        public String tradingSystemCode { get; set; }

        [DataMember(IsRequired = true)]        
        public String tradingSystemKey { get; set; }

        [DataMember(IsRequired = true)]   
        public String confirmationStatusCode { get; set; }

        [DataMember]
        public WorkflowInd workflowInd { get; set; }

        public TradeConfirmationStatusChangeRequest()
        {
        }

        public TradeConfirmationStatusChangeRequest(String tradingSystemCode, String tradingSystemKey, String confirmationStatusCode )
        {
            this.tradingSystemCode = tradingSystemCode;
            this.tradingSystemKey = tradingSystemKey;
            this.confirmationStatusCode = confirmationStatusCode;
        }

        override public String ToString()
        {
            return "TradeConfirmationStatusChangeRequest( "+
                "tradingSystemCode=" + (tradingSystemCode == null ? "null" : "'" + tradingSystemCode + "'" )+
                ", tradingSystemKey=" + (tradingSystemKey == null ? "null" : "'" + tradingSystemKey + "'") +
                ", confirmationStatusCode=" + (confirmationStatusCode == null ? "null" : "'" + confirmationStatusCode + "'") +
                ", workflowInd=" +  workflowInd + "'" +
                ")";
        }
    }
}
