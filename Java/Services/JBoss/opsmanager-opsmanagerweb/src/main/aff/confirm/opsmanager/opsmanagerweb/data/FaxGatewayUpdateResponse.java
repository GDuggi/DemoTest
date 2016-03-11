package aff.confirm.opsmanager.opsmanagerweb.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class FaxGatewayUpdateResponse extends BaseResponse{
	
	private FaxGatewayUpdateRequest request;

	public FaxGatewayUpdateRequest getRequest() {
		return request;
	}

	public void setRequest(FaxGatewayUpdateRequest request) {
		this.request = request;
	}

}
