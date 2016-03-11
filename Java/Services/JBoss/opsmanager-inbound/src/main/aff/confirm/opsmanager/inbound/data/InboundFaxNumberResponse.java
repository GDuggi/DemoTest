package aff.confirm.opsmanager.inbound.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

import java.util.ArrayList;

public class InboundFaxNumberResponse extends BaseResponse{

	private ArrayList<String> faxNumbers;

	public ArrayList<String> getFaxNumbers() {
		return faxNumbers;
	}

	public void setFaxNumbers(ArrayList<String> faxNumbers) {
		this.faxNumbers = faxNumbers;
	}
	
	
}
