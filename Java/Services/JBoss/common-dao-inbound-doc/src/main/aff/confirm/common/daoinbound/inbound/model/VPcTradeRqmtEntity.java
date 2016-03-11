package aff.confirm.common.daoinbound.inbound.model;

import javax.persistence.*;
import java.io.Serializable;
import java.sql.Date;
import java.sql.Timestamp;

/**
 * User: mthoresen
 * Date: Sep 24, 2012
 * Time: 7:38:45 AM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "V_PC_TRADE_RQMT")
public class VPcTradeRqmtEntity implements Serializable {
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

    private String trdSysTicket;

    @Basic
    @Column(name = "TRD_SYS_TICKET", length = 50)
    public String getTrdSysTicket() {
        return trdSysTicket;
    }

    public void setTrdSysTicket(String trdSysTicket) {
        this.trdSysTicket = trdSysTicket;
    }

    private String trdSysCode;

    @Basic
    @Column(name = "TRD_SYS_CODE", nullable = false, length = 5)
    public String getTrdSysCode() {
        return trdSysCode;
    }

    public void setTrdSysCode(String trdSysCode) {
        this.trdSysCode = trdSysCode;
    }

    private Long rqmtTradeNotifyId;

    @Basic
    @Column(name = "RQMT_TRADE_NOTIFY_ID", length = 12)
    public Long getRqmtTradeNotifyId() {
        return rqmtTradeNotifyId;
    }

    public void setRqmtTradeNotifyId(Long rqmtTradeNotifyId) {
        this.rqmtTradeNotifyId = rqmtTradeNotifyId;
    }

    private String rqmt;

    @Basic
    @Column(name = "RQMT", nullable = false, length = 10)
    public String getRqmt() {
        return rqmt;
    }

    public void setRqmt(String rqmt) {
        this.rqmt = rqmt;
    }

    private String status;

    @Basic
    @Column(name = "STATUS", nullable = false, length = 10)
    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    private Date completedDt;

    @Basic
    @Column(name = "COMPLETED_DT", length = 7)
    public Date getCompletedDt() {
        return completedDt;
    }

    public void setCompletedDt(Date completedDt) {
        this.completedDt = completedDt;
    }

    private Timestamp completedTimestampGmt;

    @Basic
    @Column(name = "COMPLETED_TIMESTAMP_GMT", length = 7)
    public Timestamp getCompletedTimestampGmt() {
        return completedTimestampGmt;
    }

    public void setCompletedTimestampGmt(Timestamp completedTimestampGmt) {
        this.completedTimestampGmt = completedTimestampGmt;
    }

    private String reference;

    @Basic
    @Column(name = "REFERENCE", length = 30)
    public String getReference() {
        return reference;
    }

    public void setReference(String reference) {
        this.reference = reference;
    }

    private Long cancelTradeNotifyId;

    @Basic
    @Column(name = "CANCEL_TRADE_NOTIFY_ID", length = 12)
    public Long getCancelTradeNotifyId() {
        return cancelTradeNotifyId;
    }

    public void setCancelTradeNotifyId(Long cancelTradeNotifyId) {
        this.cancelTradeNotifyId = cancelTradeNotifyId;
    }

    private String cmt;

    @Basic
    @Column(name = "CMT", length = 500)
    public String getCmt() {
        return cmt;
    }

    public void setCmt(String cmt) {
        this.cmt = cmt;
    }

    private String secondCheckFlag;

    @Basic
    @Column(name = "SECOND_CHECK_FLAG", length = 1)
    public String getSecondCheckFlag() {
        return secondCheckFlag;
    }

    public void setSecondCheckFlag(String secondCheckFlag) {
        this.secondCheckFlag = secondCheckFlag;
    }

    private Long transactionSeq;

    @Basic
    @Column(name = "TRANSACTION_SEQ", length = 12)
    public Long getTransactionSeq() {
        return transactionSeq;
    }

    public void setTransactionSeq(Long transactionSeq) {
        this.transactionSeq = transactionSeq;
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

    private String displayText;

    @Basic
    @Column(name = "DISPLAY_TEXT", nullable = false, length = 20)
    public String getDisplayText() {
        return displayText;
    }

    public void setDisplayText(String displayText) {
        this.displayText = displayText;
    }

    private String category;

    @Basic
    @Column(name = "CATEGORY", length = 10)
    public String getCategory() {
        return category;
    }

    public void setCategory(String category) {
        this.category = category;
    }

    private String terminalFlag;

    @Basic
    @Column(name = "TERMINAL_FLAG", nullable = false, length = 1)
    public String getTerminalFlag() {
        return terminalFlag;
    }

    public void setTerminalFlag(String terminalFlag) {
        this.terminalFlag = terminalFlag;
    }

    private String problemFlag;

    @Basic
    @Column(name = "PROBLEM_FLAG", nullable = false, length = 1)
    public String getProblemFlag() {
        return problemFlag;
    }

    public void setProblemFlag(String problemFlag) {
        this.problemFlag = problemFlag;
    }

    private String guiColorCode;

    @Basic
    @Column(name = "GUI_COLOR_CODE", length = 10)
    public String getGuiColorCode() {
        return guiColorCode;
    }

    public void setGuiColorCode(String guiColorCode) {
        this.guiColorCode = guiColorCode;
    }

    private String delphiConstant;

    @Basic
    @Column(name = "DELPHI_CONSTANT", length = 10)
    public String getDelphiConstant() {
        return delphiConstant;
    }

    public void setDelphiConstant(String delphiConstant) {
        this.delphiConstant = delphiConstant;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        VPcTradeRqmtEntity that = (VPcTradeRqmtEntity) o;

        if (cancelTradeNotifyId != that.cancelTradeNotifyId) return false;
        if (id != that.id) return false;
        if (rqmtTradeNotifyId != that.rqmtTradeNotifyId) return false;
        if (tradeId != that.tradeId) return false;
        if (trdSysTicket != that.trdSysTicket) return  false;
        if (trdSysCode != that.trdSysCode) return  false;
        if (transactionSeq != that.transactionSeq) return false;
        if (category != null ? !category.equals(that.category) : that.category != null) return false;
        if (cmt != null ? !cmt.equals(that.cmt) : that.cmt != null) return false;
        if (completedDt != null ? !completedDt.equals(that.completedDt) : that.completedDt != null) return false;
        if (completedTimestampGmt != null ? !completedTimestampGmt.equals(that.completedTimestampGmt) : that.completedTimestampGmt != null)
            return false;
        if (delphiConstant != null ? !delphiConstant.equals(that.delphiConstant) : that.delphiConstant != null)
            return false;
        if (displayText != null ? !displayText.equals(that.displayText) : that.displayText != null) return false;
        if (finalApprovalFlag != null ? !finalApprovalFlag.equals(that.finalApprovalFlag) : that.finalApprovalFlag != null)
            return false;
        if (guiColorCode != null ? !guiColorCode.equals(that.guiColorCode) : that.guiColorCode != null) return false;
        if (problemFlag != null ? !problemFlag.equals(that.problemFlag) : that.problemFlag != null) return false;
        if (reference != null ? !reference.equals(that.reference) : that.reference != null) return false;
        if (rqmt != null ? !rqmt.equals(that.rqmt) : that.rqmt != null) return false;
        if (secondCheckFlag != null ? !secondCheckFlag.equals(that.secondCheckFlag) : that.secondCheckFlag != null)
            return false;
        if (status != null ? !status.equals(that.status) : that.status != null) return false;
        if (terminalFlag != null ? !terminalFlag.equals(that.terminalFlag) : that.terminalFlag != null) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = (int) (id ^ (id >>> 32));
        result = 31 * result + (int) (tradeId ^ (tradeId >>> 32));
        result = 31 * result + (int) (rqmtTradeNotifyId ^ (rqmtTradeNotifyId >>> 32));
        result = 31 * result + (trdSysTicket != null ? trdSysTicket.hashCode():0);
        result = 31 * result + (trdSysCode != null ? trdSysCode.hashCode():0);
        result = 31 * result + (rqmt != null ? rqmt.hashCode() : 0);
        result = 31 * result + (status != null ? status.hashCode() : 0);
        result = 31 * result + (completedDt != null ? completedDt.hashCode() : 0);
        result = 31 * result + (completedTimestampGmt != null ? completedTimestampGmt.hashCode() : 0);
        result = 31 * result + (reference != null ? reference.hashCode() : 0);
        result = 31 * result + (int) (cancelTradeNotifyId ^ (cancelTradeNotifyId >>> 32));
        result = 31 * result + (cmt != null ? cmt.hashCode() : 0);
        result = 31 * result + (secondCheckFlag != null ? secondCheckFlag.hashCode() : 0);
        result = 31 * result + (int) (transactionSeq ^ (transactionSeq >>> 32));
        result = 31 * result + (finalApprovalFlag != null ? finalApprovalFlag.hashCode() : 0);
        result = 31 * result + (displayText != null ? displayText.hashCode() : 0);
        result = 31 * result + (category != null ? category.hashCode() : 0);
        result = 31 * result + (terminalFlag != null ? terminalFlag.hashCode() : 0);
        result = 31 * result + (problemFlag != null ? problemFlag.hashCode() : 0);
        result = 31 * result + (guiColorCode != null ? guiColorCode.hashCode() : 0);
        result = 31 * result + (delphiConstant != null ? delphiConstant.hashCode() : 0);
        return result;
    }
}
