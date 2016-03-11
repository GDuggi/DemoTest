package aff.confirm.common.ottradealert;



/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingEVENT_LOG_rec {
    public double ID;
    public java.sql.Date TS_GMT;
    public String MSG_TYPE;
    public String RPT_BY_PROCESS;
    public String MSG_TXT;
    public String TRD_SYS_CODE;
    public double TRADE_ID;

    public OpsTrackingEVENT_LOG_rec(){
        init();
    }

    private void init(){
        this.ID = 0;
        this.TS_GMT = null;
        this.MSG_TYPE = "";
        this.RPT_BY_PROCESS = "";
        this.MSG_TXT = "";
        this.TRD_SYS_CODE = "";
        this.TRADE_ID = 0;
    }

}
