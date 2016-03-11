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
public class QEConfirmAlert extends QAlertBase{
    private String clickAndConfirmExistsSQL = "SELECT ID from JBOSSDBQ.Q_ECONFIRM_ALERT " +
                            "where PROCESSED_FLAG = 'N' AND TRADE_ID = ? AND CLICK_AND_CONFIRM_FLAG = 'Y'";

    private String clearClickAndConfirmAlertRec = "Update JBOSSDBQ.Q_ECONFIRM_ALERT set " +
                            "CLICK_AND_CONFIRM_FLAG = 'N' where id = ?";

    public QEConfirmAlert(java.sql.Connection pConnection) throws SQLException {
        connection = pConnection;
        updateSQL = "Update JBOSSDBQ.Q_ECONFIRM_ALERT set " +
                    "PROCESSED_FLAG = ?," + //1
                    "PROCESSED_TS_GMT = sysdate " +
                    "where id = ? "; //2

        readyToProcessSQL = "SELECT * from JBOSSDBQ.Q_ECONFIRM_ALERT " +
                            "where PROCESSED_FLAG = 'N' order by ID";

    }

    public long clickAndConfirmExists(QEConfirmAlertRec pQEConfirmAlertRec) throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String sql = clickAndConfirmExistsSQL;
            statement = connection.prepareStatement(sql);
            statement.setDouble(1, pQEConfirmAlertRec.tradeID);
            rs = statement.executeQuery();
            if (rs.next())
                return rs.getLong("ID");
            else return -1;
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

    public void clearClickAndConfirmAlert(long id) throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String sql = clearClickAndConfirmAlertRec;
            statement = connection.prepareStatement(sql);
            statement.setDouble(1, id);
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

    public void insertQEConfirmAlert(QEConfirmAlertRec pQEConfirmAlertRec) throws SQLException {
        PreparedStatement statement = null;
        long clickConfirmRecID = -1;

        try {
            String insertSQL =
                    "Insert into JBOSSDBQ.Q_ECONFIRM_ALERT( " +
                    "ID, " + //-
                    "TRADING_SYSTEM, " + //1
                    "TRADE_ID, " + //2
                    "EC_PRODUCT_ID, " + //3
                    "EC_ACTION, " + //4
                    // MThoresen 4-18-2007: Added for click and confirm
                    "CLICK_AND_CONFIRM_FLAG,"  + //5
                    "EC_BKR_ACTION  ) " +
                    "values( JBOSSDBQ.SEQ_Q_ECONFIRM_ALERT.NEXTVAL, ?, ?, ?, ?,?, ?)";
            statement = connection.prepareStatement(insertSQL);
            statement.setString(1, pQEConfirmAlertRec.tradingSystem);
            statement.setDouble(2, pQEConfirmAlertRec.tradeID);
            statement.setInt(3, pQEConfirmAlertRec.ecProductID );
            statement.setString(4, pQEConfirmAlertRec.ecAction);
            statement.setString(5, pQEConfirmAlertRec.clickAndConfirmFlag);
            statement.setString(6, pQEConfirmAlertRec.ecBkrAction);
            // 6-7-2006: MThoresen: Added so not to add duplicate records in the Alert Table for
            // Click and Confirm trades...
            clickConfirmRecID = clickAndConfirmExists(pQEConfirmAlertRec);
            // IF THIS IS A C&C, VERIFY THAT THERE IS NOT ALREADY A RECORD IN THE QUEUE TO BE PROCESSED.
            // IF THERE IS NOT, ADD THE RECORD
            if (pQEConfirmAlertRec.clickAndConfirmFlag.equalsIgnoreCase("Y")){
                if(!(clickConfirmRecID > 0)) // IF THERE IS NOT A C&C ALERT REC, CREATE ONE..
                    statement.executeUpdate();
            }else if (clickConfirmRecID > 0){  // ITS NOT AN C&C, BUT...A C&C ALERT REC EXISTS.
                // UPDATE THIS RECORD SO THAT THE C&C FLAG IS NOW 'N'.  NO NEED TO ENTER ANOTHER ALERT REC.
                // WE ARE JUST USING THE C&C ONE ALREADY IN THE DB...
                clearClickAndConfirmAlert(clickConfirmRecID);
            }else { // THIS IS NOT A C&C AND THERE IS NO PRIOR C&C ALERT RECORD...JUST PROCESS AS NORMAL...
                statement.executeUpdate();
            }
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

}
