package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;


public class DocStatusRequest implements Serializable {

	private long id;
	private String statusCode;
	
	public long getId() {
		return id;
	}
	
	public void setId(long id) {
		this.id = id;
	}

	public String getStatusCode() {
		return statusCode;
	}

	public void setStatusCode(String statusCode) {
		this.statusCode = statusCode;
	}
	
}
