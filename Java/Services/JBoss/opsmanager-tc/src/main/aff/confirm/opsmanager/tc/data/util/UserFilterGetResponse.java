package aff.confirm.opsmanager.tc.data.util;

import java.util.ArrayList;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class UserFilterGetResponse extends BaseResponse {
	
	private UserFilterGetRequest request;
	private ArrayList<UserFilterData> userFilters;
	
	public UserFilterGetRequest getRequest() {
		return request;
	}
	public void setRequest(UserFilterGetRequest request) {
		this.request = request;
	}
	public ArrayList<UserFilterData> getUserFilters() {
		return userFilters;
	}
	public void setUserFilters(ArrayList<UserFilterData> userFilters) {
		this.userFilters = userFilters;
	}

}
