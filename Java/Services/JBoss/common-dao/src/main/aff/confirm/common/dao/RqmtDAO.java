package aff.confirm.common.dao;

//import sun.jdbc.rowset.CachedRowSet;


import java.sql.SQLException;
import org.jboss.logging.Logger;


/**
 * User: ifrankel
 * Date: Aug 18, 2003
 * Time: 1:16:41 PM
 */
public class RqmtDAO extends DAOBase {
    private static Logger log = Logger.getLogger(RqmtDAO.class);

    public RqmtDAO(java.sql.Connection pConnection) throws SQLException, Exception {
        try{
            connection = pConnection;
            crsSqlStatement = "select * from ops_tracking.rqmt where ACTIVE_FLAG = 'Y'";
            refreshCRS();
        } catch (SQLException ex){
            log.error( "Sql Exception for: " + crsSqlStatement + ".  For USERNAME: " + connection.getMetaData().getUserName(), ex);
            throw ex;
        }
    }


    public String getInitialStatus(String pCode) throws SQLException {
        String initialStatus = "*UNKNOWN*";
        crs.beforeFirst();
        while (crs.next()) {
            if (crs.getString("CODE").equalsIgnoreCase(pCode)) {
                initialStatus = crs.getString("INITIAL_STATUS");
                break;
            }
        }
        return initialStatus;
    }

}
