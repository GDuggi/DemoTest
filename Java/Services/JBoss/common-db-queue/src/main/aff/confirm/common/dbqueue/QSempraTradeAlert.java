package aff.confirm.common.dbqueue;

import java.sql.PreparedStatement;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Nov 25, 2003
 * Time: 7:00:07 AM
 * To change this template use Options | File Templates.
 */
public class QSempraTradeAlert extends QAlertBase {

    public QSempraTradeAlert(java.sql.Connection pConnection) throws SQLException {
        connection = pConnection;
        updateSQL = "Update jbossdbq.Q_SEMPRA_TRADE_ALERT set " +
                    "PROCESSED_FLAG = ?," + //1
                    "PROCESSED_TS_GMT = sysdate " +
                    "where id = ? "; //2

        readyToProcessSQL = "SELECT * from jbossdbq.Q_SEMPRA_TRADE_ALERT " +
                            "where PROCESSED_FLAG = 'N' order by ID";
    }


    public void insertQSempraTradeAlert(QSempraTradeAlertRec pQSempraTradeAlertRec)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String insertSQL =
                    "Insert into jbossdbq.Q_SEMPRA_TRADE_ALERT( " +
                    "ID, " + //-
                    "TRADE_AUDIT_ID, " + //1
                    "PRMNT_TRADE_ID, " + //2
                    "VERSION, " + //3
                    "UPDATE_DATETIME, " + //4
                    "EMP_ID, " + //5
                    "UPDATE_TABLE_NAME, " + //6
                    "AUDIT_TYPE_CODE, " + //7
                    "UPDATE_BUSN_DT, " + //8
                    "TRADE_TYPE_CODE, " + //9
                    "TRADE_STAT_CODE, " + //10
                    "TRADE_DT, " + //11
                    "CMPNY_SHORT_NAME, " + //12
                    "BK_SHORT_NAME, " + //13
                    "CPTY_SHORT_NAME, " + //14
                    "CDTY_CODE, " + //15
                    "BROKERSN, " + //16
                    "TRADING_SYSTEM, " + //17
                    "INST_CODE, " + //18
                    "NOTIFY_CONTRACTS_FLAG, " + //19
                    "RFRNCE_SHORT_NAME, " + //20
                    "TRADE_STTL_TYPE_CODE ) " + //21
                    "values( jbossdbq.SEQ_Q_SEMPRA_TRADE_ALERT.NEXTVAL, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ? )";

            statement = connection.prepareStatement(insertSQL);
            statement.setDouble(1, pQSempraTradeAlertRec.tradeAuditId);
            statement.setDouble(2, pQSempraTradeAlertRec.prmntTradeID);
            statement.setDouble(3, pQSempraTradeAlertRec.version );
            statement.setDate(4, pQSempraTradeAlertRec.updateDateTime);
            statement.setDouble(5, pQSempraTradeAlertRec.empId);
            statement.setString(6, pQSempraTradeAlertRec.updateTableName);
            statement.setString(7, pQSempraTradeAlertRec.auditTypeCode);
            statement.setDate(8, pQSempraTradeAlertRec.updateBusnDt);
            statement.setString(9, pQSempraTradeAlertRec.tradeTypeCode);
            statement.setString(10, pQSempraTradeAlertRec.tradeStatCode);
            statement.setDate(11, pQSempraTradeAlertRec.tradeDt);
            statement.setString(12, pQSempraTradeAlertRec.cmpnyShortName);
            statement.setString(13, pQSempraTradeAlertRec.bkShortName);
            statement.setString(14, pQSempraTradeAlertRec.cptyShortName);
            statement.setString(15, pQSempraTradeAlertRec.cdtyCode);
            statement.setString(16, pQSempraTradeAlertRec.brokerSn);
            statement.setString(17, pQSempraTradeAlertRec.tradingSystem);
            statement.setString(18, pQSempraTradeAlertRec.instCode);
            statement.setString(19, pQSempraTradeAlertRec.notifyContractsFlag);
            statement.setString(20, pQSempraTradeAlertRec.rfrnceShortName);
            statement.setString(21, pQSempraTradeAlertRec.tradeSttlTypeCode);
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

    /*public QSempraTradeAlertRec getQSempraTradeAlertRec(double pTradeAuditId)
            throws SQLException {
        QSempraTradeAlertRec qSTARec = new QSempraTradeAlertRec();
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select * from infinity_mgr.V_REALTIME_TRADE_AUDIT " +
                    "where TRADE_AUDIT_ID = ? ";
            statement = connection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeAuditId);
            rs = statement.executeQuery();
            if (rs.next()) {
                qSTARec.tradeAuditId = rs.getDouble("TRADE_AUDIT_ID");
                qSTARec.prmntTradeID = rs.getDouble("PRMNT_TRADE_ID");
                qSTARec.version = rs.getDouble("VERSION");
                qSTARec.updateDateTime = rs.getDate("UPDATE_DATETIME");
                qSTARec.empId = rs.getDouble("EMP_ID");
                qSTARec.updateTableName = rs.getString("UPDATE_TABLE_NAME");
                qSTARec.auditTypeCode = rs.getString("AUDIT_TYPE_CODE");
                qSTARec.updateBusnDt = rs.getDate("UPDATE_BUSN_DT");
                qSTARec.tradeTypeCode = rs.getString("TRADE_TYPE_CODE");
                qSTARec.tradeStatCode = rs.getString("TRADE_STAT_CODE");
                qSTARec.tradeDt = rs.getDate("TRADE_DT");
                qSTARec.cmpnyShortName = rs.getString("CMPNY_SHORT_NAME");
                qSTARec.bkShortName = rs.getString("BK_SHORT_NAME");
                qSTARec.cptyShortName = rs.getString("CPTY_SHORT_NAME");
                qSTARec.cdtyCode = rs.getString("CDTY_CODE");
                qSTARec.brokerSn = rs.getString("BROKERSN");
                qSTARec.sourceSystemCode = rs.getString("TRADING_SYSTEM");
                qSTARec.instCode = rs.getString("INST_CODE");
                qSTARec.notifyContractsFlag = rs.getString("NOTIFY_CONTRACTS_FLAG");
                qSTARec.rfrnceShortName = rs.getString("RFRNCE_SHORT_NAME");
                qSTARec.tradeSttlTypeCode = rs.getString("TRADE_STTL_TYPE_CODE");
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
        return qSTARec;
    }*/



}
