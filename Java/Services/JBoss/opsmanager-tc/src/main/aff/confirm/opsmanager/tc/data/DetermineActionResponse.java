package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class DetermineActionResponse extends BaseResponse{
	
	private DetermineActionRequest request;

	public DetermineActionRequest getRequest() {
		return request;
	}

	public void setRequest(DetermineActionRequest request) {
		this.request = request;
	}
	

}
