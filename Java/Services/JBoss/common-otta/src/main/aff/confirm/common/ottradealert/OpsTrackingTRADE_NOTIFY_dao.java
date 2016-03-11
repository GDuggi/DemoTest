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
public class OpsTrackingTRADE_NOTIFY_dao extends OpsTrackingDAO {

    public OpsTrackingTRADE_NOTIFY_dao( java.sql.Connection pOpsTrackingConnection)
            throws SQLException {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };


    public int insertTradeNotify(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec)
            throws SQLException {
        PreparedStatement preparedStatement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_trade_notify";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into ops_tracking.TRADE_NOTIFY( " +
                    "ID, " + //  1
                    "TRADE_ID, " + //  2
                    "TRADE_VERSION, " + //  3
                    "NOTIFY_TIME_GMT, " + //----
                    "ACTION )" + // 4
                    "values( ?, ?, ?, SYSDATE, ? )";

            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setInt(1, nextSeqNo);
            preparedStatement.setDouble(2, pOpsTrackingTradeAlertDataRec.tradeID);
            preparedStatement.setDouble(3, pOpsTrackingTradeAlertDataRec.version);
            preparedStatement.setString(4, pOpsTrackingTradeAlertDataRec.auditTypeCode);
            preparedStatement.executeUpdate();
        }  finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
                preparedStatement = null;
            } catch (SQLException e) { }
        }
        return nextSeqNo;
    }

    public boolean isTradeNotifyExist(double pTradeID, double ptradeVersion) throws SQLException {
        boolean recordExists = false;
        int count = 0;
        String action = "EDIT";
        if (ptradeVersion == 1)
            action = "NEW";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT /*+ INDEX(TRADE_NOTIFY  IND_TRADENTFY_TRDID) */ " +
                    "count(*) cnt from ops_tracking.TRADE_NOTIFY " +
                    "where TRADE_ID = ? AND TRADE_VERSION = ? and ACTION = ?");
            statement.setDouble(1, pTradeID);
            statement.setDouble(2, ptradeVersion);
            statement.setString(3, action);

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

