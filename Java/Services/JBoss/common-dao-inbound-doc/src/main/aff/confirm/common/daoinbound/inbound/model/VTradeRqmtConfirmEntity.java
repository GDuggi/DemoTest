package aff.confirm.common.daoinbound.inbound.model;

import javax.persistence.*;
import java.io.Serializable;
import java.sql.Timestamp;

/**
 * User: mthoresen
 * Date: Sep 25, 2012
 * Time: 2:55:51 PM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "V_TRADE_RQMT_CONFIRM")
public class VTradeRqmtConfirmEntity implements Serializable {
    private Long id;
    @Id
    @Column(name = "ID", nullable = false, length = 12)
    public Long getId() {
        return id;
    }
    public void setId(Long id) {
        this.id = id;
    }

    private Long rqmtId;
    @Basic
    @Column(name = "RQMT_ID", nullable = false, length = 0, precision = -127)
    public Long getRqmtId() {
        return rqmtId;
    }
    public void setRqmtId(Long rqmtId) {
        this.rqmtId = rqmtId;
    }

    private Long tradeId;
    @Basic
    @Column(name = "TRADE_ID", nullable = false, length = 0, precision = -127)
    public Long getTradeId() {
        return tradeId;
    }
    public void setTradeId(Long tradeId) {
        this.tradeId = tradeId;
    }

    private String nextStatusCode;
    @Basic
    @Column(name = "NEXT_STATUS_CODE", length = 10)
    public String getNextStatusCode() {
        return nextStatusCode;
    }
    public void setNextStatusCode(String nextStatusCode) {
        this.nextStatusCode = nextStatusCode;
    }

    private String confirmLabel;
    @Basic
    @Column(name = "CONFIRM_LABEL", length = 30)
    public String getConfirmLabel() {
        return confirmLabel;
    }
    public void setConfirmLabel(String confirmLabel) {
        this.confirmLabel = confirmLabel;
    }

    private String confirmCmt;
    @Basic
    @Column(name = "CONFIRM_CMT")
    public String getConfirmCmt() {
        return confirmCmt;
    }
    public void setConfirmCmt(String confirmCmt) {
        this.confirmCmt = confirmCmt;
    }

    private String faxTelexInd;
    @Basic
    @Column(name = "FAX_TELEX_IND", length = 1)
    public String getFaxTelexInd() {
        return faxTelexInd;
    }
    public void setFaxTelexInd(String faxTelexInd) {
        this.faxTelexInd = faxTelexInd;
    }

    private String faxTelexNumber;
    @Basic
    @Column(name = "FAX_TELEX_NUMBER")
    public String getFaxTelexNumber() {
        return faxTelexNumber;
    }
    public void setFaxTelexNumber(String faxTelexNumber) {
        this.faxTelexNumber = faxTelexNumber;
    }

    private String xmitStatusInd;
    @Basic
    @Column(name = "XMIT_STATUS_IND", length = 100)
    public String getXmitStatusInd() {
        return xmitStatusInd;
    }
    public void setXmitStatusInd(String xmitStatusInd) {
        this.xmitStatusInd = xmitStatusInd;
    }

    private String xmitAddr;
    @Basic
    @Column(name = "XMIT_ADDR", length = 1000)
    public String getXmitAddr() {
        return xmitAddr;
    }
    public void setXmitAddr(String xmitAddr) {
        this.xmitAddr = xmitAddr;
    }

    private String xmitCmt;
    @Basic
    @Column(name = "XMIT_CMT")
    public String getXmitCmt() {
        return xmitCmt;
    }
    public void setXmitCmt(String xmitCmt) {
        this.xmitCmt = xmitCmt;
    }

    private Timestamp xmitTimestampGmt;
    @Basic
    @Column(name = "XMIT_TIMESTAMP_GMT", length = 7)
    public Timestamp getXmitTimestampGmt() {
        return xmitTimestampGmt;
    }
    public void setXmitTimestampGmt(Timestamp xmitTimestampGmt) {
        this.xmitTimestampGmt = xmitTimestampGmt;
    }

    private String templateName;
    @Basic
    @Column(name = "TEMPLATE_NAME", length = 60)
    public String getTemplateName() {
        return templateName;
    }
    public void setTemplateName(String templateName) {
        this.templateName = templateName;
    }

    private String finalApprovalFlag;
    @Basic
    @Column(name = "FINAL_APPROVAL_FLAG", nullable = false, length = 1)
    public String getFinalApprovalFlag() {
        return finalApprovalFlag;
    }
    public void setFinalApprovalFlag(String finalApprovalFlag) {
        this.finalApprovalFlag = finalApprovalFlag;
    }

    private String activeFlag;
    @Basic
    @Column(name = "ACTIVE_FLAG", nullable = false, length = 1)
    public String getActiveFlag() {
        return activeFlag;
    }
    public void setActiveFlag(String activeFlag) {
        this.activeFlag = activeFlag;
    }

    private String preparerCanSendFlag;
    @Basic
    @Column(name = "PREPARER_CAN_SEND_FLAG", nullable = false, length = 1)
    public String getPreparerCanSendFlag() {
        return preparerCanSendFlag;
    }
    public void setPreparerCanSendFlag(String preparerCanSendFlag) {
        this.preparerCanSendFlag = preparerCanSendFlag;
    }


    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        VTradeRqmtConfirmEntity that = (VTradeRqmtConfirmEntity) o;

        if (id != that.id) return false;
        if (rqmtId != that.rqmtId) return false;
        if (tradeId != that.tradeId) return false;
        if (confirmCmt != null ? !confirmCmt.equals(that.confirmCmt) : that.confirmCmt != null) return false;
        if (confirmLabel != null ? !confirmLabel.equals(that.confirmLabel) : that.confirmLabel != null) return false;
        if (faxTelexInd != null ? !faxTelexInd.equals(that.faxTelexInd) : that.faxTelexInd != null) return false;
        if (faxTelexNumber != null ? !faxTelexNumber.equals(that.faxTelexNumber) : that.faxTelexNumber != null)
            return false;
        if (finalApprovalFlag != null ? !finalApprovalFlag.equals(that.finalApprovalFlag) : that.finalApprovalFlag != null)
            return false;
        if (nextStatusCode != null ? !nextStatusCode.equals(that.nextStatusCode) : that.nextStatusCode != null)
            return false;
        if (templateName != null ? !templateName.equals(that.templateName) : that.templateName != null) return false;
        if (xmitAddr != null ? !xmitAddr.equals(that.xmitAddr) : that.xmitAddr != null) return false;
        if (xmitCmt != null ? !xmitCmt.equals(that.xmitCmt) : that.xmitCmt != null) return false;
        if (xmitStatusInd != null ? !xmitStatusInd.equals(that.xmitStatusInd) : that.xmitStatusInd != null)
            return false;
        if (xmitTimestampGmt != null ? !xmitTimestampGmt.equals(that.xmitTimestampGmt) : that.xmitTimestampGmt != null)
            return false;
        if (activeFlag != null ? !activeFlag.equals(that.activeFlag) : that.activeFlag != null)
            return false;
        if (preparerCanSendFlag != null ? !preparerCanSendFlag.equals(that.preparerCanSendFlag) : that.preparerCanSendFlag != null)
            return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = (int) (id ^ (id >>> 32));
        result = 31 * result + (int) (rqmtId ^ (rqmtId >>> 32));
        result = 31 * result + (int) (tradeId ^ (tradeId >>> 32));
        result = 31 * result + (nextStatusCode != null ? nextStatusCode.hashCode() : 0);
        result = 31 * result + (confirmLabel != null ? confirmLabel.hashCode() : 0);
        result = 31 * result + (confirmCmt != null ? confirmCmt.hashCode() : 0);
        result = 31 * result + (faxTelexInd != null ? faxTelexInd.hashCode() : 0);
        result = 31 * result + (faxTelexNumber != null ? faxTelexNumber.hashCode() : 0);
        result = 31 * result + (xmitStatusInd != null ? xmitStatusInd.hashCode() : 0);
        result = 31 * result + (xmitAddr != null ? xmitAddr.hashCode() : 0);
        result = 31 * result + (xmitCmt != null ? xmitCmt.hashCode() : 0);
        result = 31 * result + (xmitTimestampGmt != null ? xmitTimestampGmt.hashCode() : 0);
        result = 31 * result + (templateName != null ? templateName.hashCode() : 0);
        result = 31 * result + (finalApprovalFlag != null ? finalApprovalFlag.hashCode() : 0);
        result = 31 * result + (activeFlag != null ? activeFlag.hashCode() : 0);
        result = 31 * result + (preparerCanSendFlag != null ? preparerCanSendFlag.hashCode() : 0);
        return result;
    }
}
