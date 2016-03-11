using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class VPcTradeRqmtDto
    {
        public Int64 CancelTradeNotifyId { get; set; } 
        public string Category { get; set; }
        public string Cmt { get; set; } 
        public DateTime CompletedDt { get; set; }
        public DateTime CompletedTimestampGmt { get; set; }
        public string DelphiConstant { get; set; }
        public string DisplayText { get; set; }
        public string FinalApprovalFlag { get; set; }
        public string GuiColorCode { get; set; }
        public Int64 Id { get; set; }
        public string PrelimAppr { get; set; }
        public string ProblemFlag { get; set; }
        public string Reference { get; set; }
        public string Rqmt { get; set; }
        public Int64 RqmtTradeNotifyId { get; set; }
        public string SecondCheckFlag { get; set; }
        public string Status { get; set; }
        public string TerminalFlag { get; set; }
        public Int64 TradeId { get; set; }
        public Int64 TransactionSeq { get; set; }
    }
}
