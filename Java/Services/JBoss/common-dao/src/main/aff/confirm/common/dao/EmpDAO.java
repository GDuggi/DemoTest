package aff.confirm.common.dao;



import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Aug 18, 2003
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class EmpDAO extends DAOBase {

    public EmpDAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        crsSqlStatement = "select * from infinity_mgr.emp where ACTIVE_FLAG = 'Y'";
        refreshCRS();
    }


    public String getEmpName(double pEmpId) throws SQLException {
        String firstName = "";
        String lastName = "";
        String name = EMPTY_STRING;
        crs.beforeFirst();
        while (crs.next()) {
            if (crs.getDouble("ID") == pEmpId) {
                firstName = crs.getString("FRST_NAME");
                lastName = crs.getString("LST_NAME");
                name = firstName + " " + lastName;
                break;
            }
        }
        return name;
    }

}
