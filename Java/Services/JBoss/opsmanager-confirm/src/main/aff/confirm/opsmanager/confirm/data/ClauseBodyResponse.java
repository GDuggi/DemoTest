package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

import java.util.ArrayList;

public class ClauseBodyResponse extends BaseResponse{
	
	private ArrayList<ClauseBodyData> data;

	public ArrayList<ClauseBodyData> getData() {
		return data;
	}

	public void setData(ArrayList<ClauseBodyData> data) {
		this.data = data;
	}

}
