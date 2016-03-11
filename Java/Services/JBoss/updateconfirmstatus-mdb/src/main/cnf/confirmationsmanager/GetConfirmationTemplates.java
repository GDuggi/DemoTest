
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
 *         &lt;element name="getConfirmationTemplatesRequest" type="{http://cnf/ConfirmationsManager}GetConfirmationTemplatesRequest" minOccurs="0"/>
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
    "getConfirmationTemplatesRequest"
})
@XmlRootElement(name = "getConfirmationTemplates")
public class GetConfirmationTemplates {

    @XmlElementRef(name = "getConfirmationTemplatesRequest", namespace = "http://cnf/ConfirmationsManager", type = JAXBElement.class)
    protected JAXBElement<GetConfirmationTemplatesRequest> getConfirmationTemplatesRequest;

    /**
     * Gets the value of the getConfirmationTemplatesRequest property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link GetConfirmationTemplatesRequest }{@code >}
     *     
     */
    public JAXBElement<GetConfirmationTemplatesRequest> getGetConfirmationTemplatesRequest() {
        return getConfirmationTemplatesRequest;
    }

    /**
     * Sets the value of the getConfirmationTemplatesRequest property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link GetConfirmationTemplatesRequest }{@code >}
     *     
     */
    public void setGetConfirmationTemplatesRequest(JAXBElement<GetConfirmationTemplatesRequest> value) {
        this.getConfirmationTemplatesRequest = ((JAXBElement<GetConfirmationTemplatesRequest> ) value);
    }

}
