package aff.confirm.webservices.tradegateway.data;


import aff.confirm.webservices.tradegateway.util.DateAdapter;
import aff.confirm.webservices.tradegateway.util.StringAdapter;

import javax.persistence.*;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.adapters.XmlJavaTypeAdapter;
import java.util.Date;


@Entity
@NamedNativeQuery( name="sp_tradedata",
        query = "call ops_tracking.pkg_aff_trade.p_get_trade(?,:id)",
        resultClass = aff.confirm.webservices.tradegateway.data.TradeData.class,
        hints = {@QueryHint(name="org.hibernate.callable",value = "true")})

@Table(name = "v_ops_tracking_data",schema = "infinity_mgr")
@XmlRootElement(name = "TradeData")

public class TradeData {


    @Transient
    private Integer id =new Integer(0);

    @Id
    @Column(name = "prmnt_trade_id")
    private Integer trade_id;

    @Column(name = "inception_dt")
    private Date inception_dt;

    @Column(name = "cdty_code")
    private String cdty_code = "" ;

    @Column(name = "trade_dt")
    private Date trade_dt  ;

    @Column(name = "xref")
    private String xref = "";

    @Column(name = "cpty_sn")
    private String cpty_sn = "";

    @Column(name = "total_nom_qty")
    private Double qty_tot = new Double(0) ;

    @Column(name = "qty_per")
    private Double qty = new Double(0) ;

    @Column(name = "uom_dur_code")
    private String uom_dur_code = "";

    @Column(name = "location_sn")
    private String location_sn = "";

    @Transient
    private String price_desc = "";

    @Column(name = "start_dt")
    private Date start_dt ;

    @Column(name = "end_dt")
    private Date end_dt ;

    @Column(name = "book")
    private String book = "";

    @Column(name = "trade_type_code")
    private String trade_type_code = "";

    @Column(name = "trade_sttl_type_code")
    private String sttl_type = "";

    @Column(name="broker_sn")
    private String broker_short_name = "";

    @Column(name = "bkrg_rate")
    private Double comm  = new Double(0);

    @Column(name = "buy_sell_ind")
    private String buy_sell_ind = "";

    @Column(name="reference")
    private String ref_sn = "";

    @Column(name = "pay_price")
    private String pay_price = "";

    @Column(name = "rec_price")
    private String rec_price = "";

    @Column(name = "se_cpty_sn")
    private String se_cpty_sn = "";


    @Column(name = "trade_stat_code")
    private String trade_stat_code = "" ;

    @Column(name="optn_strike_price")
    private Double optn_strike_price = new Double(0) ;

    @Column(name="optn_prem_price")
    private Double optn_prem_price  = new Double(0);

    @Column(name = "broker_price")
    private Double broker_price = new Double(0)  ;

    @Column(name = "optn_put_call_ind")
    private String optn_put_call_ind = "";

    @Transient
    private String trade_sys_code = "";

    @Transient
    private String ticket = "";

    @XmlElement(required = true)
    public Integer getId() {
        return id;
    }

