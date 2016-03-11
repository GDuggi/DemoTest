/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:30:52 AM
 * To change template for new class use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.efettradealert;

import aff.confirm.common.ottradealert.ProcessControlDAO;
import org.jboss.logging.Logger;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.queueservice.QueueService;
import aff.confirm.jboss.common.util.DbInfoWrapper;
import aff.confirm.common.dbqueue.QEFETTradeAlert;
import aff.confirm.common.dbqueue.QEFETTradeAlertRec;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.jms.Message;
import javax.management.MalformedObjectNameException;
import javax.management.Notification;
import javax.management.NotificationListener;
import javax.management.ObjectName;
import javax.naming.NamingException;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.text.SimpleDateFormat;


@Startup
@Singleton
public class EFETTradeAlertService extends QueueService
        implements EFETTradeAlertServiceMBean, NotificationListener {

    private final String EFET_SUBMISSION_CODE = "EFETSUB";
    private SimpleDateFormat sdfMessageTradeDt = new SimpleDateFormat("MM/dd/yyyy");
    private String opsTrackingDBInfoName;
    private String affinityDBInfoName;
    //private String symphonyDBInfoName;
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    //private java.sql.Connection symphonyConnection;
    private String opsTrackingDBInfoDisplayName;
    private String affinityDBInfoDisplayName;
    //private String symphonyDBInfoDisplayName;
    private QEFETTradeAlert qEFETTradeAlert;
    private QEFETTradeAlertRec qEFETTradeAlertRec;
    private ProcessControlDAO processControlDAO;

    public EFETTradeAlertService() {
        super("affinity.cwf:service=EFETTradeAlert");
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

/*    public void setSymphonyDBInfoName(String pSymphonyDBInfoName) {
        this.symphonyDBInfoName = pSymphonyDBInfoName;
    }

    public ObjectName getSymphonyDBInfo() throws MalformedObjectNameException {
        if (symphonyDBInfoName.length() > 0)
            return new ObjectName("sempra.utils:service=" + symphonyDBInfoName);
        else
            return null;
    }*/

    private void init() throws Exception {
        //Temporary for testing
        /*Properties env = new Properties( );
        env.put(Context.PROVIDER_URL, "ctprodjb:1099");
        env.put(Context.URL_PKG_PREFIXES, "org.jboss.naming:org.jnp.interfaces");
        env.put(Context.INITIAL_CONTEXT_FACTORY,"org.jnp.interfaces.NamingContextFactory");
        InitialContext ic = new InitialContext(env);*/
        //Temporary for testing

        Logger.getLogger(this.getClass()).info("Executing init... ");

        Logger.getLogger(this.getClass()).info("Connecting opsTrackingConnection to " + opsTrackingDBInfoName + "...");
        opsTrackingConnection = getOracleConnection(opsTrackingDBInfoName, opsTrackingConnection);
        Logger.getLogger(this.getClass()).info("Connected opsTrackingConnection to " + opsTrackingDBInfoName + ".");

        Logger.getLogger(this.getClass()).info("Connecting affinityConnection to " + affinityDBInfoName + "...");
        affinityConnection = getOracleConnection(affinityDBInfoName, affinityConnection);
        Logger.getLogger(this.getClass()).info("Connected affinityConnection to " + affinityDBInfoName + ".");

        /*
        Logger.getLogger(this.getClass()).info("Connecting symphonyConnection to " + symphonyDBInfoName + "...");
        symphonyConnection = getMSSqlConnection();
        Logger.getLogger(this.getClass()).info("Connected symphonyConnection to " + symphonyDBInfoName + ".");
        */
        setDbDisplayNames();
        Logger.getLogger(this.getClass()).info("opsTrackingDBInfoName = " + opsTrackingDBInfoDisplayName);
        Logger.getLogger(this.getClass()).info("affinityConnection = " + affinityDBInfoDisplayName);
//        Logger.getLogger(this.getClass()).info("symphonyConnection = " + symphonyDBInfoDisplayName);

        qEFETTradeAlert = new QEFETTradeAlert(opsTrackingConnection);
        qEFETTradeAlertRec = new QEFETTradeAlertRec();
        processControlDAO = new ProcessControlDAO(opsTrackingConnection);

        Logger.getLogger(this.getClass()).info("EFETTradeAlert Started.");
    }

    protected void onServiceStarting() throws Exception {
        Logger.getLogger(this.getClass()).info("Executing startService... ");
        //getSqlConnection().setAutoCommit(false);
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


    public void handleNotification(Notification notification, Object o) {
        if (notification.getType().equals("ORACLE_STARTUP")) {
            log.info("STARTUP notification");
            try {
                start();
            } catch (Exception e) {
                Logger.getLogger(this.getClass()).error("resume(): " + e);
            }
        } else if (notification.getType().equals("ORACLE_SHUTDOWN")) {
            log.info("SHUTDOWN notification");
            try {
                stop();
            } catch (Exception e) {
                Logger.getLogger(this.getClass()).error("pause(): " + e);
            }
        }
    }

    protected void onMessage(Message pMessage) throws StopServiceException, LogException {
        super.onMessage(pMessage);
        String ticketID = "";
        String tradeDt = "";
        try {
            //setup parameter record and insert queueTable record for econfirm_v1 submit
            qEFETTradeAlertRec.init();
            qEFETTradeAlertRec.tradingSystem = pMessage.getStringProperty("TRADING_SYSTEM");
            qEFETTradeAlertRec.tradeID = pMessage.getDoubleProperty("PRMNT_TRADE_ID");
            //truncates from 03/28/2005 00:00:00 to 03/28/2005
            qEFETTradeAlertRec.seCptySn = pMessage.getStringProperty("CMPNY_SHORT_NAME");
            qEFETTradeAlertRec.efetAction = pMessage.getStringProperty("EFET_ACTION");
            qEFETTradeAlertRec.efetSubmitState = pMessage.getStringProperty("EFET_SUBMIT_STATE");
            qEFETTradeAlertRec.docType = pMessage.getStringProperty("DOC_TYPE");
            qEFETTradeAlertRec.receiverType = pMessage.getStringProperty("RECEIVER_TYPE");
            qEFETTradeAlert.insertQEfetAlert(qEFETTradeAlertRec);

            //Israel 2/9/2015 -- Added because it's not being inserted from anywhere else.
            //processControlDAO.insertProcessControl(EFET_SUBMISSION_CODE);

            //Israel 7/28/2015 -- Updating to prevent row proliferation.
            if (processControlDAO.isProcessMastRowExist(EFET_SUBMISSION_CODE))
                processControlDAO.updateAlertRecordAll(EFET_SUBMISSION_CODE, "N");
            else
                processControlDAO.insertProcessControl(EFET_SUBMISSION_CODE);

            opsTrackingConnection.commit();

            ticketID = df.format(qEFETTradeAlertRec.tradeID);
            Logger.getLogger(this.getClass()).info("Inserted Q_EFET_TRADE_ALERT: " +
                    qEFETTradeAlertRec.tradingSystem + " " +
                    ticketID + " " +
                    qEFETTradeAlertRec.efetAction + " " +
                    qEFETTradeAlertRec.docType + " " +
                    qEFETTradeAlertRec.receiverType);
        } catch (Exception e) {
            try {
                opsTrackingConnection.rollback();
            } catch (SQLException e1) {
            }
            throw new StopServiceException(e + ", " + e.getMessage());
        }
    }

    private void setDbDisplayNames() throws NamingException {

        DbInfoWrapper dbinfo = new DbInfoWrapper(affinityDBInfoName);
        affinityDBInfoDisplayName = dbinfo.getDatabaseName();
        dbinfo = new DbInfoWrapper(opsTrackingDBInfoName);
        opsTrackingDBInfoDisplayName = dbinfo.getDatabaseName();
    }



}
