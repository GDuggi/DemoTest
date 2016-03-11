package aff.confirm.common.ottradealert;

import org.jboss.logging.Logger;

import java.sql.CallableStatement;
import java.sql.SQLException;


/**
 * User: srajaman
 * Date: Oct 9, 2009
 * Time: 4:09:09 PM
 */
public class OpsTrackingBasket_dao extends OpsTrackingDAO{

    public OpsTrackingBasket_dao( java.sql.Connection pOpsTrackingConnection)
            throws SQLException {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };

    public void finalApprove(double tradeId, String approvalFlag) throws SQLException {

        String sp = "{?=call ops_tracking.pkg_trade_appr.f_set_final_approval_flag(?,?,?,?)}";
        CallableStatement stmt = null;

        try {
            stmt = this.opsTrackingConnection.prepareCall(sp);
            stmt.registerOutParameter(1, oracle.jdbc.OracleTypes.INTEGER);
            stmt.setDouble(2,tradeId);
            stmt.setString(3,approvalFlag);
            stmt.setString(4,"N");  // force final approval
            stmt.setString(5,this.opsTrackingConnection.getMetaData().getUserName());
            // there is locking issue while final approving the trade,
            // if there is an error let us try this 5 times before throwing out the error
            for ( int i=0;i<5;++i){

                try {
                    stmt.execute();
                    break; // no error come out of the loop
                }
                catch (SQLException e){

                    Logger.getLogger(this.getClass()).error("Final Approval Error  : " , e );
                    if (i == 4 ) {
                        throw e;
                    }
                    try {
                        Thread.sleep(5000);
                    } catch (InterruptedException e1) {

                    }
                }
            }
        }
        finally {

            try {
                if (stmt != null) {
                    stmt.close();
                }
            }
            catch (Exception e){

            }
        }


    }

    public void cancelEConfirmEFETRqmt(double tradeId){
        
    }
}
