package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class UpdateMapPhraseResponse extends BaseResponse{
	
	private UpdateMapPhraseRequest request;

	public UpdateMapPhraseRequest getRequest() {
		return request;
	}

	public void setRequest(UpdateMapPhraseRequest request) {
		this.request = request;
	}
	

}
