package aff.confirm.webservices.tradegateway.data;

import aff.confirm.webservices.tradegateway.util.DateAdapter;
import aff.confirm.webservices.tradegateway.util.StringAdapter;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.adapters.XmlJavaTypeAdapter;
import java.util.Date;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 1/25/13
 * Time: 11:42 AM
 */
@XmlRootElement(name = "TradeAlertData")
public class TradeAlertData {


    private Integer trade_id;
    private String audit_type_code  = "";
    private String cdty_grp_code  = "";
    private String cdty_code = "";
    private String booking_company_short_name  = "";
    private String cpty_short_name = "";
    private String broker_short_name = "";
    private Date trade_dt ;
    private String trade_type_code = "";
    private String inst_code = "";
    private String trade_stat_code = "" ;
    private String trade_mode_name = "";


    public Integer getTrade_id() {
        return trade_id;
    }

    public String getAudit_type_code() {
        return audit_type_code;
    }

    public String getCdty_grp_code() {
        return cdty_grp_code;
    }

    public String getCdty_code() {
        return cdty_code;
    }

    public String getBooking_company_short_name() {
        return booking_company_short_name;
    }

    public String getCpty_short_name() {
        return cpty_short_name;
    }

    public String getBroker_short_name() {
        return broker_short_name;
    }

    public Date getTrade_dt() {
        return trade_dt;
    }

    public String getTrade_type_code() {
        return trade_type_code;
    }

    public String getInst_code() {
        return inst_code;
    }

    public String getTrade_stat_code() {
        return trade_stat_code;
    }

    public String getTrade_mode_name() {
        return trade_mode_name;
    }


    @XmlElement(required = true)
    public void setTrade_id(Integer trade_id) {
        this.trade_id = trade_id;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public void setAudit_type_code(String audit_type_code) {
        this.audit_type_code = audit_type_code;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public void setCdty_grp_code(String cdty_grp_code) {
        this.cdty_grp_code = cdty_grp_code;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public void setCdty_code(String cdty_code) {
        this.cdty_code = cdty_code;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public void setBooking_company_short_name(String booking_company_short_name) {
        this.booking_company_short_name = booking_company_short_name;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public void setCpty_short_name(String cpty_short_name) {
        this.cpty_short_name = cpty_short_name;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public void setBroker_short_name(String broker_short_name) {
        this.broker_short_name = broker_short_name;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(DateAdapter.class)
    public void setTrade_dt(Date trade_dt) {
        this.trade_dt = trade_dt;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public void setTrade_type_code(String trade_type_code) {
        this.trade_type_code = trade_type_code;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public void setInst_code(String inst_code) {
        this.inst_code = inst_code;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public void setTrade_stat_code(String trade_stat_code) {
        this.trade_stat_code = trade_stat_code;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public void setTrade_mode_name(String trade_mode_name) {
        this.trade_mode_name = trade_mode_name;
    }

}
