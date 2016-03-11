package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class Trade2ndCheckResponse extends BaseResponse{
	
	private Trade2ndCheckRequest request;
	
	private String apprUser;

	public Trade2ndCheckRequest getRequest() {
		return request;
	}

	public void setRequest(Trade2ndCheckRequest request) {
		this.request = request;
	}

	public String getApprUser() {
		return apprUser;
	}

	public void setApprUser(String apprUser) {
		this.apprUser = apprUser;
	}
	

}
