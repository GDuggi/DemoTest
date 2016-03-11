package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;
import java.util.Date;

public class TradeRqmtRequest implements Serializable{
	
	private long tradeId;
	private String rqmtCode;
	private String reference;
	private String  comment;
	
	private long  rqmtId;
	private Date  statusDate;
	private String status;
	private String secondCheck;
	 
	
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public String getRqmtCode() {
		return rqmtCode;
	}
	public void setRqmtCode(String rqmtCode) {
		this.rqmtCode = rqmtCode;
	}
	public String getReference() {
		return reference;
	}
	public void setReference(String reference) {
		this.reference = reference;
	}
	public String getComment() {
		return comment;
	}
	public void setComment(String comment) {
		this.comment = comment;
	}
	public long getRqmtId() {
		return rqmtId;
	}
	public void setRqmtId(long rqmtId) {
		this.rqmtId = rqmtId;
	}
	public Date getStatusDate() {
		return statusDate;
	}
	public void setStatusDate(Date statusDate) {
		this.statusDate = statusDate;
	}
	public String getStatus() {
		return status;
	}
	public void setStatus(String status) {
		this.status = status;
	}
	public String getSecondCheck() {
		return secondCheck;
	}
	public void setSecondCheck(String secondCheck) {
		this.secondCheck = secondCheck;
	}
	

}
