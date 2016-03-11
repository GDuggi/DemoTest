package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class TradeConfirmCreatorRequest implements Serializable{
	
	private long  rqmtConfirmId;
	private String userId;
	
	public long getRqmtConfirmId() {
		return rqmtConfirmId;
	}
	public void setRqmtConfirmId(long rqmtId) {
		this.rqmtConfirmId = rqmtId;
	}
	public String getUserId() {
		return userId;
	}
	public void setUserId(String approver) {
		this.userId = approver;
	}
	

}
