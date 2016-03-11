package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class RqmtConfirmUpdateResponse extends BaseResponse{

	private RqmtConfirmUpdateRequest request;

	public RqmtConfirmUpdateRequest getRequest() {
		return request;
	}

	public void setRequest(RqmtConfirmUpdateRequest request) {
		this.request = request;
	}
}
