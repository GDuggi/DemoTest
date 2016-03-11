/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:30:52 AM
 * To change template for new class use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.efetstatuspolling;

import aff.confirm.common.efet.dao.EFET_DAO;
import aff.confirm.common.efet.datarec.EFETErrorLog_DataRec;
import aff.confirm.common.efet.datarec.EFETTradeSummary_DataRec;
import org.jboss.logging.Logger;
import org.jdom.Document;
import org.jdom.Element;
import org.jdom.JDOMException;
import org.jdom.input.SAXBuilder;
import aff.confirm.common.efet.exceptions.FileNotMovedException;
import aff.confirm.common.efet.exceptions.DirectoryNotFoundException;
import aff.confirm.common.util.MailUtils;
import aff.confirm.common.dao.AppControlDAO;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskService;
import aff.confirm.jboss.common.util.DbInfoWrapper;

import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;
import javax.naming.NamingException;
import javax.mail.MessagingException;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Calendar;
import java.net.InetAddress;


@Startup
@Singleton
public class EFETStatusPollingService extends TaskService implements EFETStatusPollingServiceMBean {

    //private final SimpleDateFormat sdfEfet = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
    private final SimpleDateFormat sdfMatchedDate = new SimpleDateFormat("MM/dd/yyyy");
    private final SimpleDateFormat sdfEfetTime = new SimpleDateFormat("HHmmss");
    private DecimalFormat df = new DecimalFormat("#0");
    private String opsTrackingDBInfoName;
    private String affinityDBInfoName;
    private String opsTrackingDBInfoDisplayName;
    private String affinityDBInfoDisplayName;
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    private String efetInbox = "";
    private String efetFailed = "";
    private String efetProcessed = "";
    private EFET_DAO efetDAO = null;
    private MailUtils mailUtils;
    private String smtpHost;
    private String smtpPort;
    private String sentFromName;
    private String sentFromAddress;
    private String errorSendToAddress;
    private boolean isProduction;
    private String sendToName;
    //private QEFETTradeAlert qEFETTradeAlert;
     private java.util.Date lastDbConnection = null;
    private Calendar calc = Calendar.getInstance();

    private String environment;

