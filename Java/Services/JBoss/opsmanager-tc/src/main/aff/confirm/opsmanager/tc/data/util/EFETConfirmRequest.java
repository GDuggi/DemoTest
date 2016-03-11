package aff.confirm.opsmanager.tc.data.util;

import java.io.Serializable;



public class EFETConfirmRequest implements Serializable{

	/**
	 * 
	 */
	
	public enum _Entity_Type  {Cpty,Broker,Both};
	
	private static final long serialVersionUID = -1712253538021674587L;
	
	private long tradeId;
	private String status;
	private _Entity_Type entityType;  
	
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public void setEntityType(_Entity_Type type) {
		this.entityType = type;
	}
	public _Entity_Type getEntityType() {
		return entityType ;
	}
	public String getStatus() {
		return status;
	}
	public void setStatus(String status) {
		this.status = status;
	}
	

	
}
