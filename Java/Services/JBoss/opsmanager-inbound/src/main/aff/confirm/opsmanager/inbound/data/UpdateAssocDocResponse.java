package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class UpdateAssocDocResponse extends BaseResponse{
	
	private UpdateAssocDocRequest request;

	public UpdateAssocDocRequest getRequest() {
		return request;
	}

	public void setRequest(UpdateAssocDocRequest request) {
		this.request = request;
	}
	

}
