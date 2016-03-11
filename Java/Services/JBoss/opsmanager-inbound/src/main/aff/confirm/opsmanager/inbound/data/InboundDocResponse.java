package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class InboundDocResponse extends BaseResponse {
	
	private InboundDocRequest request;

	public InboundDocRequest getRequest() {
		return request;
	}

	public void setRequest(InboundDocRequest request) {
		this.request = request;
	}
	

}
