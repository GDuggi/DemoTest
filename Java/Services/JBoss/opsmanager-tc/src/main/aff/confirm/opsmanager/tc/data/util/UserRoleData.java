package aff.confirm.opsmanager.tc.data.util;

import javax.xml.bind.annotation.*;
import java.io.Serializable;


@XmlRootElement(name="UserRoleData")
public class UserRoleData implements Serializable{

	private String userId;
	private String role;
	private String description;
	
	public String getUserId() {
		return userId;
	}
	@XmlElement(name="UserId")
	public void setUserId(String userId) {
		this.userId = userId;
	}
	public String getRole() {
		return role;
	}
	
	@XmlElement(name="Role")
	public void setRole(String role) {
		this.role = role;
	}
	public String getDescription() {
		return description;
	}
	@XmlElement(name="Description")
	public void setDescription(String description) {
		this.description = description;
	}
	
}
