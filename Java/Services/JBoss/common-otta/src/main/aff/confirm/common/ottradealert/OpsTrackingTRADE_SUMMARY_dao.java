package aff.confirm.common.ottradealert;

import oracle.jdbc.OracleCallableStatement;

import javax.jms.JMSException;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Types;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTRADE_SUMMARY_dao extends OpsTrackingDAO {

    public OpsTrackingTRADE_SUMMARY_dao( java.sql.Connection pOpsTrackingConnection)
            throws SQLException {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };


    public OpsTrackingTRADE_SUMMARY_rec getOpsTrackingTRADE_SUMMARY_rec(double pTradeId)
            throws SQLException {
        OpsTrackingTRADE_SUMMARY_rec otTRADE_SUMMARY_rec;
        otTRADE_SUMMARY_rec = new OpsTrackingTRADE_SUMMARY_rec();
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String selectSQL = "select * from ops_tracking.TRADE_SUMMARY where TRADE_ID = ?";
            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                otTRADE_SUMMARY_rec.CATEGORY = rs.getString("CATEGORY");
                otTRADE_SUMMARY_rec.CMT = rs.getString("CMT");
                otTRADE_SUMMARY_rec.FINAL_APPROVAL_FLAG = rs.getString("FINAL_APPROVAL_FLAG");
                otTRADE_SUMMARY_rec.FINAL_APPROVAL_TIMESTAMP_GMT = rs.getDate("FINAL_APPROVAL_TIMESTAMP_GMT");
                otTRADE_SUMMARY_rec.HAS_PROBLEM_FLAG = rs.getString("HAS_PROBLEM_FLAG");
                otTRADE_SUMMARY_rec.ID = rs.getDouble("ID");
                otTRADE_SUMMARY_rec.LAST_TRD_EDIT_TIMESTAMP_GMT = rs.getDate("LAST_TRD_EDIT_TIMESTAMP_GMT");
                otTRADE_SUMMARY_rec.LAST_UPDATE_TIMESTAMP_GMT = rs.getDate("LAST_UPDATE_TIMESTAMP_GMT");
                otTRADE_SUMMARY_rec.OPEN_RQMTS_FLAG = rs.getString("OPEN_RQMTS_FLAG");
                otTRADE_SUMMARY_rec.OPS_DET_ACT_FLAG = rs.getString("OPS_DET_ACT_FLAG");
                otTRADE_SUMMARY_rec.READY_FOR_FINAL_APPROVAL_FLAG = rs.getString("READY_FOR_FINAL_APPROVAL_FLAG");
                otTRADE_SUMMARY_rec.TRADE_ID = pTradeId;
                otTRADE_SUMMARY_rec.TRANSACTION_SEQ = rs.getDouble("TRANSACTION_SEQ");
            }

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
            try {
                if (rs != null) {
                    rs.close();
                    rs = null;
                }
            } catch (SQLException e) {
            }

        }
        return otTRADE_SUMMARY_rec;
    }


    public boolean isTradeFinalApproved(double pTradeID) throws SQLException {
        boolean recordExists = false;
        int count = 0;
        final String finalApprovalFlag = "Y";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT count(*) cnt from " +
                    "ops_tracking.TRADE_SUMMARY where TRADE_ID = ? AND FINAL_APPROVAL_FLAG = ? ");
            statement.setDouble(1, pTradeID);
            statement.setString(2, finalApprovalFlag);

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

    public int execInsertTradeSummary(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec)
                throws JMSException, SQLException {
            int tradeSummaryID;
            String openRqmtsFlag = null;
            openRqmtsFlag = getOpenRqmtsFlag(pOpsTrackingTradeAlertDataRec.finalApprovalFlag);
            String category = null;
            category = getCategory(pOpsTrackingTradeAlertDataRec);

            pOpsTrackingTradeAlertDataRec.openRqtsFlag = openRqmtsFlag;
            pOpsTrackingTradeAlertDataRec.category = category;

            //tradeSummaryID = insertTradeSummary(pOpsTrackingTradeAlertDataRec);
            tradeSummaryID = insertTradeSummary(pOpsTrackingTradeAlertDataRec);

            return tradeSummaryID;
        }

    private String getCategory(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec) {
        String category;
        String tradingSystem;
        String tradeTypeCode;

        tradingSystem = pOpsTrackingTradeAlertDataRec.tradingSystem;
        tradeTypeCode = pOpsTrackingTradeAlertDataRec.tradeTypeCode;
        if (tradingSystem.equalsIgnoreCase("AFF"))
            category = tradeTypeCode;
        else if(tradingSystem.equalsIgnoreCase("SYM"))
            category = "OIL";
        else
            category = "???";
        return category;
    }


    private String getOpenRqmtsFlag(String pFinalApprovalFlag) {
        String rqmtsFlag;
        if (pFinalApprovalFlag.equalsIgnoreCase("Y"))
            rqmtsFlag = "N";
        else
            rqmtsFlag = "Y";
        return rqmtsFlag;
    }

    private int insertTradeSummary(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec)
            throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_trade_summary";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into ops_tracking.TRADE_SUMMARY( " +
                    "ID, " + //1
                    "TRADE_ID, " + //2
                    "OPEN_RQMTS_FLAG, " + //3
                    "CATEGORY, " + //4
                    "FINAL_APPROVAL_FLAG, " + //5
                    "OPS_DET_ACT_FLAG ) " + //7
                    "values( ?, ?, ?, ?, ?, ? )";
