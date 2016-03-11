using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GetDocument
{
     [DataContract(Namespace = "http://cnf/Integration")]
    public class GetConfirmationResponse
    {
        [DataMember(IsRequired = true)]
        ObjectFormatInd objectFormatInd { get; set; }

        [DataMember]
        bool confirmCompleteFlag { get; set; }

        [DataMember]
        String url { get; set; }

        [DataMember]
        byte[] objectStream { get; set; }

        public GetConfirmationResponse() { }

        public GetConfirmationResponse(ObjectFormatInd objectFormatInd, bool confirmCompleteFlag, String Url)
        {
            this.objectFormatInd = objectFormatInd;
            this.confirmCompleteFlag = confirmCompleteFlag;
            this.url = url;
        }

        public GetConfirmationResponse(ObjectFormatInd objectFormatInd, bool confirmCompleteFlag, byte[] objectStream)
        {
            this.objectFormatInd = objectFormatInd;
            this.confirmCompleteFlag = confirmCompleteFlag;
            this.objectStream = objectStream;
        }

        override public String ToString()
        {
            return "GetConfirmationResponse( " +
                 "objectFormatInd=" + objectFormatInd +
                 ", confirmCompleteFlag=" + confirmCompleteFlag +
                 ", url=" + (url == null ? "null" : "'" + url + "'") +
                 ", objectStreamLength=" + (objectStream == null ? "null" : "" + objectStream.Length) +
                 ")";
        }
    }
}
