using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Counterparty
{
    [DataContract(Namespace = "http://cnf/Integration")]
    enum TransmitMethodInd
    {
        [EnumMember]
        FAX,
        [EnumMember]
        EMAIL
    }
}
