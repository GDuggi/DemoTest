package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class MapCallerReferenceResponse extends BaseResponse{

	private MapCallerReferenceRequest request;

	public MapCallerReferenceRequest getRequest() {
		return request;
	}

	public void setRequest(MapCallerReferenceRequest request) {
		this.request = request;
	}
	
}
