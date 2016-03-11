package aff.confirm.common.daoinbound.inbound.model;

import javax.persistence.*;
import java.io.Serializable;
import java.sql.Timestamp;

/**
 * User: mthoresen
 * Date: Feb 4, 2010
 * Time: 2:28:47 PM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "TRADE_SUMMARY")
public class TradeSummaryEntity implements Serializable {
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

    private String openRqmtsFlag;

    @Basic
    @Column(name = "OPEN_RQMTS_FLAG", nullable = false, length = 1)
    public String getOpenRqmtsFlag() {
        return openRqmtsFlag;
    }

    public void setOpenRqmtsFlag(String openRqmtsFlag) {
        this.openRqmtsFlag = openRqmtsFlag;
    }

    private String category;

    @Basic
    @Column(name = "CATEGORY", nullable = false, length = 10)
    public String getCategory() {
        return category;
    }

    public void setCategory(String category) {
        this.category = category;
    }

    private Timestamp lastUpdateTimestampGmt;

    @Basic
    @Column(name = "LAST_UPDATE_TIMESTAMP_GMT", nullable = false, length = 7)
    public Timestamp getLastUpdateTimestampGmt() {
        return lastUpdateTimestampGmt;
    }

    public void setLastUpdateTimestampGmt(Timestamp lastUpdateTimestampGmt) {
        this.lastUpdateTimestampGmt = lastUpdateTimestampGmt;
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

    private String cmt;

    @Basic
    @Column(name = "CMT")
    public String getCmt() {
        return cmt;
    }

    public void setCmt(String cmt) {
        this.cmt = cmt;
    }

    private Timestamp lastTrdEditTimestampGmt;

    @Basic
    @Column(name = "LAST_TRD_EDIT_TIMESTAMP_GMT", nullable = false, length = 7)
    public Timestamp getLastTrdEditTimestampGmt() {
        return lastTrdEditTimestampGmt;
    }

    public void setLastTrdEditTimestampGmt(Timestamp lastTrdEditTimestampGmt) {
        this.lastTrdEditTimestampGmt = lastTrdEditTimestampGmt;
    }

    private String opsDetActFlag;

    @Basic
    @Column(name = "OPS_DET_ACT_FLAG", length = 1)
    public String getOpsDetActFlag() {
        return opsDetActFlag;
    }

    public void setOpsDetActFlag(String opsDetActFlag) {
        this.opsDetActFlag = opsDetActFlag;
    }

    private String readyForFinalApprovalFlag;

    @Basic
    @Column(name = "READY_FOR_FINAL_APPROVAL_FLAG", length = 1)
    public String getReadyForFinalApprovalFlag() {
        return readyForFinalApprovalFlag;
    }

    public void setReadyForFinalApprovalFlag(String readyForFinalApprovalFlag) {
        this.readyForFinalApprovalFlag = readyForFinalApprovalFlag;
    }

    private String hasProblemFlag;

    @Basic
    @Column(name = "HAS_PROBLEM_FLAG", nullable = false, length = 1)
    public String getHasProblemFlag() {
        return hasProblemFlag;
    }

    public void setHasProblemFlag(String hasProblemFlag) {
        this.hasProblemFlag = hasProblemFlag;
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

    private Timestamp finalApprovalTimestampGmt;

    @Basic
    @Column(name = "FINAL_APPROVAL_TIMESTAMP_GMT", length = 7)
    public Timestamp getFinalApprovalTimestampGmt() {
        return finalApprovalTimestampGmt;
    }

    public void setFinalApprovalTimestampGmt(Timestamp finalApprovalTimestampGmt) {
        this.finalApprovalTimestampGmt = finalApprovalTimestampGmt;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        TradeSummaryEntity that = (TradeSummaryEntity) o;

        if (id != that.id) return false;
        if (tradeId != that.tradeId) return false;
        if (transactionSeq != that.transactionSeq) return false;
        if (category != null ? !category.equals(that.category) : that.category != null) return false;
        if (cmt != null ? !cmt.equals(that.cmt) : that.cmt != null) return false;
        if (finalApprovalFlag != null ? !finalApprovalFlag.equals(that.finalApprovalFlag) : that.finalApprovalFlag != null)
            return false;
        if (finalApprovalTimestampGmt != null ? !finalApprovalTimestampGmt.equals(that.finalApprovalTimestampGmt) : that.finalApprovalTimestampGmt != null)
            return false;
        if (hasProblemFlag != null ? !hasProblemFlag.equals(that.hasProblemFlag) : that.hasProblemFlag != null)
            return false;
        if (lastTrdEditTimestampGmt != null ? !lastTrdEditTimestampGmt.equals(that.lastTrdEditTimestampGmt) : that.lastTrdEditTimestampGmt != null)
            return false;
        if (lastUpdateTimestampGmt != null ? !lastUpdateTimestampGmt.equals(that.lastUpdateTimestampGmt) : that.lastUpdateTimestampGmt != null)
            return false;
        if (openRqmtsFlag != null ? !openRqmtsFlag.equals(that.openRqmtsFlag) : that.openRqmtsFlag != null)
            return false;
        if (opsDetActFlag != null ? !opsDetActFlag.equals(that.opsDetActFlag) : that.opsDetActFlag != null)
            return false;
        if (readyForFinalApprovalFlag != null ? !readyForFinalApprovalFlag.equals(that.readyForFinalApprovalFlag) : that.readyForFinalApprovalFlag != null)
            return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = id.intValue();
        result = 31 * result + tradeId.intValue();
        result = 31 * result + (openRqmtsFlag != null ? openRqmtsFlag.hashCode() : 0);
        result = 31 * result + (category != null ? category.hashCode() : 0);
        result = 31 * result + (lastUpdateTimestampGmt != null ? lastUpdateTimestampGmt.hashCode() : 0);
        result = 31 * result + (finalApprovalFlag != null ? finalApprovalFlag.hashCode() : 0);
        result = 31 * result + (cmt != null ? cmt.hashCode() : 0);
        result = 31 * result + (lastTrdEditTimestampGmt != null ? lastTrdEditTimestampGmt.hashCode() : 0);
        result = 31 * result + (opsDetActFlag != null ? opsDetActFlag.hashCode() : 0);
        result = 31 * result + (readyForFinalApprovalFlag != null ? readyForFinalApprovalFlag.hashCode() : 0);
        result = 31 * result + (hasProblemFlag != null ? hasProblemFlag.hashCode() : 0);
        result = 31 * result + transactionSeq.intValue();
        result = 31 * result + (finalApprovalTimestampGmt != null ? finalApprovalTimestampGmt.hashCode() : 0);
        return result;
    }
}
