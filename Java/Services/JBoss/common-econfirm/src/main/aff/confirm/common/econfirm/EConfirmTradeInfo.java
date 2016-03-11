package aff.confirm.common.econfirm;

import aff.confirm.common.econfirm.datarec.EConfirmTradeInfo_DataRec;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import org.jboss.logging.Logger;
import org.jdom.Element;
import org.jdom.input.SAXBuilder;
import org.w3c.dom.Document;
import org.w3c.dom.NodeList;
import aff.confirm.common.econfirm.datarec.EConfirmTradeInfo_DataRec;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Source;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import javax.xml.transform.stream.StreamSource;
import java.io.InputStream;
import java.io.StringReader;
import java.io.StringWriter;
import java.net.URL;
import java.net.URLConnection;
import java.text.DecimalFormat;

/**
 * Created by ifrankel on 12/8/2014.
 */
public class EConfirmTradeInfo {
    public final static int NO_AGREEMENT = -1;
    public final static int AGREEMENT = 0;
    public final static int CLICK_AND_CONFIRM = 1;
    public final static String WEB_SERVICE = "WebService";
    public final static String METHOD = "Method";
    public final static String ERROR_MESSAGE = "ErrorMessage";
    private final String GET_EC_PROD_ID_METHOD_NAME = "GetEConfirmProductId";
    private final String GET_EC_XML_METHOD_NAME = "GetEConfirmXml";
    private String econfirmInfoServiceUrl = "";

    public static enum SubmitErrorField {
        WEB_SERVICE("WebService"), METHOD("Method"), ERROR_MESSAGE("ErrorMessage");

        private String errorField;

        private SubmitErrorField(String pErrorField) {
            errorField = pErrorField;
        }

        public String getErrorFieldName() {
            return errorField;
        }
    }

    public EConfirmTradeInfo(String pEConfirmInfoServiceUrl) {
        this.econfirmInfoServiceUrl = pEConfirmInfoServiceUrl;
    }

    public static String getAgreementDesc(int pAgreementType){
        String agreementDesc = "NO_AGREEMENT";
        switch (pAgreementType){
            case AGREEMENT: agreementDesc = "AGREMENT"; break;
            case CLICK_AND_CONFIRM: agreementDesc = "CLICK_AND_CONFIRM"; break;
            default: agreementDesc = "NO_AGREEMENT";
        }

        return agreementDesc;
    }

    public EConfirmTradeInfo_DataRec getEConfirmTradeInfo_DataRec(String pTradingSystemCode, double pTradeId) throws StopServiceException {
        EConfirmTradeInfo_DataRec ecTradeInfo_DataRec;
        ecTradeInfo_DataRec = new EConfirmTradeInfo_DataRec();
        String ecInfoUrl = "";
        String textResult = "";
        DecimalFormat df = new DecimalFormat("#0");
        String tradeId = df.format(pTradeId);

        try {
            String.format(ecInfoUrl = econfirmInfoServiceUrl + "/" + GET_EC_PROD_ID_METHOD_NAME + "/" +
                    pTradingSystemCode + "/" + tradeId);

            //String xmlText = getHttpValueResult(ecInfoUrl);
            //textResult = getValueFromXml(xmlText);

            textResult = getHttpResult(ecInfoUrl);


            //textResult = getHttpResult(ecInfoUrl);


            //System.out.println( "Return value from call to " + ecInfoUrl + "=" + textResult );
            //Logger.getLogger(this.getClass()).info("Return value from getEConfirmTradeInfo_DataRec(,) call to " + ecInfoUrl + "=" + textResult);

            String[] textResults = textResult.split(",");
            String prodIdStr = textResults[0];
            String isECBrokerDealStr = textResults[1];
            String isClickAndConfirmDealStr = textResults[2];
            ecTradeInfo_DataRec.tradingSystemCode = pTradingSystemCode;
            ecTradeInfo_DataRec.tradeID = pTradeId;
            ecTradeInfo_DataRec.productID = Integer.parseInt(prodIdStr);
            ecTradeInfo_DataRec.isEConfirmBrokerDeal = (isECBrokerDealStr.contains("Y")) ? true : false;
            ecTradeInfo_DataRec.isClickAndConfirmDeal = (isClickAndConfirmDealStr.contains("Y")) ? true : false;
        } catch (Exception e) {
            Logger.getLogger(this.getClass()).error("Return value from call to " + ecInfoUrl + "=" + textResult);
            throw new StopServiceException(e.getMessage());
        }

        return ecTradeInfo_DataRec;
    }

