using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VaultService
{
    [DataContract(Namespace = "http://dms/Integration")]     
    public class GetDocumentForURLRequest
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get;
            set;
        }

        [DataMember(IsRequired = true)]
        public String TradingSystemCode { get; set; }

        [DataMember]
        public string DocumentKey
        {
            get;
            set;
        }

        [DataMember]
        public string FeedType
        {
            get;
            set;
        }

       [DataMember]
        public string FileName
        {
            get;
            set;
        }

        [DataMember]
        public string URL
        {
            get;
            set;
        }
    }
}
