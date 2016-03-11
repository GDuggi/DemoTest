package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TradeEditMasterResponse extends BaseResponse{
	
	private TradeCommentResponse[] tradeCommentResponse;
	private TradeRqmtResponse[] tradeRqmtResponse;
	
	public TradeCommentResponse[] getTradeCommentResponse() {
		return tradeCommentResponse;
	}
	public void setTradeCommentResponse(TradeCommentResponse[] tradeCommentResponse) {
		this.tradeCommentResponse = tradeCommentResponse;
	}
	public TradeRqmtResponse[] getTradeRqmtResponse() {
		return tradeRqmtResponse;
	}
	public void setTradeRqmtResponse(TradeRqmtResponse[] tradeRqmtResponse) {
		this.tradeRqmtResponse = tradeRqmtResponse;
	}
	
	

}
