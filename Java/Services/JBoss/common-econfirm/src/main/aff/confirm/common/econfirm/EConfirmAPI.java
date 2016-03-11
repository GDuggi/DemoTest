package aff.confirm.common.econfirm;

//import com.sun.deploy.net.URLEncoder;

import aff.confirm.common.econfirm.exceptions.AuthenticateFailedException;
import aff.confirm.common.econfirm.exceptions.SECptyNotFoundException;
import aff.confirm.common.util.FileStore;
import org.jboss.logging.Logger;
import org.jdom.Document;
import org.jdom.Element;
import org.jdom.JDOMException;
import org.jdom.input.SAXBuilder;

import java.io.*;
import java.net.*;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.text.ParseException;

/**
 * Created by ifrankel on 12/11/2014.
 */
public class EConfirmAPI {
    public static final String EC_AUTHENTICATE_TYPE_1 = "Authenticate[Type 1]";
    public static final String EC_SUBMIT_TYPE_2 = "Submit[Type 2]";
    public static final String EC_REQ_TYPE_AUTHENTICATE_1 = "1";
    public static final String EC_REQ_TYPE_SUBMIT_TRADE_2 = "2";
    public static final String EC_REQ_TYPE_MESSAGE_LOG_QUERY_4 = "4";
    public static final String EC_REQ_TYPE_STATUS_QUERY_8 = "8";
    public static final String EC_REQ_TYPE_BROKEN_FIELDS_QUERY_9 = "9";
    public static final String EC_REQ_TYPE_BROKER_BROKEN_FIELDS_QUERY_10 = "10";
    public static final String EC_REQ_TYPE_BROKER_STATUS_QUERY_17 = "17";
    public static final String EC_REQ_TYPE_ALLEGED_TRADES_QUERY_27 = "27";

    private final String PREFIX_SUBMIT = "SUBM";
    private final String PREFIX_CANCEL = "CANC";
    private final String PREFIX_STATUS = "STATUS_RQST";
    private final String SUBMIT_RESPONSE = "RESP";
    private final String STATUS_RESPONSE = "STATUS_RESP";
    private final String PREFIX_MESSAGE_LOG = "MLOG_RQST";
    private final String PREFIX_BROKEN_FIELD = "BKFLD";
    private final String MLOG_RESPONSE = "MLOG_RESP";
    private final String NO_BROKER = "No Broker";

    private String cookie = "";
    private String proxyType = "";
    private String proxyUrl = "";
    private int proxyPort = 0;
    private String eConfirmAPIUrl = "";
    private String eConfirmInfoServiceUrl = "";
    private FileStore fileStore;
    private EConfirmTradeInfo eConfirmTradeInfo;

    public EConfirmAPI(String pEConfirmAPIUrl, String pEConfirmInfoServiceUrl, String pProxyType, String pProxyUrl, int pProxyPort,
                       String pFileStoreDir, String pServiceName, int pFileStoreExpireDays) throws Exception {
        this.eConfirmAPIUrl = pEConfirmAPIUrl;
        this.eConfirmInfoServiceUrl = pEConfirmInfoServiceUrl;
        this.proxyType = pProxyType;
        this.proxyUrl = pProxyUrl;
        this.proxyPort = pProxyPort;
        fileStore = new FileStore(pFileStoreDir, pServiceName, pFileStoreExpireDays);
        eConfirmTradeInfo = new EConfirmTradeInfo(eConfirmInfoServiceUrl);
    }

