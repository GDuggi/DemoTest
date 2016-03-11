package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;

public class AttrMapPhraseData implements Serializable{

	private long pharseid;
	private long inbAttribMapValueId;
	private String phrase;
	private String descr;
	
	
	
	public long getInbAttribMapValueId() {
		return inbAttribMapValueId;
	}
	public void setInbAttribMapValueId(long inbAttribMapValueId) {
		this.inbAttribMapValueId = inbAttribMapValueId;
	}
	public String getPhrase() {
		return phrase;
	}
	public void setPhrase(String phrase) {
		this.phrase = phrase;
	}
	public long getPharseid() {
		return pharseid;
	}
	public void setPharseid(long pharseid) {
		this.pharseid = pharseid;
	}
	public String getDescr() {
		return descr;
	}
	public void setDescr(String descr) {
		this.descr = descr;
	}
	
	
}
