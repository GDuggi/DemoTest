using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GetDocument
{
     [DataContract(Namespace = "http://cnf/Integration")]
    public class GetDealSheetResponse
    {

        [DataMember(IsRequired = true)]
        ObjectFormatInd objectFormatInd { get; set; }

        [DataMember]
        String url { get; set; }

        [DataMember]
        byte[] objectStream { get; set; }

        public GetDealSheetResponse() { }

        public GetDealSheetResponse(ObjectFormatInd objectFormatInd, String Url)
        {
            this.objectFormatInd = objectFormatInd;
            this.url = url;
        }

        public GetDealSheetResponse(ObjectFormatInd objectFormatInd, byte[] objectStream)
        {
            this.objectFormatInd = objectFormatInd;
            this.objectStream = objectStream;
        }

        override public String ToString()
        {
            return "GetDealSheetResponse( " +
                 "documentInd=" + objectFormatInd +
                 ", url=" + (url == null ? "null" : "'" + url + "'")+
                 ", objectStreamLength=" + (objectStream == null ? "null" : "" + objectStream.Length) +
                 ")";
        }
    }
}

