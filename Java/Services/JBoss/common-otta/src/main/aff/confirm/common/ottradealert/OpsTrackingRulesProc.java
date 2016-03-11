package aff.confirm.common.ottradealert;

import com.sun.rowset.CachedRowSetImpl;
import com.sun.rowset.FilteredRowSetImpl;

import javax.sql.RowSet;
import javax.sql.rowset.Predicate;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.ParseException;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import org.jboss.logging.Logger;


/**
 * User: ifrankel
 * Date: 1/7/13
 * Time: 10:37 AM
 */
public class OpsTrackingRulesProc {
    private static Logger log = Logger.getLogger(OpsTrackingRulesProc.class.getName());

    public enum Operator { EQ, NQ, LT, LE, GT, GE, NU, IN; }
    public enum AndOr { AND, OR; }
    public enum DataType { STR, INT, FLT, DAT; }
    private java.sql.Connection connection;
    private final String IGNORE_TRADE_RULE_SQL = "select * from OPS_TRACKING.v_ignore_rules";
    private final String IGNORE_TRADE_RULE_MAST_SQL = "select * from OPS_TRACKING.ignore_rule_mast";
    private final String RQMT_RULE_SQL = "select * from OPS_TRACKING.v_rqmt_rules";
    private final String RQMT_RULE_RESULTS_SQL = "select * from OPS_TRACKING.v_rqmt_rule_results";
    private final String RQMT_RULE_MAST_SQL = "select * from OPS_TRACKING.rqmt_rule_mast";
    private final String RQMT_BROKER_EXCLUDE_SQL = "select * from OPS_TRACKING.v_rqmt_broker_exclude";
    private final String RQMT_2ND_CHK_SQL = "select * from ifrankel.v_rqmt_2nd_chk";
    private final String RQMT_2ND_CHK_MAST_SQL = "select * from OPS_TRACKING.rqmt_2nd_chk_mast";
    private CachedRowSetImpl crsIgnoreTradeRules;
    private CachedRowSetImpl crsIgnoreTradeRuleMast;
    private CachedRowSetImpl crsRqmtRules;
    private CachedRowSetImpl crsRqmtRuleResults;
    private CachedRowSetImpl crsRqmtRuleMast;
    private CachedRowSetImpl crsRqmtBrokerExcludeRules;
    private FilteredRowSetImpl frsRqmt2ndChkRules;
    private CachedRowSetImpl crsRqmt2ndChkRulesMast;

    public OpsTrackingRulesProc(java.sql.Connection pOpsTrackingConnection) throws SQLException {
        this.connection = pOpsTrackingConnection;
//        crsIgnoreTradeRules = populateCrs(IGNORE_TRADE_RULE_SQL);
//        crsIgnoreTradeRuleMast = populateCrs(IGNORE_TRADE_RULE_MAST_SQL);
//        crsRqmtRules = populateCrs(RQMT_RULE_SQL);
//        crsRqmtRuleResults = populateCrs(RQMT_RULE_RESULTS_SQL);
//        crsRqmtRuleMast = populateCrs(RQMT_RULE_MAST_SQL);
//        crsRqmtBrokerExcludeRules = populateCrs(RQMT_BROKER_EXCLUDE_SQL);
//        frsRqmt2ndChkRules = populateFrs(RQMT_2ND_CHK_SQL);
//        crsRqmt2ndChkRulesMast = populateCrs(RQMT_2ND_CHK_MAST_SQL);
    }

