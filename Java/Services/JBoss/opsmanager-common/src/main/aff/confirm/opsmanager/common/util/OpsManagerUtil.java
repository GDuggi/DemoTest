package aff.confirm.opsmanager.common.util;

import aff.confirm.common.util.JndiUtil;
import aff.confirm.jboss.dbinfo.DBInfo;
import aff.confirm.opsmanager.common.data.BaseResponse;
import oracle.jdbc.OracleConnection;
import org.jboss.logging.Logger;

import javax.naming.NamingException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.Properties;

public class OpsManagerUtil {

    public static void createProxy(Connection connection, String userName) throws SQLException {

        closeProxy(connection);
//        Properties p = new Properties();
//        p.setProperty(OracleConnection.PROXY_USER_NAME, userName);
//        connection.openProxySession(OracleConnection.PROXYTYPE_USER_NAME, p);
    }

    public static void closeProxy(Connection connection) {
//        if (connection.isProxySession()) {
//            try {
//                connection.close(OracleConnection.PROXY_SESSION);
//            } catch (SQLException e) {
//                Logger.getLogger(OpsManagerUtil.class).error("ERROR", e);
//            }
//        }
    }

    public static String getUserName(Connection connection) {

//        OracleConnection oracleConnection = (OracleConnection) connection;
//        if (oracleConnection == null) {
//            return null;
//        }
//        String userName = null;
//        if (oracleConnection.isProxySession()) {
//            userName = oracleConnection.getProperties().getProperty(OracleConnection.PROXY_USER_NAME);
//        } else {
//            try {
//                userName = oracleConnection.getUserName();
//            } catch (SQLException e) {
//            }
//        }
//        return userName;
        return null;
    }

    public static OracleConnection getOracleConnection(String jndiName) throws NamingException, SQLException {
        DBInfo dbinfo = JndiUtil.lookup(jndiName);

        OracleConnection connection = (OracleConnection) DriverManager.getConnection(dbinfo.getDBUrl(), dbinfo.getDBUserName(), dbinfo.getDBPassword());
        return connection;

    }

    public static OracleConnection getOracleConnection(String dbURL, String userName, String password) throws SQLException {

        OracleConnection connection = (OracleConnection) DriverManager.getConnection(dbURL, userName, password);
        return connection;

    }

    public static BaseResponse populateErrorMessage(BaseResponse resp, Exception e) {
        //	if (resp == null){
//			resp = new BaseResponse();
        //	}
        resp.setResponseStatus(BaseResponse.ERROR);
        resp.setResponseText(e.getMessage());
        resp.setResponseStackError(e.getStackTrace().toString());
        return resp;
    }
}
