package cnf.docflow.util;

/**
 * Created by jvega on 7/17/2015.
 */
public enum TradeNotifyType {

    NEW("NEW"),
    EDIT("EDIT"),
    VOID("VOID"),
    CANCEL("CANCEL"),
    NotFound("");

    private String Code;

    TradeNotifyType(String code){
        this.Code = code;
    }

    public String getCode() {
        return this.Code;
    }

    public static TradeNotifyType getTradeNotifyType(String code){
        TradeNotifyType result = NotFound;
        for (TradeNotifyType t : values()) {
            if (t.getCode().equalsIgnoreCase(code)) {
                result = t;
            }
        }
        return result;
    }
}
