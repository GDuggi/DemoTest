package aff.confirm.common.dbqueue;



/**
 * User: ifrankel
 * Date: Nov 25, 2003
 * Time: 7:02:49 AM
 * To change this template use Options | File Templates.
 */
public class QEFETTradeAlertRec {
    public double id;
    public String tradingSystem;
    public double tradeID;
    public String seCptySn;
    //SUBMIT, CANCEL, NONE
    public String efetAction;
    public String efetSubmitState;
    public String processedFlag;
    public String docType;
    public String receiverType;

    public QEFETTradeAlertRec(String pTradingSystem, double pTradeID, String pSeCptySn, String pEfetAction,
                              String pEfetSubmitState, String pDocType, String pReceiverType) {
        this.init();
        this.tradingSystem = pTradingSystem;
        this.tradeID = pTradeID;
        this.seCptySn = pSeCptySn;
        this.efetAction = pEfetAction;
        this.efetSubmitState = pEfetSubmitState;
        this.docType = pDocType;
        this.receiverType = pReceiverType;
    }

    public QEFETTradeAlertRec() {
        this.init();
    }

    public void init(){
        this.id = 0;
        this.tradingSystem = "";
        this.tradeID = 0;
        this.seCptySn = "";
        this.efetAction = "NONE";
        this.efetSubmitState = "";
        this.processedFlag = "";
        this.docType = "";
        this.receiverType = "";
    }
}
