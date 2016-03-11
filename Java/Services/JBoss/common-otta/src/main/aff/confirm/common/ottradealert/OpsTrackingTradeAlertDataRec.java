package aff.confirm.common.ottradealert;

import javax.jms.JMSException;
import javax.jms.Message;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTradeAlertDataRec {
    public final SimpleDateFormat sdfDateTime = new SimpleDateFormat("MM/dd/yyyy");
    public String hasNoConfFlag;
    public String hasOtherRqmtsFlag;
    public String finalApprovalFlag;
    public String openRqtsFlag;
    public String opsDetActionsFlag;
    public int rqmtRuleID;
    public int rqmtBrokerExcludeRuleID;
    public String tradingSystem;
    public double tradeID;
    public double version;
    public double tradeAuditId;
    public String auditTypeCode;
    public String tradeTypeCode;
    public String empName;
    public int tradeNotifyID;
    public String category;
    private String buySellInd;
    private String locationSN;
    public Date tradeDt;
    public String strTradeDt;
    public String cptySn;
    public Date updateBusnDt;
    public String strUpdateBusnDt;
    public String cdtyCode;
    public String[] rqmts;
    //public java.sql.Date startDt;
    //public java.sql.Date endDt;
    // MThoresen - 4-18-2007:  Added for click and confirm
    public boolean isClickAndConfirm = false;

    public OpsTrackingTradeAlertDataRec(Message pMessage) throws JMSException, ParseException {
        init();
        tradingSystem = pMessage.getStringProperty("TRADING_SYSTEM");
        tradeID = pMessage.getDoubleProperty("PRMNT_TRADE_ID");
        if (pMessage.propertyExists("VERSION"))
            version = pMessage.getDoubleProperty("VERSION");
        auditTypeCode = pMessage.getStringProperty("AUDIT_TYPE_CODE");
        tradeTypeCode = pMessage.getStringProperty("TRADE_TYPE_CODE");
        strTradeDt = pMessage.getStringProperty("TRADE_DT").substring(0,10);
        tradeDt = sdfDateTime.parse(strTradeDt);
        strUpdateBusnDt = pMessage.getStringProperty("UPDATE_BUSN_DT").substring(0,10);
        updateBusnDt = sdfDateTime.parse(strUpdateBusnDt);
        cptySn = pMessage.getStringProperty("CPTY_SHORT_NAME");
        cdtyCode = pMessage.getStringProperty("CDTY_CODE");
    };

    public OpsTrackingTradeAlertDataRec(String pTradingSystem, double pTradeID, double pVersion,
                                        String pAuditTypeCode, String pTradeTypeCode) {
        init();
        tradingSystem = pTradingSystem;
        tradeID = pTradeID;
        version = pVersion;
        auditTypeCode = pAuditTypeCode;
        tradeTypeCode = pTradeTypeCode;
    };

    public void init(){
        hasNoConfFlag = "";
        hasOtherRqmtsFlag = "";
        finalApprovalFlag = "";
        openRqtsFlag = "";
        opsDetActionsFlag = "";
        rqmtRuleID = 0;
        rqmtBrokerExcludeRuleID = 0;
        tradingSystem = "";
        tradeID = 0;
        version = 0;
        tradeAuditId = 0;
        auditTypeCode = "";
        tradeTypeCode = "";
        empName = "";
        tradeNotifyID = 0;
        category = "";
        buySellInd = "";
        locationSN = "";
        rqmts = null;
        tradeDt = null;
        cptySn = "";
        cdtyCode = "";
        //startDt = null;
        //endDt = null;
        isClickAndConfirm = false;
    }
}
