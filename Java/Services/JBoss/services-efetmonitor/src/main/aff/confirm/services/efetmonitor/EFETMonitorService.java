package aff.confirm.services.efetmonitor;

import aff.confirm.common.efet.dao.EFET_DAO;
import com.sun.rowset.CachedRowSetImpl;
import org.jboss.logging.Logger;
import aff.confirm.common.dbqueue.QEFETTradeAlert;
import aff.confirm.common.dbqueue.QEFETTradeAlertRec;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskService;
import aff.confirm.jboss.common.util.DbInfoWrapper;
//import aff.confirm.services.econfirmmonitor2.EConfirmMonitor2ServiceMBean;


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
import java.text.DecimalFormat;


@Startup
@Singleton
public class EFETMonitorService extends TaskService implements EFETMonitorServiceMBean {
    private DecimalFormat df = new DecimalFormat("#0");
    private EFET_DAO efetDAO;
    private QEFETTradeAlert qEFETTradeAlert;
    private QEFETTradeAlertRec qEFETTradeAlertRec;
    private String opsTrackingDBInfoName;
    private String affinityDBInfoName;
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    private String affinityDBInfoDisplayName;
    private String opsTrackingDBInfoDisplayName;

    public EFETMonitorService() {
        super("affinity.cwf:service=EFETMonitor");
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
        efetDAO = null;
        qEFETTradeAlert = null;
        qEFETTradeAlertRec = null;

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

    public void init() throws Exception {
        Logger.getLogger(this.getClass()).info("Executing init... ");
        //Logger.getLogger(EConfirmMonitor2Service.class).info("Connecting opsTrackingConnection to " + opsTrackingDBInfoName + "...");
        opsTrackingConnection = getOracleConnection(opsTrackingDBInfoName, opsTrackingConnection);
        Logger.getLogger(this.getClass()).info("Connected opsTrackingConnection to " + opsTrackingDBInfoName + ".");

        //Logger.getLogger(EConfirmMonitor2Service.class).info("Connecting affinityConnection to " + affinityDBInfoName + "...");
        affinityConnection = getOracleConnection(affinityDBInfoName, affinityConnection);
        Logger.getLogger(this.getClass()).info("Connected affinityConnection to " + affinityDBInfoName + ".");

        setDbDisplayNames();
        Logger.getLogger(this.getClass()).info("opsTrackingDBInfoName = " + opsTrackingDBInfoDisplayName);
        Logger.getLogger(this.getClass()).info("affinityConnection = " + affinityDBInfoDisplayName);

        String text = "";
        text = "Timer interval = " + (getTimerPeriod() / 1000) + " seconds.";
        Logger.getLogger(this.getClass()).info(text);

        efetDAO = new EFET_DAO(opsTrackingConnection,affinityConnection);
        qEFETTradeAlert = new QEFETTradeAlert(opsTrackingConnection);
        qEFETTradeAlertRec = new QEFETTradeAlertRec();
        Logger.getLogger(this.getClass()).info("Init OK.");
    }

    private void setDbDisplayNames() throws NamingException {

        DbInfoWrapper dbinfo = new DbInfoWrapper(affinityDBInfoName);
        affinityDBInfoDisplayName = dbinfo.getDatabaseName();
        dbinfo = new DbInfoWrapper(opsTrackingDBInfoName);
        opsTrackingDBInfoDisplayName = dbinfo.getDatabaseName();

    }

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
        } catch (Exception e) {
            Logger.getLogger(this.getClass()).error(e);
            try {
                opsTrackingConnection.rollback();
            } catch (Exception e1) {
                Logger.getLogger(this.getClass()).error(e1);
            }
            throw new StopServiceException(e.getMessage());
        }
    }

    private void resubmitTrades() throws SQLException, StopServiceException {
        String tradingSys = "";
        String documentId = "";
        String efetAction = "";
        String efetSubmitState = "";
        String okToResubmitInd = "";
        String entityType = "";
        String rqmtCode = "";
        boolean submitBrokerCnf = false;
        final String OK_TO_RESUBMIT_ID = "N";
        double tradeID = -1;
        CachedRowSet crs;
        crs = new CachedRowSetImpl();
        crs = efetDAO.getEfetSummaryResubmit();
        crs.beforeFirst();
        while (crs.next()) {
            tradingSys = "";
            tradeID = -1;
            documentId = "";
            efetSubmitState = "";
            efetAction = "";
            entityType = "";
            rqmtCode = "";

            //Get values from efet_trade_summary
            tradingSys = crs.getString("TRADING_SYSTEM");
            tradeID = crs.getDouble("TRADE_ID");
            //documentId = crs.getString("DOCUMENT_ID");
            okToResubmitInd = crs.getString("OK_TO_RESUBMIT_IND");
            entityType = crs.getString("ENTITY_TYPE");
            efetAction = getEfetAction(okToResubmitInd);
            efetSubmitState = getSubmitState(okToResubmitInd);

            //setup parameter record and insert queueTable record for econfirm_v1 submit
            qEFETTradeAlertRec.tradingSystem = tradingSys;
            qEFETTradeAlertRec.tradeID = tradeID;
            qEFETTradeAlertRec.efetAction = efetAction;
            qEFETTradeAlertRec.efetSubmitState = efetSubmitState;

            if (entityType.equalsIgnoreCase("C")){
                qEFETTradeAlertRec.docType = "CNF";
                qEFETTradeAlertRec.receiverType = "C";
            } else {
                //If submitting a BFI doc and there is no efet cpty then also submit the CNF.
                qEFETTradeAlertRec.receiverType = "B";
                if (!efetDAO.isEfetTradeSummaryExist(tradeID,"C")){
                    qEFETTradeAlertRec.docType = "CNF";
                    String prevAction =  qEFETTradeAlertRec.efetAction;
                    qEFETTradeAlert.insertQEfetAlert(qEFETTradeAlertRec);
                    qEFETTradeAlertRec.efetAction = prevAction;
                }
                qEFETTradeAlertRec.docType = "BFI";
            }

            qEFETTradeAlert.insertQEfetAlert(qEFETTradeAlertRec);

            String cmt = "";
            if (efetAction.equalsIgnoreCase("CANCEL"))
                cmt = "Cancel pending";
            else
                cmt = "Resubmitted";

            if (entityType.equalsIgnoreCase("C"))
                rqmtCode = "EFET";
            else if (entityType.equalsIgnoreCase("B"))
                rqmtCode = "EFBKR";
            else
                throw new StopServiceException("resubmitTrades: Unknown value for entityType=" + entityType +
                ", tradeId=" + df.format(tradeID) + ", efet_action=" + efetAction);

            //IF 4/28/06 -- Displays on gui that the trade is waiting to be submitted.
            updateTradeRqmt(tradeID,"QUEUE",cmt,rqmtCode);

            //Update ec_trade_summary that trade was resubmitted.
            efetDAO.setEfetTradeSummaryOKToResubmit(tradeID, entityType, OK_TO_RESUBMIT_ID);
            opsTrackingConnection.commit();

            Logger.getLogger(this.getClass()).info("Inserted Q_EFET_TRADE_ALERT, updated EFET_TRADE_SUMMARY for: " +
                    tradingSys + " " + df.format(tradeID) +
                    ", efet_action=" + efetAction);
        }
        crs.close();
        crs = null;
    }

    private void updateTradeRqmt(double pTradeId, String pStatus, String pComment, String pRqmtCode )
            throws SQLException {
        PreparedStatement preparedStatement = null;
        try {
            String insertSQL =
                    "Update ops_tracking.TRADE_RQMT " +
                    "set STATUS = ? " + //  1
                    ", CMT = ? " + //  2
                    "where TRADE_ID = ? " + //  3
                    "and RQMT = ? "; //  4

            preparedStatement = null;
            preparedStatement = opsTrackingConnection.prepareStatement(insertSQL);
            preparedStatement.setString(1, pStatus);
            preparedStatement.setString(2, pComment);
            preparedStatement.setDouble(3, pTradeId);
            preparedStatement.setString(4, pRqmtCode);
            preparedStatement.executeUpdate();
        } finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
                preparedStatement = null;
            } catch (SQLException e) {
            }
        }
    }

    private String getEfetAction(String pOKToResubmitInd) {
        String efetAction = "";
        if (pOKToResubmitInd.equalsIgnoreCase("S"))
            efetAction = "SUBMIT";
        else if (pOKToResubmitInd.equalsIgnoreCase("R"))
            efetAction = "RESUBMIT";
        else if (pOKToResubmitInd.equalsIgnoreCase("C"))
            efetAction = "CANCEL";
        else {
            try {
                throw new StopServiceException("Unknown value for EFET.EFET_TRADE_SUMMARY.OK_TO_RESUBMIT_IND=" +
                        pOKToResubmitInd);
            } catch (Exception e) {
                Logger.getLogger(this.getClass()).error(e);
            }
        }
        return efetAction;
    }

    private String getSubmitState(String pOKToResubmitInd) {
        String submitState = "";
        if (pOKToResubmitInd.equalsIgnoreCase("S"))
            submitState = "NEW";
        else if (pOKToResubmitInd.equalsIgnoreCase("R"))
            submitState = "EDIT";
        else if (pOKToResubmitInd.equalsIgnoreCase("C"))
            submitState = "NONE";
        else {
            try {
                throw new StopServiceException("Unknown value for EFET.EFET_TRADE_SUMMARY.OK_TO_RESUBMIT_IND=" +
                        pOKToResubmitInd);
            } catch (Exception e) {
                Logger.getLogger(this.getClass()).error(e);
            }
        }
        return submitState;
    }

}
