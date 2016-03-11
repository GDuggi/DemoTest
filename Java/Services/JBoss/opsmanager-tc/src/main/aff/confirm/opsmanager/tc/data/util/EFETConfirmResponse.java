package aff.confirm.opsmanager.tc.data.util;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class EFETConfirmResponse extends BaseResponse{

	/**
	 * 
	 */
	private static final long serialVersionUID = 6273335283425325438L;

	private long tradeId;
	private EFETConfirmRequest request;
	
	
	public EFETConfirmRequest getRequest(){
		return request;
	}
	
	public void setRequest(EFETConfirmRequest request){
		this.request = request;
	}

	public long getTradeId() {
		return tradeId;
	}

	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	
}
