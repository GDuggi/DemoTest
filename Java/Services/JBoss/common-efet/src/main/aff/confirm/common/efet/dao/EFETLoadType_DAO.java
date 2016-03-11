package aff.confirm.common.efet.dao;

import aff.confirm.common.dao.DAOBase;

import java.sql.SQLException;
import java.text.ParseException;

/**
 * User: ifrankel
 * Date: Aug 18, 2003
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class EFETLoadType_DAO extends DAOBase {

    public EFETLoadType_DAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        crsSqlStatement = "select * from efet.load_type_mapping";
    }

 
    public String getLoadType(String pDurationCode)
            throws Exception {
        String loadType = "Custom";
        String durationCode = "";
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            durationCode = crs.getString("DURATION_CODE");
            if ( durationCode.equalsIgnoreCase(pDurationCode) ) {
                loadType = crs.getString("LOAD_TYPE");
                break;
            }
        }
        return loadType;
    }

}
