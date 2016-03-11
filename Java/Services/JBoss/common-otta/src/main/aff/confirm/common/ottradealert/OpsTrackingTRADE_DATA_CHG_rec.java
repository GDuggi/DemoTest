package aff.confirm.common.ottradealert;

public class OpsTrackingTRADE_DATA_CHG_rec {
    public double Id;
    public double TradeId;
    public String ColName;
    public String OldValue;
    public String NewValue;
    public java.sql.Date UpdBusnDt;
    public java.sql.Date CrtdTsGmt;
    public String UserName;
    public String AuditTypeCode;
    public String ODBIncludeFlag;
    public int ODBCxlCorrExcludeId;

    public OpsTrackingTRADE_DATA_CHG_rec(){
        init();
    }

    public void init(){
        this.Id = 0;
        this.TradeId = 0;
        this.ColName = "";
        this.OldValue = "";
        this.NewValue = "";
        this.UpdBusnDt = null;
        this.CrtdTsGmt = null;
        this.UserName = "";
        this.AuditTypeCode = "";
        this.ODBIncludeFlag = "";
        this.ODBCxlCorrExcludeId = 0;
    }

    public void setFields(double pTradeId, String pColName, String pOldValue, String pNewValue, java.sql.Date pUpdBusnDt,
                            String pUserName, String pAuditTypeCode, String pOdbIncludeFlag, int pOdbCxlCorrExcludeId){
        init();
        this.TradeId = pTradeId;
        this.ColName = pColName;
        this.OldValue = pOldValue;
        this.NewValue = pNewValue;
        this.UpdBusnDt = pUpdBusnDt;
        this.UserName = pUserName;
        this.AuditTypeCode = pAuditTypeCode;
        this.ODBIncludeFlag = pOdbIncludeFlag;
        this.ODBCxlCorrExcludeId = pOdbCxlCorrExcludeId;
    }

}