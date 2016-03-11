using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class XmitResultDto
    {
        public Int32 XmitResultId { get; set; }
        public Int32 XmitRequestId { get; set; }
        public Int32 AssociatedDocsId { get; set; }
        public Int32 TradeRqmtConfirmId { get; set; }
        public string SentByUser { get; set; }
        public string XmitStatusInd { get; set; }
        public string XmitMethodInd { get; set; }
        public string XmitDest { get; set; }
        public string XmitCmt { get; set; }
        public DateTime XmitTimestamp { get; set; }
    }
}
