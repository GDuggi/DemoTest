package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;

public class AttrMapValueData implements Serializable{

	private long id;
	private String mappedValue;
	private String descr;
	private String attribCode;
	
	
	public long getId() {
		return id;
	}
	public void setId(long id) {
		this.id = id;
	}
	public String getDescr() {
		return descr;
	}
	public void setDescr(String descr) {
		this.descr = descr;
	}
	public String getMappedValue() {
		return mappedValue;
	}
	public void setMappedValue(String mappedValue) {
		this.mappedValue = mappedValue;
	}
	public String getAttribCode() {
		return attribCode;
	}
	public void setAttribCode(String attribCode) {
		this.attribCode = attribCode;
	}
	
	 
	
}
