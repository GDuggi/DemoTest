package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;

public class AttrMapPhraseRequest implements Serializable{

	
	private String inbAttribMapValue ;
	
	private long inboundAttribMapValId;
	

	public String getInbAttribMapValue() {
		return inbAttribMapValue;
	}

	public void setInbAttribMapValue(String inbAttribMapValue) {
		this.inbAttribMapValue = inbAttribMapValue;
	}

	public long getInboundAttribMapValId() {
		return inboundAttribMapValId;
	}

	public void setInboundAttribMapValId(long inboundAttribMapValId) {
		this.inboundAttribMapValId = inboundAttribMapValId;
	}

	
	
}
