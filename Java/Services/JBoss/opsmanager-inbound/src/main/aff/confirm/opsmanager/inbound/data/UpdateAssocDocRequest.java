package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.inbound.data.AssociatedDocumentData._Assoc_Doc_Status_Code;

import java.io.Serializable;

public class UpdateAssocDocRequest implements Serializable{

	private long inboundDocId;
	private String fileName;
	private String cdtyGrpCode;
	private String cptySn;
	private String brokerSn;
	private long rqmtId;
	private String rqmtStatus;
	private String rqmtType;
	private String secondCheck;
	private long tradeId;
	private _Assoc_Doc_Status_Code docStatusCode;
	private int indexVal;
	
	public _Assoc_Doc_Status_Code getDocStatusCode() {
		return docStatusCode;
	}
	public void setDocStatusCode(_Assoc_Doc_Status_Code docStatusCode) {
		this.docStatusCode = docStatusCode;
	}
	public long getInboundDocId() {
		return inboundDocId;
	}
	public void setInboundDocId(long inboundDocId) {
		this.inboundDocId = inboundDocId;
	}
	public String getFileName() {
		return fileName;
	}
	public void setFileName(String fileName) {
		this.fileName = fileName;
	}
	public String getCdtyGrpCode() {
		return cdtyGrpCode;
	}
	public void setCdtyGrpCode(String cdtyGrpCode) {
		this.cdtyGrpCode = cdtyGrpCode;
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
	public long getRqmtId() {
		return rqmtId;
	}
	public void setRqmtId(long rqmtId) {
		this.rqmtId = rqmtId;
	}
	public String getRqmtStatus() {
		return rqmtStatus;
	}
	public void setRqmtStatus(String rqmtStatus) {
		this.rqmtStatus = rqmtStatus;
	}
	public String getRqmtType() {
		return rqmtType;
	}
	public void setRqmtType(String rqmtType) {
		this.rqmtType = rqmtType;
	}
	public String getSecondCheck() {
		return secondCheck;
	}
	public void setSecondCheck(String secondCheck) {
		this.secondCheck = secondCheck;
	}
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public int getIndexVal() {
		return indexVal;
	}
	public void setIndexVal(int indexVal) {
		this.indexVal = indexVal;
	}
	
}
