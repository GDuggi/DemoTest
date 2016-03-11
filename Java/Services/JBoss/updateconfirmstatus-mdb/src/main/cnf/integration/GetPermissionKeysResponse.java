
package cnf.integration;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlType;
import com.microsoft.schemas._2003._10.serialization.arrays.ArrayOfstring;


/**
 * <p>Java class for GetPermissionKeysResponse complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="GetPermissionKeysResponse">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="superUserFlag" type="{http://www.w3.org/2001/XMLSchema}boolean" minOccurs="0"/>
 *         &lt;element name="permissionKeyCodes" type="{http://schemas.microsoft.com/2003/10/Serialization/Arrays}ArrayOfstring"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "GetPermissionKeysResponse", propOrder = {
    "superUserFlag",
    "permissionKeyCodes"
})
public class GetPermissionKeysResponse {

    protected Boolean superUserFlag;
    @XmlElement(required = true, nillable = true)
    protected ArrayOfstring permissionKeyCodes;

    /**
     * Gets the value of the superUserFlag property.
     * 
     * @return
     *     possible object is
     *     {@link Boolean }
     *     
     */
    public Boolean isSuperUserFlag() {
        return superUserFlag;
    }

    /**
     * Sets the value of the superUserFlag property.
     * 
     * @param value
     *     allowed object is
     *     {@link Boolean }
     *     
     */
    public void setSuperUserFlag(Boolean value) {
        this.superUserFlag = value;
    }

    /**
     * Gets the value of the permissionKeyCodes property.
     * 
     * @return
     *     possible object is
     *     {@link ArrayOfstring }
     *     
     */
    public ArrayOfstring getPermissionKeyCodes() {
        return permissionKeyCodes;
    }

    /**
     * Sets the value of the permissionKeyCodes property.
     * 
     * @param value
     *     allowed object is
     *     {@link ArrayOfstring }
     *     
     */
    public void setPermissionKeyCodes(ArrayOfstring value) {
        this.permissionKeyCodes = value;
    }

}
