using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class FaxLogSentDto
    {
        public Int32 Id { get; set; }
        public string DocType { get; set; }
        public Int32 TradeId { get; set; }
        public string Sender { get; set; }
        public DateTime CrtdTsGmt { get; set; }
        public string FaxTelexCode { get; set; }
        public string FaxTelexNumber { get; set; }
        public string DocRefCode { get; set; }
    }
}
