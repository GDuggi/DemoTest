package aff.confirm.common.dbqueue;

import java.sql.PreparedStatement;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Nov 25, 2003
 * Time: 7:00:07 AM
 * To change this template use Options | File Templates.
 */
public class QBrokerMatchTradeAlert extends QAlertBase{

    public QBrokerMatchTradeAlert(java.sql.Connection pConnection) throws SQLException {
        connection = pConnection;
        updateSQL = "Update JBOSSDBQ.Q_BROKER_MATCH_TRADE_ALERT set " +
                    "PROCESSED_FLAG = ?," + //1
                    "PROCESSED_TS_GMT = sysdate " +
                    "where id = ? "; //2

        readyToProcessSQL = "SELECT * from JBOSSDBQ.Q_BROKER_MATCH_TRADE_ALERT " +
                            "where PROCESSED_FLAG = 'N' " +
                            "order by ID";
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


    public void insertQBrokerMatchAlert(QBrokerMatchTradeAlertRec pQBrokerMatchTradeAlertRec) throws SQLException {
        PreparedStatement statement = null;
        String processedFlag = "N";
        try {
            String insertSQL =
                    "Insert into JBOSSDBQ.Q_BROKER_MATCH_TRADE_ALERT( " +
                    "ID, " + //-
                    "TRADING_SYSTEM, " + //1
                    "TRADE_ID, " + //2
                    "BROKER_SN," + //3
                    "VERSION," + //4
                    "PROCESSED_FLAG)" + //5
                    "values( JBOSSDBQ.SEQ_Q_BROKER_MATCH_TRADE_ALERT.NEXTVAL, ?, ?, ?, ?, ? )";

            statement = connection.prepareStatement(insertSQL);
            statement.setString(1, pQBrokerMatchTradeAlertRec.tradingSystem);
            statement.setDouble(2, pQBrokerMatchTradeAlertRec.tradeID);
            statement.setString(3, pQBrokerMatchTradeAlertRec.brokerSn);
            statement.setInt(4, pQBrokerMatchTradeAlertRec.version);
            statement.setString(5, processedFlag);
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

}