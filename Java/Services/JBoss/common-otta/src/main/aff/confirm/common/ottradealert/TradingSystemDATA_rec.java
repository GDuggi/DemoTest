package aff.confirm.common.ottradealert;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Locale;


/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * This data record stores data retrieved from the following views:
 * Oracle:
 *      infinity_mgr.v_ops_tracking_data
 *      infinity_mgr.v_ops_tracking_data_fx
 * Sybase:
 *      contractfeed.trade_alert_ops_track
 */
public class TradingSystemDATA_rec {
    public double TRADE_ID;
    public java.sql.Date INCEPTION_DT;
    public String CDTY_CODE;
    public java.sql.Date TRADE_DT;
    public String XREF;
    public String CPTY_SN;
    public double QTY_TOT;
    public double QTY;
    public String UOM_DUR_CODE;
    private String LOCATION_SN;
    public String PRICE_DESC;
    public java.sql.Date START_DT;
    public java.sql.Date END_DT;
    public String BOOK;
    public String TRADE_TYPE_CODE;
    public String STTL_TYPE;
    public String BROKER_SN;
    public String COMM;
    private String BUY_SELL_IND;
    public String REF_SN;
    public String PAY_PRICE;
    public String REC_PRICE;
    public String SE_CPTY_SN;
    public String TRADE_STAT_CODE;
    public String CDTY_GRP_CODE;
    public String tradingSystem;
    public String OPTN_STRIKE_PRICE;
    public String OPTN_PREM_PRICE;
    public String BROKER_PRICE;
    public String OPTN_PUT_CALL_IND;
    // 5/8/2007 Israel - support EFS trades
    public String EFS_FLAG;
    public String EFS_CPTY_SN;
    private final SimpleDateFormat sdfDate = new SimpleDateFormat("MM/dd/yyyy", Locale.US);
    public final SimpleDateFormat sdfDisplayDate = new SimpleDateFormat("dd-MMM-yyyy", Locale.US);
    public final SimpleDateFormat sdfDisplayDateTime = new SimpleDateFormat("dd-MMM-yyyy HH:mm:ss");

    public boolean TEST_BOOK_FLAG;

    public TradingSystemDATA_rec(){
        init();
    }

    public java.sql.Date getJavaSqlDateFromXmlDate(String pDtValue, String pDtFormat) throws ParseException{
        java.util.Date utilDate = new SimpleDateFormat(pDtFormat, Locale.US).parse(pDtValue);
        java.sql.Date sqlDate = new java.sql.Date(utilDate.getTime());
        return sqlDate;
    }


    public java.sql.Date getJavaSqlDate(java.util.Date pUtilDate){
        java.sql.Date sqlDate = new java.sql.Date(pUtilDate.getTime());
        return sqlDate;
    }

    public String getBUY_SELL_IND() {
        String buySellInd = BUY_SELL_IND;
        if (buySellInd == null)
            buySellInd = "";
        return buySellInd;
    }

    public String getLOCATION_SN() {
        String locationSN = LOCATION_SN;
        if (locationSN == null)
            locationSN = "";
        return locationSN;
    }

    public void setLOCATION_SN(String LOCATION_SN) {
        this.LOCATION_SN = LOCATION_SN;
    }

    public void setBUY_SELL_IND(String BUY_SELL_IND) {
        this.BUY_SELL_IND = BUY_SELL_IND;
    }

    public String INCEPTION_DT_AsString() {
        String dateValue = "";
        if (this.INCEPTION_DT != null)
            dateValue = sdfDate.format(this.INCEPTION_DT);
        return dateValue;
    }

    public String TRADE_DT_AsString() {
        String dateValue = null;
        if (this.TRADE_DT != null)
            dateValue = sdfDate.format(this.TRADE_DT);
        return dateValue;
    }

    public String START_DT_AsString() {
        String dateValue = null;
        if (this.START_DT != null)
            dateValue = sdfDate.format(this.START_DT);
        return dateValue;
    }

    public String END_DT_AsString() {
        String dateValue = null;
        if (this.END_DT != null)
            dateValue = sdfDate.format(this.END_DT);
        return dateValue;
    }

    public String getFieldValue(int pColId){
        String fieldVal = "";

        switch (pColId) {
            case 0: fieldVal = "None";
                break;
            case 1: fieldVal = tradingSystem;
                break;
            case 2: fieldVal = Double.toString(TRADE_ID);
                break;
            case 3: fieldVal = sdfDisplayDate.format(INCEPTION_DT);
                break;
            case 4: fieldVal = CDTY_CODE;
                break;
            case 5: fieldVal = sdfDisplayDate.format(TRADE_DT);
                break;
            case 6: fieldVal = XREF;
                break;
            case 7: fieldVal = CPTY_SN;
                break;
            case 8: fieldVal = Double.toString(QTY_TOT);
                break;
            case 9: fieldVal = Double.toString(QTY);
                break;
            case 10: fieldVal = UOM_DUR_CODE;
                break;
            case 11: fieldVal = LOCATION_SN;
                break;
            case 12: fieldVal = PRICE_DESC;
                break;
            case 13: fieldVal = sdfDisplayDate.format(START_DT);
                break;
            case 14: fieldVal = sdfDisplayDate.format(END_DT);
                break;
            case 15: fieldVal = BOOK;
                break;
            case 16: fieldVal = TRADE_TYPE_CODE;
                break;
            case 17: fieldVal = STTL_TYPE;
                break;
            case 18: fieldVal = BROKER_SN;
                break;
            case 19: fieldVal = COMM;
                break;
            case 20: fieldVal = BUY_SELL_IND;
                break;
            case 21: fieldVal = REF_SN;
                break;
            case 22: fieldVal = PAY_PRICE;
                break;
            case 23: fieldVal = REC_PRICE;
                break;
            case 24: fieldVal = SE_CPTY_SN;
                break;
            case 25: fieldVal = TRADE_STAT_CODE;
                break;
            case 26: fieldVal = CDTY_GRP_CODE;
                break;
            case 27: fieldVal = OPTN_STRIKE_PRICE;
                break;
            case 28: fieldVal = OPTN_PREM_PRICE;
                break;
            case 29: fieldVal = BROKER_PRICE;
                break;
            case 30: fieldVal = OPTN_PUT_CALL_IND;
                break;
            case 31: fieldVal = EFS_FLAG;
                break;
            case 32: fieldVal = EFS_CPTY_SN;
                break;
            default: fieldVal = "Invalid Column Number";
                break;
        }

        return fieldVal;
    }

