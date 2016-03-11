package aff.confirm.webservices.tradegateway.data;


import aff.confirm.webservices.tradegateway.util.*;

import javax.persistence.*;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.adapters.XmlJavaTypeAdapter;
import java.util.Date;

@Entity


@NamedNativeQuery( name="sp_contract",
                    query = "call infinity_mgr.pkg_contract.p_get_contract_feed(?,:id)",
                    resultClass = aff.confirm.webservices.tradegateway.data.ContractData.class,
                    hints = {@QueryHint(name="org.hibernate.callable",value = "true")})
@XmlRootElement(name = "ContractData")

public class ContractData {
    //Declare the columns and variables
    @Column(name = "id")
    private  Integer id ;

    @Column(name="trading_system_code")
    private String trading_system_code ;

    @Id
    @Column(name="trading_system_id")
    private String trading_system_id;

    @Column(name="trade_dt")
    private Date trade_dt ;

    @Column(name="se_cmpny_sn")
    private String se_cmpny_sn = "" ;

    @Column(name="se_trader")
    private String se_trader  =  "" ;

    @Column(name="cpty_sn")
    private String cpty_sn  = "" ;

    @Column(name = "cpty_trader")
    private String cpty_trader  = "" ;

    @Column(name="broker_sn")
    private String broker_sn = ""  ;

    @Column(name="inst_type")
    private String inst_type = "" ;

    @Column(name="cdty_code")
    private String cdty_code = "" ;

    @Column(name="sttl_type")
    private String sttl_type = "" ;

    @Column(name="se_buysell_ind")
    private String se_buysell_ind = "" ;

    @Column(name="efp_flag")
    private String efp_flag = "" ;

    @Column(name="dlvry_start_dt")
    private Date dlvry_start_dt   ;

    @Column(name="dlvry_end_dt")
    private Date dlvry_end_dt  ;


    @Transient
    private String dmo_count = "" ;

    @Transient
    private String cycle_number = "" ;

    @Column(name="dlvry_location")
    private String dlvry_location  = "" ;

    @Column(name="qty_per")
    private String qty_per = "" ;

    @Column(name="qty_uom_code")
    private String qty_uom_code = "" ;

    @Column(name="qty_per_duration_code")
    private String qty_per_duration_code = "" ;

    @Column(name="qty_total")
    private String qty_total = "" ;

    @Column(name="prc_1_payor_sn")
    private String prc_1_payor_sn = "" ;

    @Column(name="prc_1_fixed_flag")
    private String prc_1_fixed_flag = "" ;

    @Column(name="prc_1_pricediff")
    private String prc_1_pricediff = "" ;

    @Column(name="prc_1_ccy_code")
    private String prc_1_ccy_code = "" ;

    @Column(name="prc_1_uom_code")
    private String prc_1_uom_code = "" ;

    @Column(name="prc_1_curve")
    private String prc_1_curve = "" ;

    @Column(name="prc_1_start_dt")
    private Date prc_1_start_dt  ;

    @Column(name="prc_1_end_dt")
    private Date prc_1_end_dt  ;

    @Transient
    private String prc_1_trig_start_dt = "" ;

    @Transient
    private String prc_1_trig_end_dt = "" ;

    @Transient
    private String prc_1_contract_month = "" ;

    @Column(name="prc_1_exch_roll_a")
    private String prc_1_exch_roll_a = "" ;

    @Transient
    private String prc_1_exch_roll_b = "" ;

    @Transient
    private String prc_2_payor_sn = "" ;

    @Transient
    private String prc_2_fixed_flag = "" ;

    @Transient
    private String prc_2_pricediff = "" ;

    @Transient
    private String prc_2_ccy_code = "" ;

    @Transient
    private String prc_2_uom_code = "" ;

    @Transient
    private String prc_2_curve = "" ;


    @Column(name = "prc_2_start_dt")
    private Date prc_2_start_dt  ;

    @Column(name = "prc_2_end_dt")
    private Date prc_2_end_dt  ;

    @Transient
    private String prc_2_trig_start_dt = "" ;

    @Transient
    private String prc_2_trig_end_dt = "" ;

    @Transient
    private String prc_2_contract_month = "" ;

    @Transient
    private String prc_2_exch_roll_a = "" ;

    @Transient
    private String prc_2_exch_roll_b = "" ;

    @Transient
    private String swap_com_prc_flag = "" ;

    @Column(name="sttl_model")
    private String sttl_model = ""  ;

