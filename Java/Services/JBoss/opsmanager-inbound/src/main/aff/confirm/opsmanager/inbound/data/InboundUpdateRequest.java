package aff.confirm.opsmanager.inbound.data;

import javax.xml.bind.annotation.XmlElement;
import java.io.Serializable;

public class InboundUpdateRequest implements Serializable{
	
	
	private long id;
	private String callerRef;
	private String sentTo;
	private String fileName;
	private String sender;
	private String comment;
	private String docStatusCode;
	
	
	public long getId() {
		return id;
	}
	@XmlElement(name="Id")
	public void setId(long id) {
		this.id = id;
	}

	public String getCallerRef() {
		return callerRef;
	}
	@XmlElement(name="CallerRef")
	public void setCallerRef(String callerRef) {
		this.callerRef = callerRef;
	}
	public String getSentTo() {
		return sentTo;
	}
	@XmlElement(name="SentTo")
	public void setSentTo(String sendTo) {
		this.sentTo = sendTo;
	}
	public String getFileName() {
		return fileName;
	}
	@XmlElement(name="FileName")
	public void setFileName(String fileName) {
		this.fileName = fileName;
	}
	public String getSender() {
		return sender;
	}
	@XmlElement(name="Sender")
	public void setSender(String sender) {
		this.sender = sender;
	}
	@XmlElement(name="DocStatusCode")
	public String getDocStatusCode() {
		return docStatusCode;
	}
	public void setDocStatusCode(String docStatusCode) {
		this.docStatusCode = docStatusCode;
	}
	public String getComment() {
		return comment;
	}
	@XmlElement(name="Cmt")
	public void setComment(String comment) {
		this.comment = comment;
	}
	
	

}
