package aff.confirm.opsmanager.opsmanagerweb.data;

import java.io.Serializable;

public class ContractDataRequest implements Serializable{
	
	private long tradeId;
	private long tradeConfirmId;
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public long getTradeConfirmId() {
		return tradeConfirmId;
	}
	public void setTradeConfirmId(long tradeConfirmId) {
		this.tradeConfirmId = tradeConfirmId;
	}
	

}
