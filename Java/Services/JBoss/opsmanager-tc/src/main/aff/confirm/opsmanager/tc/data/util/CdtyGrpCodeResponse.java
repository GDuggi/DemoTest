package aff.confirm.opsmanager.tc.data.util;

import java.util.ArrayList;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class CdtyGrpCodeResponse extends BaseResponse {
	
	
	private ArrayList<String> cdtyGrpCodes;

	public ArrayList<String> getCdtyGrpCodes() {
		return cdtyGrpCodes;
	}

	public void setCdtyGrpCodes(ArrayList<String> cdtyGrpCodes) {
		this.cdtyGrpCodes = cdtyGrpCodes;
	}
	
}
