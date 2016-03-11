package aff.confirm.webservices.tradegateway.ws;

import aff.confirm.webservices.tradegateway.common.TradeInfo;
import aff.confirm.webservices.tradegateway.data.ContractData;
import aff.confirm.webservices.tradegateway.data.GatewayConfig;
import aff.confirm.webservices.tradegateway.data.TradeAlertData;
import aff.confirm.webservices.tradegateway.data.TradeData;
import aff.confirm.webservices.tradegateway.util.DataConverter;
import org.apache.commons.httpclient.methods.StringRequestEntity;
import org.xml.sax.SAXException;

import javax.xml.bind.JAXBException;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.TransformerException;
import java.io.IOException;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 1/16/13
 * Time: 2:33 PM
 */
public class SymphonyTradeInfo implements TradeInfo {
    private  static final String _ALERT_SOAP_ACTION =  "http://tempuri.org/IOpsDataFeed/GetTradeAlertMessage";
    private  static final String _CONTRACT_SOAP_ACTION = "http://tempuri.org/IOpsDataFeed/GetContractData";
    private static final String _OPSTRACKING_SOAP_ACTION = "http://tempuri.org/IOpsDataFeed/GetOpsTrackingTrade";

    GatewayConfig config;
    @Override
    public String getOpsTrackingTrade(String tradeSystemCode, String ticket) throws  Exception{
        //Israel 9/16/2013 Replaced TradeAlertData class with String as return object
        //TradeData tradeData = null;
        String resultXML;

        SoapClient soap = new SoapClient();
        String requestXML = createOpsTrackingRequest(ticket);
        String soapRequest = SoapUtil.makeSoapRequest(requestXML);
        try {
            String soapResponse = soap.postSoapRequest(this.config.getWebServiceURL(),soapRequest,_OPSTRACKING_SOAP_ACTION);
            resultXML = SoapUtil.extractSoapResponse(soapResponse, "GetOpsTrackingTradeResult");
            //Israel 9/16/2013 Replaced TradeAlertData class with String as return object
            //tradeData = DataConverter.convertXMLToObject(TradeData.class,resultXML);
        } catch (IOException e) {
            throw new Exception(e);
        } catch (ParserConfigurationException e) {
            throw new Exception(e);
        } catch (SAXException e) {
            throw new Exception(e);
        } catch (TransformerException e) {
            throw new Exception(e);
        }
        return  resultXML;
    }

    @Override
    public String getContractData(String tradeSystemCode, String ticket) throws Exception {
        ContractData contractData = null;
        String returnXML = null;

        SoapClient soap = new SoapClient();
        String requestXML = createContractRequest(ticket);
        String soapRequest = SoapUtil.makeSoapRequest(requestXML);
        try {
            String soapResponse = soap.postSoapRequest(this.config.getWebServiceURL(),soapRequest,_CONTRACT_SOAP_ACTION);
            String resultXML = SoapUtil.extractSoapResponse(soapResponse,"GetContractDataResult");
            contractData = DataConverter.convertXMLToObject(ContractData.class,resultXML);
            returnXML = DataConverter.convertObjectToXML(ContractData.class,contractData);
        } catch (IOException e) {
            throw new Exception(e);
        } catch (ParserConfigurationException e) {
            throw new Exception(e);
        } catch (SAXException e) {
            throw new Exception(e);
        } catch (TransformerException e) {
            throw new Exception(e);
        } catch (JAXBException e) {
            throw new Exception(e);
        }
        return  returnXML;
    }

    @Override
    public String getTradeAlertMsg(String tradeSystemCode, String ticket) throws Exception {

        TradeAlertData alertData = null;
        SoapClient soap = new SoapClient();
        String requestXML = createTradeAlertRequest( ticket);
        String soapRequest = SoapUtil.makeSoapRequest(requestXML);
        String resultXML;
        try {
            String soapResponse =  soap.postSoapRequest(this.config.getWebServiceURL(),soapRequest,_ALERT_SOAP_ACTION);
            resultXML = SoapUtil.extractSoapResponse(soapResponse, "GetTradeAlertMessageResult");
            //Israel 9/16/2013 Replaced TradeAlertData class with String as return object
            //alertData  = DataConverter.convertXMLToObject(TradeAlertData.class,resultXML);
        } catch (IOException e) {
            throw new Exception(e);
        } catch (ParserConfigurationException e) {
            throw new Exception(e);
        } catch (SAXException e) {
            throw new Exception(e);
        } catch (TransformerException e) {
            throw new Exception(e);
        }
        return  resultXML;

    }

    private String createOpsTrackingRequest(String ticket) {

        String request  = "<GetOpsTrackingTrade xmlns=\"http://tempuri.org/\"><tradeId>" + ticket +"</tradeId></GetOpsTrackingTrade>";
        return request;
    }
    private  String createContractRequest(String ticket) {

        String request  = "<GetContractData xmlns=\"http://tempuri.org/\"><tradeId>" + ticket +"</tradeId></GetContractData>";
        return request;

    }
    private String createTradeAlertRequest(String ticket) {

        String request = "<GetTradeAlertMessage xmlns=\"http://tempuri.org/\"><tradeId>" + ticket +"</tradeId></GetTradeAlertMessage>";
        return  request;

    }

    @Override
    public void setConfig(GatewayConfig config) {
        this.config = config;
    }
}
