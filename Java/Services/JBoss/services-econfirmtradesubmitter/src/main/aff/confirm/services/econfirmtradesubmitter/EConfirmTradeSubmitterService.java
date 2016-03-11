/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:30:52 AM
 * To change template for new class use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.econfirmtradesubmitter;

import aff.confirm.common.econfirm.EConfirmAPI;
import aff.confirm.common.econfirm.EConfirmData;
import aff.confirm.common.econfirm.EConfirmTradeInfo;
import aff.confirm.common.econfirm.datarec.EConfirmErrorLog_DataRec;
import aff.confirm.common.econfirm.datarec.EConfirmSubmitLog_DataRec;
import aff.confirm.common.econfirm.datarec.EConfirmSummary_DataRec;
import aff.confirm.common.econfirm.exceptions.AuthenticateFailedException;
import aff.confirm.common.econfirm.exceptions.SECptyNotFoundException;
import com.sun.rowset.CachedRowSetImpl;
import org.jboss.logging.Logger;
import org.jdom.JDOMException;
import aff.confirm.common.dbqueue.QEConfirmAlert;
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
import javax.sql.rowset.CachedRowSet;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.io.IOException;
import java.io.StringWriter;
import java.net.SocketException;
import java.util.Calendar;


@Startup
@Singleton
public class EConfirmTradeSubmitterService extends TaskService implements EConfirmTradeSubmitterServiceMBean {

    private final String SERVICE_NAME = "TradeSubmitter";
    private String eConfirmAPIUrl;
    private String eConfirmTradeInfoServiceUrl;
    private EConfirmAPI eConfirmAPI;
    private EConfirmData eConfirmData;
    private EConfirmTradeInfo eConfirmTradeInfo;
    private String opsTrackingDBInfoName;
    private String affinityDBInfoName;
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    private String affinityDBInfoDisplayName;
    private String opsTrackingDBInfoDisplayName;
    private QEConfirmAlert qEConfirmAlert;
    private String fileStoreDir;
    private int fileStoreExpireDays;
    private int connectAttempts;
    private int connectAttemptsRemaining;

    private String proxyType;
    private String proxyUrl;
    private int proxyPort;

    private java.util.Date lastDbConnection = null;
    private Calendar calc = Calendar.getInstance();

