using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class RqmtStatusDto
    {
        public string RqmtCode { get; set; }
        public string DisplayText { get; set; }
        public string InitialStatus { get; set; }
        public string StatusCode { get; set; }
        public Int32    Ord { get; set; }
        public string TerminalFlag { get; set; }
        public string ProblemFlag { get; set; }
        public string ColorCode { get; set; }
        public string Descr { get; set; }
    }
}
