package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class ClauseBodyData implements Serializable{
	

	private long lutId;
	private long prmntConfirmClauseId;
	private int segment;
	private String body;
	
	
	public long getLutId() {
		return lutId;
	}
	public void setLutId(long lutId) {
		this.lutId = lutId;
	}
	public long getPrmntConfirmClauseId() {
		return prmntConfirmClauseId;
	}
	public void setPrmntConfirmClauseId(long prmntConfirmClauseId) {
		this.prmntConfirmClauseId = prmntConfirmClauseId;
	}
	public int getSegment() {
		return segment;
	}
	public void setSegment(int segment) {
		this.segment = segment;
	}
	public String getBody() {
		return body;
	}
	public void setBody(String body) {
		this.body = body;
	}
	
	
	
}
