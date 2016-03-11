package aff.confirm.opsmanager.tc.data.util;

import java.util.ArrayList;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class UserCompanyResponse extends BaseResponse{
	
	private UserCompanyRequest request;
	private ArrayList<String> companyList;
	private String accessIndicator;
	private ArrayList<String> migrateIndicator;
	
	public UserCompanyRequest getRequest() {
		return request;
	}
	public void setRequest(UserCompanyRequest request) {
		this.request = request;
	}
	public ArrayList<String> getCompanyList() {
		return companyList;
	}
	public void setCompanyList(ArrayList<String> companyList) {
		this.companyList = companyList;
	}
	public String getAccessIndicator() {
		return accessIndicator;
	}
	public void setAccessIndicator(String accessIndicator) {
		this.accessIndicator = accessIndicator;
	}
	
	public ArrayList<String> getMigrateIndicator() {
		return migrateIndicator;
	}
	public void setMigrateIndicator(ArrayList<String> migrateIndicator) {
		this.migrateIndicator = migrateIndicator;
	}
	
	

}
