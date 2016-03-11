package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;


public class GetUserFlagData implements Serializable{

	private long id;
	private long inboundDocId;
	private String inboundUser;
	private String flagType;
	private String comments;
	
	
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
	public String getInboundUser() {
		return inboundUser;
	}
	public void setInboundUser(String inboundUser) {
		this.inboundUser = inboundUser;
	}
	public String getFlagType() {
		return flagType;
	}
	public void setFlagType(String flagType) {
		this.flagType = flagType;
	}
	public String getComments() {
		return comments;
	}
	public void setComments(String comments) {
		this.comments = comments;
	}
	
	
}
