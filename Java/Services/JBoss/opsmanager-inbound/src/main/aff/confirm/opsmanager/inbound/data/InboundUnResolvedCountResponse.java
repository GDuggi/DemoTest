package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class InboundUnResolvedCountResponse extends BaseResponse{
	
	private InboundUnResolvedCountRequest request;
	private int count;
	
	public InboundUnResolvedCountRequest getRequest() {
		return request;
	}
	public void setRequest(InboundUnResolvedCountRequest request) {
		this.request = request;
	}
	public int getCount() {
		return count;
	}
	public void setCount(int count) {
		this.count = count;
	}

}
