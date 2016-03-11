using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ConfirmationsManager
{
    [DataContract(Namespace = "http://cnf/Integration", Name = "attribute")]
    public class Attribute
    {
        [DataMember(IsRequired = true, Order=0)]
        public String name  { get; set; }

        [DataMember(IsRequired = true, Order = 1)]
        public String value { get; set; }

        public Attribute()
        {
        }

        public Attribute( String name, String value )
        {
            this.name = name;
            this.value = value;
        }

        override public String ToString()
        {
            return "Attribute( " +
                "name =" + (name == null ? "null" : name) +
                ", value =" + (value == null ? "value" : name) +
                ")";
        }

    }
}