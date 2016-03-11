package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TradeConfirmStatusResponse extends BaseResponse{

	private TradeConfirmStatusRequest request;

	public TradeConfirmStatusRequest getRequest() {
		return request;
	}

	public void setRequest(TradeConfirmStatusRequest request) {
		this.request = request;
	}
	
}
