
package cnf.confirmationsmanager;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlType;
import cnf.integration.WorkflowInd;


/**
 * <p>Java class for TradeConfirmationStatusChangeRequest complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="TradeConfirmationStatusChangeRequest">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="confirmationStatusCode" type="{http://www.w3.org/2001/XMLSchema}string"/>
 *         &lt;element name="tradingSystemCode" type="{http://www.w3.org/2001/XMLSchema}string"/>
 *         &lt;element name="tradingSystemKey" type="{http://www.w3.org/2001/XMLSchema}string"/>
 *         &lt;element name="workflowInd" type="{http://cnf/Integration}WorkflowInd" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "TradeConfirmationStatusChangeRequest", propOrder = {
    "confirmationStatusCode",
    "tradingSystemCode",
    "tradingSystemKey",
    "workflowInd"
})
public class TradeConfirmationStatusChangeRequest {

    @XmlElement(required = true, nillable = true)
    protected String confirmationStatusCode;
    @XmlElement(required = true, nillable = true)
    protected String tradingSystemCode;
    @XmlElement(required = true, nillable = true)
    protected String tradingSystemKey;
    protected WorkflowInd workflowInd;

    /**
     * Gets the value of the confirmationStatusCode property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getConfirmationStatusCode() {
        return confirmationStatusCode;
    }

    /**
     * Sets the value of the confirmationStatusCode property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setConfirmationStatusCode(String value) {
        this.confirmationStatusCode = value;
    }

    /**
     * Gets the value of the tradingSystemCode property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getTradingSystemCode() {
        return tradingSystemCode;
    }

    /**
     * Sets the value of the tradingSystemCode property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setTradingSystemCode(String value) {
        this.tradingSystemCode = value;
    }

    /**
     * Gets the value of the tradingSystemKey property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getTradingSystemKey() {
        return tradingSystemKey;
    }

    /**
     * Sets the value of the tradingSystemKey property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setTradingSystemKey(String value) {
        this.tradingSystemKey = value;
    }

    /**
     * Gets the value of the workflowInd property.
     * 
     * @return
     *     possible object is
     *     {@link WorkflowInd }
     *     
     */
    public WorkflowInd getWorkflowInd() {
        return workflowInd;
    }

    /**
     * Sets the value of the workflowInd property.
     * 
     * @param value
     *     allowed object is
     *     {@link WorkflowInd }
     *     
     */
    public void setWorkflowInd(WorkflowInd value) {
        this.workflowInd = value;
    }

}
