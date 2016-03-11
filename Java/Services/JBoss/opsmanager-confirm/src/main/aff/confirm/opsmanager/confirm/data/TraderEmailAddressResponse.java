package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TraderEmailAddressResponse extends BaseResponse {
	

	private TraderEmailAddressRequest request;
	private String emailAddress;
	
	public TraderEmailAddressRequest getRequest() {
		return request;
	}
	public void setRequest(TraderEmailAddressRequest request) {
		this.request = request;
	}
	public String getEmailAddress() {
		return emailAddress;
	}
	public void setEmailAddress(String emailAddress) {
		this.emailAddress = emailAddress;
	}
	
}
