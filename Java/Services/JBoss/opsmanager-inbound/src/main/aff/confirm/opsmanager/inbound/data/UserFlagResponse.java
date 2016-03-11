package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class UserFlagResponse extends BaseResponse{

	private UserFlagRequest request;

	public UserFlagRequest getRequest() {
		return request;
	}

	public void setRequest(UserFlagRequest request) {
		this.request = request;
	}
	
}
