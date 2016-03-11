using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class TradeRqmtDto
    {
        public Int32 Id { get; set; }
        public Int32 TradeId { get; set; }
        public Int32 RqmtTradeNotifyId { get; set; }
        public string RqmtCode { get; set; }
        public string StatusCode { get; set; }
        public DateTime CompletedDt { get; set; }
        public DateTime CompletedTimestampGmt { get; set; }
        public string Reference { get; set; }
        public Int32 CancelTradeNotifyId { get; set; }
        public string Cmt { get; set; }
        public string SecondCheckFlag { get; set; }
        public string ForceCmtToNull { get; set; }
        public bool StatusDateSpecified { get; set; }

        public TradeRqmtDto()
        {
            this.ForceCmtToNull = "N";
            this.StatusDateSpecified = false;
        }
    }
}
