package aff.confirm.common.efet.datarec;

/**
 * User: ifrankel
 * Date: Jul 25, 2003
 * Time: 8:27:57 AM
 * This is the Java version of a Delphi TRecord.
 */
public class EFETTradeSummary_DataRec {
    public int id;
    public String tradingSystem;
    public double tradeID;
    //public int productID;
    public String status;
    public String cmt;
    public String cptyTradeRefID;
    public String okToResubmitInd;
    public String errorFlag;
    public String documentId;
    public int documentVersion;
    public String senderId;
    public String receiverId;
    public String entityType;

    public EFETTradeSummary_DataRec() {
        this.init();
    }

    public EFETTradeSummary_DataRec(String pTradingSystem, double pTradeID, String pDocumentId,
                                    int pDocumentVersion, String pStatus, String pCmt, String pCptyTradeRefID,
                                    String pErrorFlag, String pSenderId, String pReceiverId, String pEntityType){
        this.init();
        this.tradingSystem = pTradingSystem;
        this.tradeID = pTradeID;
        this.documentId = pDocumentId;
        this.documentVersion = pDocumentVersion;
        this.status = pStatus;
        this.cmt = pCmt;
        this.cptyTradeRefID = pCptyTradeRefID;
        //This field has been taken off the constructor because
        //it is never called by an update routine.
        //this.okToResubmitFlag = pOkToResubmitFlag;
        this.errorFlag = pErrorFlag;
        this.senderId = pSenderId;
        this.receiverId = pReceiverId;
        this.entityType = pEntityType;
    }

    public void init(){
        this.id = 0;
        this.tradingSystem = "";
        this.tradeID = 0;
        this.documentId = "";
        this.documentVersion = 0;
        this.status = "";
        this.cmt = "";
        this.cptyTradeRefID = "";
        this.okToResubmitInd = "";
        this.errorFlag = "";
        this.senderId = "";
        this.receiverId = "";
        this.entityType = "";
    }

    public void stripNulls(){
        if (this.tradingSystem == null)
            this.tradingSystem = "";
        if (this.documentId == null)
            this.documentId = "";
        if (this.status == null)
            this.status = "";
        if (this.cmt == null)
            this.cmt = "";
        if (this.cptyTradeRefID == null)
            this.cptyTradeRefID = "";
        if (this.okToResubmitInd == null)
            this.okToResubmitInd = "";
        if (this.errorFlag == null)
            this.errorFlag = "";
        if (this.senderId == null)
            this.senderId = "";
        if (this.receiverId == null)
            this.receiverId = "";
        if (this.entityType == null)
            this.entityType = "";
    }
}
