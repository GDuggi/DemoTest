using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ConfirmationsManager
{
    [DataContract(Namespace = "http://cnf/Integration")]
    public class GetPermissionKeysResponse
    {
        [DataMember(IsRequired = false,EmitDefaultValue=false,Order=0)]
        public bool superUserFlag { get; set; }

        [DataMember(IsRequired = true,Order = 1)]
        public String[] permissionKeyCodes { get; set; }

        public GetPermissionKeysResponse()
        {
        }

        public GetPermissionKeysResponse(bool superUserFlag, String[] permissionKeyCodes)
        {
            this.superUserFlag = superUserFlag;
            this.permissionKeyCodes = permissionKeyCodes;
        }

        override public String ToString()
        {
            return "GetPermissionKeysResponse( " +
                "superUserFlag=" + superUserFlag +
                ", permissionKeyCodes=" + (permissionKeyCodes == null ? "null" : "'" + String.Join(", ", (Object[])permissionKeyCodes) + "'") + 
                ")";
        }
    }
}
