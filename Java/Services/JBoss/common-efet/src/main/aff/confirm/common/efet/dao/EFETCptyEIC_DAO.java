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
public class EFETCptyEIC_DAO extends DAOBase {

    public EFETCptyEIC_DAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        crsSqlStatement = "select * from cpty.v_cpty_alias where alias_type_code = 'EFEIC'";
    }

    public String getEICCode(String pCptySn)
            throws Exception {
        String cptySn = "";
        String eicCode = "NONE";
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            cptySn = crs.getString("cpty_sn");
            if ( cptySn.equalsIgnoreCase(pCptySn)) {
                eicCode = crs.getString("alias");
                break;
            }
        }
        return eicCode;
    }

}
