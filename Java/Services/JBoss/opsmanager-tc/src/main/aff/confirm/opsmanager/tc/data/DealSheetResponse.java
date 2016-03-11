package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class DealSheetResponse extends BaseResponse{
	
	private DealSheetRequest request;
	private String dealSheetHtml;
	
	public DealSheetRequest getRequest() {
		return request;
	}
	public void setRequest(DealSheetRequest request) {
		this.request = request;
	}
	public String getDealSheetHtml() {
		return dealSheetHtml;
	}
	public void setDealSheetHtml(String dealSheetHtml) {
		this.dealSheetHtml = dealSheetHtml;
	}
	

}