    public int IsIgnoreTrade(TradeNotifyMessageRec pTradeNotifyMessageRec) throws SQLException, ParseException {
        int ignoreTradeRuleId = 0;
        int ruleId = 0;
        int colId = 0;
        int orGroup = 0;
        String msgVal = "";
        String operVal = "";
        String literalVal = "";
        String andOrVal = "";
        String dataType = "";
        String messageDisplay = "";
        Calendar msgCal = Calendar.getInstance();
        Calendar literalCal = Calendar.getInstance();
        boolean ignoreAndOk = true;
        boolean[] isOr = new boolean[10];
        boolean[] orOk = new boolean[10];
        for (int i = 1;i < 10; i++)
            isOr[i] = false;
        for (int i = 1;i < 10; i++)
            orOk[i] = false;
        boolean ignoreOrOk = true;

        crsIgnoreTradeRules.beforeFirst();
        try {
            while (crsIgnoreTradeRules.next()) {
                //Check to see if processing rule has changed. If so, check the just-completed one.
                if (ruleId != crsIgnoreTradeRules.getInt("RULE_ID") &&
                    ruleId != 0) {
                    ignoreOrOk = true;
                    for (int i = 1;i < 10; i++){
                        //Since all OR groups are ANDed together, look for any that failed and fail the rule.
                        if (isOr[i] && !orOk[i]){
                            ignoreOrOk = false;
                            break;
                        }
                    }

                    //By 'passed test', we mean 'evaluate without finding a hit'
                    if (ignoreOrOk && ignoreAndOk) {
                        ignoreTradeRuleId = ruleId;
                        //System.out.println(messageDisplay);
                        break;
                    }

                    ruleId = crsIgnoreTradeRules.getInt("RULE_ID");
                    for (int i = 1;i < 10; i++)
                        isOr[i] = false;
                    for (int i = 1;i < 10; i++)
                        orOk[i] = false;
                    ignoreAndOk = true;
                }
                else if (ruleId == 0)
                    ruleId = crsIgnoreTradeRules.getInt("RULE_ID");

                colId = crsIgnoreTradeRules.getInt("COL_ID");
                msgVal = pTradeNotifyMessageRec.getFieldValue(colId);
                operVal = crsIgnoreTradeRules.getString("OPER");
                literalVal = crsIgnoreTradeRules.getString("LITERAL");
                if (literalVal == null)
                    literalVal = "";
                andOrVal = crsIgnoreTradeRules.getString("AND_OR");
                orGroup = crsIgnoreTradeRules.getInt("OR_GROUP");
                dataType = crsIgnoreTradeRules.getString("DATA_TYPE");
                messageDisplay = "Ignore Notify Rule Triggered: " + String.valueOf(ruleId) + ":" + crsIgnoreTradeRules.getString("SHORT_NAME");
                if (DataType.valueOf(dataType) == DataType.DAT){
                    msgCal = pTradeNotifyMessageRec.getCalendarDate(colId);
                    Date tempDate = pTradeNotifyMessageRec.sdfDisplayDate.parse(literalVal);
                    literalCal.setTime(tempDate);
                }

                switch (AndOr.valueOf(andOrVal)) {
                    case AND:
                        //All ANDs must be true
                        if (!getOperatorRslt(operVal, dataType, msgVal, literalVal, msgCal, literalCal))
                            ignoreAndOk = false;
                        break;
                    case OR:
                        // 1. Keep track of ORs by group
                        // 2. Only one OR must be true per group.
                        isOr[orGroup] = true;
                        if (getOperatorRslt(operVal, dataType, msgVal, literalVal, msgCal, literalCal))
                            orOk[orGroup] = true;
                        break;
                }
        }
        } catch (Exception e) {
            log.error( "ERROR", e  );
        }

        //Process last rule on the list unless the result is already true.
        if (ignoreTradeRuleId == 0){
            ignoreOrOk = true;
            for (int i = 1;i < 10; i++){
                if (isOr[i] && !orOk[i]){
                    ignoreOrOk = false;
                }
            }

            if (ignoreOrOk && ignoreAndOk) {
                ignoreTradeRuleId = ruleId;
                //System.out.println(messageDisplay);
            }
        }

        return ignoreTradeRuleId;
    }

    public String getIgnoreRuleDescr(int pRuleId) throws Exception {
        String resultDescr = "NONE";
        if (pRuleId == 0)
            return resultDescr;
        try {
            crsIgnoreTradeRuleMast.beforeFirst();
            while (crsIgnoreTradeRuleMast.next()) {
                if (crsIgnoreTradeRuleMast.getInt("ID") == pRuleId) {
                    resultDescr = crsIgnoreTradeRuleMast.getString("SHORT_NAME") +
                            ": " + crsIgnoreTradeRuleMast.getString("DESCR");
                    break;
                }
            }

        } catch (Exception e) {
            log.error( "Occurred in getIgnoreRuleDescr for RuleId " + pRuleId, e );
        }

        return resultDescr;
    }

