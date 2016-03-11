package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class IndexCountResponse extends BaseResponse{
	
	private IndexCountRequest request;
	private int currentIndexValue;
	
	public IndexCountRequest getRequest() {
		return request;
	}
	public void setRequest(IndexCountRequest request) {
		this.request = request;
	}
	public int getCurrentIndexValue() {
		return currentIndexValue;
	}
	public void setCurrentIndexValue(int currentIndexValue) {
		this.currentIndexValue = currentIndexValue;
	}
	

}
