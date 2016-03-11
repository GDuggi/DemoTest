package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

import java.util.ArrayList;

public class AttrMapValueResponse extends BaseResponse {

	private AttrMapValueRequest request;
	private ArrayList<AttrMapValueData> data;
	
	public AttrMapValueRequest getRequest() {
		return request;
	}
	public void setRequest(AttrMapValueRequest request) {
		this.request = request;
	}
	public ArrayList<AttrMapValueData> getData() {
		return data;
	}
	public void setData(ArrayList<AttrMapValueData> data) {
		this.data = data;
	}
	
	
}
