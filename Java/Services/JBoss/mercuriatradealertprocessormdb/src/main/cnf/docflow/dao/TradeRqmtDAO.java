package cnf.docflow.dao;

import cnf.docflow.data.ConfirmMessageData;
import cnf.docflow.data.ConfirmRequirementData;
import cnf.docflow.util.ConversionUtils;
import cnf.docflow.util.RequirementType;
import cnf.docflow.util.TradeNotifyType;
import com.microsoft.sqlserver.jdbc.SQLServerCallableStatement;

import java.sql.*;
import java.text.ParseException;
import java.util.List;

/**
 * Created by jvega on 7/21/2015.
 */
public class TradeRqmtDAO extends SqlDAO {

    public TradeRqmtDAO(Connection connection) throws SQLException {
        this.dbConnection = connection;
    }

    public boolean alreadyExists(String pTradeID, String pTradeSystemCode) throws SQLException {
        double result = 0;
        PreparedStatement preparedStatement = null;
        ResultSet rs = null;
        String statementSQL = "";
        try{
            statementSQL = "select count(*) cnt from "+ schemaName + ".trade t, "+ schemaName + ".trade_rqmt tr where t.ID = tr.TRADE_ID and trd_sys_ticket = ? and trd_sys_code = ?";
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

    public int getTradeNotifyID(String pTradeID, String pTradeSystemCode) throws SQLException {
        int result = 0;
        PreparedStatement preparedStatement = null;
        ResultSet rs = null;
        String statementSQL = "";
        try {
            statementSQL = "select max(tr.id) as id from "+ schemaName + ".trade t, "+ schemaName + ".trade_notify tn where t.ID = tn.TRADE_ID and trd_sys_ticket = ? and trd_sys_code = ?";
            preparedStatement = dbConnection.prepareStatement(statementSQL);
            ConversionUtils.setStatementString(1, pTradeID, preparedStatement);
            ConversionUtils.setStatementString(2, pTradeSystemCode, preparedStatement);
            rs = preparedStatement.executeQuery();
            if (rs.next()) {
                result = rs.getInt("id");
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
        return result;
    }

    public String getRqmtInitialStatusCode(String pRequirementCode) throws SQLException {
        String result = "";
        PreparedStatement preparedStatement = null;
        ResultSet rs = null;
        String statementSQL = "";
        try {
            statementSQL = "select initial_status from "+ schemaName + ".rqmt r where r.code = ? and r.active_flag ='Y'";
            preparedStatement = dbConnection.prepareStatement(statementSQL);
            ConversionUtils.setStatementString(1, pRequirementCode, preparedStatement);
            rs = preparedStatement.executeQuery();
            if (rs.next()) {
                result = rs.getString("initial_status");
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
        return result;
    }

    public void InsertData(ConfirmMessageData msgData) throws SQLException, ParseException {
            int tradeId = msgData.getOther_TradeID();
            int tradeNotifyId = msgData.getOther_TradeNotifyID();
            List<ConfirmRequirementData> rqmtlist = msgData.getConfRqmtList();

            if (rqmtlist!=null && !rqmtlist.isEmpty()) {
                for (int x = 0; x < rqmtlist.size(); x++) {
                    ConfirmRequirementData rqmtItem = rqmtlist.get(x);
                    //String rqmtStatus = RequirementType.getRequirmentCode(rqmtItem.getRqmt_Action() + rqmtItem.getRqmt_Party() + rqmtItem.getRqmt_Method());
                    String rqmtStatus = RequirementType.getRequirmentCode(rqmtItem.getRqmt_Workflow());
                    // System.out.println("TradeRqmtDAO: rqmtStatus - " + rqmtStatus);
                    String intialStatus = getRqmtInitialStatusCode(rqmtStatus);
                    //System.out.println("TradeRqmtDAO: intialStatus - " + intialStatus);
                    PreparedStatement preparedStatement = null;

                    try {
                        String seqName = "seq_trade_rqmt";
                        int nextSeqNo = 0;
                        nextSeqNo = getNextSequence(seqName);
                        String stmtSQL =
                                "Insert into " + schemaName + ".TRADE_RQMT ( " +
                                        "ID, " +
                                        "TRADE_ID, " +
                                        "RQMT_TRADE_NOTIFY_ID, " +
                                        "RQMT, " +
                                        "STATUS " +
                                        //"COMPLETED_DT, " +
                                        //"COMPLETED_TIMESTAMP_GMT, " +
                                        //"REFERENCE, " +
                                        //"CANCEL_TRADE_NOTIFY_ID, " +
                                        //"CMT, " +
                                        //"SECOND_CHECK_FLAG " +
                                        //") values( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                        ") values( ?, ?, ?, ?, ?)";


                        preparedStatement = dbConnection.prepareStatement(stmtSQL);
                        int i = 0;
                        preparedStatement.setObject(++i, nextSeqNo, Types.NUMERIC);
                        preparedStatement.setObject(++i, tradeId, Types.NUMERIC);
                        preparedStatement.setObject(++i, tradeNotifyId, Types.NUMERIC);
                        preparedStatement.setObject(++i, rqmtStatus, Types.VARCHAR);
                        preparedStatement.setObject(++i, intialStatus, Types.VARCHAR);
                        //preparedStatement.setNull(++i, Types.TIMESTAMP); //COMPLETED_DT
                        //preparedStatement.setNull(++i, Types.TIMESTAMP); //COMPLETED_TIMESTAMP_GMT
                        //preparedStatement.setNull(++i, Types.VARCHAR); //REFERENCE
                        //preparedStatement.setNull(++i, Types.NUMERIC); //CANCEL_TRADE_NOTIFY_ID
                        //preparedStatement.setNull(++i, Types.VARCHAR); //CMT
                        //preparedStatement.setNull(++i, Types.VARCHAR); //SECOND_CHECK_FLAG
                        System.out.println("TradeRqmtDAO: sql = " + stmtSQL);
                        System.out.println("TradeRqmtDAO: ID=" + nextSeqNo);
                        System.out.println("TradeRqmtDAO: TRADE_ID=" + tradeId);
                        System.out.println("TradeRqmtDAO: RQMT_TRADE_NOTIFY_ID=" + tradeNotifyId);
                        System.out.println("TradeRqmtDAO: RQMT=" + rqmtStatus);
                        System.out.println("TradeRqmtDAO: STATUS=" + intialStatus);
                        preparedStatement.executeUpdate();
                        msgData.getConfRqmtList().get(x).setOther_TradeRqmtID(nextSeqNo);

                    } finally {
                        try {
                            if (preparedStatement != null)
                                preparedStatement.close();
                        } catch (SQLException e) {
                        }
                    }
                }
            }
    }


    public void CancelAllRqmts(ConfirmMessageData msgData) throws SQLException {
        int tradeId = getTradeID(msgData.getTrade_TradingSystemTicket(),msgData.getTrade_TradingSystemCode());
        CallableStatement statement = null;
        try {
            String stmtSQL = "{call " + schemaName + ".PKG_TRADE_RQMT$P_CANCEL_ALL_RQMTS(?,?) }";
            statement = dbConnection.prepareCall(stmtSQL);
            statement.setDouble(1, tradeId);
            statement.setString(2, msgData.getConfirm_NotifyType());
            System.out.println("JVC: set sql - " + stmtSQL);
            System.out.println("TradeRqmtDAO: TRADE_ID="+tradeId);
            System.out.println("TradeRqmtDAO: CXL_CMT="+msgData.getConfirm_NotifyType());
            statement.executeUpdate();
        } finally {
            if (statement != null)
            statement.close();
        }
    }
    //testing only
    public void DeleteData(ConfirmMessageData msgData) throws SQLException, ParseException {
        PreparedStatement preparedStatement = null;
        try {
                int tradeId = getTradeID(msgData.getTrade_TradingSystemTicket(),msgData.getTrade_TradingSystemCode());
                String stmtSQL =
                        "Delete from " + schemaName + ".TRADE_RQMT where " +
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