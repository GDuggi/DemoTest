package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class TradeAuditRequest implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private long tradeId;

	public long getTradeId() {
		return tradeId;
	}

	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	

}