    public Calendar getCalendarDate(int pColId){
        Calendar calDt = Calendar.getInstance();
        if (pColId == 3)
            calDt.setTime(INCEPTION_DT);
        else if (pColId == 5)
            calDt.setTime(TRADE_DT);
        else if (pColId == 13)
            calDt.setTime(START_DT);
        else if (pColId == 14)
            calDt.setTime(END_DT);
        return calDt;
    }



    public String ShowValues() {
        String values = "";

        values = "TRADE_ID: " + Double.toString(TRADE_ID) + "\n" +
                 "INCEPTION_DT: " + sdfDate.format(INCEPTION_DT) + "\n" +
                 "CDTY_CODE: " + CDTY_CODE + "\n" +
                 "TRADE_DT: " + sdfDate.format(TRADE_DT) + "\n" +
                 "XREF: " + XREF + "\n" +
                 "CPTY_SN: " + CPTY_SN + "\n" +
                 "QTY_TOT: " + Double.toString(QTY_TOT) + "\n" +
                 "QTY: " + Double.toString(QTY) + "\n" +
                 "UOM_DUR_CODE: " + UOM_DUR_CODE + "\n" +
                 "LOCATION_SN: " + LOCATION_SN + "\n" +
                 "PRICE_DESC: " + PRICE_DESC + "\n" +
                 "START_DT: " + sdfDate.format(START_DT) + "\n" +
                 "END_DT: " + sdfDate.format(END_DT) + "\n" +
                 "BOOK: " + BOOK + "\n" +
                 "TRADE_TYPE_CODE: " + TRADE_TYPE_CODE + "\n" +
                 "STTL_TYPE: " + STTL_TYPE + "\n" +
                 "BROKER_SN: " + BROKER_SN + "\n" +
                 "COMM: " + COMM + "\n" +
                 "BUY_SELL_IND: " + BUY_SELL_IND + "\n" +
                 "REF_SN: " + REF_SN + "\n" +
                 "PAY_PRICE: " + PAY_PRICE + "\n" +
                 "REC_PRICE: " + REC_PRICE + "\n" +
                 "SE_CPTY_SN: " + SE_CPTY_SN + "\n" +
                 "TRADE_STAT_CODE: " + TRADE_STAT_CODE + "\n" +
                 "CDTY_GRP_CODE: " + CDTY_GRP_CODE + "\n" +
                 "sourceSystemCode: " + tradingSystem + "\n" +
                 "OPTN_STRIKE_PRICE: " + OPTN_STRIKE_PRICE + "\n" +
                 "OPTN_PREM_PRICE: " + OPTN_PREM_PRICE + "\n" +
                 "BROKER_PRICE: " + BROKER_PRICE + "\n" +
                 "OPTN_PUT_CALL_IND: " + OPTN_PUT_CALL_IND + "\n" +
                 "EFS_FLAG: " + EFS_FLAG + "\n" +
                 "EFS_CPTY_SN: " + EFS_CPTY_SN;

        return values;
    }

    private void init(){
        this.TRADE_ID = 0;
        this.INCEPTION_DT = null;
        this.CDTY_CODE = "";
        this.TRADE_DT = null;
        this.XREF = "";
        this.CPTY_SN = "";
        this.QTY_TOT = 0;
        this.QTY = 0;
        this.UOM_DUR_CODE = "";
        this.LOCATION_SN = "";
        this.PRICE_DESC = "";
        this.START_DT = null;
        this.END_DT = null;
        this.BOOK = "";
        this.TRADE_TYPE_CODE = "";
        this.STTL_TYPE = "";
        this.BROKER_SN = "";
        this.COMM = "";
        this.BUY_SELL_IND = "";
        this.REF_SN = "";
        this.PAY_PRICE = "";
        this.REC_PRICE = "";
        this.SE_CPTY_SN = "";
        this.TRADE_STAT_CODE = "";
        this.CDTY_GRP_CODE = "";
        this.tradingSystem = "";
        this.OPTN_STRIKE_PRICE = "";
        this.OPTN_PREM_PRICE = "";
        this.BROKER_PRICE = "";
        this.OPTN_PUT_CALL_IND = "";
        // 5/8/2007 Israel - support EFS trades
        this.EFS_FLAG = "";
        this.EFS_CPTY_SN = "";
        this.TEST_BOOK_FLAG = false;
    }

}
