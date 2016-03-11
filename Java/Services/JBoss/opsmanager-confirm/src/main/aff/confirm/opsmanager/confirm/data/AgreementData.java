package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class AgreementData implements Serializable{

	private String agreementTypeCode;
	private String statusInd;
	private String dateSigned;
	private String terminationDate;
	private String contactName;
	private String comment;
	private long agreementId;
	private long sempraCompanyId;
	private long cptyId;
	private String sempraCompanySn;
	private String cptySn;
	
	public String getAgreementTypeCode() {
		return agreementTypeCode;
	}
	public void setAgreementTypeCode(String agreementTypeCode) {
		this.agreementTypeCode = agreementTypeCode;
	}
	public String getStatusInd() {
		return statusInd;
	}
	public void setStatusInd(String statusInd) {
		this.statusInd = statusInd;
	}
	public String getDateSigned() {
		return dateSigned;
	}
	public void setDateSigned(String dateSigned) {
		this.dateSigned = dateSigned;
	}
	public String getTerminationDate() {
		return terminationDate;
	}
	public void setTerminationDate(String terminationDate) {
		this.terminationDate = terminationDate;
	}
	public String getContactName() {
		return contactName;
	}
	public void setContactName(String contactName) {
		this.contactName = contactName;
	}
	public String getComment() {
		return comment;
	}
	public void setComment(String comment) {
		this.comment = comment;
	}
	public long getAgreementId() {
		return agreementId;
	}
	public void setAgreementId(long agreementId) {
		this.agreementId = agreementId;
	}
	public long getSempraCompanyId() {
		return sempraCompanyId;
	}
	public void setSempraCompanyId(long sempraCompanyId) {
		this.sempraCompanyId = sempraCompanyId;
	}
	public long getCptyId() {
		return cptyId;
	}
	public void setCptyId(long cptyId) {
		this.cptyId = cptyId;
	}
	public String getSempraCompanySn() {
		return sempraCompanySn;
	}
	public void setSempraCompanySn(String sempraCompanySn) {
		this.sempraCompanySn = sempraCompanySn;
	}
	public String getCptySn() {
		return cptySn;
	}
	public void setCptySn(String cptySn) {
		this.cptySn = cptySn;
	}
	
}
