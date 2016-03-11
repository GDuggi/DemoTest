package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class ContractResponse extends BaseResponse{
	
	
	private ContractRequest request;
	private String contract;

	public ContractRequest getRequest() {
		return request;
	}

	public void setRequest(ContractRequest request) {
		this.request = request;
	}

	public String getContract() {
		return contract;
	}

	public void setContract(String contract) {
		this.contract = contract;
	}
	

}
