using System;
using System.Reflection;
using System.Text.RegularExpressions;
using NSRMLogging;

namespace NSRMCommon
{
    static class LocalPeriodUtil {

        internal static DateTime datetimeFrom(string p) {
            bool duymmy;

            return datetimeFrom(p,out duymmy);
        }

        internal static DateTime datetimeFrom(string p,out bool isSpot) {
            int year,month;

            isSpot = false;
            if (string.IsNullOrEmpty(p))
                return DateTime.MinValue;

            if (Regex.IsMatch(p,"^[0-9]{6}$")) {
                year = Convert.ToInt32(p.Substring(0,4));
                month = Convert.ToInt32(p.Substring(4,2));
                return new DateTime(year,month,1);
            }
            if (Regex.IsMatch(p,"^[0-9]{6}W[1-6]$")) {
                year = Convert.ToInt32(p.Substring(0,4));
                month = Convert.ToInt32(p.Substring(4,2));
                // INVESTIGATE: capture week number here.
                return new DateTime(year,month,1);
            }
            if (Regex.IsMatch(p,"^SPOT[0-9]{2}$")) {
                isSpot = true;
                //          year = Convert.ToInt32(mdlTabMove.Substring(0,4));
                month = Convert.ToInt32(p.Substring(4,2));
                // INVESTIGATE: capture spot number here.
                return DateTime.Now.AddMonths(month);
            }
            if (string.Compare(p,"SPOT",true) == 0) {
                isSpot = true;
                // INVESTIGATE: indicate spot 0, here.
                return DateTime.Now;
            }
            throw new InvalidOperationException(Util.makeSig(MethodBase.GetCurrentMethod()) + ": unhandled period '" + p + "'!");
        }
    }
}