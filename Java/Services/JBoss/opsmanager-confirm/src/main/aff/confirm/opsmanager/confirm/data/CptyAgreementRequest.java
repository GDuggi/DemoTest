package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class CptyAgreementRequest implements Serializable{
	
	private String cptyShortName;
	private String tradeDate;
	
	public String getCptyShortName() {
		return cptyShortName;
	}
	public void setCptyShortName(String cptyShortName) {
		this.cptyShortName = cptyShortName;
	}
	public String getTradeDate() {
		return tradeDate;
	}
	public void setTradeDate(String tradeDate) {
		this.tradeDate = tradeDate;
	}
	

}
