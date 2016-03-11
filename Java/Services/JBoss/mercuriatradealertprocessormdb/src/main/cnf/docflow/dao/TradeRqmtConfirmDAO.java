package cnf.docflow.dao;

import cnf.docflow.data.ConfirmMessageData;
import cnf.docflow.data.ConfirmRequirementData;
import cnf.docflow.data.ConfirmRequirementSendToData;
import cnf.docflow.util.ConversionUtils;
import cnf.docflow.util.RequirementType;

import java.sql.*;
import java.text.ParseException;
import java.util.List;

/**
 * Created by jvega on 9/21/2015.
 */
public class TradeRqmtConfirmDAO extends SqlDAO {

    public TradeRqmtConfirmDAO(Connection connection) throws SQLException {
        this.dbConnection = connection;
    }

    public boolean alreadyExists(String pTradeID, String pTradeSystemCode) throws SQLException {
        double result = 0;
        PreparedStatement preparedStatement = null;
        ResultSet rs = null;
        String statementSQL = "";
        try{
            statementSQL = "select count(*) cnt from "+ schemaName + ".trade t, "+ schemaName + ".trade_rqmt_confirm tr where t.ID = tr.TRADE_ID and trd_sys_ticket = ? and trd_sys_code = ?";
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
        int tradeId = msgData.getOther_TradeID();
        List<ConfirmRequirementData> rqmtlist = msgData.getConfRqmtList();
        for (ConfirmRequirementData rqmtItem : rqmtlist) {
            int tradeRqmtId = rqmtItem.getOther_TradeRqmtID();

            String tradeRqmtWorkflow = rqmtItem.getRqmt_Workflow();
            String tradeRqmtTemplate = rqmtItem.getRqmt_Template();
            String tradeRqmtPreparerCanSend = rqmtItem.getRqmt_PreparerCanSend();
            if(!tradeRqmtPreparerCanSend.equals("Y"))
                tradeRqmtPreparerCanSend ="N";
            String SendToInd = "";
            String SendToNumber = "";
            System.out.println("Rqmt: Workflow("+tradeRqmtWorkflow+") = ("+RequirementType.OURPAPER.getCode()+")");
            List<ConfirmRequirementSendToData> rqmtSendTolist = rqmtItem.getConfRqmtSendToList();
            if (tradeRqmtWorkflow.equalsIgnoreCase(RequirementType.OURPAPER.getCode())) {
                if (rqmtSendTolist != null && !rqmtSendTolist.isEmpty()) {
                    for (ConfirmRequirementSendToData SendToItem : rqmtSendTolist) {
                        if (SendToItem.getRqmtSendTo_transmitMethodInd().isEmpty()) {
                            SendToInd = "";
                        } else {
                            SendToInd = SendToItem.getRqmtSendTo_transmitMethodInd().substring(0, 1);
                        }
                        if (SendToInd.equalsIgnoreCase("F")) {
                            SendToNumber = SendToItem.getRqmtSendTo_faxCountryCode() + SendToItem.getRqmtSendTo_faxAreaCode() + SendToItem.getRqmtSendTo_faxLocalNumber();
                        } else {
                            SendToNumber = SendToItem.getRqmtSendTo_emailAddress();
                        }

                        PreparedStatement preparedStatement = null;

                        try {
                            String seqName = "seq_trade_rqmt_confirm";
                            int nextSeqNo = 0;
                            nextSeqNo = getNextSequence(seqName);
                            String stmtSQL =
                                    "Insert into " + schemaName + ".TRADE_RQMT_CONFIRM ( " +
                                            "ID, " +
                                            "RQMT_ID, " +
                                            "TRADE_ID, " +
                                            "TEMPLATE_NAME, " +
                                            //"CONFIRM_CMT, " +
                                            "FAX_TELEX_IND, " +
                                            "FAX_TELEX_NUMBER, " +
                                            //"CONFIRM_LABEL, " +
                                            //"NEXT_STATUS_CODE " +
                                            "PREPARER_CAN_SEND_FLAG " +
                                            ") values( ?, ?, ?, ?, ?, ?, ?)";


                            preparedStatement = dbConnection.prepareStatement(stmtSQL);
                            int i = 0;
                            preparedStatement.setObject(++i, nextSeqNo, Types.NUMERIC);
                            preparedStatement.setObject(++i, tradeRqmtId, Types.NUMERIC);
                            preparedStatement.setObject(++i, tradeId, Types.NUMERIC);
                            preparedStatement.setObject(++i, tradeRqmtTemplate, Types.VARCHAR);
                            preparedStatement.setObject(++i, SendToInd, Types.VARCHAR);
                            preparedStatement.setObject(++i, SendToNumber, Types.VARCHAR);
                            preparedStatement.setObject(++i, tradeRqmtPreparerCanSend, Types.VARCHAR);

                            System.out.println("TradeRqmtConfirmDAO: sql = " + stmtSQL);
                            System.out.println("TradeRqmtConfirmDAO: ID=" + nextSeqNo);
                            System.out.println("TradeRqmtConfirmDAO: RQMT_ID=" + nextSeqNo);
                            System.out.println("TradeRqmtConfirmDAO: TRADE_ID=" + tradeId);
                            System.out.println("TradeRqmtConfirmDAO: TEMPLATE_NAME=" + tradeRqmtTemplate);
                            System.out.println("TradeRqmtConfirmDAO: PREPARER_CAN_SEND_FLAG =" + tradeRqmtPreparerCanSend);
                            System.out.println("TradeRqmtConfirmDAO: FAX_TELEX_IND=" + SendToInd + " based on transmitMethodInd(" + SendToItem.getRqmtSendTo_transmitMethodInd() + ")");
                            System.out.println("TradeRqmtConfirmDAO: FAX_TELEX_NUMBER=" + SendToNumber + " based on faxCountryCode/faxAreaCode/faxLocalNumber( " + SendToItem.getRqmtSendTo_faxCountryCode() + "/" + SendToItem.getRqmtSendTo_faxAreaCode() + "/" + SendToItem.getRqmtSendTo_faxLocalNumber() + " or emailAddress( " + SendToItem.getRqmtSendTo_emailAddress());
                            preparedStatement.executeUpdate();
                        } finally {
                            try {
                                if (preparedStatement != null)
                                    preparedStatement.close();
                            } catch (SQLException e) {
                            }
                        }
                    }
                } else {
                    //insert placeholder row
                    PreparedStatement preparedStatement = null;

                    try {
                        String seqName = "seq_trade_rqmt_confirm";
                        int nextSeqNo = 0;
                        nextSeqNo = getNextSequence(seqName);
                        String stmtSQL =
                                "Insert into " + schemaName + ".TRADE_RQMT_CONFIRM ( " +
                                        "ID, " +
                                        "RQMT_ID, " +
                                        "TRADE_ID, " +
                                        "TEMPLATE_NAME " +
                                        "PREPARER_CAN_SEND_FLAG " +
                                        ") values( ?, ?, ?, ?, ?)";


                        preparedStatement = dbConnection.prepareStatement(stmtSQL);
                        int i = 0;
                        preparedStatement.setObject(++i, nextSeqNo, Types.NUMERIC);
                        preparedStatement.setObject(++i, tradeRqmtId, Types.NUMERIC);
                        preparedStatement.setObject(++i, tradeId, Types.NUMERIC);
                        preparedStatement.setObject(++i, tradeRqmtTemplate, Types.VARCHAR);
                        preparedStatement.setObject(++i, tradeRqmtPreparerCanSend, Types.VARCHAR);

                        System.out.println("TradeRqmtConfirmDAO: sql = " + stmtSQL);
                        System.out.println("TradeRqmtConfirmDAO: ID=" + nextSeqNo);
                        System.out.println("TradeRqmtConfirmDAO: RQMT_ID=" + nextSeqNo);
                        System.out.println("TradeRqmtConfirmDAO: TRADE_ID=" + tradeId);
                        System.out.println("TradeRqmtConfirmDAO: TEMPLATE_NAME=" + tradeRqmtTemplate);
                        System.out.println("TradeRqmtConfirmDAO: PREPARER_CAN_SEND_FLAG =" + tradeRqmtPreparerCanSend);
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
        }
    }

    //testing only
    public void DeleteData(ConfirmMessageData msgData) throws SQLException, ParseException {
        PreparedStatement preparedStatement = null;
        try {
            int tradeId = getTradeID(msgData.getTrade_TradingSystemTicket(),msgData.getTrade_TradingSystemCode());
            String stmtSQL =
                    "Delete from " + schemaName + ".TRADE_RQMT_CONFIRM where " +
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