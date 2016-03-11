package aff.confirm.common.dbqueue;

/**
 * User: ifrankel
 * Date: Nov 25, 2003
 * Time: 7:02:49 AM
 * To change this template use Options | File Templates.
 */
public class QEConfirmAlertRec {
    public double id;
    public String tradingSystem;
    public double tradeID;
    public int ecProductID;
    //SUBMIT, CANCEL, NONE
    public String ecAction;
    public String processedFlag;
    // MThoresen 4-18-2007: Added for click and confirm
    public String clickAndConfirmFlag;
    //Samy 06-02-2009 : Added for  broker confirm
    public String ecBkrAction;

    public QEConfirmAlertRec(String pTradingSystem, double pTradeID, int pECProductID, String pECAction, String pClickAndConfirm,String pECBkrAction) {
        this.init();
        this.tradingSystem = pTradingSystem;
        this.tradeID = pTradeID;
        this.ecProductID = pECProductID;
        this.ecAction = pECAction;
        this.clickAndConfirmFlag = pClickAndConfirm;
        this.ecBkrAction = pECBkrAction;
    }

    public QEConfirmAlertRec() {
        this.init();
    }

    public void init(){
        this.id = 0;
        this.tradingSystem = "";
        this.tradeID = 0;
        this.ecProductID = 0;
        this.ecAction = "NONE";
        this.processedFlag = "";
        this.clickAndConfirmFlag = "N";
        this.ecBkrAction = "NONE";
    }
}
