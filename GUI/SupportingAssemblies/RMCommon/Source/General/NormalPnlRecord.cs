using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using NSRMCommon;
using NSRMLogging;

namespace NSRMCommon
{
    public class NormalPnlRecord {
        static readonly NormalPnlRecord Null = new NormalPnlRecord("Null");
        #region ctor

        internal NormalPnlRecord(string aDesc) {
            rowType = aDesc;
            this.closedHedgePL = double.MinValue;
            this.closedPhysPL = double.MinValue;

            this.closedLiqHedgePL = double.MinValue;
            this.closedLiqPhysPL = double.MinValue;

            this.openHedgePL = double.MinValue;
            this.openPhysPL = double.MinValue;

            this.openLiqHedgePL = double.MinValue;
            this.openLiqPhysPL = double.MinValue;

            this.totalPLNoSec = double.MinValue;
        }

        internal NormalPnlRecord(string aDesc,IPLRecord iplr)
            : this(aDesc) {
            if (iplr != null)
                this.initFrom(iplr);
        }

        #endregion

        #region properties
        [Browsable(false)]
        public string rowType { get; private set; }
        [DisplayName("Open")]
        public double openPL { get; private set; }
        [DisplayName("Closed")]
        public double closedPL { get; private set; }
        [DisplayName("Liquidated")]
        public double liquidatedPL { get; private set; }
        [DisplayName("Total P/L")]
        public double totalPL { get; private set; }

        [Browsable(false)]
        public double closedHedgePL { get; private set; }
        [Browsable(false)]
        public double closedPhysPL { get; private set; }

        [Browsable(false)]
        public double closedLiqHedgePL { get; private set; }
        [Browsable(false)]
        public double closedLiqPhysPL { get; private set; }

        [Browsable(false)]
        public double openHedgePL { get; private set; }
        [Browsable(false)]
        public double openPhysPL { get; private set; }

        [Browsable(false)]
        public double openLiqHedgePL { get; private set; }
        [Browsable(false)]
        public double openLiqPhysPL { get; private set; }

        [Browsable(false)]
        public double totalPLNoSec { get; private set; }

        [Browsable(false)]
        public bool locked { get; private set; }
        #endregion

        #region methods
        public static object generateDatasource(IEnumerable<IPLRecord> plRecords) {

            List<NormalPnlRecord> tmp = new List<NormalPnlRecord>();
            List<IPLRecord> tmp2 = new List<IPLRecord>(plRecords);
            NormalPnlRecord day,week,month,year,compYear,lifeToDate;
            IPLRecord iplr;
            int n2;

            if (plRecords == null)
                throw new ArgumentNullException("plRecords","record-vector is null!");
            tmp2 = new List<IPLRecord>(plRecords);
            if ((n2 = tmp2.Count) > 0) {
                tmp.AddRange(new NormalPnlRecord[]{
                    day=new NormalPnlRecord ("Day"),
                    week=new NormalPnlRecord ("Week"),
                    month=new NormalPnlRecord ("Month"),
                    year=new NormalPnlRecord ("Year"),
                    compYear=new NormalPnlRecord ("CompYear"),
                    lifeToDate=new NormalPnlRecord ("Life-To-Date"),
                });
                foreach (var avar in tmp)
                    avar.initFrom(tmp2[0]);
                if (n2 > 1) {
                    day.calcDifferenceFrom(tmp2[1]);
                    for (int i = 1;i < n2;i++) {
                        iplr = tmp2[i];
                        if (iplr.isWeekend)
                            updateWeekIfNeeded(week,iplr);
                        if (iplr.isMonthEnd)
                            updateWeekIfNeeded(month,iplr);
                        if (iplr.isYearEnd)
                            updateWeekIfNeeded(year,iplr);
                        if (iplr.isCompYearEnd)
                            updateWeekIfNeeded(compYear,iplr);
                    }
                }
                foreach (var avar in tmp)
                    avar.calcTotal();
            }
            return tmp.ToArray();
        }

        const string DFMT = "$#,##0.00 ;($#,##0.00)";
        const int MAX_WIDTH = 20;

        static void updateWeekIfNeeded(NormalPnlRecord arecord,IPLRecord iplr) {
            if (!arecord.locked) {
#if VERBOSE
                Debug.Print(arecord.rowType + ": " + iplr.plAsOfDate.ToString("dd-MMM-yy") + " " + iplr.totalPnLSecCost.ToString(DFMT).PadLeft(MAX_WIDTH));
#endif
                arecord.calcDifferenceFrom(iplr);
                arecord.lockRecord();
            }
        }

        void calcDifferenceFrom(IPLRecord iPLRecord) {
            this.openPL = this.totalPLNoSec - iPLRecord.totalPnLSecCost;
        }

        void calcTotal() {
            this.totalPL = this.openPL + this.closedPL + this.liquidatedPL;
        }

        void initFrom(IPLRecord iplr) {
            if (iplr == null)
                throw new ArgumentNullException("iplr",typeof(IPLRecord).FullName + " is null!");
            this.closedHedgePL = iplr.closedHedgePL;
            this.closedPhysPL = iplr.closedPhysPL;

            this.closedLiqHedgePL = iplr.liqClosedHedgePL;
            this.closedLiqPhysPL = iplr.liqClosedPhysPL;

            this.openHedgePL = iplr.openHedgePL;
            this.openPhysPL = iplr.openPhysPL;

            this.openLiqHedgePL = iplr.liqOpenHedgePL;
            this.openLiqPhysPL = iplr.liqOpenpPhysPL;

            this.openPL = this.totalPL = this.totalPLNoSec = iplr.totalPnLSecCost;
        }

        public void lockRecord() {
            locked = true;
        }

        public void calcDifferenceFrom(NormalPnlRecord npnlDay) {
            Util.show(MethodBase.GetCurrentMethod());
        }

        public override string ToString() {
            return "totalPLNoSec=" + totalPLNoSec +
                " Open=" + this.openPL +
                " Closed=" + this.closedPL +
                " Liquidated=" + this.liquidatedPL;
        }

        #endregion
    }
}