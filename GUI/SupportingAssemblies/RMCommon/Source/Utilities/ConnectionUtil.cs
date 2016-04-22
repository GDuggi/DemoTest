//#define LOCAL_CONNECTION

using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
//using NSRMLogging;

namespace NSRMCommon {
    public static class ConnectionUtil {
        #region properties
        public static string dotNetConnection { get; private set; }
        public static string javaConnection { get; private set; }

        public static void setDotNetConnection(string p) { dotNetConnection = p; }
        public static void setJavaConnection(string p) { javaConnection = p; }
        public static string dotNetConnectionString() { return dotNetConnection; }
        public static string jdbcConnectionString() { return javaConnection; }
        #endregion properties
    }
}