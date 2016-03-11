
package cnf.integration;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlElementDecl;
import javax.xml.bind.annotation.XmlRegistry;
import javax.xml.namespace.QName;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the cnf.integration package. 
 * <p>An ObjectFactory allows you to programatically 
 * construct new instances of the Java representation 
 * for XML content. The Java representation of XML 
 * content can consist of schema derived interfaces 
 * and classes representing the binding of schema 
 * type definitions, element declarations and model 
 * groups.  Factory methods for each of these are 
 * provided in this class.
 * 
 */
@XmlRegistry
public class ObjectFactory {

    private final static QName _ArrayOfconfirmationTemplate_QNAME = new QName("http://cnf/Integration", "ArrayOfconfirmationTemplate");
    private final static QName _GetPermissionKeysResponse_QNAME = new QName("http://cnf/Integration", "GetPermissionKeysResponse");
    private final static QName _ConfirmationTemplate_QNAME = new QName("http://cnf/Integration", "confirmationTemplate");
    private final static QName _WorkflowInd_QNAME = new QName("http://cnf/Integration", "WorkflowInd");
    private final static QName _ArrayOfattribute_QNAME = new QName("http://cnf/Integration", "ArrayOfattribute");
    private final static QName _Attribute_QNAME = new QName("http://cnf/Integration", "attribute");
    private final static QName _GetConfirmationTemplatesResponse_QNAME = new QName("http://cnf/Integration", "GetConfirmationTemplatesResponse");
    private final static QName _TradeConfirmationStatusChangeResponse_QNAME = new QName("http://cnf/Integration", "TradeConfirmationStatusChangeResponse");
    private final static QName _ConfirmationTemplateAttributes_QNAME = new QName("http://cnf/Integration", "attributes");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: cnf.integration
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link TradeConfirmationStatusChangeResponse }
     * 
     */
    public TradeConfirmationStatusChangeResponse createTradeConfirmationStatusChangeResponse() {
        return new TradeConfirmationStatusChangeResponse();
    }

    /**
     * Create an instance of {@link GetConfirmationTemplatesResponse }
     * 
     */
    public GetConfirmationTemplatesResponse createGetConfirmationTemplatesResponse() {
        return new GetConfirmationTemplatesResponse();
    }

    /**
     * Create an instance of {@link ArrayOfattribute }
     * 
     */
    public ArrayOfattribute createArrayOfattribute() {
        return new ArrayOfattribute();
    }

    /**
     * Create an instance of {@link GetPermissionKeysResponse }
     * 
     */
    public GetPermissionKeysResponse createGetPermissionKeysResponse() {
        return new GetPermissionKeysResponse();
    }

    /**
     * Create an instance of {@link Attribute }
     * 
     */
    public Attribute createAttribute() {
        return new Attribute();
    }

    /**
     * Create an instance of {@link ConfirmationTemplate }
     * 
     */
    public ConfirmationTemplate createConfirmationTemplate() {
        return new ConfirmationTemplate();
    }

    /**
     * Create an instance of {@link ArrayOfconfirmationTemplate }
     * 
     */
    public ArrayOfconfirmationTemplate createArrayOfconfirmationTemplate() {
        return new ArrayOfconfirmationTemplate();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfconfirmationTemplate }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/Integration", name = "ArrayOfconfirmationTemplate")
    public JAXBElement<ArrayOfconfirmationTemplate> createArrayOfconfirmationTemplate(ArrayOfconfirmationTemplate value) {
        return new JAXBElement<ArrayOfconfirmationTemplate>(_ArrayOfconfirmationTemplate_QNAME, ArrayOfconfirmationTemplate.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link GetPermissionKeysResponse }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/Integration", name = "GetPermissionKeysResponse")
    public JAXBElement<GetPermissionKeysResponse> createGetPermissionKeysResponse(GetPermissionKeysResponse value) {
        return new JAXBElement<GetPermissionKeysResponse>(_GetPermissionKeysResponse_QNAME, GetPermissionKeysResponse.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ConfirmationTemplate }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/Integration", name = "confirmationTemplate")
    public JAXBElement<ConfirmationTemplate> createConfirmationTemplate(ConfirmationTemplate value) {
        return new JAXBElement<ConfirmationTemplate>(_ConfirmationTemplate_QNAME, ConfirmationTemplate.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link WorkflowInd }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/Integration", name = "WorkflowInd")
    public JAXBElement<WorkflowInd> createWorkflowInd(WorkflowInd value) {
        return new JAXBElement<WorkflowInd>(_WorkflowInd_QNAME, WorkflowInd.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfattribute }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/Integration", name = "ArrayOfattribute")
    public JAXBElement<ArrayOfattribute> createArrayOfattribute(ArrayOfattribute value) {
        return new JAXBElement<ArrayOfattribute>(_ArrayOfattribute_QNAME, ArrayOfattribute.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Attribute }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/Integration", name = "attribute")
    public JAXBElement<Attribute> createAttribute(Attribute value) {
        return new JAXBElement<Attribute>(_Attribute_QNAME, Attribute.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link GetConfirmationTemplatesResponse }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/Integration", name = "GetConfirmationTemplatesResponse")
    public JAXBElement<GetConfirmationTemplatesResponse> createGetConfirmationTemplatesResponse(GetConfirmationTemplatesResponse value) {
        return new JAXBElement<GetConfirmationTemplatesResponse>(_GetConfirmationTemplatesResponse_QNAME, GetConfirmationTemplatesResponse.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link TradeConfirmationStatusChangeResponse }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/Integration", name = "TradeConfirmationStatusChangeResponse")
    public JAXBElement<TradeConfirmationStatusChangeResponse> createTradeConfirmationStatusChangeResponse(TradeConfirmationStatusChangeResponse value) {
        return new JAXBElement<TradeConfirmationStatusChangeResponse>(_TradeConfirmationStatusChangeResponse_QNAME, TradeConfirmationStatusChangeResponse.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfattribute }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/Integration", name = "attributes", scope = ConfirmationTemplate.class)
    public JAXBElement<ArrayOfattribute> createConfirmationTemplateAttributes(ArrayOfattribute value) {
        return new JAXBElement<ArrayOfattribute>(_ConfirmationTemplateAttributes_QNAME, ArrayOfattribute.class, ConfirmationTemplate.class, value);
    }

}
