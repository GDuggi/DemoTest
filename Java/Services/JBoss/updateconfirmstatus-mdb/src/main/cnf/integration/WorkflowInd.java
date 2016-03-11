
package cnf.integration;

import javax.xml.bind.annotation.XmlEnum;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for WorkflowInd.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * <p>
 * <pre>
 * &lt;simpleType name="WorkflowInd">
 *   &lt;restriction base="{http://www.w3.org/2001/XMLSchema}string">
 *     &lt;enumeration value="FINALAPPROVAL"/>
 *     &lt;enumeration value="OURPAPER"/>
 *     &lt;enumeration value="CPTYPAPER"/>
 *     &lt;enumeration value="BROKERPAPER"/>
 *   &lt;/restriction>
 * &lt;/simpleType>
 * </pre>
 * 
 */
@XmlType(name = "WorkflowInd")
@XmlEnum
public enum WorkflowInd {

    FINALAPPROVAL,
    OURPAPER,
    CPTYPAPER,
    BROKERPAPER;

    public String value() {
        return name();
    }

    public static WorkflowInd fromValue(String v) {
        return valueOf(v);
    }

}
