package aff.confirm.opsmanager.tc.data.util;

import java.io.Serializable;

public class UserFilterUpdateRequest implements Serializable {

	public enum _request_type  { Insert,Update,Delete}
	
	private String userId;
	private String Description;
	private String filterExpr;
	private int filterId;
	private _request_type requestType; 
	
	public String getUserId() {
		return userId;
	}
	public void setUserId(String userId) {
		this.userId = userId;
	}
	public String getDescription() {
		return Description;
	}
	public void setDescription(String description) {
		Description = description;
	}
	public String getFilterExpr() {
		return filterExpr;
	}
	public void setFilterExpr(String filterExpr) {
		this.filterExpr = filterExpr;
	}
	public int getFilterId() {
		return filterId;
	}
	public void setFilterId(int filterId) {
		this.filterId = filterId;
	}
	public _request_type getRequestType() {
		return requestType;
	}
	public void setRequestType(_request_type requestType) {
		this.requestType = requestType;
	}
	
}
