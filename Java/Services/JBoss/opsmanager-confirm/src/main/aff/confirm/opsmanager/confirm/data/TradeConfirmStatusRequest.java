package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class TradeConfirmStatusRequest implements Serializable {
	
	private String statusInd;
	private long templateId;
	private String noConfirmReason;
	private String comment;
	private String faxTelexInd;
	private long newTemplateId;
	private String faxTelexNumber;
	private long tradeId;
	public long getTemplateId() {
		return templateId;
	}
	public void setTemplateId(long templateId) {
		this.templateId = templateId;
	}
	public String getNoConfirmReason() {
		return noConfirmReason;
	}
	public void setNoConfirmReason(String noConfirmReason) {
		this.noConfirmReason = noConfirmReason;
	}
	public String getComment() {
		return comment;
	}
	public void setComment(String comment) {
		this.comment = comment;
	}
	public String getFaxTelexInd() {
		return faxTelexInd;
	}
	public void setFaxTelexInd(String faxTelexInd) {
		this.faxTelexInd = faxTelexInd;
	}
	public long getNewTemplateId() {
		return newTemplateId;
	}
	public void setNewTemplateId(long newTemplateId) {
		this.newTemplateId = newTemplateId;
	}
	public String getFaxTelexNumber() {
		return faxTelexNumber;
	}
	public void setFaxTelexNumber(String faxTelexNumber) {
		this.faxTelexNumber = faxTelexNumber;
	}
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public String getStatusInd() {
		return statusInd;
	}
	public void setStatusInd(String statusInd) {
		this.statusInd = statusInd;
	}
	

}
