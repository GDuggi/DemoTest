using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VaultService.DataContracts;

namespace VaultService
{
    [DataContract(Namespace = "http://dms/Integration")]    
    public class GetVersionInfoForDocumentRequest
    {
        [DataMember]
        public ContractInfo DocInfo
        {
            get;
            set;
        }

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
        public string TradingSystemCode
        {
            get;
            set;
        }
    }
}
