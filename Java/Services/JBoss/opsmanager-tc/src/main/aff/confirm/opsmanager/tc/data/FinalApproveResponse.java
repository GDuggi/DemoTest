package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class FinalApproveResponse extends BaseResponse{

	private FinalApproveRequest request;

	public FinalApproveRequest getRequest() {
		return request;
	}

	public void setRequest(FinalApproveRequest request) {
		this.request = request;
	}
	
	
}
