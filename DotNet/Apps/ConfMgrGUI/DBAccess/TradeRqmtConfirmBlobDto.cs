using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class TradeRqmtConfirmBlobDto
    {
        public Int32 Id { get; set; }
        public Int32 Rowid { get; set; }
        public Int32 TradeRqmtConfirmId { get; set; }
        public string ImageFileExt { get; set; }
        public byte[] DocBlob { get; set; }
    }
}
