package aff.confirm.common.daoinbound.inbound.model;

import javax.persistence.*;
import java.io.Serializable;
import java.math.BigDecimal;
import java.sql.Timestamp;

/**
 * User: mthoresen
 * Date: Feb 3, 2010
 * Time: 9:51:43 AM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "TRADE_DATA")
public class TradeDataEntity implements Serializable {
    private Long id;

    @Id
    @Column(name = "ID", nullable = false, length = 12)
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    private Long tradeId;

    @Basic
    @Column(name = "TRADE_ID", nullable = false, length = 12)
    public Long getTradeId() {
        return tradeId;
    }

    public void setTradeId(Long tradeId) {
        this.tradeId = tradeId;
    }

    private Timestamp inceptionDt;

    @Basic
    @Column(name = "INCEPTION_DT", length = 7)
    public Timestamp getInceptionDt() {
        return inceptionDt;
    }

    public void setInceptionDt(Timestamp inceptionDt) {
        this.inceptionDt = inceptionDt;
    }

    private String cdtyCode;

    @Basic
    @Column(name = "CDTY_CODE", length = 10)
    public String getCdtyCode() {
        return cdtyCode;
    }

    public void setCdtyCode(String cdtyCode) {
        this.cdtyCode = cdtyCode;
    }

    private Timestamp tradeDt;

    @Basic
    @Column(name = "TRADE_DT", length = 7)
    public Timestamp getTradeDt() {
        return tradeDt;
    }

    public void setTradeDt(Timestamp tradeDt) {
        this.tradeDt = tradeDt;
    }

    private String xref;

    @Basic
    @Column(name = "XREF", length = 20)
    public String getXref() {
        return xref;
    }

    public void setXref(String xref) {
        this.xref = xref;
    }

    private String cptySn;

    @Basic
    @Column(name = "CPTY_SN", length = 20)
    public String getCptySn() {
        return cptySn;
    }

    public void setCptySn(String cptySn) {
        this.cptySn = cptySn;
    }

    private BigDecimal qtyTot;

    @Basic
    @Column(name = "QTY_TOT", length = 24, precision = 12)
    public BigDecimal getQtyTot() {
        return qtyTot;
    }

    public void setQtyTot(BigDecimal qtyTot) {
        this.qtyTot = qtyTot;
    }

    private BigDecimal qty;

    @Basic
    @Column(name = "QTY", length = 24, precision = 12)
    public BigDecimal getQty() {
        return qty;
    }

    public void setQty(BigDecimal qty) {
        this.qty = qty;
    }

    private String uomDurCode;

    @Basic
    @Column(name = "UOM_DUR_CODE", length = 20)
    public String getUomDurCode() {
        return uomDurCode;
    }

    public void setUomDurCode(String uomDurCode) {
        this.uomDurCode = uomDurCode;
    }

    private String locationSn;

    @Basic
    @Column(name = "LOCATION_SN", length = 20)
    public String getLocationSn() {
        return locationSn;
    }

    public void setLocationSn(String locationSn) {
        this.locationSn = locationSn;
    }

    private String priceDesc;

    @Basic
    @Column(name = "PRICE_DESC", length = 50)
    public String getPriceDesc() {
        return priceDesc;
    }

    public void setPriceDesc(String priceDesc) {
        this.priceDesc = priceDesc;
    }

    private Timestamp startDt;

    @Basic
    @Column(name = "START_DT", length = 7)
    public Timestamp getStartDt() {
        return startDt;
    }

    public void setStartDt(Timestamp startDt) {
        this.startDt = startDt;
    }

    private Timestamp endDt;

    @Basic
    @Column(name = "END_DT", length = 7)
    public Timestamp getEndDt() {
        return endDt;
    }

    public void setEndDt(Timestamp endDt) {
        this.endDt = endDt;
    }

    private String book;

    @Basic
    @Column(name = "BOOK", length = 20)
    public String getBook() {
        return book;
    }

    public void setBook(String book) {
        this.book = book;
    }

    private String tradeTypeCode;

    @Basic
    @Column(name = "TRADE_TYPE_CODE", length = 10)
    public String getTradeTypeCode() {
        return tradeTypeCode;
    }

    public void setTradeTypeCode(String tradeTypeCode) {
        this.tradeTypeCode = tradeTypeCode;
    }

    private String sttlType;

    @Basic
    @Column(name = "STTL_TYPE", length = 10)
    public String getSttlType() {
        return sttlType;
    }

    public void setSttlType(String sttlType) {
        this.sttlType = sttlType;
    }

    private String brokerSn;

    @Basic
    @Column(name = "BROKER_SN", length = 20)
    public String getBrokerSn() {
        return brokerSn;
    }

    public void setBrokerSn(String brokerSn) {
        this.brokerSn = brokerSn;
    }

    private String comm;

    @Basic
    @Column(name = "COMM", length = 20)
    public String getComm() {
        return comm;
    }

    public void setComm(String comm) {
        this.comm = comm;
    }

    private String buySellInd;

    @Basic
    @Column(name = "BUY_SELL_IND", length = 1)
    public String getBuySellInd() {
        return buySellInd;
    }

    public void setBuySellInd(String buySellInd) {
        this.buySellInd = buySellInd;
    }

    private String refSn;

    @Basic
    @Column(name = "REF_SN", length = 20)
    public String getRefSn() {
        return refSn;
    }

    public void setRefSn(String refSn) {
        this.refSn = refSn;
    }

    private String payPrice;

    @Basic
    @Column(name = "PAY_PRICE", length = 60)
    public String getPayPrice() {
        return payPrice;
    }

    public void setPayPrice(String payPrice) {
        this.payPrice = payPrice;
    }

    private String recPrice;

    @Basic
    @Column(name = "REC_PRICE", length = 60)
    public String getRecPrice() {
        return recPrice;
    }

    public void setRecPrice(String recPrice) {
        this.recPrice = recPrice;
    }

    private String seCptySn;

    @Basic
    @Column(name = "SE_CPTY_SN", length = 10)
    public String getSeCptySn() {
        return seCptySn;
    }

    public void setSeCptySn(String seCptySn) {
        this.seCptySn = seCptySn;
    }

    private String tradeStatCode;

    @Basic
    @Column(name = "TRADE_STAT_CODE", length = 10)
    public String getTradeStatCode() {
        return tradeStatCode;
    }

    public void setTradeStatCode(String tradeStatCode) {
        this.tradeStatCode = tradeStatCode;
    }

    private String cdtyGrpCode;

    @Basic
    @Column(name = "CDTY_GRP_CODE", length = 5)
    public String getCdtyGrpCode() {
        return cdtyGrpCode;
    }

    public void setCdtyGrpCode(String cdtyGrpCode) {
        this.cdtyGrpCode = cdtyGrpCode;
    }

    private String brokerPrice;

    @Basic
    @Column(name = "BROKER_PRICE", length = 60)
    public String getBrokerPrice() {
        return brokerPrice;
    }

    public void setBrokerPrice(String brokerPrice) {
        this.brokerPrice = brokerPrice;
    }

    private String optnStrikePrice;

    @Basic
    @Column(name = "OPTN_STRIKE_PRICE", length = 60)
    public String getOptnStrikePrice() {
        return optnStrikePrice;
    }

    public void setOptnStrikePrice(String optnStrikePrice) {
        this.optnStrikePrice = optnStrikePrice;
    }

    private String optnPremPrice;

    @Basic
    @Column(name = "OPTN_PREM_PRICE", length = 60)
    public String getOptnPremPrice() {
        return optnPremPrice;
    }

    public void setOptnPremPrice(String optnPremPrice) {
        this.optnPremPrice = optnPremPrice;
    }

    private String optnPutCallInd;

    @Basic
    @Column(name = "OPTN_PUT_CALL_IND", length = 1)
    public String getOptnPutCallInd() {
        return optnPutCallInd;
    }

    public void setOptnPutCallInd(String optnPutCallInd) {
        this.optnPutCallInd = optnPutCallInd;
    }

    private String efsFlag;

    @Basic
    @Column(name = "EFS_FLAG", length = 1)
    public String getEfsFlag() {
        return efsFlag;
    }

    public void setEfsFlag(String efsFlag) {
        this.efsFlag = efsFlag;
    }

    private String efsCptySn;

    @Basic
    @Column(name = "EFS_CPTY_SN", length = 20)
    public String getEfsCptySn() {
        return efsCptySn;
    }

    public void setEfsCptySn(String efsCptySn) {
        this.efsCptySn = efsCptySn;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        TradeDataEntity that = (TradeDataEntity) o;

        if (id != that.id) return false;
        if (tradeId != that.tradeId) return false;
        if (book != null ? !book.equals(that.book) : that.book != null) return false;
        if (brokerPrice != null ? !brokerPrice.equals(that.brokerPrice) : that.brokerPrice != null) return false;
        if (brokerSn != null ? !brokerSn.equals(that.brokerSn) : that.brokerSn != null) return false;
        if (buySellInd != null ? !buySellInd.equals(that.buySellInd) : that.buySellInd != null) return false;
        if (cdtyCode != null ? !cdtyCode.equals(that.cdtyCode) : that.cdtyCode != null) return false;
        if (cdtyGrpCode != null ? !cdtyGrpCode.equals(that.cdtyGrpCode) : that.cdtyGrpCode != null) return false;
        if (comm != null ? !comm.equals(that.comm) : that.comm != null) return false;
        if (cptySn != null ? !cptySn.equals(that.cptySn) : that.cptySn != null) return false;
        if (efsCptySn != null ? !efsCptySn.equals(that.efsCptySn) : that.efsCptySn != null) return false;
        if (efsFlag != null ? !efsFlag.equals(that.efsFlag) : that.efsFlag != null) return false;
        if (endDt != null ? !endDt.equals(that.endDt) : that.endDt != null) return false;
        if (inceptionDt != null ? !inceptionDt.equals(that.inceptionDt) : that.inceptionDt != null) return false;
        if (locationSn != null ? !locationSn.equals(that.locationSn) : that.locationSn != null) return false;
        if (optnPremPrice != null ? !optnPremPrice.equals(that.optnPremPrice) : that.optnPremPrice != null)
            return false;
        if (optnPutCallInd != null ? !optnPutCallInd.equals(that.optnPutCallInd) : that.optnPutCallInd != null)
            return false;
        if (optnStrikePrice != null ? !optnStrikePrice.equals(that.optnStrikePrice) : that.optnStrikePrice != null)
            return false;
        if (payPrice != null ? !payPrice.equals(that.payPrice) : that.payPrice != null) return false;
        if (priceDesc != null ? !priceDesc.equals(that.priceDesc) : that.priceDesc != null) return false;
        if (qty != null ? !qty.equals(that.qty) : that.qty != null) return false;
        if (qtyTot != null ? !qtyTot.equals(that.qtyTot) : that.qtyTot != null) return false;
        if (recPrice != null ? !recPrice.equals(that.recPrice) : that.recPrice != null) return false;
        if (refSn != null ? !refSn.equals(that.refSn) : that.refSn != null) return false;
        if (seCptySn != null ? !seCptySn.equals(that.seCptySn) : that.seCptySn != null) return false;
        if (startDt != null ? !startDt.equals(that.startDt) : that.startDt != null) return false;
        if (sttlType != null ? !sttlType.equals(that.sttlType) : that.sttlType != null) return false;
        if (tradeDt != null ? !tradeDt.equals(that.tradeDt) : that.tradeDt != null) return false;
        if (tradeStatCode != null ? !tradeStatCode.equals(that.tradeStatCode) : that.tradeStatCode != null)
            return false;
        if (tradeTypeCode != null ? !tradeTypeCode.equals(that.tradeTypeCode) : that.tradeTypeCode != null)
            return false;
        if (uomDurCode != null ? !uomDurCode.equals(that.uomDurCode) : that.uomDurCode != null) return false;
        if (xref != null ? !xref.equals(that.xref) : that.xref != null) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = id.intValue();
        result = 31 * result + tradeId.intValue();
        result = 31 * result + (inceptionDt != null ? inceptionDt.hashCode() : 0);
        result = 31 * result + (cdtyCode != null ? cdtyCode.hashCode() : 0);
        result = 31 * result + (tradeDt != null ? tradeDt.hashCode() : 0);
        result = 31 * result + (xref != null ? xref.hashCode() : 0);
        result = 31 * result + (cptySn != null ? cptySn.hashCode() : 0);
        result = 31 * result + (qtyTot != null ? qtyTot.hashCode() : 0);
        result = 31 * result + (qty != null ? qty.hashCode() : 0);
        result = 31 * result + (uomDurCode != null ? uomDurCode.hashCode() : 0);
        result = 31 * result + (locationSn != null ? locationSn.hashCode() : 0);
        result = 31 * result + (priceDesc != null ? priceDesc.hashCode() : 0);
        result = 31 * result + (startDt != null ? startDt.hashCode() : 0);
        result = 31 * result + (endDt != null ? endDt.hashCode() : 0);
        result = 31 * result + (book != null ? book.hashCode() : 0);
        result = 31 * result + (tradeTypeCode != null ? tradeTypeCode.hashCode() : 0);
        result = 31 * result + (sttlType != null ? sttlType.hashCode() : 0);
        result = 31 * result + (brokerSn != null ? brokerSn.hashCode() : 0);
        result = 31 * result + (comm != null ? comm.hashCode() : 0);
        result = 31 * result + (buySellInd != null ? buySellInd.hashCode() : 0);
        result = 31 * result + (refSn != null ? refSn.hashCode() : 0);
        result = 31 * result + (payPrice != null ? payPrice.hashCode() : 0);
        result = 31 * result + (recPrice != null ? recPrice.hashCode() : 0);
        result = 31 * result + (seCptySn != null ? seCptySn.hashCode() : 0);
        result = 31 * result + (tradeStatCode != null ? tradeStatCode.hashCode() : 0);
        result = 31 * result + (cdtyGrpCode != null ? cdtyGrpCode.hashCode() : 0);
        result = 31 * result + (brokerPrice != null ? brokerPrice.hashCode() : 0);
        result = 31 * result + (optnStrikePrice != null ? optnStrikePrice.hashCode() : 0);
        result = 31 * result + (optnPremPrice != null ? optnPremPrice.hashCode() : 0);
        result = 31 * result + (optnPutCallInd != null ? optnPutCallInd.hashCode() : 0);
        result = 31 * result + (efsFlag != null ? efsFlag.hashCode() : 0);
        result = 31 * result + (efsCptySn != null ? efsCptySn.hashCode() : 0);
        return result;
    }
}
