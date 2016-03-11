using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class AssociatedDocsDto
    {
        public Int32 Id { get; set; }
        public Int32 InboundDocsId { get; set; }
        public string TradeFinalApprovalFlag { get; set; }
        public Int32 IndexVal { get; set; }
        public string FileName { get; set; }
        public Int32 TradeId { get; set; }
        public string DocStatusCode { get; set; }
        public string AssociatedBy { get; set; }
        public DateTime AssociatedDt { get; set; }
        public string FinalApprovedBy { get; set; }
        public DateTime FinalApprovedDt { get; set; }
        public string DisputedBy { get; set; }
        public DateTime DisputedDt { get; set; }
        public string DiscardedBy { get; set; }
        public DateTime DiscardedDt { get; set; }
        public string VaultedBy { get; set; }
        public DateTime VaultedDt { get; set; }
        public string CdtyGroupCode { get; set; }
        public string CptySn { get; set; }
        public string BrokerSn { get; set; }
        public string DocTypeCode { get; set; }
        public string SecValidateReqFlag { get; set; }
        public Int32 TradeRqmtId { get; set; }
        public string XmitStatusCode { get; set; }
        public string XmitValue { get; set; }
        public string SentTo { get; set; }

        //Only used by PKG_INBOUND$p_update_asso_status
        public string RqmtStatus { get; set; }

    }
}
