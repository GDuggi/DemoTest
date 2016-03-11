package aff.confirm.common.ottradealert;

import java.text.SimpleDateFormat;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 */
public class OpsTrackingSecondCheck_rec {
    public SimpleDateFormat sdfOTSecondCheck = new SimpleDateFormat("MM/dd/yyyy");
    //public double tradeId;
    public String cdtyCode;
    /*private String inceptionDt;
    private String startDt;*/
    public java.sql.Date startDt;
    public java.sql.Date endDt;
    public String seCptySn;
    public String cptySn;
    public String sttlType;
    //public String  brokerSn;
    //public boolean isEConfirm;
    public String rqmtCode;
    //public String rqmtCodes;

    public OpsTrackingSecondCheck_rec(){
        init();
    }

    /*public void setInceptionDt(String pInceptionDt){
        inceptionDt = pInceptionDt;
    }

    public void setStartDt(String pStartDt){
        startDt = pStartDt;
    }

    public java.util.Date getInceptionDt(){
        java.util.Date dtInceptDt = null;
        try {
            dtInceptDt = sdfOTSecondCheck.parse(inceptionDt);
        } catch (ParseException e) {
        }
        return dtInceptDt;
    }

    public java.util.Date getStartDt(){
        java.util.Date dtStartDt = null;
        try {
            dtStartDt = sdfOTSecondCheck.parse(startDt);
        } catch (ParseException e) {
        }
        return dtStartDt;
    }*/

    public void init(){
        //this.tradeId = 0;
        this.cdtyCode = "";
        /*this.inceptionDt = "";
        this.startDt = "";*/
        this.seCptySn = "";
        this.cptySn = "";
        this.sttlType = "";
        //this.brokerSn = "";
        this.rqmtCode = "";
        //this.rqmtCodes = "";
        this.startDt = null;
        this.endDt = null;
    }

}
