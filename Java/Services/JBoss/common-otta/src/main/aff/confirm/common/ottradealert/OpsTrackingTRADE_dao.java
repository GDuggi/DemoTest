package aff.confirm.common.ottradealert;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTRADE_dao extends OpsTrackingDAO {

    public OpsTrackingTRADE_dao( java.sql.Connection pOpsTrackingConnection)
            throws SQLException {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };


    public void insertTrade(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec)
            throws SQLException {
        PreparedStatement preparedStatement = null;
        try {
            String insertSQL =
                    "Insert into ops_tracking.TRADE( " +
                    "ID, " + //  1
                    "TRD_SYS_CODE, " + //  2
                    "TRD_SYS_ID, " + //  3
                    "TICKET )" + // 4
                    "values( ?, ?, ?, ? )";

            preparedStatement = null;
            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setDouble(1, pOpsTrackingTradeAlertDataRec.tradeID);
            preparedStatement.setString(2, pOpsTrackingTradeAlertDataRec.tradingSystem);
            preparedStatement.setDouble(3, pOpsTrackingTradeAlertDataRec.tradeID);
            preparedStatement.setDouble(4, pOpsTrackingTradeAlertDataRec.tradeID);
            preparedStatement.executeUpdate();
        } finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
                preparedStatement = null;
            } catch (SQLException e) {
            }
        }
    }

    public boolean isTradeExist(double pTradeID) throws SQLException {
        boolean recordExists = false;
        int count = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT count(*) from " +
                    "ops_tracking.TRADE where TRD_SYS_ID = ?");
            statement.setDouble(1, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                count = (rs.getInt("count(*)"));
            }
            recordExists = count > 0;
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }

        return recordExists;
    }

}

