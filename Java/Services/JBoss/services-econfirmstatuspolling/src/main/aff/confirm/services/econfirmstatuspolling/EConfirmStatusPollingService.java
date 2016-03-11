/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:30:52 AM
 * To change template for new class use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.econfirmstatuspolling;

import aff.confirm.common.econfirm.EConfirmData;
import aff.confirm.common.econfirm.datarec.ECIgnoredStatusMessage_Rec;
import aff.confirm.common.econfirm.datarec.EConfirmMessageLog_DataRec;
import aff.confirm.common.econfirm.datarec.EConfirmSummary_DataRec;
import aff.confirm.common.econfirm.exceptions.AuthenticateFailedException;
import aff.confirm.common.econfirm.exceptions.SECptyNotFoundException;
import org.jboss.logging.Logger;
import org.jdom.Document;
import org.jdom.Element;
import org.jdom.JDOMException;
import org.jdom.input.SAXBuilder;
import org.apache.xalan.processor.TransformerFactoryImpl;
import aff.confirm.common.econfirm.*;
//import aff.confirm.common.econfirm_v1.exceptions.AuthenticateFailedException;
//import aff.confirm.common.econfirm_v1.exceptions.SECptyNotFoundException;
import aff.confirm.common.util.MailUtils;
import aff.confirm.common.dao.AppControlDAO;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskService;
import aff.confirm.jboss.common.util.DbInfoWrapper;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;
import javax.naming.NamingException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerException;
import javax.xml.transform.stream.StreamSource;
import javax.xml.transform.stream.StreamResult;
import javax.mail.MessagingException;
import java.io.*;
import java.net.InetAddress;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.*;

@Startup
@Singleton
public class EConfirmStatusPollingService extends TaskService implements EConfirmStatusPollingServiceMBean {
    private final String SERVICE_NAME = "StatusPolling";
    //private String dBInfoName;
    private String eConfirmAPIUrl;
    private String eConfirmTradeInfoServiceUrl;
    //private boolean paused;
    //private int scanTime = 5999;
    //private Timer timer;
    //private Connection connection;
    //private final SimpleDateFormat sdfmm_dd_yyyyDate = new SimpleDateFormat("MM-dd-yyyy", Locale.US);
    //private final SimpleDateFormat sdfSybaseDate = new SimpleDateFormat("MM/dd/yyyy", Locale.US);
    //"2003-12-10 19:12:18";
    private static final SimpleDateFormat sdfDateTime = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.US);
    private final String END_QUERY_DATE = "2199-12-31";
    private final String UNKNOWN_UNMATCHED = "UnknownUnmatched";
    private final String NO_BROKER = "*NONE";
    private final String NO_DATA = "*NONE";
    private final String EC_ALLEGED_EMAIL = "ECALGDMAIL";
    private final String JPM_USER = "jpmpoll";
    private final int SE_CPTY_SN = 0;
    private final int CDTY_CODE = 1;
    private DecimalFormat df = new DecimalFormat("#0");
    //private EConfirmProcessor eConfirmProcessor;
    //private EConfirmDAO eConfirmDAO;
    private EConfirmAPI eConfirmAPI;
    private EConfirmData eConfirmData;
    private EConfirmSummary_DataRec eConfirmSummaryDataRec;
    private EConfirmMessageLog_DataRec ecMesssageLogRec;
    private String opsTrackingDBInfoName;
    private String affinityDBInfoName;
    //private String symphonyDBInfoName;
    private String opsTrackingDBInfoDisplayName;
    private String affinityDBInfoDisplayName;
    //private String symphonyDBInfoDisplayName;
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    //private java.sql.Connection symphonyConnection;
    //private ArrayList seCptyList;
    private String smtpHost;
    private String smtpPort;
