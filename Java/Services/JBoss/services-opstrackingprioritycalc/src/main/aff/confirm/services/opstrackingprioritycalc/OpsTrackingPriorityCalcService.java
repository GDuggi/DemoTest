package aff.confirm.services.opstrackingprioritycalc;

import org.jboss.logging.Logger;
//import aff.confirm.jms.econfirm_v1.EConfirmDAO;
//import aff.confirm.jms.efet.EFETDAO;
import aff.confirm.common.ottradealert.OpsTrackingPriorityCalc;
import aff.confirm.common.ottradealert.ProcessControlDAO;
import aff.confirm.common.ottradealert.OpsTrackingTRADE_PRIORITY_dao;
import aff.confirm.common.ottradealert.OpsTrackingTRADE_PRIORITY_rec;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskService;
import aff.confirm.jboss.common.util.DbInfoWrapper;
//import aff.confirm.services.econfirmmonitor2.EConfirmMonitor2ServiceMBean;
//import aff.confirm.services.efetmonitor.EFETMonitorServiceMBean;
//import aff.confirm.services.efetmonitor.EFETMonitorService;


import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;
import javax.naming.NamingException;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.text.DecimalFormat;

public class OpsTrackingPriorityCalcService extends TaskService implements OpsTrackingPriorityCalcServiceMBean {

    private DecimalFormat df = new DecimalFormat("#0");
    private String opsTrackingDBInfoName;
    private String affinityDBInfoName;
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    private String affinityDBInfoDisplayName;
    private String opsTrackingDBInfoDisplayName;
    private OpsTrackingPriorityCalc opsTrackingPriorityCalc;
    private ProcessControlDAO processControlDAO;
    private OpsTrackingTRADE_PRIORITY_dao otTRADE_PRIORITY_dao;

    public OpsTrackingPriorityCalcService() {
        super("affinity.cwf:service=OpsTrackingPriorityCalc");
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
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("Executing startService... ");
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
        opsTrackingPriorityCalc = null;
        processControlDAO = null;
        otTRADE_PRIORITY_dao = null;

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
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info(pConnectionName+"="+dbinfo.getDatabaseName());
        result.setAutoCommit(false);
        return result;
    }

    public void init() throws Exception {
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("Executing init... ");
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("Connecting opsTrackingConnection to " + opsTrackingDBInfoName + "...");
        opsTrackingConnection = getOracleConnection(opsTrackingDBInfoName, opsTrackingConnection);
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("Connected opsTrackingConnection to " + opsTrackingDBInfoName + ".");

        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("Connecting affinityConnection to " + affinityDBInfoName + "...");
        affinityConnection = getOracleConnection(affinityDBInfoName, affinityConnection);
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("Connected affinityConnection to " + affinityDBInfoName + ".");

        setDbDisplayNames();
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("opsTrackingDBInfoName = " + opsTrackingDBInfoDisplayName);
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("affinityConnection = " + affinityDBInfoDisplayName);

        String text = "";
        text = "Timer interval = " + (getTimerPeriod() / 1000) + " seconds.";
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info(text);

        opsTrackingPriorityCalc = new OpsTrackingPriorityCalc(opsTrackingConnection,affinityConnection);
        processControlDAO = new ProcessControlDAO(opsTrackingConnection);
        otTRADE_PRIORITY_dao = new OpsTrackingTRADE_PRIORITY_dao(opsTrackingConnection);

        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("Init OK.");
    }

    private void setDbDisplayNames() throws NamingException {
        DbInfoWrapper dbinfo = new DbInfoWrapper(affinityDBInfoName);
        affinityDBInfoDisplayName = dbinfo.getDatabaseName();
        dbinfo = new DbInfoWrapper(opsTrackingDBInfoName);
        opsTrackingDBInfoDisplayName = dbinfo.getDatabaseName();
    }