    public int GetRqmtRule(TradingSystemDATA_rec pTradingSystemDATA_rec) throws SQLException, ParseException {
        int rqmtRuleId = 0;
        int ruleId = 0;
        int colId = 0;
        int orGroup = 0;
        String tradeDataVal = "";
        String operVal = "";
        String literalVal = "";
        String andOrVal = "";
        String dataType = "";
        //String messageDisplay = "";
        Calendar tradeDataCal = Calendar.getInstance();
        Calendar literalCal = Calendar.getInstance();
        boolean rqmtAndOk = true;
        boolean[] isOr = new boolean[10];
        boolean[] orOk = new boolean[10];
        for (int i = 1;i < 10; i++)
            isOr[i] = false;
        for (int i = 1;i < 10; i++)
            orOk[i] = false;
        boolean rqmtOrOk = true;

        crsRqmtRules.beforeFirst();
        try {
            while (crsRqmtRules.next()) {
                //Check to see if processing rule has changed. If so, check the just-completed one.
                if (ruleId != crsRqmtRules.getInt("RULE_ID") &&
                        ruleId != 0) {
                    rqmtOrOk = true;
                    for (int i = 1;i < 10; i++){
                        //Since all OR groups are ANDed together, look for any that failed and fail the rule.
                        if (isOr[i] && !orOk[i]){
                            rqmtOrOk = false;
                            break;
                        }
                    }

                    if (rqmtOrOk && rqmtAndOk) {
                        rqmtRuleId = ruleId;
                        //System.out.println(messageDisplay);
                        break;
                    }

                    ruleId =  crsRqmtRules.getInt("RULE_ID");
                    for (int i = 1;i < 10; i++)
                        isOr[i] = false;
                    for (int i = 1;i < 10; i++)
                        orOk[i] = false;
                    rqmtAndOk = true;
                }
                else if (ruleId == 0)
                    ruleId = crsRqmtRules.getInt("RULE_ID");

                colId = crsRqmtRules.getInt("COL_ID");
                if (colId == 0)
                    break;

                tradeDataVal = pTradingSystemDATA_rec.getFieldValue(colId);
                operVal = crsRqmtRules.getString("OPER");
                literalVal = crsRqmtRules.getString("LITERAL");
                if (literalVal == null)
                    literalVal = "";
                andOrVal = crsRqmtRules.getString("AND_OR");
                orGroup = crsRqmtRules.getInt("OR_GROUP");
                dataType = crsRqmtRules.getString("DATA_TYPE");
                //messageDisplay = "Ignore Notify Rule Triggered: " + String.valueOf(rule_id) + ":" + pRqmtRuleset.getString("SHORT_NAME");
                if (DataType.valueOf(dataType) == DataType.DAT){
                    tradeDataCal = pTradingSystemDATA_rec.getCalendarDate(colId);
                    Date tempDate = pTradingSystemDATA_rec.sdfDisplayDate.parse(literalVal);
                    literalCal.setTime(tempDate);
                }

                switch (AndOr.valueOf(andOrVal)) {
                    case AND:
                        //All ANDs must be true
                        if (!getOperatorRslt(operVal, dataType, tradeDataVal, literalVal, tradeDataCal, literalCal))
                            rqmtAndOk = false;
                        break;
                    case OR:
                        // 1. Keep track of ORs by group
                        // 2. Only one OR must be true per group.
                        isOr[orGroup] = true;
                        if (getOperatorRslt(operVal, dataType, tradeDataVal, literalVal, tradeDataCal, literalCal))
                            orOk[orGroup] = true;
                        break;
                }
            }
        } catch (Exception e) {
            log.error( "ERROR", e);
        }

        //Process last rule on the list unless the result is already true.
        if (rqmtRuleId == 0){
            rqmtOrOk = true;
            for (int i = 1;i < 10; i++){
                if (isOr[i] && !orOk[i]){
                    rqmtOrOk = false;
                }
            }

            if (rqmtOrOk && rqmtAndOk) {
                rqmtRuleId = ruleId;
                //System.out.println(messageDisplay);
            }
        }

        return rqmtRuleId;
    }

    public ArrayList<String> GetRqmtRuleResults(int pRuleId) throws SQLException {
        ArrayList<String> rqmtList = new ArrayList<String>();

        int ruleTableRuleId = 0;
        String rqmtCode = "";

        crsRqmtRuleResults.beforeFirst();
        try {
            while (crsRqmtRuleResults.next()) {
                ruleTableRuleId = crsRqmtRuleResults.getInt("RULE_ID");
                rqmtCode = crsRqmtRuleResults.getString("RQMT_CODE");
                if (pRuleId == ruleTableRuleId)
                    rqmtList.add(rqmtCode);
            }
        } catch (Exception e) {
            log.error( "ERROR", e);
        }

        return rqmtList;
    }

