package aff.confirm.opsmanager.opsmanagerweb.data;

import java.io.Serializable;

public class FaxGatewayUpdateRequest implements Serializable{
	
	public enum _Gateway_Status { Sucess,Failed,Queued}
	
	private long tradeId;
	private long tradeRqmtId;
	private long tradeRqmtConfirmId;
	private _Gateway_Status status;
	private String recipAddr;
	private String label;
	private String sender;
	private String faxTelexInd;
	private String cmt;
	
	private long associatedDocId;
	
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
	public long getTradeRqmtConfirmId() {
		return tradeRqmtConfirmId;
	}
	public void setTradeRqmtConfirmId(long tradeRqmtConfirmId) {
		this.tradeRqmtConfirmId = tradeRqmtConfirmId;
	}
	public _Gateway_Status getStatus() {
		return status;
	}
	public void setStatus(_Gateway_Status status) {
		this.status = status;
	}
	public String getRecipAddr() {
		return recipAddr;
	}
	public void setRecipAddr(String recipAddr) {
		this.recipAddr = recipAddr;
	}
	public String getLabel() {
		return label;
	}
	public void setLabel(String label) {
		this.label = label;
	}
	public String getSender() {
		return sender;
	}
	public void setSender(String sender) {
		this.sender = sender;
	}
	public String getFaxTelexInd() {
		return faxTelexInd;
	}
	public void setFaxTelexInd(String faxTelexInd) {
		this.faxTelexInd = faxTelexInd;
	}
	public String getCmt() {
		return cmt;
	}
	public void setCmt(String cmt) {
		this.cmt = cmt;
	}
	public long getAssociatedDocId() {
		return associatedDocId;
	}
	public void setAssociatedDocId(long associatedDocId) {
		this.associatedDocId = associatedDocId;
	}
	

}
