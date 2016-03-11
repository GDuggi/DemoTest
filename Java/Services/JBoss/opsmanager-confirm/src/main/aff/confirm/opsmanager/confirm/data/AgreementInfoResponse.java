package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

import java.util.ArrayList;

public class AgreementInfoResponse extends BaseResponse{

	private AgreementInfoRequest request;
	private ArrayList<AgreementData> data;
	
	public AgreementInfoRequest getRequest() {
		return request;
	}
	public void setRequest(AgreementInfoRequest request) {
		this.request = request;
	}
	public ArrayList<AgreementData> getData() {
		return data;
	}
	public void setData(ArrayList<AgreementData> data) {
		this.data = data;
	}
	
}
