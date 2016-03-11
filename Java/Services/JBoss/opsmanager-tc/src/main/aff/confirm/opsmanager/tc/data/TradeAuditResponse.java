package aff.confirm.opsmanager.tc.data;

import java.util.ArrayList;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TradeAuditResponse extends BaseResponse{

	private TradeAuditRequest request;
	private ArrayList<TradeAuditData> auditData;
	
	public TradeAuditRequest getRequest() {
		return request;
	}
	public void setRequest(TradeAuditRequest request) {
		this.request = request;
	}
	public ArrayList<TradeAuditData> getAuditData() {
		return auditData;
	}
	public void setAuditData(ArrayList<TradeAuditData> auditData) {
		this.auditData = auditData;
	}
	
	
}
