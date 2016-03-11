package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

import java.util.ArrayList;

public class ClauseHeaderResponse extends BaseResponse{
	
	ArrayList<ClauseHeaderData> data;

	public ArrayList<ClauseHeaderData> getData() {
		return data;
	}

	public void setData(ArrayList<ClauseHeaderData> data) {
		this.data = data;
	}

}
