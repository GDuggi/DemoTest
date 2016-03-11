package aff.confirm.services.econfirmmonitor2;

import org.jboss.logging.Logger;
import aff.confirm.common.dbqueue.QEConfirmAlert;
import aff.confirm.common.dbqueue.QEConfirmAlertRec;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskService;
import aff.confirm.jboss.common.util.DbInfoWrapper;
import aff.confirm.common.econfirm.EConfirmData;
//import sun.jdbc.rowset.CachedRowSet;
import com.sun.rowset.CachedRowSetImpl;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.management.*;
import javax.sql.rowset.CachedRowSet;

import javax.naming.NamingException;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.text.DecimalFormat;


@Startup
@Singleton
public class EConfirmMonitor2Service extends TaskService implements EConfirmMonitor2ServiceMBean {

    private MBeanServer mBeanServer;
    private DecimalFormat df = new DecimalFormat("#0");
    //private EConfirmDAO eConfirmDAO;
    private EConfirmData eConfirmData;
    private QEConfirmAlert qEConfirmAlert;
    private QEConfirmAlertRec qEConfirmAlertRec;
    private String opsTrackingDBInfoName;
    private String affinityDBInfoName;
    //private String ictsDBInfoName;
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    //private java.sql.Connection ictsConnection;
    private String affinityDBInfoDisplayName;
    private String opsTrackingDBInfoDisplayName;
    //private String ICTSDBInfoDisplayName;


