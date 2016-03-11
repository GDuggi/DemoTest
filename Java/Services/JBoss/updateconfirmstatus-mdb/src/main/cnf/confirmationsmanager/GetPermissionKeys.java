
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
 *         &lt;element name="getPermissionKeysRequest" type="{http://cnf/ConfirmationsManager}GetPermissionKeysRequest" minOccurs="0"/>
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
    "getPermissionKeysRequest"
})
@XmlRootElement(name = "getPermissionKeys")
public class GetPermissionKeys {

    @XmlElementRef(name = "getPermissionKeysRequest", namespace = "http://cnf/ConfirmationsManager", type = JAXBElement.class)
    protected JAXBElement<GetPermissionKeysRequest> getPermissionKeysRequest;

    /**
     * Gets the value of the getPermissionKeysRequest property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link GetPermissionKeysRequest }{@code >}
     *     
     */
    public JAXBElement<GetPermissionKeysRequest> getGetPermissionKeysRequest() {
        return getPermissionKeysRequest;
    }

    /**
     * Sets the value of the getPermissionKeysRequest property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link GetPermissionKeysRequest }{@code >}
     *     
     */
    public void setGetPermissionKeysRequest(JAXBElement<GetPermissionKeysRequest> value) {
        this.getPermissionKeysRequest = ((JAXBElement<GetPermissionKeysRequest> ) value);
    }

}
