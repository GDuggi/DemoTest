package aff.confirm.common.ottradealert;

import aff.confirm.common.util.MessageUtils;

import javax.jms.JMSException;
import javax.jms.Message;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.ParseException;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingIGNORED_NOTIFICATIONS_dao extends OpsTrackingDAO {

    public OpsTrackingIGNORED_NOTIFICATIONS_dao( java.sql.Connection pOpsTrackingConnection)
            throws SQLException {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };

    public int insertIgnoredNotifications(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec) throws
            SQLException {
         PreparedStatement preparedStatement = null;
         int nextSeqNo = -1;
         try {
             String seqName = "seq_ignored_notifications";
             nextSeqNo = getNextSequence(seqName);
             String insertSQL =
                     "Insert into ops_tracking.IGNORED_NOTIFICATIONS( " +
                         "ID," + //1
                         //"TRADE_AUDIT_ID, " +
                         "PRMNT_TRADE_ID, " + //2
                         "VERSION, " +        //3
                         //"UPDATE_DATETIME, " +
                         //"EMP_ID, " +
                         //"UPDATE_TABLE_NAME, " +
                         "AUDIT_TYPE_CODE, " + //4
                         //"UPDATE_BUSN_DT, " +
                         "TRADE_TYPE_CODE, " + //5
                         //"TRADE_STAT_CODE, " +
                         //"TRADE_DT, " +
                         //"CMPNY_SHORT_NAME, " +
                         //"BK_SHORT_NAME, " +
                         //"CPTY_SHORT_NAME, " +
                         //"CDTY_CODE, " +
                         //"BROKERSN, " +
                         "TRADING_SYSTEM, " + //6
                         //"INST_CODE, " +
                         //"NOTIFY_CONTRACTS_FLAG, " +
                         //"RFRNCE_SHORT_NAME, " +
                         //"TRADE_STTL_TYPE_CODE, " +
                         "NOTIFY_TIME_GMT ) " + //7
                     "values( ?, ?, ?, ?, ?, ?, SYSDATE )";

             preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
             preparedStatement.setInt(1, nextSeqNo);
             preparedStatement.setDouble(2, pOpsTrackingTradeAlertDataRec.tradeID);
             preparedStatement.setDouble(3, pOpsTrackingTradeAlertDataRec.version);
             preparedStatement.setString(4, pOpsTrackingTradeAlertDataRec.auditTypeCode);
             preparedStatement.setString(5, pOpsTrackingTradeAlertDataRec.tradeTypeCode);
             preparedStatement.setString(6, pOpsTrackingTradeAlertDataRec.tradingSystem);
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


    public int insertIgnoredNotifications(Message pMessage) throws
            SQLException, JMSException, ParseException {
         PreparedStatement preparedStatement = null;
         int nextSeqNo = -1;
         try {
             String seqName = "seq_ignored_notifications";
             nextSeqNo = getNextSequence(seqName);
             String insertSQL =
                     "Insert into ops_tracking.IGNORED_NOTIFICATIONS( " +
                         "ID," + //1
                         "TRADE_AUDIT_ID, " + //2
                         "PRMNT_TRADE_ID, " + //3
                         "VERSION, " +        //4
                         "UPDATE_DATETIME, " + //5
                         "EMP_ID, " +          //6
                         "UPDATE_TABLE_NAME, " + //7
                         "AUDIT_TYPE_CODE, " + //8
                         "UPDATE_BUSN_DT, " + //9
                         "TRADE_TYPE_CODE, " + //10
                         "TRADE_STAT_CODE, " + //11
                         "TRADE_DT, " + //12
                         "CMPNY_SHORT_NAME, " + //13
                         "BK_SHORT_NAME, " + //14
                         "CPTY_SHORT_NAME, " + //15
                         "CDTY_CODE, " + //16
                         "BROKERSN, " + //17
                         "TRADING_SYSTEM, " + //18
                         "INST_CODE, " + //19
                         "NOTIFY_CONTRACTS_FLAG, " + //20
                         "RFRNCE_SHORT_NAME, " + //21
                         "TRADE_STTL_TYPE_CODE, " + //22
                         "NOTIFY_TIME_GMT ) " + //23
                     "values( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, " +
                            " ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, SYSDATE )";

             preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
             preparedStatement.setInt(1, nextSeqNo);
             MessageUtils.setStatementDouble(2, "TRADE_AUDIT_ID", preparedStatement, pMessage);
             MessageUtils.setStatementDouble(3, "PRMNT_TRADE_ID", preparedStatement, pMessage);
             MessageUtils.setStatementDouble(4, "VERSION", preparedStatement, pMessage);
             MessageUtils.setStatementDateTime(5, "UPDATE_DATETIME", preparedStatement, pMessage);
             MessageUtils.setStatementDouble(6, "EMP_ID", preparedStatement, pMessage);
             MessageUtils.setStatementString(7, "UPDATE_TABLE_NAME", preparedStatement, pMessage);
             MessageUtils.setStatementString(8, "AUDIT_TYPE_CODE", preparedStatement, pMessage);
             MessageUtils.setStatementDateTime(9, "UPDATE_BUSN_DT", preparedStatement, pMessage);
             MessageUtils.setStatementString(10, "TRADE_TYPE_CODE", preparedStatement, pMessage);
             MessageUtils.setStatementString(11, "TRADE_STAT_CODE", preparedStatement, pMessage);
             MessageUtils.setStatementDateTime(12, "TRADE_DT", preparedStatement, pMessage);
             MessageUtils.setStatementString(13, "CMPNY_SHORT_NAME", preparedStatement, pMessage);
             MessageUtils.setStatementString(14, "BK_SHORT_NAME", preparedStatement, pMessage);
             MessageUtils.setStatementString(15, "CPTY_SHORT_NAME", preparedStatement, pMessage);
             MessageUtils.setStatementString(16, "CDTY_CODE", preparedStatement, pMessage);
             MessageUtils.setStatementString(17, "BROKERSN", preparedStatement, pMessage);
             MessageUtils.setStatementString(18, "TRADING_SYSTEM", preparedStatement, pMessage);
             MessageUtils.setStatementString(19, "INST_CODE", preparedStatement, pMessage);
             MessageUtils.setStatementString(20, "NOTIFY_CONTRACTS_FLAG", preparedStatement, pMessage);
             MessageUtils.setStatementString(21, "RFRNCE_SHORT_NAME", preparedStatement, pMessage);
             MessageUtils.setStatementString(22, "TRADE_STTL_TYPE_CODE", preparedStatement, pMessage);
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


    public boolean existsInIgnoredNotify(double pTradeId) throws SQLException {
        OpsTrackingTRADE_DATA_rec otTRADE_DATA_rec;
        otTRADE_DATA_rec = new OpsTrackingTRADE_DATA_rec();
        PreparedStatement statement = null;
        ResultSet rs = null;

        boolean exists = false;

        String selectSQL = "select * from ops_tracking.IGNORED_NOTIFICATIONS where PRMNT_TRADE_ID = ? AND VERSION = ?";
        try {
            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pTradeId);
            statement.setDouble(2,1);
            rs = statement.executeQuery();

            if (rs.next()) {
                exists = true;
            }


        } catch (SQLException e) {
            throw e;
        } finally{
            if (rs != null)
                rs.close();
            rs = null;

            if (statement != null)
                statement.close();
            statement = null;
        }
        return exists;
   }

     public boolean auditExistsInIgnoredNotify(double pTradeId, double version) throws SQLException {

         boolean exists = false;
         PreparedStatement statement = null;
         ResultSet rs = null;

        String selectSQL = "select * from ops_tracking.IGNORED_NOTIFICATIONS where PRMNT_TRADE_ID = ? AND VERSION = ?";
        try {
            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pTradeId);
            statement.setDouble(2,version);
            rs = statement.executeQuery();

            if (rs.next()) {
                exists = true;
            }


        } catch (SQLException e) {
            throw e;
        } finally{
            if (rs != null)
                rs.close();
            rs = null;

            if (statement != null)
                statement.close();
            statement = null;
        }
        return exists;

     }
}
