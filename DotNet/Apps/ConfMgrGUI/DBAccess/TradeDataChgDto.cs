using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class TradeDataChgDto
    {
        public DateTime JnDatetime { get; set; }
        //public Int32 BookingCoId { get; set; }
        public string BookingCoSn { get; set; }
        public string Book { get; set; }
        //public Int32 BrokerId { get; set; }
        public string BrokerLegalName { get; set; }
        public string BrokerPrice { get; set; }
        public string BrokerSn { get; set; }
        public string BuySellInd { get; set; }
        public string CdtyCode { get; set; }
        public string CdtyGrpCode { get; set; }
        //public Int32 CptyId { get; set; }
        public string CptyLegalName { get; set; }
        public string CptySn { get; set; }
        public DateTime EndDt { get; set; }
        public DateTime InceptionDt { get; set; }
        public string LocationSn { get; set; }
        public string OptnPremPrice { get; set; }
        public string OptnPutCallInd { get; set; }
        public string OptnStrikePrice { get; set; }
        public string PermissionKey { get; set; }
        public string PriceDesc { get; set; }
        public string ProfitCenter { get; set; }
        public string QtyDesc { get; set; }
        public float QtyTot { get; set; }
        public string RefSn { get; set; }
        public DateTime StartDt { get; set; }
        public string SttlType { get; set; }
        public string Trader { get; set; }
        public string TradeDesc { get; set; }
        public DateTime TradeDt { get; set; }
        public Int32 TradeId { get; set; }
        public string TradeStatCode { get; set; }
        public string TradeTypeCode { get; set; }
        public string TransportDesc { get; set; }
        public string Xref { get; set; }

    }
}
