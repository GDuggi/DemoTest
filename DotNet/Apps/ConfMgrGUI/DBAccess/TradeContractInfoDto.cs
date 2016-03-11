using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class TradeContractInfoDto
    {
        public string TrdSysCode { get; set; }
        public string TrdSysTicket { get; set; }
        public Int32  TradeRqmtConfirmId { get; set; }
        public DateTime TradeDt { get; set; }
        public string TemplateName { get; set; }
    }
}
