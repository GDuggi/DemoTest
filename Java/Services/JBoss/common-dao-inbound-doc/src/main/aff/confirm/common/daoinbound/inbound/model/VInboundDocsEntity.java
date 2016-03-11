package aff.confirm.common.daoinbound.inbound.model;

import javax.persistence.*;
import java.io.Serializable;
import java.sql.Timestamp;

/**
 * User: mthoresen
 * Date: Sep 25, 2012
 * Time: 10:06:58 AM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "V_INBOUND_DOCS")
public class VInboundDocsEntity implements Serializable {
    private Long unresolvedcount;

    @Basic
    @Column(name = "UNRESOLVEDCOUNT", length = 0, precision = -127)
    public Long getUnresolvedcount() {
        return unresolvedcount;
    }

    public void setUnresolvedcount(Long unresolvedcount) {
        this.unresolvedcount = unresolvedcount;
    }

    private Long id;

    @Id
    @Column(name = "ID", nullable = false, length = 0, precision = -127)
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    private String mappedCptySn;

    @Basic
    @Column(name = "MAPPED_CPTY_SN", length = 50)
    public String getMappedCptySn() {
        return mappedCptySn;
    }

    public void setMappedCptySn(String mappedCptySn) {
        this.mappedCptySn = mappedCptySn;
    }

    private String callerRef;

    @Basic
    @Column(name = "CALLER_REF", length = 250)
    public String getCallerRef() {
        return callerRef;
    }

    public void setCallerRef(String callerRef) {
        this.callerRef = callerRef;
    }

    private String sentTo;

    @Basic
    @Column(name = "SENT_TO", length = 250)
    public String getSentTo() {
        return sentTo;
    }

    public void setSentTo(String sentTo) {
        this.sentTo = sentTo;
    }

    private Timestamp rcvdTs;

    @Basic
    @Column(name = "RCVD_TS", nullable = false, length = 7)
    public Timestamp getRcvdTs() {
        return rcvdTs;
    }

    public void setRcvdTs(Timestamp rcvdTs) {
        this.rcvdTs = rcvdTs;
    }

    private String fileName;

    @Basic
    @Column(name = "FILE_NAME", nullable = false, length = 80)
    public String getFileName() {
        return fileName;
    }

    public void setFileName(String fileName) {
        this.fileName = fileName;
    }

    private String sender;

    @Basic
    @Column(name = "SENDER", length = 40)
    public String getSender() {
        return sender;
    }

    public void setSender(String sender) {
        this.sender = sender;
    }

    private String cmt;

    @Basic
    @Column(name = "CMT", length = 100)
    public String getCmt() {
        return cmt;
    }

    public void setCmt(String cmt) {
        this.cmt = cmt;
    }

    private String docStatusCode;

    @Basic
    @Column(name = "DOC_STATUS_CODE", length = 25)
    public String getDocStatusCode() {
        return docStatusCode;
    }

    public void setDocStatusCode(String docStatusCode) {
        this.docStatusCode = docStatusCode;
    }

    private String hasAutoAsctedFlag;

    @Basic
    @Column(name = "HAS_AUTO_ASCTED_FLAG", nullable = false, length = 1)
    public String getHasAutoAsctedFlag() {
        return hasAutoAsctedFlag;
    }

    public void setHasAutoAsctedFlag(String hasAutoAsctedFlag) {
        this.hasAutoAsctedFlag = hasAutoAsctedFlag;
    }

    private String procFlag;

    @Basic
    @Column(name = "PROC_FLAG", nullable = false, length = 1)
    public String getProcFlag() {
        return procFlag;
    }

    public void setProcFlag(String procFlag) {
        this.procFlag = procFlag;
    }

    private String tradeids;

    @Basic
    @Column(name = "TRADEIDS", length = 4000)
    public String getTradeids() {
        return tradeids;
    }

    public void setTradeids(String tradeids) {
        this.tradeids = tradeids;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        VInboundDocsEntity that = (VInboundDocsEntity) o;

        if (id != that.id) return false;
        if (unresolvedcount != that.unresolvedcount) return false;
        if (callerRef != null ? !callerRef.equals(that.callerRef) : that.callerRef != null) return false;
        if (cmt != null ? !cmt.equals(that.cmt) : that.cmt != null) return false;
        if (docStatusCode != null ? !docStatusCode.equals(that.docStatusCode) : that.docStatusCode != null)
            return false;
        if (fileName != null ? !fileName.equals(that.fileName) : that.fileName != null) return false;
        if (hasAutoAsctedFlag != null ? !hasAutoAsctedFlag.equals(that.hasAutoAsctedFlag) : that.hasAutoAsctedFlag != null)
            return false;
        if (mappedCptySn != null ? !mappedCptySn.equals(that.mappedCptySn) : that.mappedCptySn != null) return false;
        if (procFlag != null ? !procFlag.equals(that.procFlag) : that.procFlag != null) return false;
        if (rcvdTs != null ? !rcvdTs.equals(that.rcvdTs) : that.rcvdTs != null) return false;
        if (sender != null ? !sender.equals(that.sender) : that.sender != null) return false;
        if (sentTo != null ? !sentTo.equals(that.sentTo) : that.sentTo != null) return false;
        if (tradeids != null ? !tradeids.equals(that.tradeids) : that.tradeids != null) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = (int) (unresolvedcount ^ (unresolvedcount >>> 32));
        result = 31 * result + (int) (id ^ (id >>> 32));
        result = 31 * result + (mappedCptySn != null ? mappedCptySn.hashCode() : 0);
        result = 31 * result + (callerRef != null ? callerRef.hashCode() : 0);
        result = 31 * result + (sentTo != null ? sentTo.hashCode() : 0);
        result = 31 * result + (rcvdTs != null ? rcvdTs.hashCode() : 0);
        result = 31 * result + (fileName != null ? fileName.hashCode() : 0);
        result = 31 * result + (sender != null ? sender.hashCode() : 0);
        result = 31 * result + (cmt != null ? cmt.hashCode() : 0);
        result = 31 * result + (docStatusCode != null ? docStatusCode.hashCode() : 0);
        result = 31 * result + (hasAutoAsctedFlag != null ? hasAutoAsctedFlag.hashCode() : 0);
        result = 31 * result + (procFlag != null ? procFlag.hashCode() : 0);
        result = 31 * result + (tradeids != null ? tradeids.hashCode() : 0);
        return result;
    }
}
