package aff.confirm.common.dao;



import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Aug 18, 2003
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class CdtyGrpDAO extends DAOBase {

    public CdtyGrpDAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        crsSqlStatement = "select cdty_grp_code,code from infinity_mgr.cdty where ACTIVE_FLAG = 'Y'";
        refreshCRS();
    }


    public String getCdtyGrpCode(String pCdtyCode) throws SQLException {
        String cdtyGrpCode = EMPTY_STRING;
        crs.beforeFirst();
        while (crs.next()) {
            if (crs.getString("code").equalsIgnoreCase(pCdtyCode)) {
                cdtyGrpCode = crs.getString("cdty_grp_code");
                break;
            }
        }
        return cdtyGrpCode;
    }

}
