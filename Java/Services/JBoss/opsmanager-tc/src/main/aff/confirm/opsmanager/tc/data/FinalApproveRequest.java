package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class FinalApproveRequest implements Serializable {

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private long tradeId;
	private String approvalFlag;
	private String onlyIfReadyFlag;
	
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public String getApprovalFlag() {
		return approvalFlag;
	}
	public void setApprovalFlag(String approvalFlag) {
		this.approvalFlag = approvalFlag;
	}
	public String getOnlyIfReadyFlag() {
		return onlyIfReadyFlag;
	}
	public void setOnlyIfReadyFlag(String onlyIfReadyFlag) {
		this.onlyIfReadyFlag = onlyIfReadyFlag;
	}
	
	

	
}
