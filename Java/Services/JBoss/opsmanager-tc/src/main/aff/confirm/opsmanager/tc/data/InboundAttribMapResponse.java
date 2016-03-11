package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class InboundAttribMapResponse extends BaseResponse{
	
	private InboundAttribMapRequest request;

	public InboundAttribMapRequest getRequest() {
		return request;
	}

	public void setRequest(InboundAttribMapRequest request) {
		this.request = request;
	}
	

}
