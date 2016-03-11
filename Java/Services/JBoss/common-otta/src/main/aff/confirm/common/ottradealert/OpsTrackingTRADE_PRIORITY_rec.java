package aff.confirm.common.ottradealert;



/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTRADE_PRIORITY_rec {
    public double ID;
    public double TRADE_ID;
    public int PRIORITY;
    public String PL_AMT;

    public OpsTrackingTRADE_PRIORITY_rec(){
        init();
    }

    public void init(){
        this.ID = 0;
        this.TRADE_ID = 0;
        this.PRIORITY = 0;
        this.PL_AMT = "n/a";
    }

}
