package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class DetermineActionRequest implements Serializable{

	private long tradeId;
	private String actionFlag;
	
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public String getActionFlag() {
		return actionFlag;
	}
	public void setActionFlag(String actionFlag) {
		this.actionFlag = actionFlag;
	}
	
}
