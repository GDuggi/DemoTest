
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
 *         &lt;element name="tradeConfirmationStatusChangeRequest" type="{http://cnf/ConfirmationsManager}TradeConfirmationStatusChangeRequest" minOccurs="0"/>
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
    "tradeConfirmationStatusChangeRequest"
})
@XmlRootElement(name = "tradeConfirmationStatusChange")
public class TradeConfirmationStatusChange {

    @XmlElementRef(name = "tradeConfirmationStatusChangeRequest", namespace = "http://cnf/ConfirmationsManager", type = JAXBElement.class)
    protected JAXBElement<TradeConfirmationStatusChangeRequest> tradeConfirmationStatusChangeRequest;

    /**
     * Gets the value of the tradeConfirmationStatusChangeRequest property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link TradeConfirmationStatusChangeRequest }{@code >}
     *     
     */
    public JAXBElement<TradeConfirmationStatusChangeRequest> getTradeConfirmationStatusChangeRequest() {
        return tradeConfirmationStatusChangeRequest;
    }

    /**
     * Sets the value of the tradeConfirmationStatusChangeRequest property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link TradeConfirmationStatusChangeRequest }{@code >}
     *     
     */
    public void setTradeConfirmationStatusChangeRequest(JAXBElement<TradeConfirmationStatusChangeRequest> value) {
        this.tradeConfirmationStatusChangeRequest = ((JAXBElement<TradeConfirmationStatusChangeRequest> ) value);
    }

}
