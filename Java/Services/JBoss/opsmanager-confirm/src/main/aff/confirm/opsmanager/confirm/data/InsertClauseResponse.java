package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class InsertClauseResponse extends BaseResponse{

	
	private InsertClauseRequest request;

	public InsertClauseRequest getRequest() {
		return request;
	}

	public void setRequest(InsertClauseRequest request) {
		this.request = request;
	}
}
