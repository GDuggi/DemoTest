package cnf.docflow.dao;

import cnf.docflow.data.ConfirmMessageData;
import cnf.docflow.util.ConversionUtils;

import java.sql.*;
import java.text.ParseException;

/**
 * Created by jvega on 7/17/2015.
 */
public class TradeDAO extends SqlDAO {

    public TradeDAO(Connection connection) throws SQLException {
        this.dbConnection = connection;
    }

    public boolean alreadyExists(String pTradeID, String pTradeSystemCode) throws SQLException {
        double result = 0;
        PreparedStatement preparedStatement = null;
        ResultSet rs = null;
        String statementSQL = "";
        try{
            statementSQL = "select count(*) cnt from "+ schemaName + ".trade where trd_sys_ticket = ? and trd_sys_code = ?";
            preparedStatement = dbConnection.prepareStatement(statementSQL);
            ConversionUtils.setStatementString(1, pTradeID, preparedStatement);
            ConversionUtils.setStatementString(2, pTradeSystemCode, preparedStatement);
            rs = preparedStatement.executeQuery();
            if (rs.next()) {
                result = rs.getDouble("cnt");
            }
        }
        finally {
            if (rs != null) {
                rs.close();
                rs = null;
            }
            if (preparedStatement != null){
                preparedStatement.close();
            }
        }
        return result >= 1;
    }

    public int InsertData(ConfirmMessageData msgData) throws SQLException, ParseException {
        //System.out.println("JVC: Insert TRADE");
        PreparedStatement preparedStatement = null;
        int nextSeqNo = 0;
        try {
            String seqName = "seq_trade";

            nextSeqNo = getNextSequence(seqName);
            String stmtSQL =
                    "Insert into " + schemaName + ".TRADE ( " +
                            "ID, " +
                            "TRD_SYS_CODE, " +
                            "TRD_SYS_TICKET" +
                            ") values( ?, ?, ? )";


            preparedStatement = dbConnection.prepareStatement(stmtSQL);

            preparedStatement.setObject(1, nextSeqNo, Types.NUMERIC);
            ConversionUtils.setStatementString(2, msgData.getTrade_TradingSystemCode(), preparedStatement);
            ConversionUtils.setStatementString(3, msgData.getTrade_TradingSystemTicket(), preparedStatement);
            System.out.println("TradeDAO: sql = " + stmtSQL);
            System.out.println("TradeDAO: ID=" + nextSeqNo);
            System.out.println("TradeDAO: TRD_SYS_CODE=" + msgData.getTrade_TradingSystemCode());
            System.out.println("TradeDAO: TRD_SYS_TICKET=" + msgData.getTrade_TradingSystemTicket());
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

    //testing only not to be confused with Voiding Data
    public void DeleteData(ConfirmMessageData msgData) throws SQLException, ParseException {
        PreparedStatement preparedStatement = null;
        int nextSeqNo = 0;
        try {
            String stmtSQL =
                    "Delete from " + schemaName + ".TRADE where " +
                            "TRD_SYS_CODE = ? " +
                            "and TRD_SYS_TICKET = ? ";

            preparedStatement = dbConnection.prepareStatement(stmtSQL);
            ConversionUtils.setStatementString(1, msgData.getTrade_TradingSystemCode(), preparedStatement);
            ConversionUtils.setStatementString(2, msgData.getTrade_TradingSystemTicket(), preparedStatement);
            //System.out.println("JVC: set sql - " + stmtSQL);
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


