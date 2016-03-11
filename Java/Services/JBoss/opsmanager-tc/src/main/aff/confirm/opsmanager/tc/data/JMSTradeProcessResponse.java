package aff.confirm.opsmanager.tc.data;

import aff.confirm.opsmanager.common.data.BaseResponse;

public class JMSTradeProcessResponse extends BaseResponse{
	
	private JMSTradeProcessRequest request;

	public JMSTradeProcessRequest getRequest() {
		return request;
	}

	public void setRequest(JMSTradeProcessRequest request) {
		this.request = request;
	}

}