    public EConfirmMonitor2Service() {
        super( "affinity.cwf:service=EConfirmMonitor2");
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
    /*
    public void setICTSDBInfoName(String pICTSDBInfoName) {
        this.ictsDBInfoName = pICTSDBInfoName;
    }
      */
/*
    public ObjectName getICTSDBInfo() throws MalformedObjectNameException {
        //Samy: 07/06/2011 commented the following to skip the Sybase connection
        return null;
        */
/*
        if (ictsDBInfoName.length() > 0)
            return new ObjectName("sempra.utils:service=" + ictsDBInfoName);
        else
            return null;
            *//*

    }
*/

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

    public void close() throws Exception {
        //eConfirmDAO = null;
        eConfirmData = null;
        qEConfirmAlert = null;
        qEConfirmAlertRec = null;

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
        //*** JBoss EAP 6 Code ***
        java.sql.Connection result = null;
        DbInfoWrapper dbInfo = new DbInfoWrapper(pConnectionName);
        result = DriverManager.getConnection(dbInfo.getDBUrl(), dbInfo.getDBUserName(), dbInfo.getDBPassword());
        Logger.getLogger(this.getClass()).info("Connection: " + pConnectionName + "=" + dbInfo.getDatabaseName());
        result.setAutoCommit(false);
        return result;
    }

    public void init() throws Exception {
        Logger.getLogger(this.getClass()).info("Executing init... ");
        //Logger.getLogger(this.getClass()).info("Connecting opsTrackingConnection to " + opsTrackingDBInfoName + "...");
        opsTrackingConnection = getOracleConnection(opsTrackingDBInfoName, opsTrackingConnection);
        Logger.getLogger(this.getClass()).info("Connected opsTrackingConnection to " + opsTrackingDBInfoName + ".");

        //Logger.getLogger(this.getClass()).info("Connecting affinityConnection to " + affinityDBInfoName + "...");
        affinityConnection = getOracleConnection(affinityDBInfoName, affinityConnection);
        Logger.getLogger(this.getClass()).info("Connected affinityConnection to " + affinityDBInfoName + ".");

        /*
        Logger.getLogger(this.getClass()).info("Connecting ictsConnection to " + ictsDBInfoName + "...");
        ictsConnection = getIctsConnection();
        Logger.getLogger(this.getClass()).info("Connected ictsConnection to " + ictsDBInfoName + ".");
          */
        setDbDisplayNames();
        Logger.getLogger(this.getClass()).info("opsTrackingDBInfoName = " + opsTrackingDBInfoDisplayName);
        Logger.getLogger(this.getClass()).info("affinityDBInfoName = " + affinityDBInfoDisplayName);

        String text = "";
        text = "Timer interval = " + (getTimerPeriod() / 1000) + " seconds.";
        Logger.getLogger(this.getClass()).info(text);

        //eConfirmDAO = new EConfirmDAO(opsTrackingConnection,affinityConnection,ictsConnection);
        eConfirmData = new EConfirmData(opsTrackingConnection);
        qEConfirmAlert = new QEConfirmAlert(opsTrackingConnection);
        qEConfirmAlertRec = new QEConfirmAlertRec();
        Logger.getLogger(this.getClass()).info("Init OK.");
    }

    private void setDbDisplayNames() throws NamingException {
        DbInfoWrapper dbinfo = new DbInfoWrapper(affinityDBInfoName);
        affinityDBInfoDisplayName = dbinfo.getDatabaseName();
        dbinfo = new DbInfoWrapper(opsTrackingDBInfoName);
        opsTrackingDBInfoDisplayName = dbinfo.getDatabaseName();
    }

    /**
     * JMX console method, fired on demand.
     */
    synchronized public void executeTimerEventNow() throws StopServiceException{
        Logger.getLogger(this.getClass()).info("executeTimerEventNow() executing...");
        poll();
        Logger.getLogger(this.getClass()).info("executeTimerEventNow() done.");
    }

    protected void runTask() throws StopServiceException, LogException {
        poll();
    }


    synchronized private void poll() throws StopServiceException{
        try {
//            Logger.getLogger(this.getClass()).info("Executing monitor task...");
            resubmitTrades();
            //checkForAllegedTrades();
            //opsTrackingConnection.commit();
//            Logger.getLogger(this.getClass()).info("Execute monitor task done.");
        } catch (SQLException e) {
            Logger.getLogger(this.getClass()).error(e);
            try {
                opsTrackingConnection.rollback();

            } catch (Exception e1) {
                Logger.getLogger(this.getClass()).error(e1);
            }


        }
        catch (Exception e) {
            Logger.getLogger(this.getClass()).error(e);
            try {
                opsTrackingConnection.rollback();
            } catch (Exception e1) {
                Logger.getLogger(this.getClass()).error(e1);
            }
            /*
            String responseAction =   amphorainc.errorreporting.client.ErrorReportClient.getErrorAction("", CNFErrorConstant.APP_CODE,
                    this.getClass().getName(),CNFErrorConstant.ECONF_MONITOR_ERROR,e);

            if ( responseAction.equalsIgnoreCase(CNFErrorConstant.ACTION_FATAL) || responseAction.equalsIgnoreCase(CNFErrorConstant.ACTION_ERROR) ) {
                throw new StopServiceException(e.getMessage());
            }
            */
        }
    }

    private void resubmitTrades() throws SQLException {
        String tradingSys = "";
        String eConfirmAction = "";
        String okToResubmitInd = "";
        // MThoresen 4-18-2007: Added for click and confirm changes
        String clickAndConfirmFlag;
        final String OK_TO_RESUBMIT_ID = "N";
        int productID = -1;
        double tradeID = -1;
        CachedRowSet crs;
        crs = new CachedRowSetImpl();
//        crs = eConfirmDAO.getECSummaryResubmit();
        crs = eConfirmData.getECSummaryResubmit();
        crs.beforeFirst();
        while (crs.next()) {
            tradingSys = "";
            tradeID = -1;
            productID = -1;

            //Get values from ec_trade_summary
            tradingSys = crs.getString("TRADING_SYSTEM");
            tradeID = crs.getDouble("TRADE_ID");
            productID = crs.getInt("PRODUCT_ID");
            okToResubmitInd = crs.getString("OK_TO_RESUBMIT_IND");
            // MThoresen 4-18-2007: Added for click and confirm
            clickAndConfirmFlag = "N";
            eConfirmAction = getEConfirmAction(okToResubmitInd);

            //setup parameter record and insert queueTable record for econfirm_v1 submit
            qEConfirmAlertRec.tradingSystem = tradingSys;
            qEConfirmAlertRec.tradeID = tradeID;
            qEConfirmAlertRec.ecProductID = productID;
            qEConfirmAlertRec.ecAction = eConfirmAction;
            qEConfirmAlertRec.clickAndConfirmFlag = clickAndConfirmFlag;
            qEConfirmAlertRec.ecBkrAction = eConfirmAction;
            qEConfirmAlert.insertQEConfirmAlert(qEConfirmAlertRec);

            //Update ec_trade_summary that trade was resubmitted.
//            eConfirmDAO.setECTradeSummaryOKToResubmit(tradeID, OK_TO_RESUBMIT_ID);
            eConfirmData.setECTradeSummaryOKToResubmit(tradingSys, tradeID, OK_TO_RESUBMIT_ID);
            opsTrackingConnection.commit();

            Logger.getLogger(this.getClass()).info("Inserted Q_ECONFIRM_ALERT, updated EC_SUMMARY for: " +
                    tradingSys + " " + df.format(tradeID) +
                    ", ProductID=" + productID + ", ec_action=" + eConfirmAction);
        }
        crs.close();
        crs = null;
    }


    private String getEConfirmAction(String pOKToResubmitInd) {
        String eConfirmAction = "";
        if (pOKToResubmitInd.equalsIgnoreCase("R"))
            eConfirmAction = "SUBMIT";
        else if (pOKToResubmitInd.equalsIgnoreCase("C"))
            eConfirmAction = "CANCEL";
        else {
            try {
                throw new StopServiceException("Unknown value for ECONFIRM.EC_TRADE_SUMMARY.OK_TO_RESUBMIT_IND=" +
                        pOKToResubmitInd);
            } catch (Exception e) {
                Logger.getLogger(this.getClass()).error(e);
            }
        }
        return eConfirmAction;
    }

}
