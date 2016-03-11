using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Counterparty
{
     [DataContract(Namespace = "http://cnf/Integration")]
    public class GetAgreementListResponse
    {
       
        [DataMember]
        Agreement[] agreements { get; set; }

        public GetAgreementListResponse()
        {
        }

        public GetAgreementListResponse(Agreement[] agreements)
        {
            this.agreements = agreements;
        }

        override public String ToString()
        {
            return "GetDocumentSendToRequest( " +
                "agreements=" + (agreements == null ? "null" : "'" + String.Join(", ", (Object[]) agreements ) + "'") +                                
                ")";
        }
    }
}
