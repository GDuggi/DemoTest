using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class InboundDocCallerRefDto
    {
        public Int32 CallerRefId { get; set; }
        public string CallerRef { get; set; }
        public string CptyShortCode { get; set; }
        public string ActiveFlag { get; set; }
        public string RefType { get; set; }
    }
}
