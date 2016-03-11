package cnf.docflow.util;

import java.util.EnumMap;
import java.util.Map;

/**
 * Created by jvega on 7/21/2015.
 */
public enum RequirementType {
        OURPAPER("OURPAPER"),
        CPTYPAPER("CPTYPAPER"),
        BROKERPAPER("BROKERPAPER"),
        NotFound("");

    private String code;

    RequirementType(String code){
        this.code = code;
    }

    public String getCode() {
        return this.code;
    }

    public static RequirementType getRequirementType(String code){
        RequirementType result = NotFound;
        for (RequirementType t : values()) {
            if (t.getCode().equalsIgnoreCase(code)) {
                result = t;
            }
        }
        return result;
    }

    public static String getRequirmentCode(String code) {
        String result;
         switch (getRequirementType(code)) {
             case OURPAPER: result = "XQCSP";
                                   break;
             case CPTYPAPER: result = "XQCCP";
                                   break;
             case BROKERPAPER: result = "XQBBP";
                                   break;
             default: result = NotFound.getCode();
                      break;
         }
        return result;

    }


}
