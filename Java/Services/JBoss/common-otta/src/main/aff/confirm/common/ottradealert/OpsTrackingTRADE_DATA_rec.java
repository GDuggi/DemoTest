package aff.confirm.common.ottradealert;



/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTRADE_DATA_rec {
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
    public String BROKER_PRICE;
    public String OPTN_STRIKE_PRICE;
    public String OPTN_PREM_PRICE;
    public String OPTN_PUT_CALL_IND;
    // 5/8/2007 Israel to support EFS trades
    public String EFS_FLAG;
    public String EFS_CPTY_SN;

    public boolean TEST_BOOK_FLAG;

    public OpsTrackingTRADE_DATA_rec(){
        init();
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
        this.BROKER_PRICE = "";
        this.OPTN_PREM_PRICE = "";
        this.OPTN_PUT_CALL_IND = "";
        this.OPTN_STRIKE_PRICE = "";
        // 5/8/2007 Israel to support EFS trades
        this.EFS_FLAG = "";
        this.EFS_CPTY_SN = "";
        this.TEST_BOOK_FLAG = false;
    }

}
