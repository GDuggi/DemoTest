package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class Trade2ndCheckRequest implements Serializable{
	
	private long  rqmtId;
	private String approver;
	
	public long getRqmtId() {
		return rqmtId;
	}
	public void setRqmtId(long rqmtId) {
		this.rqmtId = rqmtId;
	}
	public String getApprover() {
		return approver;
	}
	public void setApprover(String approver) {
		this.approver = approver;
	}
	

}
