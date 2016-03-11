package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class FaxLogSentResponse extends BaseResponse{
	
	private FaxLogSentRequest request;

	public FaxLogSentRequest getRequest() {
		return request;
	}

	public void setRequest(FaxLogSentRequest request) {
		this.request = request;
	}
	

}
