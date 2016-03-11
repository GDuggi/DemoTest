package cnf.docflow.dao;

import cnf.docflow.data.ConfirmMessageData;
import cnf.docflow.util.ConversionUtils;
import cnf.docflow.util.TradeNotifyType;

import java.sql.*;
import java.text.ParseException;

/**
 * Created by jvega on 7/20/2015.
 */
public class TradeSummaryDAO extends SqlDAO {

    public TradeSummaryDAO(Connection connection) throws SQLException {
        this.dbConnection = connection;
    }

    public boolean alreadyExists(String pTradeID, String pTradeSystemCode) throws SQLException {
        double result = 0;
        PreparedStatement preparedStatement = null;
        ResultSet rs = null;
        String statementSQL = "";
        try{
            statementSQL = "select count(*) cnt from "+ schemaName + ".trade t, "+ schemaName + ".trade_summary ts where t.ID = ts.TRADE_ID and trd_sys_ticket = ? and trd_sys_code = ?";
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

    public void InsertData(ConfirmMessageData msgData) throws SQLException, ParseException {
        PreparedStatement preparedStatement = null;
        try {
                int tradeId = msgData.getOther_TradeID();
                String seqName = "seq_trade_summary";
                int nextSeqNo = 0;
                nextSeqNo = getNextSequence(seqName);
                String stmtSQL =
                        "Insert into " + schemaName + ".TRADE_SUMMARY ( " +
                                "ID, " +                    //  1
                                "TRADE_ID, " +          //  2
                                "OPEN_RQMTS_FLAG, " +  //Y is has rqmt
                                //"CATEGORY, " + //nullable ENRGY  TradeTypeCode
                                "LAST_UPDATE_TIMESTAMP_GMT, " + //sysdate
                                "FINAL_APPROVAL_FLAG, " + //N
                               // "CMT, " + //nullable
                                "LAST_TRD_EDIT_TIMESTAMP_GMT, " +//sysdate
                                "OPS_DET_ACT_FLAG, " + //N
                                "READY_FOR_FINAL_APPROVAL_FLAG, " + //N
                                "HAS_PROBLEM_FLAG " + //N
                               // "TRANSACTION_SEQ, " + //nullable  TradingSystemTicket?
                                //"FINAL_APPROVAL_TIMESTAMP_GMT, " + //nullable
                                ") values( ?, ?, ?, GETDATE(), ?, GETDATE(), ?, ?, ?)";


                preparedStatement = dbConnection.prepareStatement(stmtSQL);
               
                int i = 0;
                preparedStatement.setObject(++i, nextSeqNo, Types.NUMERIC);
                preparedStatement.setObject(++i, tradeId, Types.NUMERIC);
                preparedStatement.setObject(++i, "Y" , Types.VARCHAR); //OPEN_RQMTS_FLAG
                //ConversionUtils.setStatementString(++i, msgData.getTrade_TradeTypeCode(), preparedStatement); //CATEGORY
                preparedStatement.setObject(++i, "N" , Types.VARCHAR); //FINAL_APPROVAL_FLAG
                preparedStatement.setObject(++i, "N" , Types.VARCHAR); //OPS_DET_ACT_FLAG
                preparedStatement.setObject(++i, "N" , Types.VARCHAR); //READY_FOR_FINAL_APPROVAL_FLAG
                preparedStatement.setObject(++i, "N" , Types.VARCHAR); //HAS_PROBLEM_FLAG
                System.out.println("TradeSummaryDAO: set sql = " + stmtSQL);
                System.out.println("TradeSummaryDAO: ID="+nextSeqNo);
                System.out.println("TradeSummaryDAO: TRADE_ID="+tradeId);
                System.out.println("TradeSummaryDAO: OPEN_RQMTS_FLAG="+"Y");
                //System.out.println("TradeSummaryDAO: CATEGORY="+msgData.getTrade_TradeTypeCode());
                System.out.println("TradeSummaryDAO: LAST_UPDATE_TIMESTAMP_GMT=GETDATE()");
                System.out.println("TradeSummaryDAO: FINAL_APPROVAL_FLAG="+"N");
                System.out.println("TradeSummaryDAO: LAST_TRD_EDIT_TIMESTAMP_GMT=GETDATE()");
                System.out.println("TradeSummaryDAO: OPS_DET_ACT_FLAG="+"N");
                System.out.println("TradeSummaryDAO: READY_FOR_FINAL_APPROVAL_FLAG="+"N");
                System.out.println("TradeSummaryDAO: HAS_PROBLEM_FLAG="+"N");
                preparedStatement.executeUpdate();
        } finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
            } catch (SQLException e) {
            }
        }
    }

    public void UpdateData(ConfirmMessageData msgData) throws SQLException {
        PreparedStatement preparedStatement = null;
        try {
            int tradeId = getTradeID(msgData.getTrade_TradingSystemTicket(),msgData.getTrade_TradingSystemCode());
            String stmtSQL =
                    "update " + schemaName + ".TRADE_SUMMARY SET " +
                            "OPS_DET_ACT_FLAG = ?, " +
                            "READY_FOR_FINAL_APPROVAL_FLAG = ?, " +
                            "FINAL_APPROVAL_FLAG = ? " +
                            "where TRADE_ID = ? ";

            preparedStatement = dbConnection.prepareStatement(stmtSQL);
            int i = 0;
            preparedStatement.setObject(++i, "Y", Types.VARCHAR);
            preparedStatement.setObject(++i, "N", Types.VARCHAR);
            preparedStatement.setObject(++i, "N", Types.VARCHAR);
            preparedStatement.setObject(++i, tradeId, Types.NUMERIC);
            System.out.println("JVC: set sql - " + stmtSQL);
            System.out.println("TradeSummaryDAO: OPS_DET_ACT_FLAG="+"Y");
            System.out.println("TradeSummaryDAO: READY_FOR_FINAL_APPROVAL_FLAG="+"N");
            System.out.println("TradeSummaryDAO: FINAL_APPROVAL_FLAG="+"N");
            System.out.println("TradeSummaryDAO: TRADE_ID="+tradeId);
            int iUpd = preparedStatement.executeUpdate();
            System.out.println("TradeSummaryDAO: Records Affected=" + iUpd);
        } finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
            } catch (SQLException e) {
            }
        }
    }

    //testing only
    public void DeleteData(ConfirmMessageData msgData) throws SQLException, ParseException {
        PreparedStatement preparedStatement = null;
        try {
                int tradeId = getTradeID(msgData.getTrade_TradingSystemTicket(),msgData.getTrade_TradingSystemCode());
                String stmtSQL =
                        "Delete from " + schemaName + ".TRADE_SUMMARY where " +
                                "TRADE_ID = ? ";

                preparedStatement = dbConnection.prepareStatement(stmtSQL);
                preparedStatement.setObject(1, tradeId, Types.NUMERIC);
            //  System.out.println("JVC: set sql" + stmtSQL);
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