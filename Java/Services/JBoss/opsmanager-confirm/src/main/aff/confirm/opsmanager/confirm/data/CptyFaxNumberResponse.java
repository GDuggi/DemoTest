package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class CptyFaxNumberResponse extends BaseResponse{
	
	private CptyFaxNumberRequest request;
	
	private String localNumber;
	private String phoneTypeCode;
	private String phoneTypeInd;
	private String countryCode;
	private String areaCode;
	

	public CptyFaxNumberRequest getRequest() {
		return request;
	}

	public void setRequest(CptyFaxNumberRequest request) {
		this.request = request;
	}

	public String getLocalNumber() {
		return localNumber;
	}

	public void setLocalNumber(String cptyFaxNumber) {
		this.localNumber = cptyFaxNumber;
	}

	public String getPhoneTypeCode() {
		return phoneTypeCode;
	}

	public void setPhoneTypeCode(String phoneTypeCode) {
		this.phoneTypeCode = phoneTypeCode;
	}

	public String getPhoneTypeInd() {
		return phoneTypeInd;
	}

	public void setPhoneTypeInd(String phoneTypeInd) {
		this.phoneTypeInd = phoneTypeInd;
	}

	public String getCountryCode() {
		return countryCode;
	}

	public void setCountryCode(String countryCode) {
		this.countryCode = countryCode;
	}

	public String getAreaCode() {
		return areaCode;
	}

	public void setAreaCode(String areaCode) {
		this.areaCode = areaCode;
	}
	

}
