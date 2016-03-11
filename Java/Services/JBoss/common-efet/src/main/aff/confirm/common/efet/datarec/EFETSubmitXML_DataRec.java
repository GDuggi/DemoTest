package aff.confirm.common.efet.datarec;

import aff.confirm.common.efet.dao.EFET_DAO;

import java.sql.Date;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.util.Calendar;
import java.util.Vector;

/**
 * User: ifrankel
 * Date: Jul 25, 2003
 * Time: 8:27:57 AM
 * This is the Java version of a Delphi TRecord.
 */
public class EFETSubmitXML_DataRec {
    public String tradeID;
    public String documentId;
    public String documentUsage;
    public String senderId;
    public String receiverId;
    public String brokerEicId;
    public String receiverRole;
    public String documentVersion;
    public String market;
    public String commodity;
    public String transactionType;
    public String locationSn;
    public String deliveryPointArea;
    public String buyerParty;
    public String sellerParty;
    public String loadType;
    public String agreement;
    public String currency;
    public String totalVolume;
    public String totalVolumeUnit;
    public String tradeDate;
    public String capacityUnit;
    public String priceUnitCurrency;
    public String priceUnitCapacityUnit;
    public String deliveryStartDateAndTime;
    public String deliveryEndDateAndTime;
    public String contractCapacity;
    public String price;
    public String strikePrice;
    public String totalContractValue;
    public String agentType;
    public String agentName;
    public String brokerId;
    public String traderName;
    public String qtyPerDurationCode;
    public String qtyPer;
    public String sameQtyFlag;
    public String samePriceFlag;
    public double bkrFeeTotal;
    public String bkrFeeCcy;
    public String netaBuyAccount;
    public String netaSellAccount;
    //public String extTrnsmssnInd;
    //public String netaTransmissionCharges;
    public String notifyAgentId;
    public String notifyAgentEicCode;
    public String useAffDeliveryTableFlag;
    public String ecvnaBuyerId;
    public String ecvnaSellerId;
    public String bscPartyId;

    public boolean useFractionalCapUnit;
    public int otcLocCdtyId;
    public boolean xmlDataRowFound;
    // added for emission and option trade
    public String optionTypeInd;
    public String optionStyleCode;
    public String optionWriter;
    public String optionHolder;
    public String premRate;
    public String premUnitCcyCode;
    public String premUnitUomCode;
    public String premCcyCode;
    public String premTotalValue;
    public String premPaymentDate;
    public Vector    optionShedule;

    public String computeHourVolDayDelivery ;
    public String fixedOptionExpiry ;
    public String dailyOptionExpiry;



    private DecimalFormat df = new DecimalFormat("#0.####");
    private DecimalFormat dfTotal = new DecimalFormat("#0.##");

    public EFETSubmitXML_DataRec() {
        this.init();
    }

    public void setTradeId(double pTradeId){
        this.tradeID = df.format(pTradeId);
    }

    public void setDocumentVersion(int pDocumentVersion ){
        this.documentVersion = Integer.toString(pDocumentVersion);
    }

    public void setTotalVolume(double pTotalVolume){
        //When trade is a sale TotalVolume is negative.
        //This routine strips the negative sign when present.
        double totVol = Math.abs(pTotalVolume);
        this.totalVolume = dfTotal.format(totVol);
    }

    public void setQtyPer(double pQtyPer){
        //When trade is a sale TotalVolume is negative.
        //This routine strips the negative sign when present.
        double dQtyPer = Math.abs(pQtyPer);
        this.qtyPer = df.format(dQtyPer);
    }

    public void setDeliveryStartDateAndTime(Date pStartDate){
        this.deliveryStartDateAndTime = EFET_DAO.sdfEfet.format(pStartDate);
    }

    public void setDeliveryStartDateAndTime(Date pStartDate, int pHour){
        Calendar calStart = Calendar.getInstance();
        calStart.setTime(pStartDate);
        //Sometimes calStart.add(hour, pHour) was adding an extra hour.
        //Using set instead of add solves the problem.
        calStart.set(Calendar.HOUR, pHour);
        this.deliveryStartDateAndTime = EFET_DAO.sdfEfet.format(calStart.getTime());
    }

    public void setDeliveryEndDateAndTime(Date pEndDate) throws ParseException {
        /**
         * Affinity is inclusive -- when 5/25/2005 is given as the end date,
         * it means through the end of the date. Efet, however, is exclusive.
         * Therefore, to indicate through the end you must state 5/26/2005 00:00.
         * This is done by incrementing to the next day. Since there is no time
         * element, it is automatically set to 00:00.
         */
        Calendar calEnd = Calendar.getInstance();
        calEnd.setTime(pEndDate);
        calEnd.add(Calendar.DATE,1);
        this.deliveryEndDateAndTime = EFET_DAO.sdfEfet.format(calEnd.getTime());
    }

    public void setDeliveryEndDateAndTime(Date pEndDate, int pHour) throws ParseException {
        /**
         * Affinity is inclusive -- when 5/25/2005 is given as the end date,
         * it means through the end of the date. Efet, however, is exclusive.
         * Therefore, to indicate through the end you must state 5/26/2005 00:00.
         * This is done by incrementing to the next day. Since there is no time
         * element, it is automatically set to 00:00.
         */
        Calendar calEnd = Calendar.getInstance();
        calEnd.setTime(pEndDate);
        calEnd.add(Calendar.DATE,1);
        //Sometimes calStart.add(hour, pHour) was adding an extra hour.
        //Using set instead of add solves the problem.
        calEnd.set(Calendar.HOUR,pHour);
        this.deliveryEndDateAndTime = EFET_DAO.sdfEfet.format(calEnd.getTime());
    }

