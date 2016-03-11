package aff.confirm.common.efet.datarec;

/**
 * User: ifrankel
 * Date: Jul 25, 2003
 * Time: 8:27:57 AM
 * This is the Java version of a Delphi TRecord.
 */
public class EFETSubmitLog_DataRec {
    public String tradingSystem;
    public double tradeID;
    public String statusMessage;
    public String action;
    public String docType;

    public EFETSubmitLog_DataRec() {
        this.init();
    }

    public EFETSubmitLog_DataRec(String pTradingSystem, double pTradeID, String pStatusMessage, String pAction,
                                 String pDocType){
        this.init();
        this.tradingSystem = pTradingSystem;
        this.tradeID = pTradeID;
        this.statusMessage = pStatusMessage;
        this.action = pAction;
        this.docType = pDocType;
    }

    public void init(){
        this.tradingSystem = "";
        this.tradeID = 0;
        this.statusMessage = "";
        this.action = "";
        this.docType = "";
    }
}
