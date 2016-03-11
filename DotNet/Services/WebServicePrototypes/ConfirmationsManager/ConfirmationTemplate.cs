using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ConfirmationsManager
{
    [DataContract(Namespace = "http://cnf/Integration", Name = "confirmationTemplate")]
    public class ConfirmationTemplate
    {
        [DataMember(IsRequired = true, Order = 0)]
        public String templateName { get; set; }

        [DataMember(IsRequired = false, Order = 1)]
        public Attribute[] attributes { get; set; }


        public ConfirmationTemplate()
        {
        }

        public ConfirmationTemplate(String templateName, Attribute[] attributes)
        {
            this.templateName = templateName;
            this.attributes = attributes;
        }

        override public String ToString()
        {
            return "ConfirmationTemplate( " +
                "templateName =" + (templateName == null ? "null" : templateName) +
                ", attributes =" + (attributes == null ? "null" : "'" + String.Join(", ", (Object[])attributes) + "'") +
                ")";
        }
    }
}
