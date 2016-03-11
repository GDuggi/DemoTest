package aff.confirm.common.dbqueue;


import java.sql.PreparedStatement;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Nov 25, 2003
 * Time: 7:00:07 AM
 * To change this template use Options | File Templates.
 */
public class QOpsTrackingTradeAlert extends QAlertBase{

    public QOpsTrackingTradeAlert(java.sql.Connection pConnection) throws SQLException {
        connection = pConnection;
        updateSQL = "Update JBOSSDBQ.Q_OT_TRADE_ALERT set " +
                    "PROCESSED_FLAG = ?," + //1
                    "PROCESSED_TS_GMT = sysdate " +
                    "where id = ? "; //2

        readyToProcessSQL = "SELECT * from JBOSSDBQ.Q_OT_TRADE_ALERT " +
                            "where PROCESSED_FLAG = 'N' order by ID";
    }

    public void insertQOtTradeAlert(QOpsTrackingTradeAlertRec pQOpsTrackingTradeAlertRec)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String insertSQL =
                    "Insert into JBOSSDBQ.Q_OT_TRADE_ALERT( " +
                    "ID, " + //-
                    "TRADING_SYSTEM, " + //1
                    "TRADE_ID, " + //2
                    "VERSION, " + //3
                    "AUDIT_TYPE_CODE, " + //4
                    "TRADE_TYPE_CODE, " + //5
                    "CDTY_CODE, " + //6
                    "EMP_ID, " + //7
                    "NOTIFY_CONTRACTS_FLAG ) " + //8
                    "values( JBOSSDBQ.SEQ_Q_OT_TRADE_ALERT.NEXTVAL, ?, ?, ?, ?, ?, ?, ?, ? )";

            statement = connection.prepareStatement(insertSQL);
            statement.setString(1, pQOpsTrackingTradeAlertRec.tradingSystem);
            statement.setDouble(2, pQOpsTrackingTradeAlertRec.tradeID);
            statement.setDouble(3, pQOpsTrackingTradeAlertRec.version );
            statement.setString(4, pQOpsTrackingTradeAlertRec.auditTypeCode);
            statement.setString(5, pQOpsTrackingTradeAlertRec.tradeTypeCode);
            statement.setString(6, pQOpsTrackingTradeAlertRec.cdtyCode);
            statement.setDouble(7, pQOpsTrackingTradeAlertRec.empId);
            statement.setString(8, pQOpsTrackingTradeAlertRec.notifyContractsFlag);
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

   /* public QOpsTrackingTradeAlertRec getQOpsTrackingTradeAlertRec(VRealtimeTradeAuditRec pVRealtimeTradeAuditRec){
        QOpsTrackingTradeAlertRec qOTTARec;
        qOTTARec = new QOpsTrackingTradeAlertRec();
        qOTTARec.id = pVRealtimeTradeAuditRec.tradeAuditId;
        qOTTARec.sourceSystemCode = pVRealtimeTradeAuditRec.sourceSystemCode;
        qOTTARec.tradeID = pVRealtimeTradeAuditRec.prmntTradeID;
        qOTTARec.version = pVRealtimeTradeAuditRec.version;
        qOTTARec.auditTypeCode = pVRealtimeTradeAuditRec.auditTypeCode;
        qOTTARec.tradeTypeCode = pVRealtimeTradeAuditRec.tradeTypeCode;
        qOTTARec.cdtyCode = pVRealtimeTradeAuditRec.cdtyCode;
        qOTTARec.empId = pVRealtimeTradeAuditRec.empId;
        qOTTARec.notifyContractsFlag = pVRealtimeTradeAuditRec.notifyContractsFlag;
        qOTTARec.processedFlag = "N";
        return qOTTARec;
    }*/

}
