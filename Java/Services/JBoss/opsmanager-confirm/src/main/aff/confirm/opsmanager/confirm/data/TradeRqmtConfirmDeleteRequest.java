package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class TradeRqmtConfirmDeleteRequest implements Serializable{

	private long tradeRqmtConfirmId;

	public long getTradeRqmtConfirmId() {
		return tradeRqmtConfirmId;
	}

	public void setTradeRqmtConfirmId(long tradeRqmtConfirmId) {
		this.tradeRqmtConfirmId = tradeRqmtConfirmId;
	}
	
}
