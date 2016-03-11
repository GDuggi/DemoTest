/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:30:52 AM
 * To change template for new class use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.econfirmtradealert;

import org.jboss.logging.Logger;
import aff.confirm.common.dbqueue.QEConfirmAlert;
import aff.confirm.common.dbqueue.QEConfirmAlertRec;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.queueservice.QueueService;
import aff.confirm.jboss.common.util.DbInfoWrapper;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.jms.Message;
import javax.jms.JMSException;
import javax.management.Notification;
import javax.management.NotificationListener;
import javax.management.ObjectName;
import javax.management.MalformedObjectNameException;
import javax.naming.NamingException;
import java.sql.SQLException;
import java.sql.DriverManager;
import java.util.Enumeration;


@Startup
@Singleton
public class EConfirmTradeAlertService extends QueueService
        implements EConfirmTradeAlertServiceMBean, NotificationListener {

    private String opsTrackingDBInfoName;
    private String affinityDBInfoName;
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    private QEConfirmAlert qEConfirmAlert;
    private QEConfirmAlertRec qEConfirmAlertRec;
    private String affinityDBInfoDisplayName;
    private String opsTrackingDBInfoDisplayName;

    public EConfirmTradeAlertService() {
        super("affinity.cwf:service=EConfirmTradeAlert");
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


    private void init() throws Exception {
        //Temporary for testing
        /*Properties env = new Properties( );
        env.put(Context.PROVIDER_URL, "ctprodjb:1099");
        env.put(Context.URL_PKG_PREFIXES, "org.jboss.naming:org.jnp.interfaces");
        env.put(Context.INITIAL_CONTEXT_FACTORY,"org.jnp.interfaces.NamingContextFactory");
        InitialContext ic = new InitialContext(env);*/
        //Temporary for testing

        Logger.getLogger(this.getClass()).info("Executing init... ");
        /*DBInfo dbinfo = getDbInfo();
        dbinfo.addNotificationListener(this,null,null);
        Logger.getLogger(this.getClass()).info("Connected to database... : " + dbinfo.getDBUrl());*/

        Logger.getLogger(this.getClass()).info("Connecting opsTrackingConnection to " + opsTrackingDBInfoName + "...");
        opsTrackingConnection = getOracleConnection(opsTrackingDBInfoName, opsTrackingConnection);
        Logger.getLogger(this.getClass()).info("Connected opsTrackingConnection to " + opsTrackingDBInfoName + ".");

        Logger.getLogger(this.getClass()).info("Connecting affinityConnection to " + affinityDBInfoName + "...");
        affinityConnection = getOracleConnection(affinityDBInfoName, affinityConnection);
        Logger.getLogger(this.getClass()).info("Connected affinityConnection to " + affinityDBInfoName + ".");

        setDbDisplayNames();
        Logger.getLogger(this.getClass()).info("opsTrackingDBInfoName = " + opsTrackingDBInfoDisplayName);
        Logger.getLogger(this.getClass()).info("affinityConnection = " + affinityDBInfoDisplayName);

        /*eConfirmDAO = new EConfirmDAO(opsTrackingConnection, affinityConnection);
        String ecUserId = "";
        String ecPassword = "";
        ecUserId = eConfirmDAO.getECUserId();
        ecPassword = eConfirmDAO.getECPassword();

        eConfirmProcessor = new EConfirmProcessor(affinityConnection, eConfirmURL);
        eConfirmProcessor.setEConfirmUserName(ecUserId);
        eConfirmProcessor.setEConfirmPassword(ecPassword);*/

        qEConfirmAlert = new QEConfirmAlert(opsTrackingConnection);
        qEConfirmAlertRec = new QEConfirmAlertRec();

        Logger.getLogger(this.getClass()).info("eConfirmTradeAlert Started.");
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
        qEConfirmAlert = null;
        qEConfirmAlertRec = null;
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
        String ticketID = null;
        try {
            //setup parameter record and insert queueTable record for econfirm_v1 submit
            qEConfirmAlertRec.init();
            qEConfirmAlertRec.tradingSystem = pMessage.getStringProperty("TRADING_SYSTEM");
            qEConfirmAlertRec.tradeID = pMessage.getDoubleProperty("PRMNT_TRADE_ID");
            qEConfirmAlertRec.ecProductID = pMessage.getIntProperty("EC_PRODUCT_ID");
            qEConfirmAlertRec.ecAction = pMessage.getStringProperty("EC_ACTION");
            // MThoresen 4-18-2007: Added for click and confirm
            qEConfirmAlertRec.clickAndConfirmFlag = pMessage.getStringProperty("EC_CLICK_CONFIRM");
            //Samy 6-2-2009 : Added for broker matching
            if (properyExists(pMessage,"EC_BKR_ACTION")){
                qEConfirmAlertRec.ecBkrAction  = pMessage.getStringProperty("EC_BKR_ACTION");
            }
            else {
                qEConfirmAlertRec.ecBkrAction = "NONE";
            }
            qEConfirmAlert.insertQEConfirmAlert(qEConfirmAlertRec);
            opsTrackingConnection.commit();

            ticketID = df.format(qEConfirmAlertRec.tradeID);
            Logger.getLogger(this.getClass()).info("Inserted Q_ECONFIRM_ALERT: " +
                    qEConfirmAlertRec.tradingSystem + " " +
                    ticketID + " " +
                    qEConfirmAlertRec.ecProductID + " " +
                    qEConfirmAlertRec.ecAction);
        } catch (Exception e) {
            try {
                opsTrackingConnection.rollback();
            } catch (SQLException e1) {
            }
            throw new StopServiceException(e + ", " + e.getMessage());
        }
    }

    private boolean properyExists(Message pMessage, String propName) throws JMSException {
        boolean isExists = false;
        Enumeration enumeration = pMessage.getPropertyNames();
        while (enumeration.hasMoreElements()){
            String pName = (String) enumeration.nextElement();
            if (propName.equals(pName)){
                isExists = true;
                break;
            }

        }
        return isExists;
        
    }

    private void setDbDisplayNames() throws NamingException {

        DbInfoWrapper dbinfo = new DbInfoWrapper(affinityDBInfoName);
        affinityDBInfoDisplayName = dbinfo.getDatabaseName();
        dbinfo = new DbInfoWrapper(opsTrackingDBInfoName);
        opsTrackingDBInfoDisplayName = dbinfo.getDatabaseName();
    }


}
