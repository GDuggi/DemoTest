using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InboundFileProcessor
{
    public class InboundDocsDto
    {
        public Int32 Unresolvedcount { get; set; }
        public Int64 Id { get; set; }
        public string CallerRef { get; set; }
        public string SentTo { get; set; }
        public DateTime RcvdTs { get; set; }
        public string FileName { get; set; }
        public string Sender { get; set; }
        public string Cmt { get; set; }
        public string DocStatusCode { get; set; }
        public string HasAutoAsctedFlag { get; set; }
        public string ProcFlag { get; set; }
        public string Tradeids { get; set; }
        public string MappedCptySn { get; set; }
        public string MappedBrkrSn { get; set; }
        public string MappedCdtyCode { get; set; }
        public string JobRef { get; set; }

        //Non-DB fields
        public string IgnoreFlag { get; set; }
        public string BookmarkFlag { get; set; }
        public string CommentFlag { get; set; }
        public string CommentUser { get; set; }
    }
}
