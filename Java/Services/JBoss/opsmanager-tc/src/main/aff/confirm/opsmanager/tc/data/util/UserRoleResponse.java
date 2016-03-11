package aff.confirm.opsmanager.tc.data.util;

import java.util.ArrayList;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;



import aff.confirm.opsmanager.common.data.BaseResponse;

@XmlRootElement(name="UserRoleResponse")
public class UserRoleResponse extends BaseResponse {

	private UserRoleRequest request;
	private ArrayList<UserRoleData> roleList;
	
	public UserRoleRequest getRequest() {
		return request;
	}
	@XmlElement(name="Request")
	public void setRequest(UserRoleRequest request) {
		this.request = request;
	}
	public ArrayList<UserRoleData> getRoleList() {
		return roleList;
	}
	@XmlElement(name="RoleList")
	public void setRoleList(ArrayList<UserRoleData> roleList) {
		this.roleList = roleList;
	}
	
	
}
