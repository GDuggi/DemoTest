package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class TradeCommentRequest implements Serializable{

	private long tradeId;
	private String comment;
	
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public String getComment() {
		return comment;
	}
	public void setComment(String comment) {
		this.comment = comment;
	}
	
	
}
