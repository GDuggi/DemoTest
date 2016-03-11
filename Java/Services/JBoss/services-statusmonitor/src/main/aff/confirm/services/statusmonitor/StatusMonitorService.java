package aff.confirm.services.statusmonitor;


import org.jboss.logging.Logger;
import aff.confirm.common.util.MailUtils;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.SQLConnectionAllocationFailure;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskService;
import aff.confirm.jboss.common.util.DbInfoWrapper;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.management.*;
import javax.naming.NamingException;
import java.net.InetAddress;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.SimpleDateFormat;
import java.util.*;


@Startup
@Singleton
public class StatusMonitorService extends TaskService implements StatusMonitorServiceMBean {
    private MBeanServer mBeanServer;
    //private MBeanServer mBeanServer2;
    //1. These two must always match each other.
    //2. They cannot contain a z for timezone or it will convert to GMT
    private SimpleDateFormat sdfDateTime = new SimpleDateFormat("dd-MMM-yyyy hh:mm aa", Locale.US);
    private SimpleDateFormat sdfDateTimeLocal = new SimpleDateFormat("dd-MMM-yyyy hh:mm aa", Locale.US);
    private SimpleDateFormat sdfDayOfWeek = new SimpleDateFormat("E");
    private boolean autoRestartEnabled = false;
    private String smtpHost;
    private String smtpPort;
    private String sendToAddress;
    private MailUtils mailUtils;
    private String sentFromName;
    private String sentFromAddress;
    private String sendToName;
    private String adjustedSendToAddress;
    private String dbInfoName;
    private String dbInfoDisplayName;

    private  String environment;

