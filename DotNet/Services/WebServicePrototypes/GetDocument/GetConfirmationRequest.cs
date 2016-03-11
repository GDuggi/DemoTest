using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GetDocument
{
     [DataContract(Namespace = "http://cnf/Integration")]
    public class GetConfirmationRequest
    {
        [DataMember(IsRequired = true)]
        public String tradingSystemCode { get; set; }

        [DataMember(IsRequired = true)]        
        public String tradingSystemKey { get; set; }

        [DataMember]        
        public String templateName { get; set; }

        public GetConfirmationRequest()
        {
        }

        public GetConfirmationRequest(String tradingSystemCode, String tradingSystemKey)
        {
            this.tradingSystemCode = tradingSystemCode;
            this.tradingSystemKey = tradingSystemKey;
        }

        public GetConfirmationRequest(String tradingSystemCode, String tradingSystemKey, String templateName)
        {
            this.tradingSystemCode = tradingSystemCode;
            this.tradingSystemKey = tradingSystemKey;
            this.templateName = templateName;
        }

        override public String ToString()
        {
            return "GetConfirmationRequest( " +
                "tradingSystemCode=" + (tradingSystemCode == null ? "null" : "'" + tradingSystemCode + "'" )+
                ", tradingSystemKey=" + (tradingSystemKey == null ? "null" : "'" + tradingSystemKey + "'") +
                ", templateName=" + (templateName == null ? "null" : "'" + templateName + "'") +
                ")";
        }
    }
}
