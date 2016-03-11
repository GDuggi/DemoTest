using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ConfirmationsManager
{
    [DataContract(Namespace = "http://cnf/Integration")]
    public class GetConfirmationTemplatesResponse
    {
        [DataMember(IsRequired = true)]
        public ConfirmationTemplate[] confirmationTemplates { get; set; }

        public GetConfirmationTemplatesResponse()
        {
        }

        public GetConfirmationTemplatesResponse(ConfirmationTemplate[] confirmationTemplates)
        {
            this.confirmationTemplates = confirmationTemplates;
        }

        override public String ToString()
        {
            return "GetConfirmationTemplatesResponse( " +
                "confirmationTemplates=" + (confirmationTemplates == null ? "null" : "'" + String.Join(", ", (Object[])confirmationTemplates) + "'") + 
                ")";
        }

    }
}

