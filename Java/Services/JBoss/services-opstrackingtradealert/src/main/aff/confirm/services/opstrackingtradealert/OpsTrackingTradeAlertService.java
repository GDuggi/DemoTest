/*
 * User: ifrankel
 * Date: Oct 2, 2002
 * Time: 9:30:52 AM
 */
package aff.confirm.services.opstrackingtradealert;

import aff.confirm.common.econfirm.EConfirmData;
import aff.confirm.common.econfirm.datarec.EConfirmSummary_DataRec;
import aff.confirm.common.util.JndiUtil;
import org.jboss.logging.Logger;
import aff.confirm.common.util.MessageUtils;
import aff.confirm.common.dao.EmpDAO;
import aff.confirm.common.dao.AppControlDAO;
import aff.confirm.common.ottradealert.OpsTrackingReturnDataRec;
import aff.confirm.common.ottradealert.*;
import aff.confirm.common.util.MailUtils;
import aff.confirm.common.util.StringUtils;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.queueservice.QueueService;
import aff.confirm.jboss.common.util.DbInfoWrapper;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.Queue;
import javax.jms.QueueSender;
import javax.management.MalformedObjectNameException;
import javax.management.Notification;
import javax.management.NotificationListener;
import javax.management.ObjectName;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.net.InetAddress;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.sql.ResultSet;
import java.sql.PreparedStatement;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.ArrayList;

