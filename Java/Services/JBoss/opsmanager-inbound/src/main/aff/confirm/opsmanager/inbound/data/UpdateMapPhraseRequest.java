package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;

public class UpdateMapPhraseRequest implements Serializable{

	private long id;
	private long inbAttribMapValId;
	private String phrase;
	private String activeFlag;
	
	public long getId() {
		return id;
	}
	public void setId(long id) {
		this.id = id;
	}
	public long getInbAttribMapValId() {
		return inbAttribMapValId;
	}
	public void setInbAttribMapValId(long inbAttribMapValId) {
		this.inbAttribMapValId = inbAttribMapValId;
	}
	public String getPhrase() {
		return phrase;
	}
	public void setPhrase(String phrase) {
		this.phrase = phrase;
	}
	public String getActiveFlag() {
		return activeFlag;
	}
	public void setActiveFlag(String activeFlag) {
		this.activeFlag = activeFlag;
	}
	
}
