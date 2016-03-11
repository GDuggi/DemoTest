package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class CptyFaxNumberRequest implements Serializable{

	private String cptySn;
	private String cdtyCode;
	private String instrumentType;
	
	public String getCptySn() {
		return cptySn;
	}
	public void setCptySn(String cptySn) {
		this.cptySn = cptySn;
	}
	public String getCdtyCode() {
		return cdtyCode;
	}
	public void setCdtyCode(String cdtyCode) {
		this.cdtyCode = cdtyCode;
	}
	public String getInstrumentType() {
		return instrumentType;
	}
	public void setInstrumentType(String instrumentType) {
		this.instrumentType = instrumentType;
	}
	
	
}
