using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Counterparty
{
    [DataContract(Namespace = "http://cnf/Integration")]
    public class GetDocumentSendToResponse
    {
        [DataMember]
        public SendTo[] sendTos  { get; set; }

        public GetDocumentSendToResponse()
        {
        }

        public GetDocumentSendToResponse(SendTo[] sendTos)
        {
            this.sendTos = sendTos;
        }

        override public String ToString()
        {
            return "GetDocumentSendToResponse( " +
                "sendTos=" + (sendTos == null ? "null" : "'" + String.Join( ", ", (Object[]) sendTos ) + "'") + 
                ")";
        }
    }
}
