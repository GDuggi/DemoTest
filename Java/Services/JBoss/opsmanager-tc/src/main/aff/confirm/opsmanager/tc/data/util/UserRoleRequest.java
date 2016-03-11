package aff.confirm.opsmanager.tc.data.util;

import java.io.Serializable;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;



@XmlRootElement(name="UserRoleRequest")
public class UserRoleRequest implements Serializable{
	
	private String userId;

	public String getUserId() {
		return userId;
	}

	@XmlElement(name="UserId")
	public void setUserId(String userId) {
		this.userId = userId;
	}
	

}
