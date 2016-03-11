package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TradeCommentResponse extends BaseResponse{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;

	private TradeCommentRequest request;

	public TradeCommentRequest getRequest() {
		return request;
	}

	public void setRequest(TradeCommentRequest request) {
		this.request = request;
	}
	

}
