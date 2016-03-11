package aff.confirm.common.ottradealert;

public class OpsTrackingODB_CANCEL_CORRECT_EXCLUDE_rec {
    //public SimpleDateFormat sdfOTSecondCheck = new SimpleDateFormat("MM/dd/yyyy");
    public double Id;
    public String auditTypeCode;
    public String seCptySn;
    public String cptySn;
    public String colName;
    public String bookSn;
    public String cmt;

    public OpsTrackingODB_CANCEL_CORRECT_EXCLUDE_rec(){
        init();
    }

    public void init(){
        this.Id = 0;
        this.auditTypeCode = "";
        this.seCptySn = "";
        this.cptySn = "";
        this.colName = "";
        this.bookSn = "";
        this.cmt = "";
    }

}