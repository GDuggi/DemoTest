package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class FaxStatusLogData implements Serializable{
	
	
	private long id;
	private long tradeId;
	private long tradeRqmtConfirmId;
	private String sender;
	private String statusUpdateDateTime;
	private String faxTelexInd;
	private String faxTelexNumber;
	private String comment;
	private String status;
	
	public long getId() {
		return id;
	}
	public void setId(long id) {
		this.id = id;
	}
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public long getTradeRqmtConfirmId() {
		return tradeRqmtConfirmId;
	}
	public void setTradeRqmtConfirmId(long tradeRqmtConfirmId) {
		this.tradeRqmtConfirmId = tradeRqmtConfirmId;
	}
	public String getSender() {
		return sender;
	}
	public void setSender(String sender) {
		this.sender = sender;
	}
	public String getStatusUpdateDateTime() {
		return statusUpdateDateTime;
	}
	public void setStatusUpdateDateTime(String statusUpdateDateTime) {
		this.statusUpdateDateTime = statusUpdateDateTime;
	}
	public String getFaxTelexInd() {
		return faxTelexInd;
	}
	public void setFaxTelexInd(String faxTelexInd) {
		this.faxTelexInd = faxTelexInd;
	}
	public String getFaxTelexNumber() {
		return faxTelexNumber;
	}
	public void setFaxTelexNumber(String faxTelexNumber) {
		this.faxTelexNumber = faxTelexNumber;
	}
	public String getComment() {
		return comment;
	}
	public void setComment(String comment) {
		this.comment = comment;
	}
	public String getStatus() {
		return status;
	}
	public void setStatus(String status) {
		this.status = status;
	}
	
	
}
