package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TradeRqmtResponse extends BaseResponse{

	private TradeRqmtRequest request;

	
	public TradeRqmtRequest getRequest() {
		return request;
	}

	public void setRequest(TradeRqmtRequest request) {
		this.request = request;
	}
	
}
