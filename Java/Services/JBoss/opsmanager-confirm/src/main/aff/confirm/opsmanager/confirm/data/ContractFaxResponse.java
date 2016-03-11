package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

import java.util.ArrayList;

public class ContractFaxResponse extends BaseResponse{
	
	private ContractFaxRequest request;
	private ArrayList<ContractFaxData> data;
	
	public ContractFaxRequest getRequest() {
		return request;
	}
	public void setRequest(ContractFaxRequest request) {
		this.request = request;
	}
	public ArrayList<ContractFaxData> getData() {
		return data;
	}
	public void setData(ArrayList<ContractFaxData> data) {
		this.data = data;
	}
	

}
