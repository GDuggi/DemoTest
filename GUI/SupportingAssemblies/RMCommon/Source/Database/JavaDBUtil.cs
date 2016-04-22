using System;
namespace NSRMCommon {
    public static class JavaDBUtil {
        public static double copyDouble(java.lang.Float p) {
            return p == null ? double.MinValue : p.doubleValue();
        }

        public static int copyInt(java.lang.Integer integer) {
            return integer == null ? Int32.MinValue : integer.intValue();
        }

        public static int copyShort(java.lang.Short p) {
            return p == null ? Int32.MinValue : p.intValue();
        }

        public static double copyDecimal(java.math.BigDecimal bigDecimal) {
            return bigDecimal == null ? double.MinValue : bigDecimal.intValue();
        }

        public static bool copyBool(string p) {
            return string.IsNullOrEmpty(p) ? false : char.ToUpper(p[0]) == 'Y';
        }

        public static bool copyBoolFromChar(char charValue)
        {
            return (charValue == 'Y');
        }


        public static char copyChar(string p) 
        {
            return string.IsNullOrEmpty(p) ? char.MinValue : p[0];
        }

        const string JAVA_DATE_FORMAT = "MMM dd hh:mm:ss yyyy";
        public static DateTime makeDatetime(java.util.Date date) {
            string tmp,initial;

            if (date != null) {
                initial = date.toString();
                tmp = initial.Substring(4,16) + initial.Substring(24);
                return DateTime.ParseExact(tmp,JAVA_DATE_FORMAT,null);
            }
            return DateTime.MinValue;
        }

    }
}