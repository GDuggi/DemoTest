/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:30:52 AM
 * To change template for new class use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.efettradesubmitter;

import aff.confirm.common.efet.EFETFNCLProcessor;
import aff.confirm.common.efet.EFETProcessor;
import aff.confirm.common.efet.dao.EFET_DAO;
import aff.confirm.common.efet.datarec.EFETBFIXML_DataRec;
import aff.confirm.common.efet.datarec.EFETSubmitLog_DataRec;
import aff.confirm.common.efet.datarec.EFETTradeSummary_DataRec;
import aff.confirm.common.efet.datarec.EFETTrade_DataRec;
import aff.confirm.common.efet.exceptions.DirectoryNotFoundException;
import com.sun.rowset.CachedRowSetImpl;
import oracle.jdbc.OracleCallableStatement;
import org.jboss.logging.Logger;
import org.jdom.JDOMException;
import aff.confirm.common.dbqueue.QEFETTradeAlert;
import aff.confirm.common.dbqueue.QEFETTradeAlertRec;
import aff.confirm.common.ottradealert.ProcessControlDAO;
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
import javax.sql.rowset.CachedRowSet;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.sql.*;
import java.text.ParseException;
import java.net.InetAddress;
import java.net.UnknownHostException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.ArrayList;
import java.util.Locale;

@Startup
@Singleton
public class EFETTradeSubmitterService extends TaskService implements EFETTradeSubmitterServiceMBean {

    private static String _FNCL_STTL_CODE = "FNCL";
    private final int SENDER_ID = 0;
    private final int RECEIVER_ID = 1;
    private final SimpleDateFormat sdfSPDate = new SimpleDateFormat("MM/dd/yyyy", Locale.US);
    private EFET_DAO efetDAO;
    private String efetOutbox = "";
    private String efetWorkdir = "";
    private String opsTrackingDBInfoName;
    private String affinityDBInfoName;
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    private String affinityDBInfoDisplayName;
    private String opsTrackingDBInfoDisplayName;
    private QEFETTradeAlert qEFETTradeAlert;
    private QEFETTradeAlertRec qEFETTradeAlertRec;
    private EFETProcessor efetProcessor;
    private EFETFNCLProcessor efetFnclProcessor;
    private ProcessControlDAO processControlDAO;
    private String documentUsage;

    private String smtpHost;
    private String smtpPort;

    private String devEmailAddress = "NONE";
    private String prodEmailAddress = "NONE";
    private AppControlDAO appControlDAO ;

    private java.util.Date lastDbConnection = null;
    private Calendar calc = Calendar.getInstance();

    private String environment;

    //private EFETSubmitLogDataRec efetSubmitLogDataRec;

