package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;

public class InbAttrMapValueUpdateRequest implements Serializable {
	
	private long id;
	private String inbAttribCode;
	private String mappedValue;
	private String desc;
	private String activeFlag;
	public long getId() {
		return id;
	}
	public void setId(long id) {
		this.id = id;
	}
	public String getInbAttribCode() {
		return inbAttribCode;
	}
	public void setInbAttribCode(String inbAttribCode) {
		this.inbAttribCode = inbAttribCode;
	}
	public String getMappedValue() {
		return mappedValue;
	}
	public void setMappedValue(String mappedValue) {
		this.mappedValue = mappedValue;
	}
	public String getDesc() {
		return desc;
	}
	public void setDesc(String desc) {
		this.desc = desc;
	}
	public String getActiveFlag() {
		return activeFlag;
	}
	public void setActiveFlag(String activeFlag) {
		this.activeFlag = activeFlag;
	}
	

}
