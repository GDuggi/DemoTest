package cnf.docflow.data;

import cnf.docflow.util.ConversionUtils;
import cnf.docflow.util.MessageTagType;
import cnf.docflow.util.RequirementType;
import cnf.docflow.util.TradeNotifyType;

import java.sql.Date;
import java.sql.Timestamp;
import java.text.ParseException;
import java.util.*;

/**
 * Created by jvega on 7/14/2015.
 */
public class ConfirmMessageData {
    //ConfirmMessage
    private String confirm_NotifyType;
    private String confirm_NotifyTsGmt;

    //TradeData
    private String trade_TradingSystemCode;
    private String trade_TradingSystemTicket;
    private String trade_CreationDt;
    private String trade_TradeDt;
    private String trade_Trader;
    private String trade_BookingCompany;
    private String trade_BookingCompanyId;
    private String trade_CptySn;
    private String trade_CptyLegalName;
    private String trade_CptyId;
    private String trade_CdtySn;
    private String trade_CdtyGroup;
    private String trade_BuySellInd;
    private String trade_QtyDesc;
    private String trade_QtyTot;
    private String trade_StartDt;
    private String trade_EndDt;
    private String trade_LocationSn;
    private String trade_TransportDesc;
    private String trade_PriceDesc;
    private String trade_TradeDesc;
    private String trade_TradeTypeCode;
    private String trade_SttlType;
    private String trade_BrokerSn;
    private String trade_BrokerLegalName;
    private String trade_BrokerId;
    private String trade_BrokerPriceDesc;
    private String trade_Book;
    private String trade_ProfitCenter;
    private String trade_RefSn;
    private String trade_Xref;
    private String trade_TradeStatCode;
    private String trade_OptnStrikePrice;
    private String trade_OptnPremPrice;
    private String trade_OptnPutCallInd;
    private String trade_PermissionKey;
   // private String trade_Qty;
   // private String trade_UomDurCode;
   // private String trade_Comm;
   // private String trade_PayPrice;
   // private String trade_RecPrice;
    //private String trade_CdtyGrpCode;
   // private String trade_EfsFlag;
   // private String trade_EfsCptySn;

    private int other_TradeID;
    private int other_TradeNotifyID;

    //Rqmt
    List<ConfirmRequirementData> ConfRqmtList;

    public ConfirmMessageData() {
        this.confirm_NotifyType = "";
        this.confirm_NotifyTsGmt = "";
        this.trade_TradingSystemCode = "";
        this.trade_TradingSystemTicket = "";
        this.trade_CreationDt = "";
        this.trade_CdtySn = "";
        this.trade_TradeDt= "";
        this.trade_Xref = "";
        this.trade_CptySn = "";
        this.trade_CptyLegalName = "";
        this.trade_CptyId = "";
        this.trade_QtyTot = "";
        this.trade_LocationSn = "";
        this.trade_PriceDesc = "";
        this.trade_StartDt = "";
        this.trade_EndDt = "";
        this.trade_Book = "";
        this.trade_TradeTypeCode = "";
        this.trade_SttlType = "";
        this.trade_BrokerSn = "";
        this.trade_BrokerLegalName = "";
        this.trade_BrokerId = "";
        this.trade_BuySellInd = "";
        this.trade_RefSn = "";
        this.trade_BookingCompany = "";
        this.trade_BookingCompanyId = "";
        this.trade_ProfitCenter = "";
        this.trade_TradeStatCode = "";
        this.trade_BrokerPriceDesc = "";
        this.trade_OptnStrikePrice = "";
        this.trade_OptnPremPrice = "";
        this.trade_OptnPutCallInd = "";
        this.trade_PermissionKey = "";
        this.trade_QtyDesc = "";
        this.trade_TradeDesc = "";
        this.trade_CdtyGroup = "";
        this.trade_Trader = "";
        this.trade_TransportDesc = "";

        List<ConfirmRequirementData> ConfRqmtList = new LinkedList<ConfirmRequirementData>();

        };

