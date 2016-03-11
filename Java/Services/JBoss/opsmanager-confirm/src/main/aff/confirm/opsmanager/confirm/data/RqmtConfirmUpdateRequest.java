package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class RqmtConfirmUpdateRequest implements Serializable {

	
	
	private long id;
	private long rqmtId;
	private long tradeId;
	private long templateId;
	private String confirmCmt;
	private String  faxTelexInd;
	private String faxTelexNumber;
	private String confirmLabel;
	private String nextStatusCode;
	
	
	public long getId() {
		return id;
	}
	public void setId(long id) {
		this.id = id;
	}
	public long getRqmtId() {
		return rqmtId;
	}
	public void setRqmtId(long rqmtId) {
		this.rqmtId = rqmtId;
	}
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public long getTemplateId() {
		return templateId;
	}
	public void setTemplateId(long templateId) {
		this.templateId = templateId;
	}
	public String getConfirmCmt() {
		return confirmCmt;
	}
	public void setConfirmCmt(String confirmCmt) {
		this.confirmCmt = confirmCmt;
	}
	public String getFaxTelexInd() {
		return faxTelexInd;
	}
	public void setFaxTelexInd(String transmissionMode) {
		this.faxTelexInd = transmissionMode;
	}
	public String getFaxTelexNumber() {
		return faxTelexNumber;
	}
	public void setFaxTelexNumber(String recipentAddrNumber) {
		this.faxTelexNumber = recipentAddrNumber;
	}
	public String getConfirmLabel() {
		return confirmLabel;
	}
	public void setConfirmLabel(String confirmLabel) {
		this.confirmLabel = confirmLabel;
	}
	public String getNextStatusCode() {
		return nextStatusCode;
	}
	public void setNextStatusCode(String nextStatusCode) {
		this.nextStatusCode = nextStatusCode;
	}
	
	
	
}
