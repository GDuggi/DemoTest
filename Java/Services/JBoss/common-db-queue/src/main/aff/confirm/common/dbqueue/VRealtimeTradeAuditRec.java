package aff.confirm.common.dbqueue;

import java.sql.Date;

/**
 * User: ifrankel
 * Date: Nov 25, 2003
 * Time: 7:02:49 AM
 * To change this template use Options | File Templates.
 */
public class VRealtimeTradeAuditRec {
    public double tradeAuditId;
    public double prmntTradeID;
    public double version;
    public Date updateDateTime;
    public double empId;
    public String updateTableName;
    public String auditTypeCode;
    public Date updateBusnDt;
    public String tradeTypeCode;
    public String tradeStatCode;
    public Date tradeDt;
    public String cmpnyShortName;
    public String bkShortName;
    public String cptyShortName;
    public String cdtyCode;
    public String brokerSn;
    public String tradingSystem;
    public String instCode;
    public String notifyContractsFlag;
    public String rfrnceShortName;
    public String tradeSttlTypeCode;

    public VRealtimeTradeAuditRec() {
        this.init();
    }

    public void init(){
        this.tradeAuditId = 0;
        this.prmntTradeID = 0;
        this.version = 0;
        this.updateDateTime = null;
        this.empId = 0;
        this.updateTableName = "";
        this.auditTypeCode = "";
        this.updateBusnDt = null;
        this.tradeTypeCode = "";
        this.tradeStatCode = "";
        this.tradeDt = null;
        this.cmpnyShortName = "";
        this.bkShortName = "";
        this.cptyShortName = "";
        this.cdtyCode = "";
        this.brokerSn = "";
        this.tradingSystem = "";
        this.instCode = "";
        this.notifyContractsFlag = "";
        this.rfrnceShortName = "";
        this.tradeSttlTypeCode = "";
    }
}
