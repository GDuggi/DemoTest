using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Counterparty
{

    [DataContract(Namespace = "http://cnf/Integration")]
    public class StoreDocumentSendToRequest
    {
        [DataMember(IsRequired = true)]
        public String tradingSystemCode { get; set; }

        [DataMember(IsRequired = true)]
        String cptyShortName { get; set; }

        [DataMember(IsRequired = true)]
        String documentTypeCode { get; set; }

        [DataMember(IsRequired = true)]
        String cdtyCode { get; set; }

        [DataMember(IsRequired = true)]
        String settlementTypeCode { get; set; }

        [DataMember(IsRequired = true)]
        List<SendTo> sendTos { get; set; }

        public StoreDocumentSendToRequest()
        {
        }

        public StoreDocumentSendToRequest(String tradingSystemCode, String cptyShortName, String documentTypeCode, String cdtyCode, String settlementTypeCode, List<SendTo> sendTos)
        {
            this.tradingSystemCode = tradingSystemCode;
            this.cptyShortName = cptyShortName;
            this.documentTypeCode = documentTypeCode;
            this.settlementTypeCode = settlementTypeCode;
            this.sendTos = sendTos;
        }

        override public String ToString()
        {
            return "GetDocumentSendToRequest( " +
                "tradingSystemCode=" + (tradingSystemCode == null ? "null" : "'" + tradingSystemCode + "'") +
                ", cptyShortName=" + (cptyShortName == null ? "null" : "'" + cptyShortName + "'") +
                ", documentTypeCode=" + (documentTypeCode == null ? "null" : "'" + documentTypeCode + "'") +
                ", cdtyCode=" + (cdtyCode == null ? "null" : "'" + cdtyCode + "'") +
                ", settlementType=" + (settlementTypeCode == null ? "null" : "'" + settlementTypeCode + "'") +
                ", sendTos=" + (sendTos == null ? "null" : String.Join( ", ", sendTos ) ) +
                ")";
        }

    }
}
