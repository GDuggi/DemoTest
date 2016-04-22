using System;
using com.amphora.cayenne.entity;
//using NSRiskMgrCtrls;
namespace NSRMCommon {
    public class CSProfitLossData : IPLRecord {
        #region ctor
        internal CSProfitLossData(PortfolioProfitLoss portfolioProfitLoss) {
            closedHedgePL = JavaDBUtil.copyDouble(portfolioProfitLoss.getClosedHedgePl());
            closedPhysPL = JavaDBUtil.copyDouble(portfolioProfitLoss.getClosedPhysPl());
            isCompYearEnd = JavaDBUtil.copyBool(portfolioProfitLoss.getIsCompyrEndInd());
            isMonthEnd = JavaDBUtil.copyBool(portfolioProfitLoss.getIsMonthEndInd());
            isOfficialRun = JavaDBUtil.copyBool(portfolioProfitLoss.getIsOfficialRunInd());
            isWeekend = JavaDBUtil.copyBool(portfolioProfitLoss.getIsWeekEndInd());
            isYearEnd = JavaDBUtil.copyBool(portfolioProfitLoss.getIsYearEndInd());
            liqClosedHedgePL = JavaDBUtil.copyDouble(portfolioProfitLoss.getLiqClosedHedgePl());
            liqClosedPhysPL = JavaDBUtil.copyDouble(portfolioProfitLoss.getLiqClosedPhysPl());
            liqOpenHedgePL = JavaDBUtil.copyDouble(portfolioProfitLoss.getLiqOpenHedgePl());
            liqOpenpPhysPL = JavaDBUtil.copyDouble(portfolioProfitLoss.getLiqOpenPhysPl());
            openHedgePL = JavaDBUtil.copyDouble(portfolioProfitLoss.getOpenHedgePl());
            openPhysPL = JavaDBUtil.copyDouble(portfolioProfitLoss.getOpenPhysPl());
            otherPL = JavaDBUtil.copyDouble(portfolioProfitLoss.getOtherPl());
            passRunId = JavaDBUtil.copyInt(portfolioProfitLoss.getPassRunDetailId());
            plAsOfDate = JavaDBUtil.makeDatetime(portfolioProfitLoss.getPlAsOfDate());
            plCalcDate = JavaDBUtil.makeDatetime(portfolioProfitLoss.getPlCalcDate());
            portNum = JavaDBUtil.copyInt(portfolioProfitLoss.getPortNum());
            totalPnLSecCost = JavaDBUtil.copyDecimal(portfolioProfitLoss.getTotalPlNoSecCost());
            transId = JavaDBUtil.copyInt(portfolioProfitLoss.getTransId());
        }
        #endregion

        #region properties
        public bool isCompYearEnd { get; private set; }
        public bool isMonthEnd { get; private set; }
        public bool isOfficialRun { get; private set; }
        public bool isWeekend { get; private set; }
        public bool isYearEnd { get; private set; }
        public double closedHedgePL { get; private set; }
        public double closedPhysPL { get; private set; }
        public double liqClosedHedgePL { get; private set; }
        public double liqClosedPhysPL { get; private set; }
        public double liqOpenHedgePL { get; private set; }
        public double liqOpenpPhysPL { get; private set; }
        public double openHedgePL { get; private set; }
        public double openPhysPL { get; private set; }
        public double otherPL { get; private set; }
        public double totalPnLSecCost { get; private set; }
        public int passRunId { get; private set; }
        public int portNum { get; private set; }
        public int transId { get; private set; }
        public DateTime plAsOfDate { get; private set; }
        public DateTime plCalcDate { get; private set; }
        #endregion properties

        #region methods
        public override string ToString() {
            return
                this.plAsOfDate + " : " +
                "total=" + totalPnLSecCost +
                " OpenHedge=" + openHedgePL +
                " OpenPhys=" + openPhysPL +
                " sum=" + (openHedgePL + openPhysPL);
        }
        #endregion
    }
}