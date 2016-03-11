package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class DocVaultResponse extends BaseResponse{
	
	private DocVaultRequest request;

	public DocVaultRequest getRequest() {
		return request;
	}

	public void setRequest(DocVaultRequest request) {
		this.request = request;
	}
	

}
