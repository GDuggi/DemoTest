package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class InboundUpdateResponse extends BaseResponse{
	
	private InboundUpdateRequest request;

	public InboundUpdateRequest getRequest() {
		return request;
	}

	public void setRequest(InboundUpdateRequest request) {
		this.request = request;
	}
	

}
