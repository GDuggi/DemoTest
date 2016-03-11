using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class RqmtDto
    {
        public string Code { get; set; }
        public string Descr { get; set; }
        public string Category { get; set; }
        public string InitialStatus { get; set; }
        public string DisplayText { get; set; }
        public string ActiveFlag { get; set; }
        public string DetActRqmtFlag { get; set; }
    }
}
