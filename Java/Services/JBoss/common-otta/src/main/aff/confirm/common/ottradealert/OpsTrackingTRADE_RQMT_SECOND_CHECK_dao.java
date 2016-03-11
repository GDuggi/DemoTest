package aff.confirm.common.ottradealert;

import java.sql.PreparedStatement;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTRADE_RQMT_SECOND_CHECK_dao extends OpsTrackingDAO {

    public OpsTrackingTRADE_RQMT_SECOND_CHECK_dao( java.sql.Connection pOpsTrackingConnection){
        this.opsTrackingConnection = pOpsTrackingConnection;
    };

    public void insertTradeRqmtSecondCheck(String pRqmtCode, double pTradeId)
            throws SQLException {
        PreparedStatement preparedStatement = null;
        try {
            String insertSQL =
                "insert into ops_tracking.TRADE_RQMT_SECOND_CHECK( "
                 + "rqmt_id, "
                 + "trade_id) "
                 + "values( "
                 + "(select id from ops_tracking.trade_rqmt "
                 + "where rqmt = ? "
                 + "and trade_id = ?), "
                 + "?) ";

            preparedStatement = null;
            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setString(1, pRqmtCode);
            preparedStatement.setDouble(2, pTradeId);
            preparedStatement.setDouble(3, pTradeId);
            preparedStatement.executeUpdate();
        } catch (SQLException e){
            //Makes insertion safe. The error shown below will be ignored--all others will be processed as usual.
            if (e.getMessage().indexOf("ORA-00001: unique constraint") == -1 )
                throw new SQLException("OpsTrackingTRADE_RQMT_SECOND_CHECK_dao.insertTradeRqmtSecondCheck: " , e );
        } finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
                preparedStatement = null;
            } catch (SQLException e) {
            }
        }
    }

    public void insertTradeRqmtSecondCheck(double pRqmtId, double pTradeId)
            throws SQLException {
        PreparedStatement preparedStatement = null;
        try {
            String insertSQL =
                    "Insert into ops_tracking.TRADE_RQMT_SECOND_CHECK( " +
                    "RQMT_ID, " + //  1
                    "TRADE_ID )" + // 2
                    "values( ?, ? )";

            preparedStatement = null;
            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setDouble(1, pRqmtId);
            preparedStatement.setDouble(2, pTradeId);
            preparedStatement.executeUpdate();
        } catch (SQLException e){
            //Makes insertion safe. The error shown below will be ignored--all others will be processed as usual.
            if (e.getMessage().indexOf("ORA-00001: unique constraint") == -1 )
                throw new SQLException("OpsTrackingTRADE_RQMT_SECOND_CHECK_dao.insertTradeRqmtSecondCheck: " , e);
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

