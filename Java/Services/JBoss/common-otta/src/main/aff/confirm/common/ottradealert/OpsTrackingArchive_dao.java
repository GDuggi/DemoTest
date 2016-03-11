package aff.confirm.common.ottradealert;

import oracle.jdbc.OracleCallableStatement;

import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingArchive_dao extends OpsTrackingDAO {

    public OpsTrackingArchive_dao( java.sql.Connection pOpsTrackingConnection)
            throws SQLException {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };


    public void archiveTradeData(String pTradeSysCode, double pTradeId )
            throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.pkg_trade_data.p_archive_trade_data(?,?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setString(1, pTradeSysCode);
            statement.setDouble(2, pTradeId);
            statement.executeQuery();
        }  finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

    public int restoreTradeData(String pTradeSysCode, double pTradeId )
            throws SQLException {
        int functionResult = -1;
        OracleCallableStatement statement = null;
        try {
            //String callSqlStatement = "{call ops_tracking.pkg_trade_data.f_restore_trade_data(?,?) }";
            String callSqlStatement = "begin ? := ops_tracking.pkg_trade_data.f_restore_trade_data(?,?); end;";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.registerOutParameter(1, java.sql.Types.INTEGER);
            statement.setString(2, pTradeSysCode);
            statement.setDouble(3, pTradeId);

            statement.executeQuery();
            functionResult = statement.getInt(1);
        }  finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
        return functionResult;
    }

}
