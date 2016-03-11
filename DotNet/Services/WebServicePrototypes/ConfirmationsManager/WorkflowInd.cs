using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ConfirmationsManager
{
    [DataContract(Namespace = "http://cnf/Integration")]
    public enum WorkflowInd
    {
        [EnumMember]
        FINALAPPROVAL,
        [EnumMember]
        OURPAPER,
        [EnumMember]
        CPTYPAPER,
        [EnumMember]
        BROKERPAPER
    }
}