    public ArrayList<String> GetRqmtRuleResultReference(int pRuleId) throws SQLException {
        ArrayList<String> rqmtSNList = new ArrayList<String>();

        int ruleTableRuleId = 0;
        String rqmtSN = "";

        crsRqmtRuleResults.beforeFirst();
        try {
            while (crsRqmtRuleResults.next()) {
                ruleTableRuleId = crsRqmtRuleResults.getInt("RULE_ID");
                rqmtSN = crsRqmtRuleResults.getString("RULE_SHORT_NAME") + " (Rule " + ruleTableRuleId + ")";
                if (pRuleId == ruleTableRuleId)
                    rqmtSNList.add(rqmtSN);
            }
        } catch (Exception e) {
            log.error( "ERROR", e);
        }

        return rqmtSNList;
    }

    public String GetRqmtRuleDescr(int pRuleId) throws Exception {
        String resultDescr = "NONE";
        if (pRuleId == 0)
            return resultDescr;
        try {
            crsRqmtRuleMast.beforeFirst();
            while (crsRqmtRuleMast.next()) {
                if (crsRqmtRuleMast.getInt("ID") == pRuleId) {
                    resultDescr = crsRqmtRuleMast.getString("SHORT_NAME") +
                            ": " + crsRqmtRuleMast.getString("DESCR");
                    break;
                }
            }

        } catch (Exception e) {
            log.error( "Occurred in GetRqmtRuleDescr for RuleId " + pRuleId, e );
        }

        return resultDescr;
    }

    public int GetRqmtBrokerExcludeRule(TradingSystemDATA_rec pTradingSystemDATA_rec)
            throws SQLException, ParseException {
        int rqmtBrokerExcludeRuleId = 0;

        String ruleBrokerSn = "";
        String ruleSeCptySn = "";
        String ruleCdtyGrpCode = "";
        String dataBrokerSn = "";
        String dataSeCptySn = "";
        String dataCdtyGrpCode = "";
        boolean isBrokerSnMatch = false;
        boolean isSeCptySnMatch = false;
        boolean isCdtyGrpCodeMatch = false;

        crsRqmtBrokerExcludeRules.beforeFirst();
        try {
            while (crsRqmtBrokerExcludeRules.next()) {
                ruleBrokerSn = getDataValue(crsRqmtBrokerExcludeRules.getString("BROKER_SN"));
                ruleSeCptySn = getDataValue(crsRqmtBrokerExcludeRules.getString("SE_CPTY_SN"));
                ruleCdtyGrpCode = getDataValue(crsRqmtBrokerExcludeRules.getString("CDTY_GRP_CODE"));
                dataBrokerSn = pTradingSystemDATA_rec.BROKER_SN;
                dataSeCptySn = pTradingSystemDATA_rec.SE_CPTY_SN;
                dataCdtyGrpCode = pTradingSystemDATA_rec.CDTY_GRP_CODE;

                isBrokerSnMatch = ruleBrokerSn.equalsIgnoreCase(dataBrokerSn) || ruleBrokerSn.isEmpty();
                isSeCptySnMatch = ruleSeCptySn.equalsIgnoreCase(dataSeCptySn) || ruleSeCptySn.isEmpty();
                isCdtyGrpCodeMatch = ruleCdtyGrpCode.equalsIgnoreCase(dataCdtyGrpCode) || ruleCdtyGrpCode.isEmpty();

                if (isBrokerSnMatch && isSeCptySnMatch && isCdtyGrpCodeMatch) {
                    rqmtBrokerExcludeRuleId = crsRqmtBrokerExcludeRules.getInt("RULE_ID");
                    break;
                }
            }
        } catch (Exception e) {
            log.error( "ERROR", e);
        }

        return rqmtBrokerExcludeRuleId;
    }

    public String GetRqmtBrokerExcludeRuleDescr(int pRuleId) throws Exception {
        String resultDescr = "NONE";
        if (pRuleId == 0)
            return resultDescr;
        try {
            crsRqmtBrokerExcludeRules.beforeFirst();
            while (crsRqmtBrokerExcludeRules.next()) {
                if (crsRqmtBrokerExcludeRules.getInt("RULE_ID") == pRuleId) {
                    resultDescr = crsRqmtBrokerExcludeRules.getString("RULE_SHORT_NAME") +
                            ": " + crsRqmtBrokerExcludeRules.getString("DESCR");
                    break;
                }
            }

        } catch (Exception e) {
            log.error( "Occurred in GetRqmtBrokerExcludeRuleDescr for RuleId " + pRuleId, e);
        }

        return resultDescr;
    }

