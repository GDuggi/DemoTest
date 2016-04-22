using System;
using System.Text;
using System.Text.RegularExpressions;

namespace NSRMCommon {
    public static class PeriodUtil {
        public const string COMPLEX_FRM = "CMPLX";
        public const string ITEM_SEPARATOR = " / ";
        public const string NULL_PERIOD = "-NULL-";

        public static string formatPeriod(string tradingPrd) {
            int ordinal;

            if (string.Compare(tradingPrd,COMPLEX_FRM) == 0)
                return COMPLEX_FRM;
            else if (Regex.IsMatch(tradingPrd,"^[0-9]{6}$")) {
                return DateTime.ParseExact(tradingPrd,"yyyyMM",null).ToString("MMM-yy");
            } else {
                if (Regex.IsMatch(tradingPrd,"^[0-9]{6}W[1-5]$")) {
                    DateTime dt = DateTime.ParseExact(tradingPrd.Substring(0,6),"yyyyMM",null);
                    return dt.ToString("MMM-yy") + " [Week " + tradingPrd[7] + "]";
                } else if (Regex.IsMatch(tradingPrd,"SPOT[0-9]{2}")) {
                    switch (ordinal = Convert.ToInt32(tradingPrd.Substring(4,2))) {
                        case 1: return ordinal + "st Nearby";
                        case 2: return ordinal + "nd Nearby";
                        case 3: return ordinal + "rd Nearby";
                        default: return ordinal + "th Nearby";
                    }
                }
            }
            return "*" + tradingPrd + "*";
        }

        /// <summary>Generate a string of formatted trading periods.</summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string catenatePeriods(string[] p) {
            return catenate(p,true);
        }

        /// <summary>Generate a string of items, optionally formatted as trading periods.</summary>
        /// <param name="p"></param>
        /// <param name="doPeriodFormat"></param>
        /// <returns></returns>
        public static string catenate(string[] p,bool doPeriodFormat) {
            StringBuilder sb = new StringBuilder();
            int nperiods = 0;

            foreach (string aPrd in p) {
                if (!string.IsNullOrEmpty(aPrd)) {
                    if (nperiods > 0)
                        sb.Append(ITEM_SEPARATOR);

                    sb.Append(doPeriodFormat ? formatPeriod(aPrd) : aPrd);
                    nperiods++;
                }
            }
            return sb.ToString();
        }
    }
}