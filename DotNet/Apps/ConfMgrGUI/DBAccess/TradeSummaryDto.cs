using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class TradeSummaryDto
    {
        public Int32 Id { get; set; }
        public Int32 TradeId { get; set; }
        public string CptyTradeId { get; set; }
        public string OpenRqmtsFlag { get; set; }
        public string Category { get; set; }
        public DateTime LastUpdateTimestampGmt { get; set; }
        public string FinalApprovalFlag { get; set; }
        public string Cmt { get; set; }
        public DateTime LastTrdEditTimestampGmt { get; set; }
        public string OpsDetActFlag { get; set; }
        public string ReadyForFinalApprovalFlag { get; set; }
        public string HasProblemFlag { get; set; }
        public Int32 TransactionSeq { get; set; }
        public DateTime FinalApprovalTimestampGmt { get; set; }
    }
}
