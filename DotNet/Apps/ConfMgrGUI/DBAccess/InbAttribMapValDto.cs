using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class InbAttribMapValDto
    {
        public Int32 Id { get; set; }
        public string InbAttribCode { get; set; }
        public string MappedValue { get; set; }
        public string Descr { get; set; }
        public string ActiveFlag { get; set; }
    }
}
