
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
 *         &lt;element name="tradeConfirmationStatusChangeResult" type="{http://cnf/Integration}TradeConfirmationStatusChangeResponse" minOccurs="0"/>
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
    "tradeConfirmationStatusChangeResult"
})
@XmlRootElement(name = "tradeConfirmationStatusChangeResponse")
public class TradeConfirmationStatusChangeResponse {

    @XmlElementRef(name = "tradeConfirmationStatusChangeResult", namespace = "http://cnf/ConfirmationsManager", type = JAXBElement.class)
    protected JAXBElement<cnf.integration.TradeConfirmationStatusChangeResponse> tradeConfirmationStatusChangeResult;

    /**
     * Gets the value of the tradeConfirmationStatusChangeResult property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link cnf.integration.TradeConfirmationStatusChangeResponse }{@code >}
     *     
     */
    public JAXBElement<cnf.integration.TradeConfirmationStatusChangeResponse> getTradeConfirmationStatusChangeResult() {
        return tradeConfirmationStatusChangeResult;
    }

    /**
     * Sets the value of the tradeConfirmationStatusChangeResult property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link cnf.integration.TradeConfirmationStatusChangeResponse }{@code >}
     *     
     */
    public void setTradeConfirmationStatusChangeResult(JAXBElement<cnf.integration.TradeConfirmationStatusChangeResponse> value) {
        this.tradeConfirmationStatusChangeResult = ((JAXBElement<cnf.integration.TradeConfirmationStatusChangeResponse> ) value);
    }

}
