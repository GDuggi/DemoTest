package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;

public class InboundDocCountRequest implements Serializable{

	private long inboundDocId;
	private String docStatus;
	
	public long getInboundDocId() {
		return inboundDocId;
	}
	public void setInboundDocId(long inboundDocId) {
		this.inboundDocId = inboundDocId;
	}
	public String getDocStatus() {
		return docStatus;
	}
	public void setDocStatus(String docStatus) {
		this.docStatus = docStatus;
	}
	
}
