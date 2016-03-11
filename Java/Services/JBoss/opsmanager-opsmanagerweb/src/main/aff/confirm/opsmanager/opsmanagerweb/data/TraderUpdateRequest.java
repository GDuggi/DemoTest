package aff.confirm.opsmanager.opsmanagerweb.data;

import java.io.Serializable;

public class TraderUpdateRequest implements Serializable{

	private long tradeId;
	private long rqmtId;
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public long getRqmtId() {
		return rqmtId;
	}
	public void setRqmtId(long rqmtId) {
		this.rqmtId = rqmtId;
	}
	
}
