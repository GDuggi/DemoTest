package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class InsertClauseRequest implements Serializable{
	
	private String category;
	private String clause;
	private String userId;
	public String getCategory() {
		return category;
	}
	public void setCategory(String category) {
		this.category = category;
	}
	public String getClause() {
		return clause;
	}
	public void setClause(String clause) {
		this.clause = clause;
	}
	public String getUserId() {
		return userId;
	}
	public void setUserId(String userId) {
		this.userId = userId;
	}
	
}