    public String getConfirm_NotifyType() {
        return confirm_NotifyType;
    }

    public void setConfirm_NotifyType(String confirm_NotifyType) {
        this.confirm_NotifyType = confirm_NotifyType;
    }

    public String getConfirm_NotifyTsGmt() {
        return confirm_NotifyTsGmt;
    }
    public Timestamp getConfirm_NotifyTsGmt_asDateTime() throws ParseException {
        return ConversionUtils.convertToTimeStamp(confirm_NotifyTsGmt);
    }

    public void setConfirm_NotifyTsGmt(String confirm_NotifyTsGmt) {
        this.confirm_NotifyTsGmt = confirm_NotifyTsGmt;
    }

    public String getTrade_TradingSystemCode() {
        return trade_TradingSystemCode;
    }

    public void setTrade_TradingSystemCode(String trade_TradingSystemCode) {
        this.trade_TradingSystemCode = trade_TradingSystemCode;
    }

    public String getTrade_TradingSystemTicket() {
        return trade_TradingSystemTicket;
    }

    public void setTrade_TradingSystemTicket(String trade_TradingSystemTicket) {
        this.trade_TradingSystemTicket = trade_TradingSystemTicket;
    }

    public String getTrade_CreationDt() {
        return trade_CreationDt;
    }

    public Date getTrade_CreationDt_asDate() throws ParseException {
        return ConversionUtils.convertToDateTime(trade_CreationDt);
    }

    public void setTrade_CreationDt(String trade_CreationDt) {
        this.trade_CreationDt = trade_CreationDt;
    }

    public String getTrade_CdtySn() {
        return trade_CdtySn;
    }

    public void setTrade_CdtySn(String trade_CdtySn) {
        this.trade_CdtySn = trade_CdtySn;
    }

    public String getTrade_TradeDt() {
        return trade_TradeDt;
    }

    public Date getTrade_TradeDt_asDate() throws ParseException {
        return ConversionUtils.convertToDateLong(trade_TradeDt);
    }

    public void setTrade_TradeDt(String trade_TradeDt) {
        this.trade_TradeDt = trade_TradeDt;
    }

    public String getTrade_Xref() {
        return trade_Xref;
    }

    public void setTrade_Xref(String trade_Xref) {
        this.trade_Xref = trade_Xref;
    }

    public String getTrade_CptySn() {
        return trade_CptySn;
    }

    public void setTrade_CptySn(String trade_CptySn) {
        this.trade_CptySn = trade_CptySn;
    }

    public String getTrade_CptyId() {
        return trade_CptyId;
    }

    public void setTrade_CptyId(String trade_CptyId) {
        this.trade_CptyId = trade_CptyId;
    }

    public String getTrade_QtyTot() {
        return trade_QtyTot;
    }

    public Double getTrade_QtyTot_asDouble() throws NumberFormatException {
        return ConversionUtils.convertToDouble(trade_QtyTot);
    }

    public void setTrade_QtyTot(String trade_QtyTot) {
        this.trade_QtyTot = trade_QtyTot;
    }

    public String getTrade_LocationSn() {
        return trade_LocationSn;
    }

    public void setTrade_LocationSn(String trade_LocationSn) {
        this.trade_LocationSn = trade_LocationSn;
    }

    public String getTrade_PriceDesc() {
        return trade_PriceDesc;
    }

    public void setTrade_PriceDesc(String trade_PriceDesc) {
        this.trade_PriceDesc = trade_PriceDesc;
    }

    public String getTrade_StartDt() {
        return trade_StartDt;
    }

    public Date getTrade_StartDt_asDate() throws ParseException {
        return ConversionUtils.convertToDateShort(trade_StartDt);
    }

    public void setTrade_StartDt(String trade_StartDt) {
        this.trade_StartDt = trade_StartDt;
    }

