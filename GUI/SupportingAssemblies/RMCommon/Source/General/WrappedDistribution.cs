using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using com.amphora.cayenne.entity;
using DevExpress.XtraEditors.DXErrorProvider;
using java.util;
using NSRMLogging;
using org.apache.cayenne;
using org.apache.cayenne.exp;
using org.apache.cayenne.query;
using org.apache.log4j;
using JLIST = java.util.List;
using NSRiskManager.Constants;
using NSRMCommon;


namespace NSRiskMgrCtrls
{
    public class WrappedDistribution
    {
        #region fields
        TradeItemDist tid;
        #endregion

        #region ctor
        public WrappedDistribution(TradeItemDist atid) { tid = atid; }
        #endregion

        #region properties
        [DisplayName("Distribution Num")]
        public int distNum { get { return JavaDBUtil.copyInt(tid.getDistNum()); } }
        [DisplayName("Trade Num")]
        public int tradeNum { get { return JavaDBUtil.copyInt(tid.getTradeNum()); } }
        [DisplayName("Order Num")]
        public int orderNum { get { return JavaDBUtil.copyShort(tid.getOrderNum()); } }
        [DisplayName("Item Num")]
        public int itemNum { get { return JavaDBUtil.copyShort(tid.getItemNum()); } }

        [DisplayName("Risk Qty")]
        public double RiskQty { get { return getRiskQty(); } }
        [DisplayName("Physical Qty")]
        public double PhysicalQty { get { return getPhysicalQty(); } }



        [DisplayName("Discount Qty")]
        public double DiscountQty
        {
            get
            {
                double discQty = 0;
                if (SharedContext.isRightRiskMode(this.positionType.ToString(), false, false, true))
                    discQty = JavaDBUtil.copyDouble(tid.getDiscountQty());

                //if it's sell, negate the value
                if (psInd == 'S')
                {
                    discQty = 0 - discQty;
                }

                return discQty * lotSize; ;

            }
        }

   

        private double getRiskQty()
        {

            double riskQty = 0;
            
            if (SharedContext.isRightRiskMode(this.positionType.ToString(), true, false, false))
                riskQty = JavaDBUtil.copyDouble(tid.getDistQty()) - JavaDBUtil.copyDouble(tid.getAllocQty());

            //if it's sell, negate the value
            if (psInd == 'S')
            {
                riskQty = 0 - riskQty;
            }

            return riskQty * lotSize;

        }
        private double getPhysicalQty()
        {

            double physicalQuantity = 0;

            if (SharedContext.isRightRiskMode(this.positionType.ToString(), false, true, false))
                physicalQuantity = JavaDBUtil.copyDouble(tid.getDistQty()) - JavaDBUtil.copyDouble(tid.getAllocQty());

            //if it's sell, negate the value
            if (psInd == 'S')
            {
                physicalQuantity = 0 - physicalQuantity;
            }

            return physicalQuantity * lotSize;
        }

        [Browsable(false)]
        public double lotSize { get; set; }

        [Browsable(false)]
        public int decimalPrecision { get; set; }

        [Browsable(false)]
        public string finalUomToConvertTo { get; set; }

        [Browsable(false)]
        public string originalUomToConvertTo { get; set; }

        [Browsable(false)]
        public string commodityCode { get; set; }

        [Browsable(false)]
        public char positionType { get; set; }

        [Browsable(false)]
        public int accumNum { get { return JavaDBUtil.copyShort(tid.getAccumNum()); } }

        [Browsable(false)]
        public int qppNum { get { return JavaDBUtil.copyShort(tid.getQppNum()); } }

        [Browsable(false)]
        public char psInd { get { return JavaDBUtil.copyChar(tid.getPSInd()); } }

        [Browsable(false)]
        public int distType { get { return JavaDBUtil.copyChar(tid.getDistType()); } }

        [Browsable(false)]
        public char realSynthInd { get { return JavaDBUtil.copyChar(tid.getRealSynthInd()); } }

        [Browsable(false)]
        public bool isEquivInd { get { return JavaDBUtil.copyBool(tid.getIsEquivInd()); } }


        [Browsable(false)]

