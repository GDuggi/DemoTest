package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TradeRqmtConfirmDeleteResponse extends BaseResponse{
	
	private TradeRqmtConfirmDeleteRequest request;

	public TradeRqmtConfirmDeleteRequest getRequest() {
		return request;
	}

	public void setRequest(TradeRqmtConfirmDeleteRequest request) {
		this.request = request;
	}
	
	
}
