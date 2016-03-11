package aff.confirm.common.ottradealert;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Oct 5, 2004
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingRQMT_EXT_PROCESS_DATA_dao extends OpsTrackingDAO {

    public OpsTrackingRQMT_EXT_PROCESS_DATA_dao( java.sql.Connection pOpsTrackingConnection) {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };


    public void insertTradeExtProcessData(double pRqmtID, String pExtProcCode,
                               String pAttribName, String pAttribValue )
            throws SQLException {
        PreparedStatement preparedStatement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_rqmt_ext_process_data";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into ops_tracking.RQMT_EXT_PROCESS_DATA( " +
                        "ID," + //1
                        "RQMT_ID, " + //2
                        "EXT_PROCESS_CODE, " + //3
                        "ATTRIB_NAME, " + //4
                        "ATTRIB_VALUE ) " + //5
                    "values( ?, ?, ?, ?, ? )";

            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setInt(1, nextSeqNo);
            preparedStatement.setDouble(2, pRqmtID);
            preparedStatement.setString(3, pExtProcCode);
            preparedStatement.setString(4, pAttribName);
            preparedStatement.setString(5, pAttribValue);
            preparedStatement.executeUpdate();
        }  finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
                preparedStatement = null;
            } catch (SQLException e) { }
        }
    }

    public String getAttribValue(double pRqmtID, String pExtProcessCode, String pAttribName)
            throws SQLException {
        String attribValue = null;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String selectSQL;
            selectSQL = "select attrib_value from ops_tracking.RQMT_EXT_PROCESS_DATA " +
                        "where RQMT_ID = ? and EXT_PROCESS_CODE = ? and ATTRIB_NAME = ?";

            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pRqmtID);
            statement.setString(2, pExtProcessCode);
            statement.setString(3, pAttribName);
            rs = statement.executeQuery();

            while (rs.next()) {
                attribValue = rs.getString("attrib_value");
            }

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;

            try {
                if (rs != null)
                    rs.close();
            } catch (SQLException e) { }
            rs = null;
        }
        return attribValue;
    }



}

