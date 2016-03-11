using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GetDocument
{
     [DataContract(Namespace = "http://cnf/Integration")]
    public class GetDealSheetRequest
    {
        [DataMember(IsRequired=true)]
        public String tradingSystemCode { get; set; }

        [DataMember(IsRequired = true)]        
        public String tradingSystemKey { get; set; }

        public GetDealSheetRequest()
        {
        }

        public GetDealSheetRequest(String tradingSystemCode, String tradingSystemKey)
        {
            this.tradingSystemCode = tradingSystemCode;
            this.tradingSystemKey = tradingSystemKey;
        }

        override public String ToString()
        {
            return "GetDealSheetRequest( "+
                "tradingSystemCode=" + (tradingSystemCode == null ? "null" : "'" + tradingSystemCode + "'" )+
                ", tradingSystemKey=" + (tradingSystemKey == null ? "null" : "'" + tradingSystemKey + "'") +
                ")";
        }
    }
}
