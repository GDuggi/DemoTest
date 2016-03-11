package aff.confirm.common.econfirm.datarec;

/**
 * User: ifrankel
 * Date: Jul 25, 2003
 * Time: 8:27:57 AM
 * This is the Java version of a Delphi TRecord.
 */
public class EConfirmMessageLog_DataRec {
    public String tradingSystem;
    public double tradeID;
    public String traceID;
    public String messageCode;
    public String messageType;
    public String messageStatusDt;
    public String messageDesc;

    public EConfirmMessageLog_DataRec() {
        this.init();
    }

    /*public void setMessageType(String messageType) {
        this.messageType = messageType.toUpperCase();
    }

    public String getMessageType() {
        return messageType;
    }*/

    public EConfirmMessageLog_DataRec(String pTradingSystem, double pTradeID, String pTraceID,
                                      String pMessageCode, String pMessageType, String pMessageStatusDt,
                                      String pMessageDesc){
        this.init();
        this.tradingSystem = pTradingSystem;
        this.tradeID = pTradeID;
        this.traceID = pTraceID;
        this.messageCode = pMessageCode;
        this.messageType = pMessageType;
        this.messageStatusDt = pMessageStatusDt;
        this.messageDesc = pMessageDesc;
    }

    public void init(){
        this.tradingSystem = "";
        this.tradeID = 0;
        this.traceID = "";
        this.messageCode = "";
        this.messageType = "";
        this.messageStatusDt = "";
        this.messageDesc = "";
    }
}
