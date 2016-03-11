package aff.confirm.common.ottradealert;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.DecimalFormat;

/**
 * User: ifrankel
 * Date: Oct 21, 2003
 * Time: 6:06:49 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingDAO {
    protected java.sql.Connection opsTrackingConnection;
    private DecimalFormat df = new DecimalFormat("#0;-#0");

    protected int getNextSequence(String seqName) throws SQLException {
        int nextSeqNo = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT ops_tracking." + seqName +
                    ".nextval from dual");
            rs = statement.executeQuery();
            if (rs.next()) {
                nextSeqNo = (rs.getInt("nextval"));
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
        return nextSeqNo;
    }
}