    public EFETTradeSubmitterService() {
        super("affinity.cwf:service=EFETTradeSubmitter");
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

    public String getEFETBoxOutbox() {
        return efetOutbox;
    }

    public void setEFETBoxOutbox(String pEFETBoxOutbox) {
        efetOutbox = pEFETBoxOutbox;
    }

    public String getDocumentUsage() {
        return documentUsage;
    }

    public void setDocumentUsage(String pDocumentUsage) {
        documentUsage = pDocumentUsage;
    }

    public String getEFETWorkDir() {
        return efetWorkdir;
    }

    public void setEFETWorkDir(String pEFETWorkDir) {
        efetWorkdir = pEFETWorkDir;
    }

    private void init() throws Exception {
        log.info("Executing init... ");

        log.info("Connecting opsTrackingConnection to " + opsTrackingDBInfoName + "...");
        opsTrackingConnection = getOracleConnection(opsTrackingDBInfoName, opsTrackingConnection);
        log.info("Connected opsTrackingConnection to " + opsTrackingDBInfoName + ".");

        log.info("Connecting affinityConnection to " + affinityDBInfoName + "...");
        affinityConnection = getOracleConnection(affinityDBInfoName, affinityConnection);
        log.info("Connected affinityConnection to " + affinityDBInfoName + ".");
        
        setDbDisplayNames();
        log.info("opsTrackingConnection = " + opsTrackingDBInfoDisplayName);
        log.info("affinityConnection = " + affinityDBInfoDisplayName);

        if (!validateDirectory(efetWorkdir, true)){
            log.error("Workdir: " + efetWorkdir + " NOT FOUND. Service not started.");
            throw new DirectoryNotFoundException("EFETTradeSubmitterService Init: Could not find Workdir directory: " + efetWorkdir);
        }
        log.info("Workdir: " + efetWorkdir + ".");

        if (!validateDirectory(efetOutbox, false)){
            log.error("Outbox: " + efetOutbox + " NOT FOUND. Service not started.");
            throw new DirectoryNotFoundException("EFETTradeSubmitterService Init: Could not find Outbox directory: " + efetOutbox);
        }
        log.info("EFET Outbox: " + efetOutbox + ".");

        log.info("DocumentUsage = " + documentUsage);

        String text = "";
        text = "Timer interval = " + (getTimerPeriod() / 1000) + " seconds.";
        log.info(text);

        efetDAO = new EFET_DAO(opsTrackingConnection, affinityConnection);
        efetProcessor = new EFETProcessor(affinityConnection, opsTrackingConnection, documentUsage,opsTrackingDBInfoDisplayName);
        efetFnclProcessor = new EFETFNCLProcessor(affinityConnection, opsTrackingConnection, documentUsage,opsTrackingDBInfoDisplayName);

        processControlDAO = new ProcessControlDAO(opsTrackingConnection);

        qEFETTradeAlert = new QEFETTradeAlert(opsTrackingConnection);
        loadMailSettings();
        lastDbConnection = new java.util.Date();
        log.info("EFETTradeSubmitterService Started.");
    }



    private void loadMailSettings(){


        try {
            appControlDAO = new AppControlDAO(this.affinityConnection,"EFET");
            devEmailAddress = appControlDAO.getValue("DEV_EMAIL_ADDR");
            prodEmailAddress    = appControlDAO.getValue("PROD_EMAIL_ADDR");
        } catch (Exception e) {
            log.error("Error Loading email list " , e );
        }

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

    protected void onServiceStarting() throws Exception {
        log.info("Executing startService... ");
        init();
    }

    protected void onServiceStoping() {
        try {
            close();
        } catch (Exception e) {
            log.error(e);
        }
    }

    protected void close() {
        qEFETTradeAlert = null;
        efetProcessor = null;
        efetFnclProcessor  = null;
        efetDAO = null;
        processControlDAO = null;


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
        result = DriverManager.getConnection(dbinfo.getDBUrl().toLowerCase(), dbinfo.getDBUserName(), dbinfo.getDBPassword());
        log.info(pConnectionName+"="+dbinfo.getDatabaseName());
        result.setAutoCommit(false);
        return result;
    }


    private void setDbDisplayNames() throws NamingException {

        DbInfoWrapper dbinfo = new DbInfoWrapper(affinityDBInfoName);
        affinityDBInfoDisplayName = dbinfo.getDatabaseName();
        dbinfo = new DbInfoWrapper(opsTrackingDBInfoName);
        opsTrackingDBInfoDisplayName = dbinfo.getDatabaseName();

    }

    synchronized public void executeTimerEventNow() throws StopServiceException{
        log.info("executeTimerEventNow() executing...");
        poll();
        log.info("executeTimerEventNow() done.");
    }

    public String getSmtpHost() {
        return smtpHost;
    }

    public void setSmtpHost(String pSMTPHost) {
        smtpHost = pSMTPHost;
    }

    public String getSmtpPort() {
        return smtpPort;
    }

    public void setSmtpPort(String pSMTPPort) {
       smtpPort = pSMTPPort;
    }

    public String getEnv() {
        return environment;
    }


    public void setEnv(String pEnv) {
        environment = pEnv;
    }

    protected void runTask() throws StopServiceException, LogException {
        checkToReconnect();
        poll();
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
            log.info("The connection will be reinitialized");
            close();
            try {
                init();
            } catch (Exception e) {
                throw new StopServiceException(e.getMessage());
            }
        }
    }

    synchronized private void poll() throws StopServiceException {
        try {
//            log.info("Executing EFETTradeSubmitter task...");
            if (processControlDAO.isReadyToProcess("EFETSUB")){
                processCancelEFETTrades();
                processEFETTradeAlertRecords(false);
                processControlDAO.updateAlertRecord("EFETSUB","Y");
                opsTrackingConnection.commit();
            }
            else {
                processCancelEFETTrades();
                processEFETTradeAlertRecords(true);
            }
//            log.info("Execute EFETTradeSubmitter task done.");

        } catch (Exception e) {
            log.error(e);
            try {
                opsTrackingConnection.rollback();
            } catch (Exception e1) {
                log.error(e1);
            }
            String msg =e.getMessage();
            if (msg == null){
                msg = "";
            }
            throw new StopServiceException(msg);
        }
    }

    private void processCancelEFETTrades() throws SQLException {
    // At submission time, sometime we have both submit and cancel request together
    // and cancel reqeust submitted to the box without the original trade submitted to the
    //box and submitting the trade and immediately cancelling trade in the trade.
    // This functions marked those cancel and submit request as "X".

       String sql = "SELECT * from JBOSSDBQ.Q_EFET_TRADE_ALERT " +
                            "where PROCESSED_FLAG = 'N' and EFET_ACTION = 'CANCEL' ";

       String updateSQl = "update JBOSSDBQ.Q_EFET_TRADE_ALERT set PROCESSED_FLAG = 'X' " +
                          " where TRADE_ID = ? AND PROCESSED_FLAG = 'N' ";
       Statement cs = null;
        PreparedStatement ps = null;
        ResultSet rs = null;
        try {
            cs = opsTrackingConnection.createStatement();
            rs = cs.executeQuery(sql);

            double tradeId = 0;
            String trdSys;
            String docType;
            ps = opsTrackingConnection.prepareStatement(updateSQl);

            while (rs.next()) {
                tradeId = rs.getDouble("TRADE_ID");
                trdSys = rs.getString("TRADING_SYSTEM");
                docType = rs.getString("DOC_TYPE");
                log.info("Processing Pre Cancellation: " + trdSys + " " + tradeId);
                String documentId = efetDAO.getEfetDocumentId(tradeId,docType);
                if (documentId == null || "NONE".equalsIgnoreCase(documentId)){
                    ps.setDouble(1,tradeId);
                    ps.executeUpdate();
                    efetDAO.setNotifyOpsTrackingCptyCancelled(tradeId);
                    log.info("Cancelled thru preprocessing: " + trdSys + " " + tradeId);
                    opsTrackingConnection.commit();
                }
            }
        }
        finally {
            if (rs != null){
                try {
                    rs.close();
                    cs.close();
                    rs = null;
                    cs=null;
                }
                catch (SQLException e){}
            }

            if ( ps != null) {
                try {
                    ps.close();
                    ps = null;
                }
                catch (SQLException e){}
            }
        }
    }
    private void processEFETTradeAlertRecords(boolean reSubmitOnly)
            throws JDOMException, IOException, Exception {
        String trdSys = "";
        String ticketID = "";
        String submitXML = "";
        String documentId;
        int documentVersion = 0;
        String workFileName;
        String destFileName;
        String initialStatus;
        String errorFlag;
        double tradeID = -1;
        int id = 0;
        String seCptySn = "";
        String efetAction = "";
        String efetSubmitState = "";
        String docType = "";
        String receiverType = "";
        String entityType = "";
        String[] summaryArgs = new String[2];
        EFETTradeSummary_DataRec efetTradeSummaryDataRec;
        efetTradeSummaryDataRec = new EFETTradeSummary_DataRec();
        EFETBFIXML_DataRec efetBFIXMLDataRec;
        efetBFIXMLDataRec = new EFETBFIXML_DataRec();

        CachedRowSet crs;
        crs = new CachedRowSetImpl();
        if (reSubmitOnly){
            crs = qEFETTradeAlert.getReadyForReSubmitOrCancel();
        }else{
            crs = qEFETTradeAlert.getReadyToProcess();
        }
        crs.beforeFirst();
        // array list to add all the bad trades
        ArrayList tradeList = new ArrayList();
        int numberDocs = 0;
        String errList = "";
        while (crs.next()) {
            trdSys = crs.getString("TRADING_SYSTEM");
            tradeID = crs.getDouble("TRADE_ID");
            ticketID = df.format(tradeID);
            efetAction = crs.getString("EFET_ACTION");
            efetSubmitState = crs.getString("EFET_SUBMIT_STATE");
            seCptySn = crs.getString("SE_CPTY_SN");
            docType = crs.getString("DOC_TYPE");
            receiverType = crs.getString("RECEIVER_TYPE");

            EFETSubmitLog_DataRec efetSubmitLogDataRec;
            initialStatus = "";
            errorFlag = "";
            submitXML = "";
            documentId = "";
            workFileName = "";
            destFileName = "";
            documentVersion = 0;
            summaryArgs[SENDER_ID] = "";
            summaryArgs[RECEIVER_ID] = "";
            entityType = "";

            try {
                /*cnfReceiverRole = "C";
                if (docType.equalsIgnoreCase(EFETDAO.CNF) &&
                    receiverType.equalsIgnoreCase("B"))
                    cnfReceiverRole = "B";*/

                    /*
                        Check for the trades whether they are already matched in the
                        database.
                */
                id = crs.getInt("ID");
                // check the current status of the record

                if ( !readyToProcess(id) ){
                    continue;
                }

                if ( isTradeMatched(tradeID,docType,receiverType)) {

                    qEFETTradeAlert.updateAlertRecord(id, "Y");
                    opsTrackingConnection.commit();
                    continue;
                }
                //EC_ACTION = SUBMIT
                if (efetAction.equalsIgnoreCase("SUBMIT")) {
                    log.info("Submitting: " + trdSys + " " + ticketID +
                            " " + docType + " " + receiverType + "...");

                    if ( hasProblem(tradeList,tradeID) ){
                        log.info("Has Problem: " + trdSys + " " + ticketID +
                            " " + docType + " " + receiverType + "...Skipped");
                        continue;
                    }
                    //If this was re-submitted it doesn't have a SECpty
                    if (seCptySn == null){
                        EFETTrade_DataRec efetTradeDataRec = new EFETTrade_DataRec();
                        efetTradeDataRec = efetDAO.getEFETTradeDataRec(tradeID);
                        seCptySn = efetTradeDataRec.companySN;
                    }

                    if (efetSubmitState.equalsIgnoreCase("NEW")){
                        documentId = efetDAO.getDocumentID(docType,trdSys,tradeID,seCptySn);
                        documentVersion = 1;
                    }
                    else if (efetSubmitState.equalsIgnoreCase("EDIT")){
                        documentId = efetDAO.getEfetDocumentId(tradeID, docType);
                        if (documentId == null)
                            documentId = efetDAO.getDocumentID(docType, trdSys,tradeID,seCptySn);
                        //Israel - 12/27/2006 -- Fixes doc version synchronization problem.
                        //Israel - 2/12/2007 -- switched to more reliable tradeconfirmation, backing up with efet.document.
                        // samy: 09/18/2009 removed the code to get trade document version from EFET_BOX schema
                        // it is not needed anymore and reduces one more database connection in
                        // dbinfo service as per israel
                        /*
                        documentVersion = efetDAO.getTradeConfirmationDocumentVersion(documentId);
                        if (documentVersion > 0)
                            documentVersion += 1;
                        else
                         */
                        documentVersion = efetDAO.getEfetDocumentVersion(tradeID, docType) + 1;
                    }
                    else {
                        errList = errList + "\n" + "Invalid SubmitState=" + efetSubmitState + ", TradeId="+ticketID ;
                      //  sendEmail("EFET Trade Submitter: processEFETTradeAlertRecords","Invalid SubmitState=" + efetSubmitState + ", TradeId="+ticketID );
                        log.error("Unknown efetSubmitState="+efetSubmitState+ ",TradeId="+ticketID);
                        qEFETTradeAlert.updateAlertRecordByTradeId(tradeID,"I");
                        opsTrackingConnection.commit();
                        continue;
                       // throw new StopServiceException("Unknown efetSubmitState="+efetSubmitState);
                    }

                    if (docType.equalsIgnoreCase(EFET_DAO.CNF))  {

                        if (isFNCLTrade(tradeID)) { // added the check for the swap trades.
                           submitXML = efetFnclProcessor.getCNFXML(tradeID, documentId, documentVersion, receiverType, summaryArgs );
                           if (efetFnclProcessor.isEicMissing()){ // do we have eic code missing for the trade
                               errList = errList + "\n" + "EFET Trade Submitter Error : EIC code missing for the trade = " + ticketID + ". Pls update the eic code and resubmit the trade.";
                           }
                        }
                        else {
                            submitXML = efetProcessor.getCNFXML(tradeID, documentId, documentVersion, receiverType, summaryArgs );
                            if (efetProcessor.isEicMissing()){ // do we have eic code missing for the trade
                                errList = errList + "\n" + "EFET Trade Submitter Error : EIC code missing for the trade = " + ticketID + ". Pls update the eic code and resubmit the trade.";
                            }
                        }
                    }
                    else if (docType.equalsIgnoreCase(EFET_DAO.BFI)){
                        String linkedToDocId = "";
                        linkedToDocId = efetDAO.getEfetDocumentId(tradeID,EFET_DAO.CNF);
                        if (linkedToDocId.equalsIgnoreCase("NONE")) {
                           errList = errList + "\n" + "EFET Trade Submitter Error : processEFETTradeAlertRecords : Null LinkedToDocID; tradeId="+ticketID;
                          // sendEmail("EFET Trade Submitter: processEFETTradeAlertRecords","Null LinkedToDocID;\n" +"tradeId="+ticketID);
                            log.error("processEFETTradeAlertRecords: Null CNF LinkedToDocID; tradeId="+ticketID);
                            qEFETTradeAlert.updateAlertRecordByTradeId(tradeID,"I");
                            opsTrackingConnection.commit();
                            continue;
                           // throw new StopServiceException("processEFETTradeAlertRecords: Null CNF LinkedToDocID; tradeId="+ticketID);
                        }
                        //efetBFIXMLDataRec = getEFETXMLDataRec(tradeID, documentId, documentVersion,linkedToDocId);
                        efetBFIXMLDataRec =  efetProcessor.getEfetBFIDataRec(tradeID, documentId, documentVersion, linkedToDocId);

                        submitXML = efetProcessor.getBFIXML(efetBFIXMLDataRec);
                        summaryArgs[SENDER_ID] = efetBFIXMLDataRec.senderId;
                        summaryArgs[RECEIVER_ID] = efetBFIXMLDataRec.receiverId;
                    }
                    else{
                         errList = errList + "\n" + "EFET Trade Submitter: Invalid DocType=" + docType + ", TradeId="+ticketID;
                        // sendEmail("EFET Trade Submitter: processEFETTradeAlertRecords","Invalid DocType=" + docType + ", TradeId="+ticketID);
                         log.error("Invalid DocType=" + docType + ", TradeId="+ticketID);
                         qEFETTradeAlert.updateAlertRecordByTradeId(tradeID,"I");
                         opsTrackingConnection.commit();
                         continue;
                        //throw new StopServiceException("Invalid DocType=" + docType + ", TradeId="+ticketID);
                    }

                    // not to stop the service
                    if (submitXML.equalsIgnoreCase("XML_DATA_ROW_NOT_FOUND")){
                        //notify user
                        errList = errList + "\n" + "EFET Trade Submitter Error : Data not found for the trade = " + ticketID + ".\nPossibile reason: EIC Code missing for the cpty.";
                        //  sendEmail("EFET Trade Submitter Error : Data not found","Data not found for the trade = " + ticketID + ".\nPossibile reason: EIC Code missing for the cpty.");
                       log.error("infinity_mgr.v_efet_xml_data row not found: tradeId="+ticketID);
                        qEFETTradeAlert.updateAlertRecordByTradeId(tradeID, "I");
                        double rqmtId = getEFETRqmtId(tradeID);
                        java.sql.Date updateDt = new java.sql.Date(System.currentTimeMillis());
                        updateTradeRqmt(rqmtId, "ERROR", updateDt, "Xml was not available. Queue item for trade has been deferred. Fix problem then resubmit.");
                        opsTrackingConnection.commit();
                        continue;
                     //   throw new StopServiceException("infinity_mgr.v_efet_xml_data row not found: tradeId="+ticketID);
                    }

                    initialStatus = "SUBMITTED";
                    errorFlag = "N";
                    efetSubmitLogDataRec = new EFETSubmitLog_DataRec(trdSys, tradeID, initialStatus, efetAction, docType);
                    int submitID = -1;
                    submitID = efetDAO.insertEfetSubmitLog(efetSubmitLogDataRec);

                    //Records in EFETTradeSummary are used to track rqmt status-- Broker/BFI and Cpty/CNF.
                    //If it's Broker/CNF we ignore it.
                    if (docType.equalsIgnoreCase(EFET_DAO.BFI) ||
                        docType.equalsIgnoreCase(EFET_DAO.CNF) && receiverType.equalsIgnoreCase("C")){
                        efetTradeSummaryDataRec = new EFETTradeSummary_DataRec(trdSys, tradeID, documentId,
                                                                        documentVersion, initialStatus, "", "", errorFlag,
                                                                        summaryArgs[SENDER_ID], summaryArgs[RECEIVER_ID],
                                                                        receiverType);

                        if (!efetDAO.isEfetTradeSummaryExist(tradeID, receiverType))
                            efetDAO.insertEfetTradeSummary(efetTradeSummaryDataRec);
                        else
                            efetDAO.updateEfetTradeSummary(tradeID,initialStatus,errorFlag,
                                                            summaryArgs[SENDER_ID], summaryArgs[RECEIVER_ID],receiverType);
                    }

                    //Check for unknown doc types but only update based on existing rqmts
                    //(i.e., exclude broker/CNF)
                    if (docType.equalsIgnoreCase(EFET_DAO.CNF)){
                        if (receiverType.equalsIgnoreCase("C"))
                            efetDAO.setNotifyOpsTrackingCptySubmit(tradeID);
                    }
                    else if (docType.equalsIgnoreCase(EFET_DAO.BFI))
                        efetDAO.setNotifyOpsTrackingBkrSubmit(tradeID);
                    else{
                        //throw new StopServiceException("processEFETTradeAlertRecords: Invalid DocType=" + docType + ", TradeId="+ticketID);
                         errList = errList + "EFET Trade Submitter:Invalid DocType=" + docType + ", TradeId="+ticketID;
                       // sendEmail("EFET Trade Submitter: processEFETTradeAlertRecords","Invalid DocType=" + docType + ", TradeId="+ticketID );
                        log.error("processEFETTradeAlertRecords: Invalid DocType=" + docType + ", TradeId="+ticketID);
                        qEFETTradeAlert.updateAlertRecordByTradeId(tradeID,"I");
                        opsTrackingConnection.commit();
                        continue;
                    }

                    //insert the doc id into DOCUMENT table
                    if (efetDAO.isEfetDocumentExist(tradeID, docType))
                        efetDAO.updateEfetDocumentVersion(tradeID, docType, documentVersion);
                    else
                        efetDAO.insertEfetDocument(tradeID, docType, documentId, documentVersion);

                    //It is necessary to update the database before creating the file,
                    //since the database update can be rolled back while file creation can't
                    //Create file in work dir. Once created move it.
                    workFileName = efetWorkdir + "\\" + documentId + ".xml";
                    saveAsTextFile(workFileName, submitXML);

                    //Move to efetbox outbox
                    destFileName = efetOutbox + "\\" + documentId + ".xml";
                    moveFile(workFileName,destFileName);

                    log.info("EFET_SUBMIT_LOG: " + trdSys + " " + ticketID +
                            " " + docType + " " + receiverType + " :" + initialStatus);
                } else if (efetAction.equalsIgnoreCase("CANCEL")) {
                    log.info("Cancelling: " + trdSys + " " + ticketID + "...");

                    //Get data from Efet Trade summary
                    efetTradeSummaryDataRec.init();
                    efetTradeSummaryDataRec = efetDAO.getEfetSummaryDataRec(tradeID, receiverType);
                    efetTradeSummaryDataRec.documentId = efetDAO.getEfetDocumentId(tradeID,docType);
                    //Israel - 2/6/2007 -- Get current doc version which is used as cancel doc version.
                    //efetTradeSummaryDataRec.documentVersion = efetDAO.getEfetDocumentVersion(tradeID,docType);
                    //Israel 2/12/2007 -- switched to more reliable tradeconfirmation, backing up with efet.document.
                  //  System.out.println("Passing document id to efet schema = " + efetTradeSummaryDataRec.documentId);
                 //   efetTradeSummaryDataRec.documentVersion = efetDAO.getTradeConfirmationDocumentVersion(efetTradeSummaryDataRec.documentId);
                 //   if (efetTradeSummaryDataRec.documentVersion == 0) {
                        documentVersion = efetDAO.getEfetDocumentVersion(tradeID, docType);
                        efetTradeSummaryDataRec.documentVersion = documentVersion;
                //    }

                    String canDocumentId = efetDAO.getDocumentID(EFET_DAO.CAN,trdSys,tradeID,seCptySn);
                    //int canDocumentVersion = 1;

                    try {
                        submitXML = efetProcessor.getCANXML(canDocumentId, efetTradeSummaryDataRec, receiverType);
                    } catch (NullPointerException e) {
                        log.error("efetProcessor.getCancelXML failed for tradeId=" + ticketID +
                                ". Add missing SENDER_ID, RECEIVER_ID to efet.efet_trade_summary then restart service.");
                        qEFETTradeAlert.updateAlertRecordByTradeId(tradeID,"I");
                        opsTrackingConnection.commit();
                        continue;
                       // throw new StopServiceException("efetProcessor.getCancelXML failed for tradeId=" + ticketID +
                       //         ". Add missing SENDER_ID, RECEIVER_ID to efet.efet_trade_summary then restart service.");
                    }

                    if (submitXML.equalsIgnoreCase("XML_DATA_ROW_NOT_FOUND")) {
                        log.error("infinity_mgr.v_efet_xml_data row not found: tradeId="+ticketID);
                        qEFETTradeAlert.updateAlertRecordByTradeId(tradeID,"I");
                        opsTrackingConnection.commit();
                        continue;
                       // throw new StopServiceException("infinity_mgr.v_efet_xml_data row not found: tradeId="+ticketID);
                    }


                    workFileName = efetWorkdir + "\\" + canDocumentId + ".xml";
                    saveAsTextFile(workFileName, submitXML);

                    //Move to efetbox outbox
                    destFileName = efetOutbox + "\\" + canDocumentId + ".xml";
                    moveFile(workFileName,destFileName);

                    efetSubmitLogDataRec = new EFETSubmitLog_DataRec(trdSys, tradeID, initialStatus, efetAction, docType);
                    int submitID = -1;
                    submitID = efetDAO.insertEfetSubmitLog(efetSubmitLogDataRec);

                    log.info("Cancelled: " + trdSys + " " + ticketID);
                } else
                    log.info("Unknown efetAction=" + efetAction);

                id = crs.getInt("ID");
                qEFETTradeAlert.updateAlertRecord(id,"Y");
                numberDocs++;
                opsTrackingConnection.commit();
            }
            catch ( Exception e) {
                opsTrackingConnection.rollback();
                addToList(tradeList,tradeID);
                log.error("Error=" , e );
                 errList = errList + "\n" + "EFET Trade Submitter: Runtime Error=" + e.getMessage() + ", TradeId="+ticketID;
            }

        }
        notifyProcess(errList);
        crs.close();
        crs = null;

    }

    public void updateTradeRqmt(double pRqmtID, String pNewStatus, java.sql.Date pCompletedDate, String pCmt )
            throws SQLException {
        String stringDate = "";
        stringDate = sdfSPDate.format(pCompletedDate);
        OracleCallableStatement statement;
        //String callSqlStatement = "{call ops_tracking.PKG_TRADE_RQMT.p_update_trade_rqmt(?, ?, ?, ?) }";
        String callSqlStatement = "{call ops_tracking.PKG_TRADE_RQMT.p_update_mult_trade_rqmts(?,?,?,?,?,?,?,?,?) }";
        statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
        statement.setDouble(1, pRqmtID);
        statement.setString(2, pNewStatus);
        statement.setString(3, stringDate);
        statement.setString(4, "");
        statement.setString(5, pCmt);
        statement.setString(6, "Y");
        statement.setString(7, "Y");
        statement.setString(8, "N");
        statement.setString(9, "Y");
        statement.executeQuery();
        statement.close();
        statement = null;
    }

    private double getEFETRqmtId(double pTradeId) {
        boolean toProcess = false;
        String sql = " select Id from trade_rqmt " +
                " where trade_id = ? " +
                " and rqmt = 'EFET'";

        double rqmtId = 0;
        PreparedStatement ps = null;
        ResultSet rs = null;
        try {
            ps = opsTrackingConnection.prepareStatement(sql);
            ps.setDouble(1,pTradeId);
            rs = ps.executeQuery();
            if (rs.next()){
                rqmtId = rs.getDouble("Id");
            }
        }
        catch (SQLException e){
            Logger.getLogger(EFETTradeSubmitterService.class).error("TradeMatched Check=" + e.getMessage());
        }
        finally {
            try {
                if (rs != null){
                    rs.close();
                }
                if (ps != null) {
                    ps.close();
                }
            }
            catch (SQLException e){
            }
        }
        return rqmtId;
    }

    private void notifyProcess(String msgError ){

       String bodyText = "The EFET submitter found the following issues while submitting the trades:\n";

       if ( msgError == null || "".equalsIgnoreCase(msgError.trim())){
            return ;
       }
       bodyText = bodyText + msgError;
       sendEmail("EFET Submission Status",bodyText);
       


   }
    private boolean hasProblem(ArrayList tradeList,double tradeId){
        boolean tradeProblem = false;
        if (tradeList != null) {
            int size = tradeList.size();
            for (int i=0 ;i<size; ++i){
                Double d = (Double) tradeList.get(i);
                if (d != null) {
                    if ( tradeId == d.doubleValue()) {
                       tradeProblem = true;
                       break;
                    }
                }
            }
        }
        return tradeProblem;
    }
    private void addToList(ArrayList tradeList, double tradeId){

        if (tradeList != null){
            Double d = new Double(tradeId);
            tradeList.add(d);
        }
    }

    private boolean readyToProcess(int id) {
        boolean toProcess = false;
        String sql = "Select * From jbossdbq.Q_EFET_TRADE_ALERT where id = ?";
        PreparedStatement ps = null;
        ResultSet rs = null;
        try {
            ps = opsTrackingConnection.prepareStatement(sql);
            ps.setDouble(1,id);
             rs = ps.executeQuery();
            if (rs.next()){
                toProcess = (("N".equalsIgnoreCase(rs.getString("Processed_Flag"))) || ("R".equalsIgnoreCase(rs.getString("Processed_Flag"))));
            }

        }
        catch (SQLException e){
            log.error("TradeMatched Check=" , e );
        }
        finally {
            try {

                if (rs != null){
                    rs.close();
                }
                if (ps != null) {
                    ps.close();
                }
            }
                catch (SQLException e){

                }
          }

        return toProcess;

    }
    private boolean isFNCLTrade(double tradeId) throws SQLException {
        String sql = "select trade_sttl_type_code from infinity_mgr.energy_trade where prmnt_trade_id = ? and exp_dt = '31-DEC-2299'";
        boolean fnclTrade = false;
        PreparedStatement ps = null;
        ResultSet rs = null;
        try {
            ps  = this.affinityConnection.prepareStatement(sql);
            ps.setDouble(1,tradeId);
            rs = ps.executeQuery();
            if (rs.next()){
                String sttlTypeCode = rs.getString("trade_sttl_type_code");
                fnclTrade = _FNCL_STTL_CODE.equalsIgnoreCase(sttlTypeCode);
            }
        }
        finally {
            try {
                if (rs != null){
                    rs.close();
                }
                if ( ps != null){
                    ps.close();
                }
            }
            catch (SQLException e){
                
            }
        }
        return fnclTrade;
    }
    private boolean isTradeMatched(double tradeId, String docType,String receiverType) {
        boolean tradeMatched = false;
        String rqmt = "NONE";
        String sql = "Select * From ops_tracking.trade_rqmt where trade_id = ? and rqmt = ? order by id desc";
        if ("CNF".equalsIgnoreCase(docType) && "C".equalsIgnoreCase(receiverType)) {
           rqmt = "EFET";
        }
        else {
            rqmt = "EFBKR";
        }
       // ("CNF".equalsIgnoreCase(docType) && "C".equalsIgnoreCase(receiverType))
        PreparedStatement ps = null;
        try {
            ps = opsTrackingConnection.prepareStatement(sql);
            ps.setDouble(1,tradeId);
            ps.setString(2,rqmt);
            ResultSet rs = ps.executeQuery();
            if (rs.next()){
                tradeMatched = "MATCH".equalsIgnoreCase(rs.getString("status"));
            }
            rs.close();
        }
        catch (SQLException e){
            log.error("TradeMatched Check=" , e );
        }
        finally {
            if (ps != null) {
                try {
                    ps.close();
                }
                catch (SQLException e){

                }
            }
        }
        return tradeMatched;
    }

    //Israel 2/6/2015 -- Uncommented, doing analysis to see if relevent
    private String getEntityType(String documentType, double dTradeId) throws SQLException {
        String entityType;
        /**
         * There's no way of knowing what the entity type is from the BRES doc.
         * Also, make sure the rqmt you find isn't cancelled or about to be cancelled.
         */
        entityType = "C";
        if (documentType.equalsIgnoreCase(EFET_DAO.BFI))
                entityType = "B";
        else if (documentType.equalsIgnoreCase(EFET_DAO.CNF)&&
                !qEFETTradeAlert.isEFETQueued(dTradeId,"CANCEL",EFET_DAO.CNF,entityType)){
            if (efetDAO.isTradeRqmtExist(dTradeId,"EFET"))
                entityType = "C";
            else
                entityType = "B";
        }
        return entityType;
    }


/*    private EFETBFIXMLDataRec getEFETXMLDataRec(double pTradeId, String pDocumentId, int pDocumentVersion, String pLinkedTo)
                throws Exception, ParseException {
        EFETBFIXMLDataRec efetBFIXMLDataRec;
        efetBFIXMLDataRec = new EFETBFIXMLDataRec();
        EFETSubmitXMLDataRec efetSubmitXMLDataRec;
        efetSubmitXMLDataRec = new EFETSubmitXMLDataRec();
        efetSubmitXMLDataRec = efetProcessor.getEfetSubmitXMLDataRec(pTradeId);

        efetBFIXMLDataRec.init();
        efetBFIXMLDataRec.documentId = pDocumentId;
        efetBFIXMLDataRec.setDocumentVersion(pDocumentVersion);
        efetBFIXMLDataRec.documentUsage = documentUsage;
        efetBFIXMLDataRec.senderId = efetSubmitXMLDataRec.senderId;
        efetBFIXMLDataRec.receiverId = efetSubmitXMLDataRec.brokerId;
        efetBFIXMLDataRec.receiverRole = "Broker";
        efetBFIXMLDataRec.linkedTo = pLinkedTo;
        efetBFIXMLDataRec.setTotalFee(efetSubmitXMLDataRec.bkrFeeTotal);
        efetBFIXMLDataRec.feeCurrency = efetSubmitXMLDataRec.bkrFeeCcy;
        return efetBFIXMLDataRec;
    }*/

    private boolean moveFile(String pFromFileName, String pToFileName) {
        boolean renameSuccess = false;
        File fromFile = new File(pFromFileName);
        File toFile = new File(pToFileName);
        renameSuccess = fromFile.renameTo(toFile);
        fromFile = null;
        toFile = null;
        return renameSuccess;
    }

    private void saveAsTextFile(String pFileName, String pTextToSave)
            throws ParseException, IOException {
        //String filePathName = pDirLocation + "\\" + pFileName;
        FileWriter fileWriter = null;
        fileWriter = new FileWriter(pFileName);
        fileWriter.write(pTextToSave);
        fileWriter.flush();
        fileWriter.close();
        fileWriter = null;
    }
    private void sendEmail(String subject, String msg) {

        MailUtils mail = new MailUtils(this.smtpHost,this.smtpPort);
        String toAddress = "NONE";
        if (!environment.equalsIgnoreCase("PROD"))
           toAddress = devEmailAddress;
        else {
            toAddress = prodEmailAddress;
        }
        String hostName = null;
        log.info("Sending Email");
        try {
            hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
            String sentFromName = "EFETTradeSubmitter_" + hostName;
            String sentFromAddress = "JBossOn_" + hostName + "@amphorainc.com";
            mail.sendMail(toAddress,toAddress,sentFromName,sentFromAddress,subject,msg,"");
            log.info("Recipient Address =" + toAddress);
            log.info("Sent Email");

        } catch (Exception e) {
            Logger.getLogger( this.getClass()).error( "ERROR", e );
        }

    }


}