    public int getEConfirmProductId(String pTradingSystemCode, double pTradeId) throws StopServiceException {
        int eConfirmProductId = 0;
        EConfirmTradeInfo_DataRec ecTradeInfo_DataRec = new EConfirmTradeInfo_DataRec();

        try {
            ecTradeInfo_DataRec = getEConfirmTradeInfo_DataRec(pTradingSystemCode, pTradeId);
            eConfirmProductId = ecTradeInfo_DataRec.productID;

        } catch (Exception e) {
            Logger.getLogger(this.getClass()).error("Return value from getEConfirmProductId: " + eConfirmProductId);
            throw new StopServiceException(e.getMessage());
        }

        return  eConfirmProductId;
    }

    public boolean isEConfirmBrokerDeal(String pTradingSystemCode, double pTradeId) throws StopServiceException {
        boolean isEConfirmBroker = false;
        EConfirmTradeInfo_DataRec ecTradeInfo_DataRec = new EConfirmTradeInfo_DataRec();

        try {
            ecTradeInfo_DataRec = getEConfirmTradeInfo_DataRec(pTradingSystemCode, pTradeId);
            isEConfirmBroker = ecTradeInfo_DataRec.isEConfirmBrokerDeal;

        } catch (Exception e) {
            Logger.getLogger(this.getClass()).error("Return value from isEConfirmBrokerDeal: " + isEConfirmBroker);
            throw new StopServiceException(e.getMessage());
        }

        return  isEConfirmBroker;
    }


    public int getAgreementType(String pTradingSystemCode, double pTradeId) throws StopServiceException {
        int agreementType = NO_AGREEMENT;
        EConfirmTradeInfo_DataRec ecTradeInfo_DataRec = new EConfirmTradeInfo_DataRec();

        try {
            ecTradeInfo_DataRec = getEConfirmTradeInfo_DataRec(pTradingSystemCode, pTradeId);
            if (ecTradeInfo_DataRec.productID > 0){
                if (ecTradeInfo_DataRec.isClickAndConfirmDeal)
                    agreementType = CLICK_AND_CONFIRM;
                else
                    agreementType = AGREEMENT;
            }

        } catch (Exception e) {
            Logger.getLogger(this.getClass()).error("Return value from getAgreementType: TradingSystemCode=" +
                    pTradingSystemCode + ", TradeId=" + pTradeId);
            throw new StopServiceException(e.getMessage());
        }

        return agreementType;
    }

    public String getEConfirmXml(int pProductId, String pTradingSystemCode, double pTradeId) throws StopServiceException {
        String econfirmXml = "";
        String ecInfoUrl = "";
        String textResult = "";
        DecimalFormat df = new DecimalFormat("#0");
        String tradeId = df.format(pTradeId);

        try {
            String.format(ecInfoUrl = econfirmInfoServiceUrl + "/" + GET_EC_XML_METHOD_NAME +
                    "/" + pProductId + "/" + pTradingSystemCode + "/" + tradeId);

            econfirmXml = getHttpResult(ecInfoUrl);

            //Logger.getLogger(this.getClass()).info("Return value from getEConfirmXml(,,) call to " + ecInfoUrl + "=" + econfirmXml);
        } catch (Exception e) {
            Logger.getLogger(this.getClass()).error("Return value from call to " + ecInfoUrl + "=" + textResult);
            throw new StopServiceException(e.getMessage());
        }

        return econfirmXml;
    }

    public boolean isWebServiceSubmitError(String pSubmitXml) throws StopServiceException {
        boolean isSubmitError = false;
        SAXBuilder saxBuilder = new SAXBuilder();

        try {
            org.jdom.Document doc = null;
            doc = saxBuilder.build(new StringReader(pSubmitXml));
            Element rootElem = doc.getRootElement();
            Element tradeInfoElem = rootElem.getChild("Error");

            //If element exists it won't be null.
            if (tradeInfoElem != null)
                isSubmitError = true;
        } catch (Exception e) {
            Logger.getLogger(this.getClass()).error("Return value from isWebServiceSubmitError(pSubmitXml): " + pSubmitXml);
            throw new StopServiceException(e.getMessage());
        }
        return  isSubmitError;
    }