    public boolean isSuccess(String pResponseXML, String pMessageLabel)
            throws IOException, JDOMException {
        SAXBuilder saxBuilder = new SAXBuilder();
        String status = null;
        Document doc = null;
        doc = saxBuilder.build(new StringReader(pResponseXML));
        Element rootElem = doc.getRootElement();
        Element statusElem = rootElem.getChild("Status");

        status = statusElem.getText();
        if (!"SUCCESS".equalsIgnoreCase(status)) {
            Element messageElem = rootElem.getChild("Message");
            String code = messageElem.getChildText("Code");
            String type = messageElem.getChildText("Type");
            String description = messageElem.getChildText("Description");
            String message = "isSuccess(" + pMessageLabel + "): FAILURE: Code=" + code +
                    ", Type=" + type + ", Description=" + description;
            //System.out.println(message);
            Logger.getLogger(this.getClass()).warn(message);
            return false;
        }
        return "SUCCESS".equalsIgnoreCase(status);
    }

    //RequestType 1
    public String authenticate(String pEConfirmUserName, String pEConfirmPassword)
            throws IOException, SECptyNotFoundException, AuthenticateFailedException {
        if (pEConfirmUserName.equalsIgnoreCase("") ||
                pEConfirmPassword.equalsIgnoreCase(""))
            throw new SECptyNotFoundException("EConfirmAPI.authenticate(): eConfirmUserName or eConfirmPassword are not set.");

        String authRequestXML = null;
        String responseXML = null;
        authRequestXML = getAuthRequestXML(pEConfirmUserName, pEConfirmPassword);

        try {
            //Thread.sleep(2000);
            responseXML = sendToEConfirm(EC_REQ_TYPE_AUTHENTICATE_1, authRequestXML, false);
            //if (responseXML != null)
            //  throw new Exception("simulated error...");
        } catch (Exception e) {
            throw new AuthenticateFailedException(e.toString() + ", Submission failed: \nRequest XML=" + authRequestXML +
                    " \nResponse XML=" + responseXML);
        }
        return responseXML;
    }

    //RequestType 2
    public String submitToEConfirm(String pRequestType, int pProductID, String pTradingSystemCode, double pTradeID,
                                   String pEConfirmUserName, String pEConfirmPassword)
            throws Exception, IOException, JDOMException, SECptyNotFoundException, ParseException, SQLException {
        String authResponseXML = "";
        String submitResponseXML = "";
        String submitXML = null;
        boolean success = false;
        DecimalFormat df = new DecimalFormat("#0");
        String tradeId = df.format(pTradeID);

        authResponseXML = authenticate(pEConfirmUserName, pEConfirmPassword);
        success = isSuccess(authResponseXML, EC_AUTHENTICATE_TYPE_1);
        if (success != true)
            throw new AuthenticateFailedException("Occurred in: EConfirmAPI.submitToEConfirm()");

        if (pRequestType.equalsIgnoreCase(EC_REQ_TYPE_SUBMIT_TRADE_2))
            submitXML = eConfirmTradeInfo.getEConfirmXml(pProductID, pTradingSystemCode, pTradeID);
        else
            throw new Exception("pRequestType="+ pRequestType + " is not currently supported.");

        Logger.getLogger(this.getClass() ).info("submitXML=" + submitXML);

        if(submitXML != null){
            String timeStamp = fileStore.getTimeStamp();
            String fileName = tradeId + "_" + timeStamp + PREFIX_SUBMIT + ".xml";
            fileStore.storeAsTextFile(fileName, submitXML);
            submitResponseXML = sendToEConfirm(pRequestType, submitXML, true);
            fileName = tradeId + "_" + timeStamp + SUBMIT_RESPONSE + ".xml";
            fileStore.storeAsTextFile(fileName, submitResponseXML);
        }
        else {
            Logger.getLogger(this.getClass()).info("pProductID="+pProductID + ", pTradingSystemCode="+pTradingSystemCode +
                ", pTradeID="+pTradeID + ": Returned a null Request Xml");
            submitResponseXML = "NO_SUBMIT_XML";
        }

        return submitResponseXML;
    }

