package aff.confirm.common.econfirm.datarec;

/**
 * Created by IntelliJ IDEA.
 * User: ifrankel
 * Date: Jul 25, 2003
 * Time: 8:27:57 AM
 * This is the Java version of a Delphi TRecord.
 */
public class ECIgnoredStatusMessage_Rec {
    public String senderTradeRefId;
    public String traceId;
    public String status;
    public String buyer;
    public String seller;
    public String submissionCompany;
    public String statusDate;
    public String tradeDate;
    //public Date crtdTimestampGmt;

    public ECIgnoredStatusMessage_Rec() {
        this.init();
    }

    public ECIgnoredStatusMessage_Rec(String pSenderTradeRefId, String pTraceId, String pStatus,
                                      String pBuyer, String pSeller, String pSubmissionCompany,
                                      String pStatusDate, String pTradeDate){
        this.init();
        this.senderTradeRefId = pSenderTradeRefId;
        this.traceId = pTraceId;
        this.status = pStatus;
        this.buyer = pBuyer;
        this.seller = pSeller;
        this.submissionCompany = pSubmissionCompany;
        this.statusDate = pStatusDate;
        this.tradeDate = pTradeDate;
    }

    public void init(){
        this.senderTradeRefId = "";
        this.traceId = "";
        this.status = "";
        this.buyer = "";
        this.seller = "";
        this.submissionCompany = "";
        this.statusDate = "";
        this.tradeDate = "";
    }
}
