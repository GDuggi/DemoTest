package aff.confirm.common.dbqueue;

import java.sql.Date;

/**
 * User: ifrankel
 * Date: Nov 25, 2003
 * Time: 7:02:49 AM
 * To change this template use Options | File Templates.
 */
public class QAffinityTradeAlertRec {
    public double Id;
    public double tradeID;
    public double version;
    public Date updateDateTime;
    public double empId;
    public String updateTableName;
    public String auditTypeCode;
    public String lastEditedByGrpMemberFlag;
    public Date updateBusnDt;
    public String processedFlag;
    public Date processedTsGMT;

    public QAffinityTradeAlertRec() {
        this.init();
    }

    public void init(){
        this.tradeID = 0;
        this.version = 0;
        this.updateDateTime = null;
        this.empId = 0;
        this.updateTableName = "";
        this.auditTypeCode = "";
        this.lastEditedByGrpMemberFlag = "";
        this.updateBusnDt = null;
        this.processedFlag = "";
        this.processedTsGMT = null;
    }
}
