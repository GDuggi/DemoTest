namespace NSRMCommon {
   public interface IPLRecord {
        bool isCompYearEnd { get; }
        bool isMonthEnd { get; }
        bool isOfficialRun { get; }
        bool isWeekend { get; }
        bool isYearEnd { get; }

        double closedHedgePL { get; }
        double closedPhysPL { get; }

        double liqClosedHedgePL { get; }
        double liqClosedPhysPL { get; }

        double liqOpenHedgePL { get; }
        double liqOpenpPhysPL { get; }

        double openHedgePL { get; }
        double openPhysPL { get; }

        double otherPL { get; }
        double totalPnLSecCost { get; }
        int passRunId { get; }
        int portNum { get; }
        int transId { get; }
        System.DateTime plAsOfDate { get; }
        System.DateTime plCalcDate { get; }
    }
}