package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class ReopenResponse extends BaseResponse{

	private ReopenRequest request;

	public ReopenRequest getRequest() {
		return request;
	}

	public void setRequest(ReopenRequest request) {
		this.request = request;
	}
	
}
