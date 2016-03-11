package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

import java.util.ArrayList;


public class GetUserFlagResponse extends BaseResponse{

	private GetUserFlagRequest request;
	private ArrayList<GetUserFlagData> userFlagData  ;
	
	public GetUserFlagRequest getRequest() {
		return request;
	}
	public void setRequest(GetUserFlagRequest request) {
		this.request = request;
	}
	public ArrayList<GetUserFlagData> getUserFlagData() {
		return userFlagData;
	}
	public void setUserFlagData(ArrayList<GetUserFlagData> userFlagData) {
		this.userFlagData = userFlagData;
	}
	
	
}
