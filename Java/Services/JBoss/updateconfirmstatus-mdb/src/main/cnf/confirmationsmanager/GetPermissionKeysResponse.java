
package cnf.confirmationsmanager;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for anonymous complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType>
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="getPermissionKeysResult" type="{http://cnf/Integration}GetPermissionKeysResponse" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = {
    "getPermissionKeysResult"
})
@XmlRootElement(name = "getPermissionKeysResponse")
public class GetPermissionKeysResponse {

    @XmlElementRef(name = "getPermissionKeysResult", namespace = "http://cnf/ConfirmationsManager", type = JAXBElement.class)
    protected JAXBElement<cnf.integration.GetPermissionKeysResponse> getPermissionKeysResult;

    /**
     * Gets the value of the getPermissionKeysResult property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link cnf.integration.GetPermissionKeysResponse }{@code >}
     *     
     */
    public JAXBElement<cnf.integration.GetPermissionKeysResponse> getGetPermissionKeysResult() {
        return getPermissionKeysResult;
    }

    /**
     * Sets the value of the getPermissionKeysResult property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link cnf.integration.GetPermissionKeysResponse }{@code >}
     *     
     */
    public void setGetPermissionKeysResult(JAXBElement<cnf.integration.GetPermissionKeysResponse> value) {
        this.getPermissionKeysResult = ((JAXBElement<cnf.integration.GetPermissionKeysResponse> ) value);
    }

}
