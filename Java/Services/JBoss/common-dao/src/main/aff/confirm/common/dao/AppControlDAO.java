package aff.confirm.common.dao;

import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Aug 18, 2003
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class AppControlDAO extends DAOBase {
    public AppControlDAO(java.sql.Connection pConnection, String pServiceCode) throws SQLException, Exception {
        connection = pConnection;
        String serviceCode = "'" + pServiceCode + "'";
        crsSqlStatement = "select * from jbossusr.app_control where SERVICE_CODE = " + serviceCode;
        refreshCRS();
    }


    public String getValue(String pKeyName) throws SQLException {
        String keyName = "";
        String value = EMPTY_STRING;
        crs.beforeFirst();
        while (crs.next()) {
            keyName = crs.getString("KEY_NAME");
            if (keyName.equalsIgnoreCase(pKeyName) ) {
                value = crs.getString("VALUE");
                break;
            }
        }
        return value;
    }

}
