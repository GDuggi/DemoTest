package aff.confirm.opsmanager.tc.data.util;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class EConfirmResponse extends BaseResponse{

	/**
	 * 
	 */
	private static final long serialVersionUID = 5567365892797547516L;
	
	private long tradeId;
	private EConfirmRequest request;
	

	public EConfirmRequest getRequest(){
		return request;
	}
	public void setRequest(EConfirmRequest request){
		this.request = request; 
	}
	
	public long getTradeId() {
		return tradeId;
	}

	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	

	
}
