package aff.confirm.opsmanager.tc.data.util;

import java.io.Serializable;

public class ECMBoxSubmitRequest implements Serializable{
	
	private String processMasterCode;

	public String getProcessMasterCode() {
		return processMasterCode;
	}

	public void setProcessMasterCode(String processMasterCode) {
		this.processMasterCode = processMasterCode;
	}
	

}
