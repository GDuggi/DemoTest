using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VaultService
{
    [DataContract(Namespace = "http://dms/Integration")]     
    public class UploadDocumentResponse
    {
        //public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        //{
        //    get;
        //    set;
        //}

        //[DataMember(IsRequired = true)]
        [DataMember]
        public string Status
        {
            get;
            set;
        }

        //[DataMember(IsRequired = true)]
        [DataMember]
        public string URL
        {
            get;
            set;
        }
    }
}
