package aff.confirm.common.dbqueue;

/**
 * User: ifrankel
 * Date: Nov 25, 2003
 * Time: 7:02:49 AM
 * To change this template use Options | File Templates.
 */
public class QOpsTrackingTradeAlertRec {
    public double id;
    public String tradingSystem;
    public double tradeID;
    public double version;
    public String auditTypeCode;
    public String tradeTypeCode;
    public String cdtyCode;
    public double empId;
    public String notifyContractsFlag;
    public String processedFlag;

    public QOpsTrackingTradeAlertRec() {
        this.init();
    }

    public void init(){
        this.id = 0;
        this.tradingSystem = "";
        this.tradeID = 0;
        this.version = 0;
        this.auditTypeCode = "";
        this.tradeTypeCode = "";
        this.cdtyCode = "";
        this.empId = 0;
        this.notifyContractsFlag = "";
        this.processedFlag = "";
    }
}
