package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;

public class MapCallerReferenceRequest implements Serializable{

	private String callerRef;
	private String cptySn;
	private String refType;
	
	
	public String getCallerRef() {
		return callerRef;
	}
	public void setCallerRef(String callerRef) {
		this.callerRef = callerRef;
	}
	public String getCptySn() {
		return cptySn;
	}
	public void setCptySn(String cptySn) {
		this.cptySn = cptySn;
	}
	public String getRefType() {
		return refType;
	}
	public void setRefType(String refType) {
		this.refType = refType;
	}
	
}
