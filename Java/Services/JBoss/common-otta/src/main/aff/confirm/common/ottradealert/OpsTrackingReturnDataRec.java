package aff.confirm.common.ottradealert;

/**
 * User: ifrankel
 * Date: Jul 25, 2003
 * Time: 8:27:57 AM
 * This is the Java version of a Delphi TRecord.
 */
public class OpsTrackingReturnDataRec {
    public double tradeID;
    public int ecProductID;
    //SUBMIT, CANCEL, NONE, MATCHED
    public String ecAction;
    public String efetCptyAction;
    public String efetCptySubmitState;
    public String efetCnfReceiver;
    public String efetBkrAction;
    public String efetBkrSubmitState;
    public String ecBkrAction;

    public OpsTrackingReturnDataRec() {
        this.init();
    }

    public void init(){
        this.tradeID = 0;
        this.ecProductID = 0;
        this.ecAction = "NONE";
        this.efetCptyAction = "NONE";
        this.efetCptySubmitState = "NONE";
        this.efetCnfReceiver = "";
        this.efetBkrAction = "NONE";
        this.efetBkrSubmitState = "NONE";
        this.ecBkrAction = "NONE";
    }
}