    public String getTrade_EndDt() {
        return trade_EndDt;
    }

    public Date getTrade_EndDt_asDate() throws ParseException {
        return ConversionUtils.convertToDateShort(trade_EndDt);
    }

    public void setTrade_EndDt(String trade_EndDt) {
        this.trade_EndDt = trade_EndDt;
    }

    public String getTrade_Book() {
        return trade_Book;
    }

    public void setTrade_Book(String trade_Book) {
        this.trade_Book = trade_Book;
    }

    public String getTrade_TradeTypeCode() {
        return trade_TradeTypeCode;
    }

    public void setTrade_TradeTypeCode(String trade_TradeTypeCode) {
        this.trade_TradeTypeCode = trade_TradeTypeCode;
    }

    public String getTrade_SttlType() {
        return trade_SttlType;
    }

    public void setTrade_SttlType(String trade_SttlType) {
        this.trade_SttlType = trade_SttlType;
    }

    public String getTrade_BrokerSn() {
        return trade_BrokerSn;
    }

    public void setTrade_BrokerSn(String trade_BrokerSn) {
        this.trade_BrokerSn = trade_BrokerSn;
    }

    public String getTrade_BrokerLegalName() {
        return trade_BrokerLegalName;
    }

    public void setTrade_BrokerLegalName(String trade_BrokerLegalName) {
        this.trade_BrokerLegalName = trade_BrokerLegalName;
    }

    public String getTrade_BrokerId() {
        return trade_BrokerId;
    }

    public void setTrade_BrokerId(String trade_BrokerId) {
        this.trade_BrokerId = trade_BrokerId;
    }

    public String getTrade_BuySellInd() {
        return trade_BuySellInd;
    }

    public void setTrade_BuySellInd(String trade_BuySellInd) {
        this.trade_BuySellInd = trade_BuySellInd;
    }

    public String getTrade_RefSn() {
        return trade_RefSn;
    }

    public void setTrade_RefSn(String trade_RefSn) {
        this.trade_RefSn = trade_RefSn;
    }

    public String getTrade_BookingCompany() {
        return trade_BookingCompany;
    }

    public void setTrade_BookingCompany(String trade_BookingCompany) {
        this.trade_BookingCompany = trade_BookingCompany;
    }

    public String getTrade_BookingCompanyId() {
        return trade_BookingCompanyId;
    }

    public void setTrade_BookingCompanyId(String trade_BookingCompanyId) {
        this.trade_BookingCompanyId = trade_BookingCompanyId;
    }

    public String getTrade_TradeStatCode() {
        return trade_TradeStatCode;
    }

    public void setTrade_TradeStatCode(String trade_TradeStatCode) {
        this.trade_TradeStatCode = trade_TradeStatCode;
    }

    public String getTrade_BrokerPriceDesc() {
        return trade_BrokerPriceDesc;
    }

    public void setTrade_BrokerPriceDesc(String trade_BrokerPriceDesc) {
        this.trade_BrokerPriceDesc = trade_BrokerPriceDesc;
    }

    public String getTrade_OptnStrikePrice() {
        return trade_OptnStrikePrice;
    }

    public void setTrade_OptnStrikePrice(String trade_OptnStrikePrice) {
        this.trade_OptnStrikePrice = trade_OptnStrikePrice;
    }

    public String getTrade_OptnPremPrice() {
        return trade_OptnPremPrice;
    }

    public void setTrade_OptnPremPrice(String trade_OptnPremPrice) {
        this.trade_OptnPremPrice = trade_OptnPremPrice;
    }

    public String getTrade_OptnPutCallInd() {
        return trade_OptnPutCallInd;
    }

    public void setTrade_OptnPutCallInd(String trade_OptnPutCallInd) {
        this.trade_OptnPutCallInd = trade_OptnPutCallInd;
    }

