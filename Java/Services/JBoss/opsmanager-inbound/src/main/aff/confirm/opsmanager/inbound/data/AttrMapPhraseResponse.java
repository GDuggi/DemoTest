package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

import java.util.ArrayList;

public class AttrMapPhraseResponse extends BaseResponse{

	private AttrMapPhraseRequest request;
	private ArrayList<AttrMapPhraseData> data;

	public AttrMapPhraseRequest getRequest() {
		return request;
	}

	public void setRequest(AttrMapPhraseRequest request) {
		this.request = request;
	}

	public ArrayList<AttrMapPhraseData> getData() {
		return data;
	}

	public void setData(ArrayList<AttrMapPhraseData> data) {
		this.data = data;
	}
	
	
}
