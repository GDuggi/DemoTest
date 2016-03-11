package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;


public class DocStatusResponse extends BaseResponse{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private DocStatusRequest request;

	public DocStatusRequest getRequest() {
		return request;
	}

	public void setRequest(DocStatusRequest request) {
		this.request = request;
	}
	
	

}
