package aff.confirm.opsmanager.tc.data.util;

import java.io.Serializable;

public class UserFilterData implements Serializable{
	
	private int filterId;
	private String userId;
	private String description;
	private String filterExpr;
	public int getFilterId() {
		return filterId;
	}
	public void setFilterId(int filterId) {
		this.filterId = filterId;
	}
	public String getUserId() {
		return userId;
	}
	public void setUserId(String userId) {
		this.userId = userId;
	}
	public String getDescription() {
		return description;
	}
	public void setDescription(String description) {
		this.description = description;
	}
	public String getFilterExpr() {
		return filterExpr;
	}
	public void setFilterExpr(String filterExpr) {
		this.filterExpr = filterExpr;
	}
	

}
