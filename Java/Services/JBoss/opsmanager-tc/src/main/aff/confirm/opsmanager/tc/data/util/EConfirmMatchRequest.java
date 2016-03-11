package aff.confirm.opsmanager.tc.data.util;

import java.io.Serializable;

public class EConfirmMatchRequest implements Serializable{
	
	private long tradeId;
	private String cptyRefId;
	private String statusDateStr;
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public String getCptyRefId() {
		return cptyRefId;
	}
	public void setCptyRefId(String cptyRefId) {
		this.cptyRefId = cptyRefId;
	}
	public String getStatusDateStr() {
		return statusDateStr;
	}
	public void setStatusDateStr(String statusDateStr) {
		this.statusDateStr = statusDateStr;
	}
	

}
