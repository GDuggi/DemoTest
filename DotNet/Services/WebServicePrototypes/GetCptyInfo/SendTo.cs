using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Counterparty
{
    [DataContract(Namespace = "http://cnf/Integration", Name="sendTo")]
    public class SendTo
    {
        [DataMember( IsRequired=true) ]
        TransmitMethodInd transmitMethodInd { get; set; }
        [DataMember]
        String emailAddress { get; set; }
        [DataMember]
        String faxCountryCode { get; set; }
        [DataMember]
        String faxAreaCode { get; set; }
        [DataMember]
        String faxLocalNumber { get; set; }

        public SendTo(String emailAddress)
        {
            this.transmitMethodInd = TransmitMethodInd.EMAIL;
            this.emailAddress = emailAddress;
        }

        public SendTo(String faxCountryCode, String faxAreaCode, String faxLocalNumber )
        {
            this.transmitMethodInd = TransmitMethodInd.FAX;
            this.faxCountryCode = faxCountryCode;
            this.faxAreaCode = faxAreaCode;
            this.faxLocalNumber = faxLocalNumber;
        }

        override public String ToString()
        {
            return "SendTo( "
                + "transmitMethodInd=" + transmitMethodInd + ", "                
                + "faxCountryCode=" + (faxCountryCode == null ? "null" : "'" + faxCountryCode + "'") + ", "
                + "faxAreaCode="+(faxAreaCode == null ? "null" : "'" + faxAreaCode + "'") + ", "
                + "faxLocalNumber=" + (faxLocalNumber == null ? "null" : "'" + faxLocalNumber + "'") + ")";
        }
    }
}
