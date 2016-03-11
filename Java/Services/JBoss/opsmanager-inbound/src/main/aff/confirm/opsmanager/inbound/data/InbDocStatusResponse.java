package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class InbDocStatusResponse extends BaseResponse{

	private InbDocStatusRequest request;

	public InbDocStatusRequest getRequest() {
		return request;
	}

	public void setRequest(InbDocStatusRequest request) {
		this.request = request;
	}
	
	
}
