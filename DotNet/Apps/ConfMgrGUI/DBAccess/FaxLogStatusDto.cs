using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class FaxLogStatusDto
    {
        public Int32 Id { get; set; }
        public Int32 TradeId { get; set; }
        public Int32 TradeRqmtConfirmId { get; set; }
        public string Sender { get; set; }
        public DateTime CrtdTsGmt { get; set; }
        public string FaxTelexInd { get; set; }
        public string FaxTelexNumber { get; set; }
        public string FaxStatus { get; set; }
        public string Cmt { get; set; }
    }
}
