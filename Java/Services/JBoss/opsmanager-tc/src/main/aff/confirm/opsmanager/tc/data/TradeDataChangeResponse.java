package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;
import java.util.ArrayList;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class TradeDataChangeResponse extends BaseResponse implements Serializable{
	
	private ArrayList<TradeDataChange> tradeDataChange;
	private TradeDataChangeRequest request;
	

	public ArrayList<TradeDataChange> getTradeDataChange() {
		return tradeDataChange;
	}

	public void setTradeDataChange(
			ArrayList<TradeDataChange> tradeCorrectionData) {
		this.tradeDataChange = tradeCorrectionData;
	}

	public TradeDataChangeRequest getRequest() {
		return request;
	}

	public void setRequest(TradeDataChangeRequest request) {
		this.request = request;
	}
	

}
