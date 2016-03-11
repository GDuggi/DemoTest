package aff.confirm.common.ottradealert;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.util.Date;

/**
 * User: ifrankel
 * Date: Oct 10, 2005
 * Time: 5:33:43 AM
 */
public class OpsTrackingPriorityCalc {
    private DecimalFormat df = new DecimalFormat("#,##0.00;(#,##0.00)");
    private final String VALUE_NONE = "*NONE*";
    final long DAY_IN_MILLIS = 60*60*24*1000;
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    //private java.sql.Connection ictsConnection;
    private String cptySn;
    private String brokerSn;
    private boolean isEConfirm;
    private boolean isEFET;
    private java.sql.Date startDt;
    private java.sql.Date endDt;
    private java.sql.Date inceptionDt;


    public OpsTrackingPriorityCalc(java.sql.Connection pOpsTrackingConnection,
                                    java.sql.Connection pAffinityConnection )  {
                                    //java.sql.Connection pICTSConnection )  {
        this.opsTrackingConnection = pOpsTrackingConnection;
        this.affinityConnection = pAffinityConnection;
        //this.ictsConnection = pICTSConnection;
    }

    public OpsTrackingTRADE_PRIORITY_rec getTradePriority(String pTradeSysCode, double pTradeId)
            throws Exception {
        OpsTrackingTRADE_PRIORITY_rec otTradePriorityRec;
        otTradePriorityRec = new OpsTrackingTRADE_PRIORITY_rec();
        int tradePriority = 5;
        int brokerPriority = 5;
        int plPriority = 5;
        int durationPriority = 5;
        int agingPriority = 5;
        boolean foundIt = false;

        foundIt = loadTradeData(pTradeSysCode,pTradeId);
        if (foundIt){
            brokerPriority = getBrokerPriority();
            otTradePriorityRec = getPLPriority(pTradeSysCode, pTradeId);
            plPriority = otTradePriorityRec.PRIORITY;
            durationPriority = getDurationPriority();
            agingPriority = getAgingPriority();
            tradePriority = getPriority(brokerPriority, plPriority, durationPriority, agingPriority);
            otTradePriorityRec.PRIORITY = tradePriority;
        }
        return otTradePriorityRec;
    }

    private int getBrokerPriority(){
        int brokerPriority = 5;
//        if (cptySn.equalsIgnoreCase("FIMAT"))
//            brokerPriority = 4;
//        else if (brokerSn.equalsIgnoreCase("ICE")){
//            if (isEConfirm)
//                brokerPriority = 5;
//            else
//                brokerPriority = 4;}
//        else if (brokerSn.equalsIgnoreCase("NGX"))
//            brokerPriority = 5;
        if (isEConfirm || isEFET){
            if (!brokerSn.equalsIgnoreCase(VALUE_NONE))
                brokerPriority = 3;
            else
                brokerPriority = 2;}
        else if (!brokerSn.equalsIgnoreCase(VALUE_NONE))
            brokerPriority = 2;
        else
            brokerPriority = 1;

        return brokerPriority;

    }

    private OpsTrackingTRADE_PRIORITY_rec getPLPriority(String pTradeSysCode, double pTradeId) throws SQLException {
        OpsTrackingTRADE_PRIORITY_rec otPLPriorityRec;
        otPLPriorityRec = new OpsTrackingTRADE_PRIORITY_rec();
        int plPriority = 5;
        double plAmt = 0;

        if (pTradeSysCode.equalsIgnoreCase("AFF"))
            otPLPriorityRec.PL_AMT = getAffPLAmount(pTradeId);
        else if (pTradeSysCode.equalsIgnoreCase("SYM"))
            otPLPriorityRec.PL_AMT = getIctsPLAmount(pTradeId);

        if (otPLPriorityRec.PL_AMT.equalsIgnoreCase("n/a")){
            otPLPriorityRec.PRIORITY = 3;
            return otPLPriorityRec;
        }
        else
            plAmt = Double.parseDouble(otPLPriorityRec.PL_AMT);

        if (Math.abs(plAmt) < 100001)
            plPriority = 5;
        else if (Math.abs(plAmt) < 250001)
            plPriority = 4;
        else if (Math.abs(plAmt) < 500001)
            plPriority = 3;
        else if (Math.abs(plAmt) < 1000001)
            plPriority = 2;
        else
            plPriority = 1;

        otPLPriorityRec.PL_AMT = df.format(plAmt);
        otPLPriorityRec.PRIORITY = plPriority;
        return otPLPriorityRec;
    }

    private int getDurationPriority(){
        int durationPriority = 5;
        long durationDays = 0;

        if (endDt == null || startDt == null)
            return durationPriority;

        durationDays = (endDt.getTime() - startDt.getTime()) / DAY_IN_MILLIS;
        if (durationDays < 4)
            durationPriority = 5;
        else if (durationDays < 30)
            durationPriority = 4;
        else if (durationDays < 90)
            durationPriority = 3;
        else if (durationDays < 180)
            durationPriority = 2;
        else
            durationPriority = 1;

        return durationPriority;
    }

