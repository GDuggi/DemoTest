package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class SearchContractRequest implements Serializable {
	
	private String tradeSystemCode;
	private long tradeId;
	private String blotterNo;
	
	public String getTradeSystemCode() {
		return tradeSystemCode;
	}
	public void setTradeSystemCode(String tradeSystemCode) {
		this.tradeSystemCode = tradeSystemCode;
	}
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public String getBlotterNo() {
		return blotterNo;
	}
	public void setBlotterNo(String blotterNo) {
		this.blotterNo = blotterNo;
	}
	

}
