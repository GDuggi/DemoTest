using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Counterparty
{
     [DataContract(Namespace = "http://cnf/Integration")]
    public class GetAgreementListRequest
    {

        [DataMember(IsRequired = true)]
        String tradingSystemCode { get; set; }

        [DataMember(IsRequired = true)]
        String bookingCompanyShortName { get; set; }

        [DataMember(IsRequired = true)]
        String cptyShortName { get; set; }

        public GetAgreementListRequest()
        {

        }

        public GetAgreementListRequest(String tradingSystemCode, String bookingCompanyShortName, String cptyShortName)
        {
            this.tradingSystemCode = tradingSystemCode;
            this.bookingCompanyShortName = bookingCompanyShortName;
            this.cptyShortName = cptyShortName;
        }

        override public String ToString()
        {
            return "GetAgreementListRequest( " +
                "tradingSystemCode=" + (tradingSystemCode == null ? "null" : "'" + tradingSystemCode + "'") +
                ", bookingCompanyShortName=" + (bookingCompanyShortName==null ? "null" : "'" + bookingCompanyShortName + "'" ) +
                ", cptyShortName=" + (cptyShortName == null ? "null" : "'" + cptyShortName + "'" ) + 
                ")";
        }

    }
}
