package aff.confirm.common.econfirm.datarec;

/**
 * User: ifrankel
 * Date: Jul 25, 2003
 * Time: 8:27:57 AM
 * This is the Java version of a Delphi TRecord.
 */
public class EConfirmSubmitLog_DataRec {
    public String tradingSystem;
    public double tradeID;
    public String traceID;
    public String statusMessage;
    public String action;

    public EConfirmSubmitLog_DataRec() {
        this.init();
    }

    public EConfirmSubmitLog_DataRec(String pTradingSystem, double pTradeID, String pTraceID,
                                     String pStatusMessage, String pAction){
        this.init();
        this.tradingSystem = pTradingSystem;
        this.tradeID = pTradeID;
        this.traceID = pTraceID;
        this.statusMessage = pStatusMessage;
        this.action = pAction;
    }

    public void init(){
        this.tradingSystem = "";
        this.tradeID = 0;
        this.traceID = "";
        this.statusMessage = "";
        this.action = "";
    }
}