    public List<ConfirmRequirementData> getConfRqmtList() {
        return ConfRqmtList;
    }

    public void setConfRqmtList(List<ConfirmRequirementData> confRqmtList) {
        ConfRqmtList = confRqmtList;
    }

    public String getTrade_CptyLegalName() {
        return trade_CptyLegalName;
    }

    public void setTrade_CptyLegalName(String trade_CptyLegalName) {
        this.trade_CptyLegalName = trade_CptyLegalName;
    }

    public String getTrade_ProfitCenter() {
        return trade_ProfitCenter;
    }

    public void setTrade_ProfitCenter(String trade_ProfitCenter) {
        this.trade_ProfitCenter = trade_ProfitCenter;
    }

    public String getTrade_PermissionKey() {
        return trade_PermissionKey;
    }

    public void setTrade_PermissionKey(String trade_PermissionKey) {
        this.trade_PermissionKey = trade_PermissionKey;
    }

    public int getOther_TradeID() {
        return other_TradeID;
    }

    public void setOther_TradeID(int other_TradeID) {
        this.other_TradeID = other_TradeID;
    }

    public int getOther_TradeNotifyID() {
        return other_TradeNotifyID;
    }

    public void setOther_TradeNotifyID(int other_TradeNotifyID) {
        this.other_TradeNotifyID = other_TradeNotifyID;
    }

    public String getTrade_QtyDesc() {
        return trade_QtyDesc;
    }

    public void setTrade_QtyDesc(String trade_QtyDesc) {
        this.trade_QtyDesc = trade_QtyDesc;
    }

    public String getTrade_TradeDesc() {
        return trade_TradeDesc;
    }

    public void setTrade_TradeDesc(String trade_TradeDesc) {
        this.trade_TradeDesc = trade_TradeDesc;
    }

    public String getTrade_Trader() {
        return trade_Trader;
    }

    public void setTrade_Trader(String trade_Trader) {
        this.trade_Trader = trade_Trader;
    }

    public String getTrade_CdtyGroup() {
        return trade_CdtyGroup;
    }

    public void setTrade_CdtyGroup(String trade_CdtyGroup) {
        this.trade_CdtyGroup = trade_CdtyGroup;
    }

    public String getTrade_TransportDesc() {
        return trade_TransportDesc;
    }

    public void setTrade_TransportDesc(String trade_TransportDesc) {
        this.trade_TransportDesc = trade_TransportDesc;
    }

