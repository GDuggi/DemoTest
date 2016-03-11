package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class InboundDocCountResponse extends BaseResponse{

	private InboundDocCountRequest request;
	private int count;
	
	public InboundDocCountRequest getRequest() {
		return request;
	}
	public void setRequest(InboundDocCountRequest request) {
		this.request = request;
	}
	public int getCount() {
		return count;
	}
	public void setCount(int count) {
		this.count = count;
	}
	
}
