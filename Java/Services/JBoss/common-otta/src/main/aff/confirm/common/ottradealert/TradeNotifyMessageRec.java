package aff.confirm.common.ottradealert;

import javax.jms.JMSException;
import javax.jms.Message;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.Locale;

/**
 * Created with IntelliJ IDEA.
 * User: ifrankel
 * Date: 1/7/13
 * Time: 8:48 AM
 */
public class TradeNotifyMessageRec {
    public final SimpleDateFormat sdfPrivateDate = new SimpleDateFormat("MM/dd/yyyy");
    public final SimpleDateFormat sdfPrivateDateTime = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss");
    public final SimpleDateFormat sdfDisplayDate = new SimpleDateFormat("dd-MMM-yyyy", Locale.US);
    public final SimpleDateFormat sdfDisplayDateTime = new SimpleDateFormat("dd-MMM-yyyy HH:mm:ss");


    public String tradingSystem;
    public double tradeId;
    public double version;
    public String auditTypeCode;
    public double tradeAuditId;
    public Date updateTimeStampGMT;
    public String updateUserId;
    public String empName;
    public Date updateBusnDt;
    public String strUpdateBusnDt;
    public String tradeStatCode;
    public String tradeTypeCode;
    public String tradeSttlTypeCode;
    public String cdtyCode;
    public Date tradeDt;
    public String strTradeDt;
    public String companyShortName;
    public String bookShortName;
    public String cptyShortName;
    public String brokerShortName;
    public String referenceShortName;
    public String notifyContractsFlag;
    //private  String[] ruleTableFields = new String[17];

    public TradeNotifyMessageRec(Message pMessage) throws JMSException, ParseException {
        init();
        tradingSystem = getMessageValue("TRADING_SYSTEM", pMessage);
        tradeId = pMessage.getDoubleProperty("PRMNT_TRADE_ID");
        if (pMessage.propertyExists("VERSION"))
            version = pMessage.getDoubleProperty("VERSION");
        auditTypeCode = getMessageValue("AUDIT_TYPE_CODE", pMessage);
        tradeAuditId = pMessage.getDoubleProperty("TRADE_AUDIT_ID");
        //updateTimeStampGMT =
        if (pMessage.propertyExists("EMP_ID"))
            updateUserId = pMessage.getStringProperty("EMP_ID");

        if (tradingSystem.equalsIgnoreCase("SYM"))
        {
            if (getMessageValue("EMP_NAME", pMessage).length() > 0)
                empName = getMessageValue("EMP_NAME", pMessage).toUpperCase();
            else
                empName  = "NONE";
        }
        else
            empName  = "NONE";

        strUpdateBusnDt = pMessage.getStringProperty("UPDATE_BUSN_DT").substring(0,10);
        updateBusnDt = sdfPrivateDate.parse(strUpdateBusnDt);
        tradeStatCode = getMessageValue("TRADE_STAT_CODE", pMessage);
        tradeTypeCode = getMessageValue("TRADE_TYPE_CODE", pMessage);
        tradeSttlTypeCode = getMessageValue("TRADE_STTL_TYPE_CODE", pMessage);
        cdtyCode = getMessageValue("CDTY_CODE", pMessage);
        strTradeDt = pMessage.getStringProperty("TRADE_DT").substring(0,10);
        tradeDt = sdfPrivateDate.parse(strTradeDt);
        companyShortName = getMessageValue("CMPNY_SHORT_NAME", pMessage);
        bookShortName = getMessageValue("BK_SHORT_NAME", pMessage);
        cptyShortName = getMessageValue("CPTY_SHORT_NAME", pMessage);
        brokerShortName = getMessageValue("BROKERSN", pMessage);
        referenceShortName = getMessageValue("RFRNCE_SHORT_NAME", pMessage);
        notifyContractsFlag = getMessageValue("NOTIFY_CONTRACTS_FLAG", pMessage);
    };

    public TradeNotifyMessageRec(){
        init();
    }

