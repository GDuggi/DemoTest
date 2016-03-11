package aff.confirm.opsmanager.tc.data.util;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class UserFilterUpdateResponse extends BaseResponse{

	private UserFilterUpdateRequest request;

	public UserFilterUpdateRequest getRequest() {
		return request;
	}

	public void setRequest(UserFilterUpdateRequest request) {
		this.request = request;
	}
	
}
