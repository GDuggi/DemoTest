package aff.confirm.opsmanager.opsmanagerweb.data;

import java.util.ArrayList;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class FaxReconcileResponse extends BaseResponse {
	
	private FaxReconcileRequest request;
	private int faxCount;
	private int opsManagerCount;
	private ArrayList<FaxReconcileData> data;
	
	public FaxReconcileRequest getRequest() {
		return request;
	}
	public void setRequest(FaxReconcileRequest request) {
		this.request = request;
	}
	public int getFaxCount() {
		return faxCount;
	}
	public void setFaxCount(int faxCount) {
		this.faxCount = faxCount;
	}
	public int getOpsManagerCount() {
		return opsManagerCount;
	}
	public void setOpsManagerCount(int opsManagerCount) {
		this.opsManagerCount = opsManagerCount;
	}
	public ArrayList<FaxReconcileData> getData() {
		return data;
	}
	public void setData(ArrayList<FaxReconcileData> data) {
		this.data = data;
	}

	
	

}
