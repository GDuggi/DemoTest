using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class InbAttribMapPhraseDto
    {
        public Int32 Id { get; set; }
        public Int32 InbAttribMapValId { get; set; }
        public string Phrase { get; set; }
        public string ActiveFlag { get; set; }
    }
}
