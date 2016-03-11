package cnf.docflow.util;

/**
 * Created by jvega on 7/28/2015.
 */
public enum DebuggingType {
    Enabled("TRUE"),
    Disabled("FALSE");

    private String Code;

    DebuggingType(String code) {
        this.Code = code;
    }

    public String getCode() {
        return this.Code;
    }

    public static DebuggingType getDebuggingType(String code) {
        DebuggingType result = DebuggingType.Disabled;
        for (DebuggingType t : values()) {
            if (t.getCode().equalsIgnoreCase(code)) {
                result = t;
            }
        }
        return result;
    }
}
