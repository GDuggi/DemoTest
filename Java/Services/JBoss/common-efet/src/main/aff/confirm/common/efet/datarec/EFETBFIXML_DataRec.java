package aff.confirm.common.efet.datarec;

import java.text.DecimalFormat;

public class EFETBFIXML_DataRec {
    public String documentId;
    public String documentUsage;
    public String senderId;
    public String receiverId;
    public String receiverRole;
    public String documentVersion;
    public String linkedTo;
    public String totalFee;
    public String feeCurrency;
    public boolean xmlDataRowFound;
    private DecimalFormat df = new DecimalFormat("#0.####");
    private DecimalFormat dfFee = new DecimalFormat("#0.##");

    public EFETBFIXML_DataRec() {
        this.init();
    }

    public void setDocumentVersion(int pDocumentVersion ){
        this.documentVersion = Integer.toString(pDocumentVersion);
    }

    public void setTotalFee(double pTotalFee){
        double dTotalFee = Math.abs(pTotalFee);
        this.totalFee = dfFee.format(dTotalFee);
    }

    public void init(){
        this.documentId = "";
        this.documentUsage = "";
        this.senderId = "";
        this.receiverId = "";
        this.receiverRole = "Broker";
        this.documentVersion = "";
        this.linkedTo = "";
        this.totalFee = "";
        this.feeCurrency = "";
    }
}
