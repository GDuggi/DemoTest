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
public class OpsTrackingTRADE_PRIORITY_dao extends OpsTrackingDAO {

    public OpsTrackingTRADE_PRIORITY_dao( java.sql.Connection pOpsTrackingConnection)
            throws SQLException {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };


    public void insertTradePriority(double pTradeId, int pPriority, String pPlAmt)
            throws SQLException {
        PreparedStatement preparedStatement = null;
        try {
            String insertSQL =
                    "Insert into ops_tracking.TRADE_PRIORITY( " +
                    "ID, " + //
                    "TRADE_ID, " + //  1
                    "PRIORITY, " + // 2
                    "PL_AMT )" + // 3
                    "values( ops_tracking.seq_trade_priority.nextval, ?, ?, ? )";

            preparedStatement = null;
            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setDouble(1, pTradeId);
            preparedStatement.setInt(2, pPriority);
            preparedStatement.setString(3, pPlAmt);
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


    public void updateTradePriority(double pTradeId, int pPriority, String pPlAmt)
            throws SQLException {
        PreparedStatement preparedStatement = null;
        try {
            String insertSQL =
                    "Update ops_tracking.TRADE_PRIORITY " +
                    "set priority = ?" + //1
                    ",pl_amt = ?" + //2
                    "where trade_id = ?" + //3
                    "and (priority <> ? " +//4
                    "or pl_amt <> ?)"; //5

            preparedStatement = null;
            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setInt(1, pPriority);
            preparedStatement.setString(2, pPlAmt);
            preparedStatement.setDouble(3, pTradeId);
            preparedStatement.setInt(4, pPriority);
            preparedStatement.setString(5, pPlAmt);
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

    public boolean isTradePriorityExist(double pTradeID) throws SQLException {
        boolean recordExists = false;
        int count = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT count(*) cnt from " +
                    "ops_tracking.TRADE_PRIORITY where TRADE_ID = ?");
            statement.setDouble(1, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                count = (rs.getInt("cnt"));
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