    public void init(){
        tradingSystem = "";
        tradeId = 0;
        version = 0;
        auditTypeCode = "";
        tradeAuditId = 0;
        updateTimeStampGMT = null;
        updateUserId = "";
        empName = "";
        updateBusnDt = null;
        strUpdateBusnDt = "";
        tradeStatCode = "";
        tradeTypeCode = "";
        tradeSttlTypeCode = "";
        cdtyCode = "";
        tradeDt = null;
        strTradeDt = "";
        companyShortName = "";
        bookShortName = "";
        cptyShortName = "";
        brokerShortName = "";
        referenceShortName = "";
        notifyContractsFlag = "";

        /*ruleTableFields[0] = "None";
        ruleTableFields[1] = "sourceSystemCode";
        ruleTableFields[2] = "version";
        ruleTableFields[3] = "auditTypeCode";
        ruleTableFields[4] = "updateUserId";
        ruleTableFields[5] = "updateBusnDt";
        ruleTableFields[6] = "tradeStatCode";
        ruleTableFields[7] = "tradeTypeCode";
        ruleTableFields[8] = "tradeSttlTypeCode";
        ruleTableFields[9] = "cdtyCode";
        ruleTableFields[10] = "tradeDt";
        ruleTableFields[11] = "companyShortName";
        ruleTableFields[12] = "bookShortName";
        ruleTableFields[13] = "cptyShortName";
        ruleTableFields[14] = "brokerShortName";
        ruleTableFields[15] = "referenceShortName";
        ruleTableFields[16] = "notifyContractsFlag";*/
    }

    public Calendar getCalendarDate(int pColId){
        Calendar calDt = Calendar.getInstance();
        if (pColId == 5)
            calDt.setTime(updateBusnDt);
        else if (pColId == 10)
            calDt.setTime(tradeDt);
        return calDt;
    }

    public String getFieldValue(int pColId){
        String fieldVal = "";

        switch (pColId) {
            case 0: fieldVal = "None";
                    break;
            case 1: fieldVal = tradingSystem;
                    break;
            case 2: fieldVal = Double.toString(version);
                    break;
            case 3: fieldVal = auditTypeCode;
                    break;
            case 4: fieldVal = updateUserId;
                    break;
            case 5: fieldVal = sdfDisplayDate.format(updateBusnDt);
                    break;
            case 6: fieldVal = tradeStatCode;
                    break;
            case 7: fieldVal = tradeTypeCode;
                    break;
            case 8: fieldVal = tradeSttlTypeCode;
                    break;
            case 9: fieldVal = cdtyCode;
                    break;
            case 10: fieldVal = sdfDisplayDate.format(tradeDt);
                    break;
            case 11: fieldVal = companyShortName;
                    break;
            case 12: fieldVal = bookShortName;
                    break;
            case 13: fieldVal = cptyShortName;
                    break;
            case 14: fieldVal = brokerShortName;
                    break;
            case 15: fieldVal = referenceShortName;
                    break;
            case 16: fieldVal = notifyContractsFlag;
                    break;
            default: fieldVal = "Invalid Column Number";
                    break;
        }

        return fieldVal;
    }

    public void loadTestData() throws ParseException {
        tradingSystem = "SYM";
        tradeId = 2574;
        version = 0;
        auditTypeCode = "EDIT";
        tradeAuditId = 7745;
        updateTimeStampGMT = sdfPrivateDateTime.parse("01/07/2013 11:15:22");
        updateUserId = "ISRAEL";
        empName = "";
        updateBusnDt = sdfPrivateDate.parse("01/07/2013");
        strUpdateBusnDt = "";
        tradeStatCode = "OPEN";
        tradeTypeCode = "FUT";
        tradeSttlTypeCode = "PHYS";
        cdtyCode = "OIL";
        tradeDt = sdfPrivateDate.parse("01/07/2013");
        strTradeDt = "";
        companyShortName = "AMPHORA US";
        bookShortName = "USB";
        cptyShortName = "";
        brokerShortName = "";
        referenceShortName = "";
        notifyContractsFlag = "Y";
    }


    private static String getMessageValue(String pFieldName, Message pMessage)
            throws JMSException {
        String msgValue = "";
        if (pMessage.propertyExists(pFieldName) &&
                pMessage.getStringProperty(pFieldName) != null) {
            msgValue = pMessage.getStringProperty(pFieldName);
        }
        return msgValue;
    }

}
