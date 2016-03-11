package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class TradeAuditData implements Serializable {

	private long tradeId;
	private long tradeRqmtId;
	private String rqmt;
	private String userId;
	private String timeStamp;
	private String operation;
	private String machineName;
	private String status;
	private String completedDate;
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public long getTradeRqmtId() {
		return tradeRqmtId;
	}
	public void setTradeRqmtId(long tradeRqmtId) {
		this.tradeRqmtId = tradeRqmtId;
	}
	public String getRqmt() {
		return rqmt;
	}
	public void setRqmt(String rqmt) {
		this.rqmt = rqmt;
	}
	public String getUserId() {
		return userId;
	}
	public void setUserId(String userId) {
		this.userId = userId;
	}
	public String getTimeStamp() {
		return timeStamp;
	}
	public void setTimeStamp(String timeStamp) {
		this.timeStamp = timeStamp;
	}
	public String getOperation() {
		return operation;
	}
	public void setOperation(String operation) {
		this.operation = operation;
	}
	public String getMachineName() {
		return machineName;
	}
	public void setMachineName(String machineName) {
		this.machineName = machineName;
	}
	public String getStatus() {
		return status;
	}
	public void setStatus(String status) {
		this.status = status;
	}
	public String getCompletedDate() {
		return completedDate;
	}
	public void setCompletedDate(String completedDate) {
		this.completedDate = completedDate;
	}
	
}
