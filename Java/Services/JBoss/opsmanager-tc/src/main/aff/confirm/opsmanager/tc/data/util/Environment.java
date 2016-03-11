package aff.confirm.opsmanager.tc.data.util;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class Environment extends BaseResponse	 {

	/**
	 * 
	 */
	private static final long serialVersionUID = -7342618779986744868L;
	private String affinityDatabase;

	public String getAffinityDatabase() {
		return affinityDatabase;
	}

	public void setAffinityDatabase(String affinityDatabase) {
		this.affinityDatabase = affinityDatabase;
	}
	
}
