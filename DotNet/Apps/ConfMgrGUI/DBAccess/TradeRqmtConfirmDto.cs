using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class TradeRqmtConfirmDto
    {
        public string ActiveFlag { get; set; }
        public string ConfirmCmt { get; set; }
        public string ConfirmLabel { get; set; }
        public string FaxTelexInd { get; set; }
        public string FaxTelexNumber { get; set; }
        public string FinalApprovalFlag { get; set; }
        public Int32 Id { get; set; }
        public string NextStatusCode { get; set; }
        public Int32 RqmtId { get; set; }
        public string TemplateCategory { get; set; }
        public Int32 TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateTypeInd { get; set; }
        public Int32 TradeId { get; set; }
        public string XmitAddr { get; set; }
        public string XmitCmt { get; set; }
        public string XmitStatusInd { get; set; }
        public DateTime XmitTimestampGmt { get; set; }
    }
}
