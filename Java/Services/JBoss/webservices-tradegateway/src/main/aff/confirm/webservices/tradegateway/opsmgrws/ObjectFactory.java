
package aff.confirm.webservices.tradegateway.opsmgrws;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlElementDecl;
import javax.xml.bind.annotation.XmlRegistry;
import javax.xml.namespace.QName;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the tc.opsmgrws package. 
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

    private final static QName _TradeId_QNAME = new QName("http://tc/OpsMgrWS", "tradeId");
    private final static QName _Ticket_QNAME = new QName("http://tc/OpsMgrWS", "ticket");
    private final static QName _GetTradeAlertMessageXMLResponse_QNAME = new QName("http://tc/OpsMgrWS", "getTradeAlertMessageXMLResponse");
    private final static QName _TicketId_QNAME = new QName("http://tc/OpsMgrWS", "ticketId");
    private final static QName _GetTradeAlertOpsTrackXMLResponse_QNAME = new QName("http://tc/OpsMgrWS", "getTradeAlertOpsTrackXMLResponse");
    private final static QName _GetContractFeedXMLResponse_QNAME = new QName("http://tc/OpsMgrWS", "getContractFeedXMLResponse");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: tc.opsmgrws
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Integer }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://tc/OpsMgrWS", name = "tradeId")
    public JAXBElement<Integer> createTradeId(Integer value) {
        return new JAXBElement<Integer>(_TradeId_QNAME, Integer.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Integer }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://tc/OpsMgrWS", name = "ticket")
    public JAXBElement<Integer> createTicket(Integer value) {
        return new JAXBElement<Integer>(_Ticket_QNAME, Integer.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://tc/OpsMgrWS", name = "getTradeAlertMessageXMLResponse")
    public JAXBElement<String> createGetTradeAlertMessageXMLResponse(String value) {
        return new JAXBElement<String>(_GetTradeAlertMessageXMLResponse_QNAME, String.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Integer }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://tc/OpsMgrWS", name = "ticketId")
    public JAXBElement<Integer> createTicketId(Integer value) {
        return new JAXBElement<Integer>(_TicketId_QNAME, Integer.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://tc/OpsMgrWS", name = "getTradeAlertOpsTrackXMLResponse")
    public JAXBElement<String> createGetTradeAlertOpsTrackXMLResponse(String value) {
        return new JAXBElement<String>(_GetTradeAlertOpsTrackXMLResponse_QNAME, String.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://tc/OpsMgrWS", name = "getContractFeedXMLResponse")
    public JAXBElement<String> createGetContractFeedXMLResponse(String value) {
        return new JAXBElement<String>(_GetContractFeedXMLResponse_QNAME, String.class, null, value);
    }

}
