using System;
using NSRMCommon;
using com.amphora.entities;

namespace NSRMCommon
{
    public class RiskGroupAtom
    {

        #region ctor
        
        public RiskGroupAtom(IPositionEntity iposEnty)
        {
            this.cmdtyCode = iposEnty.getCommodityCode();
            this.discountQty = iposEnty.getDiscountQty().doubleValue();
            this.isFuturesEquivalent = (iposEnty.getIsEquivInd() != null && iposEnty.getIsEquivInd().Equals("Y") && iposEnty.getEquivSourceInd() != null && iposEnty.getEquivSourceInd().Equals("Y"));
            this.isEquivInd = (iposEnty.getIsEquivInd() != null && iposEnty.getIsEquivInd().Equals("Y"));
            this.equivSource = (iposEnty.getEquivSourceInd() != null && iposEnty.getEquivSourceInd().Equals("Y"));
            this.isHedgeInd = iposEnty.getIsHedgeInd().Equals("Y");
            this.longQty = iposEnty.getLongQty().doubleValue();
            this.commktKey = iposEnty.getCommktKey().intValue();
            try
            {
                this.mktCode = iposEnty.getMarketCode();
            }
            catch (Exception e)
            {
                this.mktCode = null;
            }
            this.portNum = iposEnty.getPortId().intValue();
            this.posNum = iposEnty.getPosNum().intValue();
            this.posType = iposEnty.getPosType();
            this.putCallInd = iposEnty.getPutCallInd();
            this.qtyUom = iposEnty.getQtyUomCode();
            this.shortQty = iposEnty.getShortQty().doubleValue();
            this.strikePrice = iposEnty.getStrikePrice() != null ? iposEnty.getStrikePrice().doubleValue() : 0.0; 
            this.tradingPrd = LocalPeriodUtil.datetimeFrom(iposEnty.getTradingPeriod());
            this.transId = iposEnty.getTransactionId().intValue();
            this.optionType = iposEnty.getOptionType();

           
        }
        #endregion

        #region properties
        public DateTime tradingPrd { get; private set; }
        public bool isFuturesEquivalent { get; private set; }
        public bool isHedgeInd { get; private set; }
        public double discountQty { get; private set; }
        public double longQty { get;  set; }
        public double shortQty { get;  set; }
        public double asOfRiskQty { get; set; }
        public double asOfPhyQty { get; set; }
        public double strikePrice { get; private set; }
        public int portNum { get; private set; }
        public int posNum { get; private set; }
        public int transId { get; private set; }
        public string cmdtyCode { get; private set; }
        public string mktCode { get; private set; }
        public int commktKey { get; private set; }
        public string posType { get; private set; }
        public string putCallInd { get; private set; }
        public string qtyUom { get;  set; }
        public bool isEquivInd { get; private set; }
        public bool equivSource { get; private set; }
        public string optionType { get; private set; }
        #endregion

        #region methods
        public override string ToString() {
            return posNum +
                " Type  =" + posType +
                " Long  =" + longQty +
                " Short =" + shortQty +
                (this.isFuturesEquivalent ? " [FUT-EQU]" : string.Empty);
        }
        #endregion
    }
}