    public int GetRqmt2ndChk(String pRqmtCode, TradingSystemDATA_rec pTradingSystemDATA_rec) throws SQLException, ParseException {
        final String RQMT_CODE = "RQMT_CODE";
        int rqmtRuleId = 0;
        int ruleId = 0;
        int colId = 0;
        int orGroup = 0;
        String tradeDataVal = "";
        String operVal = "";
        String literalVal = "";
        String andOrVal = "";
        String dataType = "";
        //String messageDisplay = "";
        Calendar tradeDataCal = Calendar.getInstance();
        Calendar literalCal = Calendar.getInstance();
        boolean rqmtAndOk = true;
        boolean[] isOr = new boolean[10];
        boolean[] orOk = new boolean[10];
        for (int i = 1;i < 10; i++)
            isOr[i] = false;
        for (int i = 1;i < 10; i++)
            orOk[i] = false;
        boolean rqmtOrOk = true;
        ColumnFilter filter = new ColumnFilter(RQMT_CODE, pRqmtCode);
        frsRqmt2ndChkRules.setFilter(filter);

        frsRqmt2ndChkRules.beforeFirst();
        try {
            while (frsRqmt2ndChkRules.next()) {
                //Check to see if processing rule has changed. If so, check the just-completed one.
                if (ruleId != frsRqmt2ndChkRules.getInt("RULE_ID") &&
                        ruleId != 0) {
                    rqmtOrOk = true;
                    for (int i = 1;i < 10; i++){
                        //Since all OR groups are ANDed together, look for any that failed and fail the rule.
                        if (isOr[i] && !orOk[i]){
                            rqmtOrOk = false;
                            break;
                        }
                    }

                    if (rqmtOrOk && rqmtAndOk) {
                        rqmtRuleId = ruleId;
                        //System.out.println(messageDisplay);
                        break;
                    }

                    ruleId =  frsRqmt2ndChkRules.getInt("RULE_ID");
                    for (int i = 1;i < 10; i++)
                        isOr[i] = false;
                    for (int i = 1;i < 10; i++)
                        orOk[i] = false;
                    rqmtAndOk = true;
                }
                else if (ruleId == 0)
                    ruleId = frsRqmt2ndChkRules.getInt("RULE_ID");

                colId = frsRqmt2ndChkRules.getInt("COL_ID");
                if (colId == 0)
                    break;

                tradeDataVal = pTradingSystemDATA_rec.getFieldValue(colId);
                operVal = frsRqmt2ndChkRules.getString("OPER");
                literalVal = frsRqmt2ndChkRules.getString("LITERAL");
                if (literalVal == null)
                    literalVal = "";
                andOrVal = frsRqmt2ndChkRules.getString("AND_OR");
                orGroup = frsRqmt2ndChkRules.getInt("OR_GROUP");
                dataType = frsRqmt2ndChkRules.getString("DATA_TYPE");
                //messageDisplay = "Ignore Notify Rule Triggered: " + String.valueOf(rule_id) + ":" + pRqmtRuleset.getString("SHORT_NAME");
                if (DataType.valueOf(dataType) == DataType.DAT){
                    tradeDataCal = pTradingSystemDATA_rec.getCalendarDate(colId);
                    Date tempDate = pTradingSystemDATA_rec.sdfDisplayDate.parse(literalVal);
                    literalCal.setTime(tempDate);
                }

                switch (AndOr.valueOf(andOrVal)) {
                    case AND:
                        //All ANDs must be true
                        if (!getOperatorRslt(operVal, dataType, tradeDataVal, literalVal, tradeDataCal, literalCal))
                            rqmtAndOk = false;
                        break;
                    case OR:
                        // 1. Keep track of ORs by group
                        // 2. Only one OR must be true per group.
                        isOr[orGroup] = true;
                        if (getOperatorRslt(operVal, dataType, tradeDataVal, literalVal, tradeDataCal, literalCal))
                            orOk[orGroup] = true;
                        break;
                }
            }
        } catch (Exception e) {
            log.error( "ERROR", e);
        }

        //Process last rule on the list unless the result is already true.
        if (rqmtRuleId == 0){
            rqmtOrOk = true;
            for (int i = 1;i < 10; i++){
                if (isOr[i] && !orOk[i]){
                    rqmtOrOk = false;
                }
            }

            if (rqmtOrOk && rqmtAndOk) {
                rqmtRuleId = ruleId;
                //System.out.println(messageDisplay);
            }
        }

        return rqmtRuleId;
    }

