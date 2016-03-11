package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class ClauseHeaderData implements Serializable{

	private long lutId;
	private String quickCode;
	private String shortName;
	private long prmntConfirmClauseId;
	private String category;
	private String tokenString;
	private String comment;
	
	public long getLutId() {
		return lutId;
	}
	public void setLutId(long lutId) {
		this.lutId = lutId;
	}
	public String getQuickCode() {
		return quickCode;
	}
	public void setQuickCode(String quickCode) {
		this.quickCode = quickCode;
	}
	public String getShortName() {
		return shortName;
	}
	public void setShortName(String shortName) {
		this.shortName = shortName;
	}
	public long getPrmntConfirmClauseId() {
		return prmntConfirmClauseId;
	}
	public void setPrmntConfirmClauseId(long prmntConfirmClauseId) {
		this.prmntConfirmClauseId = prmntConfirmClauseId;
	}
	public String getCategory() {
		return category;
	}
	public void setCategory(String category) {
		this.category = category;
	}
	public String getTokenString() {
		return tokenString;
	}
	public void setTokenString(String tokenString) {
		this.tokenString = tokenString;
	}
	public String getComment() {
		return comment;
	}
	public void setComment(String comment) {
		this.comment = comment;
	}
	
}