    public StatusMonitorService() {
        super("affinity.cwf:service=StatusMonitor");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    protected void onServiceStarting() throws Exception {
        Logger.getLogger(StatusMonitorService.class).info("Executing startService... ");
        init();
    }

    protected void onServiceStoping() {
        try {
            close();
        } catch (Exception e) {
            log.error(e);
        }
    }

    public void close() throws Exception {
        mBeanServer = null;
    }

    public void init() throws Exception {
        Logger.getLogger(StatusMonitorService.class).info("Executing init... ");
        mBeanServer = getMBeanServer();
        sdfDateTimeLocal.setTimeZone(TimeZone.getTimeZone("Local"));
        mailUtils = new MailUtils(smtpHost, smtpPort);
        String hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
        sentFromName = "StatusMonitor_" + hostName;
        sentFromAddress = "JBossOn" + hostName + "@amphorainc.com";
        sendToName = "StatusMonitorRecipients";

        setDbDisplayName();
        Logger.getLogger(StatusMonitorService.class).info("Database connection=" + dbInfoDisplayName);
        if (environment.equalsIgnoreCase("PROD"))
            adjustedSendToAddress = sendToAddress;
        else
            adjustedSendToAddress = "ifrankel@amphorainc.com";

        Logger.getLogger(StatusMonitorService.class).info("SendToAddress="+adjustedSendToAddress);
        Logger.getLogger(StatusMonitorService.class).info("Init OK.");
    }

     private void setDbDisplayName() throws NamingException {
         DbInfoWrapper dbinfo = new DbInfoWrapper(dbInfoName);
         dbInfoDisplayName = dbinfo.getDatabaseName();
     }

    public String getDBInfoName() {
        return dbInfoName;
    }

    public void setDBInfoName(String pDBInfoName) {
        this.dbInfoName = pDBInfoName;
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

    public void setSmtpPort(String smtpPort) {
        this.smtpPort = smtpPort;
    }

    public String getSendToAddress() {
        return sendToAddress;
    }

    public void setSendToAddress(String pSendToAddress) {
        this.sendToAddress = pSendToAddress;
    }

    public String getEnv() {
        return environment;
    }

    public void setEnv(String pEnv) {
        environment = pEnv;
    }

    private MBeanServer getMBeanServer()  {
        if (mBeanServer == null) {
            List svrs = MBeanServerFactory.findMBeanServer(null);
            Iterator iter = svrs.iterator();
            while (iter.hasNext()) {
                MBeanServer svr = (MBeanServer) iter.next();
                String domain = svr.getDefaultDomain();
                //Israel - 6/18/2013 -- commented previous code.
                //if (domain.equals("jboss")) {
                    mBeanServer = svr;
                    return mBeanServer;
                //}
            }

            Logger.getLogger(StatusMonitorService.class).info("Error intializing MBeanServer object");
            throw new RuntimeException("Error intializing MBeanServer object. 'jboss' domain not found.");
        }

        return mBeanServer;
    }

    private String getServiceAttribute(String pServiceName, String pAtrribute) {
        String result = null;
        try {
            ObjectName name = new ObjectName(pServiceName);
            result = mBeanServer.getAttribute(name, pAtrribute).toString();
        } catch (InstanceNotFoundException e){
            result = "Not Deployed";
        } catch (Exception e) {
            result = e.toString();
        }
        return result;
    }

    private String invokeService(String pServiceName, String pAction) {
        String result = null;
        try {
            ObjectName name = new ObjectName(pServiceName);
            //result = mBeanServer.getAttribute(name, pAtrribute).toString();
            //Integer i = new Integer("0");
            //Attribute att = new Attribute(pAtrribute, i);
            //mBeanServer.setAttribute(name, att);
            Object[] params = null;
            String[] sig = null;
            mBeanServer.invoke(name,pAction,params,sig);
            result = pServiceName + " " + pAction + " succeeded";
        } catch (InstanceNotFoundException e){
            result = "Not Deployed";
        } catch (Exception e) {
            result = e.toString();
        }
        return result;
    }

    /**
    private String getServiceAttribute2(String pServiceName, String pAtrribute) {
         String result = null;
         try {
             Properties env = new Properties( );
             env.put(Context.PROVIDER_URL, "devjboss2:1099");
             env.put(Context.URL_PKG_PREFIXES, "org.jboss.naming:org.jnp.interfaces");
             env.put(Context.INITIAL_CONTEXT_FACTORY,"org.jnp.interfaces.NamingContextFactory");
             InitialContext trackingJNDI = new InitialContext(env);

             RMIAdaptor server = (RMIAdaptor) trackingJNDI.lookup("jmx/rmi/RMIAdaptor");

             //ObjectName name = new ObjectName("jboss:service=JNDIView");
             //ObjectName name = new ObjectName("sempra.cwf:service=TrackingAlert");
             ObjectName name = new ObjectName(pServiceName);
             //String className = "sempra.services.trackingalert.TrackingService";

             //MBeanInfo info = server.getMBeanInfo(name);
             result = server.getAttribute(name, pAtrribute).toString();
             //result = mBeanServer.getAttribute(name, pAtrribute).toString();
             trackingJNDI = null;
         } catch (InstanceNotFoundException e){
             result = "Not Deployed";
         } catch (Exception e) {
             result = e.toString();
         }
         return result;
     }
    */

    public String getDSProcessorStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=DSProcessor", "StateString");
        String queueDepth = getServiceAttribute("jboss.mq.destination:name=DEALSHEET,service=Queue", "QueueDepth");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }

    public String getEConfirmMonitor2State() throws StopServiceException {
        return getServiceAttribute("sempra.cwf:service=EConfirmMonitor2", "StateString");
    }

    public String getEConfirmStatusPollingState() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=EConfirmStatusPolling", "StateString");
        String sql = "select last_status_datetime dateResult from econfirm.ec_control where id = 0";
        String lastStatus = getDateFromDB(sql, false);
        sql = "select LAST_MESSAGE_DATETIME dateResult from econfirm_v1.ec_control where id = 0";
        String lastError = getDateFromDB(sql, false);
        return "State=" + state + ", LastStatusChange=" + lastStatus + ", LastError=" + lastError;
    }

    public String getEConfirmTradeAlertStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=EConfirmTradeAlert", "StateString");
        String queueDepth = getServiceAttribute("jboss.mq.destination:name=ECONFIRM_TRADE_SUBMIT,service=Queue", "QueueDepth");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }

    public String getEConfirmTradeAlert2Status() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=EConfirmTradeAlert2", "StateString");
        String queueDepth =  getDBQueueDepth("jbossdbq.q_econfirm_alert");
        String sql = "SELECT max(PROCESSED_TS_GMT) dateResult from jbossdbq.q_econfirm_alert " +
                     "where processed_flag = 'Y'";
        String lastProcessed = getDateFromDB(sql, true);
        sql = " select max(submit_timestamp_gmt) dateResult from econfirm.ec_submit_log"   +
                     " where status_message = 'SUBMITTED'"  ;
        String lastSubmit = getDateFromDB(sql, true);
        return "State=" + state + ", QueueDepth=" + queueDepth + ", LastProcessed=" + lastProcessed +
                ", LastSubmitted=" + lastSubmit;
    }

    public String getEFETMonitorState() throws StopServiceException {
        return getServiceAttribute("sempra.cwf:service=EFETMonitor", "StateString");
    }

    public String getEFETStatusPollingState() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=EFETStatusPolling", "StateString");
        /*String sql = "select last_status_datetime dateResult from efet.ec_control where id = 0";
        String lastStatus = getDateFromDB(sql, false);
        sql = "select LAST_MESSAGE_DATETIME dateResult from econfirm_v1.ec_control where id = 0";
        String lastError = getDateFromDB(sql, false);
        return "State=" + state + ", LastStatusChange=" + lastStatus + ", LastError=" + lastError;*/
        return "State=" + state;
    }

    public String getEFETTradeAlertStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=EFETTradeAlert", "StateString");
        String queueDepth = getServiceAttribute("jboss.mq.destination:name=EFET_TRADE_SUBMIT,service=Queue", "QueueDepth");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }

    public String getEFETTradeSubmitterStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=EFETTradeSubmitter", "StateString");
        String queueDepth =  getDBQueueDepth("jbossdbq.q_efet_trade_alert");
        String sql = "SELECT max(PROCESSED_TS_GMT) dateResult from jbossdbq.q_efet_trade_alert " +
                     "where processed_flag = 'Y'";
        String lastProcessed = getDateFromDB(sql, true);
        sql = " select max(submit_timestamp_gmt) dateResult from efet.efet_submit_log"   +
                     " where status_message = 'SUBMITTED'"  ;
        String lastSubmit = getDateFromDB(sql, true);
        return "State=" + state + ", QueueDepth=" + queueDepth + ", LastProcessed=" + lastProcessed +
                ", LastSubmitted=" + lastSubmit;
    }

    public String getICTSProcessorState() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=ICTSProcessor", "StateString");
        String scanningMode = getServiceAttribute("sempra.cwf:service=ICTSProcessor", "ScanningMode");
        return "State=" + state + ", ScanningMode=" + scanningMode;
    }

    public String getOpsTrackingTradeAlertStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=OpsTrackingTradeAlert", "StateString");
        String queueDepth = getServiceAttribute("jboss.mq.destination:name=OPS_TRACKING_TRADE_ALERT,service=Queue", "QueueDepth");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }

    /*public String getFeedConfirmStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=FeedConfirm", "StateString");
        String queueDepth = getServiceAttribute("jboss.mq.destination:name=FEED_CONFIRM_ALERT,service=Queue", "QueueDepth");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }*/

    public String getSTAProcessorStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=STAProcessor", "StateString");
        String queueDepth = getServiceAttribute("jboss.mq.destination:name=SEMPRA_TRADE_ALERT,service=Queue", "QueueDepth");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }

    public String getTALoggerStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=TALogger", "StateString");
        String queueDepth = getServiceAttribute("jboss.mq.destination:name=TRADE_ALERT_LOG,service=Queue", "QueueDepth");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }

    public String getTrackingAlertState() throws StopServiceException {
        return getServiceAttribute("sempra.cwf:service=TrackingAlert", "StateString");
    }

    public String getRTPResenderState() throws StopServiceException {
        return getServiceAttribute("sempra.rt:service=RTPResender", "StateString");
    }

    public String getRTPublisherStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.rt:service=RTPublisher", "StateString");
        String queueDepth = getServiceAttribute("jboss.mq.destination:name=RT_PUBLISHER,service=Queue", "QueueDepth");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }
    public String getDBInfoState() throws StopServiceException {
        return getServiceAttribute("sempra.utils:service=DBInfo", "StateString");
    }
    public String getICTSDBInfoState() throws StopServiceException {
        return getServiceAttribute("sempra.utils:service=ICTSDBInfo", "StateString");
    }
    public String getIntegrityCheckServiceState() throws StopServiceException {
        return getServiceAttribute("sempra.utils:service=IntegrityCheckService", "StateString");
    }
    public String getDemurrageServiceState() throws StopServiceException {
        return getServiceAttribute("sempra.cwf:service=Demurrage", "StateString");
    }

    public String getOpsTrackingFinalApproveServiceState() throws StopServiceException {
        return getServiceAttribute("sempra.cwf:service=OpsTrackingFinalApprove", "StateString");
    }

    public String getOpsTrackingPriorityCalcServiceState() throws StopServiceException {
        return getServiceAttribute("sempra.cwf:service=OpsTrackingPriorityCalc", "StateString");
    }

    public String getMailNotifierState() throws StopServiceException {
        return getServiceAttribute("sempra.utils:service=MailNotifier", "StateString");
    }

    public String getVaultAlertServiceStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=VaultAlert", "StateString");
        String queueDepth = getServiceAttribute("jboss.mq.destination:name=VAULT_IMPORT,service=Queue", "QueueDepth");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }

    public String getVaultImportServiceStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=VaultImport", "StateString");
        String queueDepth =  getDBQueueDepth("jbossdbq.q_vault_alert");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }

    public String getEditValueDateNotifyServiceStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=EditValueDateNotify", "StateString");
        String queueDepth = getServiceAttribute("jboss.mq.destination:name=EDIT_VALUE_DATE_ALERT,service=Queue", "QueueDepth");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }

    public String getEditAutoEntryNotifyServiceStatus() throws StopServiceException {
        String state = getServiceAttribute("sempra.cwf:service=EditAutoEntryNotify", "StateString");
        String queueDepth = getServiceAttribute("jboss.mq.destination:name=EDIT_AUTO_ENTRY_ALERT,service=Queue", "QueueDepth");
        return "State=" + state + ", QueueDepth=" + queueDepth;
    }

    public String getOPS_TRACKING_ACTIVITY_ALERT() throws StopServiceException {
        return "AllSubscriptionsCount=" +
        getServiceAttribute("jboss.mq.destination:name=OPS_TRACKING_ACTIVITY_ALERT,service=Topic", "AllSubscriptionsCount");
    }

    public boolean getAutoRestartEnabled() throws StopServiceException{
        return autoRestartEnabled;
    };

    private String getDateFromDB(String pSQL, boolean pConvertToLocal) {
        String dbString = "";
        String localString = "Error";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = createPreparedStatement(pSQL);
            rs = statement.executeQuery();
            if (rs.next()) {
                //Get the date exactly as it is in the database.
                dbString = sdfDateTime.format(rs.getTimestamp("dateResult"));

                //Either convert from GMT to local or send it back as is.
                if (pConvertToLocal){
                    Date localDate = sdfDateTimeLocal.parse(dbString);
                    localString = sdfDateTime.format(localDate) + " Local";
                }
                else
                    localString = dbString + " Local";
            }
        } catch (NullPointerException npe) {
            return "Never";
        } catch (Exception e) {
            return e.toString();
        }  finally {
            if (rs != null) {
                try {
                    rs.close();
                } catch (SQLException e) {
                }
                rs = null;
            }
            if (statement != null) {
                try {
                    statement.close();
                } catch (SQLException e) {
                }
                statement = null;
            }
        }
        return localString;
    }

    private String getDBQueueDepth(String pTableName) {
        int recCount = -1;
        String queueDepth = "Error";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = createPreparedStatement("SELECT count(*) count from " + pTableName +
                    " where processed_flag = 'N'");
            rs = statement.executeQuery();
            if (rs.next()) {
                recCount = (rs.getInt("count"));
                queueDepth = Integer.toString(recCount);
            }
        } catch (SQLException e) {
            return e.toString();
        } catch (SQLConnectionAllocationFailure sqlConnectionAllocationFailure) {
            return sqlConnectionAllocationFailure.toString();
        } finally {
            if (rs != null) {
                try {
                    rs.close();
                } catch (SQLException e) {
                }
                rs = null;
            }
            if (statement != null) {
                try {
                    statement.close();
                } catch (SQLException e) {
                }
                statement = null;
            }
        }
        return queueDepth;
    }


    /**
     * JMX console method, fired on demand.
     */
    /*synchronized public void executeTimerEventNow() throws StopServiceException{
        Logger.getLogger(StatusMonitorService.class).info("executeTimerEventNow() executing...");
        poll();
        Logger.getLogger(StatusMonitorService.class).info("executeTimerEventNow() done.");
    }*/

    public void setAutoRestartEnabled() throws StopServiceException{
        if (autoRestartEnabled == true)
            autoRestartEnabled = false;
        else
            autoRestartEnabled = true;
    };

    public String resetMarginRespProcFlag() throws StopServiceException{
        try {
            callResetMarginRespProcFlag();
            getSqlConnection().commit();
        } catch (Exception e) {
            return "Update failed: " + e.getMessage();
        }
        return "Success.";
    };

    private void callResetMarginRespProcFlag()
            throws SQLException, SQLConnectionAllocationFailure, NamingException {
        String sql = "{call infinity_mgr.ResetMarginRespProcFlag() }";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = createPreparedStatement(sql);
            rs = statement.executeQuery();
        }  finally {
            if (rs != null) {
                try {
                    rs.close();
                } catch (SQLException e) {
                }
                rs = null;
            }
            if (statement != null) {
                try {
                    statement.close();
                } catch (SQLException e) {
                }
                statement = null;
            }
        }
    }


    /*public String startEConfirmStatusPolling() throws StopServiceException{
        return invokeService("sempra.cwf:service=EConfirmStatusPolling","start");
    }*/

    protected void runTask() throws StopServiceException, LogException {
        Date currentDate = new Date();
        if (autoRestartEnabled &&
            !sdfDayOfWeek.format(currentDate).equalsIgnoreCase("SAT") &&
            !sdfDayOfWeek.format(currentDate).equalsIgnoreCase("SUN"))
            poll();
        currentDate = null;
    }

    synchronized private void poll() throws StopServiceException{
        try {
            Logger.getLogger(StatusMonitorService.class).info("Executing Status Monitor task...");

            String state = "";
            state = getServiceAttribute("sempra.cwf:service=EConfirmStatusPolling", "StateString");
            if (state.equalsIgnoreCase("stopped")){
                invokeService("sempra.cwf:service=EConfirmStatusPolling","start");
                String subject = "EConfirmStatusPollingService has been restarted";
                String mailDesc =
                        "The EConfirmStatusPollingService has been restarted automatically." +
                        "\nTo disable AutoRestart go to the StatusMonitor" +
                        "\n  and press the setAutoRestartEnabled button.";
                mailUtils.sendMail(adjustedSendToAddress, sendToName, sentFromAddress, sentFromName,
                        subject, mailDesc, "");
                Logger.getLogger(StatusMonitorService.class).info("eConfirmStatusPollingService has been restarted");
            }
            state = "";
            state = getServiceAttribute("sempra.cwf:service=EConfirmTradeAlert2", "StateString");
            if (state.equalsIgnoreCase("stopped")){
                invokeService("sempra.cwf:service=EConfirmTradeAlert2","start");
                String subject = "EConfirmTradeAlert2 has been restarted";
                String mailDesc =
                        "The EConfirmTradeAlert2Service has been restarted automatically." +
                        "\nTo disable AutoRestart go to the StatusMonitor" +
                        "\n  and press the setAutoRestartEnabled button.";
                mailUtils.sendMail(adjustedSendToAddress, sendToName, sentFromAddress, sentFromName,
                        subject, mailDesc, "");
                Logger.getLogger(StatusMonitorService.class).info("EConfirmTradeAlert2 has been restarted");
            }
            Logger.getLogger(StatusMonitorService.class).info("Execute Status Monitor task done.");
        } catch (Exception e) {
            log.error("ERROR", e);
            throw new StopServiceException(e.getMessage());
        }
    }


////////////////////////////////////////////////////////////////////////////////////

    private void doSomething(ObjectName _mbeanConfig) throws Exception {
        //mBeanServer = getMBeanServer();

        //invoke a method on the other mbean
       /* mBeanServer.invoke(_mbeanConfig, "yourMethodName",
                new Object[] { "YourParameters" }, new String[] { "java.lang.String" } );*/

        //obtain attributes for an mbean
        String state = "";
        state = mBeanServer.getAttribute(_mbeanConfig, "StateString").toString();
        log.info("state=" + state);
        //mBeanServer.setAttribute(_mbeanConfig, new Attribute("SomeAttributeName", "NewAttributeValue"));
    }


}
