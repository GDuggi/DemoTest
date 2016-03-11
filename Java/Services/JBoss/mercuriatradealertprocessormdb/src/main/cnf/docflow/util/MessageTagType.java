package cnf.docflow.util;

/**
 * Created by jvega on 7/22/2015.
 */
public enum MessageTagType {

    NotifyType("NotifyType"),
    NotifyTsGmt("NotifyTsGmt"),

    TradingSystemCode("TradingSystemCode"),
    TradingSystemTicket("TradingSystemTicket"),

    CreationDt("CreationDt"),
    CdtySn("CdtySn"),
    TradeDt("TradeDt"),
    Xref("Xref"),
    CptySn("CptySn"),
    CptyLegalName("CptyLegalName"),
    QtyTot("QtyTot"),
    LocationSn("LocationSn"),
    PriceDesc("PriceDesc"),
    StartDt("StartDt"),
    EndDt("EndDt"),
    Book("Book"),
    TradeTypeCode("TradeTypeCode"),
    SttlType("SttlType"),
    BrokerSn("BrokerSn"),
    BuySellInd("BuySellInd"),
    RefSn("RefSn"),
    PayPrice("PayPrice"),
    RecPrice("RecPrice"),
    BookingCompany("BookingCompany"),
    ProfitCenter("ProfitCenter"),
    TradeStatCode("TradeStatCode"),
    BrokerPriceDesc("BrokerPriceDesc"),
    OptnStrikePrice("OptnStrikePrice"),
    OptnPremPrice("OptnPremPrice"),
    OptnPutCallInd("OptnPutCallInd"),
    PermissionKey("PermissionKey"),
    QtyDesc("QtyDesc"),
    TradeDesc("TradeDesc"),
    Workflow("Workflow"),
    Template("Template"),
    transmitMethodInd("transmitMethodInd"),
    faxCountryCode("faxCountryCode"),
    faxAreaCode("faxAreaCode"),
    faxLocalNumber("faxLocalNumber"),
    emailAddress("emailAddress"),
    CptyId("CptyId"),
    BookingCompanyId("BookingCompanyId"),
    BrokerId("BrokerId"),
    BrokerLegalName("BrokerLegalName"),
    CdtyGroup("CdtyGroup"),
    Trader("Trader"),
    TransportDesc("TransportDesc"),
    PreparerCanSend("PreparerCanSend");

    private String Code;

    MessageTagType(String code) {
        this.Code = code;
    }

    public String getCode() {
        return this.Code;
    }

    public static MessageTagType getMessageTagType(String code) {
        MessageTagType result = null;
        for (MessageTagType t : values()) {
            if (t.getCode().equalsIgnoreCase(code)) {
                result = t;
            }
        }
        return result;
    }
}
