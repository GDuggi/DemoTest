using System;
using System.Diagnostics;
using System.Reflection;
using NSRMLogging;

/*

namespace NSRMCommon {
    //#warning left off here.  this needs work.
    public class PosQtyInfo {
        public double shortQty { get; private set; }
        public double longQty { get; private set; }
        public double discountQty { get; private set; }
        // need currency
        // neef Uom 
    }

    public class PosOptData {
        public string type { get; private set; }
        public DateTime expireDate { get; private set; }
        public DateTime startDate { get; private set; }
        public string periodicity { get; private set; }
        public string priceSrcCode { get; private set; }
        public string putCall { get; private set; }
    }

    public class JSPosition : IPosition {
          com.amphora.cayenne.entity.Position position;


        #region ctors
        public JSPosition() { }

        public JSPosition(IPosition ipd)
            : this() {
            PositionFieldUtil.cloneFields(this,ipd);
        }
    
        public JSPosition(string cmdtyCode,string mktCode,string tradingPrd,int portNum,Random rnd) {
            Util.show(MethodBase.GetCurrentMethod(),"implement this!");
        }

        public JSPosition(com.amphora.cayenne.entity.Position position) {
            IPosition ithis = this as IPosition;

            this.position = position;
            ithis.posType = position.getPosType();
            ithis.discountQty = ithis.discountedQty = position.getDiscountQty().doubleValue();
            ithis.isEquivInd = string.Compare(position.getIsEquivInd(),"Y",true) == 0;
            ithis.isHedge = string.Compare(position.getIsHedgeInd(),"Y",true) == 0;
            ithis.longQty = position.getLongQty().doubleValue();
            ithis.putCallInd = position.getPutCallInd();
            ithis.posNum = position.getPosNum().intValue();
            ithis.priceUom = position.getPriceUom().getUomCode();
            ithis.qtyUom = position.getQtyUom().getUomCode();
            ithis.portNum = position.getRealPortfolio().getPortNum().intValue();
            ithis.secondaryDiscountQty = position.getSecDiscountQty().doubleValue();
            ithis.secondaryLongQty = position.getSecLongQty().doubleValue();
            ithis.secondaryShortQty = position.getSecShortQty().doubleValue();
            ithis.secondaryPosUom = position.getSecUom().getUomCode();

            ithis.shortQty = position.getShortQty().doubleValue();
            ithis.tradingPrd = position.getTradingPrd();
            ithis.transactionId = position.getTransId().intValue();
            ithis.cmdtyCode = position.getCommodityCode();
            ithis.mktCode = position.getMarket().getMktCode();
            if (string.Compare(ithis.posType,"X",true) == 0) {
                ithis.strikePrice = position.getStrikePrice() == null ? -1 : position.getStrikePrice().doubleValue();
                ithis.strikeUom = position.getStrikePriceUom() == null ? "NONE" : position.getStrikePriceUom().getUomCode();
                ithis.putCallInd = position.getPutCallInd();
                // INVESTIGATE: set expiryDate here -- position.getOptExpDate=
                // INVESTIGATE:             position.getOptionType()
                //                Util.show(MethodBase.GetCurrentMethod(),"OPTION here!");
            }
            ithis.commktKey = position.getCommktKey().intValue();
        }
        #endregion

        #region methods
        public override string ToString() {
            return GetType().Name + " " +
                ((IPosition)this).posNum + " " +
                ((IPosition)this).riskQty + " " +
                ((IPosition)this).tradingPrd + " " +
                ((IPosition)this).cmdtyCode + " " +
                ((IPosition)this).mktCode + " " +
                ((IPosition)this).transactionId;
        }

        #endregion

        #region IPosition implementation
        double IPosition.longQty { get; set; }
        double IPosition.shortQty { get; set; }
        double IPosition.discountQty { get; set; }
        double IPosition.secondaryLongQty { get; set; }
        double IPosition.secondaryShortQty { get; set; }
        double IPosition.secondaryDiscountQty { get; set; }
        int IPosition.portNum { get; set; }
        int IPosition.commktKey { get; set; }
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