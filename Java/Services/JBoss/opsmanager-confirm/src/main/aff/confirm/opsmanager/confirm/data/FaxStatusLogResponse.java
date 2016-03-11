package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

import java.util.ArrayList;

public class FaxStatusLogResponse extends BaseResponse{
	
	private FaxStatusLogRequest request;
	private ArrayList<FaxStatusLogData> data;
	
	public FaxStatusLogRequest getRequest() {
		return request;
	}
	public void setRequest(FaxStatusLogRequest request) {
		this.request = request;
	}
	public ArrayList<FaxStatusLogData> getData() {
		return data;
	}
	public void setData(ArrayList<FaxStatusLogData> data) {
		this.data = data;
	}
	
}
