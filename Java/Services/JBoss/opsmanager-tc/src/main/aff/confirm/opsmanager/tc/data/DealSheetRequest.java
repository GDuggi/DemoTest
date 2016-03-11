package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class DealSheetRequest implements Serializable{
	public enum _trading_system { Affinity,JMS} 
	
	private _trading_system tradingSystem;
	private long tradeId;
	
	public _trading_system getTradingSystem() {
		return tradingSystem;
	}
	public void setTradingSystem(_trading_system tradingSystem) {
		this.tradingSystem = tradingSystem;
	}
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	

}
