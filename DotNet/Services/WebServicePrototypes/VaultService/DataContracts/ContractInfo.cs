using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VaultService.DataContracts
{
    [DataContract(Namespace = "http://dms/Integration")]  
    public class ContractInfo
    {
        [DataMember]
        public string BookingCompShortName
        {
            get;
            set;
        }

        [DataMember]
        public string CmdtyGroup
        {
            get;
            set;
        }

        [DataMember]
        public int DocID
        {
            get;
            set;
        }

        [DataMember]
        public string DocName
        {
            get;
            set;
        }

        [DataMember]
        public string DocType
        {
            get;
            set;
        }

        [DataMember]
        public string FileType
        {
            get;
            set;
        }

        [DataMember]
        public bool IsLatestVersion
        {
            get;
            set;
        }

        [DataMember]
        public string ProfitCenter
        {
            get;
            set;
        }

        [DataMember]
        public string Source
        {
            get;
            set;
        }

        [DataMember]
        public int TradeNum
        {
            get;
            set;
        }

        [DataMember]
        public string TraderInit
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

        [DataMember]
        public string VersionNum
        {
            get;
            set;
        }        
    }
}
