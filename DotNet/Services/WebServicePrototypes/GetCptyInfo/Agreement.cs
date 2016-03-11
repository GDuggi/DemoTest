using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Counterparty
{
    [DataContract(Namespace = "http://cnf/Integration", Name="agreement")]
    public class Agreement
    {
        [DataMember]
        public String agreementTypeCode { get; set; }

        public Agreement()
        {
        }

        public Agreement(String agreementTypeCode)
        {
            this.agreementTypeCode = agreementTypeCode;
        }

        
        override public String ToString()
        {
            return "Agreement( " +
                 "agreementTypeCode=" + (agreementTypeCode == null ? "null" : "'" + agreementTypeCode + "'") +                
                 ")";
        }

    }
}
