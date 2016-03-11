using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Counterparty
{
    [DataContract(Namespace = "http://cnf/Integration")]
    public class StoreDocumentSendToResponse
    {
        [DataMember]
        public bool success { get; set; }

        public StoreDocumentSendToResponse()
        {

        }

        public StoreDocumentSendToResponse(bool success)
        {
            this.success = success;
        }

        override public String ToString()
        {
            return "StoreDocumentSendToResponse( " +
                "success=" + success +              
                ")";
        }
    }
}
