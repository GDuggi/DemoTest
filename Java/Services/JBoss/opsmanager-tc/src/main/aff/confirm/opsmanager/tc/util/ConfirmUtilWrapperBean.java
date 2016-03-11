package aff.confirm.opsmanager.tc.util;

import java.util.ArrayList;
import javax.ejb.EJB;
import javax.ejb.Stateless;
import javax.jws.WebMethod;
import javax.jws.WebParam;
import javax.jws.WebService;
import org.jboss.ws.api.annotation.WebContext;
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

@Stateless(name="ConfirmUtilWrapperBean",mappedName="ConfirmUtilWrapperBean",description="Wrapper Bean for utility function")
@WebService(serviceName="ConfirmUtilService",name="ConfirmUtilService",targetNamespace="http://rbssempra.com/confirm")
@WebContext(contextRoot="OpsManager")
public class ConfirmUtilWrapperBean implements ConfirmUtilWrapper{

	@EJB private EConfirm eConfirm;
	@EJB private EFETConfirm efetConfirm;
	@EJB private OpsMgrUtil opsUtil;
	
	

	@WebMethod(operationName="batchSubmitToEconfirm")
	public  ArrayList<EConfirmResponse> batchSubmitToEConfirm(
			@WebParam (name="tradeList") ArrayList<EConfirmRequest> tradeList, 
			@WebParam(name="userName",header=true) String userName) {
		
		return eConfirm.batchSubmit(tradeList, userName);
		
	}
	
	@WebMethod(operationName="batchEConfirmMatch")
	public ArrayList<EConfirmMatchResponse> batchEConfirmMatch(
			@WebParam (name="tradeList") ArrayList<EConfirmMatchRequest> tradeList, 
			@WebParam(name="userName",header=true) String userName) {
		
		return eConfirm.batchMatch(tradeList, userName);
	}

	
	@WebMethod(operationName="batchSubmitToEFET")
	public ArrayList<EFETConfirmResponse> batchSubmitToEEFT(
			@WebParam (name="tradeList") ArrayList<EFETConfirmRequest> tradeList, 
			@WebParam(name="userName",header=true) String userName) {
		 
		return efetConfirm.batchSubmit(tradeList, userName) ;
	}

	
	
	@WebMethod(operationName="getOpsServerEnv")
	public Environment getAppServerEnv(
			@WebParam(name="userName",header=true) String userName)	{
		return opsUtil.getAppServerEnv(userName);
	}

	@WebMethod(operationName="submitToECMBox")
	public ECMBoxSubmitResponse submitToECMBox(
			@WebParam(name="request") ECMBoxSubmitRequest request,
			@WebParam(name="userName",header=true) String userName) {
		
		return efetConfirm.submitToECMBox(request, userName);
		
	}

	@WebMethod(operationName="getUserFilter")
	public UserFilterGetResponse getUserFilter(
			@WebParam(name="request") UserFilterGetRequest request,
			@WebParam(name="userName",header=true) String userName) {

		return opsUtil.getUserFilter(request, userName);
	}

	@WebMethod(operationName="updateUserFilter")
	public UserFilterUpdateResponse updateUserFilter(
			@WebParam(name="request") UserFilterUpdateRequest request, 
			@WebParam(name="userName",header=true) String userName) {
		return opsUtil.updateUserFilter(request, userName);
	}

	@WebMethod(operationName="getUserRoles")
	public UserRoleResponse getUserRoles(
			@WebParam(name="request")  UserRoleRequest request,
			@WebParam(name="userName",header=true) String userName) {
		return opsUtil.getUserRoles(request, userName);
	}

	@WebMethod(operationName="getCdtyGrpCodes")
	public CdtyGrpCodeResponse getCdtyGrpCodes(
			@WebParam(name="userName",header=true)  String userName) {
		return opsUtil.getCdtyGrpCodes(userName);
	}

	@WebMethod(operationName="getUserCompanyList")
	public UserCompanyResponse getUserCompanyList(
			@WebParam(name="request") UserCompanyRequest request) {
		return opsUtil.getCompanyList(request);
	}

	
	
}