    @XmlElement(required = true)
    public Integer getTrade_id() {
        return trade_id;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(DateAdapter.class)
    public Date getInception_dt() {
        return inception_dt;
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getCdty_code() {
        return cdty_code;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(DateAdapter.class)
    public Date getTrade_dt() {
        return trade_dt;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getXref() {
        return xref;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getCpty_sn() {
        return cpty_sn;
    }
    @XmlElement(required = true)
    public Double getQty_tot() {
        if (qty_tot != null) {
            return qty_tot;
        }
        return new Double(0);
    }

    @XmlElement(required = true)
    public Double getQty() {
        if (qty != null) {
            return qty;
        }
        return new Double(0);
    }

    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getUom_dur_code() {
        return uom_dur_code;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getLocation_sn() {
        return location_sn;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrice_desc() {
        return price_desc;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(DateAdapter.class)
    public Date getStart_dt() {
        return start_dt;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(DateAdapter.class)
    public Date getEnd_dt() {
        return end_dt;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getBook() {
        return book;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getTrade_type_code() {
        return trade_type_code;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getSttl_type() {
        return sttl_type;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getBroker_short_name() {
        return broker_short_name;
    }
    @XmlElement(required = true)
    public Double getComm() {
        if (comm != null) {
            return comm;
        }
        return  new Double(0);
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getBuy_sell_ind() {
        return buy_sell_ind;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getRef_sn() {
        return ref_sn;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPay_price() {
        return pay_price;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getRec_price() {
        return rec_price;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getSe_cpty_sn() {
        return se_cpty_sn;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getTrade_stat_code() {
        return trade_stat_code;
    }
    @XmlElement(required = true)
    public Double getOptn_strike_price() {
        if (optn_strike_price != null) {
            return optn_strike_price;
        }
        return new Double(0);
    }
    @XmlElement(required = true)
    public Double getOptn_prem_price() {
        if (optn_prem_price != null ) {
            return optn_prem_price;
        }
        return new Double(0);
    }
    @XmlElement(required = true)
    public Double getBroker_price() {
        if (broker_price != null) {
            return broker_price;
        }
        return new Double(0);
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getOptn_put_call_ind() {
        return optn_put_call_ind;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getTrade_sys_code() {
        return trade_sys_code;
    }
    @XmlElement(required = true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getTicket() {
        return ticket;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public void setTrade_id(Integer trade_id) {
        this.trade_id = trade_id;
    }

    public void setInception_dt(Date inception_dt) {
        this.inception_dt = inception_dt;
    }

    public void setCdty_code(String cdty_code) {
        this.cdty_code = cdty_code;
    }

    public void setTrade_dt(Date trade_dt) {
        this.trade_dt = trade_dt;
    }

    public void setXref(String xref) {
        this.xref = xref;
    }

    public void setCpty_sn(String cpty_sn) {
        this.cpty_sn = cpty_sn;
    }

    public void setQty_tot(Double qty_tot) {
        this.qty_tot = qty_tot;
    }

    public void setQty(Double qty) {
        this.qty = qty;
    }

    public void setUom_dur_code(String uom_dur_code) {
        this.uom_dur_code = uom_dur_code;
    }

    public void setLocation_sn(String location_sn) {
        this.location_sn = location_sn;
    }

    public void setPrice_desc(String price_desc) {
        this.price_desc = price_desc;
    }

    public void setStart_dt(Date start_dt) {
        this.start_dt = start_dt;
    }

    public void setEnd_dt(Date end_dt) {
        this.end_dt = end_dt;
    }

    public void setBook(String book) {
        this.book = book;
    }

    public void setTrade_type_code(String trade_type_code) {
        this.trade_type_code = trade_type_code;
    }

    public void setSttl_type(String sttl_type) {
        this.sttl_type = sttl_type;
    }

    public void setBroker_short_name(String broker_short_name) {
        this.broker_short_name = broker_short_name;
    }

    public void setComm(Double comm) {
        this.comm = comm;
    }

    public void setBuy_sell_ind(String buy_sell_ind) {
        this.buy_sell_ind = buy_sell_ind;
    }

    public void setRef_sn(String ref_sn) {
        this.ref_sn = ref_sn;
    }

    public void setPay_price(String pay_price) {
        this.pay_price = pay_price;
    }

    public void setRec_price(String rec_price) {
        this.rec_price = rec_price;
    }

    public void setSe_cpty_sn(String se_cpty_sn) {
        this.se_cpty_sn = se_cpty_sn;
    }

    public void setTrade_stat_code(String trade_stat_code) {
        this.trade_stat_code = trade_stat_code;
    }

    public void setOptn_strike_price(Double optn_strike_price) {
        this.optn_strike_price = optn_strike_price;
    }

    public void setOptn_prem_price(Double optn_prem_price) {
        this.optn_prem_price = optn_prem_price;
    }

    public void setBroker_price(Double broker_price) {
        this.broker_price = broker_price;
    }

    public void setOptn_put_call_ind(String optn_put_call_ind) {
        this.optn_put_call_ind = optn_put_call_ind;
    }

    public void setTrade_sys_code(String trade_sys_code) {
        this.trade_sys_code = trade_sys_code;
    }

    public void setTicket(String ticket) {
        this.ticket = ticket;
    }
}