    public EFETStatusPollingService() {
        super("affinity.cwf:service=EFETStatusPolling");
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

    public String getEFETBoxInbox() {
        return efetInbox;
    }

    public void setEFETBoxInbox(String pEFETBoxInbox) {
        efetInbox = pEFETBoxInbox;
    }

    public String getEFETBoxFailed() {
        return efetFailed;
    }

    public void setEFETBoxFailed(String pEFETBoxFailed) {
        efetFailed = pEFETBoxFailed;
    }

    public String getEFETBoxProcessed() {
        return efetProcessed;
    }

    public void setEFETBoxProcessed(String pEFETBoxProcessed) {
        efetProcessed = pEFETBoxProcessed;
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
        efetDAO = null;
        //qEFETTradeAlert = null;
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

    private void init() throws Exception {
        Logger.getLogger(this.getClass()).info("Executing init... ");
        //paused = true;

        Logger.getLogger(this.getClass()).info("Connecting opsTrackingConnection to " + opsTrackingDBInfoName + "...");
        opsTrackingConnection = getOracleConnection(opsTrackingDBInfoName, opsTrackingConnection);
        Logger.getLogger(this.getClass()).info("Connected opsTrackingConnection to " + opsTrackingDBInfoName + ".");

        Logger.getLogger(this.getClass()).info("Connecting affinityConnection to " + affinityDBInfoName + "...");
        affinityConnection = getOracleConnection(affinityDBInfoName, affinityConnection);
        Logger.getLogger(this.getClass()).info("Connected affinityConnection to " + affinityDBInfoName + ".");

        mailUtils = new MailUtils(smtpHost, smtpPort);

        String text = "";
        text = "Timer interval = " + (getTimerPeriod() / 1000) + " seconds.";
        Logger.getLogger(this.getClass()).info(text);

        setDbDisplayNames();
        if (environment.equalsIgnoreCase("PROD"))
            isProduction = true;

        if (!validateDirectory(efetInbox, false)){
            Logger.getLogger(this.getClass()).error("EFET Inbox: " + efetInbox + " NOT FOUND. Service NOT started.");
            throw new DirectoryNotFoundException("EFETStatusPollingService Init: Could not find Inbox directory: " + efetInbox);
        }
        Logger.getLogger(this.getClass()).info("EFET Inbox: " + efetInbox + ".");

        if (!validateDirectory(efetFailed, false)) {
            Logger.getLogger(this.getClass()).error("EFET Failed: " + efetFailed + " NOT FOUND. Service NOT started.");
            throw new DirectoryNotFoundException("EFETStatusPollingService Init: Could not find Failed directory: " + efetFailed);
        }
        Logger.getLogger(this.getClass()).info("EFET Failed: " + efetFailed + ".");

        if (!validateDirectory(efetProcessed, true)) {
            Logger.getLogger(this.getClass()).error("EFET Processed: " + efetProcessed + " NOT FOUND. Service NOT started.");
            throw new DirectoryNotFoundException("EFETStatusPollingService Init: Could not create Processed directory: " + efetProcessed);
        }
        Logger.getLogger(this.getClass()).info("EFET Processed: " + efetProcessed + ".");

        initMailVariables();
        efetDAO = new EFET_DAO(opsTrackingConnection, affinityConnection);
        //qEFETTradeAlert = new QEFETTradeAlert(opsTrackingConnection);

        lastDbConnection = new java.util.Date();
        Logger.getLogger(this.getClass()).info("Init OK.");
    }

    private void initMailVariables() throws Exception {
        String hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
        sentFromName = "EFET_Error_" + hostName;
        sentFromAddress = "JBossOn" + hostName + "@amphorainc.com";
        sendToName = "EFET_Error_Recipients";

        AppControlDAO appControlDAO;
        appControlDAO = new AppControlDAO(opsTrackingConnection,"EFET");
        errorSendToAddress = appControlDAO.getValue("EMAIL_ERROR_LIST");

        if (!isProduction){
            errorSendToAddress = "ifrankel@amphorainc.com";
        }

        Logger.getLogger(this.getClass()).info("errorSendToAddress="+errorSendToAddress);
    }

    private boolean validateDirectory(String pDir, boolean pCreateDir) throws Exception {
        boolean validateOK = false;
        File fileDir = new File(pDir);
        if (fileDir.exists())
            validateOK = true;
        else {
            if (pCreateDir) {
                fileDir.mkdirs();
                if (fileDir.exists())
                    validateOK = true;
            }
        }
        return validateOK;
    }

    private void setDbDisplayNames() throws NamingException {

        DbInfoWrapper dbinfo = new DbInfoWrapper(affinityDBInfoName);
        affinityDBInfoDisplayName = dbinfo.getDatabaseName();
        dbinfo = new DbInfoWrapper(opsTrackingDBInfoName);
        opsTrackingDBInfoDisplayName = dbinfo.getDatabaseName();
    }

    synchronized public void executeTimerEventNow() throws StopServiceException, LogException{
        //Logger.getLogger(this.getClass()).info("executeTimerEventNow() executing...");
        poll();
        //Logger.getLogger(this.getClass()).info("executeTimerEventNow() done.");
    }


    public String getEnv() {
        return environment;
    }

    @Override
    public void setEnv(String pEnv) {
        environment = pEnv;
    }

    protected void runTask() throws StopServiceException, LogException {
    //    checkToReconnect();
        poll();
    }

    synchronized private void poll() throws StopServiceException{
        try {
//            Logger.getLogger(this.getClass()).info("Executing polling task...");
            readInbox();
            readFailed();
//            Logger.getLogger(this.getClass()).info("Execute polling task done.");
        } catch (Exception e) {
            try {
                opsTrackingConnection.rollback();
                //Logger.getLogger(EConfirmStatusPollingService.class).error(e);
            } catch (Exception e1) {
                Logger.getLogger(this.getClass()).error(e1);
            }
            throw new StopServiceException(e.getMessage());
        }
    }

    private void readInbox()
            throws Exception, ParseException, SQLException, FileNotMovedException, IOException {
        EFETTradeSummary_DataRec efetTradeSummaryDataRec = new EFETTradeSummary_DataRec();
        File scannedDir = new File(efetInbox);
        SAXBuilder saxBuilder = new SAXBuilder();
        String documentID;
        String documentType;
        String documentVersion;
        String ebXMLMessageId;
        String state;
        String brokerState;
        String timestamp;
        String trdSysCode;
        String tradeId;
        double dTradeId;
        String entityType;
        Date matchedDate;
        String matchedDateStr;
        //boolean isCptyRqmt = false;
        //boolean isBkrRqmt = false;
        try {
            if (scannedDir.isDirectory()) {
                File[] scanFiles = scannedDir.listFiles();
                if (scanFiles != null) {
                    for (int i = 0; i < scanFiles.length; i++) {
                        File scanFile = scanFiles[i];
                        if (scanFile.isFile()) {
                            documentID = "";
                            documentType = "";
                            documentVersion = "";
                            ebXMLMessageId = "";
                            state = "";
                            brokerState = "";
                            timestamp = "";
                            trdSysCode = "";
                            tradeId = "";
                            dTradeId = 0;
                            entityType = "";
                            matchedDate = null;
                            matchedDateStr = "";
                            Document doc = null;
                            String reasonCode = "";
                            String reasonText = "";
                            //String errorSource = "";
                            doc = saxBuilder.build(new FileReader(scanFile));
                            Element rootElem = doc.getRootElement();
                            documentVersion = rootElem.getChildText("DocumentVersion");
                            documentType = rootElem.getChildText("DocumentType");
                            ebXMLMessageId = rootElem.getChildText("ebXMLMessageId");

                            if (rootElem.getChildText("State") != null)
                                state = rootElem.getChildText("State");
                            if (rootElem.getChildText("BrokerState") != null)
                                brokerState = rootElem.getChildText("BrokerState");

                            timestamp = rootElem.getChildText("Timestamp");
                            documentID = rootElem.getChildText("DocumentID");
                            trdSysCode = EFET_DAO.getTrdSysCode(documentID);

                            //If it's a cpty timeout get rid of it and go on
                            if (!trdSysCode.equalsIgnoreCase("AFF") &&
                                !trdSysCode.equalsIgnoreCase("SYM")){
                                Logger.getLogger(this.getClass()).info("DocId=" + documentID +
                                        ", Version=" + documentVersion + ", timestamp=" +timestamp +
                                        ", state=" + state);

                                String fileName = efetProcessed + "\\" + scanFile.getName();
                                File toFile = new File(fileName);
                                if (!scanFile.renameTo(toFile))
                                    throw new FileNotMovedException(scanFile.getName() + " could not be moved from " +
                                           efetInbox + " to " + efetProcessed + "." );
                                continue;
                            }

                            Element reasonElem = rootElem.getChild("Reason");
                            if (reasonElem != null) {
                                reasonCode = reasonElem.getChildText("ReasonCode");
                                reasonText = reasonElem.getChildText("ReasonText");
                                //errorSource = reasonElem.getChildText("ErrorSource");
                            }

                            tradeId = EFET_DAO.getTradeId(documentID, trdSysCode);
                            dTradeId = Double.parseDouble(tradeId);

                            entityType = getEntityType(documentType, dTradeId);
                            //isCptyRqmt = efetDAO.isTradeRqmtExist(dTradeId,"EFET" );
                            //isBkrRqmt = efetDAO.isTradeRqmtExist(dTradeId, "EFETB" );


                            efetTradeSummaryDataRec.init();
                            efetTradeSummaryDataRec = efetDAO.getEfetSummaryDataRec(dTradeId, entityType);
                            /*efetTradeSummaryDataRec.sourceSystemCode = trdSysCode;
                            efetTradeSummaryDataRec.tradeID = dTradeId;
                            //efetTradeSummaryDataRec.status = "ERROR";
                            efetTradeSummaryDataRec.errorFlag = "N";
                            efetTradeSummaryDataRec.entityType = entityType;*/

                            //certain failures leave this null(!)
                            if (state == null)
                                state = "FAILED";

                            if (state.equalsIgnoreCase("PENDING")){
                                efetDAO.setNotifyOpsTrackingCptyPending(dTradeId);
                                Logger.getLogger(this.getClass()).info(trdSysCode +
                                        " " + tradeId + " is now PENDING");
                            }
                            else if (state.equalsIgnoreCase("MATCHED")){
                                String cptyDocumentID = "";
                                //String cptyDocumentVersion = "";
                                String cptyTradeId = "";
                                Element cptyElem = rootElem.getChild("Counterparty");
                                if (cptyElem != null) {
                                    cptyDocumentID = cptyElem.getChildText("DocumentID");
                                    //cptyDocumentVersion = cptyElem.getChildText("DocumentVersion");
                                    if (EFET_DAO.getCptyDocId(cptyDocumentID).length() > 99)
                                        cptyTradeId = EFET_DAO.getCptyDocId(cptyDocumentID).substring(0,99);
                                    else
                                        cptyTradeId = EFET_DAO.getCptyDocId(cptyDocumentID);
                                    
                                    efetTradeSummaryDataRec.cptyTradeRefID = cptyTradeId;
                                }
                                matchedDate = EFET_DAO.sdfEfet.parse(timestamp);
                                matchedDateStr = sdfMatchedDate.format(matchedDate);
                                efetDAO.setNotifyOpsTrackingCptyMatched(dTradeId, cptyTradeId, matchedDateStr);
                                Logger.getLogger(this.getClass()).info(trdSysCode + " " +
                                        tradeId + " is now MATCHED");
                            }
                            else if (state.equalsIgnoreCase("AMENDED")){
                                //Nothing...
                                Logger.getLogger(this.getClass()).info(trdSysCode +
                                        " " + tradeId + " is now AMENDED");
                            }
                            else if (state.equalsIgnoreCase("VALID")){
                                efetDAO.setNotifyOpsTrackingBkrValid(dTradeId);
                               // efetDAO.setNotifyOpsTrackingBkrPending(dTradeId);
                                Logger.getLogger(this.getClass()).info(trdSysCode +
                                        " " + tradeId + " is now VALID");
                            }
                            else if (state.equalsIgnoreCase("CANCELLED")){
                                efetDAO.setNotifyOpsTrackingCptyCancelled(dTradeId);
                                Logger.getLogger(this.getClass()).info(trdSysCode +
                                        " " + tradeId + " is now CANCELLED");
                            }
                            else if (state.equalsIgnoreCase("FAILED")){
                                String mesg = "ReasonCode: " + reasonCode + ", ReasonText: " + reasonText;
                                efetDAO.setNotifyOpsTrackingCptyFail(dTradeId, "State: FAILED, " + mesg);

                                EFETErrorLog_DataRec efetErrorLogDataRec = new EFETErrorLog_DataRec(dTradeId,
                                        state, timestamp, reasonCode, reasonText, documentID,
                                        documentVersion, ebXMLMessageId, documentType);
                                efetDAO.insertEfetErrorLog(efetErrorLogDataRec);

                                String subject = "EFET Inbox: Failed: " + trdSysCode + " " + tradeId;
                                String mailDesc =
                                        " State=" + state +
                                        "\n trdSysCode=" + trdSysCode +
                                        "\n tradeId=" + tradeId +
                                        "\n efetStateTimestamp=" + timestamp +
                                        "\n reasonCode=" + reasonCode +
                                        "\n reasonText=" + reasonText +
                                        "\n documentId=" + documentID +
                                        "\n documentVersion=" + documentVersion +
                                        "\n ebXmlMessageId=" + ebXMLMessageId +
                                        "\n affinityDBInfoName=" + affinityDBInfoDisplayName +
                                        "\n opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                                mailUtils.sendMail(errorSendToAddress, sendToName, sentFromAddress, sentFromName,
                                    subject, mailDesc, "" );
                                Logger.getLogger(this.getClass()).info(trdSysCode +
                                        " " + tradeId + " is now FAILED");
                            }
                            else if (state.equalsIgnoreCase("TIMED_OUT") ||
                                     state.equalsIgnoreCase("NOT_SENT")){
                                String mesg = "ReasonCode: " + reasonCode + ", ReasonText: " + reasonText;
                                efetDAO.setNotifyOpsTrackingCptyFail(dTradeId, "State: " + state + ", " + mesg );

                                EFETErrorLog_DataRec efetErrorLogDataRec = new EFETErrorLog_DataRec(dTradeId,
                                        state, timestamp, reasonCode, reasonText, documentID,
                                        documentVersion, ebXMLMessageId, documentType);
                                efetDAO.insertEfetErrorLog(efetErrorLogDataRec);

                                Logger.getLogger(this.getClass()).info(trdSysCode +
                                        " " + tradeId + " is now FAILED");
                            }

                            //Broker is updated via BrokerState tag.
                            if (brokerState.equalsIgnoreCase("PENDING")){
                                if (documentType.equalsIgnoreCase("BFI")) { // update the status only if BFI document 
                                    efetDAO.setNotifyOpsTrackingBkrPending(dTradeId);
                                    Logger.getLogger(this.getClass()).info(trdSysCode +
                                            " " + tradeId + ": BrokerState is now PENDING");
                                }
                            }
                            else if (brokerState.equalsIgnoreCase("PRELIMINARY_MATCHED")){
                                efetDAO.setNotifyOpsTrackingBkrPrematched(dTradeId);
                                Logger.getLogger(this.getClass()).info(trdSysCode +
                                        " " + tradeId + ": BrokerState is now PRELIMINARY_MATCHED");
                            }
                            else if (brokerState.equalsIgnoreCase("TIMED_OUT")) {
                                String mesg = "ReasonCode: " + reasonCode + ", ReasonText: " + reasonText;
                                efetDAO.setNotifyOpsTrackingBkrFail(dTradeId, "State: " + brokerState + ", " + mesg );
                                Logger.getLogger(this.getClass()).info(trdSysCode +
                                        " " + tradeId + ": BrokerState is now TIMED_OUT");
                            }
                            else if (brokerState.equalsIgnoreCase("MATCHED")){
                                String cptyDocumentID = "";
                                //String cptyDocumentVersion = "";
                                String cptyTradeId = "";
                                Element cptyElem = rootElem.getChild("Broker");
                                if (cptyElem != null) {
                                    cptyDocumentID = cptyElem.getChildText("DocumentID");
                                    //cptyDocumentVersion = cptyElem.getChildText("DocumentVersion");
                                    if (EFET_DAO.getCptyDocId(cptyDocumentID).length() > 99)
                                        cptyTradeId = EFET_DAO.getCptyDocId(cptyDocumentID).substring(0,99);
                                    else
                                        cptyTradeId = EFET_DAO.getCptyDocId(cptyDocumentID);

                                    efetTradeSummaryDataRec.cptyTradeRefID = cptyTradeId;
                                }
                                matchedDate = EFET_DAO.sdfEfet.parse(timestamp);
                                matchedDateStr = sdfMatchedDate.format(matchedDate);
                                efetDAO.setNotifyOpsTrackingBkrMatched(dTradeId, cptyTradeId, matchedDateStr);
                                Logger.getLogger(this.getClass()).info(trdSysCode + " " +
                                        tradeId + ": BrokerState is now MATCHED");
                            }

                            if (state.equalsIgnoreCase("CANCELLED")) {
                                efetTradeSummaryDataRec.status = "CANCELED";
                            }
                            else {
                                efetTradeSummaryDataRec.status = state;
                            }

                            // broker status comes from CNF document type response
                            // if broker status value is set then, we need to update
                            // broker document record in the efet summary table
                            // instead of CNF document

                            if ( brokerState != null && !"".equals(brokerState) ) {
                                 if (brokerState.equalsIgnoreCase("CANCELLED")) {
                                    efetTradeSummaryDataRec.status = "CANCELED";
                                 }
                                 else {
                                    efetTradeSummaryDataRec.status = brokerState;
                                 }
                                efetTradeSummaryDataRec.entityType = "B"; // set to broker
                            }

                            //if (efetDAO.isEfetTradeSummaryExist(dTradeId,entityType))
                            efetDAO.updateEfetTradeSummary(efetTradeSummaryDataRec);
                            //else
                                //efetDAO.insertEfetTradeSummary(efetTradeSummaryDataRec);

                            String fileName = efetProcessed + "\\" + scanFile.getName();
                            File toFile = new File(fileName);
                            if (scanFile.renameTo(toFile))
                                opsTrackingConnection.commit();
                            else
                                throw new FileNotMovedException(scanFile.getName() + " could not be moved from " +
                                       efetInbox + " to " + efetProcessed + "." );
                        }
                    }
                }
            }
        } finally {
            scannedDir = null;
        }
    }

    private void readFailed()
            throws Exception, JDOMException, SQLException, MessagingException, FileNotMovedException {
        File scannedDir = new File(efetFailed);
        SAXBuilder saxBuilder = new SAXBuilder();
        EFETErrorLog_DataRec efetErrorLogDataRec = new EFETErrorLog_DataRec();
        EFETTradeSummary_DataRec efetTradeSummaryDataRec = new EFETTradeSummary_DataRec();
        String documentID;
        String documentVersion;
        String documentType;
        String ebXMLMessageId;
        String state;
        String efetTimestamp;
        String trdSysCode;
        String tradeId;
        double dTradeId;
        String entityType;
        String reasonCode;
        String reasonText;
        try {
            if (scannedDir.isDirectory()) {
                File[] scanFiles = scannedDir.listFiles();
                if (scanFiles != null) {
                    for (int i = 0; i < scanFiles.length; i++) {
                        File scanFile = scanFiles[i];
                        if (scanFile.isFile()) {
                            documentID = "";
                            documentVersion = "";
                            documentType = "";
                            ebXMLMessageId = "";
                            state = "";
                            efetTimestamp = "";
                            trdSysCode = "";
                            tradeId = "";
                            dTradeId = 0;
                            entityType = "";
                            reasonCode = "";
                            reasonText = "";
                            Document doc = null;
                            doc = saxBuilder.build(new FileReader(scanFile));
                            Element rootElem = doc.getRootElement();
                            documentID = rootElem.getChildText("DocumentID");
                            trdSysCode = EFET_DAO.getTrdSysCode(documentID);
                            tradeId = EFET_DAO.getTradeId(documentID, trdSysCode);
                            dTradeId = Double.parseDouble(tradeId);
                            documentVersion = rootElem.getChildText("DocumentVersion");
                            documentType = rootElem.getChildText("DocumentType");
                            if (documentType == null)
                                documentType = "CNF";
                            state = "ERROR";
                            //rootElem.getChildText("State");
                            /*ebXMLMessageId = rootElem.getChildText("ebXMLMessageId");
                            efetTimestamp = rootElem.getChildText("Timestamp");
                            Element reasonElem = rootElem.getChild("Reason");
                            if (reasonElem != null) {
                                reasonCode = reasonElem.getChildText("ReasonCode");
                                reasonText = reasonElem.getChildText("ReasonText");
                            }*/

                            reasonCode = "FAILED";
                            reasonText = "FailedBox: Failed";
                            String messageDesc = "FailedBox: Failed";
                            efetDAO.setNotifyOpsTrackingCptyError(dTradeId, messageDesc);

                            efetErrorLogDataRec.init();
                            efetErrorLogDataRec.tradeID = dTradeId;
                            efetErrorLogDataRec.efetState = state;
                            //efetErrorLogDataRec.efetTimestamp = efetTimestamp;
                            efetErrorLogDataRec.docId = documentID;
                            efetErrorLogDataRec.docVersion = documentVersion;
                            //efetErrorLogDataRec.ebXmlMessageId = ebXMLMessageId;
                            efetErrorLogDataRec.reasonCode = reasonCode;
                            efetErrorLogDataRec.reasonText = reasonText;
                            efetErrorLogDataRec.docType = documentType;
                            efetDAO.insertEfetErrorLog(efetErrorLogDataRec);

                            entityType = getEntityType(documentType,dTradeId);
                            efetTradeSummaryDataRec.init();
                            efetTradeSummaryDataRec.tradingSystem = trdSysCode;
                            efetTradeSummaryDataRec.tradeID = dTradeId;
                            efetTradeSummaryDataRec.status = "ERROR";
                            efetTradeSummaryDataRec.errorFlag = "Y";
                            efetTradeSummaryDataRec.cptyTradeRefID = "";
                            efetTradeSummaryDataRec.entityType = entityType;

                            //if (efetDAO.isEfetTradeSummaryExist(dTradeId,entityType))
                            efetDAO.updateEfetTradeSummary(efetTradeSummaryDataRec);
                            //else
                                //efetDAO.insertEfetTradeSummary(efetTradeSummaryDataRec);

                            if (state.equalsIgnoreCase("TIMED_OUT")){
                                Logger.getLogger(this.getClass()).info(trdSysCode +
                                        " " + tradeId + " is now TIMED_OUT");
                            }
                            else if (state.equalsIgnoreCase("FAILED")){
                                Logger.getLogger(this.getClass()).info(trdSysCode +
                                        " " + tradeId + " is now FAILED");
                            }

                            String subject = "EFET FailedBox: Failed: " + trdSysCode + " " + tradeId;
                            String mailDesc =
                                    " State=" + efetErrorLogDataRec.efetState +
                                    "\n trdSysCode=" + trdSysCode +
                                    "\n tradeId=" + tradeId +
                                    //"\n efetStateTimestamp=" + efetTimestamp +
                                    "\n reasonCode=" + efetErrorLogDataRec.reasonCode +
                                    "\n reasonText=" + efetErrorLogDataRec.reasonText +
                                    "\n documentId=" + efetErrorLogDataRec.docId +
                                    "\n documentVersion=" + efetErrorLogDataRec.docVersion +
                                    //"\n ebXmlMessageId=" + efetErrorLogDataRec.ebXmlMessageId +
                                    "\n affinityDBInfoName=" + affinityDBInfoDisplayName +
                                    "\n opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                            mailUtils.sendMail(errorSendToAddress, sendToName, sentFromAddress, sentFromName,
                                subject, mailDesc, "" );

                            Date now = new Date();
                            String fileNameNoExt = scanFile.getName().substring(0, scanFile.getName().length() - 4) +
                                    "_" + sdfEfetTime.format(now) + ".xml";
                            String fileName = efetProcessed + "\\" + fileNameNoExt;
                            File toFile = new File(fileName);
                            if (scanFile.renameTo(toFile))
                                opsTrackingConnection.commit();
                            else
                                throw new FileNotMovedException(scanFile.getName() + " could not be moved from " +
                                       efetFailed + " to " + fileName + "." );
                        }
                    }
                }
            } else
                log.info("not a directory");
        } finally {
            scannedDir = null;
        }
    }

    private String getEntityType(String documentType, double dTradeId) throws SQLException {
        String entityType;
        /**
         * There's no way of knowing what the entity type is from the BRES doc.
         * Also, make sure the rqmt you find isn't cancelled or about to be cancelled.
         */
        entityType = "";
        if (documentType.equalsIgnoreCase(EFET_DAO.BFI))
                entityType = "B";
        else if (documentType.equalsIgnoreCase(EFET_DAO.CNF)){
            if (efetDAO.isTradeRqmtExist(dTradeId,"EFET"))
                entityType = "C";
            else
                entityType = "";
        }
        return entityType;
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


    private void scan() {
    }



}
