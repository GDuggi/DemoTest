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
    public class GetVersionInfoForResponse
    {
        [DataMember]
        public ContractInfo[] QueryResult
        {
            get;
            set;
        }
    }
}