    public EConfirmTradeSubmitterService() {
        super("affinity.cwf:service=EConfirmTradeSubmitter");
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

    public String getFileStoreDir() {
        return fileStoreDir;
    }

    public void setFileStoreDir(String pFileStoreDir) {
        this.fileStoreDir = pFileStoreDir;
    }

    public int getFileStoreExpireDays() {
        return fileStoreExpireDays;
    }

    public void setFileStoreExpireDays(int pFileStoreExpireDays) {
        this.fileStoreExpireDays = pFileStoreExpireDays;
    }

    public int getConnectAttempts() {
        return this.connectAttempts;
    }

    public void setConnectAttempts(int connectAttempts) {
        this.connectAttempts = connectAttempts;
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

    private void init() throws Exception {
        Logger.getLogger(this.getClass()).info("Executing init... ");

        Logger.getLogger(this.getClass()).info("Connecting opsTrackingConnection to " + opsTrackingDBInfoName + "...");
        opsTrackingConnection = getOracleConnection(opsTrackingDBInfoName, opsTrackingConnection);
        Logger.getLogger(this.getClass()).info("Connected opsTrackingConnection to " + opsTrackingDBInfoName + ".");

        Logger.getLogger(this.getClass()).info("Connecting affinityConnection to " + affinityDBInfoName + "...");
        affinityConnection = getOracleConnection(affinityDBInfoName, affinityConnection);
        Logger.getLogger(this.getClass()).info("Connected affinityConnection to " + affinityDBInfoName + ".");

        setDbDisplayNames();
        Logger.getLogger(this.getClass()).info("opsTrackingDBInfoName = " + opsTrackingDBInfoDisplayName);
        Logger.getLogger(this.getClass()).info("affinityConnection = " + affinityDBInfoDisplayName);

        //Samy : 07/06/2011 commented to skip the icts conneciton
        /*
        Logger.getLogger(EConfirmTradeAlert2Service.class).info("Connecting ictsConnection to " + ictsDBInfoName + "...");

        ictsConnection = getIctsConnection();
        Logger.getLogger(EConfirmTradeAlert2Service.class).info("Connected ictsConnection to " + ictsDBInfoName + ".");
        */
        Logger.getLogger(this.getClass()).info("eConfirmTradeInfoServiceUrl=" + eConfirmTradeInfoServiceUrl);
        Logger.getLogger(this.getClass()).info("eConfirmAPIUrl=" + eConfirmAPIUrl);

        String text = "";
        text = "Timer interval = " + (getTimerPeriod() / 1000) + " seconds.";
        Logger.getLogger(this.getClass()).info(text);

        eConfirmAPI = new EConfirmAPI(eConfirmAPIUrl, eConfirmTradeInfoServiceUrl, proxyType, proxyUrl, proxyPort, fileStoreDir, SERVICE_NAME, fileStoreExpireDays);
        //eConfirmDAO = new EConfirmDAO(opsTrackingConnection, affinityConnection);

        String ecUserId = "";
        String ecPassword = "";

        eConfirmData = new EConfirmData(opsTrackingConnection);
        eConfirmTradeInfo = new EConfirmTradeInfo(eConfirmTradeInfoServiceUrl);

//        ecUserId = eConfirmDAO.getECUserId();
//        ecPassword = eConfirmDAO.getECPassword();
//
//        eConfirmProcessor = new EConfirmProcessor(affinityConnection, ictsConnection, eConfirmURL,
//                fileStoreDir,"TradeSubmit",fileStoreExpireDays);
//        eConfirmProcessor.setEConfirmUserName(ecUserId);
//        eConfirmProcessor.setEConfirmPassword(ecPassword);
//        eConfirmProcessor.setEcCompanySecurity(eConfirmDAO.getECUserList());
//        eConfirmProcessor.setProxyType(this.proxyType);
//        eConfirmProcessor.setProxyUrl(this.proxyUrl);
//        eConfirmProcessor.setProxyPort(this.proxyPort);
        
        qEConfirmAlert = new QEConfirmAlert(opsTrackingConnection);
        connectAttemptsRemaining = this.getConnectAttempts();

        Logger.getLogger(this.getClass()).info("eConfirmTradeAlert Started.");
        lastDbConnection =  new java.util.Date(); 
    }

    protected void onServiceStarting() throws Exception {
        Logger.getLogger(this.getClass()).info("Executing startService... ");
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
        //eConfirmDAO = null;
        //eConfirmProcessor = null;
        eConfirmAPI = null;
        eConfirmData = null;
        qEConfirmAlert = null;

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

/*
        if(ictsConnection != null){
            try {
                ictsConnection.close();
            } catch (SQLException e) {
                log.error(e);
            }
            ictsConnection = null;
        }
*/
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


    private void setDbDisplayNames() throws NamingException {

        DbInfoWrapper dbinfo = new DbInfoWrapper(affinityDBInfoName);
        affinityDBInfoDisplayName = dbinfo.getDatabaseName();
        dbinfo = new DbInfoWrapper(opsTrackingDBInfoName);
        opsTrackingDBInfoDisplayName = dbinfo.getDatabaseName();
    }

    synchronized public void executeTimerEventNow() throws StopServiceException {
        runTask();
    }

    protected void runTask() throws StopServiceException {
        checkToReconnect();
        poll();
    }

    synchronized private void poll() throws StopServiceException{
        StringWriter sWriter = null;
        try {
            sWriter = new StringWriter();
//            Logger.getLogger(EConfirmTradeAlert2Service.class).info("Executing trade alert task...");
            processEConfirmAlertRecords();
            //opsTrackingConnection.commit();
//            Logger.getLogger(EConfirmTradeAlert2Service.class).info("Execute trade alert task done.");
        } catch (SocketException se){
            Logger.getLogger(this.getClass()).error("ERROR", se);
            try {
                opsTrackingConnection.rollback();
            } catch (Exception e1) {
                Logger.getLogger(this.getClass()).error("ERROR", e1);
                throw new StopServiceException(sWriter.getBuffer().toString());
            }
            if (connectAttemptsRemaining <= 0)
                throw new StopServiceException(sWriter.getBuffer().toString());
            else connectAttemptsRemaining = connectAttemptsRemaining -1;
            //if (connectAttemptsRemaining <= 0) {

                /*
                String responseAction =   amphorainc.errorreporting.client.ErrorReportClient.getErrorAction("", CNFErrorConstant.APP_CODE,
                        this.getClass().getName(),CNFErrorConstant.HTTP_SOCKET_ERROR,se);

                if ( responseAction.equalsIgnoreCase(CNFErrorConstant.ACTION_FATAL) || responseAction.equalsIgnoreCase(CNFErrorConstant.ACTION_ERROR) ) {
                    throw new StopServiceException(sWriter.getBuffer().toString());
                }
                else {
                    connectAttemptsRemaining = this.getConnectAttempts();
                }
                */

            //}
            //else connectAttemptsRemaining = connectAttemptsRemaining -1;
        } catch (IOException e){
            Logger.getLogger(this.getClass()).error("ERROR", e);
            try {
                opsTrackingConnection.rollback();
            } catch (Exception e1) {
                Logger.getLogger(this.getClass()).error("ERROR", e);
                throw new StopServiceException(sWriter.getBuffer().toString());
            }
            if (connectAttemptsRemaining <= 0)
                throw new StopServiceException(sWriter.getBuffer().toString());
            else connectAttemptsRemaining = connectAttemptsRemaining -1;
        } catch (AuthenticateFailedException ae) {
            Logger.getLogger(this.getClass()).error("ERROR", ae);
            try {
                opsTrackingConnection.rollback();
            } catch (Exception e1) {
                Logger.getLogger(this.getClass()).error("ERROR", e1);
                throw new StopServiceException(sWriter.getBuffer().toString());
            }
            if (connectAttemptsRemaining <= 0)
                throw new StopServiceException(sWriter.getBuffer().toString());
            else connectAttemptsRemaining = connectAttemptsRemaining -1;
        } catch (Exception e) {
            Logger.getLogger(this.getClass()).error("ERROR", e);
            try {
                opsTrackingConnection.rollback();
            } catch (Exception e1) {
                Logger.getLogger(this.getClass()).error("ERROR", e);
            }
            throw new StopServiceException(sWriter.getBuffer().toString());
        }
    }

    private void processEConfirmAlertRecords()
            throws JDOMException, IOException, AuthenticateFailedException, SECptyNotFoundException, SocketException, Exception {
        String trdSys = null;
        String ticketID = null;
        double tradeID = -1;
        int id = 0;
        int productID = -1;
        boolean okToSubmit = false;
        String eConfirmAction = "";
        boolean isClickAndConfirm = false;
        String eBkrConfirmAction = "";
        String eAction = ""; //samy added to consoldate cpty and broker code.
        CachedRowSet crs;
        crs = new CachedRowSetImpl();
        crs = qEConfirmAlert.getReadyToProcess();
        crs.beforeFirst();
        while (crs.next()) {
            trdSys = crs.getString("TRADING_SYSTEM");
            tradeID = crs.getDouble("TRADE_ID");
            ticketID = df.format(tradeID);
            productID = crs.getInt("EC_PRODUCT_ID");
            eConfirmAction = crs.getString("EC_ACTION");
            // MThoresen 4-18-2007 : To handle click and confirm
            isClickAndConfirm = (crs.getString("CLICK_AND_CONFIRM_FLAG").equalsIgnoreCase("Y"));
            eBkrConfirmAction = crs.getString("EC_BKR_ACTION");
            // added code to put the security based on the company
            String seCptySn = getTradeDataSECptySn(tradeID);
            //assignUserNamePassword(seCptySn);
            String ecUserId = eConfirmData.getECUserId(seCptySn);
            String ecPassword = eConfirmData.getECPassword(seCptySn);

            if (eBkrConfirmAction == null){
                eBkrConfirmAction = "NONE";
            }

            // consolidate the cpty and broker action
            eAction =  eConfirmAction;
            if (eAction == null) {
                eAction = "NONE";
            }
            if ( "NONE".equalsIgnoreCase(eAction)) {
                eAction = eBkrConfirmAction;
            }

            String responseXML = "";
            String statusMessage = "";
            String traceID = "";

            String submitXml = "";
            String submitXmlRqmtMessage = "";
            boolean isWebServiceSubmitError = false;
            EConfirmSubmitLog_DataRec eConfirmSubmitDataRec;
            EConfirmSummary_DataRec eConfirmSummaryDataRec;
            EConfirmErrorLog_DataRec eConfirmErrorLogDataRec;
            String initialStatus = "";
            String initialBkrStatus = "";
            String errorFlag = "";

            String rqmtStatus = "";
            boolean updateQEConfirmAlertTable = true;

            //EC_ACTION = SUBMIT
            if (eAction.equalsIgnoreCase("SUBMIT")) {
                    okToSubmit = (productID > 0);
                if ((isClickAndConfirm) && (okToSubmit)){
                    rqmtStatus = eConfirmData.getRqmtStatus(tradeID, "XQBBP");
                    okToSubmit = ((rqmtStatus.equalsIgnoreCase("APPR")) || ((eConfirmData.isCandCValidVerbalRqmt(tradeID))));
                }
                if (okToSubmit) {
                    //check whether broker changed and it needs to be cancelled.... case cpty resubmit and broker cancel
                    
                    if (eBkrConfirmAction.equalsIgnoreCase("CANCEL")) {
                        cancelTrade(trdSys,tradeID,ticketID,true);
                        opsTrackingConnection.commit();
                    }

                    submitXml = eConfirmTradeInfo.getEConfirmXml(productID, trdSys, tradeID);
                    isWebServiceSubmitError = eConfirmTradeInfo.isWebServiceSubmitError(submitXml);
                    if (isWebServiceSubmitError) {
                        submitXmlRqmtMessage = eConfirmTradeInfo.getSubmitErrorRqmtMessage(submitXml);
                    }

                    Logger.getLogger(this.getClass()).info("Submitting: " + trdSys + " " + ticketID + "...");
//                    responseXML = eConfirmProcessor.submitToEConfirm(trdSys, tradeID, productID);
                    responseXML = eConfirmAPI.submitToEConfirm(EConfirmAPI.EC_REQ_TYPE_SUBMIT_TRADE_2, productID, trdSys, tradeID, ecUserId, ecPassword);

                    //6/22/15 Israel -- added isWebServiceSubmitError handling
                    if (isWebServiceSubmitError) {
                        Logger.getLogger(EConfirmTradeSubmitterService.class).info("Failed to create a submission xml. Submission not attempted.");
                    } else if (responseXML.equalsIgnoreCase("NO_SUBMIT_XML")) {
                        Logger.getLogger(this.getClass()).info("Submission Failed! Could not create submission XML.");
                        throw new Exception("Failed to submit xml to econfirm. " +
                                "Could not create submission XML. " +
                                "It may be an error in the trade data or a missing table entry in POWER_DATA or POWER_DATA_LOC. " +
                                "ticketID=" + ticketID + ", productID=" + productID);
                    } else if (responseXML == null || responseXML.equalsIgnoreCase("")) {
                        Logger.getLogger(this.getClass()).info("Submission Failed! eConfirm Website may be down.");
                        throw new Exception("Failed to submit xml to econfirm. EConfirm Website may be down. " +
                                "ticketID=" + ticketID + ", productID=" + productID + ", responseXML=" + responseXML);
                    } else
                        //without the \n there's no line feed and the next log line is continued on this one.
                        Logger.getLogger(this.getClass()).info("Submitted: " + trdSys + " " +
                                ticketID + " " + responseXML + "\n");

                    if (!isWebServiceSubmitError) {
                    statusMessage = eConfirmAPI.getStatusMessage(responseXML, EConfirmAPI.EC_SUBMIT_TYPE_2);
                    if (statusMessage.equalsIgnoreCase("SUCCESS")) {
//                        traceID = eConfirmProcessor.getTraceID(responseXML);
                        traceID = eConfirmAPI.getTraceID(responseXML);
                        //**todo Handle nothing submitted (wrong cpty, couldn't find product, non-standard deal)
                        if ("NONE".equalsIgnoreCase(eConfirmAction)) {
                            initialStatus = "NONE";
                        }
                        else {
                            initialStatus = "SUBMITTED";
                        }
                        if ("NONE".equalsIgnoreCase(eBkrConfirmAction)) {
                            initialBkrStatus =  "NONE";
                        }
                        else {
                            initialBkrStatus =  "SUBMITTED";
                        }
                        errorFlag = "N";
                        eConfirmSubmitDataRec = new EConfirmSubmitLog_DataRec(trdSys, tradeID, traceID, initialStatus, eAction);
                        int submitID = -1;
                        //submitID = eConfirmDAO.insertECSubmitLog(eConfirmSubmitDataRec);
                        submitID = eConfirmData.insertECSubmitLog(eConfirmSubmitDataRec);
                        // samy: added 2 more columns for broker matching
                        eConfirmSummaryDataRec = new EConfirmSummary_DataRec(trdSys, tradeID, productID, initialStatus, "", "", errorFlag,initialBkrStatus,"");
                        eConfirmSummaryDataRec = new EConfirmSummary_DataRec(trdSys, tradeID, productID, initialStatus, "", "", errorFlag,initialBkrStatus,"");
                        if (!eConfirmData.isECTradeSummaryExist(trdSys, tradeID))
                            eConfirmData.insertECTradeSummary(eConfirmSummaryDataRec);
                        else
                            eConfirmData.updateECTradeSummary(eConfirmSummaryDataRec);

                        eConfirmData.setNotifyOpsTrackingSubmit(tradeID);
                        eConfirmData.setNotifyOpsTrackingBkrSubmit(tradeID); // samy : added for the broker matching.
                        Logger.getLogger(this.getClass()).info("EC_SUBMIT_LOG: " + trdSys + " " + ticketID + " :" + initialStatus);
                     } else {
                         if ("NONE".equalsIgnoreCase(eConfirmAction)) {
                            initialStatus = "FAILED";
                        }
                        else {
                            initialStatus = "SUBMITTED";
                        }
                        if ("NONE".equalsIgnoreCase(eBkrConfirmAction)) {
                            initialBkrStatus =  "NONE";
                        }
                        else {
                            initialBkrStatus =  "FAILED";
                        }
                        errorFlag = "Y";
                        eConfirmSubmitDataRec = new EConfirmSubmitLog_DataRec(trdSys, tradeID, traceID, initialStatus, eConfirmAction);
                        eConfirmData.insertECSubmitLog(eConfirmSubmitDataRec);

                        eConfirmErrorLogDataRec = new EConfirmErrorLog_DataRec(responseXML);
                        eConfirmErrorLogDataRec.tradeID = tradeID;
                        eConfirmData.insertECErrorLog(eConfirmErrorLogDataRec);

                        eConfirmSummaryDataRec = new EConfirmSummary_DataRec(trdSys, tradeID, productID, initialStatus, "", "", errorFlag,initialBkrStatus,"");
                        if (!eConfirmData.isECTradeSummaryExist(trdSys, tradeID))
                            eConfirmData.insertECTradeSummary(eConfirmSummaryDataRec);
                        else
                            eConfirmData.updateECTradeSummary(eConfirmSummaryDataRec);

                        String comment = eConfirmErrorLogDataRec.ecDesc;
                        if (eConfirmErrorLogDataRec.ecDesc.length() > 255 )
                            comment = eConfirmErrorLogDataRec.ecDesc.substring(0, 254);

                        eConfirmData.setNotifyOpsTrackingFail(tradeID, comment);
                        eConfirmData.setNotifyOpsTrackingBkrFail(tradeID,comment);
                        Logger.getLogger(this.getClass()).info("EC_SUBMIT_LOG: " + trdSys + " " + ticketID + " :" + initialStatus);
                    }
                } //!isWebServiceSubmitError
                    //EC_ACTION = SUBMIT

                } else if (!isClickAndConfirm && !isWebServiceSubmitError) {
                    initialStatus = "IGNORED";
                    eConfirmSubmitDataRec = new EConfirmSubmitLog_DataRec(trdSys, tradeID, traceID, initialStatus, eConfirmAction);
                    eConfirmData.insertECIgnoredLog(eConfirmSubmitDataRec);
                    Logger.getLogger(this.getClass()).info("EC_IGNORED_LOG: " + trdSys +
                            " " + ticketID + " :" + initialStatus);
                    // MThoresen 4-25-2007 - added updateQEConfirmAlertTable so that the q_econfirm_alert record does not get updated to processed.
                } else if (!isWebServiceSubmitError)
                    updateQEConfirmAlertTable = false;
                //EC_ACTION = CANCEL
            } else if (eAction.equalsIgnoreCase("CANCEL")) {
                    cancelTrade(trdSys,tradeID,ticketID,false);
                    //EC_ACTION <> SUBMIT or CANCEL
            } else
                Logger.getLogger(this.getClass()).info("Unknown eConfirmAction=" + eAction);

            // MThoresen 4-25-2007 - added updateQEConfirmAlertTable so that the q_econfirm_alert record does not get updated to processed.
            if (updateQEConfirmAlertTable){
                id = crs.getInt("ID");

                //Israel 6/22/2015
                if (isWebServiceSubmitError){
                    qEConfirmAlert.updateAlertRecord(id, "X");
                    eConfirmData.setNotifyOpsTrackingError(tradeID, submitXmlRqmtMessage);
                }
                else
                    qEConfirmAlert.updateAlertRecord(id, "Y");
            }
            opsTrackingConnection.commit();
        }
        crs.close();
        crs = null;
    }

    // assign the user name and password based on the sempra company
/*    private void assignUserNamePassword(String seCptySn) {

        Hashtable hs =  eConfirmProcessor.getEcCompanySecurity();
        if (hs != null){
            String ecAccessInfo = (String) hs.get(seCptySn);
            if (ecAccessInfo != null){
               String[] userInfo = ecAccessInfo.split("\\|");
                eConfirmProcessor.setEConfirmUserName(userInfo[0]);
                eConfirmProcessor.setEConfirmPassword(userInfo[1]);
            }

        }
    }*/

    private void cancelTrade(String trdSys,double tradeID,String ticketID,boolean updateBkrRqmt) throws Exception {

        Logger.getLogger(this.getClass()).info("Cancelling: " + trdSys + " " + ticketID + "...");
        int ecSummaryProductId;
        String seCptySn = getTradeDataSECptySn(tradeID);

        //1/23/2007 Israel - Routine is now universal to accommodate ICTS. For oil trades the trade id
        //is prefixed with the A or J for Affinity or JMS. Determine this by keeping database hits to a minimum.
        String senderTradeRefId = ticketID;
        if (trdSys.equalsIgnoreCase("SYM"))
           senderTradeRefId = trdSys.substring(0,1) + ticketID;
        else {
            ecSummaryProductId = eConfirmData.getECTradeSummaryProductId(tradeID);
           if (ecSummaryProductId < 300 ||
              (ecSummaryProductId > 1099 && ecSummaryProductId < 1200 ))
               senderTradeRefId = trdSys.substring(0,1) + ticketID;
        }
        String ecUserId = eConfirmData.getECUserId(seCptySn);
        String ecPassword = eConfirmData.getECPassword(seCptySn);
        String ecBatchId = eConfirmData.getBatchId(seCptySn);
        String responseXML = eConfirmAPI.cancelEConfirmTrade(ecBatchId, senderTradeRefId, ecUserId, ecPassword);
        if (responseXML != null &&  !responseXML.equalsIgnoreCase("")) {
            Logger.getLogger(this.getClass()).info("Cancelled: " + trdSys + " " + ticketID + " " + responseXML + "\n");
        }
        else {
            Logger.getLogger(this.getClass()).info("Cancel Failed!");
        }
        if (updateBkrRqmt) {
            eConfirmData.setNotifyOpsTrackingBkrCancelled(tradeID);
        }
    }

    public String getTradeDataSECptySn(double pTradeID) throws SQLException {
        String seCptySn = "RBS SET";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT SE_CPTY_SN from " +
                    "ops_tracking.TRADE_DATA where trade_id = ?");
            statement.setDouble(1, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                seCptySn = (rs.getString("SE_CPTY_SN"));
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

        return seCptySn;
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

}
