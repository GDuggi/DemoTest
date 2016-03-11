package aff.confirm.common.ottradealert;

import java.sql.PreparedStatement;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTRADE_APPR_dao extends OpsTrackingDAO {

    public OpsTrackingTRADE_APPR_dao( java.sql.Connection pOpsTrackingConnection)
            throws SQLException {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };


    public void insertTradeApprover(double pTradeId, String pApprFlag, String pApprByUserName)
            throws SQLException {
        PreparedStatement preparedStatement = null;
        try {
            String insertSQL =
                    "Insert into ops_tracking.TRADE_APPR( " +
                    "ID, " + //
                    "TRADE_ID, " + //  1
                    "APPR_FLAG, " + // 2
                    "APPR_BY_USERNAME )" + // 3
                    "values( ops_tracking.seq_trade_appr.nextval, ?, ?, ? )";

            preparedStatement = null;
            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setDouble(1, pTradeId);
            preparedStatement.setString(2, pApprFlag);
            preparedStatement.setString(3, pApprByUserName);
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

}