    public void executeTimerEventNow() throws StopServiceException{
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("executeTimerEventNow() executing...");
        //poll();
        try {
            priorityCalc();
            processControlDAO.updateAlertRecord("PRICALC","Y");
            opsTrackingConnection.commit();
        } catch (Exception e) {
            Logger.getLogger(OpsTrackingPriorityCalcService.class).error(e);
            try {
                opsTrackingConnection.rollback();
            } catch (Exception e1) {
                Logger.getLogger(OpsTrackingPriorityCalcService.class).error(e1);
            }
            throw new StopServiceException(e.getMessage());
        }
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("executeTimerEventNow() done.");
    }

    protected void runTask() throws StopServiceException, LogException {
        poll();
    }

    private void poll() throws StopServiceException{
        /**
         * 1. Check flag to see if ready to process.
         * 2. If ready, call CalcPriority
         * 3. After CalcPriority executes, reset flag.
         */
        boolean isReadyToProcess = false;
        try {
//            Logger.getLogger(OpsTrackingPriorityCalcService.class).info("Executing Poll event...");
            isReadyToProcess = processControlDAO.isReadyToProcess("PRICALC");
            if (isReadyToProcess){
                priorityCalc();
                processControlDAO.updateAlertRecord("PRICALC","Y");
                opsTrackingConnection.commit();
            }
//            Logger.getLogger(OpsTrackingPriorityCalcService.class).info("Execute Poll event done.");
        } catch (Exception e) {
            Logger.getLogger(OpsTrackingPriorityCalcService.class).error(e);
            try {
                opsTrackingConnection.rollback();
            } catch (Exception e1) {
                Logger.getLogger(OpsTrackingPriorityCalcService.class).error(e1);
            }
            throw new StopServiceException(e.getMessage());
        }
    }

    synchronized private void priorityCalc() throws StopServiceException, Exception {
        /**
         * 1. get list of trades to process.
         * 2. get priority for each trade.
         * 3. Update the priority table for each trade.
         */
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("Executing priorityCalc task...");
        OpsTrackingTRADE_PRIORITY_rec otTRADE_PRIORITY_rec;
        otTRADE_PRIORITY_rec = new OpsTrackingTRADE_PRIORITY_rec();
        PreparedStatement statement = null;
        ResultSet rs = null;
        String tradeSysCode = "";
        double tradeID = 0;
        //int tradePriority = 5;
        int counter = 0;
        try {
            String selectSQL;
            /*selectSQL = "select trade_id, cpty_sn, broker_sn, setc_rqmt, " +
                    "start_dt, end_dt, inception_dt " +*/
            selectSQL = "select trade_id, trd_sys_code " +
                    "from ops_tracking.v_trade_summary " +
                    //"where trd_sys_code = 'AFF' " +
                    "where FINAL_APPROVAL_FLAG = 'N'";

            statement = opsTrackingConnection.prepareStatement(selectSQL);
            rs = statement.executeQuery();
            while (rs.next()) {
                counter++;
                otTRADE_PRIORITY_rec.init();
                tradeSysCode = rs.getString("trd_sys_code");
                tradeID = rs.getDouble("trade_id");
                //tradePriority = 5;
                otTRADE_PRIORITY_rec = opsTrackingPriorityCalc.getTradePriority(tradeSysCode,tradeID);
                if (otTRADE_PRIORITY_dao.isTradePriorityExist(tradeID))
                    otTRADE_PRIORITY_dao.updateTradePriority(tradeID, otTRADE_PRIORITY_rec.PRIORITY,
                                                                      otTRADE_PRIORITY_rec.PL_AMT);
                else
                    otTRADE_PRIORITY_dao.insertTradePriority(tradeID, otTRADE_PRIORITY_rec.PRIORITY,
                                                                      otTRADE_PRIORITY_rec.PL_AMT);
                if (counter % 1000 == 0)
                    opsTrackingConnection.commit();

                /*} catch (NullPointerException e) {
                    String tradeIdStr = df.format(tradeID);
                    Logger.getLogger(OpsTrackingPriorityCalcService.class).info("TradeId="+tradeIdStr);
                 }*/
            }
            opsTrackingConnection.commit();
        }
        finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;

            try {
                if (rs != null)
                    rs.close();
            } catch (SQLException e) {
            }
            rs = null;
        }
        Logger.getLogger(OpsTrackingPriorityCalcService.class).info("Executing priorityCalc task is done.");
    }


}
