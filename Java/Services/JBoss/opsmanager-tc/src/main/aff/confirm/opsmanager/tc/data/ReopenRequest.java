package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;




public class ReopenRequest implements Serializable{

	private long tradeId;
	private String tradeSysCode;
	
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public String getTradeSysCode() {
		return tradeSysCode;
	}
	public void setTradeSysCode(String tradeSysCode) {
		this.tradeSysCode = tradeSysCode;
	}
	
}
