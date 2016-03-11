using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GetDocument
{
    [DataContract(Namespace = "http://cnf/Integration")]
    public enum ObjectFormatInd
    {
        [EnumMember]
        PDF,
        [EnumMember]
        DOCX,
        [EnumMember]
        HTML
    }
}