    public void setContractCapacity(double pContractCapacity){
        double dContractCapacity = Math.abs(pContractCapacity);
        this.contractCapacity = df.format(dContractCapacity);
    }

    public void setPrice(double pPrice){
        //Israel 8/12/15 -- Remove negative prohibition.
        //Also replace < 1 because unnecessary with mask
        //double dPrice = Math.abs(pPrice);
        //Gets rid of leading zero when price is a decimal
        //if (dPrice == 0)
        //    this.price = "0";
        //else if (dPrice < 1 )
        //    this.price = df.format(dPrice).substring(1);
        ///else
        //    this.price = df.format(dPrice);

        this.price = df.format(pPrice);
    }

    public void setStrikePrice(double pPrice){
        //Israel 8/12/15 -- Remove negative prohibition.
        //double dPrice = Math.abs(pPrice);
        //this.strikePrice = df.format(dPrice);
        this.strikePrice = df.format(pPrice);
    }

    public void setTotalContractValue(double pTotalContractValue){
        double dValue = Math.abs(pTotalContractValue);
        this.totalContractValue = dfTotal.format(dValue);
    }
    public void setPremRate(double pPremDate) {
        double dValue = Math.abs(pPremDate);
        this.premRate = df.format(dValue);
    }
    public void setPremTotalValue(double pPremTotalValue) {
        double dValue = Math.abs(pPremTotalValue);
        this.premTotalValue = dfTotal.format(dValue);
    }

    public void setPremDPaymentDate(Date pPaymentDate){
        Calendar calStart = Calendar.getInstance();
        calStart.setTime(pPaymentDate);
        //Sometimes calStart.add(hour, pHour) was adding an extra hour.
        //Using set instead of add solves the problem.
        this.premPaymentDate = EFET_DAO.sdfEfet.format(calStart.getTime());
    }




    public void init(){
        this.tradeID = "";
        this.documentId = "";
        this.documentUsage = "";
        this.senderId = "";
        this.receiverId = "";
        this.brokerEicId = "";
        this.receiverRole = "";
        this.documentVersion = "";
        this.market = "";
        this.commodity = "";
        this.transactionType = "";
        this.locationSn = "";
        this.deliveryPointArea = "";
        this.buyerParty = "";
        this.sellerParty = "";
        this.loadType = "";
        this.agreement = "";
        this.currency = "";
        this.totalVolume = "";
        this.totalVolumeUnit = "";
        this.tradeDate = "";
        this.capacityUnit = "";
        this.priceUnitCurrency = "";
        this.priceUnitCapacityUnit = "";
        this.deliveryStartDateAndTime = "";
        this.deliveryEndDateAndTime = "";
        this.contractCapacity = "";
        this.price = "";
        this.totalContractValue = "";
        this.agentType = "";
        this.agentName = "";
        this.brokerId = "";
        this.traderName = "";
        this.qtyPerDurationCode = "";
        this.qtyPer = "";
        this.sameQtyFlag = "";
        this.samePriceFlag = "";
        this.bkrFeeTotal = 0;
        this.bkrFeeCcy = "";
        this.netaBuyAccount = "";
        this.netaSellAccount = "";
        //this.extTrnsmssnInd = "";
        //this.netaTransmissionCharges = "";
        this.notifyAgentId = "";
        this.notifyAgentEicCode = "";
        this.ecvnaBuyerId = "";
        this.ecvnaSellerId = "";
        this.bscPartyId = "";
        this.useFractionalCapUnit = false;
        this.otcLocCdtyId = 0;
        this.useAffDeliveryTableFlag = "";
        this.optionTypeInd = "";
        this.optionStyleCode ="";
        this.optionWriter = "";
        this.optionHolder = "";
        this.premRate = "";
        this.premUnitCcyCode ="";
        this.premUnitUomCode = "";
        this.premCcyCode = "";
        this.premTotalValue = "";
        this.premPaymentDate = "";
        this.optionShedule = null;
        this.computeHourVolDayDelivery  = "";
        this.fixedOptionExpiry  = "";
        this.dailyOptionExpiry = "";

    }

    public void setTestData(){
        this.tradeID = "22918210";
        this.documentId = "CNF_040102_AFF22918210@11XSEMPRA------0";
        this.documentUsage = "Test";
        this.senderId = "11XSEMPRA------0";
        this.receiverId = "11XRWETRADING--0";
        this.receiverRole = "Trader";
        this.documentVersion = "1";
        this.market = "DE";
        this.commodity = "Power";
        this.transactionType = "FOR";
        this.deliveryPointArea = "10YDE-RWENET---I";
        this.buyerParty = "11XRWETRADING--0";
        this.sellerParty = "11XSEMPRA------0";
        this.loadType = "Peak";
        this.agreement = "EFET";
        this.currency = "EUR";
        this.totalVolume = "300";
        this.totalVolumeUnit = "MWh";
        this.tradeDate = "2004-01-02";
        this.capacityUnit = "MWh";
        this.priceUnitCurrency = "EUR";
        this.priceUnitCapacityUnit = "MWh";
        this.deliveryStartDateAndTime = "2004-01-05T08:00:00";
        this.deliveryEndDateAndTime = "2004-01-05T20:00:00";
        this.contractCapacity = "300";
        this.price = "50";
        this.qtyPer = "5";
        this.totalContractValue = "15000";
        this.agentType = "Broker";
        this.agentName = "ICAP Energy AS";
        this.brokerId = "BRICA";
        this.traderName = "Monika Budjonova";
        this.sameQtyFlag = "Y";
        this.samePriceFlag = "Y";
        this.notifyAgentEicCode = "11XSEMPRA------0";
    }
}
