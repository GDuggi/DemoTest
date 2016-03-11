package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class InfMgrFaxResponse extends BaseResponse{
	
	private String faxNumber;
	private InfMgrFaxrequest request;
	
	public String getFaxNumber() {
		return faxNumber;
	}
	public void setFaxNumber(String faxNumber) {
		this.faxNumber = faxNumber;
	}
	public InfMgrFaxrequest getRequest() {
		return request;
	}
	public void setRequest(InfMgrFaxrequest request) {
		this.request = request;
	}
	

}
