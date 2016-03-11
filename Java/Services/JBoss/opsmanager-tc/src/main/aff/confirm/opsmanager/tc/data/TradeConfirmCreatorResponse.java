package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TradeConfirmCreatorResponse extends BaseResponse{
	
	private TradeConfirmCreatorRequest request;
	
	private String userId;

	public TradeConfirmCreatorRequest getRequest() {
		return request;
	}

	public void setRequest(TradeConfirmCreatorRequest request) {
		this.request = request;
	}

	public String getUserId() {
		return userId;
	}

	public void setUserId(String apprUser) {
		this.userId = apprUser;
	}
	

}