    public String getRqmt2ndChkDescr(int pRuleId) throws Exception {
        String resultDescr = "NONE";
        if (pRuleId == 0)
            return resultDescr;
        try {
            crsRqmt2ndChkRulesMast.beforeFirst();
            while (crsRqmt2ndChkRulesMast.next()) {
                if (crsRqmt2ndChkRulesMast.getInt("ID") == pRuleId) {
                    resultDescr = crsRqmt2ndChkRulesMast.getString("SHORT_NAME") +
                            ": " + crsRqmt2ndChkRulesMast.getString("DESCR");
                    break;
                }
            }

        } catch (Exception e) {
            log.error( "Occurred in getRqmt2ndChkDescr for RuleId " + pRuleId , e);
        }

        return resultDescr;
    }

    private boolean getOperatorRslt(String pOper, String pDataType, String pDataFeedVal, String pLiteralVal, Calendar pDataFeedCal, Calendar pLiteralCal){
        boolean result = false;
        int literalVal = 0;
        int msgVal = 0;
        String[] literalList = null;
        if (DataType.valueOf(pDataType) == DataType.INT){
            literalVal = Integer.parseInt(pLiteralVal);
            msgVal = Integer.parseInt(pDataFeedVal);
        }
        if (Operator.valueOf(pOper) == Operator.IN)
            literalList = pLiteralVal.split(",");

        switch (Operator.valueOf(pOper)){
            case EQ:
                switch (DataType.valueOf(pDataType)){
                    case STR:
                        if (pDataFeedVal.equalsIgnoreCase(pLiteralVal)) result = true;
                        break;
                    case INT:
                        if (msgVal == literalVal) result = true;
                        break;
                    case FLT:
                        if (msgVal == literalVal) result = true;
                        break;
                    case DAT:
                        if (pDataFeedCal.equals(pLiteralCal)) result = true;
                        break;
                }  break;
            case NQ:
                switch (DataType.valueOf(pDataType)){
                    case STR:
                        if (!pDataFeedVal.equalsIgnoreCase(pLiteralVal)) result = true;
                        break;
                    case INT:
                        if (msgVal != literalVal) result = true;
                        break;
                    case FLT:
                        if (msgVal != literalVal) result = true;
                        break;
                    case DAT:
                        if (!pDataFeedCal.equals(pLiteralCal)) result = true;
                        break;
                }   break;
            case LT:
                switch (DataType.valueOf(pDataType)){
                    case STR:
                        if (pDataFeedVal.compareToIgnoreCase(pLiteralVal) < 0) result = true;
                        break;
                    case INT:
                        if (msgVal < literalVal) result = true;
                        break;
                    case FLT:
                        if (msgVal < literalVal) result = true;
                        break;
                    case DAT:
                        if (pDataFeedCal.before(pLiteralCal)) result = true;
                        break;
                }  break;
            case LE:
                switch (DataType.valueOf(pDataType)){
                    case STR:
                        if ((pDataFeedVal.compareToIgnoreCase(pLiteralVal) < 0) ||
                            (pDataFeedVal.equalsIgnoreCase(pLiteralVal))) result = true;
                        break;
                    case INT:
                        if ((msgVal < literalVal) || (msgVal == literalVal)) result = true;
                        break;
                    case FLT:
                        if ((msgVal < literalVal) || (msgVal == literalVal)) result = true;
                        break;
                    case DAT:
                        if ((pDataFeedCal.before(pLiteralCal)) || (pDataFeedCal.equals(pLiteralCal))) result = true;
                        break;
                }  break;
            case GT:
                switch (DataType.valueOf(pDataType)){
                    case STR:
                        if (pDataFeedVal.compareToIgnoreCase(pLiteralVal) > 0) result = true;
                        break;
                    case INT:
                        if (msgVal > literalVal) result = true;
                        break;
                    case FLT:
                        if (msgVal > literalVal) result = true;
                        break;
                    case DAT:
                        if (pDataFeedCal.after(pLiteralCal)) result = true;
                        break;
                }  break;
            case GE:
                switch (DataType.valueOf(pDataType)){
                    case STR:
                        if ((pDataFeedVal.compareToIgnoreCase(pLiteralVal) > 0) ||
                                (pDataFeedVal.equalsIgnoreCase(pLiteralVal))) result = true;
                        break;
                    case INT:
                        if ((msgVal > literalVal) || (msgVal == literalVal)) result = true;
                        break;
                    case FLT:
                        if ((msgVal > literalVal) || (msgVal == literalVal)) result = true;
                        break;
                    case DAT:
                        if ((pDataFeedCal.after(pLiteralCal)) || (pDataFeedCal.equals(pLiteralCal))) result = true;
                        break;
                }  break;
            case NU:
                switch (DataType.valueOf(pDataType)){
                    case STR:
                        if (pDataFeedVal.length() == 0) result = true;
                    case INT:
                        //if (msgVal == null) result = true;
                        break;
                    case FLT:
                        //if (msgVal == null) result = true;
                        break;
                    case DAT:
                        //if (pMsgCal.) result = true;
                        break;
                }  break;
            case IN:
                switch (DataType.valueOf(pDataType)){
                    case STR:
                        for (int i=0;i < literalList.length; i++){
                            if (pDataFeedVal.equalsIgnoreCase(literalList[i])){
                                result = true;
                                break;
                            }
                        }
                        break;
                    case INT:
                        //if (msgVal == literalVal) result = true;
                        break;
                    case FLT:
                        //if (msgVal == literalVal) result = true;
                        break;
                    case DAT:
                        //if (pMsgCal.equals(pLiteralCal)) result = true;
                        break;
                }  break;
        }

        return result;
    }

