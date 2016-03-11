package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TradeGroupResponse extends BaseResponse{

	/**
	 * 
	 */
	private static final long serialVersionUID = -373342020669260987L;

	private TradeGroupRequest request;

	public TradeGroupRequest getRequest() {
		return request;
	}

	public void setRequest(TradeGroupRequest request) {
		this.request = request;
	}
	
	
}
