package cnf.docflow.util;

/**
 * Created by jvega on 7/22/2015.
 */
public enum MessageSectionType {
    ConfirmMessage("ConfirmMessage"),
    TradeData("TradeData"),
    Rqmt("Rqmt"),
    SendTo("SendTo");

    private String Code;

    MessageSectionType(String code) {
        this.Code = code;
    }

    public String getCode() {
        return this.Code;
    }

    public static MessageSectionType getMessageSectionType(String code) {
        MessageSectionType result = null;
        for (MessageSectionType t : values()) {
            if (t.getCode().equalsIgnoreCase(code)) {
                result = t;
            }
        }
        return result;
    }
}