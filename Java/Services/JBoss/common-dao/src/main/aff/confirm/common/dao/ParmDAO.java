package aff.confirm.common.dao;



import com.sun.rowset.CachedRowSetImpl;

import javax.sql.rowset.CachedRowSet;
import java.sql.Connection;
import java.sql.SQLException;
import org.jboss.logging.Logger;


/**
 * User: ifrankel
 * Date: Apr 22, 2003
 * Time: 3:15:18 PM
 */
public class ParmDAO {
    private static Logger log = Logger.getLogger(ParmDAO.class);
    //private Connection connection;
    private CachedRowSet crset = null;

    public ParmDAO( Connection pConnection ) throws SQLException {
        //connection = pConnection;
        String sqlCommand = null;
        sqlCommand = "select * from wf_cont.parm where active_flag = 'Y'";
        crset = new CachedRowSetImpl();
        crset.setCommand(sqlCommand);
        crset.execute(pConnection);
        while (crset.next()){
            log.info("ID= "+crset.getInt("ID") + ",  PARM_NAME= " + crset.getString("PARM_NAME"));
        }

    }

}
