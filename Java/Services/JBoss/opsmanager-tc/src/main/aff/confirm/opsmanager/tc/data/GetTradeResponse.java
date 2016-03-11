package aff.confirm.opsmanager.tc.data;

import java.util.ArrayList;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class GetTradeResponse extends BaseResponse{
	
	
	private ArrayList<TradeSummary> trades;
	private ArrayList<TradeRqmt> rqmts;
	private ArrayList<TradeRqmtConfirm> confirms;
	private ArrayList<AssociatedDoc> associatedDocs;
	
	
	private GetTradeRequest request;
	
	public ArrayList<TradeSummary> getTrades() {
		return trades;
	}
	public void setTrades(ArrayList<TradeSummary> trades) {
		this.trades = trades;
	}
	public GetTradeRequest getRequest() {
		return request;
	}
	public void setRequest(GetTradeRequest request) {
		this.request = request;
	}
	public ArrayList<TradeRqmt> getRqmts() {
		return rqmts;
	}
	public void setRqmts(ArrayList<TradeRqmt> rqmts) {
		this.rqmts = rqmts;
	}
	public ArrayList<TradeRqmtConfirm> getConfirms() {
		return confirms;
	}
	public void setConfirms(ArrayList<TradeRqmtConfirm> confirms) {
		this.confirms = confirms;
	}
	public ArrayList<AssociatedDoc> getAssociatedDocs() {
		return associatedDocs;
	}
	public void setAssociatedDocs(ArrayList<AssociatedDoc> associatedDocs) {
		this.associatedDocs = associatedDocs;
	}
	
}
