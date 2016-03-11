package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class TraderEmailAddressRequest implements Serializable {
	
	private long tradeId;

	public long getTradeId() {
		return tradeId;
	}

	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}

}