//    private String sendToAddress;
//    private String sendToName;
//    private String elecUSSentFromName;
//    private String ngasUSSentFromName;
//    private String oilSentFromName;
//    private String elecUKSentFromName;
//    private String ngasUKSentFromName;
//    private String ngasSGEFromName;
//    private String elecSGEFromName;
//    private String oilSGEFromName;
//    private String elecUSSendToAddress;
//    private String ngasUSSendToAddress;
//    private String oilSendToAddress;
//    private String elecUKSendToAddress;
//    private String ngasUKSendToAddress;
//    private String ngasSendToSGEAddress;
//    private String elecSendToSGEAddress;
//    private String oilSendToSGEAddress;
//    private String elecUSSendToName;
//    private String ngasUSSendToName;
//    private String oilSendToName;
//    private String elecUKSendToName;
//    private String ngasUKSendToName;
//    private String ngasSendToSGEName;
//    private String elecSendToSGEName;
//    private String oilSendToSGEName;
    private String sendToStatus;
    private String sendToMessageLog;
    private String sentFromAddress;
    private String sentFromName;
    private String confirmSupportSendToAddress;
    private String allegedEMailSendToAddress;
    private MailUtils mailUtils;
    private String fileStoreDir;
    private int fileStoreExpireDays;
    private String allegedQueryXSL;
    private String allegedQueryHTML;
    private String allegedQueryStartDate;
    private String allegedEMailSendAtHourGMT;
    private int statusRetry = 0;
    private int messageRetry = 0;
    private int messageLogDisplayIgnoreCount = 0;
    private int messageLogDisplayCounter = 0;
    private boolean isDisplayLogMessage = false;
    //private ProcessControlDAO processControlDAO;
    private int connectRetryAttempts;
    private int connectAttemptsRemaining;
    private String proxyType;
    private String proxyUrl;
    private int proxyPort;
    private java.util.Date lastDbConnection = null;
    private Calendar calc = Calendar.getInstance();
    private SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy");
    private String environment;

    public EConfirmStatusPollingService() {
        super("affinity.cwf:service=EConfirmStatusPolling");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    public String getEConfirmAPIUrl() {
        return eConfirmAPIUrl;
    }

    public void setEConfirmAPIUrl(String pEConfirmAPIUrl) {
        this.eConfirmAPIUrl = pEConfirmAPIUrl;
    }

    public String getEConfirmTradeInfoServiceUrl() {
        return eConfirmTradeInfoServiceUrl;
    }

    public void setEConfirmTradeInfoServiceUrl(String pEConfirmTradeInfoServiceUrl) {
        this.eConfirmTradeInfoServiceUrl = pEConfirmTradeInfoServiceUrl;
    }

    public void setOpsTrackingDBInfoName(String pOpsTrackingDBInfoName) {
        this.opsTrackingDBInfoName = pOpsTrackingDBInfoName;
    }

    public ObjectName getOpsTrackingDBInfo() throws MalformedObjectNameException {
        if (opsTrackingDBInfoName.length() > 0)
            return new ObjectName("sempra.utils:service=" + opsTrackingDBInfoName);
        else
            return null;
    }

    public void setAffinityDBInfoName(String pAffinityDBInfoName) {
        this.affinityDBInfoName = pAffinityDBInfoName;
    }

    public ObjectName getAffinityDBInfo() throws MalformedObjectNameException {
        if (affinityDBInfoName.length() > 0)
            return new ObjectName("sempra.utils:service=" + affinityDBInfoName);
        else
            return null;
    }

/*    public void setSymphonyDBInfoName(String pSymphonyDBInfoName) {
        this.symphonyDBInfoName = pSymphonyDBInfoName;
    }

    public ObjectName getSymphonyDBInfo() throws MalformedObjectNameException {
        if (symphonyDBInfoName.length() > 0)
            return new ObjectName("sempra.utils:service=" + symphonyDBInfoName);
        else
            return null;
    }*/

    public String getSmtpHost() {
        return smtpHost;
    }

    public void setSmtpHost(String pSMTPHost) {
        this.smtpHost = pSMTPHost;
    }

    public String getSmtpPort() {
        return smtpPort;
    }

    public void setSmtpPort(String pSMTPPort) {
        this.smtpPort = pSMTPPort;
    }

    public String getFileStoreDir() {
        return fileStoreDir;
    }

    public void setFileStoreDir(String pFileStoreDir) {
        this.fileStoreDir = pFileStoreDir;
    }

    public int getFileStoreExpireDays() {
        return this.fileStoreExpireDays;
    }

    public void setFileStoreExpireDays(int pStoreDirExpireDays) {
        this.fileStoreExpireDays = pStoreDirExpireDays;
    }

    public String getAllegedQueryXSL(){
        return this.allegedQueryXSL;
    }

    public void setAllegedQueryXSL(String pEConfirmAllegedXSL){
        allegedQueryXSL = pEConfirmAllegedXSL;
    }

    public String getAllegedQueryHTML(){
        return this.allegedQueryHTML;
    }

    public void setAllegedQueryHTML(String pEConfirmAllegedHTML){
        allegedQueryHTML = pEConfirmAllegedHTML;
    }

    public String getProxyType() {
        return proxyType;
     }
    public void setProxyType(String proxyType) {
        this.proxyType = proxyType;
     }

    public String getProxyUrl() {
         return this.proxyUrl;
     }

    public void setProxyUrl(String proxyUrl) {
         this.proxyUrl = proxyUrl;
    }

    public int getProxyPort() {
         return this.proxyPort;
     }

    public void setProxyPort(int proxyPort) {
         this.proxyPort =  proxyPort;
     }

    public int getMessageLogDisplayIgnoreCount() {
        return this.messageLogDisplayIgnoreCount;
    }

    public void setMessageLogDisplayIgnoreCount(int pMessageLogDisplayIgnoreCount) {
        this.messageLogDisplayIgnoreCount =  pMessageLogDisplayIgnoreCount;
    }

    public String getEnv() {
        return environment;
    }

    public void setEnv(String pEnv) {
        environment = pEnv;
    }

    protected void onServiceStarting() throws Exception {
        Logger.getLogger(this.getClass()).info("Executing startService... ");//getSqlConnection().setAutoCommit(false);
        init();
    }

    protected void onServiceStoping() {
        try {
            close();
        } catch (Exception e) {
            log.error(e);
        }
    }

    public void close()  {
        //eConfirmProcessor = null;
        //eConfirmDAO = null;
        eConfirmData = null;
        eConfirmAPI = null;

        eConfirmSummaryDataRec = null;
        ecMesssageLogRec = null;
        //processControlDAO = null;
        //brokerBlockedIgnoreDAO = null;

        if(opsTrackingConnection != null){
            try {
                opsTrackingConnection.close();
            } catch (SQLException e) {
                log.error(e);
            }
            opsTrackingConnection = null;
        }

        if(affinityConnection != null){
            try {
                affinityConnection.close();
            } catch (SQLException e) {
                log.error(e);
            }
            affinityConnection = null;
        }

/*        if(symphonyConnection != null){
            try {
                symphonyConnection.close();
            } catch (SQLException e) {
                log.error(e);
            }
            symphonyConnection = null;
        }*/
    }

    private void init() throws Exception {
        Logger.getLogger(this.getClass()).info("Executing init... ");
        //paused = true;

        Logger.getLogger(this.getClass()).info("Connecting opsTrackingConnection to " + opsTrackingDBInfoName + "...");
        opsTrackingConnection = getOracleConnection(opsTrackingDBInfoName, opsTrackingConnection);
        Logger.getLogger(this.getClass()).info("Connected opsTrackingConnection to " + opsTrackingDBInfoName + ".");

        Logger.getLogger(this.getClass()).info("Connecting affinityConnection to " + affinityDBInfoName + "...");
        affinityConnection = getOracleConnection(affinityDBInfoName, affinityConnection);
        Logger.getLogger(this.getClass()).info("Connected affinityConnection to " + affinityDBInfoName + ".");

/*
        Logger.getLogger(this.getClass()).info("Connecting symphonyConnection to " + symphonyDBInfoName + "...");
        symphonyConnection = getSymphonyConnection();
        Logger.getLogger(this.getClass()).info("Connected symphonyConnection to " + symphonyDBInfoName + ".");
*/

        String text = "";
        text = "Timer interval = " + (getTimerPeriod() / 1000) + " seconds.";
        Logger.getLogger(this.getClass()).info(text);

        text = "MessageLogDisplayIgnoreCount = " + messageLogDisplayIgnoreCount;
        Logger.getLogger(this.getClass()).info(text);
        //eConfirmDAO = new EConfirmDAO(opsTrackingConnection,affinityConnection,symphonyConnection);
        eConfirmData = new EConfirmData(opsTrackingConnection);

//        String ecUserId = "";
//        String ecPassword = "";
//        ecUserId = eConfirmDAO.getECUserId();
//        ecPassword = eConfirmDAO.getECPassword();
//
//        eConfirmProcessor = new EConfirmProcessor(affinityConnection, symphonyConnection, eConfirmURL, fileStoreDir,
//                "StatusPolling", fileStoreExpireDays);
//        eConfirmProcessor.setEConfirmUserName(ecUserId);
//        eConfirmProcessor.setEConfirmPassword(ecPassword);
//
//        // added JPM enhancements
//        eConfirmProcessor.setEcUniqueUserIdList(eConfirmDAO.getUniqueUserList());
//
//        eConfirmProcessor.setProxyType(this.proxyType);
//        eConfirmProcessor.setProxyUrl(this.proxyUrl);
//        eConfirmProcessor.setProxyPort(this.proxyPort);

        eConfirmAPI = new EConfirmAPI(eConfirmAPIUrl, eConfirmTradeInfoServiceUrl, proxyType, proxyUrl, proxyPort, fileStoreDir, SERVICE_NAME, fileStoreExpireDays);

        Logger.getLogger(this.getClass()).info("eConfirmTradeInfoServiceUrl=" + eConfirmTradeInfoServiceUrl);
        Logger.getLogger(this.getClass()).info("eConfirmAPIUrl=" + eConfirmAPIUrl);

        AppControlDAO appControlDAO;
        appControlDAO = new AppControlDAO(opsTrackingConnection,"ESPS");
        allegedQueryStartDate = appControlDAO.getValue("ALLEGED_QUERY_START_DATE");
        allegedEMailSendAtHourGMT = appControlDAO.getValue("ALLEGED_EMAIL_SEND_AT_HOUR_GMT");
        allegedEMailSendToAddress = appControlDAO.getValue("ALLEGED_EMAIL_SEND_TO_ADDRESS");
        sendToMessageLog = appControlDAO.getValue("MESSAGE_LOG_SEND_TO_ADDRESS");
        sendToStatus = appControlDAO.getValue("STATUS_SEND_TO_ADDRESS");

        Logger.getLogger(this.getClass()).info("allegedQueryXSL="  + allegedQueryXSL);
        Logger.getLogger(this.getClass()).info("allegedQueryHTML="  + allegedQueryHTML);
        Logger.getLogger(this.getClass()).info("allegedQueryStartDate="  + allegedQueryStartDate);
        Logger.getLogger(this.getClass()).info("allegedEMailSendAtHourGMT="  + allegedEMailSendAtHourGMT);
        Logger.getLogger(this.getClass()).info("allegedEMailSendToAddress="  + allegedEMailSendToAddress);

        mailUtils = new MailUtils(smtpHost, smtpPort);
        String hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
        sentFromName = "eConfirm_Error_" + hostName;
        sentFromAddress = "JBossOn" + hostName + "@sempratrading.com";

        //processControlDAO = new ProcessControlDAO(opsTrackingConnection);
        //brokerBlockedIgnoreDAO = new BrokerBlockedIgnoreDAO(opsTrackingConnection);

        ecMesssageLogRec = new EConfirmMessageLog_DataRec();
        eConfirmSummaryDataRec = new EConfirmSummary_DataRec();
        setDbDisplayNames();

        //Israel 12/17/2014 -- removed from service.
        //initMailVariables(appControlDAO);

        connectAttemptsRemaining = connectRetryAttempts;

        Logger.getLogger(this.getClass()).info("Init OK.");
        lastDbConnection =  new java.util.Date();
    }

/*    private void initMailVariables(AppControlDAO pAppControlDAO) throws Exception {
        String hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
        sentFromName = "eConfirm_Error_" + hostName;
        sentFromAddress = "JBossOn" + hostName + "@sempratrading.com";
        sendToName = "eConfirm Message Recipients";
        elecUSSentFromName = "eConfirm_USElec_" + hostName;
        ngasUSSentFromName = "eConfirm_USNGas_" + hostName;
        oilSentFromName = "eConfirm_Oil_" + hostName;
        elecUKSentFromName = "eConfirm_UKElec_" + hostName;
        ngasUKSentFromName = "eConfirm_UKNGas_" + hostName;
        ngasSGEFromName  = "eConfirm_SGENGas_" + hostName;
        elecSGEFromName  = "eConfirm_SGEELEC_" + hostName;
        oilSGEFromName = "eConfirm_SGEOIL_" + hostName;
        //allCdtySentFromName = "eConfirm_AllCdty_" + hostName;
        elecUSSendToName = "ElecUSNotifyList";
        ngasUSSendToName = "NGasUSNotifyList";
        oilSendToName = "OilNotifyList";
        elecUKSendToName = "ElecUKNotifyList";
        ngasUKSendToName = "NGasUKNotifyList";
        ngasSendToSGEName = "NGasSGENotifyLi*//*st";
        elecSendToSGEName = "ElecSGENotifyList";
        oilSendToSGEName  = "OilSGENotifyList";


        elecUSSendToAddress = pAppControlDAO.getValue("ECONFIRM_EMAIL_US_ELEC");
        ngasUSSendToAddress = pAppControlDAO.getValue("ECONFIRM_EMAIL_US_NGAS");
        oilSendToAddress = pAppControlDAO.getValue("ECONFIRM_EMAIL_US_OIL");
        elecUKSendToAddress = pAppControlDAO.getValue("ECONFIRM_EMAIL_UK_ELEC");
        ngasUKSendToAddress = pAppControlDAO.getValue("ECONFIRM_EMAIL_UK_NGAS");
        ngasSendToSGEAddress = pAppControlDAO.getValue("ECONFIRM_EMAIL_SGE_NGAS");
        elecSendToSGEAddress = pAppControlDAO.getValue("ECONFIRM_EMAIL_SGE_ELEC");
        oilSendToSGEAddress = pAppControlDAO.getValue("ECONFIRM_EMAIL_SGE_OIL"); 
        
        confirmSupportSendToAddress = pAppControlDAO.getValue("CONFIRM_SUPPORT_ADDRESS");

        if (!environment.equalsIgnoreCase("PROD")){
            final String testAddress = "sraj@sempratrading.com";
            sendToMessageLog = testAddress;
            sendToStatus = testAddress;
            elecUSSendToAddress = testAddress;
            ngasUSSendToAddress = testAddress;
            oilSendToAddress = testAddress;
            elecUKSendToAddress = testAddress;
            ngasUKSendToAddress = testAddress;
            ngasSendToSGEAddress = testAddress;
            elecSendToSGEAddress = testAddress;
            oilSendToSGEAddress = testAddress;
        }

        Logger.getLogger(this.getClass()).info("localHostName="+hostName);
        //Logger.getLogger(this.getClass()).info("prodHostName="+prodHostName);
        Logger.getLogger(this.getClass()).info("sendToMessageLog="+sendToMessageLog);
        Logger.getLogger(this.getClass()).info("sendToStatus="+sendToStatus);
        Logger.getLogger(this.getClass()).info("elecUSSendToAddress="+elecUSSendToAddress);
        Logger.getLogger(this.getClass()).info("ngasUSSendToAddress="+ngasUSSendToAddress);
        Logger.getLogger(this.getClass()).info("oilSendToAddress="+oilSendToAddress);
        Logger.getLogger(this.getClass()).info("elecUKSendToAddress="+elecUKSendToAddress);
        Logger.getLogger(this.getClass()).info("ngasUKSendToAddress="+ngasUKSendToAddress);
        Logger.getLogger(this.getClass()).info("ngasSendToSGEAddress="+ngasSendToSGEAddress);
        Logger.getLogger(this.getClass()).info("elecSendToSGEAddress="+elecSendToSGEAddress);
        Logger.getLogger(this.getClass()).info("oilSendToSGEAddress="+oilSendToSGEAddress);
        Logger.getLogger(this.getClass()).info("confirmSupportSendToAddress="+confirmSupportSendToAddress);
    }*/

    private String getGreaterDateTime(String pCurrentGreatest, String pMessageDate)
            throws ParseException {
        Calendar currentGreatestCal = Calendar.getInstance();
        Calendar messageDateCal = Calendar.getInstance();
        Date currentGreatest = sdfDateTime.parse(pCurrentGreatest);
        Date messageDate = sdfDateTime.parse(pMessageDate);
        currentGreatestCal.setTime(currentGreatest);
        messageDateCal.setTime(messageDate);
        String greaterDateTime = "";
        if (messageDateCal.after(currentGreatestCal))
            greaterDateTime = pMessageDate;
        else
            greaterDateTime = pCurrentGreatest;
        return greaterDateTime;
    }

    private String incDateTime(String pDateTime) throws ParseException {
        Date date = sdfDateTime.parse(pDateTime);
        Calendar rollDateTimeCal = Calendar.getInstance();
        rollDateTimeCal.setTime(date);
        rollDateTimeCal.add(Calendar.SECOND, 1);
        return sdfDateTime.format(rollDateTimeCal.getTime());
    }

    /**
     * JMX console method, fired on demand.
     */
    synchronized public void executeTimerEventNow() throws StopServiceException, LogException{
        Logger.getLogger(this.getClass()).info("executeTimerEventNow() executing...");
        runTask();
        Logger.getLogger(this.getClass()).info("executeTimerEventNow() done.");
    }

    protected void runTask() throws StopServiceException, LogException {
        checkToReconnect();
        poll();
    }

    synchronized private void poll() throws StopServiceException{
        try {
            if (messageLogDisplayCounter >= messageLogDisplayIgnoreCount) {
                isDisplayLogMessage = true;
                messageLogDisplayCounter = 0;
            }
            else
                isDisplayLogMessage = false;
            messageLogDisplayCounter++;

            if (isDisplayLogMessage)
                Logger.getLogger(this.getClass()).info("Executing polling task...");
            Hashtable userList = eConfirmData.getUniqueUserList();
            Enumeration enumer = userList.keys();
            while (enumer.hasMoreElements()) {
                String ecUserId = (String) enumer.nextElement();
                String ecPassword = (String) userList.get(ecUserId);
                callCheckStatus(ecUserId, ecPassword);
                callCheckMessageLog(ecUserId, ecPassword);
                //callGetAlleged();
                callCheckBkrStatus(ecUserId, ecPassword);
            }
            //callEMAilAlleged();
            connectAttemptsRemaining = connectRetryAttempts;
            if (isDisplayLogMessage)
                Logger.getLogger(this.getClass()).info("Execute polling task done. " +
                    "[Log messages were suppressed for previous " +
                    Integer.toString(messageLogDisplayIgnoreCount) + " executions.]");
        } catch (Exception e) {
            try {
                Logger.getLogger(this.getClass()).error(e);
                opsTrackingConnection.rollback();
            } catch (Exception e1) {
                Logger.getLogger(this.getClass()).error(e1);
            }
            if (connectAttemptsRemaining <= 0)
                throw new StopServiceException(e.getMessage());
            connectAttemptsRemaining = connectAttemptsRemaining -1;
        }
    }

    private int getCurrentHour(){
        SimpleDateFormat sdfHourInDay = new SimpleDateFormat("H");
        String currentHourStr = "";
        int currentHour = -1;
        Date currentDt = new Date();
        currentHourStr = sdfHourInDay.format(currentDt);
        currentHour = Integer.parseInt(currentHourStr);
        return currentHour;
    }

    private void sendAllegedMail() throws MessagingException, UnsupportedEncodingException {
        String subject = "EConfirm Alleged Trades";
        String mailDesc = "EConfirm Alleged Trades";
        String allegedSendToName = "EConfirm Alleged Query Recipients";

        mailUtils.sendMail(allegedEMailSendToAddress, allegedSendToName, sentFromAddress, sentFromName,
            subject, mailDesc, allegedQueryHTML );

        Logger.getLogger(this.getClass()).info("Alleged Trade EMail sent to: " +
                allegedEMailSendToAddress);
    }

    private void callCheckStatus(String pECUserId, String pECPassword) throws SQLException, ParseException, StopServiceException {
        String lastDateTime;
        String currentDateTime;
        String newestDateTime;
        lastDateTime = eConfirmData.getLastStatusDateTime(pECUserId);
        currentDateTime = incDateTime(lastDateTime);
        newestDateTime = checkStatus(currentDateTime, pECUserId, pECPassword);
        if (!currentDateTime.equalsIgnoreCase(newestDateTime))
            eConfirmData.updateLastStatusDateTime(newestDateTime, pECUserId);
        opsTrackingConnection.commit();
    }

    private void callCheckMessageLog(String pECUserId, String pECPassword)
            throws SQLException, AuthenticateFailedException, JDOMException, IOException, NamingException,
            SECptyNotFoundException, ParseException, StopServiceException, MessagingException {
        String lastDateTime;
        String currentDateTime;
        String newestDateTime;
        lastDateTime = eConfirmData.getLastMessageDateTime(pECUserId);
        currentDateTime = incDateTime(lastDateTime);
        newestDateTime = checkMessageLog(currentDateTime, pECUserId, pECPassword);
        if (!currentDateTime.equalsIgnoreCase(newestDateTime))
            eConfirmData.updateLastMessageDateTime(newestDateTime, pECUserId);
        opsTrackingConnection.commit();
    }

    // 06-16-2009 Samy: Added for broker matching.
    private void callCheckBkrStatus(String pECUserId, String pECPassword) throws SQLException, StopServiceException, ParseException {
        String lastDateTime;
        String currentDateTime;
        String newestDateTime;
        lastDateTime = eConfirmData.getLastBkrStatusDateTime(pECUserId);
        currentDateTime = incDateTime(lastDateTime);
        newestDateTime = checkBkrStatus(currentDateTime, pECUserId, pECPassword);
        if (!currentDateTime.equalsIgnoreCase(newestDateTime))
            eConfirmData.updateLastBkrStatusDateTime(newestDateTime, pECUserId);
        opsTrackingConnection.commit();
    }

    /**
     * Does a type 8 query to check for matched trades or other status changes.
     */
    private String checkStatus(String pStatusDateTime, String pECUserId, String pECPassword)
            throws StopServiceException
            {
        String resultXML = null;
        String greatestDateTime;
        String tradeID = "";
        try {
            //debug
            resultXML = eConfirmAPI.queryStatus(pStatusDateTime, pECUserId, pECPassword);
            SAXBuilder saxBuilder = new SAXBuilder();
            String tradingSys = "";
            String status = "";
            String errorFlag = "N";
            String statusDate = "";
            String traceId = "";
            String buyer = "";
            String seller = "";
            String submissionCompany = "";
            String tradeDt = "";

            tradeID = "";
            String brokenFieldData = "";
            double dTradeID = -1;
            boolean isClickAndConfirm = false;
            ECIgnoredStatusMessage_Rec ecIgnoredStatusMessageRec;
            Document doc = null;
            //debug
            doc = saxBuilder.build(new StringReader(resultXML));
            //doc = saxBuilder.build(new FileReader("c:\\102350_RESP.xml"));
            Element rootElem = doc.getRootElement();
            Element tradeInfoElem = rootElem.getChild("EConfirmTradeInfo");

            String prevRqmtStatus = "";
            String cptyTradeID = "";
            greatestDateTime = pStatusDateTime;
            final SimpleDateFormat sdfMatchedDate = new SimpleDateFormat("MM/dd/yyyy", Locale.US);

            List nodes = tradeInfoElem.getChildren("EConfirmSimpleResponse");
            if (nodes != null) {
                for (Iterator i = nodes.iterator(); i.hasNext();) {
                    tradeID = "";
                    status = "";
                    statusDate = "";
                    traceId = "";
                    buyer = "";
                    seller = "";
                    submissionCompany = "";
                    tradeDt = "";

                    isClickAndConfirm = false;
                    Element elem = (Element) i.next();
                    tradeID = elem.getChildText("SenderTradeRefId");
                    status = elem.getChildText("Status");
                    statusDate = elem.getChildText("StatusDate");

                    //Israel 7/2/15 -- Implement logging non-ops_tracking trades to ignored_status_message
                    traceId = elem.getChildText("TraceId");
                    buyer = elem.getChildText("Buyer");
                    seller = elem.getChildText("Seller");
                    submissionCompany = elem.getChildText("SubmissionCompany");
                    tradeDt = elem.getChildText("TradeDate");
                    ecIgnoredStatusMessageRec = new ECIgnoredStatusMessage_Rec(tradeID, traceId, status, buyer, seller,
                            submissionCompany, statusDate, tradeDt);

                    //IF 2/10/2004 - third parm for setNotifyOpsTrackingMatched method call below.
                    Date matchedDate = sdfDateTime.parse(statusDate);
                    String matchedDateStr = sdfMatchedDate.format(matchedDate);

                    String cptyTradeRefId = "";
                    String counterParty = "";
                    Element cptyInfoElem = elem.getChild("CounterPartyInfo");
                    if (cptyInfoElem != null) {
                        Element tradeElem = cptyInfoElem.getChild("Trade");
                        if (tradeElem != null) {
                            counterParty = tradeElem.getChildText("CounterParty");
                            cptyTradeRefId = tradeElem.getChildText("SenderTradeRefId");
                        }
                    }

/*
                    if (tradeID.trim().length() > 7)
                        tradingSys = "AFF";
                    else
                        tradingSys = "SYM";
*/

                    try {
                        //Samy  do not process the JP trades                        
/*
                        if (JPM_USER.equalsIgnoreCase(this.eConfirmProcessor.getEConfirmUserName())) {
                            if (!tradeID.startsWith("A") && !tradeID.startsWith("J")) {
                                continue;
                            }
                        }
*/
                        //Israel 1/5/2007 ICTS interface includes trd sys prefix.
                        String strippedTradeID = stripTradingSystemPrefix(tradeID);
                        dTradeID = Double.parseDouble(strippedTradeID);
                        tradingSys = eConfirmData.getECTradeSummaryTradingSys(dTradeID);
                    } catch (NumberFormatException e) {
                        //This happens when we click and confirm
                        Logger.getLogger(this.getClass()).info(greatestDateTime +
                                ": " + tradingSys + " " + tradeID + " has a non-standard trade id and will be skipped.");
                        String subject = "Non-standard Trade Id processed: " + tradingSys + " " + tradeID;
                        String mailDesc =
                                "A trade with a non-Standard trade id was confirmed. " +
                                "The system is skipping the trade(s) without updating Ops Tracking. " +
                                "No fee has been added for this trade. " +
                                "\n  TradeId=" + tradeID +
                                "\n  Cpty=" + counterParty +
                                "\n  CptyTradeRef=" + cptyTradeRefId +
                                "\n  Status=" + status +
                                "\n  StatusDate=" + statusDate +
                                "\n  --------------------------------------------------------------" +
                                "\n  eConfirmAPIUrl=" + eConfirmAPIUrl +
                                "\n  affinityDBInfoName=" + affinityDBInfoDisplayName +
                                "\n  opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                        //5/3/07 Israel - group-specific email
                        //sendMailByGroup(dTradeID, subject, mailDesc);
                        //mailUtils.sendMail(sendToStatus, sendToName, sentFromAddress, sentFromName,
                          //      subject, mailDesc, "");

                        //Must be set here in case this is the last one (or only one) processed.
                        greatestDateTime = getGreaterDateTime(greatestDateTime,statusDate);
                        //Skip to next iteration of the for loop.
                        continue;
                    }

                    prevRqmtStatus = "";
                    if (status.equalsIgnoreCase("MATCHED") ||
                        status.equalsIgnoreCase("MATCHED-CLICK AND CONFIRMED")) {
                        if (status.equalsIgnoreCase("MATCHED-CLICK AND CONFIRMED")){
                            isClickAndConfirm = true;
                            status = "MATCHED";

                            if (!isEConfRqmt(dTradeID)){

                                String subject = "A trade without an EConfirm Rqmt was processed: " + tradingSys + " " + tradeID;
                                String mailDesc =
                                        "A trade without an EConfirm requirement was processed. " +
                                        "The system is skipping the trade(s) without updating Ops Tracking. " +
                                        "No fee has been added for this trade. " +
                                        "\n  TradeId=" + tradeID +
                                        "\n  Cpty=" + counterParty +
                                        "\n  CptyTradeRef=" + cptyTradeRefId +
                                        "\n  Status=" + status +
                                        "\n  StatusDate=" + statusDate +
                                        "\n  --------------------------------------------------------------" +
                                        "\n  eConfirmAPIUrl=" + eConfirmAPIUrl +
                                        "\n  affinityDBInfoName=" + affinityDBInfoDisplayName +
                                        "\n  opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                                //5/3/07 Israel - group-specific email
                                //sendMailByGroup(dTradeID, subject, mailDesc);
                                //mailUtils.sendMail(sendToStatus, sendToName, sentFromAddress, sentFromName,
                                  //      subject, mailDesc, "");

                                //Must be set here in case this is the last one (or only one) processed.
                                greatestDateTime = getGreaterDateTime(greatestDateTime,statusDate);
                                //Skip to next iteration of the for loop.
                                continue;
                            }

                            String brokerSn = getBrokerSn(dTradeID);
                            if ("".equalsIgnoreCase(brokerSn) ||
                                NO_BROKER.equalsIgnoreCase(brokerSn)){

                                Logger.getLogger(this.getClass()).info(greatestDateTime +
                                        ": " + tradingSys + " " + tradeID + " is an unbrokered click and confirm.");

                                String subject = "Unbrokered Click and Confirm Match: " + tradingSys + " " + tradeID;
                                String mailDesc =
                                        "An unbrokered trade has been matched via click and confirm. " +
                                        "The system has been updated as usual and the fee added. " +
                                        "\n  TradeId=" + tradeID +
                                        "\n  Cpty=" + counterParty +
                                        "\n  CptyTradeRef=" + cptyTradeRefId +
                                        "\n  Status=" + status +
                                        "\n  StatusDate=" + statusDate +
                                        "\n  Click And Confirmed=" + isClickAndConfirm +
                                        "\n  --------------------------------------------------------------" +
                                        "\n  eConfirmAPIUrl=" + eConfirmAPIUrl +
                                        "\n  affinityDBInfoName=" + affinityDBInfoDisplayName +
                                        "\n  opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                                //5/3/07 Israel - group-specific email
                                //sendMailByGroup(dTradeID, subject, mailDesc);
                            }
                        }


                        if (eConfirmData.isECTradeSummaryExist(tradingSys, dTradeID)) {
                            eConfirmData.setNotifyOpsTrackingMatched(dTradeID, cptyTradeRefId, matchedDateStr);
                            //sempra.prod test makes sure we update only in production environment

                            // 8-27-07: MThoresen:  Commented out for Sybase DB Changes.
                            /*  if (tradingSys.equalsIgnoreCase("SYM") && opsTrackingDBInfoDisplayName.equalsIgnoreCase("SEMPRA.PROD"))
                                updateICTSFee(dTradeID, cptyTradeRefId);   */
                            Logger.getLogger(this.getClass()).info(greatestDateTime + ": " +
                                    tradingSys + " " + tradeID + " is now MATCHED");
                        }
                        else {
                            Logger.getLogger(this.getClass()).info(greatestDateTime +
                                    ": " + tradingSys + " " + tradeID + " is unexpected and will be skipped.");
                            String subject = "Non-eConfirm trade matched: " + tradingSys + " " + tradeID;
                            String mailDesc =
                                    "A trade that was not submitted to eConfirm has been matched on eConfirm. " +
                                    "The system is skipping the trade(s) without updating Ops Tracking. " +
                                    "No fee has been added for this trade. " +
                                    "\n  TradeId=" + tradeID +
                                    "\n  Cpty=" + counterParty +
                                    "\n  CptyTradeRef=" + cptyTradeRefId +
                                    "\n  Status=" + status +
                                    "\n  StatusDate=" + statusDate +
                                    "\n  Click And Confirmed=" + isClickAndConfirm +
                                    "\n  --------------------------------------------------------------" +
                                    "\n  eConfirmAPIUrl=" + eConfirmAPIUrl +
                                    "\n  affinityDBInfoName=" + affinityDBInfoDisplayName +
                                    "\n  opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                            //5/3/07 Israel - group-specific email
                            //sendMailByGroup(dTradeID, subject, mailDesc);
                            //mailUtils.sendMail(sendToStatus, sendToName, sentFromAddress, sentFromName,
                              //      subject, mailDesc, "");

                            greatestDateTime = getGreaterDateTime(greatestDateTime,statusDate);
                            continue;
                        }
                    } else if (status.equalsIgnoreCase("UNMATCHED")) {
                        brokenFieldData = "";
                        brokenFieldData = getBrokenFieldData(tradeID, pECUserId, pECPassword);
                        if (brokenFieldData.equalsIgnoreCase(UNKNOWN_UNMATCHED)){
                            String messageDesc = "PROBLEM: eConfirm erroneously considers this trade Unmatched.";
                            eConfirmData.setNotifyOpsTrackingError(dTradeID, messageDesc);
                            status = "ERROR";
                            errorFlag = "Y";
                            Logger.getLogger(this.getClass()).info(greatestDateTime +
                                ": " + tradingSys + " " + tradeID + " now has an ERROR: " + UNKNOWN_UNMATCHED);
                            String subject = "Unknown Unmatched trade Errror: " + tradingSys + " " + tradeID;
                            String mailDesc = messageDesc +
                                    "\nSOLUTION: We must phone the eConfirm Help Desk (770-738-2102) to tell them about the problem." +
                                    " Once they resubmit it on their end the status will go to matched" +
                                    " and the error condition will automatically be resolved on our system. CAUTION:" +
                                    " There may be more than just the one trade listed here affected, so have" +
                                    " eConfirm check all trades having an UNMATCHED status." +
                                    "\n  TradeId=" + tradeID +
                                    "\n  Status=" + status +
                                    "\n  StatusDate=" + statusDate +
                                    "\n  eConfirmAPIUrl=" + eConfirmAPIUrl +
                                    "\n  affinityDBInfoName=" + affinityDBInfoDisplayName +
                                    "\n  opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                            //5/3/07 Israel - group-specific email
                            //sendMailByGroup(dTradeID, subject, mailDesc);
                            //mailUtils.sendMail(sendToStatus, sendToName, sentFromAddress, sentFromName,
                              //  subject, mailDesc, "" );
                        }
                        else {
                            //brokenFieldData = "No more data from econfirm_v1";
                            eConfirmData.setNotifyOpsTrackingUnmatched(dTradeID, brokenFieldData);
                            Logger.getLogger(this.getClass()).info(greatestDateTime +
                                ": " + tradingSys + " " + tradeID + " is now UNMATCHED: " + brokenFieldData);
                        }
                    } else if (status.equalsIgnoreCase("PENDING")) {
                        prevRqmtStatus = eConfirmData.getEConfirmRqmtStatus(dTradeID);
                        eConfirmData.setNotifyOpsTrackingPending(dTradeID);
                        Logger.getLogger(this.getClass()).info(greatestDateTime + ": "
                                + tradingSys + " " + tradeID + " is now PENDING");
                    } else if (status.equalsIgnoreCase("CANCELED")) {
                        prevRqmtStatus = eConfirmData.getEConfirmRqmtStatus(dTradeID);
                        eConfirmData.setNotifyOpsTrackingCancelled(dTradeID);
                        eConfirmData.setNotifyOpsTrackingBkrCancelled(dTradeID); // samy: added to cancel broker too
                        Logger.getLogger(this.getClass()).info(greatestDateTime + ": " +
                                tradingSys + " " + tradeID + " is now CANCELED");
                    } else if (status.equalsIgnoreCase("DISPUTED")) {
                        String subject = "Trade is now DISPUTED: " + tradingSys + " " + tradeID;
                        String mailDesc =
                                "A trade that was submitted to eConfirm is now under dispute. " +
                                "The status of the trade is still MATCHED and will be until it becomes CANCELED. " +
                                "\n  TradeId=" + tradeID +
                                "\n  Cpty=" + counterParty +
                                "\n  CptyTradeRef=" + cptyTradeRefId +
                                "\n  Status=" + status +
                                "\n  StatusDate=" + statusDate +
                                "\n  --------------------------------------------------------------" +
                                "\n  eConfirmAPIUrl=" + eConfirmAPIUrl +
                                "\n  affinityDBInfoName=" + affinityDBInfoDisplayName +
                                "\n  opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                        //mailUtils.sendMail(disputedSendToAddress, sendToName, sentFromAddress, sentFromName,
                          //      subject, mailDesc, "");

                        //5/3/07 Israel - group-specific email
                        //sendMailByGroup(dTradeID, subject, mailDesc);

                        Logger.getLogger(this.getClass()).info(greatestDateTime + ": "
                                + tradingSys + " " + tradeID + " is DISPUTED (status still MATCHED)");
                    } else {
                        Logger.getLogger(this.getClass()).info(greatestDateTime + ": " +
                                tradingSys + " " + tradeID + ": Status not found. Status=" + status);
                      //  throw new StopServiceException(tradingSys + " " + tradeID + ": Status not found. Status=" + status);
                    }

                    eConfirmSummaryDataRec.init();
                    eConfirmSummaryDataRec.tradingSystem = tradingSys;
                    eConfirmSummaryDataRec.tradeID = dTradeID;
                    eConfirmSummaryDataRec.status = status;
                    eConfirmSummaryDataRec.errorFlag = errorFlag;
                    //eConfirmSummaryDataRec.cmt = "currentPollDateTime: " + currentPollDateTime;
                    eConfirmSummaryDataRec.cptyTradeRefID = cptyTradeRefId;

                    //Israel 7/1/15 -- Allows trades added outside of
                    //Confirmations manager to be processed without blowing up and stopping queue.
                    if (eConfirmData.isECTradeSummaryExist(tradingSys, dTradeID)) {
                        eConfirmData.updateECTradeSummary(eConfirmSummaryDataRec);
                    } else if (tradingSys.length() > 0) {
                        eConfirmData.insertECTradeSummary(eConfirmSummaryDataRec);
                    } else
                        eConfirmData.insertECIgnoredStatusMessage(ecIgnoredStatusMessageRec);

                    greatestDateTime = getGreaterDateTime(greatestDateTime,statusDate);
                    statusRetry = 0;

                    //WARNING--The procedure called here does a commit.
                    if ((status.equalsIgnoreCase("PENDING") || status.equalsIgnoreCase("CANCELED")) &&
                        prevRqmtStatus.equalsIgnoreCase("MATCH")) {
                        //Israel 3/28/07 Was blowing up when submitting a JMS trade number to affinity
                        //Now also deletes JMS fee
                        //if (tradingSys.equalsIgnoreCase("AFF"))
                            //eConfirmDAO.setCancelEConfirmFee(dTradeID);

                        // 8-27-07: MThoresen:  Commented out for Sybase DB Changes.
                        /*
                        else if (tradingSys.equalsIgnoreCase("SYM"))
                            deleteICTSFee(dTradeID); */

                        Logger.getLogger(this.getClass()).info(greatestDateTime +
                            ": The EConfirm fee for "
                            + tradingSys + " " + tradeID + " has been cancelled.");
                    }
                }
            }
        } catch (Exception e) {
            try {
                opsTrackingConnection.rollback();
                Logger.getLogger(this.getClass()).error("In checkStatus for trade "+ tradeID + ": " +e);
            } catch (SQLException e1) {
                Logger.getLogger(this.getClass()).error(e1);
            }
          throw new StopServiceException("In checkStatus: "+e.getMessage());
        }
        return greatestDateTime;
    }


    /**
     * Does a type 17 query to check for matched trades or other status changes for broker.
     */
    private String checkBkrStatus(String pStatusDateTime, String pECUserId, String pECPassword)
            throws StopServiceException
            {
        String resultXML = null;
        String greatestDateTime;
        String tradeID = "";
        try {
            //debug
            resultXML = eConfirmAPI.queryBkrStatus(pStatusDateTime, pECUserId, pECPassword);
            SAXBuilder saxBuilder = new SAXBuilder();
            String tradingSys = "";
            String status = "";
            String errorFlag = "N";
            String statusDate = "";
            tradeID = "";
            String brokenFieldData = "";
            double dTradeID = -1;
            boolean isClickAndConfirm = false;
            Document doc = null;
            //debug
            doc = saxBuilder.build(new StringReader(resultXML));
            //doc = saxBuilder.build(new FileReader("c:\\102350_RESP.xml"));
            Element rootElem = doc.getRootElement();
            Element tradeInfoElem = rootElem.getChild("EConfirmTradeInfo");

            String prevRqmtStatus = "";
            String cptyTradeID = "";
            greatestDateTime = pStatusDateTime;
            final SimpleDateFormat sdfMatchedDate = new SimpleDateFormat("MM/dd/yyyy", Locale.US);

            List nodes = tradeInfoElem.getChildren("EConfirmSimpleResponse");
            if (nodes != null) {
                for (Iterator i = nodes.iterator(); i.hasNext();) {
                    tradeID = "";
                    status = "";
                    statusDate = "";
                    isClickAndConfirm = false;
                    Element elem = (Element) i.next();
                    tradeID = elem.getChildText("SenderTradeRefId");
                    status = elem.getChildText("BrokerBuyerStatus");
                    if (status == null){
                        status = elem.getChildText("BrokerSellerStatus");
                    }
                    statusDate = elem.getChildText("BrokerBuyerStatusDate");
                    if (statusDate == null){
                       statusDate = elem.getChildText("BrokerSellerStatusDate");
                    }

                    //IF 2/10/2004 - third parm for setNotifyOpsTrackingMatched method call below.
                    Date matchedDate = sdfDateTime.parse(statusDate);
                    String matchedDateStr = sdfMatchedDate.format(matchedDate);

                    String brokerTradeRefId = "";
                    String brokerName = "";
                    brokerName = elem.getChildText("BrokerCompany");
                    Element cptyInfoElem = elem.getChild("BrokerInfo");
                    if (cptyInfoElem != null) {
                        Element tradeE = cptyInfoElem.getChild("Trade");
                        if ( tradeE != null) {
                            brokerTradeRefId = tradeE.getChildText("SenderTradeRefId");
                        }
                    }

/*
                    if (tradeID.trim().length() > 7)
                        tradingSys = "AFF";
                    else
                        tradingSys = "SYM";
*/

                    try {
                        // Samy : skip JPM trades
/*
                        if (JPM_USER.equalsIgnoreCase(this.eConfirmProcessor.getEConfirmUserName())) {
                            if (!tradeID.startsWith("A") && !tradeID.startsWith("J")) {
                                continue;
                            }
                        }
*/
                        //Israel 1/5/2007 ICTS interface includes trd sys prefix.
                        String strippedTradeID = stripTradingSystemPrefix(tradeID);
                        dTradeID = Double.parseDouble(strippedTradeID);
                        tradingSys = eConfirmData.getECTradeSummaryTradingSys(dTradeID);
                    } catch (NumberFormatException e) {
                        //This happens when we click and confirm
                        Logger.getLogger(this.getClass()).info(greatestDateTime +
                                ": " + tradingSys + " " + tradeID + " has a non-standard trade id and will be skipped for Broker.");
                        String subject = "Non-standard Trade Id processed: " + tradingSys + " " + tradeID;
                        String mailDesc =
                                "A trade with a non-Standard trade id was confirmed. " +
                                "The system is skipping the trade(s) without updating Ops Tracking. " +
                                "No fee has been added for this trade. " +
                                "\n  TradeId=" + tradeID +
                                "\n  Broker=" + brokerName +
                                "\n  BrokerRef=" + brokerTradeRefId +
                                "\n  Status=" + status +
                                "\n  StatusDate=" + statusDate +
                                "\n  --------------------------------------------------------------" +
                                "\n  eConfirmAPIUrl=" + eConfirmAPIUrl +
                                "\n  affinityDBInfoName=" + affinityDBInfoDisplayName +
                                "\n  opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                        //5/3/07 Israel - group-specific email
                        //sendMailByGroup(dTradeID, subject, mailDesc);
                        //mailUtils.sendMail(sendToStatus, sendToName, sentFromAddress, sentFromName,
                          //      subject, mailDesc, "");

                        //Must be set here in case this is the last one (or only one) processed.
                        greatestDateTime = getGreaterDateTime(greatestDateTime,statusDate);
                        //Skip to next iteration of the for loop.
                        continue;
                    }

                    prevRqmtStatus = "";
                    if (status.equalsIgnoreCase("MATCHED") ||
                        status.equalsIgnoreCase("MATCHED-CLICK AND CONFIRMED")) {
                        if (status.equalsIgnoreCase("MATCHED-CLICK AND CONFIRMED")){
                            isClickAndConfirm = true;
                            status = "MATCHED";

                            if (!isEConfBkrRqmt(dTradeID)){

                                String subject = "A trade without an EConfirm Broker Rqmt was processed: " + tradingSys + " " + tradeID;
                                String mailDesc =
                                        "A trade without an EConfirm requirement was processed. " +
                                        "The system is skipping the trade(s) without updating Ops Tracking. " +
                                        "No fee has been added for this trade. " +
                                        "\n  TradeId=" + tradeID +
                                        "\n  Broker=" + brokerName +
                                        "\n  BrokerRef=" + brokerTradeRefId +
                                        "\n  Status=" + status +
                                        "\n  StatusDate=" + statusDate +
                                        "\n  --------------------------------------------------------------" +
                                        "\n  eConfirmAPIUrl=" + eConfirmAPIUrl +
                                        "\n  affinityDBInfoName=" + affinityDBInfoDisplayName +
                                        "\n  opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                                //5/3/07 Israel - group-specific email
                                //sendMailByGroup(dTradeID, subject, mailDesc);
                                //mailUtils.sendMail(sendToStatus, sendToName, sentFromAddress, sentFromName,
                                  //      subject, mailDesc, "");
                                
                                //Must be set here in case this is the last one (or only one) processed.
                                greatestDateTime = getGreaterDateTime(greatestDateTime,statusDate);
                                //Skip to next iteration of the for loop.
                                continue;
                            }
                        }


                        if (eConfirmData.isECTradeSummaryExist(tradingSys, dTradeID)) {
                            eConfirmData.setNotifyOpsTrackingBkrMatched(dTradeID, brokerTradeRefId, matchedDateStr);
                            //sempra.prod test makes sure we update only in production environment

                            // 8-27-07: MThoresen:  Commented out for Sybase DB Changes.
                            /*  if (tradingSys.equalsIgnoreCase("SYM") && opsTrackingDBInfoDisplayName.equalsIgnoreCase("SEMPRA.PROD"))
                                updateICTSFee(dTradeID, cptyTradeRefId);   */
                            Logger.getLogger(this.getClass()).info(greatestDateTime + ": " +
                                    tradingSys + " " + tradeID + " is now Broker MATCHED");
                        }
                        else {

                            Logger.getLogger(this.getClass()).info(greatestDateTime +
                                    ": " + tradingSys + " " + tradeID + " is unexpected and will be skipped for Broker.");

                            String subject = "Non-eConfirm trade matched: " + tradingSys + " " + tradeID;
                            String mailDesc =
                                    "A trade that was not submitted to eConfirm has been matched on eConfirm. " +
                                    "The system is skipping the trade(s) without updating Ops Tracking. " +
                                    "No fee has been added for this trade. " +
                                    "\n  TradeId=" + tradeID +
                                    "\n  Broker=" + brokerName +
                                    "\n  CptyTradeRef=" + brokerTradeRefId +
                                    "\n  Status=" + status +
                                    "\n  StatusDate=" + statusDate +
                                    "\n  Click And Confirmed=" + isClickAndConfirm +
                                    "\n  --------------------------------------------------------------" +
                                    "\n  eConfirmAPIUrl=" + eConfirmAPIUrl +
                                    "\n  affinityDBInfoName=" + affinityDBInfoDisplayName +
                                    "\n  opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                            //5/3/07 Israel - group-specific email
                            //sendMailByGroup(dTradeID, subject, mailDesc);
                            //mailUtils.sendMail(sendToStatus, sendToName, sentFromAddress, sentFromName,
                              //      subject, mailDesc, "");
                           
                            greatestDateTime = getGreaterDateTime(greatestDateTime,statusDate);
                            continue;
                        }
                    } else if (status.equalsIgnoreCase("UNMATCHED")) {
                        brokenFieldData = "";
                        brokenFieldData = getBrokerBrokenFieldData(tradeID, pECUserId, pECPassword);
                        if (brokenFieldData.equalsIgnoreCase(UNKNOWN_UNMATCHED)){
                            String messageDesc = "PROBLEM: eConfirm erroneously considers this trade Broker Unmatched.";
                            eConfirmData.setNotifyOpsTrackingBkrError(dTradeID, messageDesc);
                            status = "ERROR";
                            errorFlag = "Y";
                            Logger.getLogger(this.getClass()).info(greatestDateTime +
                                ": " + tradingSys + " " + tradeID + " now has an ERROR for Broker Matching: " + UNKNOWN_UNMATCHED);
                            String subject = "Unknown Unmatched trade Errror: " + tradingSys + " " + tradeID;
                            String mailDesc = messageDesc +
                                    "\nSOLUTION: We must phone the eConfirm Help Desk (770-738-2102) to tell them about the problem." +
                                    " Once they resubmit it on their end the status will go to matched" +
                                    " and the error condition will automatically be resolved on our system. CAUTION:" +
                                    " There may be more than just the one trade listed here affected, so have" +
                                    " eConfirm check all trades having an UNMATCHED status." +
                                    "\n  TradeId=" + tradeID +
                                    "\n  Status=" + status +
                                    "\n  StatusDate=" + statusDate +
                                    "\n  eConfirmAPIUrl=" + eConfirmAPIUrl +
                                    "\n  affinityDBInfoName=" + affinityDBInfoDisplayName +
                                    "\n  opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                            //5/3/07 Israel - group-specific email
                            //sendMailByGroup(dTradeID, subject, mailDesc);
                            //mailUtils.sendMail(sendToStatus, sendToName, sentFromAddress, sentFromName,
                              //  subject, mailDesc, "" );
                        }
                        else {
                            //brokenFieldData = "No more data from econfirm_v1";
                            eConfirmData.setNotifyOpsTrackingBkrUnmatched(dTradeID, brokenFieldData);
                            Logger.getLogger(this.getClass()).info(greatestDateTime +
                                ": " + tradingSys + " " + tradeID + " is now Broker UNMATCHED: " + brokenFieldData);
                        }
                    } else if (status.equalsIgnoreCase("PENDING")) {
                        eConfirmData.setNotifyOpsTrackingBkrPending(dTradeID);
                        Logger.getLogger(this.getClass()).info(greatestDateTime + ": "
                                + tradingSys + " " + tradeID + " is now Broker PENDING");
                    } else if (status.equalsIgnoreCase("CANCELED")) {
                        eConfirmData.setNotifyOpsTrackingBkrCancelled(dTradeID);
                        Logger.getLogger(this.getClass()).info(greatestDateTime + ": " +
                                tradingSys + " " + tradeID + " is now Broker CANCELED");
                    } else if (status.equalsIgnoreCase("DISPUTED")) {
                        String subject = "Trade is now DISPUTED: " + tradingSys + " " + tradeID;
                        String mailDesc =
                                "A trade that was submitted to eConfirm is now under dispute. " +
                                "The status of the trade is still MATCHED and will be until it becomes CANCELED. " +
                                "\n  TradeId=" + tradeID +
                                "\n  Broker=" + brokerName +
                                "\n  BrokerRef=" + brokerTradeRefId +
                                "\n  Status=" + status +
                                "\n  StatusDate=" + statusDate +
                                "\n  --------------------------------------------------------------" +
                                "\n  eConfirmAPIUrl=" + eConfirmAPIUrl +
                                "\n  affinityDBInfoName=" + affinityDBInfoDisplayName +
                                "\n  opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                        //mailUtils.sendMail(disputedSendToAddress, sendToName, sentFromAddress, sentFromName,
                          //      subject, mailDesc, "");

                        //5/3/07 Israel - group-specific email
                        //sendMailByGroup(dTradeID, subject, mailDesc);

                        Logger.getLogger(this.getClass()).info(greatestDateTime + ": "
                                + tradingSys + " " + tradeID + " is Broker DISPUTED (status still MATCHED)");
                    } else {
                        Logger.getLogger(this.getClass()).info(greatestDateTime + ": " +
                                tradingSys + " " + tradeID + ": Status not found. Status=" + status);
                      //  throw new StopServiceException(tradingSys + " " + tradeID + ": Status not found. Status=" + status);
                    }

                    eConfirmSummaryDataRec.init();
                    eConfirmSummaryDataRec.tradingSystem = tradingSys;
                    eConfirmSummaryDataRec.tradeID = dTradeID;
                    eConfirmSummaryDataRec.bkrStatus = status;
                    eConfirmSummaryDataRec.errorFlag = errorFlag;
                    eConfirmSummaryDataRec.bkrTradeRefID = brokerTradeRefId;
                    if (eConfirmData.isECTradeSummaryExist(tradingSys, dTradeID)) {
                        eConfirmData.updateBkrECTradeSummary(eConfirmSummaryDataRec);
                    } else
                        eConfirmData.insertECTradeSummary(eConfirmSummaryDataRec);

                    greatestDateTime = getGreaterDateTime(greatestDateTime,statusDate);
                    statusRetry = 0;
                    
                }
            }
        } catch (Exception e) {
            try {
                opsTrackingConnection.rollback();
                Logger.getLogger(this.getClass()).error("In checkStatus for trade "+ tradeID + ": " +e);
            } catch (SQLException e1) {
                Logger.getLogger(this.getClass()).error(e1);
            }
          throw new StopServiceException("In checkStatus: "+e.getMessage());
        }
        return greatestDateTime;
    }

/*
    private void sendMailByGroup(double pTradeID, String pSubject, String pMailDesc)
            throws SQLException, MessagingException, UnsupportedEncodingException {
        String[] tradeData = new String[2];
        tradeData = getTradeData(pTradeID);
        String seCptySn = tradeData[SE_CPTY_SN];
        String cdtyCode = tradeData[CDTY_CODE];

        if (seCptySn.equalsIgnoreCase(NO_DATA) || cdtyCode.equalsIgnoreCase(NO_DATA))
            mailUtils.sendMail(confirmSupportSendToAddress, "ConfirmSupport", sentFromAddress, "NoSECptyOrCdtyData",
                    pSubject, pMailDesc, "");
        */
/*
        else if ((seCptySn.equalsIgnoreCase("SEMPRA EGY") || seCptySn.equalsIgnoreCase("RBS SET")) && cdtyCode.equalsIgnoreCase("ELEC"))
            mailUtils.sendMail(elecUSSendToAddress, elecUSSendToName, sentFromAddress, elecUSSentFromName,
                    pSubject, pMailDesc, "");
        else if ((seCptySn.equalsIgnoreCase("SEMPRA EGY") || seCptySn.equalsIgnoreCase("RBS SET") ) && cdtyCode.equalsIgnoreCase("NGAS"))
            mailUtils.sendMail(ngasUSSendToAddress, ngasUSSendToName, sentFromAddress, ngasUSSentFromName,
                    pSubject, pMailDesc, "");
        else if (( seCptySn.equalsIgnoreCase("SET EUROPE") || seCptySn.equalsIgnoreCase("RBS SEEL") ) && cdtyCode.equalsIgnoreCase("ELEC"))
            mailUtils.sendMail(elecUKSendToAddress, elecUKSendToName, sentFromAddress, elecUKSentFromName,
                    pSubject, pMailDesc, "");
        else if ( (seCptySn.equalsIgnoreCase("SET EUROPE") || seCptySn.equalsIgnoreCase("RBS SEEL") ) && cdtyCode.equalsIgnoreCase("NGAS"))
            mailUtils.sendMail(ngasUKSendToAddress, ngasUKSendToName, sentFromAddress, ngasUKSentFromName,
                    pSubject, pMailDesc, "");
        else if ( (seCptySn.equalsIgnoreCase("SGE USA") ||  seCptySn.equalsIgnoreCase("SGE CAN") ) && cdtyCode.equalsIgnoreCase("NGAS")) {
            mailUtils.sendMail(ngasSendToSGEAddress, ngasSendToSGEName, sentFromAddress, ngasSGEFromName,
                    pSubject, pMailDesc, "");
        }
        else if ( (seCptySn.equalsIgnoreCase("SGE USA") || seCptySn.equalsIgnoreCase("SGE CAN") ) && cdtyCode.equalsIgnoreCase("ELEC")) {
            mailUtils.sendMail(elecSendToSGEAddress, elecSendToSGEName, sentFromAddress, elecSGEFromName,
                    pSubject, pMailDesc, "");
        }
        else if (seCptySn.equalsIgnoreCase("SGE USA") || seCptySn.equalsIgnoreCase("SGE CAN")) {
            mailUtils.sendMail(oilSendToSGEAddress, oilSendToSGEName, sentFromAddress, oilSGEFromName,
                    pSubject, pMailDesc, "");            
        }
        *//*

        else {
            mailUtils.sendMail(oilSendToAddress, oilSendToName, sentFromAddress, oilSentFromName,
                    pSubject, pMailDesc, "");
        }
        
    }
*/

    private void callGetAlleged(String pECUserId, String pECPassword)
            throws SECptyNotFoundException, IOException, AuthenticateFailedException,
            JDOMException, TransformerException {
        final SimpleDateFormat sdfAllegedQuery = new SimpleDateFormat("MM/dd/yyyy", Locale.US);
        String endDate = "";
        String resultXML = "";

        Date currentDt = new Date();
        endDate =  sdf.format(currentDt);
        allegedQueryStartDate = "12/01/2008";
        resultXML = eConfirmAPI.queryAllegedTrades(allegedQueryStartDate, endDate, pECUserId, pECPassword);
        if (resultXML.matches("FAILURE"))
            Logger.getLogger(this.getClass()).info("AllegedQueryFailed: " + resultXML);

        File styleSheetURI = new File(allegedQueryXSL);
        StringReader xmlReader = new StringReader(resultXML);
        TransformerFactory tFactory = TransformerFactoryImpl.newInstance();
        StreamSource sourceXSL = new StreamSource(styleSheetURI);
        FileWriter writerHTML = new FileWriter(allegedQueryHTML);
        Transformer transformer = tFactory.newTransformer(sourceXSL);
        StreamSource sourceXML = new StreamSource(xmlReader);
        StreamResult resultHTML = new StreamResult(writerHTML);
        transformer.transform(sourceXML, resultHTML);

        //Releases OS file so it can be manipulated outside of the system.
        writerHTML.close();
    }

    //Israel 12/17/2014 - removed from service
/*    private void callEMAilAlleged() throws SQLException, Exception {
        *//**
     * --email notify gmt : gmt = EST + 5, EDT + 4
     * select to_char(sysdate,'HH24') from dual
     * select * from process_control
     * where process_mast_code = 'ECALLEGED'
     * and trunc(crtd_process_ts_gmt) = '31-mar-2006'
     *//*
        SimpleDateFormat sdfDayOfWeek = new SimpleDateFormat("E");
        Date currentDt = new Date();
        boolean isMailedYet = false;
        //Only execute on weekdays.
        if (!sdfDayOfWeek.format(currentDt).equalsIgnoreCase("SAT") &&
            !sdfDayOfWeek.format(currentDt).equalsIgnoreCase("SUN")) {
            int currentHour = -1;
            currentHour = getCurrentHour();
            int allegedHourTest = Integer.parseInt(allegedEMailSendAtHourGMT);
            //Adjust for GMT
            allegedHourTest -= 4;
            if (currentHour >= allegedHourTest) {
                //isMailedYet = processControlDAO.isRowForServiceCodeAndDate(EC_ALLEGED_EMAIL);
                if (!isMailedYet){
                    sendAllegedMail();
                    processControlDAO.insertProcessControl(EC_ALLEGED_EMAIL);
                    opsTrackingConnection.commit();
                }
            }
        }
    }*/

    /**
     * Does a type 4 query to check for errors. Only errors appear here.
     * @throws AuthenticateFailedException
     * @throws JDOMException
     * @throws IOException
     */
    public String checkMessageLog(String pMessageDateTime, String pECUserId, String pECPassword)
            throws AuthenticateFailedException, JDOMException, IOException, NamingException,
            SECptyNotFoundException, ParseException, StopServiceException {
        //currentPollDateTime = "2003-07-29 12:00:00";
        String greatestDateTime;
        boolean brokerBlockedExists = false;
        boolean ignoreBrokerBlockedMessage = false;

        try {
            String lastPollDate = "";
            lastPollDate = pMessageDateTime.substring(0, 10);
            String resultXML = "";
            resultXML = eConfirmAPI.queryMessageLog(lastPollDate, END_QUERY_DATE, pECUserId, pECPassword);
            //EConfirmMessageLogDataRec ecMesssageLogRec;
            //ecMesssageLogRec = new EConfirmMessageLogDataRec();

            SAXBuilder saxBuilder = new SAXBuilder();
            String tradingSys = "";
            String tradeID = "";
            double dTradeID = -1;
            String traceID = "";
            String statusDate = "";
            String messageDesc = "";
            String errorFlag = "Y";
            String messageType = "";
            greatestDateTime = pMessageDateTime;

            Calendar lastPollCal = Calendar.getInstance();
            Date dtlastPollCal = sdfDateTime.parse(pMessageDateTime);
            lastPollCal.setTime(dtlastPollCal);
            Document doc = null;
            doc = saxBuilder.build(new StringReader(resultXML));
            Element rootElem = doc.getRootElement();
            Element tradeInfoElem = rootElem.getChild("EConfirmMessageLogInfo");

            List nodes = tradeInfoElem.getChildren("EConfirmSimpleResponse");
            if (nodes != null) {
                for (Iterator i = nodes.iterator(); i.hasNext();) {
                    tradeID = "";
                    traceID = "";
                    statusDate = "";
                    Element elem = (Element) i.next();
                    tradeID = elem.getChildText("SenderTradeRefId");
                    traceID = elem.getChildText("TraceId");
                    messageDesc = "";
                    messageType = "";

/*
                    if (tradeID.trim().length() > 7)
                        tradingSys = "AFF";
                    else
                        tradingSys = "SYM";
*/

                    // Samy : skip JPM trades
/*
                        if (JPM_USER.equalsIgnoreCase(this.eConfirmProcessor.getEConfirmUserName())) {
                            if (!tradeID.startsWith("A") && !tradeID.startsWith("J")) {
                                continue;
                            }
                       }
*/
                    //Israel 1/5/2007 ICTS interface includes trd sys prefix.
                    String strippedTradeID = stripTradingSystemPrefix(tradeID);
                    try {
                        dTradeID = Double.parseDouble(strippedTradeID);
                        tradingSys = eConfirmData.getECTradeSummaryTradingSys(dTradeID);
                    }
                    catch (Exception e3){
                        Element messageElem = elem.getChild("Message");
                        statusDate = messageElem.getChildText("StatusDate");
                        Calendar statusDateCal = Calendar.getInstance();
                        Date dtStatusDateCal = sdfDateTime.parse(statusDate);
                        statusDateCal.setTime(dtStatusDateCal);
                        if (statusDateCal.after(lastPollCal)) {
                            String subject = "Invalid SenderTradeRefId: " + tradeID;
                            String mailDesc = messageDesc +
                                    ". TraceId=" + traceID +
                                    ", TradeId=" + tradeID +
                                    //", Code=" + messageElem.getChildText("Code") +
                                    //", Type=" + ecMesssageLogRec.messageType +
                                    ", StatusDate=" + statusDate +
                                    ", eConfirmAPIUrl=" + eConfirmAPIUrl +
                                    ", affinityDBInfoName=" + affinityDBInfoDisplayName +
                                    ", opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;
                            mailUtils.sendMail(confirmSupportSendToAddress, "ConfirmSupport", sentFromAddress, "Invalid SenderTradeRefId",
                                    subject, mailDesc, "");
                        }
                        continue;
                    }

                    Element messageElem = elem.getChild("Message");
                    statusDate = messageElem.getChildText("StatusDate");
                    Calendar statusDateCal = Calendar.getInstance();
                    Date dtStatusDateCal = sdfDateTime.parse(statusDate);
                    statusDateCal.setTime(dtStatusDateCal);
                    if (statusDateCal.after(lastPollCal)) {
                        ecMesssageLogRec.init();
                        ecMesssageLogRec.tradingSystem = tradingSys;
                        ecMesssageLogRec.tradeID = dTradeID;
                        ecMesssageLogRec.traceID = traceID;
                        ecMesssageLogRec.messageCode = messageElem.getChildText("Code");
                        ecMesssageLogRec.messageType = messageElem.getChildText("Type").toUpperCase();
                        messageDesc = messageElem.getChildText("Description");
                        ecMesssageLogRec.messageDesc = messageDesc;
                        ecMesssageLogRec.messageStatusDt = statusDate;

                        //Call this here to save processing cycles when message is not 2122.
                        if (ecMesssageLogRec.messageCode.equalsIgnoreCase("2122"))
                            //brokerBlockedExists = brokerBlockedIgnoreDAO.isBrokerExist(messageDesc);
                            brokerBlockedExists = eConfirmData.isBrokerExist(messageDesc);
                        ignoreBrokerBlockedMessage = ecMesssageLogRec.messageCode.equalsIgnoreCase("2148") || brokerBlockedExists;

                        //Israel 7/2/15 -- Implement support for handling non-ops_tracking messages.
                        boolean isTradingSysCodeExist = (tradingSys.length() > 0);
                        if (!ignoreBrokerBlockedMessage && isTradingSysCodeExist){
                            eConfirmData.insertECMessageLog(ecMesssageLogRec);
                            if (messageDesc != null && messageDesc.toLowerCase().indexOf("commission") <0 ) {
                                eConfirmData.setNotifyOpsTrackingError(dTradeID, messageDesc);
                            }
                            if (messageDesc != null && messageDesc.toLowerCase().indexOf("blocked") <0 ) {
                                eConfirmData.setNotifyOpsTrackingBkrError(dTradeID, messageDesc);
                            }

                        }
                        eConfirmSummaryDataRec.init();
                        eConfirmSummaryDataRec.tradingSystem = tradingSys;
                        eConfirmSummaryDataRec.tradeID = dTradeID;
                        eConfirmSummaryDataRec.status = "ERROR";
                        eConfirmSummaryDataRec.errorFlag = errorFlag;
                        //eConfirmSummaryDataRec.cmt = "";
                        eConfirmSummaryDataRec.cptyTradeRefID = "";

                        if (!ignoreBrokerBlockedMessage && isTradingSysCodeExist){
                            if (eConfirmData.isECTradeSummaryExist(tradingSys, dTradeID)) {
                                eConfirmData.updateECTradeSummary(eConfirmSummaryDataRec);
                            } else
                                eConfirmData.insertECTradeSummary(eConfirmSummaryDataRec);
                        }

                        //5/3/2007 Israel - automatically cancelling
                        if (messageDesc.indexOf("Trade to be cancelled") > -1 &&
                                messageDesc.indexOf("does not exist") > -1) {
                            eConfirmData.setNotifyOpsTrackingCancelled(dTradeID);
                            Logger.getLogger(this.getClass()).info(greatestDateTime + ": " +
                                    tradingSys + " " + tradeID + " is now CANCELED");
                        } else if (ignoreBrokerBlockedMessage) {
                            Logger.getLogger(this.getClass()).info(greatestDateTime + ": " +
                                    tradingSys + " " + tradeID + ", Message Code=" + ecMesssageLogRec.messageCode +
                                    ": Broker blocked error ignored.");
                        } else {
                            String subject = "Message Log Errror: " + tradingSys + " " + tradeID;
                            String mailDesc = messageDesc +
                                    ". TraceId=" + ecMesssageLogRec.traceID +
                                    ", TradeId=" + strippedTradeID +
                                    ", Code=" + ecMesssageLogRec.messageCode +
                                    ", Type=" + ecMesssageLogRec.messageType +
                                    ", StatusDate=" + statusDate +
                                    ", eConfirmAPIUrl=" + eConfirmAPIUrl +
                                    ", affinityDBInfoName=" + affinityDBInfoDisplayName +
                                    ", opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                            //5/3/07 Israel - group-specific email
                            // send email only if not commissin error
                             //if (messageDesc != null && messageDesc.toLowerCase().indexOf("commission") <0 ) {
                                //sendMailByGroup(dTradeID, subject, mailDesc);
                            }
                            //mailUtils.sendMail(sendToMessageLog, sendToName, sentFromAddress, sentFromName,
                              //      subject, mailDesc, "");
                        }
                        greatestDateTime = getGreaterDateTime(greatestDateTime, statusDate);
                        if (!ignoreBrokerBlockedMessage && isDisplayLogMessage )
                            Logger.getLogger(this.getClass()).info("Message Log: " +tradingSys + " " +
                                    tradeID + " :" + messageDesc);
                    }
                }
        } catch (Exception e) {
              try {
                opsTrackingConnection.rollback();
                Logger.getLogger(this.getClass()).error("In checkMessageLog: "+e);
            } catch (SQLException e1) {
                Logger.getLogger(this.getClass()).error(e1);
            }
          throw new StopServiceException("In checkMessageLog: " +e.getMessage());
        }
        return greatestDateTime;
    }


    private String getBrokenFieldData(String pTradeID, String pECUserId, String pECPassword)
            throws JDOMException, AuthenticateFailedException, IOException, SECptyNotFoundException, ParseException {
        String resultXML = "";
        resultXML = eConfirmAPI.queryBrokenFields(pTradeID, pECUserId, pECPassword);
        if (resultXML == null || resultXML.equalsIgnoreCase("")){
            return UNKNOWN_UNMATCHED;
        }

        SAXBuilder saxBuilder = new SAXBuilder();
        String status = "";
        String messageDesc = "";
        String field = "";
        String myValue = "";
        String cpValue = "";

        Document doc = null;
        doc = saxBuilder.build(new StringReader(resultXML));
        //doc = saxBuilder.build(new FileReader("H:\\EConfirm\\Test\\Type09_BrokenFields\\Test_02.xml"));
        Element rootElem = doc.getRootElement();
        Element tradeInfoElem = rootElem.getChild("EConfirmTradeInfo");

        List nodes = tradeInfoElem.getChildren("EConfirmSimpleResponse");
        if (nodes != null) {
            for (Iterator i = nodes.iterator(); i.hasNext();) {
                Element elem = (Element) i.next();
                status = elem.getChildText("Status");
                if (!status.equalsIgnoreCase("UNMATCHED"))
                    break;

 //               Element cptyBreakInfo = elem.getChild("CounterPartyBreakInfo");
   //             Element cpTrade = cptyBreakInfo.getChild("CPTrade");


                //null pointer exception on phony unmatched trades occurs here
                //IF 5/5/2004 - Catch the NPE and handle it gracefully.
                List breakNodes = null;
                try{
                    Element cptyBreakInfo = elem.getChild("CounterPartyBreakInfo");
                    Element cpTrade = cptyBreakInfo.getChild("CPTrade");

                    breakNodes = cpTrade.getChildren("Break");
                }
                catch (NullPointerException e){
                    return UNKNOWN_UNMATCHED;
                }

                messageDesc = "";
                if (breakNodes != null) {
                    for (Iterator iInner = breakNodes.iterator(); iInner.hasNext();) {
                        Element elemInner = (Element) iInner.next();
                        field = elemInner.getChildText("Field");
                        myValue = elemInner.getChildText("MyValue");
                        cpValue = elemInner.getChildText("CPValue");
                        if (messageDesc != "")
                            messageDesc = messageDesc + "; ";
                        messageDesc = messageDesc + field + ": " + myValue + " " + "{" + cpValue + "}";
                    }
                }
            }
        }
        return messageDesc;
    }

     private String getBrokerBrokenFieldData(String pTradeID, String pECUserId, String pECPassword)
            throws JDOMException, AuthenticateFailedException, IOException, SECptyNotFoundException, ParseException {
        String resultXML = "";
        resultXML = eConfirmAPI.queryBkrBrokenFields(pTradeID, pECUserId, pECPassword);
        if (resultXML == null || resultXML.equalsIgnoreCase("")){
            return UNKNOWN_UNMATCHED;
        }

        SAXBuilder saxBuilder = new SAXBuilder();
        String status = "";
        String messageDesc = "";
        String field = "";
        String myValue = "";
        String cpValue = "";

        Document doc = null;
        doc = saxBuilder.build(new StringReader(resultXML));
        //doc = saxBuilder.build(new FileReader("H:\\EConfirm\\Test\\Type09_BrokenFields\\Test_02.xml"));
        Element rootElem = doc.getRootElement();
        Element tradeInfoElem = rootElem.getChild("EConfirmTradeInfo");

        List nodes = tradeInfoElem.getChildren("EConfirmSimpleResponse");
        if (nodes != null) {
            for (Iterator i = nodes.iterator(); i.hasNext();) {
                Element elem = (Element) i.next();
                status = elem.getChildText("Status");
                if (!status.equalsIgnoreCase("UNMATCHED"))
                    break;

 //               Element cptyBreakInfo = elem.getChild("CounterPartyBreakInfo");
   //             Element cpTrade = cptyBreakInfo.getChild("CPTrade");


                //null pointer exception on phony unmatched trades occurs here
                //IF 5/5/2004 - Catch the NPE and handle it gracefully.
                List breakNodes = null;
                try{
                    Element cptyBreakInfo = elem.getChild("BrokerBreakInfo");
                    Element cpTrade = cptyBreakInfo.getChild("BrokerTrade");

                    breakNodes = cpTrade.getChildren("Break");
                }
                catch (NullPointerException e){
                    return UNKNOWN_UNMATCHED;
                }

                messageDesc = "";
                if (breakNodes != null) {
                    for (Iterator iInner = breakNodes.iterator(); iInner.hasNext();) {
                        Element elemInner = (Element) iInner.next();
                        field = elemInner.getChildText("Field");
                        myValue = elemInner.getChildText("MyValue");
                        cpValue = elemInner.getChildText("BrokerValue");
                        if (messageDesc != "")
                            messageDesc = messageDesc + "; ";
                        messageDesc = messageDesc + field + ": " + myValue + " " + "{" + cpValue + "}";
                    }
                }
            }
        }
        return messageDesc;
    }

    private String getBrokerSn(double pTradeId)
            throws SQLException {
        String brokerSn = NO_BROKER;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String selectSQL = "select BROKER_SN from ops_tracking.TRADE_DATA where TRADE_ID = ?";
            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                brokerSn = rs.getString("BROKER_SN");
            }

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
            try {
                if (rs != null) {
                    rs.close();
                    rs = null;
                }
            } catch (SQLException e) {
            }

        }
        return brokerSn;
    }

    /*private String getCdtyCode(double pTradeId)
            throws SQLException {
        String cdtyCode = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String selectSQL = "select CDTY_CODE from ops_tracking.TRADE_DATA where TRADE_ID = ?";
            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                cdtyCode = rs.getString("CDTY_CODE");
            }

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
            try {
                if (rs != null) {
                    rs.close();
                    rs = null;
                }
            } catch (SQLException e) {
            }

        }
        return cdtyCode;
    }*/

    private String[] getTradeData(double pTradeId)
            throws SQLException {
        String[] tradeDataResult = new String[2];
        tradeDataResult[SE_CPTY_SN] = NO_DATA;
        tradeDataResult[CDTY_CODE] = NO_DATA;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String selectSQL = "select SE_CPTY_SN, CDTY_CODE from ops_tracking.TRADE_DATA where TRADE_ID = ?";
            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                tradeDataResult[SE_CPTY_SN] = rs.getString("SE_CPTY_SN");
                tradeDataResult[CDTY_CODE] = rs.getString("CDTY_CODE");
            }

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
            try {
                if (rs != null) {
                    rs.close();
                    rs = null;
                }
            } catch (SQLException e) {
            }
        }
        return tradeDataResult;
    }

    public boolean isEConfRqmt(double pTradeId) throws SQLException {
        int rowsFound = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select count(*) count from ops_tracking.trade_rqmt where rqmt = 'ECONF' " +
                           "and trade_id = ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                rowsFound = (rs.getInt("count"));
            }
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }

        return rowsFound == 1;
    }

    public boolean isEConfBkrRqmt(double pTradeId) throws SQLException {
        int rowsFound = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select count(*) count from ops_tracking.trade_rqmt where rqmt = 'ECBKR' " +
                           "and trade_id = ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                rowsFound = (rs.getInt("count"));
            }
        } finally {

            try {
                if (rs != null) {
                    rs.close();
                    rs = null;
                }
                if (statement != null) {
                    statement.close();
                    statement = null;
                }

            }
            catch (Exception e){}
        }

        return rowsFound == 1;
    }
