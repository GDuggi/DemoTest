using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ConfirmationsManager
{
    [DataContract( Namespace="http://cnf/ConfirmationsManager" )]
    public class GetConfirmationTemplatesRequest
    {
        [DataMember(IsRequired = true)]
        String tradingSystemCode { get; set; }

        public GetConfirmationTemplatesRequest()
        {
        }

        public GetConfirmationTemplatesRequest( String tradingSystemCode)
        {
            this.tradingSystemCode = tradingSystemCode;            
        }

        override public String ToString()
        {
            return "GetConfirmationTemplatesRequest( " +
                "tradingSystemCode =" + (tradingSystemCode == null ? "null" : "'" + tradingSystemCode + "'") +             
                ")";
        }

    }
}

