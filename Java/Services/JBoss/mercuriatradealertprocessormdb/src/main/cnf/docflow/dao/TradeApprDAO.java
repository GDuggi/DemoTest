package cnf.docflow.dao;

import cnf.docflow.data.ConfirmMessageData;
import java.sql.*;
import java.text.ParseException;
/**
 * Created by jvega on 10/14/2015.
 */
public class TradeApprDAO extends SqlDAO {

    public TradeApprDAO(Connection connection) throws SQLException {
        this.dbConnection = connection;
    }

    public Boolean getCanInsert(ConfirmMessageData msgData) throws SQLException {
        Boolean result = false;
        PreparedStatement preparedStatement = null;
        ResultSet rs = null;
        String statementSQL = "";
        try {
            statementSQL = "select FINAL_APPROVAL_FLAG from "+ schemaName + ".TRADE_SUMMARY where TRADE_ID = ? ";
            preparedStatement = dbConnection.prepareStatement(statementSQL);
            preparedStatement.setObject(1, msgData.getOther_TradeID(), Types.NUMERIC);
            rs = preparedStatement.executeQuery();
            if (rs.next()) {
                result = rs.getString("FINAL_APPROVAL_FLAG").equalsIgnoreCase("Y");
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

    public void InsertData(ConfirmMessageData msgData) throws SQLException {
        PreparedStatement preparedStatement = null;
        try {
                String seqName = "seq_trade_appr";
                int nextSeqNo = 0;
                nextSeqNo = getNextSequence(seqName);
                int tradeId = msgData.getOther_TradeID();
                String stmtSQL =
                        "Insert into " + schemaName + ".TRADE_APPR ( " +
                                "ID, " +
                                "TRADE_ID, " +
                                "APPR_FLAG " +
                                ") values( ?, ?, ?)";

                preparedStatement = dbConnection.prepareStatement(stmtSQL);
                int i = 0;
                preparedStatement.setObject(++i, nextSeqNo, Types.NUMERIC);
                preparedStatement.setObject(++i, tradeId, Types.NUMERIC);
                preparedStatement.setObject(++i, "N", Types.VARCHAR);
                System.out.println("TradeApprDAO: sql = " + stmtSQL);
                System.out.println("TradeApprDAO: ID=" + nextSeqNo);
                System.out.println("TradeApprDAO: TRADE_ID=" + tradeId);
                System.out.println("TradeApprDAO: APPR_FLAG="+"N");
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