    @Override
    public String toString() {
        return "ConfirmMessageData{" +
                "confirm_NotifyType='" + confirm_NotifyType + '\'' +
                ", confirm_NotifyTsGmt='" + confirm_NotifyTsGmt + '\'' +
                ", trade_TradingSystemCode='" + trade_TradingSystemCode + '\'' +
                ", trade_TradingSystemTicket='" + trade_TradingSystemTicket + '\'' +
                ", trade_CreationDt='" + trade_CreationDt + '\'' +
                ", trade_CdtySn='" + trade_CdtySn + '\'' +
                ", trade_TradeDt='" + trade_TradeDt + '\'' +
                ", trade_Xref='" + trade_Xref + '\'' +
                ", trade_CptySn='" + trade_CptySn + '\'' +
                ", trade_CptyId='" + trade_CptyId + '\'' +
                ", trade_CptyLegalName='" + trade_CptyLegalName + '\'' +
                ", trade_QtyTot='" + trade_QtyTot + '\'' +
                ", trade_LocationSn='" + trade_LocationSn + '\'' +
                ", trade_PriceDesc='" + trade_PriceDesc + '\'' +
                ", trade_StartDt='" + trade_StartDt + '\'' +
                ", trade_EndDt='" + trade_EndDt + '\'' +
                ", trade_Book='" + trade_Book + '\'' +
                ", trade_TradeTypeCode='" + trade_TradeTypeCode + '\'' +
                ", trade_SttlType='" + trade_SttlType + '\'' +
                ", trade_BrokerSn='" + trade_BrokerSn + '\'' +
                ", trade_BrokerLegalName='" + trade_BrokerLegalName + '\'' +
                ", trade_BrokerId='" + trade_BrokerId + '\'' +
                ", trade_BuySellInd='" + trade_BuySellInd + '\'' +
                ", trade_RefSn='" + trade_RefSn + '\'' +
                ", trade_BookingCompany='" + trade_BookingCompany + '\'' +
                ", trade_BookingCompanyId='" + trade_BookingCompanyId + '\'' +
                ", trade_ProfitCenter='" + trade_ProfitCenter + '\'' +
                ", trade_TradeStatCode='" + trade_TradeStatCode + '\'' +
                ", trade_BrokerPriceDesc='" + trade_BrokerPriceDesc + '\'' +
                ", trade_OptnStrikePrice='" + trade_OptnStrikePrice + '\'' +
                ", trade_OptnPremPrice='" + trade_OptnPremPrice + '\'' +
                ", trade_OptnPutCallInd='" + trade_OptnPutCallInd + '\'' +
                ", trade_PermissionKey='" + trade_PermissionKey + '\'' +
                ", trade_QtyDesc='" + trade_QtyDesc + '\'' +
                ", trade_TradeDesc='" + trade_TradeDesc + '\'' +
                ", trade_CdtyGroup='" + trade_CdtyGroup + '\'' +
                ", trade_Trader='" + trade_Trader + '\'' +
                ", trade_TransportDesc='" + trade_TransportDesc + '\'' +
                ", ConfRqmtList=" + ConfRqmtList +
                '}';
    }

