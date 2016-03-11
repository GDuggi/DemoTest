using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ConfirmationsManager
{
    [DataContract( Namespace="http://cnf/ConfirmationsManager" )]
    public class GetPermissionKeysRequest
    {
        [DataMember(IsRequired = true)]
        String userId { get; set; }

        [DataMember(IsRequired = true)]
        String applicationName{ get; set; }

        public GetPermissionKeysRequest()
        {
        }

        public GetPermissionKeysRequest(String userId, String applicationName)
        {
            this.userId = userId;
            this.applicationName = applicationName;
        }

        override public String ToString()
        {
            return "GetPermissionKeysRequest( " +
                "userId=" + (userId == null ? "null" : "'" + userId + "'") +
                ", applicationName=" + (applicationName==null ? "null" : "'" + applicationName + "'" ) +                
                ")";
        }

    }
}