    public String getSubmitErrorValue(String pSubmitXml, SubmitErrorField pSubmitErrorField)
            throws StopServiceException {
        String errorFieldValue = "No Error Message Found";
        SAXBuilder saxBuilder = new SAXBuilder();

        try {
            org.jdom.Document doc = null;
            doc = saxBuilder.build(new StringReader(pSubmitXml));
            Element rootElem = doc.getRootElement();
            Element tradeInfoElem = rootElem.getChild("Error");

            if (tradeInfoElem == null)
                return errorFieldValue;
            else {
                errorFieldValue = tradeInfoElem.getChildText(pSubmitErrorField.getErrorFieldName());
            }
        } catch (Exception e) {
            Logger.getLogger(this.getClass()).error("Return value from isWebServiceSubmitError(pSubmitXml): " + pSubmitXml);
            throw new StopServiceException(e.getMessage());
        }
        return errorFieldValue;
    }

    public String getSubmitErrorRqmtMessage(String pSubmitXml) throws StopServiceException {
        String errorMessage = "";
        errorMessage = "WebService: " + getSubmitErrorValue(pSubmitXml, SubmitErrorField.WEB_SERVICE) + "; " +
                "Method: " + getSubmitErrorValue(pSubmitXml, SubmitErrorField.METHOD) + "; " +
                "ErrorMessage: " + getSubmitErrorValue(pSubmitXml, SubmitErrorField.ERROR_MESSAGE);
        return errorMessage;
    }


    private String getHttpResult(String pUri) throws Exception {
        String textResult = "";
        URL url = new URL(pUri);
        URLConnection connection = url.openConnection();
        Document doc = parseXML(connection.getInputStream());

        Transformer transformer = TransformerFactory.newInstance().newTransformer();
        StreamResult result = new StreamResult(new StringWriter());
        DOMSource source = new DOMSource(doc);
        transformer.transform(source, result);

        //Strips off enclosing <string>xxxx</string> tag added by server.
        //Final textResult may plain text or an fully-formed xml string depending on call.
        NodeList nodes = doc.getElementsByTagName("string");
        textResult = nodes.item(0).getTextContent();

        //xmlResult = StringEscapeUtils.unescapeXml(xmlResult);
        return textResult;
    }

    private Document parseXML(InputStream stream) throws Exception
    {
        DocumentBuilderFactory objDocumentBuilderFactory = null;
        DocumentBuilder objDocumentBuilder = null;
        Document doc = null;
        try
        {
            objDocumentBuilderFactory = DocumentBuilderFactory.newInstance();
            objDocumentBuilder = objDocumentBuilderFactory.newDocumentBuilder();
            doc = objDocumentBuilder.parse(stream);
        }
        catch(Exception ex)
        {
            throw ex;
        }

        return doc;
    }

    private String getFormattedXml(String pInput, int pIndent){
        try
        {
            Source xmlInput = new StreamSource(new StringReader(pInput));
            StringWriter stringWriter = new StringWriter();
            StreamResult xmlOutput = new StreamResult(stringWriter);
            TransformerFactory transformerFactory = TransformerFactory.newInstance();
            Transformer transformer = transformerFactory.newTransformer();
            transformer.setOutputProperty(OutputKeys.INDENT, "yes");
            transformer.setOutputProperty("{http://xml.apache.org/xslt}indent-amount", String.valueOf(pIndent));
            transformer.transform(xmlInput, xmlOutput);
            return xmlOutput.getWriter().toString();
        }
        catch(Throwable t)
        {
            return pInput;
        }
    }

/*
    private String getHttpResult(String pUri) throws Exception {
        String textValue = "";
        URL url = new URL(pUri);
        HttpURLConnection connection = (HttpURLConnection) url.openConnection();
        connection.setRequestMethod("GET");
        connection.setRequestProperty("Accept", "application/xml");

        BufferedReader br = new BufferedReader(new InputStreamReader((connection.getInputStream())));
        String output;
        String xmlText = "";
        while ((output = br.readLine()) != null) {
            xmlText += output;
        }
        connection.disconnect();

        textValue = getValueFromXml(xmlText);
        return textValue;
    }

    //EConfirmTradeInfo Web service wraps all results in a <string>xxxx</string> xml.
    //This function unwraps them--as either plain text or as an inner, fully formed xml.
    private String getValueFromXml(String pXmlText) throws Exception {
        String textResult = null;
        InputSource inputSrc = new InputSource();
        inputSrc.setCharacterStream(new StringReader(pXmlText));
        DOMParser parser = new DOMParser();
        parser.parse(inputSrc);
        Document doc = parser.getDocument();

        NodeList nodes = doc.getElementsByTagName("string");
        textResult = nodes.item(0).getTextContent();
        return textResult;
    }
*/


}
