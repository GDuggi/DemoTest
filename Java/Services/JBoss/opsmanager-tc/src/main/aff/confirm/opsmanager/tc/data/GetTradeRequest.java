package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class GetTradeRequest implements Serializable{

	private String tradingSystem;
	private String tradeIdList;
	private String rbsSempraSn;
	private String cptySn;
	private String cdtyCode;
	private String tradeStartDate;
	private String tradeEndDate;
	private String queryName;
	
	public String getTradingSystem() {
		return tradingSystem;
	}
	public void setTradingSystem(String tradingSystem) {
		this.tradingSystem = tradingSystem;
	}
	public String getTradeIdList() {
		return tradeIdList;
	}
	public void setTradeIdList(String tradeIds) {
		this.tradeIdList = tradeIds;
	}
	public String getRbsSempraSn() {
		return rbsSempraSn;
	}
	public void setRbsSempraSn(String rbsSempraSn) {
		this.rbsSempraSn = rbsSempraSn;
	}
	public String getCptySn() {
		return cptySn;
	}
	public void setCptySn(String cptySn) {
		this.cptySn = cptySn;
	}
	public String getCdtyCode() {
		return cdtyCode;
	}
	public void setCdtyCode(String cdtyCode) {
		this.cdtyCode = cdtyCode;
	}
	public String getTradeStartDate() {
		return tradeStartDate;
	}
	public void setTradeStartDate(String tradeStartDate) {
		this.tradeStartDate = tradeStartDate;
	}
	public String getTradeEndDate() {
		return tradeEndDate;
	}
	public void setTradeEndDate(String endDate) {
		this.tradeEndDate = endDate;
	}
	public String getQueryName() {
		return queryName;
	}
	public void setQueryName(String queryName) {
		this.queryName = queryName;
	}
	
	
}