    @Column(name="sttl_ccy_code")
    private String sttl_ccy_code = "" ;

    @Column(name="sttl_month_offset")
    private String sttl_month_offset = "" ;

    @Column(name="sttl_days_offset")
    private String sttl_days_offset = "" ;


    @Column(name="trade_time")
    private String trade_time= "" ;

    @Column(name="book_sn")
    private String  book_sn= "" ;

    @Column(name="reference")
    private String  reference= "" ;

    @Column(name="service_code")
    private String  service_code = "" ;

    @Column(name="dlvry_region")
    private String  dlvry_region = "" ;

    @Column(name="dlvry_zone")
    private String  dlvry_zone= "" ;

    @Column(name="prc_1_model")
    private String  prc_1_model= "" ;

    @Column(name="prc_1_hrs_ind")
    private String  prc_1_hrs_ind= "" ;

    @Column(name="sttl_dt_final")
    private Date  sttl_dt_final  ;

    @Column(name="neta_buy_acct_no")
    private String  neta_buy_acct_no = "" ;

    @Column(name="neta_sell_acct_no")
    private String  neta_sell_acct_no = "" ;

    @Column(name="ext_trnsmssn_ind")
    private String  ext_trnsmssn_ind = "" ;

    @Column(name="dlvry_period_text")
    private String  dlvry_period_text = "" ;


    @Column(name="dlvry_rate_text")
    private String  dlvry_rate_text = "" ;

    @Column(name="prc_1_prc_cnv_factor")
    private String  prc_1_prc_cnv_factor = "" ;

    @Column(name="xref")
    private String  xref = "" ;

    @Column(name="strategy_sn")
    private String  strategy_sn = "" ;

    @Column(name="deal_sn")
    private String  deal_sn = "" ;

    @Column(name="var_qty_wording")
    private String  var_qty_wording = "" ;

    @Column(name="ama_fuel_cost")
    private String  ama_fuel_cost = "" ;

    @Column(name="ama_cdty_rate")
    private String  ama_cdty_rate = "" ;

    @Column(name="esco_addr_rate")
    private String  esco_addr_rate = "" ;

    //Added 7/26/13 by Israel
    @Column(name="mot_type")
    private String mot_type = "" ;

    @Column(name="mot")
    private String mot = "" ;

    @Column(name="lease_tank")
    private String lease_tank = "" ;

    @Column(name="load_port_loc")
    private String load_port_loc = "" ;

    @Column(name="disch_port_loc")
    private String disch_port_loc = "" ;

    @Column(name="origin_country")
    private String origin_country = "" ;

