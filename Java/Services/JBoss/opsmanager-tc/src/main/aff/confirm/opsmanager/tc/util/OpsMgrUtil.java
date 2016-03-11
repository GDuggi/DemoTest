package aff.confirm.opsmanager.tc.util;

import javax.ejb.Remote;

import aff.confirm.opsmanager.tc.data.util.*;

@Remote
public interface OpsMgrUtil {

	Environment getAppServerEnv(String userName);
	UserFilterUpdateResponse updateUserFilter(UserFilterUpdateRequest request,String userName);
	UserFilterGetResponse getUserFilter(UserFilterGetRequest request,String userName);
	UserRoleResponse getUserRoles(UserRoleRequest request,String userName);
	CdtyGrpCodeResponse getCdtyGrpCodes(String userName);
	UserCompanyResponse getCompanyList(UserCompanyRequest request);
	
}
