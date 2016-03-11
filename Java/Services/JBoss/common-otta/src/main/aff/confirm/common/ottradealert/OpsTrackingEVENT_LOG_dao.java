package aff.confirm.common.ottradealert;

import java.sql.PreparedStatement;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingEVENT_LOG_dao extends OpsTrackingDAO {

    public OpsTrackingEVENT_LOG_dao( java.sql.Connection pOpsTrackingConnection)
            throws SQLException {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };


    public int insertEventLog(OpsTrackingEVENT_LOG_rec pOpsTrackingEVENT_LOG_rec) throws SQLException {
        PreparedStatement preparedStatement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_trade_notify";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into ops_tracking.EVENT_LOG( " +
                    "ID, " + //  1
                    "TRADE_ID, " + //  2
                    "TRADE_VERSION, " + //  3
                    "NOTIFY_TIME_GMT, " + //----
                    "ACTION )" + // 4
                    "values( ?, ?, ?, SYSDATE, ? )";

            int tempVAr;
            tempVAr = 123;

            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setInt(1, nextSeqNo);
            preparedStatement.setDouble(2, pOpsTrackingEVENT_LOG_rec.TRADE_ID);
            //preparedStatement.setDouble(3, pOpsTrackingEVENT_LOG_rec.MSG_TYPE);
            //preparedStatement.setString(4, pOpsTrackingTradeAlertDataRec.auditTypeCode);
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


}