    private CachedRowSetImpl populateCrs(String pSqlStatement) throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        CachedRowSetImpl crs = new CachedRowSetImpl();

        try {
            statement = connection.prepareStatement(pSqlStatement);
            rs = statement.executeQuery();
            crs.populate(rs);
        } catch (SQLException e) {
            if( e.getErrorCode() == 942 ) {// Table or view does not Exist
                log.warn("Table not found: " + pSqlStatement);
            } else {
                log.error("ERROR", e);
            }
        } catch (Exception e) {
            log.error( "ERROR", e);
        } finally {
            if (statement != null) {
                try {
                    statement.close();
                } catch (SQLException e) {
                    log.error( "ERROR", e);
                }
                statement = null;
            }
            if (rs != null) {
                try {
                    rs.close();
                } catch (SQLException e) {
                    log.error( "ERROR", e);
                }
                rs = null;
            }
        }
        return crs;
    }

    private FilteredRowSetImpl populateFrs(String pSqlStatement) throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        FilteredRowSetImpl frs = new FilteredRowSetImpl();

        try {
            statement = connection.prepareStatement(pSqlStatement);
            rs = statement.executeQuery();
            frs.populate(rs);
        } catch (SQLException e) {
            if( e.getErrorCode() == 942 ) {// Table or view does not Exist
                log.warn("Table not found: " + pSqlStatement);
            } else {
                log.error("ERROR", e);
            }
        } catch (Exception e) {
            log.error( "ERROR", e);
        } finally {
            if (statement != null) {
                try {
                    statement.close();
                } catch (SQLException e) {
                    log.error( "ERROR", e);
                }
                statement = null;
            }
            if (rs != null) {
                try {
                    rs.close();
                } catch (SQLException e) {
                    log.error( "ERROR", e);
                }
                rs = null;
            }
        }
        return frs;
    }

    private String getDataValue(String pDataVal) {
        String dataVal = "";
        if (pDataVal != null) {
            dataVal = pDataVal;
        }
        return dataVal;
    }


}



// Used by the FilteredCachedRowset
class ColumnFilter implements Predicate {

    private String colName;
    private String colValue;

    public ColumnFilter(String pColumnName, String pColumnValue){
        this.colName = pColumnName;
        this.colValue = pColumnValue;
    }

    public boolean evaluate(Object pValue, int pColIndex){
        return false;
    }

    public boolean evaluate(Object pValue, String pColName){
        return false;
    }

    public boolean evaluate(RowSet rs) {
        if (rs == null) {
            return false;
        }

        FilteredRowSetImpl frs = (FilteredRowSetImpl) rs;
        boolean evaluation = false;
        try {
            String columnValue = frs.getString(this.colName);
            if (columnValue.equalsIgnoreCase(colValue)) {
                evaluation = true;
            }
        } catch (SQLException e) {
            return false;
        }
        return evaluation;
    }
}
