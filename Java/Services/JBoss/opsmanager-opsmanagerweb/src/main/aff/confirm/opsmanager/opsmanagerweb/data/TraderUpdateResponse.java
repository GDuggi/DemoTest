package aff.confirm.opsmanager.opsmanagerweb.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TraderUpdateResponse extends BaseResponse{

	private TraderUpdateRequest request;

	public TraderUpdateRequest getRequest() {
		return request;
	}

	public void setRequest(TraderUpdateRequest request) {
		this.request = request;
	}
}
