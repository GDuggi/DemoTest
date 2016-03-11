package aff.confirm.common.econfirm.datarec;

/**
 * User: ifrankel
 * Date: Jul 25, 2003
 * Time: 8:27:57 AM
 * This is the Java version of a Delphi TRecord.
 */
public class EConfirmSummary_DataRec {
    public String tradingSystem;
    public double tradeID;
    public int productID;
    public String status;
    public String cmt;
    public String cptyTradeRefID;
    public String okToResubmitInd;
    public String errorFlag;
    public String bkrStatus;
    public String bkrTradeRefID;



    public EConfirmSummary_DataRec() {
        this.init();
    }

    public EConfirmSummary_DataRec(String pTradingSystem, double pTradeID, int pProductID, String pStatus,
                                   String pCmt, String pCptyTradeRefID, String pErrorFlag){
        this.init();
        this.tradingSystem = pTradingSystem;
        this.tradeID = pTradeID;
        this.productID = pProductID;
        this.status = pStatus;
        this.cmt = pCmt;
        this.cptyTradeRefID = pCptyTradeRefID;
        //This field has been taken off the constructor because
        //it is never called by an update routine.
        //this.okToResubmitFlag = pOkToResubmitFlag;
        this.errorFlag = pErrorFlag;
    }

    public EConfirmSummary_DataRec(String pTradingSystem, double pTradeID, int pProductID, String pStatus,
                                   String pCmt, String pCptyTradeRefID, String pErrorFlag, String pBkrStatus, String pBkrTradeRefId){
        this.init();
        this.tradingSystem = pTradingSystem;
        this.tradeID = pTradeID;
        this.productID = pProductID;
        this.status = pStatus;
        this.cmt = pCmt;
        this.cptyTradeRefID = pCptyTradeRefID;
        //This field has been taken off the constructor because
        //it is never called by an update routine.
        //this.okToResubmitFlag = pOkToResubmitFlag;
        this.errorFlag = pErrorFlag;
        this.bkrStatus = pBkrStatus;
        this.bkrTradeRefID = pBkrTradeRefId;
    }

    public void init(){
        this.tradingSystem = "";
        this.tradeID = 0;
        this.productID = 0;
        this.status = "";
        this.cmt = "";
        this.cptyTradeRefID = "";
        this.okToResubmitInd = "";
        this.errorFlag = "";
        this.bkrStatus = "";
        this.bkrTradeRefID = "";
    }
}
