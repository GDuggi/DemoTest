package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class TradeDataChange  implements Serializable{

	private long id;
	private long tradeId;
	private String updateBusnDate;
	private String columnName;
	private String oldValue;
	private String newValue;
	private String createDateTime;
	
	private String userName;
	private String auditTypeCode;
	private String odbIncludeFlag;
	private long  odbCancelCorrectExclId;
	
	
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
	
	public String getUpdateBusnDate() {
		return updateBusnDate;
	}
	public void setUpdateBusnDate(String updateBusnDate) {
		this.updateBusnDate = updateBusnDate;
	}
	public String getColumnName() {
		return columnName;
	}
	public void setColumnName(String columnName) {
		this.columnName = columnName;
	}
	public String getOldValue() {
		return oldValue;
	}
	public void setOldValue(String oldValue) {
		this.oldValue = oldValue;
	}
	public String getNewValue() {
		return newValue;
	}
	public void setNewValue(String newValue) {
		this.newValue = newValue;
	}
	public String getCreateDateTime() {
		return createDateTime;
	}
	
	public void setCreateDateTime(String createDateTime) {
		this.createDateTime = createDateTime;
	}
	public String getUserName() {
		return userName;
	}
	public void setUserName(String userName) {
		this.userName = userName;
	}
	public String getAuditTypeCode() {
		return auditTypeCode;
	}
	public void setAuditTypeCode(String auditTypeCode) {
		this.auditTypeCode = auditTypeCode;
	}
	public String getOdbIncludeFlag() {
		return odbIncludeFlag;
	}
	public void setOdbIncludeFlag(String odbIncludeFlag) {
		this.odbIncludeFlag = odbIncludeFlag;
	}
	public long getOdbCancelCorrectExclId() {
		return odbCancelCorrectExclId;
	}
	public void setOdbCancelCorrectExclId(long odbCancelCorrectExclId) {
		this.odbCancelCorrectExclId = odbCancelCorrectExclId;
	}
	
	
}
