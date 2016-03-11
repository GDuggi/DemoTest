package aff.confirm.common.efet.datarec;

/**
 * User: ifrankel
 * Date: Jul 28, 2003
 * Time: 1:26:42 PM
 * To change this template use Options | File Templates.
 */
public class EFETTrade_DataRec {
    public double prmntTradeId;
    public String companySN;
    public int mstrCmpnyId;
    public String cptySN;
    public int mstrCptyId;
    public String brokerSN;
    public int mstrBrokerId;
    public String tradeTypeCode;
    public String cdtyCode;
    public String tradeSttlTypeCode;
    public java.sql.Date tradeStartDt;
    public java.sql.Date tradeEndDt;
    public String stdProductFlag;
    public String payCurve;
    public String payPriceModel;
    public String payMkt;
    public String payUom;
    public String payCcy;
    public String payExchRollInd;
    public String recCurve;
    public String recPriceModel;
    public String recMkt;
    public String recUom;
    public String recCcy;
    public String recExchRollInd;
    public String qtyUomCode;
    public String sttlCcyCode;
    public String sttlPerInd;
    public String valDtModel;
    public int valDtMonths;
    public int valDtDays;
    public java.sql.Date tradeDt;
    public String locationSN;
    public String qtyPerDurationCode;
    public String serviceCode;
    public String strikePriceModel;
    public String rfrnce;
    public String optionStyleCode;

    public EFETTrade_DataRec() {
        this.init();
    }

    public void init(){
    this.prmntTradeId = 0;
    this.companySN = "";
    this.mstrCmpnyId = 0;
    this.cptySN = "";
    this.mstrCptyId = 0;
    this.tradeTypeCode = "";
    this.cdtyCode = "";
    this.tradeSttlTypeCode = "";
    this.tradeStartDt = null;
    this.tradeEndDt = null;
    this.stdProductFlag = "";
    this.payCurve = "";
    this.payPriceModel = "";
    this.payMkt = "";
    this.payUom = "";
    this.payCcy = "";
    this.payExchRollInd = "";
    this.recCurve = "";
    this.recPriceModel = "";
    this.recMkt = "";
    this.recUom = "";
    this.recCcy = "";
    this.recExchRollInd = "";
    this.qtyUomCode = "";
    this.sttlCcyCode = "";
    this.sttlPerInd = "";
    this.valDtModel = "";
    this.valDtMonths = 0;
    this.valDtDays = 0;
    this.tradeDt = null;
    this.locationSN = "";
    this.serviceCode = "";
    this.strikePriceModel = "";
    this.rfrnce = "";
    this.brokerSN = "";
    this.mstrBrokerId = 0;
    this.optionStyleCode = "";    
    }

}
