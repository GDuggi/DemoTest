package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class InboundAttribMapRequest implements Serializable{
	

	private String attribCode;
	private String phrase;
	private String ourValue;
	
	public String getAttribCode() {
		return attribCode;
	}
	public void setAttribCode(String attribCode) {
		this.attribCode = attribCode;
	}
	public String getPhrase() {
		return phrase;
	}
	public void setPhrase(String phrase) {
		this.phrase = phrase;
	}
	public String getOurValue() {
		return ourValue;
	}
	public void setOurValue(String ourValue) {
		this.ourValue = ourValue;
	}
	
}