//IF Stub --
// Add two new columns and add ? for each one to values statement.
            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            statement.setDouble(2, pOpsTrackingTradeAlertDataRec.tradeID);
            statement.setString(3, pOpsTrackingTradeAlertDataRec.openRqtsFlag);
            statement.setString(4, pOpsTrackingTradeAlertDataRec.category);
            statement.setString(5, pOpsTrackingTradeAlertDataRec.finalApprovalFlag);
            statement.setString(6, pOpsTrackingTradeAlertDataRec.opsDetActionsFlag);
//IF Stub --
            //statement.setString(7, pOpsTrackingTradeAlertDataRec.rqmtRuleID);
            //statement.setString(8, pOpsTrackingTradeAlertDataRec.rqmtBrokerExcludeRuleID);
            statement.executeUpdate();

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
        return nextSeqNo;
    }


    public void updateTradeSummary(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec)
            throws SQLException {
        OracleCallableStatement statement = null;
        try {
            //String callSqlStatement = "{call ops_tracking.pkg_trade_summary.p_update_trade_summary(?,?,?,?) }";
            String callSqlStatement = "{call ops_tracking.p_update_trade_summary(?,?,?,?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pOpsTrackingTradeAlertDataRec.tradeID);
            statement.setString(2, pOpsTrackingTradeAlertDataRec.finalApprovalFlag);
            statement.setString(3, pOpsTrackingTradeAlertDataRec.opsDetActionsFlag);

            if (pOpsTrackingTradeAlertDataRec.openRqtsFlag.equalsIgnoreCase("Y") ||
                pOpsTrackingTradeAlertDataRec.openRqtsFlag.equalsIgnoreCase("N"))
                statement.setString(4, pOpsTrackingTradeAlertDataRec.openRqtsFlag);
            else
                statement.setNull(4, Types.VARCHAR);

            statement.executeQuery();
        }  finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

        public void updateEditedSentTrade(double pTradeId, String pReadyForFinalApprovalFlag)
            throws SQLException {
        PreparedStatement preparedStatement = null;
        try {
            String insertSQL =
                    "Update ops_tracking.TRADE_SUMMARY " +
                    "set ready_for_final_approval_flag = ?" + //3
                    "where trade_id = ?"; //4

            preparedStatement = null;
            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setString(1, pReadyForFinalApprovalFlag);
            preparedStatement.setDouble(2, pTradeId);
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