    private int getAgingPriority(){
        int agingPriority = 5;
        long agingDays = 0;
        Date today;
        today = new Date();

        if (inceptionDt == null)
            return agingPriority;

        agingDays = (today.getTime() - inceptionDt.getTime()) / DAY_IN_MILLIS;
        if (agingDays < 3)
            agingPriority = 5;
        else if (agingDays < 6)
            agingPriority = 4;
        else if (agingDays < 11)
            agingPriority = 3;
        else if (agingDays < 16)
            agingPriority = 2;
        else
            agingPriority = 1;

        return agingPriority;
    }

    private int getPriority(int pBrokerPriority, int pPLPriority, int pDurationPriority, int pAgingPriority){
        int tradePriority = 5;
        double tempResult = 0;
        tempResult = (double)((pBrokerPriority * 2) + (pPLPriority * 2) + pDurationPriority + pAgingPriority) / 6;

        if (tempResult % 1 == 0)
            tradePriority = (int)(tempResult);
        else
            //Always rounds up. Example: 3.01 becomes 13
            tradePriority = (int)(tempResult + 1);

        return tradePriority;
    }

    private void initTradeData(){
        cptySn = "";
        brokerSn = "";
        isEConfirm = false;
        isEFET = false;
        startDt = null;
        endDt = null;
        inceptionDt = null;
    }

    private boolean loadTradeData(String pTradeSysCode, double pTradeId)
            throws SQLException {
        initTradeData();
        PreparedStatement statement = null;
        ResultSet rs = null;
        boolean foundIt = false;
        String setCRqmt = "";
        try {
            String selectSQL;
            selectSQL = "select cpty_sn, broker_sn, setc_rqmt, " +
                    "start_dt, end_dt, inception_dt " +
                    "from ops_tracking.v_trade_summary " +
                    "where trd_sys_code = ? " +
                    "and trade_id = ?";

            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setString(1, pTradeSysCode);
            statement.setDouble(2, pTradeId);
            rs = statement.executeQuery();

            while (rs.next()) {
                foundIt = true;
                cptySn = rs.getString("cpty_sn");
                if (cptySn == null)
                    cptySn = VALUE_NONE;
                brokerSn = rs.getString("broker_sn");
                if (brokerSn == null)
                    brokerSn = VALUE_NONE;
                setCRqmt = rs.getString("setc_rqmt");
                if (setCRqmt != null){
                    if (setCRqmt.equalsIgnoreCase("ECONF"))
                        isEConfirm = true;
                    else if (setCRqmt.equalsIgnoreCase("EFET"))
                        isEFET = true;
                }
                startDt = rs.getDate("start_dt");
                endDt = rs.getDate("end_dt");
                inceptionDt = rs.getDate("inception_dt");
            }
            /*if (!foundIt)
                throw new StopServiceException("OpsTrackingPriorityCalc.loadTradeData failed: Row not found. SQL statement=" +
                        selectSQL + " for TradeID=" + pTradeId);*/

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;

            try {
                if (rs != null)
                    rs.close();
            } catch (SQLException e) { }
            rs = null;
        }
        return foundIt;
    }

    private String getAffPLAmount(double pTradeId)
            throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        double plAmt = 0;
        boolean foundIt = false;
        String plAmtResult = "n/a";
        try {
            String selectSQL;
            selectSQL = " SELECT  "   +
                " sum(itdpl_usd) itdpl_usd"   +
                " FROM realtime.rpt_gtmp"   +
                " WHERE pl_ind not in ('X','R','K')"   +
                " and realized_flag = 'N'"   +
                " and ticket = ?"  ;

            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();

            while (rs.next()) {
                foundIt = true;
                plAmt = rs.getDouble("itdpl_usd");
                }

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;

            try {
                if (rs != null)
                    rs.close();
            } catch (SQLException e) { }
            rs = null;
        }

        //return Math.abs(plAmt);
        if (foundIt)
            plAmtResult = Double.toString(plAmt);  //df.format(plAmt);

        return plAmtResult;
    }


    private String getIctsPLAmount(double pTradeId)
            throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        double plAmt = 0;
        boolean foundIt = false;
        String plAmtResult = "n/a";
        try {
            String selectSQL;
            selectSQL = " select " +
                " USD_PV_ITDPL from"   +
                " jms.V_POS_PL_DET_GTMP_JMS"   +
                " where ticket = ?"  ;

            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();

            while (rs.next()) {
                foundIt = true;
                plAmt = rs.getDouble("USD_PV_ITDPL");
                }

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;

            try {
                if (rs != null)
                    rs.close();
            } catch (SQLException e) { }
            rs = null;
        }

        //return Math.abs(plAmt);
        if (foundIt)
            plAmtResult = Double.toString(plAmt);  //df.format(plAmt);

        return plAmtResult;
    }

}
