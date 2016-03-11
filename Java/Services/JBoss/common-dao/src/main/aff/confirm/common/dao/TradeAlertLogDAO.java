package aff.confirm.common.dao;

import aff.confirm.common.util.MessageUtils;

import javax.jms.JMSException;
import javax.jms.Message;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.ParseException;

/**
 * User: ifrankel
 * Date: Apr 9, 2003
 * Time: 2:35:42 PM
 * To change this template use Options | File Templates.
 */
public class TradeAlertLogDAO {
    private Connection connection = null;
    private PreparedStatement preparedStatement;

    public TradeAlertLogDAO(Connection connection) {
        this.connection = connection;
    }

    public void insertTradeAlertLog(Message pMessage) throws SQLException, JMSException, ParseException {
        try {
            String seqName = "seq_trade_alert_log";
            int nextSeqNo = 0;
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                "Insert into ops_tracking.TRADE_ALERT_LOG( " +
                "ID, " +                    //  1
                "TRADING_SYSTEM, " +        //  2
                "PRMNT_TRADE_ID, " +        //  3
                "VERSION, " +               //  4
                "TRADE_AUDIT_ID, " +        //  5
                "EMP_ID, " +                //  6
                "TRADE_DT, " +              //  7
                "UPDATE_BUSN_DT, " +        //  8
                "UPDATE_DATETIME, " +       //  9
                "AUDIT_TYPE_CODE, " +       // 10
                "TRADE_TYPE_CODE, " +       // 11
                "TRADE_STAT_CODE, " +       // 12
                "INST_CODE, " +             // 13
                "CDTY_CODE, " +             // 14
                "NOTIFY_CONTRACTS_FLAG, " + // 15
                "CRTD_DT_GMT, " +           //----
                "CMPNY_SHORT_NAME, " +      // 16
                "BK_SHORT_NAME, " +         // 17
                "CPTY_SHORT_NAME, " +       // 18
                "BROKERSN, " +              // 19
                "RFRNCE_SHORT_NAME, " +     // 20
                "UPDATE_TABLE_NAME )" +     // 21
                "values( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, SYSDATE, ?, ?, ?, ?, ?, ? )";

            preparedStatement = null;
            preparedStatement = connection.prepareStatement(insertSQL);
            preparedStatement.setInt(1, nextSeqNo);
            MessageUtils.setStatementString(2, "TRADING_SYSTEM", preparedStatement, pMessage);
            MessageUtils.setStatementDouble(3, "PRMNT_TRADE_ID", preparedStatement, pMessage);
            MessageUtils.setStatementDouble(4, "VERSION", preparedStatement, pMessage);
            MessageUtils.setStatementDouble(5, "TRADE_AUDIT_ID", preparedStatement, pMessage);
            MessageUtils.setStatementDouble(6, "EMP_ID", preparedStatement, pMessage);
            MessageUtils.setStatementDateTime(7, "TRADE_DT", preparedStatement, pMessage);
            MessageUtils.setStatementDateTime(8, "UPDATE_BUSN_DT", preparedStatement, pMessage);
            MessageUtils.setStatementDateTime(9, "UPDATE_DATETIME", preparedStatement, pMessage);
            MessageUtils.setStatementString(10, "AUDIT_TYPE_CODE", preparedStatement, pMessage);
            MessageUtils.setStatementString(11, "TRADE_TYPE_CODE", preparedStatement, pMessage);
            MessageUtils.setStatementString(12, "TRADE_STAT_CODE", preparedStatement, pMessage);
            MessageUtils.setStatementString(13, "INST_CODE", preparedStatement, pMessage);
            MessageUtils.setStatementString(14, "CDTY_CODE", preparedStatement, pMessage);
            MessageUtils.setStatementString(15, "NOTIFY_CONTRACTS_FLAG", preparedStatement, pMessage);
            MessageUtils.setStatementString(16, "CMPNY_SHORT_NAME", preparedStatement, pMessage);
            MessageUtils.setStatementString(17, "BK_SHORT_NAME", preparedStatement, pMessage);
            MessageUtils.setStatementString(18, "CPTY_SHORT_NAME", preparedStatement, pMessage);
            MessageUtils.setStatementString(19, "BROKERSN", preparedStatement, pMessage);
            MessageUtils.setStatementString(20, "RFRNCE_SHORT_NAME", preparedStatement, pMessage);
            MessageUtils.setStatementString(21, "UPDATE_TABLE_NAME", preparedStatement, pMessage);
            preparedStatement.executeUpdate();
        }
        finally {
            try {
                if (preparedStatement != null)
                   preparedStatement.close();
            }
              catch (SQLException e) {}
            }
    }

    private int getNextSequence(String seqName) throws SQLException {
        int nextSeqNo = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try{
            statement = connection.prepareStatement("SELECT ops_tracking." + seqName +
                    ".nextval from dual");
            rs = statement.executeQuery();
            if (rs.next()) {
                nextSeqNo = (rs.getInt("nextval"));
        }
        }
        finally {
            if (statement != null){
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }
        return nextSeqNo;
    }
}