    public void validate() {
        if (ConversionUtils.isEmptyString(confirm_NotifyType)) {
            throw new IllegalArgumentException("Value for " + MessageTagType.NotifyType.getCode() + " is missing.");
        } else {
            if (TradeNotifyType.getTradeNotifyType(confirm_NotifyType).getCode().equalsIgnoreCase(TradeNotifyType.NotFound.getCode())) {
                throw new IllegalArgumentException(MessageTagType.NotifyType.getCode() + "=" + confirm_NotifyType + " is not valid.");
            }
        }
        if (ConversionUtils.isEmptyString(trade_TradingSystemCode)) {
            throw new IllegalArgumentException("Value for " + MessageTagType.TradingSystemCode.getCode() + " is missing.");
        }
        if (ConversionUtils.isEmptyString(trade_TradingSystemTicket)) {
            throw new IllegalArgumentException("Value for " + MessageTagType.TradingSystemTicket.getCode() + " is missing.");
        }

        boolean isRqrdToValidateTrdData = true;
        if (confirm_NotifyType.equalsIgnoreCase(TradeNotifyType.VOID.getCode()) ||
                confirm_NotifyType.equalsIgnoreCase(TradeNotifyType.CANCEL.getCode())) {

            isRqrdToValidateTrdData = false;
        }
        if (isRqrdToValidateTrdData) {
            if (ConversionUtils.isEmptyString(trade_CptySn)) {
                throw new IllegalArgumentException("Value for " + MessageTagType.CptySn.getCode() + " is missing.");
            }
            if (ConversionUtils.isEmptyString(trade_CptyLegalName)) {
                throw new IllegalArgumentException("Value for " + MessageTagType.CptyLegalName.getCode() + " is missing.");
            }
            if (ConversionUtils.isEmptyString(trade_CptyId)) {
                throw new IllegalArgumentException("Value for " + MessageTagType.CptyId.getCode() + " is missing.");
            }
            if (!ConversionUtils.isEmptyString(confirm_NotifyTsGmt)) {
                try {
                    Timestamp temp = getConfirm_NotifyTsGmt_asDateTime();
                } catch (ParseException e) {
                    throw new IllegalArgumentException(MessageTagType.NotifyTsGmt.getCode() + "=" + confirm_NotifyTsGmt + " is not valid.  Expected format: '" + ConversionUtils.getDatePattern(confirm_NotifyTsGmt) + "'.", e); //ConversionUtils.sdfDateTime.toPattern()
                }
            }
            if (!ConversionUtils.isEmptyString(trade_CreationDt)) {
                try {
                    Date temp = getTrade_CreationDt_asDate();
                } catch (ParseException e) {
                    throw new IllegalArgumentException(MessageTagType.CreationDt.getCode() + "=" + trade_CreationDt + " is not valid.  Expected format: '" + ConversionUtils.getDatePattern(trade_CreationDt) + "'.", e);//ConversionUtils.sdfDateTime.toPattern()
                }
            }
            else{
                throw new IllegalArgumentException("Value for " + MessageTagType.CreationDt.getCode() + " is missing.");
            }

            if (!ConversionUtils.isEmptyString(trade_TradeDt)) {
                try {
                    Date temp = getTrade_TradeDt_asDate();
                } catch (ParseException e) {
                    throw new IllegalArgumentException(MessageTagType.TradeDt.getCode() + "=" + trade_TradeDt + " is not valid.  Expected format: '" + ConversionUtils.getDatePattern(trade_TradeDt) + "'.", e); //ConversionUtils.sdfDateLong.toPattern()
                }
            }
            else{
                throw new IllegalArgumentException("Value for " + MessageTagType.TradeDt.getCode() + " is missing.");
            }

            if (!ConversionUtils.isEmptyString(trade_StartDt)) {
                try {
                    Date temp = getTrade_StartDt_asDate();
                } catch (ParseException e) {
                    throw new IllegalArgumentException(MessageTagType.StartDt.getCode() + "=" + trade_StartDt + " is not valid.  Expected format: '" + ConversionUtils.sdfDateShort.toPattern() + "'.", e);
                }
            }
            else{
                throw new IllegalArgumentException("Value for " + MessageTagType.StartDt.getCode() + " is missing.");
            }

            if (!ConversionUtils.isEmptyString(trade_EndDt)) {
                try {
                    Date temp = getTrade_EndDt_asDate();
                } catch (ParseException e) {
                    throw new IllegalArgumentException(MessageTagType.EndDt.getCode() + "=" + trade_EndDt + " is not valid.  Expected format: '" + ConversionUtils.sdfDateShort.toPattern() + "'.", e);
                }
            }
            else{
                throw new IllegalArgumentException("Value for " + MessageTagType.EndDt.getCode() + " is missing.");
            }
            if (ConversionUtils.isEmptyString(trade_PermissionKey)) {
                throw new IllegalArgumentException("Value for " + MessageTagType.PermissionKey.getCode() + " is missing.");
            }

            if(ConfRqmtList == null || ConfRqmtList.isEmpty()){
                throw new IllegalArgumentException("There is no trade requirements.");
            }
            for (ConfirmRequirementData rqmtItem : ConfRqmtList) {
                if (!rqmtItem.getRqmt_Workflow().isEmpty()) {
                    if (RequirementType.getRequirmentCode(rqmtItem.getRqmt_Workflow()).equalsIgnoreCase(RequirementType.NotFound.getCode())) {
                        throw new IllegalArgumentException(MessageTagType.Workflow.getCode() + "=" + rqmtItem.getRqmt_Workflow() + " is not valid.");
                    }
                    if (rqmtItem.getRqmt_Workflow().equalsIgnoreCase(RequirementType.OURPAPER.getCode())) {
                        if (rqmtItem.getRqmt_Template().isEmpty()) {
                            throw new IllegalArgumentException(MessageTagType.Workflow.getCode() + "=" + RequirementType.OURPAPER.getCode() + ". " +
                                    MessageTagType.Template.getCode() + " is required");
                        }
                    }
                }
            }
        }
    }
}


