package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class InbAttrMapValueUpdateResponse extends BaseResponse{
	
	private InbAttrMapValueUpdateRequest request;

	
	public InbAttrMapValueUpdateRequest getRequest() {
		return request;
	}

	public void setRequest(InbAttrMapValueUpdateRequest request) {
		this.request = request;
	}
	

}