    //RequestType 4
    public String queryMessageLog(String pStartDate, String pEndDate, String pEConfirmUserName, String pEConfirmPassword)
            throws AuthenticateFailedException, IOException, JDOMException, SECptyNotFoundException, ParseException {
        String authResponseXML = "";
        String queryXMLRequest = "";
        String queryXMLResponse = "";
        boolean success = false;

        authResponseXML = authenticate(pEConfirmUserName, pEConfirmPassword);
        success = isSuccess(authResponseXML, EC_AUTHENTICATE_TYPE_1);
        if (success != true)
            throw new AuthenticateFailedException("Occurred in: EConfirmAPI.queryMessageLog(pStartDate,pEndDate)");

        queryXMLRequest = getMessageLoqQueryXML(pStartDate, pEndDate);
        String timeStamp = fileStore.getTimeStamp();
        String fileName = timeStamp + "_" + PREFIX_MESSAGE_LOG + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLRequest);

        queryXMLResponse = sendToEConfirm(EC_REQ_TYPE_MESSAGE_LOG_QUERY_4, queryXMLRequest, true);
        fileName = timeStamp + "_" + MLOG_RESPONSE + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLResponse);

        return queryXMLResponse;
    }

    //RequestType 4
    public String queryMessageLog(String[] args, String pEConfirmUserName, String pEConfirmPassword)
            throws AuthenticateFailedException, IOException, JDOMException, SECptyNotFoundException, ParseException {
        String authResponseXML = "";
        String queryMessageLogXMLRequest = "";
        String queryMessageLogXMLResponse = "";
        boolean success = false;

        authResponseXML = authenticate(pEConfirmUserName, pEConfirmPassword);
        success = isSuccess(authResponseXML, EC_AUTHENTICATE_TYPE_1);
        if (success != true)
            throw new AuthenticateFailedException("Occurred in: EConfirmAPI.queryMessageLog(args)");

        queryMessageLogXMLRequest = getIdListQueryXML("EConfirmMessageLogQueryRequest", args);

        String timeStamp = fileStore.getTimeStamp();
        String fileName = timeStamp + "_" + PREFIX_MESSAGE_LOG + ".xml";
        fileStore.storeAsTextFile(fileName, queryMessageLogXMLRequest);

        queryMessageLogXMLResponse = sendToEConfirm(EC_REQ_TYPE_MESSAGE_LOG_QUERY_4, queryMessageLogXMLRequest, true);
        fileName = timeStamp + "_" + MLOG_RESPONSE + ".xml";
        fileStore.storeAsTextFile(fileName, queryMessageLogXMLResponse);

        return queryMessageLogXMLResponse;
    }

    //RequestType 5
    public String cancelEConfirmTrade(String pBatchId, String pTradeRefId, String pECUserId, String pECPassword)
            throws AuthenticateFailedException, IOException, JDOMException, SECptyNotFoundException,
            ParseException, SQLException {
        String authResponseXML = "";
        String queryXMLRequest = "";
        String queryXMLResponse = "";
        boolean success = false;

        authResponseXML = authenticate(pECUserId, pECPassword);
        success = isSuccess(authResponseXML, EC_AUTHENTICATE_TYPE_1);
        if (success != true)
            throw new AuthenticateFailedException("Occurred in: EConfirmAPI.cancelTrade()");

        //double tradeId = Double.parseDouble(pTradeId);
        queryXMLRequest = getCancelTradeXML(pBatchId, pTradeRefId);

        String timeStamp = fileStore.getTimeStamp();
        String fileName = pTradeRefId + "_" + timeStamp + PREFIX_SUBMIT + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLRequest);

        queryXMLResponse = sendToEConfirm("5", queryXMLRequest, true);

        fileName = pTradeRefId + "_" + timeStamp + SUBMIT_RESPONSE + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLResponse);

        return queryXMLResponse;
    }

    //RequestType 8
    public String queryStatus(String pDateTimeStamp, String pECUserId, String pECPassword)
            throws AuthenticateFailedException, IOException, JDOMException, SECptyNotFoundException, ParseException {
        String authResponseXML = "";
        String queryXMLRequest = "";
        String queryXMLResponse = "";
        boolean success = false;

        authResponseXML = authenticate(pECUserId, pECPassword);
        success = isSuccess(authResponseXML, EC_AUTHENTICATE_TYPE_1);
        if (success != true)
            throw new AuthenticateFailedException("Occurred in: EConfirmAPI.queryStatus(): "+authResponseXML);

        queryXMLRequest = getQueryStatusXML(pDateTimeStamp);
        String timeStamp = fileStore.getTimeStamp();
        String fileName = timeStamp + "_" + PREFIX_STATUS + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLRequest);

        queryXMLResponse = sendToEConfirm(EC_REQ_TYPE_STATUS_QUERY_8, queryXMLRequest, true);
        fileName = timeStamp + "_" + STATUS_RESPONSE + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLResponse);
        return queryXMLResponse;
    }

    //RequestType 9
    public String queryBrokenFields(String pTradeID, String pECUserId, String pECPassword)
            throws AuthenticateFailedException, IOException, JDOMException, SECptyNotFoundException, ParseException {
        String authResponseXML = "";
        String queryXMLRequest = "";
        String queryXMLResponse = "";
        boolean success = false;

        authResponseXML = authenticate(pECUserId, pECPassword);
        success = isSuccess(authResponseXML, EC_AUTHENTICATE_TYPE_1);
        if (success != true)
            throw new AuthenticateFailedException("Occurred in: EConfirmAPI.queryBkrBrokenFields()");

        queryXMLRequest = getQueryBrokenFieldsXML(pTradeID);
        String timeStamp = fileStore.getTimeStamp();
        String fileName = pTradeID + "_" + timeStamp + "_" + PREFIX_BROKEN_FIELD + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLRequest);

        queryXMLResponse = sendToEConfirm(EC_REQ_TYPE_BROKEN_FIELDS_QUERY_9, queryXMLRequest, true);
        fileName = pTradeID + "_" + timeStamp + "_" + STATUS_RESPONSE + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLResponse);

        return queryXMLResponse;
    }

    //RequestType 10
    public String queryBkrBrokenFields(String pTradeID, String pECUserId, String pECPassword)
            throws AuthenticateFailedException, IOException, JDOMException, SECptyNotFoundException, ParseException {
        String authResponseXML = "";
        String queryXMLRequest = "";
        String queryXMLResponse = "";
        boolean success = false;

        authResponseXML = authenticate(pECUserId, pECPassword);
        success = isSuccess(authResponseXML, EC_AUTHENTICATE_TYPE_1);
        if (success != true)
            throw new AuthenticateFailedException("Occurred in: EConfirmProcessor.queryBkrBrokenFields()");

        queryXMLRequest = getQueryBkrBrokenFieldsXML(pTradeID);
        String timeStamp = fileStore.getTimeStamp();
        String fileName = pTradeID + "_" + timeStamp + "_" + PREFIX_BROKEN_FIELD + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLRequest);

        queryXMLResponse = sendToEConfirm(EC_REQ_TYPE_BROKER_BROKEN_FIELDS_QUERY_10, queryXMLRequest, true);
        fileName = pTradeID + "_" + timeStamp + "_" + STATUS_RESPONSE + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLResponse);

        return queryXMLResponse;
    }



    //RequestType 17
    public String queryBkrStatus(String pDateTimeStamp, String pECUserId, String pECPassword)
            throws AuthenticateFailedException, IOException, JDOMException, SECptyNotFoundException, ParseException {
        String authResponseXML = "";
        String queryXMLRequest = "";
        String queryXMLResponse = "";
        boolean success = false;

        authResponseXML = authenticate(pECUserId, pECPassword);
        success = isSuccess(authResponseXML, EC_AUTHENTICATE_TYPE_1);
        if (success != true)
            throw new AuthenticateFailedException("Occurred in: EConfirmAPI.queryStatus(): "+authResponseXML);

        queryXMLRequest = getQueryBkrStatusXML(pDateTimeStamp);
        String timeStamp = fileStore.getTimeStamp();
        String fileName = timeStamp + "_" + PREFIX_STATUS + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLRequest);

        queryXMLResponse = sendToEConfirm(EC_REQ_TYPE_BROKER_STATUS_QUERY_17, queryXMLRequest, true);
        fileName = timeStamp + "_" + STATUS_RESPONSE + ".xml";
        fileStore.storeAsTextFile(fileName, queryXMLResponse);
        return queryXMLResponse;
    }

    //RequestType 27
    public String queryAllegedTrades(String pStartDate, String pEndDate, String pECUserId, String pECPassword)
            throws AuthenticateFailedException, IOException, JDOMException, SECptyNotFoundException{
        String authResponseXML = "";
        String queryXMLRequest = "";
        String queryXMLResponse = "";
        boolean success = false;

        authResponseXML = authenticate(pECUserId, pECPassword);
        success = isSuccess(authResponseXML, EC_AUTHENTICATE_TYPE_1);
        if (success != true)
            throw new AuthenticateFailedException("Occurred in: EConfirmAPI.queryDetailedAllegedTrades()");

        queryXMLRequest = getQueryAllegedTradesXML(pStartDate, pEndDate);
        queryXMLResponse = sendToEConfirm(EC_REQ_TYPE_ALLEGED_TRADES_QUERY_27, queryXMLRequest, true);
        return queryXMLResponse;
    }


    /* Determines if a call is a success.
        Instead of returning a boolean like isSuccess,
        returns a strings stating SUCCESS or failure message.*/
    public String getStatusMessage(String pResponseXML, String pMessageLabel)
            throws IOException, JDOMException {
        SAXBuilder saxBuilder = new SAXBuilder();
        String status = null;
        Document doc = null;
        doc = saxBuilder.build(new StringReader(pResponseXML));
        Element rootElem = doc.getRootElement();
        Element statusElem = rootElem.getChild("Status");
        status = statusElem.getText();
        if (!"SUCCESS".equalsIgnoreCase(status)) {
            Element messageElem = rootElem.getChild("Message");
            String code = messageElem.getChildText("Code");
            String type = messageElem.getChildText("Type");
            String description = messageElem.getChildText("Description");
            String message = pMessageLabel + ": FAILURE: Code=" + code +
                    ", Type=" + type + ", Description=" + description;
            return message;
        }
        return "SUCCESS";
    }

    public String getTraceID(String pResponseXML) throws IOException, JDOMException {
        SAXBuilder saxBuilder = new SAXBuilder();
        String traceID = null;
        Document doc = null;
        doc = saxBuilder.build(new StringReader(pResponseXML));
        Element rootElem = doc.getRootElement();
        Element traceIDElem = rootElem.getChild("TraceId");
        traceID = traceIDElem.getText();
        return traceID;
    }

    private String getAuthRequestXML(String pEConfirmUserName, String pEConfirmPassword) {
        String authRequestXML = null;
        authRequestXML =
                "<?xml version=\"1.0\"?>\r" +
                        "<AuthRequest>\r" +
                        "  <Name>" + pEConfirmUserName + "</Name>\r" +
                        "  <Password>" + pEConfirmPassword + "</Password>\r" +
                        "</AuthRequest>";
        return authRequestXML;
    }

    private String getCancelTradeXML(String pBatchId, String pTradeRefId) throws SQLException, ParseException {
        String requestXML = null;
        requestXML =
                "<?xml version=\"1.0\"?>" +
                        "<EConfirmRequest BatchId=\"" + pBatchId + "\">" +
                        "  <EConfirmTrade>" +
                        "    <Action>CANCEL</Action>" +
                        "    <SenderTradeRefId>" + pTradeRefId + "</SenderTradeRefId>" +
                        "  </EConfirmTrade>" +
                        "</EConfirmRequest>";
        return requestXML;
    }

    private String getQueryStatusXML(String pDateTimeStamp) {
        String requestXML = null;
        requestXML =
                "<?xml version=\"1.0\"?>" +
                        "<EConfirmStatusQueryRequest>" +
                        "  <StatusDate>" + pDateTimeStamp + "</StatusDate>" +
                        "</EConfirmStatusQueryRequest>";
        return requestXML;
    }

    private String getQueryBkrStatusXML(String pDateTimeStamp) {
        String requestXML = null;
        requestXML =
                "<?xml version=\"1.0\"?>" +
                        "<EConfirmBrokerStatusQueryRequest>" +
                        "  <StatusDate>" + pDateTimeStamp + "</StatusDate>" +
                        "</EConfirmBrokerStatusQueryRequest>";
        return requestXML;
    }

    private String getQueryAllegedTradesXML(String pStartDate, String pEndDate) {
        String requestXML = null;
        requestXML =
                "<?xml version=\"1.0\"?>" +
                        "<EConfirmDetailedAllegedQueryRequest>" +
                        "  <DateRange>" +
                        "    <Type>TradeDate</Type>" +
                        "    <StartDate>" + pStartDate + "</StartDate>" +
                        "    <EndDate>" + pEndDate + "</EndDate>" +
                        "  </DateRange>" +
                        "</EConfirmDetailedAllegedQueryRequest>";
        return requestXML;
    }

    private String getMessageLoqQueryXML(String pStartDate, String pEndDate) {
        String requestXML = null;
        requestXML =
                "<?xml version=\"1.0\"?>\r" +
                        "<EConfirmMessageLogQueryRequest>\r" +
                        "  <CompoundQuery>\r" +
                        "     <StartDate>" + pStartDate + "</StartDate>\r" +
                        "     <EndDate>" + pEndDate + "</EndDate>\r" +
                        "  </CompoundQuery>\r" +
                        "</EConfirmMessageLogQueryRequest>";
        return requestXML;
    }

    private String getQueryBrokenFieldsXML(String pTradeRefID) {
        String requestXML = null;
        requestXML =
                "<?xml version=\"1.0\"?>" +
                        "<EConfirmBreakFieldQueryRequest>" +
                        "  <Ids>" +
                        "    <SenderTradeRefId>" + pTradeRefID + "</SenderTradeRefId>" +
                        "  </Ids>" +
                        "</EConfirmBreakFieldQueryRequest>";
        return requestXML;
    }

    private String getQueryBkrBrokenFieldsXML(String pTradeRefID) {
        String requestXML = null;
        requestXML =
                "<?xml version=\"1.0\"?>" +
                        "<EConfirmBrokerBreakFieldQueryRequest>" +
                        "  <Ids>" +
                        "    <SenderTradeRefId>" + pTradeRefID + "</SenderTradeRefId>" +
                        "  </Ids>" +
                        "</EConfirmBrokerBreakFieldQueryRequest>";
        return requestXML;
    }

    private String getIdListQueryXML(String pMainTag, String[ ] args) {
        String requestXML = null;
        String tradeRefIDs = "";
        requestXML =
                "<?xml version=\"1.0\"?>\r" +
                        "<" + pMainTag + ">\r" +
                        "  <Ids>\r";

        int count;
        for (count=0;count<args.length;count++)
            tradeRefIDs = tradeRefIDs +
                    "<SenderTradeRefId>" + args[count] + "</SenderTradeRefId>\r";

        requestXML = requestXML + tradeRefIDs +
                "  </Ids>\r" +
                "</" + pMainTag + ">";
        return requestXML;
    }


    public String getStatusMessageFromMessageLogQueryResult(String pResultXML)
            throws JDOMException, IOException {
        SAXBuilder saxBuilder = new SAXBuilder();
        String status = null;
        Document doc = null;
        doc = saxBuilder.build(new StringReader(pResultXML));
        Element rootElem = doc.getRootElement();
        Element messageLogInfoElem = rootElem.getChild("EConfirmMessageLogInfo");
        Element simpleResponseElem = messageLogInfoElem.getChild("EConfirmSimpleResponse");
        Element messageElem = simpleResponseElem.getChild("Message");
        String code = messageElem.getChildText("Code");
        String type = messageElem.getChildText("Type");
        String description = messageElem.getChildText("Description");
        String message = "Code=" + code +
                ", Type=" + type + ", Description=" + description;

        return message;
    }

    private Proxy.Type convertProxyType(){
        Proxy.Type type = Proxy.Type.valueOf(proxyType);
        return type;
    }

    private String sendToEConfirm(String pRequestType, String pRequestValue, boolean pHasCookie) throws IOException {
        String result = "";
        URL url = null;
        HttpURLConnection urlc = null;

        Proxy.Type proxyType = convertProxyType();
        InetSocketAddress proxyAddress = null;
        Proxy proxy = null;
        try {
            url = new URL(eConfirmAPIUrl);
            if (proxyType  == Proxy.Type.DIRECT){
                urlc = (HttpURLConnection) url.openConnection();
                //Logger.getLogger(this.getClass()).info("Connected without Proxy") ;
            }
            else {
                proxyAddress=new InetSocketAddress(proxyUrl, proxyPort);
                proxy =new Proxy(proxyType,proxyAddress);
                urlc = (HttpURLConnection) url.openConnection(proxy);
                //Logger.getLogger(this.getClass()).info("Connected thru Proxy => " + "Proxy Type  " + proxyType + "; URL = " + proxyUrl + "; Port " + proxyPort) ;
            }
            urlc.setRequestMethod("POST");
            urlc.setUseCaches(false);
            urlc.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");

            //For all types higher than 1
            if (pHasCookie == true) {
                urlc.setRequestProperty("Cookie", cookie);
            }

            String reqVal = null;
            //reqVal = URLEncoder.encode(pRequestValue);
            reqVal = URLEncoder.encode(pRequestValue, "UTF-8" );

            String stContent = "requestType=" + pRequestType + "&requestValue=" + reqVal ;
            byte[] arbContent = stContent.getBytes();
            urlc.setDoOutput(true);
            urlc.setDoInput(true);
            BufferedOutputStream os = null;
            os = new BufferedOutputStream(urlc.getOutputStream());
            os.write(arbContent, 0, arbContent.length);
            os.flush();
            os.close();
            os = null;

            //Logger.getLogger(EConfirm.class).info("os closed ");
            // init cookie - For type 1
            if (pHasCookie == false) {
                String tempCookie = null;
                tempCookie = urlc.getHeaderField("Set-Cookie");
                if (tempCookie != null) {
                    cookie = tempCookie;
                }
            }

            InputStream inputStream = null;
            try {
                inputStream = urlc.getInputStream();
            } catch (IOException e) {
                Logger.getLogger(this.getClass()).info("Error in Get Input Stream!!  Last HTTP Response Code: " + urlc.getResponseCode());
                urlc.disconnect();
                urlc = null;
                throw e;
            }

            BufferedReader in = new BufferedReader(new InputStreamReader(inputStream));
            String str = "";
            StringBuffer sbResult = new StringBuffer();
            while (str != null) {
                str = in.readLine();
                if (str != null)
                    sbResult.append(str + "\r");
            }
            //urlc.getResponseCode();
            in.close();
            in = null;
            result = sbResult.toString();
        }
        finally{
            if (urlc != null){
                //Logger.getLogger(this.getClass()).info("Last HTTP Response Code: " + urlc.getResponseCode());
                urlc.disconnect();
                urlc = null;
            }
        }
        return result;
    }
}
