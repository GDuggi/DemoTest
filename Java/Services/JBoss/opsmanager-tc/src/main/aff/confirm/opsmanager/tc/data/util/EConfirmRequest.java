package aff.confirm.opsmanager.tc.data.util;

import java.io.Serializable;


public class EConfirmRequest implements Serializable{

	
	
	private long tradeId;
	private String status;
	
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public String getStatus() {
		return status;
	}
	public void setStatus(String status) {
		this.status = status;
	}
	
	

}
