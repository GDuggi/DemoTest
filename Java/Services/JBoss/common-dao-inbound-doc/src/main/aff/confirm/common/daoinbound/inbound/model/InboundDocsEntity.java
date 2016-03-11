package aff.confirm.common.daoinbound.inbound.model;

/**
 * User: mthoresen
 * Date: Jun 22, 2009
 * Time: 1:26:09 PM
 */

import javax.persistence.*;
import java.io.Serializable;
import java.util.Date;
import java.util.Set;

/**
 * User: InboundDocsEntity
 * Date: Mar 17, 2008
 * Time: 2:45:45 PM
 */
@Entity
@Table(schema = "ConfirmMgr", name = "INBOUND_DOCS")
@SequenceGenerator(name="SEQ_INBOUND_DOCS", sequenceName="OPS_TRACKING.SEQ_INBOUND_DOCS", allocationSize = 1)
public class InboundDocsEntity implements Serializable {
    private Long id;
    private String callerRef;
    private String sentTo;
    private Date rcvdTs;
    private String fileName;
    private String sender;
    private String cmt;
    private String docStatusCode = DocStatusCode.INBDOC_OPEN;
    private String hasAutoAsctedFlag;
    private String mappedCptySn;
    private String jobRef;
    private String procFlag = "Y"; //Reset to N default when OCR and Indexing turned on in Ops Manager.
    private String mappedCdtyCode;
    private String mappedBrkrSn;
    private Set<AssociatedDocsEntity> associatedDocs;
    private InboundFaxNosEntity inboundFaxNos;

    @Id
    @Column(name = "ID", nullable = false, length = 22)
    @GeneratedValue(strategy=GenerationType.SEQUENCE,  generator="SEQ_INBOUND_DOCS")
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    @Basic
    @Column(name = "CALLER_REF", length = 250)
    public String getCallerRef() {
        return callerRef;
    }

    public void setCallerRef(String callerRef) {
        this.callerRef = callerRef;
    }

    @Basic
    @Column(name = "SENT_TO",length = 250)
    public String getSentTo() {
        return sentTo;
    }

    public void setSentTo(String sentTo) {
        this.sentTo = sentTo;
    }

    @Basic
    @Column(name = "RCVD_TS", nullable = false, length = 7)
    public Date getRcvdTs() {
        return rcvdTs;
    }

    public void setRcvdTs(Date rcvdTs) {
        this.rcvdTs = rcvdTs;
    }

    @Basic
    @Column(name = "FILE_NAME", nullable = false, length = 80)
    public String getFileName() {
        return fileName;
    }

    public void setFileName(String fileName) {
        this.fileName = fileName;
    }

    @Basic
    @Column(name = "SENDER", length = 40)
    public String getSender() {
        return sender;
    }

    public void setSender(String sender) {
        this.sender = sender;
    }

    @Basic
    @Column(name = "CMT", length = 100)
    public String getCmt() {
        return cmt;
    }

    public void setCmt(String cmt) {
        this.cmt = cmt;
    }

    @Basic
    @Column(name = "DOC_STATUS_CODE", length = 25)
    public String getDocStatusCode() {
        return docStatusCode;
    }

    public void setDocStatusCode(String docStatusCode) {
        this.docStatusCode = docStatusCode;
    }

    @Basic
    @Column(name = "HAS_AUTO_ASCTED_FLAG", nullable = false, length = 1)
    public String getHasAutoAsctedFlag() {
        return hasAutoAsctedFlag;
    }

    public void setHasAutoAsctedFlag(String hasAutoAsctedFlag) {
        this.hasAutoAsctedFlag = hasAutoAsctedFlag;
    }

    @Basic
    @Column(name = "MAPPED_CPTY_SN", length = 50)
    public String getMappedCptySn() {
        return mappedCptySn;
    }

    public void setMappedCptySn(String mappedCptySn) {
        this.mappedCptySn = mappedCptySn;
    }

    @Basic
    @Column(name = "JOB_REF", length = 50)
    public String getJobRef() {
        return jobRef;
    }

    public void setJobRef(String jobRef) {
        this.jobRef = jobRef;
    }

    @Basic
    @Column(name = "PROC_FLAG", length = 50)
    public String getProcFlag() {
        return procFlag;
    }

    public void setProcFlag(String procFlag) {
        this.procFlag = procFlag;
    }

    @Basic
    @Column(name = "MAPPED_CDTY_CODE", length = 25)
    public String getMappedCdtyCode() {
        return mappedCdtyCode;
    }

    public void setMappedCdtyCode(String mappedCdtyCode) {
        this.mappedCdtyCode = mappedCdtyCode;
    }

    @Basic
    @Column(name = "MAPPED_BRKR_SN", length = 50)
    public String getMappedBrkrSn() {
        return mappedBrkrSn;
    }

    public void setMappedBrkrSn(String mappedBrkrSn) {
        this.mappedBrkrSn = mappedBrkrSn;
    }


    @OneToMany(mappedBy = "inboundDocs")
    public Set<AssociatedDocsEntity> getAssociatedDocs() {
        return associatedDocs;
    }

    public void setAssociatedDocs(Set<AssociatedDocsEntity> associatedDocs) {
        this.associatedDocs = associatedDocs;
    }

    @OneToOne(optional = true)
    @JoinColumn(name = "SENT_TO", insertable = false, updatable = false)
    public InboundFaxNosEntity getInboundFaxNos() {
        return inboundFaxNos;
    }

    public void setInboundFaxNos(InboundFaxNosEntity inboundFaxNos) {
        this.inboundFaxNos = inboundFaxNos;
    }

    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        InboundDocsEntity that = (InboundDocsEntity) o;

        return !(id != null ? !id.equals(that.id) : that.id != null);
    }

    public int hashCode() {
        return (id != null ? id.hashCode() : 0);
    }

    public void setMappedValue(String inbAttribCode, String mappedValue) {
        if(inbAttribCode.equals(InbAttribEntity.CPTY_SN)){
           this.setMappedCptySn(mappedValue);
        } else if(inbAttribCode.equals(InbAttribEntity.CDTY_CODE)){
           this.setMappedCdtyCode(mappedValue);
        } else if(inbAttribCode.equals(InbAttribEntity.BRKR_SN)){
           this.setMappedBrkrSn(mappedValue);  
        }
    }
}
