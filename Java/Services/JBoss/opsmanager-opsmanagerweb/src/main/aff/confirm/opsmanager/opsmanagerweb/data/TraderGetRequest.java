package aff.confirm.opsmanager.opsmanagerweb.data;

import java.io.Serializable;

public class TraderGetRequest implements Serializable{

	private String traderName;
	private String getAllFlag;

	public String getTraderName() {
		return traderName;
	}

	public void setTraderName(String traderName) {
		this.traderName = traderName;
	}

	public String getGetAllFlag() {
		return getAllFlag;
	}

	public void setGetAllFlag(String getAllFlag) {
		this.getAllFlag = getAllFlag;
	}
	
}