        public bool EquivSourceInd
        {
            get
            {
                if (tid.getEquivSourceInd() != null)
                    return JavaDBUtil.copyBool(tid.getEquivSourceInd().ToString());
                else
                    return false;
            }
        }

        [Browsable(false)]
        public int commktKey
        {
            get
            {
                var tradingPeriod = tid.getTradingPeriod();

                if (tradingPeriod == null)
                    return 0;

                var commodityMarketKey = tradingPeriod.getCommodityMarketKey();
                if (commodityMarketKey == null)
                    return 0;
                else
                    return commodityMarketKey.intValue();
            }
        }

        [Browsable(false)]
        public string tradingPeriod
        {
            get
            {
                string tmp;
                DateTime dt;

                if (tid.getTradingPeriod() == null)
                    return "NULL";
                tmp = tid.getTradingPeriod().getTradingPrdId();
                if (Regex.IsMatch(tmp, "[0-9]{6}W[1-4]"))
                    tmp = tmp.Substring(0, 6);
                dt = DateTime.ParseExact(tmp, "yyyyMM", null);
                return dt.ToString("MMM-yy");
            }
        }
        [Browsable(false)]
        public double distQty { get { return JavaDBUtil.copyDouble(tid.getDistQty()); } }
        [Browsable(false)]
        public double allocQty { get { return JavaDBUtil.copyDouble(tid.getAllocQty()); } }
        [Browsable(false)]
        public double deliveredQty { get { return JavaDBUtil.copyDouble(tid.getDeliveredQty()); } }

        [Browsable(false)]
        public string originalquantityUom
        {
            get;
            set;
        }
        [Browsable(false)]
        public double qtyUomConvRate { get { return JavaDBUtil.copyDouble(tid.getQtyUomConvRate()); } }
        [Browsable(false)]
        public double secConversionFactor { get { return JavaDBUtil.copyDouble(tid.getSecConversionFactor()); } }
        [Browsable(false)]
        public double pricedQty { get { return JavaDBUtil.copyDouble(tid.getPricedQty()); } }
        [Browsable(false)]
        public double priceCurrConvRate { get { return JavaDBUtil.copyDouble(tid.getPriceCurrConvRate()); } }
        [Browsable(false)]
        public DateTime businessDate { get { return JavaDBUtil.makeDatetime(tid.getBusDate()); } }
        [Browsable(false)]
        public int posNum { get { return JavaDBUtil.copyInt(tid.getPosNum()); } }
        [Browsable(false)]
        public int realPortNum { get { return JavaDBUtil.copyInt(tid.getRealPortNum()); } }
        [Browsable(false)]
        public int spreadPosGroupNum { get { return JavaDBUtil.copyInt(tid.getSpreadPosGroupNum()); } }
        [Browsable(false)]
        public double estimateQty { get { return JavaDBUtil.copyDecimal(tid.getEstimateQty()); } }

        [Browsable(false)]
        public DateTime plsAsOfDate { get { return JavaDBUtil.makeDatetime(tid.getPlAsofDate()); } }
        [Browsable(false)]
        public DateTime busDate { get { return JavaDBUtil.makeDatetime(tid.getBusDate()); } }
        [Browsable(false)]
        public char whatIfInd { get { return JavaDBUtil.copyChar(tid.getWhatIfInd()); } }
        [Browsable(false)]
        public double addlCostSum { get { return JavaDBUtil.copyDouble(tid.getAddlCostSum()); } }
        [Browsable(false)]
        public double closedPl { get { return JavaDBUtil.copyDouble(tid.getClosedPl()); } }
        [Browsable(false)]
        public double openPl { get { return JavaDBUtil.copyDouble(tid.getOpenPl()); } }
        [Browsable(false)]
        public int formulaBodyNum { get { return JavaDBUtil.copyInt(tid.getFormulaBodyNum()); } }
        [Browsable(false)]
        public int formulaNum { get { return JavaDBUtil.copyInt(tid.getFormulaNum()); } }
        [Browsable(false)]
        public int transId { get { return JavaDBUtil.copyInt(tid.getTransId()); } }
        #endregion



    }

}
