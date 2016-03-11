package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

import java.util.ArrayList;

public class SearchContractResponse extends BaseResponse{
	
	private SearchContractRequest request;
	private ArrayList<SearchContractData> data;
	public SearchContractRequest getRequest() {
		return request;
	}
	public void setRequest(SearchContractRequest request) {
		this.request = request;
	}
	public ArrayList<SearchContractData> getData() {
		return data;
	}
	public void setData(ArrayList<SearchContractData> data) {
		this.data = data;
	}
	

}
