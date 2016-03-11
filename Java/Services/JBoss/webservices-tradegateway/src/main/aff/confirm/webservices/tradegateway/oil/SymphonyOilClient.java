package aff.confirm.webservices.tradegateway.oil;

import aff.confirm.webservices.tradegateway.common.TradeInfo;
import aff.confirm.webservices.tradegateway.data.ContractData;
import aff.confirm.webservices.tradegateway.data.GatewayConfig;
import aff.confirm.webservices.tradegateway.data.TradeAlertData;
import aff.confirm.webservices.tradegateway.data.TradeData;
import aff.confirm.webservices.tradegateway.util.DataConverter;
//import affinity.confirm.tradegateway.oil
import org.apache.axis.message.SOAPHeaderElement;

import javax.xml.soap.SOAPElement;
import javax.xml.soap.SOAPException;
import java.lang.*;
import java.lang.Exception;
import java.net.MalformedURLException;
import java.net.URL;
import org.jboss.logging.Logger;


/**
 * User: ifrankel
 * Date: 8/13/13
 * Time: 12:03 PM
 */
public class SymphonyOilClient implements TradeInfo {
    private static Logger log = Logger.getLogger(SymphonyOilClient.class);
    final String HEADER_ELEMENT_ID = "SSOHeader";
    private GatewayConfig config;
    private int tradeNum = 0;
    private URL uri = null;
    //private OpsMgrWSServiceLocator serviceLocator;
    private String endPointAddress = null;
    private OpsMgrWSServiceSoapBindingStub wsStub;
    private SOAPHeaderElement soapHeader;
    private String wsUserId = null;
    private String retVal = null;


    public SymphonyOilClient() throws Exception {
//        wsUserId = config.getUserId();
//        String webHeaderElement = config.getWebHeaderElement();
//        try {
//            soapHeader = new SOAPHeaderElement(webHeaderElement, HEADER_ELEMENT_ID);
//            soapHeader.setPrefix("");
//            SOAPElement node = soapHeader.addChildElement("Key");
//            node.addTextNode(wsUserId.toLowerCase());
//            wsStub.setHeader(soapHeader);
//
//            serviceLocator = new OpsMgrWSServiceLocator();
//            endPointAddress= System.getProperty("Symphony.OpsMgrWS.Address");
//            serviceLocator.setOpsMgrWSPortEndpointAddress(endPointAddress);
//        } catch (SOAPException e) {
//            throw new Exception(e);
//        }
    }

    private void initRequest(String tradeSystemCode, String pTicket) throws Exception {
        String webHeaderElement;
        OpsMgrWSServiceLocator serviceLocator;
        try {
            serviceLocator = new OpsMgrWSServiceLocator();
            endPointAddress= System.getProperty("Symphony.OpsMgrWS.Address");
            serviceLocator.setOpsMgrWSPortEndpointAddress(endPointAddress);
            uri = new URL(this.config.getWebServiceURL());
//            try {
            wsStub = new OpsMgrWSServiceSoapBindingStub(uri,serviceLocator);
//            } catch (Exception excep) {};

            webHeaderElement = config.getWebHeaderElement();
            soapHeader = new SOAPHeaderElement(webHeaderElement, HEADER_ELEMENT_ID);
            soapHeader.setPrefix("");
            SOAPElement node = soapHeader.addChildElement("Key");
            wsUserId = config.getUserId();
            node.addTextNode(wsUserId.toLowerCase());
            wsStub.setHeader(soapHeader);
            tradeNum = Integer.parseInt(pTicket);
        } catch (Exception e2) {
            log.error("ERROR", e2);
            throw new Exception(e2);
        }
    }

    @Override
    public String getOpsTrackingTrade(String tradeSystemCode, String ticket) throws Exception {
        //Israel 9/16/2013 Replaced TradeAlertData class with String as return object
        //TradeData tradeData = null;
        String alertDataXML = null;
        try {
            initRequest(tradeSystemCode, ticket);
            //Israel 9/16/2013 Replaced TradeAlertData class with String as return object
            //retVal = wsStub.getTradeAlertOpsTrackXML(tradeNum);
            alertDataXML = wsStub.getTradeAlertOpsTrackXML(tradeNum);
            if(alertDataXML.equals((String.valueOf(tradeNum)))){
                throw new Exception("Access Denied. Please consult System Administrator.");
            }
            //Israel 9/16/2013 Replaced TradeAlertData class with String as return object
            //tradeData = DataConverter.convertXMLToObject(TradeData.class, retVal);
        } catch (Exception e) {
            throw new Exception(e);
        }
        return alertDataXML;
    }

    @Override
    public String getContractData(String tradeSystemCode, String ticket) throws Exception {
        ContractData contractData = null;
        String returnXML = null;
        try {
            initRequest(tradeSystemCode, ticket);
            retVal = wsStub.getContractFeedXML(tradeNum);
            if(retVal.equals((String.valueOf(tradeNum)))){
                throw new Exception("Access Denied. Please consult System Administrator.");
            }
            contractData = DataConverter.convertXMLToObject(ContractData.class,retVal);
            returnXML = DataConverter.convertObjectToXML(ContractData.class, contractData);
        } catch (Exception e) {
            throw new Exception(e);
        }
        return returnXML;
    }

    @Override
    public String getTradeAlertMsg(String tradeSystemCode, String ticket) throws Exception {
        // Israel 9/16/2013 - changing return from TradeAlertData to String
        //TradeAlertData alertData = null;
        String alertDataXML = null;
        try {
            initRequest(tradeSystemCode, ticket);
            alertDataXML = wsStub.getTradeAlertMessageXML(tradeNum);
            if(alertDataXML.equals((String.valueOf(tradeNum)))){
                throw new Exception("Access Denied. Please consult System Administrator.");
            }
            //alertData  = DataConverter.convertXMLToObject(TradeAlertData.class, retVal);
        } catch (Exception e) {
            throw new Exception(e);
        }
        return alertDataXML;
    }

    @Override
    public void setConfig(GatewayConfig config) {
        this.config = config;
    }
}
