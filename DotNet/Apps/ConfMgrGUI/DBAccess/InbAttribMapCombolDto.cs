using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    //Used by InbAttribMapPhraseDal.GetPhrases(string pMappedValue)
    public class InbAttribMapComboDto
    {
        public Int32 MappedValId { get; set; }
        public Int32 PhraseId { get; set; }
        public string Phrase { get; set; }
        public string Descr { get; set; }
    }
}
