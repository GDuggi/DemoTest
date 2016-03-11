using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VaultService
{

   [DataContract(Namespace = "http://dms/Integration")]     
   public class UploadDocumentRequest
    {

        //public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        //{
        //    get;
        //    set;
        //}

        //[DataMember(IsRequired = true)]
       [DataMember]
        public String TradingSystemCode { get; set; }

        [DataMember]
        public string DocumentKey
        {
            get;
            set;
        }

        [DataMember]
        public string DocumentType
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
        public string ObjectFormatInd
        {
            get;
            set;
        }

        [DataMember]
        public byte[] ObjectStream
        {
            get;
            set;
        }
    }
}
