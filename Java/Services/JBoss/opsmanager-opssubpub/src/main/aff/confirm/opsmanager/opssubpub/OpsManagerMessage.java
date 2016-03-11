package aff.confirm.opsmanager.opssubpub;

import java.io.Serializable;
import java.util.Date;

public class OpsManagerMessage implements  Serializable{

	/**
	 *
	 */
	private static final long serialVersionUID = -5772039117501144797L;

	private String messageType;
	private Date publishedDateTime;
	private Object data;

	public String getMessageType() {
		return messageType;
	}
	public void setMessageType(String messageType) {
		this.messageType = messageType;
	}
	public Date getPublishedDateTime() {
		return publishedDateTime;
	}
	public void setPublishedDateTime(Date publishedDateTime) {
		this.publishedDateTime = publishedDateTime;
	}
	public Object getData() {
		return data;
	}
	public void setData(Object data) {
		this.data = data;
	}
}
