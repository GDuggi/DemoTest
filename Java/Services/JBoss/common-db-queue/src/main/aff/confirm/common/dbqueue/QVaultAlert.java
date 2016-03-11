package aff.confirm.common.dbqueue;



import java.sql.PreparedStatement;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Sep 22, 2005
 * Time: 7:00:07 AM
 * To change this template use Options | File Templates.
 */
public class QVaultAlert extends QAlertBase{

    public QVaultAlert(java.sql.Connection pConnection) throws SQLException {
        connection = pConnection;
        updateSQL = "Update JBOSSDBQ.Q_VAULT_ALERT set " +
                    "PROCESSED_FLAG = ?," + //1
                    "PROCESSED_TS_GMT = sysdate " +
                    "where id = ? "; //2


        //Object is to pick out JMS trades first. rownum alone will override sort order
        readyToProcessSQL = " SELECT /*+ index (t IND_AVA_TS) */  * "   +
                            " FROM jbossdbq.q_vault_alert t "   +
                            " WHERE processed_flag = 'N' AND ROWNUM < 11 AND trading_system = 'JMS' "   +
                            " UNION"   +
                            " SELECT /*+ index (t IND_AVA_TS) */   *"   +
                            " FROM jbossdbq.q_vault_alert t"   +
                            " WHERE processed_flag = 'N' AND ROWNUM < 11"   +
                            " ORDER BY 2 DESC, 14"  ;


 //       readyToProcessSQL = " SELECT * from JBOSSDBQ.Q_VAULT_ALERT"   +
  //                          " where ID = 6059376 and processed_flag = 'N'";
    }

    public void insertQVaultAlert(QVaultAlertRec pQVaultAlertRec) throws SQLException {
        PreparedStatement statement = null;
        try {
            String insertSQL =
                    "Insert into JBOSSDBQ.Q_VAULT_ALERT( " +
                    "ID, " + //-
                    "TRADING_SYSTEM, " + //1
                    "TRADE_ID, " + //2
                    "VERSION, " + //3
                    "FILE_NAME, " + //4
                    "IDX_FIELDS, " + //5
                    "IDX_VALUES, " + //6
                    "LOG_DEALSHEET_ID, " + //7
                    "TITLE, " + //8
                    "AUTHOR, " + //9
                    "DOC_TYPE, " + //10
                    "DESCRIPTION, " + //11
                    "ON_BEHALF_OF ) " + //12
                    "values( JBOSSDBQ.SEQ_Q_VAULT_ALERT.NEXTVAL, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ? )";

            statement = connection.prepareStatement(insertSQL);
            statement.setString(1, pQVaultAlertRec.tradingSystem);
            statement.setDouble(2, pQVaultAlertRec.tradeId);
            statement.setInt(3, pQVaultAlertRec.version);
            statement.setString(4, pQVaultAlertRec.fileName);
            statement.setString(5, pQVaultAlertRec.idxFields);
            statement.setString(6, pQVaultAlertRec.idxValues );
            statement.setDouble(7, pQVaultAlertRec.logDealsheetId);
            statement.setString(8, pQVaultAlertRec.title );
            statement.setString(9, pQVaultAlertRec.author );
            statement.setString(10, pQVaultAlertRec.docType );
            statement.setString(11, pQVaultAlertRec.description );
            statement.setString(12, pQVaultAlertRec.onBehalfOf );
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

}
