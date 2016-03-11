package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class TradeGroupRequest implements Serializable {
	
	private long tradeId;
	private String xRef;
	
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public String getXRef() {
		return xRef;
	}
	public void setXRef(String ref) {
		xRef = ref;
	}
	

}
