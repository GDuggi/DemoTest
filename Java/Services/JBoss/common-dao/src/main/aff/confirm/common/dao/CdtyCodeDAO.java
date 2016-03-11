package aff.confirm.common.dao;

import com.sun.rowset.CachedRowSetImpl;

import javax.sql.rowset.CachedRowSet;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Aug 18, 2003
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class CdtyCodeDAO {
    private java.sql.Connection connection;
    public CachedRowSet crs;

    public CdtyCodeDAO(java.sql.Connection pConnection) throws SQLException {
        connection = pConnection;
        refreshCRS();
    }

    private void refreshCRS() throws SQLException {
        ResultSet rs = null;
        crs = new CachedRowSetImpl();
        PreparedStatement statement = null;
        String sqlStatement;
        try {
            //Statement produces code, cdty_grp_code
            /*sqlStatement = "select code, cdty_grp_code from infinity_mgr.cdty" +
                           " union " +
                           " select short_name, cdty_grp_code from infinity_mgr.cdty";*/
            sqlStatement = "select * from jms.v_cdty_to_aff_cdty_grp";
            statement    = connection.prepareStatement(sqlStatement);
            rs = statement.executeQuery();
            crs.populate(rs);
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
    }


    public String getCdtyGrpCode(String pCdtyCode) throws SQLException {
        String cdtyGrpCode = "";
        String sourceSystem = "";
        String destSystem = "";
        boolean sourceSystemOK = false;
        crs.beforeFirst();
        while (crs.next()) {
            String test_cdty = "";
            test_cdty = crs.getString("CODE");
            sourceSystem = crs.getString("SOURCE_SYSTEM_TYPE_CODE");
            destSystem = crs.getString("DEST_SYSTEM_TYPE_CODE");
            sourceSystemOK = (sourceSystem.equalsIgnoreCase("*") || sourceSystem.equalsIgnoreCase("SYM"));
            if ( test_cdty.equalsIgnoreCase(pCdtyCode.toString()) &&
                 sourceSystemOK &&
                 destSystem.equalsIgnoreCase("AFNTY") ) {
                cdtyGrpCode = crs.getString("CDTY_GRP_CODE");
                break;
            }
        }

        return cdtyGrpCode;
    }

}
