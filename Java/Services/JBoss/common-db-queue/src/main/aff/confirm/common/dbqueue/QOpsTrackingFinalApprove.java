package aff.confirm.common.dbqueue;


/**
 * User: ifrankel
 * Date: Sep 22, 2005
 * Time: 7:00:07 AM
 * To change this template use Options | File Templates.
 */
public class QOpsTrackingFinalApprove extends QAlertBase{

    public QOpsTrackingFinalApprove(java.sql.Connection pConnection) {
        connection = pConnection;
        updateSQL = "Update OPS_TRACKING.Q_FINAL_APPROVE set " +
                    "PROCESSED_FLAG = ?," + //1
                    "PROCESSED_TS_GMT = sysdate " +
                    "where id = ? "; //2

        readyToProcessSQL = " SELECT * from OPS_TRACKING.Q_FINAL_APPROVE"   +
                            " where PROCESSED_FLAG = 'N'";
    }

}