/*
    public void updateICTSFee(double pTradeID, String pCptyTradeId)
            throws SQLException {
        SybCallableStatement statement;
        int tradeId = (int) pTradeID;
        String callSqlStatement = "{call contractfeed.sp_econfirm_match(?, ?) }";
        statement = (SybCallableStatement) symphonyConnection.prepareCall(callSqlStatement);
        statement.setInt(1, tradeId);
        if (pCptyTradeId.length() > 20)
            statement.setString(2, pCptyTradeId.substring(1,20));
        else
            statement.setString(2, pCptyTradeId);
        statement.executeQuery();
        statement.close();
        statement = null;
    }

    public void deleteICTSFee(double pTradeID)
            throws SQLException {
        SybCallableStatement statement;
        int tradeId = (int) pTradeID;
        String callSqlStatement = "{call contractfeed.delete_econfirm_cost(?) }";
        statement = (SybCallableStatement) symphonyConnection.prepareCall(callSqlStatement);
        statement.setInt(1, tradeId);
        statement.executeQuery();
        statement.close();
        statement = null;
    }
  */
    private String stripTradingSystemPrefix(String pTradeId){
        String strippedTradeId = pTradeId;
        if (pTradeId.startsWith("A") ||
            pTradeId.startsWith("J"))
            strippedTradeId = pTradeId.substring(1,pTradeId.length());
        return strippedTradeId;
    }

     private void checkToReconnect() throws StopServiceException {

        java.util.Date dateNow = new java.util.Date();
        boolean reInitialize  = false;
        if (lastDbConnection == null) {
            reInitialize  = true;
        }
        else {
            calc.setTime(dateNow);
            int dayNow = calc.get(Calendar.DATE);
            calc.setTime(lastDbConnection);
            int lastRefreshDay = calc.get(Calendar.DATE);
            if (dayNow != lastRefreshDay){
                reInitialize = true;
            }
        }
        if  (reInitialize){
            Logger.getLogger(this.getClass()).info("The connection will be reinitialized");
            close();
            try {
                init();
            } catch (Exception e) {
                throw new StopServiceException(e.getMessage());
            }
        }
    }

    private void setDbDisplayNames() throws NamingException {
        DbInfoWrapper dbinfo = new DbInfoWrapper(affinityDBInfoName);
        affinityDBInfoDisplayName = dbinfo.getDatabaseName();
        dbinfo = new DbInfoWrapper(opsTrackingDBInfoName);
        opsTrackingDBInfoDisplayName = dbinfo.getDatabaseName();

    }

    public java.sql.Connection getOracleConnection(String pConnectionName, java.sql.Connection pConnection) throws
            SQLException, NamingException {
        if (pConnection == null) {
            pConnection = connectToOracle(pConnectionName);
        }
        return pConnection;
    }

    private java.sql.Connection connectToOracle(String pConnectionName) throws NamingException, SQLException {
        java.sql.Connection result = null;
        DbInfoWrapper dbinfo = new DbInfoWrapper(pConnectionName);
        result = DriverManager.getConnection(dbinfo.getDBUrl(), dbinfo.getDBUserName(), dbinfo.getDBPassword());
        Logger.getLogger(this.getClass()).info(pConnectionName+"="+dbinfo.getDatabaseName());
        result.setAutoCommit(false);
        return result;
    }

}
