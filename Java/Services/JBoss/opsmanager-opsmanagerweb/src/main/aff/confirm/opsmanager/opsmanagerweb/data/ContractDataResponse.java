package aff.confirm.opsmanager.opsmanagerweb.data;
import aff.confirm.opsmanager.common.data.BaseResponse;

public class ContractDataResponse extends BaseResponse {

	private ContractDataRequest request;
	private String contractData;
	
	public ContractDataRequest getRequest() {
		return request;
	}
	public void setRequest(ContractDataRequest request) {
		this.request = request;
	}
	public String getContractData() {
		return contractData;
	}
	public void setContractData(String contractData) {
		this.contractData = contractData;
	}
	
}
