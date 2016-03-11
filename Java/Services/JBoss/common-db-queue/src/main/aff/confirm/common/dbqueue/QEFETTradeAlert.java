package aff.confirm.common.dbqueue;



import com.sun.rowset.CachedRowSetImpl;

import javax.sql.rowset.CachedRowSet;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Nov 25, 2003
 * Time: 7:00:07 AM
 * To change this template use Options | File Templates.
 */
public class QEFETTradeAlert extends QAlertBase{

    private String readyToReSubmitOrCancel;

    public QEFETTradeAlert(java.sql.Connection pConnection) throws SQLException {
        connection = pConnection;
        updateSQL = "Update JBOSSDBQ.Q_EFET_TRADE_ALERT set " +
                    "PROCESSED_FLAG = ?," + //1
                    "PROCESSED_TS_GMT = sysdate " +
                    "where id = ? "; //2

        //order must allow CNF docs to be processed first then BFI because of dependency
        //of BFI on CNF.
        readyToProcessSQL = "SELECT * from JBOSSDBQ.Q_EFET_TRADE_ALERT " +
                            "where PROCESSED_FLAG = 'N' " +
                            "order by DOC_TYPE DESC, ID";

        readyToReSubmitOrCancel =   "SELECT * from JBOSSDBQ.Q_EFET_TRADE_ALERT " +
                            "where (PROCESSED_FLAG = 'R') OR (PROCESSED_FLAG = 'N' " +
                            "AND EFET_ACTION = 'CANCEL')" +
                            " order by DOC_TYPE DESC, ID";
    }


    public void updateAlertRecordByTradeId(double pTradeId, String pProcessedFlag, String pDocType, String pReceiverType)
            throws SQLException, Exception {
        String updateSQLByTradeId = "";
        PreparedStatement statement = null;
        try {
            updateSQLByTradeId =
                    "Update JBOSSDBQ.Q_EFET_TRADE_ALERT set " +
                    "PROCESSED_FLAG = ?," + //1
                    "PROCESSED_TS_GMT = sysdate " +
                    "where trade_id = ? " + //2
                    " and doc_type = ? " + //3
                    " and receiver_type = ?"; //4
            statement = connection.prepareStatement(updateSQLByTradeId);
            statement.setString(1, pProcessedFlag);
            statement.setDouble(2, pTradeId);
            statement.setString(3, pDocType);
            statement.setString(4, pReceiverType);
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
    }

    public void updateAlertRecordByTradeId(double pTradeId, String pProcessedFlag)
                throws SQLException, Exception {
            String updateSQLByTradeId = "";
            PreparedStatement statement = null;
            try {
                updateSQLByTradeId =
                        "Update JBOSSDBQ.Q_EFET_TRADE_ALERT set " +
                        "PROCESSED_FLAG = ?," + //1
                        "PROCESSED_TS_GMT = sysdate " +
                        "where trade_id = ? " + //2
                        " and processed_flag  = 'N' ";
                statement = connection.prepareStatement(updateSQLByTradeId);
                statement.setString(1, pProcessedFlag);
                statement.setDouble(2, pTradeId);
                statement.executeUpdate();
            } finally {
                try {
                    if (statement != null)
                        statement.close();
                } catch (SQLException e) {
                }
                statement = null;
            }
        }


    public void insertQEfetAlert(QEFETTradeAlertRec pQEFETTradeAlertRec) throws SQLException {
        PreparedStatement statement = null;
        String processedFlag = "N";
        if (pQEFETTradeAlertRec.efetAction == "RESUBMIT"){
            pQEFETTradeAlertRec.efetAction = "SUBMIT";
            processedFlag = "R";
        }

        try {
            String insertSQL =
                    "Insert into JBOSSDBQ.Q_EFET_TRADE_ALERT( " +
                    "ID, " + //-
                    "TRADING_SYSTEM, " + //1
                    "TRADE_ID, " + //2
                    "SE_CPTY_SN," + //3
                    "EFET_ACTION," + //4
                    "EFET_SUBMIT_STATE, " + //5
                    "DOC_TYPE," + //6
                    "PROCESSED_FLAG," + //7
                    "RECEIVER_TYPE )" + //8
                    "values( JBOSSDBQ.SEQ_Q_EFET_TRADE_ALERT.NEXTVAL, ?, ?, ?, ?, ?, ?, ?, ? )";

            statement = connection.prepareStatement(insertSQL);
            statement.setString(1, pQEFETTradeAlertRec.tradingSystem);
            statement.setDouble(2, pQEFETTradeAlertRec.tradeID);
            statement.setString(3, pQEFETTradeAlertRec.seCptySn);
            statement.setString(4, pQEFETTradeAlertRec.efetAction);
            statement.setString(5, pQEFETTradeAlertRec.efetSubmitState);
            statement.setString(6, pQEFETTradeAlertRec.docType);
            statement.setString(7, processedFlag);
            statement.setString(8, pQEFETTradeAlertRec.receiverType);
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

    public boolean isEFETQueued(double pTradeId, String pEFETAction, String pDocType, String pReceiverType ) throws SQLException {
        int rowsFound = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select count(*) cnt from JBOSSDBQ.Q_EFET_TRADE_ALERT" +
                           " where trade_id = ? " +
                           " and efet_action = ?" +
                           " and processed_flag = 'N'" +
                           " and doc_type = ?" +
                           " and receiver_type = ?";
            statement = connection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            statement.setString(2, pEFETAction);
            statement.setString(3, pDocType);
            statement.setString(4, pReceiverType);
            rs = statement.executeQuery();
            if (rs.next()) {
                rowsFound = (rs.getInt("cnt"));
            }
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

        return rowsFound > 0;
    }

    public CachedRowSet cancelAlertRecords() throws SQLException {


        PreparedStatement statement = null;
        String sql = "SELECT * from JBOSSDBQ.Q_EFET_TRADE_ALERT " +
                            "where PROCESSED_FLAG = 'N' And EFET_ACTION = 'CANCEL' " +
                            "order by DOC_TYPE DESC, ID";
        ResultSet rs = null;
        CachedRowSet crs;
        crs = new CachedRowSetImpl();
               try {
                   statement = connection.prepareStatement(readyToProcessSQL);
                   rs = statement.executeQuery();
                   crs.populate(rs);
               } catch (SQLException e) {
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
               return crs;


    }

    public CachedRowSet getReadyForReSubmitOrCancel() throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        CachedRowSet crs;
        crs = new CachedRowSetImpl();
        try {
            statement = connection.prepareStatement(readyToReSubmitOrCancel);
            rs = statement.executeQuery();
            crs.populate(rs);
        } catch (SQLException e) {
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
        return crs;
    }
}
