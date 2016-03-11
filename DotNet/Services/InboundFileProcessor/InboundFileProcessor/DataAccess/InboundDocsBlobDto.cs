using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InboundFileProcessor.DataAccess
{
    public class InboundDocsBlobDto
    {
        public Int32 Id { get; set; }
        public Int32 RowId { get; set; }
        public float InboundDocsId { get; set; }
        public string OrigImageFileExt { get; set; }
        public byte[] OrigImageBlob { get; set; }
        public string MarkupImageFileExt { get; set; }
        public byte[] MarkupImageBlob { get; set; }
    }
}
