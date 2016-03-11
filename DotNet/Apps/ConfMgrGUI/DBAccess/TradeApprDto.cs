using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class TradeApprDto
    {
        public Int32 TradeId { get; set; }
        public string ApprovalFlag { get; set; }
        public string OnlyIfReadyFlag { get; set; }
        public string ApprByUserName { get; set; }

    }
}
