
package aff.confirm.updateconfirmstatus.mdb.jaxb;

import javax.xml.namespace.QName;
import javax.xml.ws.Service;
import javax.xml.ws.WebEndpoint;
import javax.xml.ws.WebServiceClient;
import javax.xml.ws.WebServiceFeature;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.logging.Logger;


/**
 * This class was generated by the JAX-WS RI.
 * JAX-WS RI 2.1.7-b01-
 * Generated source version: 2.1
 * 
 */
@WebServiceClient(name = "ConfirmationsManager", targetNamespace = "http://cnf/ConfirmationsManager", wsdlLocation = "file:/C:/New%20Projects/Merecuria-Webservice%20JAXB%20code/ConfirmationsManager.wsdl")
public class ConfirmationsManager_Service
    extends Service
{

    private final static URL CONFIRMATIONSMANAGER_WSDL_LOCATION;
    private final static Logger logger = Logger.getLogger(aff.confirm.updateconfirmstatus.mdb.jaxb.ConfirmationsManager_Service.class.getName());

    static {
        URL url = null;
        try {
            URL baseUrl;
            baseUrl = aff.confirm.updateconfirmstatus.mdb.jaxb.ConfirmationsManager_Service.class.getResource(".");
            url = new URL(baseUrl, "http://cnf02inf01:11111/ConfirmationsManager?wsdl");
        } catch (MalformedURLException e) {
            logger.warning("Failed to create URL for the wsdl Location: 'file:/C:/New%20Projects/Merecuria-Webservice%20JAXB%20code/ConfirmationsManager.wsdl', retrying as a local file");
            logger.warning(e.getMessage());
        }
        CONFIRMATIONSMANAGER_WSDL_LOCATION = url;
    }

    public ConfirmationsManager_Service(URL wsdlLocation, QName serviceName) {
        super(wsdlLocation, serviceName);
    }

    public ConfirmationsManager_Service() {
        super(CONFIRMATIONSMANAGER_WSDL_LOCATION, new QName("http://cnf/ConfirmationsManager", "ConfirmationsManager"));
    }

    /**
     * 
     * @return
     *     returns ConfirmationsManager
     */
    @WebEndpoint(name = "BasicHttpBinding_ConfirmationsManager")
    public ConfirmationsManager getBasicHttpBindingConfirmationsManager() {
        return super.getPort(new QName("http://cnf/ConfirmationsManager", "BasicHttpBinding_ConfirmationsManager"), ConfirmationsManager.class);
    }

    /**
     * 
     * @param features
     *     A list of {@link javax.xml.ws.WebServiceFeature} to configure on the proxy.  Supported features not in the <code>features</code> parameter will have their default values.
     * @return
     *     returns ConfirmationsManager
     */
    @WebEndpoint(name = "BasicHttpBinding_ConfirmationsManager")
    public ConfirmationsManager getBasicHttpBindingConfirmationsManager(WebServiceFeature... features) {
        return super.getPort(new QName("http://cnf/ConfirmationsManager", "BasicHttpBinding_ConfirmationsManager"), ConfirmationsManager.class, features);
    }

}
