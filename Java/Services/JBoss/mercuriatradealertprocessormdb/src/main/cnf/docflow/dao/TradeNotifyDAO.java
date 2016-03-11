package cnf.docflow.dao;

import cnf.docflow.data.ConfirmMessageData;
import cnf.docflow.util.ConversionUtils;
import cnf.docflow.util.TradeNotifyType;

import java.sql.*;
import java.text.ParseException;

/**
 * Created by jvega on 7/20/2015.
 */
public class TradeNotifyDAO extends SqlDAO {

    public TradeNotifyDAO(Connection connection) throws SQLException {
        this.dbConnection = connection;
    }

    public boolean alreadyExists(String pTradeID, String pTradeSystemCode) throws SQLException {
        double result = 0;
        PreparedStatement preparedStatement = null;
        ResultSet rs = null;
        String statementSQL = "";
        try {
            statementSQL = "select count(*) cnt from " + schemaName + ".trade t, " + schemaName + ".trade_notify tn where t.ID = tn.TRADE_ID and trd_sys_ticket = ? and trd_sys_code = ?";
            preparedStatement = dbConnection.prepareStatement(statementSQL);
            ConversionUtils.setStatementString(1, pTradeID, preparedStatement);
            ConversionUtils.setStatementString(2, pTradeSystemCode, preparedStatement);
            rs = preparedStatement.executeQuery();
            if (rs.next()) {
                result = rs.getDouble("cnt");
            }
        } finally {
            if (rs != null) {
                rs.close();
                rs = null;
            }
            if (preparedStatement != null) {
                preparedStatement.close();
            }
        }
        return result >= 1;
    }

    public int InsertData(ConfirmMessageData msgData) throws SQLException, ParseException {
        PreparedStatement preparedStatement = null;
        int nextSeqNo = 0;
        int tradeId = 0;
        try {
            if (msgData.getConfirm_NotifyType().equalsIgnoreCase(TradeNotifyType.NEW.getCode())) {
                tradeId = msgData.getOther_TradeID();
            } else{
                tradeId = getTradeID(msgData.getTrade_TradingSystemTicket(),msgData.getTrade_TradingSystemCode());
                msgData.setOther_TradeID(tradeId);
            }

            String seqName = "seq_trade_notify";
            nextSeqNo = getNextSequence(seqName);
            String stmtSQL =
                    "Insert into " + schemaName + ".TRADE_NOTIFY ( " +
                            "ID, " +                    //  1
                            "TRADE_ID, " +          //  2
                            // "TRADE_VERSION, " +          //  3
                            "NOTIFY_TIME_GMT, " +          //  4
                            "ACTION " +          //  5
                            // ") values( ?, ?, ?, CONVERT(datetime2(0),GETDATE()), ? )";
                            ") values( ?, ?, GETDATE(), ?)";


            preparedStatement = dbConnection.prepareStatement(stmtSQL);

            int i = 0;
            preparedStatement.setObject(++i, nextSeqNo, Types.NUMERIC);
            preparedStatement.setObject(++i, tradeId, Types.NUMERIC);
            //preparedStatement.setNull(++i, Types.NULL); //TRADE_VERSION
            //System.out.println("JVC: set TRADE_VERSION as null");
            //ConversionUtils.setStatementDateTime(++i, msgData.getConfirm_NotifyTsGmt(), preparedStatement);
            ConversionUtils.setStatementString(++i, msgData.getConfirm_NotifyType(), preparedStatement);
            //System.out.println("JVC: set ACTION as "+msgData.getConfirm_NotifyType());
            System.out.println("TradeNotifyDAO: sql = " + stmtSQL);
            System.out.println("TradeNotifyDAO: ID=" + nextSeqNo);
            System.out.println("TradeNotifyDAO: TRADE_ID=" + tradeId);
            System.out.println("TradeNotifyDAO: NOTIFY_TIME_GMT=GETDATE()");
            System.out.println("TradeNotifyDAO: ACTION=" + msgData.getConfirm_NotifyType());
            preparedStatement.executeUpdate();
        } finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
            } catch (SQLException e) {
            }
        }
        return nextSeqNo;
    }

    //not to be confused with Voiding Data
    public void DeleteData(ConfirmMessageData msgData) throws SQLException, ParseException {
        PreparedStatement preparedStatement = null;
        try {
                int tradeId = getTradeID(msgData.getTrade_TradingSystemTicket(),msgData.getTrade_TradingSystemCode());
                String stmtSQL =
                        "Delete from " + schemaName + ".TRADE_NOTIFY where " +
                                "TRADE_ID = ? ";

                preparedStatement = dbConnection.prepareStatement(stmtSQL);
                preparedStatement.setObject(1, tradeId, Types.NUMERIC);
                //System.out.println("JVC: set sql" + stmtSQL);
                preparedStatement.executeUpdate();
        } finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
            } catch (SQLException e) {
            }
        }
    }

}
