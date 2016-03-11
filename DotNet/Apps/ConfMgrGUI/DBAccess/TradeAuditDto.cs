using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class TradeAuditDto
    {
        public Int32 TradeId { get; set; }
        public Int32 TradeRqmtId { get; set; }
        public string Operation { get; set; }
        public string Rqmt { get; set; }
        public string Status { get; set; }
        public string Machine { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime CompletedDt { get; set; }
    }
}
