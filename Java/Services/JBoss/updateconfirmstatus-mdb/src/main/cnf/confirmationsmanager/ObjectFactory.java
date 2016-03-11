
package cnf.confirmationsmanager;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlElementDecl;
import javax.xml.bind.annotation.XmlRegistry;
import javax.xml.namespace.QName;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the cnf.confirmationsmanager package. 
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

    private final static QName _TradeConfirmationStatusChangeRequest_QNAME = new QName("http://cnf/ConfirmationsManager", "TradeConfirmationStatusChangeRequest");
    private final static QName _GetPermissionKeysRequest_QNAME = new QName("http://cnf/ConfirmationsManager", "GetPermissionKeysRequest");
    private final static QName _GetConfirmationTemplatesRequest_QNAME = new QName("http://cnf/ConfirmationsManager", "GetConfirmationTemplatesRequest");
    private final static QName _GetConfirmationTemplatesGetConfirmationTemplatesRequest_QNAME = new QName("http://cnf/ConfirmationsManager", "getConfirmationTemplatesRequest");
    private final static QName _GetConfirmationTemplatesResponseGetConfirmationTemplatesResult_QNAME = new QName("http://cnf/ConfirmationsManager", "getConfirmationTemplatesResult");
    private final static QName _GetPermissionKeysResponseGetPermissionKeysResult_QNAME = new QName("http://cnf/ConfirmationsManager", "getPermissionKeysResult");
    private final static QName _TradeConfirmationStatusChangeResponseTradeConfirmationStatusChangeResult_QNAME = new QName("http://cnf/ConfirmationsManager", "tradeConfirmationStatusChangeResult");
    private final static QName _GetPermissionKeysGetPermissionKeysRequest_QNAME = new QName("http://cnf/ConfirmationsManager", "getPermissionKeysRequest");
    private final static QName _TradeConfirmationStatusChangeTradeConfirmationStatusChangeRequest_QNAME = new QName("http://cnf/ConfirmationsManager", "tradeConfirmationStatusChangeRequest");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: cnf.confirmationsmanager
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link GetPermissionKeysRequest }
     * 
     */
    public GetPermissionKeysRequest createGetPermissionKeysRequest() {
        return new GetPermissionKeysRequest();
    }

    /**
     * Create an instance of {@link GetConfirmationTemplates }
     * 
     */
    public GetConfirmationTemplates createGetConfirmationTemplates() {
        return new GetConfirmationTemplates();
    }

    /**
     * Create an instance of {@link cnf.confirmationsmanager.GetConfirmationTemplatesResponse }
     * 
     */
    public cnf.confirmationsmanager.GetConfirmationTemplatesResponse createGetConfirmationTemplatesResponse() {
        return new cnf.confirmationsmanager.GetConfirmationTemplatesResponse();
    }

    /**
     * Create an instance of {@link cnf.confirmationsmanager.TradeConfirmationStatusChangeResponse }
     * 
     */
    public cnf.confirmationsmanager.TradeConfirmationStatusChangeResponse createTradeConfirmationStatusChangeResponse() {
        return new cnf.confirmationsmanager.TradeConfirmationStatusChangeResponse();
    }

    /**
     * Create an instance of {@link GetPermissionKeys }
     * 
     */
    public GetPermissionKeys createGetPermissionKeys() {
        return new GetPermissionKeys();
    }

    /**
     * Create an instance of {@link GetConfirmationTemplatesRequest }
     * 
     */
    public GetConfirmationTemplatesRequest createGetConfirmationTemplatesRequest() {
        return new GetConfirmationTemplatesRequest();
    }

    /**
     * Create an instance of {@link TradeConfirmationStatusChangeRequest }
     * 
     */
    public TradeConfirmationStatusChangeRequest createTradeConfirmationStatusChangeRequest() {
        return new TradeConfirmationStatusChangeRequest();
    }

    /**
     * Create an instance of {@link cnf.confirmationsmanager.GetPermissionKeysResponse }
     * 
     */
    public cnf.confirmationsmanager.GetPermissionKeysResponse createGetPermissionKeysResponse() {
        return new cnf.confirmationsmanager.GetPermissionKeysResponse();
    }

    /**
     * Create an instance of {@link TradeConfirmationStatusChange }
     * 
     */
    public TradeConfirmationStatusChange createTradeConfirmationStatusChange() {
        return new TradeConfirmationStatusChange();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link TradeConfirmationStatusChangeRequest }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/ConfirmationsManager", name = "TradeConfirmationStatusChangeRequest")
    public JAXBElement<TradeConfirmationStatusChangeRequest> createTradeConfirmationStatusChangeRequest(TradeConfirmationStatusChangeRequest value) {
        return new JAXBElement<TradeConfirmationStatusChangeRequest>(_TradeConfirmationStatusChangeRequest_QNAME, TradeConfirmationStatusChangeRequest.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link GetPermissionKeysRequest }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/ConfirmationsManager", name = "GetPermissionKeysRequest")
    public JAXBElement<GetPermissionKeysRequest> createGetPermissionKeysRequest(GetPermissionKeysRequest value) {
        return new JAXBElement<GetPermissionKeysRequest>(_GetPermissionKeysRequest_QNAME, GetPermissionKeysRequest.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link GetConfirmationTemplatesRequest }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/ConfirmationsManager", name = "GetConfirmationTemplatesRequest")
    public JAXBElement<GetConfirmationTemplatesRequest> createGetConfirmationTemplatesRequest(GetConfirmationTemplatesRequest value) {
        return new JAXBElement<GetConfirmationTemplatesRequest>(_GetConfirmationTemplatesRequest_QNAME, GetConfirmationTemplatesRequest.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link GetConfirmationTemplatesRequest }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/ConfirmationsManager", name = "getConfirmationTemplatesRequest", scope = GetConfirmationTemplates.class)
    public JAXBElement<GetConfirmationTemplatesRequest> createGetConfirmationTemplatesGetConfirmationTemplatesRequest(GetConfirmationTemplatesRequest value) {
        return new JAXBElement<GetConfirmationTemplatesRequest>(_GetConfirmationTemplatesGetConfirmationTemplatesRequest_QNAME, GetConfirmationTemplatesRequest.class, GetConfirmationTemplates.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link cnf.integration.GetConfirmationTemplatesResponse }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/ConfirmationsManager", name = "getConfirmationTemplatesResult", scope = cnf.confirmationsmanager.GetConfirmationTemplatesResponse.class)
    public JAXBElement<cnf.integration.GetConfirmationTemplatesResponse> createGetConfirmationTemplatesResponseGetConfirmationTemplatesResult(cnf.integration.GetConfirmationTemplatesResponse value) {
        return new JAXBElement<cnf.integration.GetConfirmationTemplatesResponse>(_GetConfirmationTemplatesResponseGetConfirmationTemplatesResult_QNAME, cnf.integration.GetConfirmationTemplatesResponse.class, cnf.confirmationsmanager.GetConfirmationTemplatesResponse.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link cnf.integration.GetPermissionKeysResponse }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/ConfirmationsManager", name = "getPermissionKeysResult", scope = cnf.confirmationsmanager.GetPermissionKeysResponse.class)
    public JAXBElement<cnf.integration.GetPermissionKeysResponse> createGetPermissionKeysResponseGetPermissionKeysResult(cnf.integration.GetPermissionKeysResponse value) {
        return new JAXBElement<cnf.integration.GetPermissionKeysResponse>(_GetPermissionKeysResponseGetPermissionKeysResult_QNAME, cnf.integration.GetPermissionKeysResponse.class, cnf.confirmationsmanager.GetPermissionKeysResponse.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link cnf.integration.TradeConfirmationStatusChangeResponse }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/ConfirmationsManager", name = "tradeConfirmationStatusChangeResult", scope = cnf.confirmationsmanager.TradeConfirmationStatusChangeResponse.class)
    public JAXBElement<cnf.integration.TradeConfirmationStatusChangeResponse> createTradeConfirmationStatusChangeResponseTradeConfirmationStatusChangeResult(cnf.integration.TradeConfirmationStatusChangeResponse value) {
        return new JAXBElement<cnf.integration.TradeConfirmationStatusChangeResponse>(_TradeConfirmationStatusChangeResponseTradeConfirmationStatusChangeResult_QNAME, cnf.integration.TradeConfirmationStatusChangeResponse.class, cnf.confirmationsmanager.TradeConfirmationStatusChangeResponse.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link GetPermissionKeysRequest }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/ConfirmationsManager", name = "getPermissionKeysRequest", scope = GetPermissionKeys.class)
    public JAXBElement<GetPermissionKeysRequest> createGetPermissionKeysGetPermissionKeysRequest(GetPermissionKeysRequest value) {
        return new JAXBElement<GetPermissionKeysRequest>(_GetPermissionKeysGetPermissionKeysRequest_QNAME, GetPermissionKeysRequest.class, GetPermissionKeys.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link TradeConfirmationStatusChangeRequest }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://cnf/ConfirmationsManager", name = "tradeConfirmationStatusChangeRequest", scope = TradeConfirmationStatusChange.class)
    public JAXBElement<TradeConfirmationStatusChangeRequest> createTradeConfirmationStatusChangeTradeConfirmationStatusChangeRequest(TradeConfirmationStatusChangeRequest value) {
        return new JAXBElement<TradeConfirmationStatusChangeRequest>(_TradeConfirmationStatusChangeTradeConfirmationStatusChangeRequest_QNAME, TradeConfirmationStatusChangeRequest.class, TradeConfirmationStatusChange.class, value);
    }

}
