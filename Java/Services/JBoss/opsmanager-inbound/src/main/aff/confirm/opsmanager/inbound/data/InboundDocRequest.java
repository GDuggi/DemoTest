package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;

public class InboundDocRequest implements Serializable{

	private long id;
	private String callerRef;
	private String sentTo;
	private String rcvdTimeStamp;
	private String fileName;
	private String sender;
	private String cmt;
	private String docStatusCode;
	private String hasAutoAssocFlag;
	private String mappedCptySn;
	
	
	
	public long getId() {
		return id;
	}
	public void setId(long id) {
		this.id = id;
	}
	public String getCallerRef() {
		return callerRef;
	}
	public void setCallerRef(String callerRef) {
		this.callerRef = callerRef;
	}
	public String getSentTo() {
		return sentTo;
	}
	public void setSentTo(String sentTo) {
		this.sentTo = sentTo;
	}
	public String getRcvdTimeStamp() {
		return rcvdTimeStamp;
	}
	public void setRcvdTimeStamp(String rcvdTimeStamp) {
		this.rcvdTimeStamp = rcvdTimeStamp;
	}
	public String getFileName() {
		return fileName;
	}
	public void setFileName(String fileName) {
		this.fileName = fileName;
	}
	public String getSender() {
		return sender;
	}
	public void setSender(String sender) {
		this.sender = sender;
	}
	public String getCmt() {
		return cmt;
	}
	public void setCmt(String cmt) {
		this.cmt = cmt;
	}
	public String getDocStatusCode() {
		return docStatusCode;
	}
	public void setDocStatusCode(String docStatusCode) {
		this.docStatusCode = docStatusCode;
	}
	public String getHasAutoAssocFlag() {
		return hasAutoAssocFlag;
	}
	public void setHasAutoAssocFlag(String hasAutoAssocFlag) {
		this.hasAutoAssocFlag = hasAutoAssocFlag;
	}
	public String getMappedCptySn() {
		return mappedCptySn;
	}
	public void setMappedCptySn(String mappedCptySn) {
		this.mappedCptySn = mappedCptySn;
	}
	
	
}
