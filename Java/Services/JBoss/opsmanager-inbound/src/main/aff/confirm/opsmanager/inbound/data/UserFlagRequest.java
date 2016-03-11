package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;

public class UserFlagRequest implements Serializable{
	public enum _update_user_flag  { Delete,Update}; 

	private long inboundDocId;
	private String userName;
	private String flagType;
	private String comment;
	private _update_user_flag updateDeleteFlag;
	
	public long getInboundDocId() {
		return inboundDocId;
	}
	public void setInboundDocId(long inboundDocId) {
		this.inboundDocId = inboundDocId;
	}
	public String getUserName() {
		return userName;
	}
	public void setUserName(String userName) {
		this.userName = userName;
	}
	public String getFlagType() {
		return flagType;
	}
	public void setFlagType(String flagType) {
		this.flagType = flagType;
	}
	public String getComment() {
		return comment;
	}
	public void setComment(String comment) {
		this.comment = comment;
	}
	public _update_user_flag getUpdateDeleteFlag() {
		return updateDeleteFlag;
	}
	public void setUpdateDeleteFlag(_update_user_flag updateDeleteFlag) {
		this.updateDeleteFlag = updateDeleteFlag;
	}
	
}
