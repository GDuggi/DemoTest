package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class SearchContractData implements Serializable{
	

	private String tradeDate;
	private String templateName;
	private long tradeConfirmId;
	private long tradeRqmtConfirmId;
	
	public String getTradeDate() {
		return tradeDate;
	}
	public void setTradeDate(String tradeDate) {
		this.tradeDate = tradeDate;
	}
	public String getTemplateName() {
		return templateName;
	}
	public void setTemplateName(String templateName) {
		this.templateName = templateName;
	}
	public long getTradeConfirmId() {
		return tradeConfirmId;
	}
	public void setTradeConfirmId(long tradeConfirmId) {
		this.tradeConfirmId = tradeConfirmId;
	}
	public long getTradeRqmtConfirmId() {
		return tradeRqmtConfirmId;
	}
	public void setTradeRqmtConfirmId(long tradeRqmtConfirmId) {
		this.tradeRqmtConfirmId = tradeRqmtConfirmId;
	}
	
}
