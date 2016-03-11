package aff.confirm.common.ottradealert;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTRADE_EXT_PROCESS_DATA_dao extends OpsTrackingDAO {

    public OpsTrackingTRADE_EXT_PROCESS_DATA_dao( java.sql.Connection pOpsTrackingConnection)
            throws SQLException {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };


    public void insertTradeExtProcessData(double pTradeID, String pExtProcCode,
                               String pAttribName, String pAttribValue )
            throws SQLException {
        PreparedStatement preparedStatement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_trade_ext_proc_data";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into ops_tracking.TRADE_EXT_PROCESS_DATA( " +
                        "ID," + //1
                        "TRADE_ID, " + //2
                        "EXT_PROCESS_CODE, " + //3
                        "ATTRIB_NAME, " + //4
                        "ATTRIB_VALUE ) " + //5
                    "values( ?, ?, ?, ?, ? )";

            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setInt(1, nextSeqNo);
            preparedStatement.setDouble(2, pTradeID);
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

    public String getAttribValue(double pTradeID, String pExtProcessCode, String pAttribName)
            throws SQLException {
        String attribValue = null;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String selectSQL;
            selectSQL = "select attrib_value from ops_tracking.TRADE_EXT_PROCESS_DATA " +
                        "where TRADE_ID = ? and EXT_PROCESS_CODE = ? and ATTRIB_NAME = ?";

            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pTradeID);
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

