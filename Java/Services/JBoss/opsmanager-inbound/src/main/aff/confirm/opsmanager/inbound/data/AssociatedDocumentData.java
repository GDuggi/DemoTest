package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;
import java.util.Date;

public class AssociatedDocumentData implements Serializable {

	public enum _Assoc_Doc_Status_Code  { Associated,UnAssociated,Reserved,Approved,PreApproved,Discarded,Open,Closed,Disputed,Vaulted};
	
	/**
	 * 
	 */
		private static final long serialVersionUID = 1L;

		private long id;
		private long inboundDocId;
		private long indexVal;
		private String fileName;
		private long tradeId;
		private _Assoc_Doc_Status_Code docStatusCode;
		private String associatedBy;
		private Date associatedDate;
		private String finalApprovedBy;
		private Date finalApprovedDate;
		private String disputedBy;
		private Date disputedDate;
		private String discardedBy;
		private Date discardedDate;
		private String vaultedBy;
		private Date vaultedDate;
		private String cdtyGroupCode;
		private String cptySn;
		private String brokerSn;
		private String docTypeCode;
		private String secValidateReqFlag;
		private long tradeRqmtId;
		
		public long getId() {
			return id;
		}
		public void setId(long id) {
			this.id = id;
		}
		public long getInboundDocId() {
			return inboundDocId;
		}
		public void setInboundDocId(long inboundDocId) {
			this.inboundDocId = inboundDocId;
		}
		public long getIndexVal() {
			return indexVal;
		}
		public void setIndexVal(long indexVal) {
			this.indexVal = indexVal;
		}
		public String getFileName() {
			return fileName;
		}
		public void setFileName(String fileName) {
			this.fileName = fileName;
		}
		public long getTradeId() {
			return tradeId;
		}
		public void setTradeId(long tradeId) {
			this.tradeId = tradeId;
		}
		public _Assoc_Doc_Status_Code getDocStatusCode() {
			return docStatusCode;
		}
		public void setDocStatusCode(_Assoc_Doc_Status_Code docStatusCode) {
			this.docStatusCode = docStatusCode;
		}
		public String getAssociatedBy() {
			return associatedBy;
		}
		public void setAssociatedBy(String associatedBy) {
			this.associatedBy = associatedBy;
		}
		public Date getAssociatedDate() {
			return associatedDate;
		}
		public void setAssociatedDate(Date associatedDate) {
			this.associatedDate = associatedDate;
		}
		public String getFinalApprovedBy() {
			return finalApprovedBy;
		}
		public void setFinalApprovedBy(String finalApprovedBy) {
			this.finalApprovedBy = finalApprovedBy;
		}
		public Date getFinalApprovedDate() {
			return finalApprovedDate;
		}
		public void setFinalApprovedDate(Date finalApprovedDate) {
			this.finalApprovedDate = finalApprovedDate;
		}
		public String getDisputedBy() {
			return disputedBy;
		}
		public void setDisputedBy(String disputedBy) {
			this.disputedBy = disputedBy;
		}
		public Date getDisputedDate() {
			return disputedDate;
		}
		public void setDisputedDate(Date disputedDate) {
			this.disputedDate = disputedDate;
		}
		public String getDiscardedBy() {
			return discardedBy;
		}
		public void setDiscardedBy(String discardedBy) {
			this.discardedBy = discardedBy;
		}
		public Date getDiscardedDate() {
			return discardedDate;
		}
		public void setDiscardedDate(Date discardedDate) {
			this.discardedDate = discardedDate;
		}
		public String getVaultedBy() {
			return vaultedBy;
		}
		public void setVaultedBy(String vaultedBy) {
			this.vaultedBy = vaultedBy;
		}
		public Date getVaultedDate() {
			return vaultedDate;
		}
		public void setVaultedDate(Date vaultedDate) {
			this.vaultedDate = vaultedDate;
		}
		public String getCdtyGroupCode() {
			return cdtyGroupCode;
		}
		public void setCdtyGroupCode(String cdtyGroupCode) {
			this.cdtyGroupCode = cdtyGroupCode;
		}
		public String getCptySn() {
			return cptySn;
		}
		public void setCptySn(String cptySn) {
			this.cptySn = cptySn;
		}
		public String getBrokerSn() {
			return brokerSn;
		}
		public void setBrokerSn(String brokerSn) {
			this.brokerSn = brokerSn;
		}
		public String getDocTypeCode() {
			return docTypeCode;
		}
		public void setDocTypeCode(String docTypeCode) {
			this.docTypeCode = docTypeCode;
		}
		public String getSecValidateReqFlag() {
			return secValidateReqFlag;
		}
		public void setSecValidateReqFlag(String secValidateReqFlag) {
			this.secValidateReqFlag = secValidateReqFlag;
		}
		public long getTradeRqmtId() {
			return tradeRqmtId;
		}
		public void setTradeRqmtId(long tradeRqmtId) {
			this.tradeRqmtId = tradeRqmtId;
		}
		public static long getSerialVersionUID() {
			return serialVersionUID;
		}
		
	
}
