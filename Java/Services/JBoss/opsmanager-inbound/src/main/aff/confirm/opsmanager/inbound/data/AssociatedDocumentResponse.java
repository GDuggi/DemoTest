package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;



public class AssociatedDocumentResponse extends BaseResponse{


	private AssociatedDocumentRequest request;

	public AssociatedDocumentRequest getRequest() {
		return request;
	}

	public void setRequest(AssociatedDocumentRequest request) {
		this.request = request;
	}
	
}
