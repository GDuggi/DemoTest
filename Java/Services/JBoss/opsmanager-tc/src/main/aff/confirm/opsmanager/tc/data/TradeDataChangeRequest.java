package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class TradeDataChangeRequest implements Serializable{

	private long tradeId;

	public long getTradeId() {
		return tradeId;
	}

	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	
}
