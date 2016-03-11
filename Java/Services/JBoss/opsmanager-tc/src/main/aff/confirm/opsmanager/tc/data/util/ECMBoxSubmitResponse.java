package aff.confirm.opsmanager.tc.data.util;

import java.io.Serializable;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class ECMBoxSubmitResponse extends BaseResponse implements Serializable{
	
	private ECMBoxSubmitRequest request;

	public ECMBoxSubmitRequest getRequest() {
		return request;
	}

	public void setRequest(ECMBoxSubmitRequest request) {
		this.request = request;
	}
	

}
