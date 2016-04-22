using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;


namespace NSRMCommon
{
    public class HistoricalPnlRecord {
        #region CTOR
        public HistoricalPnlRecord(IPLRecord iplr) {
            this.date = iplr.plAsOfDate;
            this.profitLoss = iplr.openHedgePL - iplr.openPhysPL;
            this.profitLoss = iplr.totalPnLSecCost;
        }
        #endregion

        #region methods

        public static object generateDatasource(IEnumerable<IPLRecord> plRecords) {
            List<HistoricalPnlRecord> tmp = new List<HistoricalPnlRecord>();
            HistoricalPnlRecord prev = null;
            int n;

            foreach (IPLRecord iplr in plRecords)
                tmp.Add(new HistoricalPnlRecord(iplr));
            if ((n = tmp.Count) > 0) {
                for (int i = n - 1;i >= 0;i--) {
                    tmp[i].calculateAgainst(prev);
                    prev = tmp[i];
                }
                foreach (var avar in tmp)
                    avar.calcSomething();
            }
            return tmp.ToArray();
        }

        HistoricalPnlRecord calculateAgainst(HistoricalPnlRecord prev) {
            if (prev != null) {
                this.delta = this.profitLoss - prev.profitLoss;
                hadPrev = true;
            }
            return this;
        }

        bool hadPrev;
        void calcSomething() {
       //     Debug.Print("calc somethjign");
            if (!hadPrev) {
                this.delta = this.profitLoss;
            }
        }

        public override string ToString() {
            return GetType().FullName;
        }
        #endregion

        #region properties
        [DisplayName("Date")]
        public DateTime date { get; private set; }
        [DisplayName("P/(L)")]
        public double profitLoss { get; private set; }
        [DisplayName("Change for Day")]
        public double delta { get; private set; }
        #endregion
    }
}