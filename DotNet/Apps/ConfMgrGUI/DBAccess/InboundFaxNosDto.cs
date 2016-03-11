using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class InboundFaxNosDto
    {
        public string Faxno { get; set; }
        public string LocCode { get; set; }
        public string ActiveFlag { get; set; }
        public string MigrateInd { get; set; }
    }
}
