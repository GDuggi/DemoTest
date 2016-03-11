package aff.confirm.opsmanager.common.data;

import java.io.Serializable;

public class BaseResponse implements Serializable {

	public static final String SUCCESS = "OK";
	public static final String ERROR = "ERROR";
	public static final String WARN = "WARN";
	
	private String responseStatus;
	private String responseText;
	private String responseStack;
	
	public String getResponseStatus() {
		return responseStatus;
	}
	public void setResponseStatus(String responseStatus) {
		this.responseStatus = responseStatus;
	}
	public String getResponseText() {
		return responseText;
	}
	public void setResponseText(String responseText) {
		this.responseText = responseText;
	}
	public String getResponseStackError() {
		return responseStack;
	}
	public void setResponseStackError(String responseStack) {
		this.responseStack = responseStack;
	}
	
}
