using System.Diagnostics;
using org.apache.log4j;
namespace NSRMCommon {
    public class JavaConnection {
        #region methods
        internal static void makeConnection() {
            string cstr;

            Logger.getRootLogger().setLevel(Level.ERROR);
            Trace.WriteLine("ConnectionString : " + (cstr = ConnectionUtil.jdbcConnectionString()));
            java.lang.System.setProperty("cayenne.jdbc.driver","net.sourceforge.jtds.jdbc.Driver");
            java.lang.System.setProperty("cayenne.jdbc.url",cstr);
            java.lang.System.setProperty("cayenne.jdbc.min_connections","1");
            java.lang.System.setProperty("cayenne.jdbc.max_connections", "20");
            java.lang.System.setProperty("-Dlogback.configurationFile", @"C:\oksana\RiskManager\Service\risk-manager-server\risk-manager-data\cayenne-data-model\src\main\resources\log-config.properties");
        }
        #endregion
    }
}