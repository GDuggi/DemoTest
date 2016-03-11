package aff.confirm.opsmanager.confirm.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

import java.util.ArrayList;

public class DesignationResponse extends BaseResponse{

	private ArrayList<DesignationData> data;

	public ArrayList<DesignationData> getData() {
		return data;
	}

	public void setData(ArrayList<DesignationData> data) {
		this.data = data;
	}
	
}
