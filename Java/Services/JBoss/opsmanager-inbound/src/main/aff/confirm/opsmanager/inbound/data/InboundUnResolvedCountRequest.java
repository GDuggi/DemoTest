package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;

public class InboundUnResolvedCountRequest implements Serializable{

	private long inboundDocId;

	public long getInboundDocId() {
		return inboundDocId;
	}

	public void setInboundDocId(long inboundDocId) {
		this.inboundDocId = inboundDocId;
	}
	
}