    //Declare xml elements, getters
    @XmlElement(required =true)
    public Integer getId() {
        return id;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getTrading_system_code() {
        return trading_system_code;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getTrading_system_id() {
        return trading_system_id;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(ContractDateAdapter.class)
    public Date getTrade_dt() {
        return trade_dt;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getSe_cmpny_sn() {
        return se_cmpny_sn;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getSe_trader() {
        return se_trader;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getCpty_sn() {
        return cpty_sn;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getCpty_trader() {
        return cpty_trader;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getBroker_sn() {
        return broker_sn;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getInst_type() {
        return inst_type;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getCdty_code() {
        return cdty_code;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getSttl_type() {
        return sttl_type;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getSe_buysell_ind() {
        return se_buysell_ind;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getEfp_flag() {
        return efp_flag;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(ContractDateAdapter.class)
    public Date getDlvry_start_dt() {
        return dlvry_start_dt;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(ContractDateAdapter.class)
    public Date getDlvry_end_dt() {
        return dlvry_end_dt;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getDmo_count() {
        return dmo_count;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getCycle_number() {
        return cycle_number;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getDlvry_location() {
        return dlvry_location;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getQty_per() {
        return qty_per;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getQty_uom_code() {
        return qty_uom_code;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getQty_per_duration_code() {
        return qty_per_duration_code;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getQty_total() {
        return qty_total;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_payor_sn() {
        return prc_1_payor_sn;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_fixed_flag() {
        return prc_1_fixed_flag;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_pricediff() {
        return prc_1_pricediff;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_ccy_code() {
        return prc_1_ccy_code;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_uom_code() {
        return prc_1_uom_code;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_curve() {
        return prc_1_curve;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(ContractDateAdapter.class)
    public Date getPrc_1_start_dt() {
        return prc_1_start_dt;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(ContractDateAdapter.class)
    public Date getPrc_1_end_dt() {
        return prc_1_end_dt;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_trig_start_dt() {
        return prc_1_trig_start_dt;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_trig_end_dt() {
        return prc_1_trig_end_dt;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_contract_month() {
        return prc_1_contract_month;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_exch_roll_a() {
        return prc_1_exch_roll_a;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_exch_roll_b() {
        return prc_1_exch_roll_b;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_2_payor_sn() {
        return prc_2_payor_sn;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_2_fixed_flag() {
        return prc_2_fixed_flag;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_2_pricediff() {
        return prc_2_pricediff;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_2_ccy_code() {
        return prc_2_ccy_code;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_2_uom_code() {
        return prc_2_uom_code;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_2_curve() {
        return prc_2_curve;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(ContractDateAdapter.class)
    public Date getPrc_2_start_dt() {
        return prc_2_start_dt;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(ContractDateAdapter.class)
    public Date getPrc_2_end_dt() {
        return prc_2_end_dt;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_2_trig_start_dt() {
        return prc_2_trig_start_dt;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_2_trig_end_dt() {
        return prc_2_trig_end_dt;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_2_contract_month() {
        return prc_2_contract_month;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_2_exch_roll_a() {
        return prc_2_exch_roll_a;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_2_exch_roll_b() {
        return prc_2_exch_roll_b;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getSwap_com_prc_flag() {
        return swap_com_prc_flag;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getSttl_model() {
        return sttl_model;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getSttl_ccy_code() {
        return sttl_ccy_code;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getSttl_month_offset() {
        return sttl_month_offset;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getSttl_days_offset() {
        return sttl_days_offset;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getTrade_time() {
        return trade_time;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getBook_sn() {
        return book_sn;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getReference() {
        return reference;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getService_code() {
        return service_code;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getDlvry_region() {
        return dlvry_region;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getDlvry_zone() {
        return dlvry_zone;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_model() {
        return prc_1_model;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_hrs_ind() {
        return prc_1_hrs_ind;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(ContractDateAdapter.class)
    public Date getSttl_dt_final() {
        return sttl_dt_final;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getNeta_buy_acct_no() {
        return neta_buy_acct_no;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getNeta_sell_acct_no() {
        return neta_sell_acct_no;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getExt_trnsmssn_ind() {
        return ext_trnsmssn_ind;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getDlvry_period_text() {
        return dlvry_period_text;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getDlvry_rate_text() {
        return dlvry_rate_text;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getPrc_1_prc_cnv_factor() {
        return prc_1_prc_cnv_factor;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getXref() {
        return xref;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getStrategy_sn() {
        return strategy_sn;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getDeal_sn() {
        return deal_sn;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getVar_qty_wording() {
        return var_qty_wording;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getAma_fuel_cost() {
        return ama_fuel_cost;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getAma_cdty_rate() {
        return ama_cdty_rate;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getEsco_addr_rate() {
        return esco_addr_rate;
    }

    // Added 7/26/13 by Israel
    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getMot_type() {
        return mot_type;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getMot() {
        return mot;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getLease_tank() {
        return lease_tank;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getLoad_port_loc() {
        return load_port_loc;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getDisch_port_loc() {
        return disch_port_loc;
    }

    @XmlElement(required =true)
    @XmlJavaTypeAdapter(StringAdapter.class)
    public String getOrigin_country() {
        return origin_country;
    }

    //Declare the setters
    public void setId(Integer id) {
        this.id = id;
    }

    public void setTrading_system_code(String trading_system_code) {
        this.trading_system_code = trading_system_code;
    }

    public void setTrading_system_id(String trading_system_id) {
        this.trading_system_id = trading_system_id;
    }

    public void setTrade_dt(Date trade_dt) {
        this.trade_dt = trade_dt;
    }

    public void setSe_cmpny_sn(String se_cmpny_sn) {
        this.se_cmpny_sn = se_cmpny_sn;
    }

    public void setSe_trader(String se_trader) {
        this.se_trader = se_trader;
    }

    public void setCpty_sn(String cpty_sn) {
        this.cpty_sn = cpty_sn;
    }

    public void setCpty_trader(String cpty_trader) {
        this.cpty_trader = cpty_trader;
    }

    public void setBroker_sn(String broker_sn) {
        this.broker_sn = broker_sn;
    }

    public void setInst_type(String inst_type) {
        this.inst_type = inst_type;
    }

    public void setCdty_code(String cdty_code) {
        this.cdty_code = cdty_code;
    }

    public void setSttl_type(String sttl_type) {
        this.sttl_type = sttl_type;
    }

    public void setSe_buysell_ind(String se_buysell_ind) {
        this.se_buysell_ind = se_buysell_ind;
    }

    public void setEfp_flag(String efp_flag) {
        this.efp_flag = efp_flag;
    }

    public void setDlvry_start_dt(Date dlvry_start_dt) {
        this.dlvry_start_dt = dlvry_start_dt;
    }

    public void setDlvry_end_dt(Date dlvry_end_dt) {
        this.dlvry_end_dt = dlvry_end_dt;
    }

    public void setDmo_count(String dmo_count) {
        this.dmo_count = dmo_count;
    }

    public void setCycle_number(String cycle_number) {
        this.cycle_number = cycle_number;
    }

    public void setDlvry_location(String dlvry_location) {
        this.dlvry_location = dlvry_location;
    }

    public void setQty_per(String qty_per) {
        this.qty_per = qty_per;
    }

    public void setQty_uom_code(String qty_uom_code) {
        this.qty_uom_code = qty_uom_code;
    }

    public void setQty_per_duration_code(String qty_per_duration_code) {
        this.qty_per_duration_code = qty_per_duration_code;
    }

    public void setQty_total(String qty_total) {
        this.qty_total = qty_total;
    }

    public void setPrc_1_payor_sn(String prc_1_payor_sn) {
        this.prc_1_payor_sn = prc_1_payor_sn;
    }

    public void setPrc_1_fixed_flag(String prc_1_fixed_flag) {
        this.prc_1_fixed_flag = prc_1_fixed_flag;
    }

    public void setPrc_1_pricediff(String prc_1_pricediff) {
        this.prc_1_pricediff = prc_1_pricediff;
    }

    public void setPrc_1_ccy_code(String prc_1_ccy_code) {
        this.prc_1_ccy_code = prc_1_ccy_code;
    }

    public void setPrc_1_uom_code(String prc_1_uom_code) {
        this.prc_1_uom_code = prc_1_uom_code;
    }

    public void setPrc_1_curve(String prc_1_curve) {
        this.prc_1_curve = prc_1_curve;
    }

    public void setPrc_1_start_dt(Date prc_1_start_dt) {
        this.prc_1_start_dt = prc_1_start_dt;
    }

    public void setPrc_1_end_dt(Date prc_1_end_dt) {
        this.prc_1_end_dt = prc_1_end_dt;
    }

    public void setPrc_1_trig_start_dt(String prc_1_trig_start_dt) {
        this.prc_1_trig_start_dt = prc_1_trig_start_dt;
    }

    public void setPrc_1_trig_end_dt(String prc_1_trig_end_dt) {
        this.prc_1_trig_end_dt = prc_1_trig_end_dt;
    }

    public void setPrc_1_contract_month(String prc_1_contract_month) {
        this.prc_1_contract_month = prc_1_contract_month;
    }

    public void setPrc_1_exch_roll_a(String prc_1_exch_roll_a) {
        this.prc_1_exch_roll_a = prc_1_exch_roll_a;
    }

    public void setPrc_1_exch_roll_b(String prc_1_exch_roll_b) {
        this.prc_1_exch_roll_b = prc_1_exch_roll_b;
    }

    public void setPrc_2_payor_sn(String prc_2_payor_sn) {
        this.prc_2_payor_sn = prc_2_payor_sn;
    }

    public void setPrc_2_fixed_flag(String prc_2_fixed_flag) {
        this.prc_2_fixed_flag = prc_2_fixed_flag;
    }

    public void setPrc_2_pricediff(String prc_2_pricediff) {
        this.prc_2_pricediff = prc_2_pricediff;
    }

    public void setPrc_2_ccy_code(String prc_2_ccy_code) {
        this.prc_2_ccy_code = prc_2_ccy_code;
    }

    public void setPrc_2_uom_code(String prc_2_uom_code) {
        this.prc_2_uom_code = prc_2_uom_code;
    }

    public void setPrc_2_curve(String prc_2_curve) {
        this.prc_2_curve = prc_2_curve;
    }

    public void setPrc_2_start_dt(Date prc_2_start_dt) {
        this.prc_2_start_dt = prc_2_start_dt;
    }

    public void setPrc_2_end_dt(Date prc_2_end_dt) {
        this.prc_2_end_dt = prc_2_end_dt;
    }

    public void setPrc_2_trig_start_dt(String prc_2_trig_start_dt) {
        this.prc_2_trig_start_dt = prc_2_trig_start_dt;
    }

    public void setPrc_2_trig_end_dt(String prc_2_trig_end_dt) {
        this.prc_2_trig_end_dt = prc_2_trig_end_dt;
    }

    public void setPrc_2_contract_month(String prc_2_contract_month) {
        this.prc_2_contract_month = prc_2_contract_month;
    }

    public void setPrc_2_exch_roll_a(String prc_2_exch_roll_a) {
        this.prc_2_exch_roll_a = prc_2_exch_roll_a;
    }

    public void setPrc_2_exch_roll_b(String prc_2_exch_roll_b) {
        this.prc_2_exch_roll_b = prc_2_exch_roll_b;
    }

    public void setSwap_com_prc_flag(String swap_com_prc_flag) {
        this.swap_com_prc_flag = swap_com_prc_flag;
    }

    public void setSttl_model(String sttl_model) {
        this.sttl_model = sttl_model;
    }

    public void setSttl_ccy_code(String sttl_ccy_code) {
        this.sttl_ccy_code = sttl_ccy_code;
    }

    public void setSttl_month_offset(String sttl_month_offset) {
        this.sttl_month_offset = sttl_month_offset;
    }

    public void setSttl_days_offset(String sttl_days_offset) {
        this.sttl_days_offset = sttl_days_offset;
    }

    public void setTrade_time(String trade_time) {
        this.trade_time = trade_time;
    }

    public void setBook_sn(String book_sn) {
        this.book_sn = book_sn;
    }

    public void setReference(String reference) {
        this.reference = reference;
    }

    public void setService_code(String service_code) {
        this.service_code = service_code;
    }

    public void setDlvry_region(String dlvry_region) {
        this.dlvry_region = dlvry_region;
    }

    public void setDlvry_zone(String dlvry_zone) {
        this.dlvry_zone = dlvry_zone;
    }

    public void setPrc_1_model(String prc_1_model) {
        this.prc_1_model = prc_1_model;
    }

    public void setPrc_1_hrs_ind(String prc_1_hrs_ind) {
        this.prc_1_hrs_ind = prc_1_hrs_ind;
    }

    public void setSttl_dt_final(Date sttl_dt_final) {
        this.sttl_dt_final = sttl_dt_final;
    }

    public void setNeta_buy_acct_no(String neta_buy_acct_no) {
        this.neta_buy_acct_no = neta_buy_acct_no;
    }

    public void setNeta_sell_acct_no(String neta_sell_acct_no) {
        this.neta_sell_acct_no = neta_sell_acct_no;
    }

    public void setExt_trnsmssn_ind(String ext_trnsmssn_ind) {
        this.ext_trnsmssn_ind = ext_trnsmssn_ind;
    }

    public void setDlvry_period_text(String dlvry_period_text) {
        this.dlvry_period_text = dlvry_period_text;
    }

    public void setDlvry_rate_text(String dlvry_rate_text) {
        this.dlvry_rate_text = dlvry_rate_text;
    }

    public void setPrc_1_prc_cnv_factor(String prc_1_prc_cnv_factor) {
        this.prc_1_prc_cnv_factor = prc_1_prc_cnv_factor;
    }

    public void setXref(String xref) {
        this.xref = xref;
    }

    public void setStrategy_sn(String strategy_sn) {
        this.strategy_sn = strategy_sn;
    }

    public void setDeal_sn(String deal_sn) {
        this.deal_sn = deal_sn;
    }

    public void setVar_qty_wording(String var_qty_wording) {
        this.var_qty_wording = var_qty_wording;
    }

    public void setAma_fuel_cost(String ama_fuel_cost) {
        this.ama_fuel_cost = ama_fuel_cost;
    }

    public void setAma_cdty_rate(String ama_cdty_rate) {
        this.ama_cdty_rate = ama_cdty_rate;
    }

    public void setEsco_addr_rate(String esco_addr_rate) {
        this.esco_addr_rate = esco_addr_rate;
    }

    // Added 7/26/13 by Israel
    public void setMot_type(String mot_type) {
        this.mot_type = mot_type;
    }

    public void setMot(String mot) {
        this.mot = mot;
    }

    public void setLease_tank(String lease_tank) {
        this.lease_tank = lease_tank;
    }

    public void setLoad_port_loc(String load_port_loc) {
        this.load_port_loc = load_port_loc;
    }

    public void setDisch_port_loc(String disch_port_loc) {
        this.disch_port_loc = disch_port_loc;
    }

    public void setOrigin_country(String origin_country) {
        this.origin_country = origin_country;
    }
}
