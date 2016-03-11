
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
 *         &lt;element name="getConfirmationTemplatesResult" type="{http://cnf/Integration}GetConfirmationTemplatesResponse" minOccurs="0"/>
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
    "getConfirmationTemplatesResult"
})
@XmlRootElement(name = "getConfirmationTemplatesResponse")
public class GetConfirmationTemplatesResponse {

    @XmlElementRef(name = "getConfirmationTemplatesResult", namespace = "http://cnf/ConfirmationsManager", type = JAXBElement.class)
    protected JAXBElement<cnf.integration.GetConfirmationTemplatesResponse> getConfirmationTemplatesResult;

    /**
     * Gets the value of the getConfirmationTemplatesResult property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link cnf.integration.GetConfirmationTemplatesResponse }{@code >}
     *     
     */
    public JAXBElement<cnf.integration.GetConfirmationTemplatesResponse> getGetConfirmationTemplatesResult() {
        return getConfirmationTemplatesResult;
    }

    /**
     * Sets the value of the getConfirmationTemplatesResult property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link cnf.integration.GetConfirmationTemplatesResponse }{@code >}
     *     
     */
    public void setGetConfirmationTemplatesResult(JAXBElement<cnf.integration.GetConfirmationTemplatesResponse> value) {
        this.getConfirmationTemplatesResult = ((JAXBElement<cnf.integration.GetConfirmationTemplatesResponse> ) value);
    }

}
