package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TradeCompanyResponse extends BaseResponse {
	
	private TradeCompanyRequest request;
	private String companyID;
	
	public TradeCompanyRequest getRequest() {
		return request;
	}
	public void setRequest(TradeCompanyRequest request) {
		this.request = request;
	}
	public String getCompanyID() {
		return companyID;
	}
	public void setCompanyID(String companyID) {
		this.companyID = companyID;
	}
	
	
	


}
