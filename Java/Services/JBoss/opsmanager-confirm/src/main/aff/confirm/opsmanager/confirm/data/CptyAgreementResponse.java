package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class CptyAgreementResponse extends BaseResponse{

	private CptyAgreementRequest request;
	private String agreementData;
	public CptyAgreementRequest getRequest() {
		return request;
	}
	public void setRequest(CptyAgreementRequest request) {
		this.request = request;
	}
	public String getAgreementData() {
		return agreementData;
	}
	public void setAgreementData(String agreementData) {
		this.agreementData = agreementData;
	}
	
}
