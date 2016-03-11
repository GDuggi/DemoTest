package aff.confirm.common.efet.datarec;

/**
 * User: ifrankel
 * Date: Jul 25, 2003
 * Time: 8:27:57 AM
 * This is the Java version of a Delphi TRecord.
 */
public class EFETErrorLog_DataRec {
    public double tradeID;
    public String efetState;
    public String efetTimestamp;
    public String reasonCode;
    public String reasonText;
    public String docId;
    public String docVersion;
    public String ebXmlMessageId;
    public String docType;

    public EFETErrorLog_DataRec() {
        this.init();
    }

    public EFETErrorLog_DataRec(double pTradeID, String pEfetState,
                                String pEfetTimestamp, String pReasonCode,
                                String pReasonText, String pDocId, String pDocVersion,
                                String pEbXmlMessageId, String pDocType){
        this.init();
        this.tradeID = pTradeID;
        this.efetState = pEfetState;
        this.efetTimestamp = pEfetTimestamp;
        this.reasonCode = pReasonCode;
        this.reasonText = pReasonText;
        this.docId = pDocId;
        this.docVersion = pDocVersion;
        this.ebXmlMessageId = pEbXmlMessageId;
        this.docType = pDocType;
    }

    public void init(){
        this.tradeID = 0;
        this.efetState = "";
        this.efetTimestamp = "";
        this.reasonCode = "";
        this.reasonText = "";
        this.docId = "";
        this.docVersion = "";
        this.ebXmlMessageId = "";
        this.docType = "";
    }


}
