package aff.confirm.opsmanager.tc.util;

import javax.ejb.Remote;
import java.util.*;

import aff.confirm.opsmanager.tc.data.util.CdtyGrpCodeResponse;
import aff.confirm.opsmanager.tc.data.util.ECMBoxSubmitRequest;
import aff.confirm.opsmanager.tc.data.util.ECMBoxSubmitResponse;
import aff.confirm.opsmanager.tc.data.util.EConfirmMatchRequest;
import aff.confirm.opsmanager.tc.data.util.EConfirmMatchResponse;
import aff.confirm.opsmanager.tc.data.util.EConfirmRequest;
import aff.confirm.opsmanager.tc.data.util.EConfirmResponse;
import aff.confirm.opsmanager.tc.data.util.EFETConfirmRequest;
import aff.confirm.opsmanager.tc.data.util.EFETConfirmResponse;
import aff.confirm.opsmanager.tc.data.util.Environment;
import aff.confirm.opsmanager.tc.data.util.UserCompanyRequest;
import aff.confirm.opsmanager.tc.data.util.UserCompanyResponse;
import aff.confirm.opsmanager.tc.data.util.UserFilterGetRequest;
import aff.confirm.opsmanager.tc.data.util.UserFilterGetResponse;
import aff.confirm.opsmanager.tc.data.util.UserFilterUpdateRequest;
import aff.confirm.opsmanager.tc.data.util.UserFilterUpdateResponse;
import aff.confirm.opsmanager.tc.data.util.UserRoleRequest;
import aff.confirm.opsmanager.tc.data.util.UserRoleResponse;


@Remote
public interface ConfirmUtilWrapper {

	ArrayList<EConfirmResponse> batchSubmitToEConfirm(ArrayList<EConfirmRequest> tradeList,String userName);
	ArrayList<EConfirmMatchResponse> batchEConfirmMatch(ArrayList<EConfirmMatchRequest> tradeList,String userName);
	ArrayList<EFETConfirmResponse> batchSubmitToEEFT(ArrayList<EFETConfirmRequest> tradeList,String userName);
	Environment getAppServerEnv(String userName);
	ECMBoxSubmitResponse submitToECMBox(ECMBoxSubmitRequest request, String userName);
	UserFilterUpdateResponse updateUserFilter(UserFilterUpdateRequest request,String userName);
	UserFilterGetResponse getUserFilter(UserFilterGetRequest request,String userName);
	UserRoleResponse getUserRoles(UserRoleRequest request,String userName);
	CdtyGrpCodeResponse getCdtyGrpCodes(String userName);
	UserCompanyResponse getUserCompanyList(UserCompanyRequest request);
	
}
