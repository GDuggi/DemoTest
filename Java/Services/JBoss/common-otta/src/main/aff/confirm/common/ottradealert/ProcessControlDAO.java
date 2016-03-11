package aff.confirm.common.ottradealert;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Jan 16, 2004
 * Time: 8:17:26 AM
 * To change this template use Options | File Templates.
 */
public class ProcessControlDAO {
    public final String EMPTY_STRING = "NONE";
    private java.sql.Connection connection;
    private String updateSQL = EMPTY_STRING;

    public ProcessControlDAO(java.sql.Connection pConnection) {
        connection = pConnection;
    }

    public void updateAlertRecord(String pProcessMastCode, String pProcessedFlag ) throws SQLException, Exception {
        String updateSQL =
                "update ops_tracking.process_control " +
                "set processed_flag = ? " +
                ",processed_ts_gmt = sysdate " +
                "where process_mast_code = ? " +
                "and processed_flag = 'N'";
        PreparedStatement statement = null;
        try {
            statement = connection.prepareStatement(updateSQL);
            statement.setString(1, pProcessedFlag);
            statement.setString(2, pProcessMastCode);
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

    public void updateAlertRecordAll(String pProcessMastCode, String pProcessedFlag ) throws SQLException, Exception {
        String updateSQL =
                "update ops_tracking.process_control " +
                        "set processed_flag = ? " +
                        ",processed_ts_gmt = sysdate " +
                        "where process_mast_code = ? ";
        PreparedStatement statement = null;
        try {
            statement = connection.prepareStatement(updateSQL);
            statement.setString(1, pProcessedFlag);
            statement.setString(2, pProcessMastCode);
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

    public void insertProcessControl(String pProcessMastCode) throws SQLException {
        PreparedStatement statement = null;
        try {
            String insertSQL =
                    "Insert into OPS_TRACKING.PROCESS_CONTROL( " +
                    "ID, " + //-
                    "PROCESS_MAST_CODE, " + //1
                    "PROCESSED_FLAG, " + //2
                    "CRTD_PROCESS_TS_GMT, " + //3
                    "PROCESSED_TS_GMT ) " + //4
                    "values( OPS_TRACKING.SEQ_PROCESS_CONTROL.NEXTVAL, ?, ?, sysdate, sysdate )";

            statement = connection.prepareStatement(insertSQL);
            statement.setString(1, pProcessMastCode);
            statement.setString(2, "N");
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

    public boolean isReadyToProcess(String pProcessCode) throws SQLException, Exception {
        String readyToProcessSQL =
                "select count(*) cnt from ops_tracking.process_control " +
                "where process_mast_code = ? " +
                "and processed_flag = 'N'";
        PreparedStatement statement = null;
        ResultSet rs = null;
        boolean isReady = false;
        int count = 0;
        try {
            statement = connection.prepareStatement(readyToProcessSQL);
            statement.setString(1, pProcessCode);            
            rs = statement.executeQuery();
            if (rs.next()) {
                count = rs.getInt("cnt");
            }
        } catch (SQLException e) {
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
        if (count > 0)
            isReady = true;

        return isReady;
    }

    public boolean isProcessMastRowExist(String pProcessCode) throws SQLException, Exception {
        String readyToProcessSQL =
                "select count(*) cnt from ops_tracking.process_control " +
                        "where process_mast_code = ? ";
        PreparedStatement statement = null;
        ResultSet rs = null;
        boolean isRowExist = false;
        int count = 0;
        try {
            statement = connection.prepareStatement(readyToProcessSQL);
            statement.setString(1, pProcessCode);
            rs = statement.executeQuery();
            if (rs.next()) {
                count = rs.getInt("cnt");
            }
        } catch (SQLException e) {
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
        if (count > 0)
            isRowExist = true;

        return isRowExist;
    }

    public boolean isRowForServiceCodeAndDate(String pProcessCode, int pSendAtHourGMT) throws SQLException, Exception {
        String readyToProcessSQL =
                "select count(*) cnt from ops_tracking.process_control " +
                "where process_mast_code = ? " +
                "and trunc(crtd_process_ts_gmt) = trunc(sysdate)";
        PreparedStatement statement = null;
        ResultSet rs = null;
        boolean isReady = false;
        int count = 0;
        try {
            statement = connection.prepareStatement(readyToProcessSQL);
            statement.setString(1, pProcessCode);
            rs = statement.executeQuery();
            if (rs.next()) {
                count = rs.getInt("cnt");
            }
        } catch (SQLException e) {
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
        if (count > 0)
            isReady = true;

        return isReady;
    }


}
