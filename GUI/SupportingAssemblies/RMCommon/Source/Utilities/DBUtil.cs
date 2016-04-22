using System.Text;
using System;

namespace NSRMCommon {
    public static class DBUtil {
        // real_port_num
        public static string makeEqualOrInClause(int[] portNums,string fieldName) {
            const string DEFAULT_VALUE = "1=0";
            string ret = DEFAULT_VALUE;
            StringBuilder sb;
            int n;

            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentNullException("fieldName","table's field-name is null!");
            if (portNums != null && (n = portNums.Length) > 0) {
                if (n == 1)
                    ret = fieldName +" = " + portNums[0];
                else {
                    sb = new StringBuilder();
                    sb.Append(fieldName+" in ");
                    sb.Append("(");
                    for (int i = 0;i < n;i++) {
                        if (i > 0)
                            sb.Append(",");
                        sb.Append(portNums[i]);
                    }
                    sb.Append(")");
                    ret = sb.ToString();
                }
            }
            return ret;
        }
    }
}