package aff.confirm.opsmanager.tc.data;

import java.util.ArrayList;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class InboundAttribResponse extends BaseResponse{
	
	private ArrayList<InboundAttrib> data;

	public ArrayList<InboundAttrib> getData() {
		return data;
	}

	public void setData(ArrayList<InboundAttrib> data) {
		this.data = data;
	}
	
	
	
}
