package aff.confirm.opsmanager.opsmanagerweb.data;

import java.io.Serializable;

public class FaxReconcileRequest implements Serializable{

	private String startDate;
	private String endDate;
	private String location;
	
	public String getStartDate() {
		return startDate;
	}
	public void setStartDate(String startDate) {
		this.startDate = startDate;
	}
	public String getEndDate() {
		return endDate;
	}
	public void setEndDate(String endDate) {
		this.endDate = endDate;
	}
	public String getLocation() {
		return location;
	}
	public void setLocation(String location) {
		this.location = location;
	}
	
	
}
