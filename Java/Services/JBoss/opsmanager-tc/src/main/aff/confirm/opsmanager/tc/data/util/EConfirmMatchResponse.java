package aff.confirm.opsmanager.tc.data.util;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class EConfirmMatchResponse extends BaseResponse{

	private EConfirmMatchRequest request;

	public EConfirmMatchRequest getRequest() {
		return request;
	}

	public void setRequest(EConfirmMatchRequest request) {
		this.request = request;
	}
	
}
