package aff.confirm.common.dbqueue;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Nov 25, 2003
 * Time: 7:00:07 AM
 * To change this template use Options | File Templates.
 */
public class QAffinityTradeAlert extends QAlertBase {

    public QAffinityTradeAlert(java.sql.Connection pConnection) throws SQLException {
        connection = pConnection;
        updateSQL = "Update infinity_mgr.AFFINITY_TRADE_ALERT set " +
                    "PROCESSED_FLAG = ?," + //1
                    "PROCESSED_TS_GMT = sysdate " +
                    "where id = ? "; //2

        readyToProcessSQL = "SELECT * from infinity_mgr.AFFINITY_TRADE_ALERT " +
                            "where PROCESSED_FLAG = 'N' order by ID";
    }

    //Get it from here since it has the necessary db connection
    public QSempraTradeAlertRec getQSempraTradeAlertRec(double pTradeAuditId)
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
                qSTARec.tradingSystem = rs.getString("TRADING_SYSTEM");
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
    }


    /*public VRealtimeTradeAuditRec getVRealtimeTradeAuditRec(double pTradeAuditId)
            throws SQLException {
        VRealtimeTradeAuditRec vRTARec = new VRealtimeTradeAuditRec();
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
                vRTARec.tradeAuditId = rs.getDouble("TRADE_AUDIT_ID");
                vRTARec.prmntTradeID = rs.getDouble("PRMNT_TRADE_ID");
                vRTARec.version = rs.getDouble("VERSION");
                vRTARec.updateDateTime = rs.getDate("UPDATE_DATETIME");
                vRTARec.empId = rs.getDouble("EMP_ID");
                vRTARec.updateTableName = rs.getString("UPDATE_TABLE_NAME");
                vRTARec.auditTypeCode = rs.getString("AUDIT_TYPE_CODE");
                vRTARec.updateBusnDt = rs.getDate("UPDATE_BUSN_DT");
                vRTARec.tradeTypeCode = rs.getString("TRADE_TYPE_CODE");
                vRTARec.tradeStatCode = rs.getString("TRADE_STAT_CODE");
                vRTARec.tradeDt = rs.getDate("TRADE_DT");
                vRTARec.cmpnyShortName = rs.getString("CMPNY_SHORT_NAME");
                vRTARec.bkShortName = rs.getString("BK_SHORT_NAME");
                vRTARec.cptyShortName = rs.getString("CPTY_SHORT_NAME");
                vRTARec.cdtyCode = rs.getString("CDTY_CODE");
                vRTARec.brokerSn = rs.getString("BROKERSN");
                vRTARec.sourceSystemCode = rs.getString("TRADING_SYSTEM");
                vRTARec.instCode = rs.getString("INST_CODE");
                vRTARec.notifyContractsFlag = rs.getString("NOTIFY_CONTRACTS_FLAG");
                vRTARec.rfrnceShortName = rs.getString("RFRNCE_SHORT_NAME");
                vRTARec.tradeSttlTypeCode = rs.getString("TRADE_STTL_TYPE_CODE");
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
        return vRTARec;
    }*/

}
