package aff.confirm.opsmanager.opsmanagerweb.data;

import java.io.Serializable;

public class FaxReconcileData implements Serializable{
	
	private String sender;
	private String faxNumber;
	private String jobReference;
	private String receivedAt;
	private String message;
	
	public String getSender() {
		return sender;
	}
	public void setSender(String sender) {
		this.sender = sender;
	}
	public String getFaxNumber() {
		return faxNumber;
	}
	public void setFaxNumber(String faxNumber) {
		this.faxNumber = faxNumber;
	}
	public String getJobReference() {
		return jobReference;
	}
	public void setJobReference(String jobReference) {
		this.jobReference = jobReference;
	}
	public String getReceivedAt() {
		return receivedAt;
	}
	public void setReceivedAt(String receivedAt) {
		this.receivedAt = receivedAt;
	}
	public String getMessage() {
		return message;
	}
	public void setMessage(String message) {
		this.message = message;
	}
	
	

}
