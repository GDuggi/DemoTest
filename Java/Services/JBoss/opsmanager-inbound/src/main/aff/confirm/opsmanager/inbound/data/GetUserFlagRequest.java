package aff.confirm.opsmanager.inbound.data;

import java.io.Serializable;


public class GetUserFlagRequest implements Serializable{
	
	private String userName;

	public String getUserName() {
		return userName;
	}

	public void setUserName(String userName) {
		this.userName = userName;
	}
	

}
