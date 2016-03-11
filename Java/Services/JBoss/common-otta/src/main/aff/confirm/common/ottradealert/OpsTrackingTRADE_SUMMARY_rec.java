package aff.confirm.common.ottradealert;



/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTRADE_SUMMARY_rec {
    public String CATEGORY;
    public String CMT;
    public String FINAL_APPROVAL_FLAG;
    public java.sql.Date FINAL_APPROVAL_TIMESTAMP_GMT;
    public String HAS_PROBLEM_FLAG;
    public double ID;
    public java.sql.Date LAST_TRD_EDIT_TIMESTAMP_GMT;
    public java.sql.Date LAST_UPDATE_TIMESTAMP_GMT;
    public String OPEN_RQMTS_FLAG;
    public String OPS_DET_ACT_FLAG;
    public String READY_FOR_FINAL_APPROVAL_FLAG;
    public double TRADE_ID;
    public double TRANSACTION_SEQ;


    public OpsTrackingTRADE_SUMMARY_rec(){
        init();
    }

    private void init(){
        this.CATEGORY = "";
        this.CMT ="";
        this.FINAL_APPROVAL_FLAG = "";
        this.FINAL_APPROVAL_TIMESTAMP_GMT = null;
        this.HAS_PROBLEM_FLAG = "";
        this.ID = 0;
        this.LAST_TRD_EDIT_TIMESTAMP_GMT = null;
        this.LAST_UPDATE_TIMESTAMP_GMT = null;
        this.OPEN_RQMTS_FLAG = "";
        this.OPS_DET_ACT_FLAG = "";
        this.READY_FOR_FINAL_APPROVAL_FLAG = "";
        this.TRADE_ID = 0;
        this.TRANSACTION_SEQ = 0;
    }

}
