package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;

public class AttrMapValueRequest implements Serializable{
	
	private String inboundAttributeCode;
	
	private String mappedValue ;

	public String getInboundAttributeCode() {
		return inboundAttributeCode;
	}

	public void setInboundAttributeCode(String inboundAttributeCode) {
		this.inboundAttributeCode = inboundAttributeCode;
	}

	public String getMappedValue() {
		return mappedValue;
	}

	public void setMappedValue(String mappedValue) {
		this.mappedValue = mappedValue;
	}
	

}
