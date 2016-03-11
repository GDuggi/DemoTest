package aff.confirm.common.daoinbound.inbound.model;

import javax.persistence.*;
import java.io.Serializable;
import java.sql.Timestamp;

/**
 * User: mthoresen
 * Date: Mar 2, 2010
 * Time: 8:36:22 AM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "ASSOCIATED_DOCS")
public class AssociatedDocsEntity implements Serializable {
    private Long id;

    @Id
    @Column(name = "ID", nullable = false, length = 8)
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    private Long inboundDocsId;

    @Basic
    @Column(name = "INBOUND_DOCS_ID", nullable = false, length = 22)
    public Long getInboundDocsId() {
        return inboundDocsId;
    }

    public void setInboundDocsId(Long inboundDocsId) {
        this.inboundDocsId = inboundDocsId;
    }

    private Long indexVal;

    @Basic
    @Column(name = "INDEX_VAL", nullable = false, length = 22)
    public Long getIndexVal() {
        return indexVal;
    }

    public void setIndexVal(Long indexVal) {
        this.indexVal = indexVal;
    }

    private String fileName;

    @Basic
    @Column(name = "FILE_NAME", length = 80)
    public String getFileName() {
        return fileName;
    }

    public void setFileName(String fileName) {
        this.fileName = fileName;
    }

    private Long tradeId;

    @Basic
    @Column(name = "TRADE_ID", length = 22)
    public Long getTradeId() {
        return tradeId;
    }

    public void setTradeId(Long tradeId) {
        this.tradeId = tradeId;
    }

    private String docStatusCode;

    @Basic
    @Column(name = "DOC_STATUS_CODE", nullable = false, length = 25)
    public String getDocStatusCode() {
        return docStatusCode;
    }

    public void setDocStatusCode(String docStatusCode) {
        this.docStatusCode = docStatusCode;
    }

    private String associatedBy;

    @Basic
    @Column(name = "ASSOCIATED_BY", length = 50)
    public String getAssociatedBy() {
        return associatedBy;
    }

    public void setAssociatedBy(String associatedBy) {
        this.associatedBy = associatedBy;
    }

    private Timestamp associatedDt;

    @Basic
    @Column(name = "ASSOCIATED_DT", length = 7)
    public Timestamp getAssociatedDt() {
        return associatedDt;
    }

    public void setAssociatedDt(Timestamp associatedDt) {
        this.associatedDt = associatedDt;
    }

    private String finalApprovedBy;

    @Basic
    @Column(name = "FINAL_APPROVED_BY", length = 50)
    public String getFinalApprovedBy() {
        return finalApprovedBy;
    }

    public void setFinalApprovedBy(String finalApprovedBy) {
        this.finalApprovedBy = finalApprovedBy;
    }

    private Timestamp finalApprovedDt;

    @Basic
    @Column(name = "FINAL_APPROVED_DT", length = 7)
    public Timestamp getFinalApprovedDt() {
        return finalApprovedDt;
    }

    public void setFinalApprovedDt(Timestamp finalApprovedDt) {
        this.finalApprovedDt = finalApprovedDt;
    }

    private String disputedBy;

    @Basic
    @Column(name = "DISPUTED_BY", length = 50)
    public String getDisputedBy() {
        return disputedBy;
    }

    public void setDisputedBy(String disputedBy) {
        this.disputedBy = disputedBy;
    }

    private Timestamp disputedDt;

    @Basic
    @Column(name = "DISPUTED_DT", length = 7)
    public Timestamp getDisputedDt() {
        return disputedDt;
    }

    public void setDisputedDt(Timestamp disputedDt) {
        this.disputedDt = disputedDt;
    }

    private String discardedBy;

    @Basic
    @Column(name = "DISCARDED_BY", length = 50)
    public String getDiscardedBy() {
        return discardedBy;
    }

    public void setDiscardedBy(String discardedBy) {
        this.discardedBy = discardedBy;
    }

    private Timestamp discardedDt;

    @Basic
    @Column(name = "DISCARDED_DT", length = 7)
    public Timestamp getDiscardedDt() {
        return discardedDt;
    }

    public void setDiscardedDt(Timestamp discardedDt) {
        this.discardedDt = discardedDt;
    }

    private String vaultedBy;

    @Basic
    @Column(name = "VAULTED_BY", length = 50)
    public String getVaultedBy() {
        return vaultedBy;
    }

    public void setVaultedBy(String vaultedBy) {
        this.vaultedBy = vaultedBy;
    }

    private Timestamp vaultedDt;

    @Basic
    @Column(name = "VAULTED_DT", length = 7)
    public Timestamp getVaultedDt() {
        return vaultedDt;
    }

    public void setVaultedDt(Timestamp vaultedDt) {
        this.vaultedDt = vaultedDt;
    }

    private String cdtyGroupCode;

    @Basic
    @Column(name = "CDTY_GROUP_CODE", length = 20)
    public String getCdtyGroupCode() {
        return cdtyGroupCode;
    }

    public void setCdtyGroupCode(String cdtyGroupCode) {
        this.cdtyGroupCode = cdtyGroupCode;
    }

    private String cptySn;

    @Basic
    @Column(name = "CPTY_SN", length = 50)
    public String getCptySn() {
        return cptySn;
    }

    public void setCptySn(String cptySn) {
        this.cptySn = cptySn;
    }

    private String brokerSn;

    @Basic
    @Column(name = "BROKER_SN", length = 50)
    public String getBrokerSn() {
        return brokerSn;
    }

    public void setBrokerSn(String brokerSn) {
        this.brokerSn = brokerSn;
    }

    private String docTypeCode;

    @Basic
    @Column(name = "DOC_TYPE_CODE", length = 20)
    public String getDocTypeCode() {
        return docTypeCode;
    }

    public void setDocTypeCode(String docTypeCode) {
        this.docTypeCode = docTypeCode;
    }

    private String secValidateReqFlag;

    @Basic
    @Column(name = "SEC_VALIDATE_REQ_FLAG", nullable = false, length = 1)
    public String getSecValidateReqFlag() {
        return secValidateReqFlag;
    }

    public void setSecValidateReqFlag(String secValidateReqFlag) {
        this.secValidateReqFlag = secValidateReqFlag;
    }

    private Long tradeRqmtId;

    @Basic
    @Column(name = "TRADE_RQMT_ID", length = 12)
    public Long getTradeRqmtId() {
        return tradeRqmtId;
    }

    public void setTradeRqmtId(Long tradeRqmtId) {
        this.tradeRqmtId = tradeRqmtId;
    }

    private String xmitStatusCode;

    @Basic
    @Column(name = "XMIT_STATUS_CODE", length = 10)
    public String getXmitStatusCode() {
        return xmitStatusCode;
    }

    public void setXmitStatusCode(String xmitStatusCode) {
        this.xmitStatusCode = xmitStatusCode;
    }

    private String xmitValue;

    @Basic
    @Column(name = "XMIT_VALUE", length = 250)
    public String getXmitValue() {
        return xmitValue;
    }

    public void setXmitValue(String xmitValue) {
        this.xmitValue = xmitValue;
    }

    private String vaultedFlag;

    @Basic
    @Column(name = "VAULTED_FLAG", length = 1)
    public String getVaultedFlag() {
        return vaultedFlag;
    }

    public void setVaultedFlag(String vaultedFlag) {
        this.vaultedFlag = vaultedFlag;
    }

    private InboundDocsEntity inboundDocs;

    @ManyToOne
    @JoinColumn(name = "INBOUND_DOCS_ID", referencedColumnName = "ID", insertable = false, updatable = false)
    public InboundDocsEntity getInboundDocs() {
        return inboundDocs;
    }

    public void setInboundDocs(InboundDocsEntity inboundDocs) {
        this.inboundDocs = inboundDocs;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        AssociatedDocsEntity that = (AssociatedDocsEntity) o;

        if (id != that.id) return false;
        if (inboundDocsId != that.inboundDocsId) return false;
        if (indexVal != that.indexVal) return false;
        if (tradeId != that.tradeId) return false;
        if (tradeRqmtId != that.tradeRqmtId) return false;
        if (associatedBy != null ? !associatedBy.equals(that.associatedBy) : that.associatedBy != null) return false;
        if (associatedDt != null ? !associatedDt.equals(that.associatedDt) : that.associatedDt != null) return false;
        if (brokerSn != null ? !brokerSn.equals(that.brokerSn) : that.brokerSn != null) return false;
        if (cdtyGroupCode != null ? !cdtyGroupCode.equals(that.cdtyGroupCode) : that.cdtyGroupCode != null)
            return false;
        if (cptySn != null ? !cptySn.equals(that.cptySn) : that.cptySn != null) return false;
        if (discardedBy != null ? !discardedBy.equals(that.discardedBy) : that.discardedBy != null) return false;
        if (discardedDt != null ? !discardedDt.equals(that.discardedDt) : that.discardedDt != null) return false;
        if (disputedBy != null ? !disputedBy.equals(that.disputedBy) : that.disputedBy != null) return false;
        if (disputedDt != null ? !disputedDt.equals(that.disputedDt) : that.disputedDt != null) return false;
        if (docStatusCode != null ? !docStatusCode.equals(that.docStatusCode) : that.docStatusCode != null)
            return false;
        if (docTypeCode != null ? !docTypeCode.equals(that.docTypeCode) : that.docTypeCode != null) return false;
        if (fileName != null ? !fileName.equals(that.fileName) : that.fileName != null) return false;
        if (finalApprovedBy != null ? !finalApprovedBy.equals(that.finalApprovedBy) : that.finalApprovedBy != null)
            return false;
        if (finalApprovedDt != null ? !finalApprovedDt.equals(that.finalApprovedDt) : that.finalApprovedDt != null)
            return false;
        if (secValidateReqFlag != null ? !secValidateReqFlag.equals(that.secValidateReqFlag) : that.secValidateReqFlag != null)
            return false;
        if (vaultedBy != null ? !vaultedBy.equals(that.vaultedBy) : that.vaultedBy != null) return false;
        if (vaultedDt != null ? !vaultedDt.equals(that.vaultedDt) : that.vaultedDt != null) return false;
        if (vaultedFlag != null ? !vaultedFlag.equals(that.vaultedFlag) : that.vaultedFlag != null) return false;
        if (xmitStatusCode != null ? !xmitStatusCode.equals(that.xmitStatusCode) : that.xmitStatusCode != null)
            return false;
        if (xmitValue != null ? !xmitValue.equals(that.xmitValue) : that.xmitValue != null) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = id.intValue();
        result = 31 * result + inboundDocsId.intValue();
        result = 31 * result + indexVal.intValue();
        result = 31 * result + (fileName != null ? fileName.hashCode() : 0);
        result = 31 * result + tradeId.intValue();
        result = 31 * result + (docStatusCode != null ? docStatusCode.hashCode() : 0);
        result = 31 * result + (associatedBy != null ? associatedBy.hashCode() : 0);
        result = 31 * result + (associatedDt != null ? associatedDt.hashCode() : 0);
        result = 31 * result + (finalApprovedBy != null ? finalApprovedBy.hashCode() : 0);
        result = 31 * result + (finalApprovedDt != null ? finalApprovedDt.hashCode() : 0);
        result = 31 * result + (disputedBy != null ? disputedBy.hashCode() : 0);
        result = 31 * result + (disputedDt != null ? disputedDt.hashCode() : 0);
        result = 31 * result + (discardedBy != null ? discardedBy.hashCode() : 0);
        result = 31 * result + (discardedDt != null ? discardedDt.hashCode() : 0);
        result = 31 * result + (vaultedBy != null ? vaultedBy.hashCode() : 0);
        result = 31 * result + (vaultedDt != null ? vaultedDt.hashCode() : 0);
        result = 31 * result + (cdtyGroupCode != null ? cdtyGroupCode.hashCode() : 0);
        result = 31 * result + (cptySn != null ? cptySn.hashCode() : 0);
        result = 31 * result + (brokerSn != null ? brokerSn.hashCode() : 0);
        result = 31 * result + (docTypeCode != null ? docTypeCode.hashCode() : 0);
        result = 31 * result + (secValidateReqFlag != null ? secValidateReqFlag.hashCode() : 0);
        result = 31 * result + tradeRqmtId.intValue();
        result = 31 * result + (xmitStatusCode != null ? xmitStatusCode.hashCode() : 0);
        result = 31 * result + (xmitValue != null ? xmitValue.hashCode() : 0);
        result = 31 * result + (vaultedFlag != null ? vaultedFlag.hashCode() : 0);
        return result;
    }
}
