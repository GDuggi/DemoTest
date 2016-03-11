package aff.confirm.opsmanager.opsmanagerweb.data;

import java.util.ArrayList;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TraderGetResponse extends BaseResponse{

	private TraderGetRequest request;
	private ArrayList<TraderGetData> data;
	public TraderGetRequest getRequest() {
		return request;
	}
	public void setRequest(TraderGetRequest request) {
		this.request = request;
	}
	public ArrayList<TraderGetData> getData() {
		return data;
	}
	public void setData(ArrayList<TraderGetData> data) {
		this.data = data;
	}
	
}
