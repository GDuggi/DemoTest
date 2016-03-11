package aff.confirm.opsmanager.tc.data.util;

import java.io.Serializable;

public class UserFilterGetRequest  implements Serializable{
	
	private String userId;

	public String getUserId() {
		return userId;
	}

	public void setUserId(String userId) {
		this.userId = userId;
	}
	

}
