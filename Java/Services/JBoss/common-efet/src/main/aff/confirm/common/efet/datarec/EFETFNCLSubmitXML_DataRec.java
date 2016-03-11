package aff.confirm.common.efet.datarec;

import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

/**
 * User: srajaman
 * Date: Mar 24, 2009
 * Time: 11:43:59 AM
 */
public class EFETFNCLSubmitXML_DataRec {

    private double tradeId;
    private String documentId;
    private String docUsage;
    private String senderId;
    private String receiverId;
    private String receiverRole;
    private int documentVersion;
    private String transactionType;
    private String buyerParty;
    private String sellerParty;
    private String agreement;
    private String sttlCurrency;
    private double totalVolume;
    private String totalVolumeUnit;
    private Date tradeDate;
    private int rounding;
    private String commonPricing;
    private Date effectiveDate;
    
    private ArrayList priceInfos;
    private ArrayList deliveryList;
    private String brokerId;
    private String brokerName;

    private double prmntTradeLegId;
    private boolean dataFound;

    public boolean isDataFound() {
        return dataFound;
    }

    public void setDataFound(boolean dataFound) {
        this.dataFound = dataFound;
    }

    public double getPrmntTradeLegId() {
        return prmntTradeLegId;
    }

    public void setPrmntTradeLegId(double prmntTradeLegId) {
        this.prmntTradeLegId = prmntTradeLegId;
    }

    public String getBrokerName() {
        return brokerName;
    }

    public void setBrokerName(String brokerName) {
        this.brokerName = brokerName;
    }

    private DecimalFormat df = new DecimalFormat("#0.####");
    private SimpleDateFormat sdfDate = new SimpleDateFormat("yyyy-MM-dd");

    public String getBrokerId() {
        return brokerId;
    }

    public void setBrokerId(String brokerId) {
        this.brokerId = brokerId;
    }

    public ArrayList getDeliveryList() {
        return deliveryList;
    }

    public void setDeliveryList(ArrayList deliveryList) {
        this.deliveryList = deliveryList;
    }

    private Date terminationDate;public double getTradeId() {
        return tradeId;
    }

    public void setTradeId(double tradeId) {
        this.tradeId = tradeId;
    }

    public String getDocumentId() {
        return documentId;
    }

    public void setDocumentId(String documentId) {
        this.documentId = documentId;
    }

    public String getDocUsage() {
        return docUsage;
    }

    public void setDocUsage(String docUsage) {
        this.docUsage = docUsage;
    }

    public String getSenderId() {
        return senderId;
    }

    public void setSenderId(String senderId) {
        this.senderId = senderId;
    }

    public String getReceiverId() {
        return receiverId;
    }

    public void setReceiverId(String receiverId) {
        this.receiverId = receiverId;
    }

    public String getReceiverRole() {
        return receiverRole;
    }

    public void setReceiverRole(String receiverRole) {
        this.receiverRole = receiverRole;
    }

    public int getDocumentVersion() {
        return documentVersion;
    }

    public void setDocumentVersion(int documentVersion) {
        this.documentVersion = documentVersion;
    }

    public String getTransactionType() {
        return transactionType;
    }

    public void setTransactionType(String transactionType) {
        this.transactionType = transactionType;
    }

    public String getBuyerParty() {
        return buyerParty;
    }

    public void setBuyerParty(String buyerParty) {
        this.buyerParty = buyerParty;
    }

    public String getSellerParty() {
        return sellerParty;
    }

    public void setSellerParty(String sellerParty) {
        this.sellerParty = sellerParty;
    }

    public String getAgreement() {
        return agreement;
    }

    public void setAgreement(String agreement) {
        this.agreement = agreement;
    }

    public String getSttlCurrency() {
        return sttlCurrency;
    }

    public void setSttlCurrency(String sttlCurrency) {
        this.sttlCurrency = sttlCurrency;
    }

    public double getTotalVolume() {
        return totalVolume;
    }

    public String getTotalVolumeFmt() {
        return df.format(totalVolume);
    }

    public void setTotalVolume(double totalVolume) {
        this.totalVolume = totalVolume;
    }

    public String getTotalVolumeUnit() {
        return totalVolumeUnit;
    }

    public void setTotalVolumeUnit(String totalVolumeUnit) {
        this.totalVolumeUnit = totalVolumeUnit;
    }

    public Date getTradeDate() {
        return tradeDate;
    }

    public String getTradeDateFmt() {
        if (tradeDate != null ) {
            return sdfDate.format(tradeDate);
        }
        return "";
    }

    public void setTradeDate(Date tradeDate) {
        this.tradeDate = tradeDate;
    }


    public int getRounding() {
        return rounding;
    }

    public void setRounding(int rounding) {
        this.rounding = rounding;
    }

    public String getCommonPricing() {
        return commonPricing;
    }

    public void setCommonPricing(String commonPricing) {
        this.commonPricing = commonPricing;
    }

    public Date getEffectiveDate() {
        return effectiveDate;
    }
    public String getEffectiveDateFmt() {
        if ( effectiveDate != null ) {
            return sdfDate.format(effectiveDate);
        }
        return "";
    }

    public void setEffectiveDate(Date effectiveDate) {
        this.effectiveDate = effectiveDate;
    }

    public String getTerminationDateFmt() {
        if (terminationDate != null) {
            return sdfDate.format(terminationDate);
        }
        return "";
    }

    public Date getTerminationDate() {
        return terminationDate;
    }

    public void setTerminationDate(Date terminationDate) {
        this.terminationDate = terminationDate;
    }

    public ArrayList getPriceInfos() {
        return priceInfos;
    }

    public void setPriceInfos(ArrayList priceInfos) {
        this.priceInfos = priceInfos;
    }



     

    
}