@Startup
@Singleton
public class OpsTrackingTradeAlertService extends QueueService
        implements OpsTrackingTradeAlertServiceMBean, NotificationListener {

    private final String EC_SUBMIT = "SUBMIT";
    private final String EC_CANCEL = "CANCEL";
    private String opsTrackingDBInfoName;
    private String affinityDBInfoName;
    //private String symphonyDBInfoName;
    private String eConfirmTradeSubmitQueueName;
    private String efetTradeSubmitQueueName;
    private QueueSender senderEConfirmTradeSubmit;
    private QueueSender senderEFETTradeSubmit;
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
//    private java.sql.Connection symphonyConnection;
    private OpsTrackingTradeAlert otTradeAlertDAO = null;
    private OpsTrackingIGNORED_NOTIFICATIONS_dao otIGNORED_NOTIFICATIONS = null;
    private TradingSystemDATA_dao tsDATA_dao = null;
    private String smtpHost;
    private String smtpPort;
    private String sendToUSElec;
    private String sendToUSNGas;
    private String sendToUSOil;
    private String sendToUKElec;
    private String sendToUKNGas;
    private String sendToSGEGas;
    private String sendToSGEPower;
    private String sendToSGEOil;
    private String sendToAddress;
    private String sendToName;
    private String sentFromAddress;
    private String sentFromName;
    private String efetSentFromName;
    //private String houseEditFromName;
    private String ecFailedLogAddress;
    private String systemsNotifyToAddress;
    private String systemsNotifyFromDomain;
    private String efetWarningAddress;
    private String stopServiceNotifyAddress;
    private MailUtils mailUtils;
    private String affinityDBInfoDisplayName;
    private String opsTrackingDBInfoDisplayName;
    private EmpDAO empDAO;
    private OpsTrackingTRADE_PRIORITY_rec otTRADE_PRIORITY_rec;
    private OpsTrackingPriorityCalc opsTrackingPriorityCalc;
    private OpsTrackingTRADE_PRIORITY_dao otTRADE_PRIORITY_dao;
    private OpsTrackingTRADE_DATA_dao otTRADE_DATA_dao;
    private OpsTrackingTRADE_DATA_CHG_dao otTRADE_DATA_CHG_dao;
    private OpsTrackingTRADE_RQMT_dao otTRADE_RQMT_dao;
    private OpsTrackingTRADE_SUMMARY_dao otTRADE_SUMMARY_dao;
    private OpsTrackingTRADE_DATA_rec otDataRec;
    private TradingSystemDATA_rec tsDataRec;
    private AppControlDAO appControlDAO;
    private EConfirmData eConfirmData;
    private String[] updateTradeDataAuditCodeList;
    //Not necessary because infinity_mgr.v_realtime_trade_audit filters out double-booked trades.
    //When EMS publisher is implemented, uncomment ictsBookList and related code to filter out double-booked trades.
    private ArrayList ictsBookList = new ArrayList();
    private String environment;
    private String tradeDataWebServiceUrl;
    private String econfirmTradeInfoServiceUrl;
    private String tradeDataRootTagName;


    //private SecondCheckDAO secondCheckDAO;
    //private OpsTrackingSecondCheck_rec otSecondCheckRec;
    //private OpsTrackingTRADE_RQMT_SECOND_CHECK_dao otTRADE_RQMT_SECOND_CHECK_dao;

    // 8-07-2007: MThoresen - Added to prevent stoppage on unique contraint....for trades already existing.
    private OpsTrackingTRADE_NOTIFY_dao opsTrackingTradeNotifyDao = null;

    public OpsTrackingTradeAlertService() {
        super("affinity.cwf:service=OpsTrackingTradeAlert");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
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

//   Israel 12/4/14
/*    public void setSymphonyDBInfoName(String pSymphonyDBInfoName) {
        this.symphonyDBInfoName = pSymphonyDBInfoName;
    }

    public ObjectName getSymphonyDBInfo() throws MalformedObjectNameException {
        if (symphonyDBInfoName.length() > 0)
            return new ObjectName("sempra.utils:service=" + symphonyDBInfoName);
        else
            return null;
    }*/

    public void setTradeDataWebServiceUrl(String pTradeDataWebServiceUrl) {
        this.tradeDataWebServiceUrl = pTradeDataWebServiceUrl;
    }

    public String getTradeDataWebServiceUrl() {
        return tradeDataWebServiceUrl;
    }

    public void setTradeDataRootTagName(String pTradeDataRootTagName) {
        this.tradeDataRootTagName = pTradeDataRootTagName;
    }

    public String getTradeDataRootTagName() {
        return tradeDataRootTagName;
    }

    public void setEConfirmTradeSubmitQueueName(String pEConfirmTradeSubmitQueueName) {
        this.eConfirmTradeSubmitQueueName = pEConfirmTradeSubmitQueueName;
    }

    public void setEFETTradeSubmitQueueName(String pEFETTradeSubmitQueueName) {
        this.efetTradeSubmitQueueName = pEFETTradeSubmitQueueName;
    }

    public String getSmtpHost() {
        return smtpHost;
    }

    public void setSmtpHost(String smtpHost) {
        this.smtpHost = smtpHost;
    }

    public String getSmtpPort() {
        return smtpPort;
    }

    public void setSmtpPort(String pSMTPPort) {
        this.smtpPort = pSMTPPort;
    }

    public String getSendToAddress() {
        return sendToAddress;
    }

    public void setSendToAddress(String pSendToAddress) {
        this.sendToAddress = pSendToAddress;
    }

    public String getSendToName() {
        return sendToName;
    }

    public void setSendToName(String pSendToName) {
        this.sendToName = pSendToName;
    }

    public String getECFailedLogAddress() {
        return ecFailedLogAddress;
    }

    public void setECFailedLogAddress(String pECFailedLogAddress) {
        this.ecFailedLogAddress = pECFailedLogAddress;
    }

    public String getSystemsNotifyToAddress() {
        return systemsNotifyToAddress;
    }

    public void setSystemsNotifyToAddress(String pSystemsNotifyToAddress) {
        this.systemsNotifyToAddress = pSystemsNotifyToAddress;
    }

    public String getSystemsNotifyFromDomain() {
        return systemsNotifyFromDomain;
    }

    public void setSystemsNotifyFromDomain(String pSystemsNotifyFromDomain) {
        this.systemsNotifyFromDomain = pSystemsNotifyFromDomain;
    }

    public String getEFETWarningAddress() {
        return efetWarningAddress;
    }

    public void setEFETWarningAddress(String pEFETWarningAddress) {
        this.efetWarningAddress = pEFETWarningAddress;
    }

    public String getStopServiceNotifyAddress() {
        return stopServiceNotifyAddress;
    }

    public void setStopServiceNotifyAddress(String pStopServiceNotifyAddress) {
        this.stopServiceNotifyAddress = pStopServiceNotifyAddress;
    }

    public String getEnv() {
        return environment;
    }

    public void setEnv(String pEnv) {
        environment = pEnv;
    }

    public String getEConfirmTradeInfoServiceUrl() {
        return econfirmTradeInfoServiceUrl;
    }

    public void setEConfirmTradeInfoServiceUrl(String pEConfirmTradeInfoServiceUrl) {
        econfirmTradeInfoServiceUrl = pEConfirmTradeInfoServiceUrl;
    }

    public void init() throws Exception {
        InitialContext ic = null;
        try{
            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Initializing...");

            Queue EConfirmSubmitQueue = JndiUtil.lookup("queue/"+ eConfirmTradeSubmitQueueName);
            senderEConfirmTradeSubmit = getQueueSession().createSender(EConfirmSubmitQueue);

            //Israel 1/6/15 - Restored EFET Queue
            Queue EFETSubmitQueue = JndiUtil.lookup("queue/" + efetTradeSubmitQueueName);
            senderEFETTradeSubmit = getQueueSession().createSender(EFETSubmitQueue);

            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Connecting opsTrackingConnection to " + opsTrackingDBInfoName + "...");
            opsTrackingConnection = getOracleConnection(opsTrackingDBInfoName, opsTrackingConnection);
            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Connected opsTrackingConnection to " + opsTrackingDBInfoName + ".");

            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Connecting affinityConnection to " + affinityDBInfoName + "...");
            affinityConnection = getOracleConnection(affinityDBInfoName, affinityConnection);
            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Connected affinityConnection to " + affinityDBInfoName + ".");

/*
            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Connecting symphonyConnection to " + symphonyDBInfoName + "...");
            symphonyConnection = getMSSqlConnection();
            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Connected symphonyConnection to " + symphonyDBInfoName + ".");
*/
            //Logger.getLogger(OpsTrackingTradeAlertService.class).info("symphonyConnection was ignored.");
            Logger.getLogger(OpsTrackingTradeAlertService.class).info("econfirmTradeInfoServiceUrl: " + econfirmTradeInfoServiceUrl + ".");

            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Trade data web service Url: " + tradeDataWebServiceUrl + ".");
            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Trade data root tag name: " + tradeDataRootTagName + ".");

            String hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
            sentFromName = "OpsTrackingTradeAlert_" + hostName;
            efetSentFromName = "EFET_Warning_" + hostName;
            //houseEditFromName = "HouseTradeEdit_" + hostName;
            sentFromAddress = "JBossOn_" + hostName + "@" + systemsNotifyFromDomain;
            mailUtils = new MailUtils(smtpHost, smtpPort);

            otTradeAlertDAO = new OpsTrackingTradeAlert(opsTrackingConnection, affinityConnection, /*symphonyConnection, */
                                tradeDataWebServiceUrl, tradeDataRootTagName, mailUtils, ecFailedLogAddress, econfirmTradeInfoServiceUrl);
            otTradeAlertDAO.systemsNotifyAddress = this.systemsNotifyToAddress;
            otTradeAlertDAO.sentFromAddress = sentFromAddress;
            otTradeAlertDAO.sentFromName = sentFromName;

            //eConfirmDAO = new EConfirmDAO(opsTrackingConnection,affinityConnection,mailUtils,ecFailedLogAddress);
            otIGNORED_NOTIFICATIONS = new OpsTrackingIGNORED_NOTIFICATIONS_dao(opsTrackingConnection);
            empDAO = new EmpDAO(affinityConnection);
            tsDATA_dao = new TradingSystemDATA_dao(affinityConnection, /*symphonyConnection,*/ tradeDataWebServiceUrl,tradeDataRootTagName);

            opsTrackingPriorityCalc = new OpsTrackingPriorityCalc(opsTrackingConnection,affinityConnection);
            otTRADE_PRIORITY_dao = new OpsTrackingTRADE_PRIORITY_dao(opsTrackingConnection);
            otTRADE_PRIORITY_rec = new OpsTrackingTRADE_PRIORITY_rec();
            otTRADE_DATA_CHG_dao = new OpsTrackingTRADE_DATA_CHG_dao(opsTrackingConnection);
            otTRADE_DATA_dao = new OpsTrackingTRADE_DATA_dao(opsTrackingConnection);
            otTRADE_RQMT_dao = new OpsTrackingTRADE_RQMT_dao(opsTrackingConnection,affinityConnection, /*symphonyConnection,*/
                    tradeDataWebServiceUrl,tradeDataRootTagName,mailUtils,ecFailedLogAddress, econfirmTradeInfoServiceUrl);
            otTRADE_SUMMARY_dao = new OpsTrackingTRADE_SUMMARY_dao(opsTrackingConnection);
            opsTrackingTradeNotifyDao = new OpsTrackingTRADE_NOTIFY_dao(opsTrackingConnection);

            otDataRec = new OpsTrackingTRADE_DATA_rec();
            tsDataRec = new TradingSystemDATA_rec();
            eConfirmData = new EConfirmData(opsTrackingConnection);

            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Loading base data...");
            appControlDAO = new AppControlDAO(opsTrackingConnection,"OTTA");
            String auditTypeCodes = appControlDAO.getValue("UPDATE_TRADE_DATA_AUDIT_CODES");
            updateTradeDataAuditCodeList = auditTypeCodes.split(",");

            sendToUSElec = appControlDAO.getValue("ECONFIRM_EMAIL_US_ELEC");
            sendToUSNGas = appControlDAO.getValue("ECONFIRM_EMAIL_US_NGAS");
            sendToUSOil = appControlDAO.getValue("ECONFIRM_EMAIL_US_OIL");
            sendToUKElec = appControlDAO.getValue("ECONFIRM_EMAIL_UK_ELEC");
            sendToUKNGas = appControlDAO.getValue("ECONFIRM_EMAIL_UK_NGAS");
            sendToSGEGas   = appControlDAO.getValue("ECONFIRM_EMAIL_SGE_NGAS");
            sendToSGEPower = appControlDAO.getValue("ECONFIRM_EMAIL_SGE_ELEC");
            sendToSGEOil = appControlDAO.getValue("ECONFIRM_EMAIL_SGE_OIL");
            setDbDisplayNames();

            //Double book filter code-- uncomment for EMS publisher support
            //initIctsBookList();
            //Logger.getLogger(OpsTrackingTradeAlertService.class).info("ICTS book list initialized.");

            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Operation environment: " + environment);
            Logger.getLogger(OpsTrackingTradeAlertService.class).info("Initialization complete.");
        }finally{
            if(ic != null){
                ic.close();
                ic = null;
            }

        }
    }

    //Israel 3/27/2008 - helps detect alpha-mandated double book entries.
    //Double book filter code-- uncomment for EMS publisher support
   private void initIctsBookList() throws SQLException, Exception {
        ResultSet rs = null;
        PreparedStatement statement = null;
        String sqlStatement =
            "SELECT DISTINCT short_name "
            + "FROM  "
            + "     infinity_mgr.bk "
            + "WHERE "
            + "     system_type_code = 'JMS'     "
            + "     AND SHORT_NAME <> 'ZZ' "
            + "     and active_flag = 'Y' "
            + "     and cost_only_flag = 'N' "
            + "ORDER BY short_name  ";
        try {
            statement = affinityConnection.prepareStatement(sqlStatement);
            rs = statement.executeQuery();
            while (rs.next()) {
                ictsBookList.add(rs.getString("short_name").trim());
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
    }


    private void setDbDisplayNames() throws NamingException {
        DbInfoWrapper dbinfo = new DbInfoWrapper(affinityDBInfoName);
        affinityDBInfoDisplayName = dbinfo.getDatabaseName();
        dbinfo = new DbInfoWrapper(opsTrackingDBInfoName);
        opsTrackingDBInfoDisplayName = dbinfo.getDatabaseName();
    }

    public void onMessage(Message pMessage) throws StopServiceException, LogException {
        /*String messageData = null;
        messageData = messageToString(pMessage);
        Logger.getLogger(OpsTrackingTradeAlertService.class).info(messageData);*/
        final SimpleDateFormat sdfCompare = new SimpleDateFormat("MM/dd/yyyy");
        String trdSysCode = null;
        String ticketID = null;
        String tradeTypeCode = null;
        String auditTypeCode = null;
        String cdtyCode = null;
        String notifyContractsFlag = null;
        String tradeSttlTypeCode = null;
        String eConfirmAction = "NONE";
        String eConfirmBkrAction = "NONE";
        String updateBusnDt = "";
        String action = "";
        String cptySn = "";
        String seCptySn = "";
        String brokerSn = "";
        String book = "";
        String bookSn = "";
        String instCode = "";
        String refSN = "";
        String updateTableName = "";
        double tradeID = -1;
        double version = 0;
        double empId = -1;
        String empName = "";
        int eConfirmProductID = -1;
        String tradeAuditID = "";
        boolean isTradeValid = true;
        boolean isIgnoreTradeType = false;
        //Double book filter code-- uncomment for EMS publisher support
        boolean isDoubleBooked = false;
        boolean isExcludedType = false;
        boolean isIgnoreUpdateTradeData = false;
        boolean isNotExoticBasket = false;

        OpsTrackingTradeAlertDataRec opsTrackingTradeAlertDataRec;
        TradingSystemDATA_rec tsDATA_rec;
        OpsTrackingReturnDataRec otReturnDataRec;
        otReturnDataRec = new OpsTrackingReturnDataRec();
        // MThoresen 4-18-2007 For click and confirm project
        EConfirmSummary_DataRec eConfirmSummaryDataRec;

        //String brokerSN = null;
        //double prmntTradeID = 0;
        try {
            tradeTypeCode = MessageUtils.getMessageValue("TRADE_TYPE_CODE", pMessage);
            trdSysCode = MessageUtils.getMessageValue("TRADING_SYSTEM", pMessage);
            ticketID = df.format(pMessage.getDoubleProperty("PRMNT_TRADE_ID"));
            tradeID = pMessage.getDoubleProperty("PRMNT_TRADE_ID");
            if (pMessage.propertyExists("VERSION"))
                version = pMessage.getDoubleProperty("VERSION");
            auditTypeCode = MessageUtils.getMessageValue("AUDIT_TYPE_CODE", pMessage).trim();
            notifyContractsFlag = MessageUtils.getMessageValue("NOTIFY_CONTRACTS_FLAG", pMessage);
            cdtyCode = MessageUtils.getMessageValue("CDTY_CODE", pMessage);
            tradeSttlTypeCode = MessageUtils.getMessageValue("TRADE_STTL_TYPE_CODE", pMessage);
            tradeAuditID = df.format(pMessage.getDoubleProperty("TRADE_AUDIT_ID"));
            empId = pMessage.getDoubleProperty("EMP_ID");
            cptySn = MessageUtils.getMessageValue("CPTY_SHORT_NAME", pMessage);
            seCptySn = MessageUtils.getMessageValue("CMPNY_SHORT_NAME", pMessage);
            updateBusnDt = MessageUtils.getMessageValue("UPDATE_BUSN_DT", pMessage);
            brokerSn = MessageUtils.getMessageValue("BROKERSN", pMessage);
            instCode = MessageUtils.getMessageValue("INST_CODE", pMessage);
            refSN = MessageUtils.getMessageValue("RFRNCE_SHORT_NAME", pMessage);
            bookSn = MessageUtils.getMessageValue("BK_SHORT_NAME", pMessage);
            updateTableName = MessageUtils.getMessageValue("UPDATE_TABLE_NAME", pMessage);
            if (trdSysCode.equalsIgnoreCase("SYM"))
            {
//          Removed 10/29/2013 as per Jack
//                if (MessageUtils.getMessageValue("EMP_NAME", pMessage).length() > 0)
//                    empName = MessageUtils.getMessageValue("EMP_NAME", pMessage).toUpperCase();
//                else
                    empName  = "NONE";
            }
            else
                empName = empDAO.getEmpName(empId);
            

            Logger.getLogger(OpsTrackingTradeAlertService.class).info("AuditID = " + tradeAuditID + ", " +
                    trdSysCode + " " + ticketID + " " + tradeTypeCode + " message received... ");

            opsTrackingTradeAlertDataRec = new OpsTrackingTradeAlertDataRec(pMessage);
            opsTrackingTradeAlertDataRec.empName = empName;
            

            /* When a JMS trade is deleted (auditTypeCode == VOID), the cdtyCode is null.
               Because it is null, the assignment that uses it below raises a NullPointerException.
               By setting it to a non-null value, the test is performed without blowing up. */
            if (cdtyCode == null)
                cdtyCode = "NONE";
            if (brokerSn == null)
                brokerSn = "";

            //3/12/2007 Israel - Sets variable used to skip JMS delete/void.
            isTradeValid = true;
            if (pMessage.getStringProperty("CDTY_CODE") == null ||
                pMessage.getStringProperty("CMPNY_SHORT_NAME") == null ||
                pMessage.getStringProperty("INST_CODE") == null){
                isTradeValid = false;
                // 5/11/2007 Israel -- update trade data when dumping an ICTS void
                otTRADE_DATA_dao.updateSymphonyTradeDataVoid(tradeID);
            }

            //3/27/2008 Israel - Detects double-booked trades (for Alpha)
            //Double book filter code-- uncomment for EMS publisher support
            if (ictsBookList.indexOf(bookSn)>-1 &&
                trdSysCode.equalsIgnoreCase("AFF"))
                isDoubleBooked = true;

            //11/20/2007 Israel - Support ignoring these trade types that are edited after final approval.
            isIgnoreTradeType = false;
            if (((trdSysCode.equalsIgnoreCase("SYM") && instCode.equalsIgnoreCase("STORAGE")) ||
                  tradeTypeCode.equalsIgnoreCase("STRG") ||
                  tradeTypeCode.equalsIgnoreCase("TRNSM")) &&
                  auditTypeCode.equalsIgnoreCase("EDIT")){
                if (otTRADE_SUMMARY_dao.isTradeFinalApproved(tradeID))
                    isIgnoreTradeType = true;
            }

            //5/23/2008 Israel - Exclude trades for OpsDashboard enhancements.
            isExcludedType = false;
            //boolean isExcludedCptySnCorp =  true;


         //   boolean isExcludedCptySnCorp =  (seCptySn.equalsIgnoreCase("SEMPRA EGY") ||
         //                                   seCptySn.equalsIgnoreCase("RBS SET"));

            //boolean isExcludedCptySnSarl = false;


        //    boolean isExcludedCptySnSarl = (cptySn.equalsIgnoreCase("SOTSARL") ||
         //                                   cptySn.equalsIgnoreCase("RBS SING") ||
         //                                   cptySn.equalsIgnoreCase("RBS SEEL") ||
         //                                   cptySn.equalsIgnoreCase("RBSSOTSARL"));
            //boolean isExcludedCptySnAll = isExcludedCptySnCorp && isExcludedCptySnSarl;
            //if (isExcludedCptySnAll && refSN.equalsIgnoreCase("OPTION EXERCISE"))
            if (refSN.equalsIgnoreCase("OPTION EXERCISE"))
                isExcludedType = true;

           // samy: 10/08/2009 for exotic basket handling

            if (tradeTypeCode.equalsIgnoreCase("BASKT")){
               // check for exotic basket trades
               isNotExoticBasket = !tsDATA_dao.isExoticBasketTrade(tradeID);
            }


            //6/18/2008 Israel - Exclude trades --
            isIgnoreUpdateTradeData = false;
            if ( trdSysCode.equalsIgnoreCase("AFF") &&
                 auditTypeCode.equalsIgnoreCase("EDIT") &&
                notifyContractsFlag.equalsIgnoreCase("Y") &&
                isTradeValid &&
                !isDoubleBooked &&
                !isIgnoreTradeType &&
                !isExcludedType &&
                !seCptySn.equalsIgnoreCase("DELTAHOUSE") &&
                !seCptySn.equalsIgnoreCase("SEMPRA CON") &&
                !cptySn.equalsIgnoreCase("INVNTRY TR") &&
                !cptySn.equalsIgnoreCase("HOUSE") &&
                !refSN.equalsIgnoreCase("WELLPOINT") &&
                !auditTypeCode.equalsIgnoreCase("ACCPT") &&
                !tradeTypeCode.equalsIgnoreCase("FUT") &&
                !tradeTypeCode.equalsIgnoreCase("FWD") &&
                !tradeTypeCode.equalsIgnoreCase("OPFUT") &&
                !tradeTypeCode.equalsIgnoreCase("SAFEE") &&
                !tradeTypeCode.equalsIgnoreCase("EXCHANGE") &&
              //  !tradeTypeCode.equalsIgnoreCase("BASKT") &&
                 !isNotExoticBasket &&              // to process only exotic basket trade
                //tradeTypeCode.equalsIgnoreCase("STORAGE") ||
                !tradeTypeCode.equalsIgnoreCase("FUTURE")) {
                otDataRec = otTRADE_DATA_dao.getOpsTrackingTRADE_DATA_rec(tradeID);
                tsDataRec = tsDATA_dao.getTradingSystemDATA_rec(trdSysCode, tradeID, tradeTypeCode);
                //1. For every edit, was only the XRef changed?
                if (tsDATA_dao.isOTTradeDataOnlyXREFChanged(tsDataRec,otDataRec))
                    isIgnoreUpdateTradeData = true;
                //2. Find out if any ops data was changed. If not, ignore.
                else if (!tsDATA_dao.isOTTradeDataAnyChanged(tsDataRec,otDataRec))
                    isIgnoreUpdateTradeData = true;
            }


            //If house edit then send email.
            //House trades can be Option Exercise, Day power or Gas, as per SW
            if (notifyContractsFlag.equalsIgnoreCase("Y") &&
                    cptySn.equalsIgnoreCase("HOUSE") &&
                    //Double book filter code-- uncomment for EMS publisher support
                    !isDoubleBooked &&
                    !auditTypeCode.equalsIgnoreCase("NEW") &&
                    !auditTypeCode.equalsIgnoreCase("ACCPT") &&
                    !tradeTypeCode.equalsIgnoreCase("FUT") &&
                    !tradeTypeCode.equalsIgnoreCase("FWD") &&
                    !tradeTypeCode.equalsIgnoreCase("OPFUT") &&
                    !tradeTypeCode.equalsIgnoreCase("SAFEE") &&
                    !tradeTypeCode.equalsIgnoreCase("EXCHANGE") &&
                  //  !tradeTypeCode.equalsIgnoreCase("BASKT") &&
                    !isNotExoticBasket &&              // to process only exotic basket trade
                    //!tradeTypeCode.equalsIgnoreCase("STORAGE") &&
                    !tradeTypeCode.equalsIgnoreCase("FUTURE")) {
                tsDATA_rec = tsDATA_dao.getTradingSystemDATA_rec(opsTrackingTradeAlertDataRec.tradingSystem,
                        opsTrackingTradeAlertDataRec.tradeID,
                        opsTrackingTradeAlertDataRec.tradeTypeCode);
                Date dtUpdateBusnDt = sdfCompare.parse(updateBusnDt);
                Date dtInceptionDt = tsDATA_rec.INCEPTION_DT;
                boolean userIdOK = (empId != 11871) && (empId != 28238); //bkdoor and rtbkdoor
                if ((dtInceptionDt.compareTo(dtUpdateBusnDt) != 0) && userIdOK ) {
                    book = tsDATA_rec.BOOK; //only way to get accurate book for JMS
                    String houseEditSubject = "House trade edited: " + trdSysCode + " " + ticketID;
                    String editedBy = "";
                    if (trdSysCode.equalsIgnoreCase("AFF"))
                        editedBy = "A house trade was edited by " + empDAO.getEmpName(empId);
                    else
                        editedBy = "A house trade was edited.";

                    String houseEditMailDesc =
                            editedBy +
                            //"A house trade was edited by " + empDAO.getEmpName(empId) +
                            "\n  Cdty:  " + cdtyCode +
                            "\n  Book:  " + book +
                            "\n  Trade: " + trdSysCode + " " + ticketID +
                            "\n  affinityDBInfoName:    " + affinityDBInfoDisplayName +
                            "\n  opsTrackingDBInfoName: " + opsTrackingDBInfoDisplayName;

                    //Send to Israel only unless it is running in production.
                    //String houseEditSendTo = houseEditAddress;
                    //String houseEditToName = "House Edit Recipients";
                    //if (!environment.equalsIgnoreCase("PROD"))
                    //    houseEditSendTo = "ifrankel@amphorainc.com";

//                    mailUtils.sendMail(houseEditSendTo, houseEditToName, sentFromAddress, houseEditFromName,
//                            houseEditSubject, houseEditMailDesc, "");
                }
            }

            // samy : 10/09/2009  added to process exotic basket trades
            
            if ((auditTypeCode.equalsIgnoreCase("BASKT"))) {

                String basketProcessType = tsDATA_dao.getBasketMemberProcessingFlag(tradeID);
               // Y- basketed exotic trade
                // R - Unbasketed exotic trade
                if ("Y".equalsIgnoreCase(basketProcessType) || "R".equalsIgnoreCase(basketProcessType)) {
                    otReturnDataRec  = otTradeAlertDAO.processBasketMemberTrade(opsTrackingTradeAlertDataRec,basketProcessType);
                    Logger.getLogger(OpsTrackingTradeAlertService.class).info("Basket Member Trade Processed: " +
                        auditTypeCode + "," + tradeTypeCode + ": " + trdSysCode + " " + ticketID);
                }
                else {
                    otIGNORED_NOTIFICATIONS.insertIgnoredNotifications(pMessage);
                    Logger.getLogger(OpsTrackingTradeAlertService.class).info("IGNORED_NOTIFICATIONS inserted: " +
                        auditTypeCode + "," + tradeTypeCode + ": " + trdSysCode + " " + ticketID);
                }

            }
            else if (notifyContractsFlag.equalsIgnoreCase("N") ||
                !isTradeValid ||
                isDoubleBooked ||
                isIgnoreTradeType ||
                isIgnoreUpdateTradeData ||
                isExcludedType ||
                seCptySn.equalsIgnoreCase("DELTAHOUSE") ||
                seCptySn.equalsIgnoreCase("SEMPRA CON") ||
                cptySn.equalsIgnoreCase("INVNTRY TR") ||
                cptySn.equalsIgnoreCase("HOUSE") ||
                refSN.equalsIgnoreCase("WELLPOINT") ||
                tradeTypeCode.equalsIgnoreCase("FUT") ||
                tradeTypeCode.equalsIgnoreCase("FWD") ||
                tradeTypeCode.equalsIgnoreCase("OPFUT") ||
                tradeTypeCode.equalsIgnoreCase("SAFEE") ||
                tradeTypeCode.equalsIgnoreCase("EXCHANGE") ||
              //  tradeTypeCode.equalsIgnoreCase("BASKT") ||
                 isNotExoticBasket ||              // to process only exotic basket trade
                //tradeTypeCode.equalsIgnoreCase("STORAGE") ||
                tradeTypeCode.equalsIgnoreCase("FUTURE")){
                otIGNORED_NOTIFICATIONS.insertIgnoredNotifications(pMessage);
                Logger.getLogger(OpsTrackingTradeAlertService.class).info("IGNORED_NOTIFICATIONS inserted: " +
                        auditTypeCode + "," + tradeTypeCode + ": " + trdSysCode + " " + ticketID);

                //12/20/2006 Israel -- added to allow updating of trade data by ignored audit type codes.
                //The audit codes it picks up will necessarily by notifyContractsFlag=N otherwise the edit
                //procedure would handle these.
                //1/19/2007 Israel -- Excluding ignored types so view doesn't try to retrieve data for them because
                //view won't return data for BASKT even if the audit type code = BKEDT, etc.
                boolean auditTypeCodeOK = StringUtils.isValueInArray(auditTypeCode, updateTradeDataAuditCodeList);
                try {
                    if (auditTypeCodeOK &&
                        isTradeValid &&
                        !isDoubleBooked &&
                        !isIgnoreTradeType &&
                        !isIgnoreUpdateTradeData &&
                        !isExcludedType &&
                        !seCptySn.equalsIgnoreCase("DELTAHOUSE") &&
                        !seCptySn.equalsIgnoreCase("SEMPRA CON") &&
                        !cptySn.equalsIgnoreCase("INVNTRY TR") &&
                        !cptySn.equalsIgnoreCase("HOUSE") &&
                        !refSN.equalsIgnoreCase("WELLPOINT") &&
                        !tradeTypeCode.equalsIgnoreCase("FUT") &&
                        !tradeTypeCode.equalsIgnoreCase("FWD") &&
                        !tradeTypeCode.equalsIgnoreCase("OPFUT") &&
                        !tradeTypeCode.equalsIgnoreCase("SAFEE") &&
                        !tradeTypeCode.equalsIgnoreCase("EXCHANGE") &&
                     //   !tradeTypeCode.equalsIgnoreCase("BASKT") &&
                        !isNotExoticBasket &&              // to process only exotic basket trade
                        //!tradeTypeCode.equalsIgnoreCase("STORAGE") &&
                        !tradeTypeCode.equalsIgnoreCase("FUTURE")) {
                        tsDATA_rec = tsDATA_dao.getTradingSystemDATA_rec(opsTrackingTradeAlertDataRec.tradingSystem,
                                opsTrackingTradeAlertDataRec.tradeID,
                                opsTrackingTradeAlertDataRec.tradeTypeCode);

                        //7/6/09 Israel-- Get data to call routine to insert trade_data_chg row
                        OpsTrackingTRADE_DATA_rec otTradeDataRec = new OpsTrackingTRADE_DATA_rec();
                        otTradeDataRec = otTRADE_DATA_dao.getOpsTrackingTRADE_DATA_rec(opsTrackingTradeAlertDataRec.tradeID);
                        
                        otTRADE_DATA_dao.updateTradeData(tsDATA_rec, auditTypeCode);
                        //7/6/09 Israel-- insert trade_data_chg row as necessary
                        otTRADE_DATA_CHG_dao.insertTradeDataChgRows(otTradeDataRec, tsDATA_rec, auditTypeCode, empName);
                        Logger.getLogger(OpsTrackingTradeAlertService.class).info("Trade data updated for ignored audit code: " +
                                auditTypeCode + "," + tradeTypeCode + ": " + trdSysCode + " " + ticketID);
                    }
                } catch (aff.confirm.common.ottradealert.exceptions.RowNotFoundException e) {
                    Logger.getLogger(OpsTrackingTradeAlertService.class).info("Trade data not found for audit code: " +
                            auditTypeCode + "," + tradeTypeCode + ": " + trdSysCode + " " + ticketID);
                }
            } else if (isBackDoor(auditTypeCode, empId) ) {
                //NOTE: Backdoor trades may have originally been econfirm_v1, but these are typically processed
                //as backdoor trades long after the time the econfirm_v1 status will have been resolved.
                otTradeAlertDAO.processBackdoorTrade(opsTrackingTradeAlertDataRec);
                Logger.getLogger(OpsTrackingTradeAlertService.class).info(" Backdoor trade processed: " + ticketID);
            } else if (auditTypeCode.equalsIgnoreCase("VOID")) {

                  // 10/12/2009 Samy : check the trade is the member of exotic basket,
                    // then put it in the ignored notication
                    // if the trade is unbasketed, then that the trade gets processed with
                    // current trading system data in the ops tracking scheme
                 if (tsDATA_dao.isExoticBasketMember(opsTrackingTradeAlertDataRec.tradeID)) {

                        otIGNORED_NOTIFICATIONS.insertIgnoredNotifications(pMessage);
                        Logger.getLogger(OpsTrackingTradeAlertService.class).info(trdSysCode + " exotic member ignored: " +
                                auditTypeCode + "," + tradeTypeCode + ": " + ticketID);
                    }
                else {
                    //ICTS uses VOID, except it has trailing blanks which were trimmed off above.
                    otReturnDataRec = otTradeAlertDAO.processEditedTrade(opsTrackingTradeAlertDataRec);
                    eConfirmProductID = otReturnDataRec.ecProductID;
                    eConfirmAction = otReturnDataRec.ecAction;
                    eConfirmBkrAction = otReturnDataRec.ecBkrAction;
                 }
            } else if (auditTypeCode.equalsIgnoreCase("ACCPT")) {
                 // 10/12/2009 Samy : check the trade is the member of exotic basket,
                    // then put it in the ignored notication
                    // if the trade is unbasketed, then that the trade gets processed with
                    // current trading system data in the ops tracking scheme
                if (tsDATA_dao.isExoticBasketMember(opsTrackingTradeAlertDataRec.tradeID)) {

                        otIGNORED_NOTIFICATIONS.insertIgnoredNotifications(pMessage);
                        Logger.getLogger(OpsTrackingTradeAlertService.class).info(trdSysCode + " exotic member ignored: " +
                                auditTypeCode + "," + tradeTypeCode + ": " + ticketID);
                    }
                else {
                    //Leave the initial eConfirmAction value set to 'NONE'.
                    otTradeAlertDAO.processEditedTrade(opsTrackingTradeAlertDataRec);
                }
            } else if (auditTypeCode.equalsIgnoreCase("NEW")) {
                //Contains efetAction field, accessed further down
                otReturnDataRec = otTradeAlertDAO.processNewTrade(opsTrackingTradeAlertDataRec);
                eConfirmProductID = otReturnDataRec.ecProductID;
                if (eConfirmProductID > 0) {
                    eConfirmAction = otReturnDataRec.ecAction;
                    eConfirmBkrAction = otReturnDataRec.ecBkrAction;
                }

                setPriority(trdSysCode,tradeID,"I");

                Logger.getLogger(OpsTrackingTradeAlertService.class).info(trdSysCode + " inserted: " +
                        tradeTypeCode + ": " + ticketID);
            } else {
                //Contains efetAction field, accessed further down
                if (opsTrackingTradeNotifyDao.isTradeNotifyExist(opsTrackingTradeAlertDataRec.tradeID, version) ||
                       otIGNORED_NOTIFICATIONS.auditExistsInIgnoredNotify(opsTrackingTradeAlertDataRec.tradeID,version)                         
                        ){
//                    mailUtils.sendMail("ConfirmSupport@amphorainc.com", "Confirm Support", sentFromAddress, sentFromName, "WARNING: Edit Constraint Violation has been handled.  Process has not stopped.", "Edited Trade already exists in Trade Notify; TradeID: " + ticketID + " AuditID: " + tradeAuditID, "");
                    Logger.getLogger(OpsTrackingTradeAlertService.class).info(trdSysCode + " Edit Already Exists.  No Update of record. " +
                            tradeTypeCode + ": " + ticketID);
                }
                else{
                    // 10/12/2009 Samy : check the trade is the member of exotic basket,
                    // then put it in the ignored notication
                    // if the trade is unbasketed, then that the trade gets processed with
                    // current trading system data in the ops tracking scheme
                    if (tsDATA_dao.isExoticBasketMember(opsTrackingTradeAlertDataRec.tradeID)) {

                        otIGNORED_NOTIFICATIONS.insertIgnoredNotifications(pMessage);                        
                        Logger.getLogger(OpsTrackingTradeAlertService.class).info(trdSysCode + " exotic member ignored: " +
                                auditTypeCode + "," + tradeTypeCode + ": " + ticketID);
                    }
                    else {  // process the edit.
                        otReturnDataRec = otTradeAlertDAO.processEditedTrade(opsTrackingTradeAlertDataRec);
                        eConfirmProductID = otReturnDataRec.ecProductID;
                        eConfirmAction = otReturnDataRec.ecAction;
                        eConfirmBkrAction = otReturnDataRec.ecBkrAction;
                        setPriority(trdSysCode,tradeID,"U");
                        Logger.getLogger(OpsTrackingTradeAlertService.class).info(trdSysCode + " updated: " +
                                auditTypeCode + "," + tradeTypeCode + ": " + ticketID);
                    }
                }
            }

            //7/23/2009 Israel Have system auto insert rqmts on ec cancel
            //Insert trade rqmt rows when econfirm_v1/efet trade has been cancelled.
            boolean autoConfirmCancel = (eConfirmAction.equalsIgnoreCase("CANCEL") ||
                                        otReturnDataRec.efetCptyAction.equalsIgnoreCase(EC_CANCEL) ||
                                        otReturnDataRec.efetBkrAction.equalsIgnoreCase(EC_CANCEL) );

            if (autoConfirmCancel && !auditTypeCode.equalsIgnoreCase("VOID")) {
                tsDATA_rec = tsDATA_dao.getTradingSystemDATA_rec(opsTrackingTradeAlertDataRec.tradingSystem,
                        opsTrackingTradeAlertDataRec.tradeID,
                        opsTrackingTradeAlertDataRec.tradeTypeCode);

                otTRADE_RQMT_dao.determineRqmts(opsTrackingTradeAlertDataRec, tsDATA_rec);
                otTRADE_RQMT_dao.ReconcileRqmtLists(opsTrackingTradeAlertDataRec.tradeID);
                otTRADE_RQMT_dao.insertTradeRqmts(opsTrackingTradeAlertDataRec, tsDATA_rec, false);
            }
            //Publish eConfirm submit message.
            if (eConfirmAction.equalsIgnoreCase("SUBMIT") || eConfirmAction.equalsIgnoreCase("CANCEL") || eConfirmBkrAction.equalsIgnoreCase("SUBMIT") || eConfirmBkrAction.equalsIgnoreCase("CANCEL") ) {
                // MThoresen 4-18-2007: Add field to sendEconfirmMessage for click and confirm.
                sendEConfirmMessage(pMessage, eConfirmProductID, eConfirmAction, opsTrackingTradeAlertDataRec.isClickAndConfirm,eConfirmBkrAction);
                // MThoresen 4-18-2007: Add field to sendEconfirmMessage for click and confirm. Once submitted, insert a record into the
                // ec_trade_summary table. Do not need to call the update.  We just need a row in the summary table
                if(opsTrackingTradeAlertDataRec.isClickAndConfirm){
//                    if (otTradeAlertDAO.geteConfirmDAO().isECTradeSummaryExist(opsTrackingTradeAlertDataRec.tradeID) == false){
//                        otTradeAlertDAO.geteConfirmDAO().insertECTradeSummary(new EConfirmSummary_DataRec(opsTrackingTradeAlertDataRec.tradingSystem,
                    if (eConfirmData.isECTradeSummaryExist(opsTrackingTradeAlertDataRec.tradingSystem, opsTrackingTradeAlertDataRec.tradeID) == false){
                        eConfirmData.insertECTradeSummary(new EConfirmSummary_DataRec(opsTrackingTradeAlertDataRec.tradingSystem,
                                opsTrackingTradeAlertDataRec.tradeID,
                                otReturnDataRec.ecProductID,
                                "CLICK",
                                "",
                                "",
                                "N"));
                    }
                }else{
                    // what if this was a c&c before, and now is not.  What should be do to the ECTradeSummary record that
                    // could have been added above?
                }
                Logger.getLogger(OpsTrackingTradeAlertService.class).info("Sent ECONFIRM_TRADE_SUBMIT action=" +
                        eConfirmAction + ", productID=" + eConfirmProductID + " " + trdSysCode + " " + ticketID);
            } else if (eConfirmAction.equalsIgnoreCase("MATCHED") &&
                    !auditTypeCode.equalsIgnoreCase("ACCPT")) {
                //IF 2/23/2004 added "eConfirm" to the subject
                String subject = "Matched trade was edited: " + trdSysCode + " " + ticketID;
                String mailDesc = "Matched trade was edited by " +
                        empDAO.getEmpName(empId) +
                        ", CptySn=" + cptySn +
                        ", Trade=" + trdSysCode + " " + ticketID +
                        ", affinityDBInfoName=" + affinityDBInfoDisplayName +
                        ", opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                //Send to Israel only unless it is running in production.
                String sendTo;
                String toName = sendToName;
                /*
                if (cdtyCode.equalsIgnoreCase("ELEC") && ( seCptySn.equalsIgnoreCase("SET EUROPE") || seCptySn.equalsIgnoreCase("RBS SEEL") ) ) {
                    sendTo = sendToUKElec;
                    toName = "Power Trade Correction UK";
                }
                else if (cdtyCode.equalsIgnoreCase("ELEC")) {
                    sendTo = sendToUSElec;
                    toName = "Power Trade Corrections US";
                }
                else if (cdtyCode.equalsIgnoreCase("NGAS") && ( seCptySn.equalsIgnoreCase("SET EUROPE") || seCptySn.equalsIgnoreCase("RBS SEEL") ) ) {
                    sendTo = sendToUKNGas;
                    toName = "Natural Gas Correction UK";
                }
                else if (cdtyCode.equalsIgnoreCase("NGAS") && (seCptySn.equalsIgnoreCase("SGE USA") || seCptySn.equalsIgnoreCase("SGE CAN"))) {
                    sendTo = sendToSGEGas;
                    toName = "SGE Trade Correction";
                }
                else if (cdtyCode.equalsIgnoreCase("ELEC") && (seCptySn.equalsIgnoreCase("SGE USA") || seCptySn.equalsIgnoreCase("SGE CAN"))) {
                    sendTo = sendToSGEPower;
                    toName = "SGE Trade Correction";
                }
                else if (cdtyCode.equalsIgnoreCase("NGAS")) {
                    sendTo = sendToUSNGas;
                    toName = "Natural Gas Corrections US";
                }
                else if (seCptySn.equalsIgnoreCase("SGE USA") || seCptySn.equalsIgnoreCase("SGE CAN")) {
                    sendTo = sendToSGEOil;
                    toName = "SGE Trade Correction";

                }
                else {      */
                    sendTo = sendToUSOil;
                    toName = "Oil Confirmations Stamford";
              //  }

                //sendTo = sendToAddress;
                //toName = sendToName;
                if (!environment.equalsIgnoreCase("PROD")){
                    sendTo = "ifrankel@amphorainc.com";
                    toName = "OpsTracking Dev Test";
                }

//                mailUtils.sendMail(sendTo, toName, sentFromAddress, sentFromName, subject, mailDesc, "");
            }

            //EFET - Submit efet cpty trades
            if (otReturnDataRec.efetCptyAction.equalsIgnoreCase(EC_SUBMIT) ||
                    otReturnDataRec.efetCptyAction.equalsIgnoreCase(EC_CANCEL)) {
                //IF 12/5/05 - was submitting edits as new trades
                sendEFETMessage(pMessage, otReturnDataRec.efetCptyAction, otReturnDataRec.efetCptySubmitState, "CNF",
                                otReturnDataRec.efetCnfReceiver);
                Logger.getLogger(OpsTrackingTradeAlertService.class).info("Sent EFET_TRADE_SUBMIT CNF action=" +
                        otReturnDataRec.efetCptyAction + " " + trdSysCode + " " + ticketID);
            } else if (otReturnDataRec.efetCptyAction.equalsIgnoreCase("MATCHED") &&
                    !auditTypeCode.equalsIgnoreCase("ACCPT")) {
                String subject = "Matched trade was edited: " + trdSysCode + " " + ticketID;
                String mailDesc = "Matched trade was edited by " +
                        empDAO.getEmpName(empId) +
                        ", CptySn=" + cptySn +
                        ", Trade=" + trdSysCode + " " + ticketID +
                        ", affinityDBInfoName=" + affinityDBInfoDisplayName +
                        ", opsTrackingDBInfoName=" + opsTrackingDBInfoDisplayName;

                //Send to Israel only unless it is running in production.
                String sendTo = efetWarningAddress;
                String toName = "EFET Warning Recipients";
                if (!environment.equalsIgnoreCase("PROD"))
                    sendTo = "ifrankel@amphorainc.com";

//                mailUtils.sendMail(sendTo, toName, sentFromAddress, efetSentFromName, subject, mailDesc, "");
            }

            //EFET - Submit efet broker trades
            if (otReturnDataRec.efetBkrAction.equalsIgnoreCase(EC_SUBMIT) ||
                    otReturnDataRec.efetBkrAction.equalsIgnoreCase(EC_CANCEL)) {
                sendEFETMessage(pMessage, otReturnDataRec.efetBkrAction, otReturnDataRec.efetBkrSubmitState, "BFI", "B");
                Logger.getLogger(OpsTrackingTradeAlertService.class).info("Sent EFET_TRADE_SUBMIT BFI action=" +
                        otReturnDataRec.efetBkrAction + " " + trdSysCode + " " + ticketID);
            }

            opsTrackingConnection.commit();
            Logger.getLogger(OpsTrackingTradeAlertService.class).info( "AuditID= " + tradeAuditID + ", " +
                    trdSysCode + " " + ticketID + " " +
                    tradeTypeCode + " message processed.");
        } catch (Exception e) {
            try {
                opsTrackingConnection.rollback();
            } catch (SQLException e1) { }
            String error = "---------------Rolled Back: AuditID= " + tradeAuditID + ", " + auditTypeCode + " ," +
                    tradeTypeCode + ": " + trdSysCode + " " + ticketID + ": " + e;

            if (e instanceof NullPointerException) {
//               Logger.getLogger(OpsTrackingTradeAlertService.class).info("Caught exception: " +  ticketID + ": " + e);
               try {
//                   mailUtils.sendMail("ConfirmSupport@amphorainc.com", "Confirm Support", sentFromAddress, sentFromName, "WARNING:  Null pointer exception has been handled.  Process has not stopped.", "TradeID: " + ticketID + " AuditID: " + tradeAuditID, "");
               } catch (Exception e3) {}
            }
            else if ((e instanceof SQLException) && e.getMessage().startsWith("ORA-00001: unique constraint")){
                try {
                    if (environment.equalsIgnoreCase("PROD"))
//                        mailUtils.sendMail("ConfirmSupport@amphorainc.com", "Confirm Support", sentFromAddress, sentFromName, "WARNING:  Unique Constraint Violation has been handled.  Process has not stopped.", "New Trade; TradeID: " + ticketID + " AuditID: " + tradeAuditID, "");
                    Logger.getLogger(OpsTrackingTradeAlertService.class).info(trdSysCode + " Already Exists.  No insertion of record. " +
                             tradeTypeCode + ": " + ticketID);
                } catch (Exception e2) {}
            }
            else
                throw new StopServiceException(error);
        }
    }

    private void setPriority(String pTrdSysCode, double pTradeId, String pAction) throws Exception {
        otTRADE_PRIORITY_rec.init();
        otTRADE_PRIORITY_rec = opsTrackingPriorityCalc.getTradePriority(pTrdSysCode, pTradeId);
        if (pAction.equalsIgnoreCase("U")) {
            if (otTRADE_PRIORITY_dao.isTradePriorityExist(pTradeId))
                otTRADE_PRIORITY_dao.updateTradePriority(pTradeId, otTRADE_PRIORITY_rec.PRIORITY,
                        otTRADE_PRIORITY_rec.PL_AMT);
            else
                otTRADE_PRIORITY_dao.insertTradePriority(pTradeId, otTRADE_PRIORITY_rec.PRIORITY,
                        otTRADE_PRIORITY_rec.PL_AMT);
        } else if (pAction.equalsIgnoreCase("I"))
            otTRADE_PRIORITY_dao.insertTradePriority(pTradeId, otTRADE_PRIORITY_rec.PRIORITY,
                    otTRADE_PRIORITY_rec.PL_AMT);
        else
            throw new StopServiceException("setPriority: Internal error: Unknown action=" + pAction);
    }

    private boolean isBackDoor(String pAuditTypeCode, double pEmpId){
        boolean auditType = !pAuditTypeCode.equalsIgnoreCase("NEW") &&
                            !pAuditTypeCode.equalsIgnoreCase("ACCPT");
        boolean empId = pEmpId == 11871 || pEmpId == 28238;
        boolean backDoor = auditType && empId;
        return backDoor;
    }

    private void sendEConfirmMessage(Message pMessage, int pEConfirmProductID, String pEConfirmAction, boolean pIsClickAndConfirm,String pBkrEConfirmAction) throws JMSException {
        String clickAndConfirm = "N";
        Message message;
        message = getQueueSession().createMessage();
        copyMessage(pMessage, message);
        message.setIntProperty("EC_PRODUCT_ID", pEConfirmProductID);
        message.setStringProperty("EC_ACTION", pEConfirmAction);
        // MThoresen - 4-18-2007: new message property to handle click and confirm.
        if (pIsClickAndConfirm)
            clickAndConfirm = "Y";
        message.setStringProperty("EC_CLICK_CONFIRM", clickAndConfirm);
        message.setStringProperty("EC_BKR_ACTION",pBkrEConfirmAction);
        senderEConfirmTradeSubmit.send(message);
        message = null;


    }

    private void sendEFETMessage(Message pMessage, String pEFETAction, String pEFETSubmitState,
                                 String pDocType, String pReceiverType) throws JMSException {
        Message message;
        message = getQueueSession().createMessage();
        copyMessage(pMessage, message);
        message.setStringProperty("EFET_ACTION", pEFETAction);
        //NEW, EDIT
        message.setStringProperty("EFET_SUBMIT_STATE", pEFETSubmitState);
        message.setStringProperty("DOC_TYPE", pDocType);
        message.setStringProperty("RECEIVER_TYPE", pReceiverType);
        senderEFETTradeSubmit.send(message);
        message = null;
    }

    /*private void sendFeedConfirmMessage(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec, String pAction )
            throws JMSException {
        double rqmtID = 0;
        double prmntTradeConfirmId = 0;
        String confirmStatusInd = "";
        String noConfirmReason = "";
        if (pOpsTrackingTradeAlertDataRec.insertTradeConfirm){
            rqmtID = pOpsTrackingTradeAlertDataRec.insertRqmtId;
            prmntTradeConfirmId = pOpsTrackingTradeAlertDataRec.insertPrmntTradeConfirmId;
            confirmStatusInd = pOpsTrackingTradeAlertDataRec.insertConfirmStatusInd;
            noConfirmReason = pOpsTrackingTradeAlertDataRec.insertNoConfirmReason;
        }
        else if (pOpsTrackingTradeAlertDataRec.cancelTradeConfirm){
            rqmtID = pOpsTrackingTradeAlertDataRec.cancelRqmtId;
            prmntTradeConfirmId = pOpsTrackingTradeAlertDataRec.cancelPrmntTradeConfirmId;
            confirmStatusInd = pOpsTrackingTradeAlertDataRec.cancelConfirmStatusInd;
            noConfirmReason = pOpsTrackingTradeAlertDataRec.cancelNoConfirmReason;
        }

        Message message;
        message = getQueueSession().createMessage();
        message.setDoubleProperty("RQMT_ID", rqmtID);
        message.setDoubleProperty("VERSION", pOpsTrackingTradeAlertDataRec.version);
        message.setStringProperty("TRADING_SYSTEM", pOpsTrackingTradeAlertDataRec.sourceSystemCode);
        message.setDoubleProperty("TRADE_ID", pOpsTrackingTradeAlertDataRec.tradeID);
        message.setStringProperty("STR_TRADE_DT",pOpsTrackingTradeAlertDataRec.strTradeDt);
        message.setStringProperty("CPTY_SN",pOpsTrackingTradeAlertDataRec.cptySn);
        message.setStringProperty("CDTY_CODE",pOpsTrackingTradeAlertDataRec.cdtyCode);
        message.setStringProperty("TRADE_TYPE_CODE",pOpsTrackingTradeAlertDataRec.tradeTypeCode);
        message.setStringProperty("ACTION", pAction);
        message.setDoubleProperty("PRMNT_TRADE_CONFIRM_ID", prmntTradeConfirmId);
        message.setStringProperty("CONFIRM_STATUS_IND", confirmStatusInd);
        message.setStringProperty("NO_CONFIRM_REASON", noConfirmReason);
        senderFeedConfirmAlert.send(message);
        message = null;
    }*/

    protected void onServiceStarting() throws Exception {
        Logger.getLogger(OpsTrackingTradeAlertService.class).info("Executing startService... ");
        init();
    }

    protected void onServiceStoping(){
        otTradeAlertDAO = null;
        //eConfirmDAO = null;
        otIGNORED_NOTIFICATIONS = null;
        empDAO = null;
        opsTrackingPriorityCalc = null;
        otTRADE_PRIORITY_dao = null;
        otTRADE_PRIORITY_rec = null;
        otTRADE_DATA_dao = null;
        otTRADE_SUMMARY_dao = null;
        opsTrackingTradeNotifyDao = null;
        eConfirmData = null;

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
        if(symphonyConnection != null){
            try {
                symphonyConnection.close();
            } catch (SQLException e) {
                log.error(e);
            }
            symphonyConnection = null;
        }
*/
    }

    public void handleNotification(Notification notification, Object o) {
        if (notification.getType().equals("ORACLE_STARTUP")) {
            log.info("STARTUP notification");
            try {
                start();
            } catch (Exception e) {
                Logger.getLogger(OpsTrackingTradeAlertService.class).error("resume(): " + e);
            }
        } else if (notification.getType().equals("ORACLE_SHUTDOWN")) {
            log.info("SHUTDOWN notification");
            try {
                stop();
            } catch (Exception e) {
                Logger.getLogger(OpsTrackingTradeAlertService.class).error("pause(): " + e);
            }
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
        Logger.getLogger(OpsTrackingTradeAlertService.class).info(pConnectionName+"="+dbinfo.getDatabaseName());
        result.setAutoCommit(false);

        return result;
    }

}
