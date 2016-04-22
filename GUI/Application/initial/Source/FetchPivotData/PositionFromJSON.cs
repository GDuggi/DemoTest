using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NSRMCommon;

/*
namespace NSRiskManager {
    class PositionFromJSON : IPosition {
        #region Melvin's properties
        public string tradingPeriod {
            get { return ((IPosition)this).tradingPrd; }
            set {
                DateTime dt;

                if (Regex.IsMatch(value,"[A-Za-z]{3}\\-[0-9]{2}")) {
                    dt = DateTime.ParseExact(value,"MMM-yy",null);
                } else if (Regex.IsMatch(value,"^[0-9]{6}$")) {
                    dt = DateTime.ParseExact(value,"yyyyMM",null);
                } else
                    dt = new DateTime(2035,1,1);
                ((IPosition)this).tradingPrd = dt.ToString("yyyyMM");
            }
        }

        public char isHedgeInd { get { return ((IPosition)this).isHedge ? 'Y' : 'N'; } set { ((IPosition)this).isHedge = (value == 'Y'); } }
        public char posType { get { return ((IPosition)this).posType[0]; } set { ((IPosition)this).posType = (value == char.MinValue) ? string.Empty : new string(value,1); } }
        public char putCallInd { get { return ((IPosition)this).putCallInd[0]; } set { ((IPosition)this).putCallInd = (value == char.MinValue) ? string.Empty : new string(value,1); } }
        public double discountQty { get { return ((IPosition)this).discountedQty; } set { ((IPosition)this).discountedQty = (value == double.MinValue) ? 0 : value; } }
        public double longQty { get { return ((IPosition)this).longQty; } set { ((IPosition)this).longQty = (value == double.MinValue) ? 0 : value; } }
        public double shortQty { get { return ((IPosition)this).shortQty; } set { ((IPosition)this).shortQty = (value == double.MinValue) ? 0 : value; } }
        public double secDiscountQty { get { return ((IPosition)this).secondaryDiscountQty; } set { ((IPosition)this).secondaryDiscountQty = (value == double.MinValue) ? 0 : value; } }
        public double secLongQty { get { return ((IPosition)this).secondaryLongQty; } set { ((IPosition)this).secondaryLongQty = (value == double.MinValue) ? 0 : value; } }
        public double secShortQty { get { return ((IPosition)this).secondaryShortQty; } set { ((IPosition)this).secondaryShortQty = (value == double.MinValue) ? 0 : value; } }
        public double strikePrice { get { return ((IPosition)this).strikePrice; } set { ((IPosition)this).strikePrice = (value == double.MinValue) ? 0 : value; } }
        public double quantity { get { return double.MinValue; } set { Debug.Print("found quantity=" + value); } }

        public int portId { get { return ((IPosition)this).portNum; } set { ((IPosition)this).portNum = value; } }
        public int posNum { get { return ((IPosition)this).posNum; } set { ((IPosition)this).posNum = value; } }
        public int transactionId { get { return ((IPosition)this).transactionId; } set { ((IPosition)this).transactionId = value; } }
        public string commodityCode { get { return ((IPosition)this).cmdtyCode; } set { ((IPosition)this).cmdtyCode = value; } }
        public string marketCode { get { return ((IPosition)this).mktCode; } set { ((IPosition)this).mktCode = value; } }
        public string qtyUom { get { return ((IPosition)this).qtyUom; } set { ((IPosition)this).qtyUom = string.IsNullOrEmpty(value) ? string.Empty : value; } }
        public string strikePriceUom { get { return ((IPosition)this).strikeUom; } set { ((IPosition)this).strikeUom = string.IsNullOrEmpty(value) ? string.Empty : value; } }

        public string optExpDate { get { return ((IPosition)this).expiryDate.ToString(); } set { NSRMLogging.Util.show(System.Reflection.MethodBase.GetCurrentMethod()); } }
        public DateTime optStartDate { get; set; }
        public char isClearedInd { get; set; }
        public char isEquivInd { get; set; }
        public char optPeriodicity { get; set; }
        public char optionType { get; set; }
        public char whatIfInd { get; set; }
        public decimal lastMtmPrice { get; set; }
        public double avgPurchPrice { get; set; }
        public double avgSalePrice { get; set; }
        public double priceQty { get; set; }
        public double rolledQty { get; set; }
        public double secPriceQty { get; set; }
        public double secPricedQty { get; set; }
        public double secRolledQty { get; set; }
        public int commktKey { get; set; }
        public int formulaBodyNum { get; set; }
        public int formulaNum { get; set; }
        public string acctShortName { get; set; }
        public string desiredOptEvalMethod { get; set; }
        public string formulaName { get; set; }
        public string optPrice { get; set; }
        public string otcOption { get; set; }
        public string posStatus { get; set; }
        public string priceCurrency { get; set; }
        public string priceQtyUom { get; set; }
        [JsonProperty("secPriceQtyUom")]
        public string secPriceUom { get; set; } // JsonProperty: secPriceQtyUom
        public string settlementType { get; set; }
        public string strikePriceCurrency { get; set; }

        #endregion Melvin's properties

        #region IPosition implementation
        double IPosition.longQty { get; set; }
        double IPosition.shortQty { get; set; }
        double IPosition.discountQty { get; set; }
        double IPosition.secondaryLongQty { get; set; }
        double IPosition.secondaryShortQty { get; set; }
        double IPosition.secondaryDiscountQty { get; set; }

        int IPosition.portNum { get; set; }
        bool IPosition.isHedge { get; set; }
        string IPosition.cmdtyCode { get; set; }
        string IPosition.mktCode { get; set; }
        string IPosition.posType { get; set; }
        string IPosition.tradingPrd { get; set; }
        string IPosition.putCallInd { get; set; }
        string IPosition.priceUom { get; set; }
        string IPosition.qtyUom { get; set; }
        string IPosition.secondaryPosUom { get; set; }
        string IPosition.strikeUom { get; set; }
        double IPosition.strikePrice { get; set; }
        int IPosition.posNum { get; set; }
        int IPosition.transactionId { get; set; }

        // derived
        DateTime IPosition.expiryDate { get; set; }
        double IPosition.riskQty { get; set; }
        double IPosition.physQty { get; set; }
        double IPosition.discountedQty { get; set; }
        bool IPosition.isZero { get; set; }
        bool IPosition.isEquivInd { get; set; }
        bool IPosition.equivSource { get; set; }
        #endregion IPosition implementation
    }
}
*/