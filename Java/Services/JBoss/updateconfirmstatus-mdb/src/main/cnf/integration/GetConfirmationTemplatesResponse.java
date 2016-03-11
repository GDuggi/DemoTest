
package cnf.integration;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for GetConfirmationTemplatesResponse complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="GetConfirmationTemplatesResponse">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="confirmationTemplates" type="{http://cnf/Integration}ArrayOfconfirmationTemplate"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "GetConfirmationTemplatesResponse", propOrder = {
    "confirmationTemplates"
})
public class GetConfirmationTemplatesResponse {

    @XmlElement(required = true, nillable = true)
    protected ArrayOfconfirmationTemplate confirmationTemplates;

    /**
     * Gets the value of the confirmationTemplates property.
     * 
     * @return
     *     possible object is
     *     {@link ArrayOfconfirmationTemplate }
     *     
     */
    public ArrayOfconfirmationTemplate getConfirmationTemplates() {
        return confirmationTemplates;
    }

    /**
     * Sets the value of the confirmationTemplates property.
     * 
     * @param value
     *     allowed object is
     *     {@link ArrayOfconfirmationTemplate }
     *     
     */
    public void setConfirmationTemplates(ArrayOfconfirmationTemplate value) {
        this.confirmationTemplates = value;
    }

}
