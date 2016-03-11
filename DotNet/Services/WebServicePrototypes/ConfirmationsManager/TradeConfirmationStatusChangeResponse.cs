using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ConfirmationsManager
{
    [DataContract(Namespace = "http://cnf/Integration")]
    public class TradeConfirmationStatusChangeResponse
    {
        [DataMember(IsRequired = true)]
        public bool success { get; set; }

        public TradeConfirmationStatusChangeResponse()
        {

        }

        public TradeConfirmationStatusChangeResponse(Boolean success)
        {
            this.success = success;
        }

        override public String ToString()
        {
            return "TradeConfirmationStatusChangeResponse( " +
                "success =" + success +
                ")";
        }
    }
}
