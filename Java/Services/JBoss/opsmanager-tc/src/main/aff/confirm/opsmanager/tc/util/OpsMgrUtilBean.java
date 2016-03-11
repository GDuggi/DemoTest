package aff.confirm.opsmanager.tc.util;

import java.sql.SQLException;
import javax.ejb.EJBException;
import javax.ejb.Stateless;
import javax.naming.NamingException;
import org.jboss.logging.Logger;
import aff.confirm.opsmanager.common.BaseOpsBean;
import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.tc.common.util.TCUtilProcessor;
import aff.confirm.opsmanager.tc.data.util.CdtyGrpCodeResponse;
import aff.confirm.opsmanager.tc.data.util.Environment;
import aff.confirm.opsmanager.tc.data.util.UserCompanyRequest;
import aff.confirm.opsmanager.tc.data.util.UserCompanyResponse;
import aff.confirm.opsmanager.tc.data.util.UserFilterGetRequest;
import aff.confirm.opsmanager.tc.data.util.UserFilterGetResponse;
import aff.confirm.opsmanager.tc.data.util.UserFilterUpdateRequest;
import aff.confirm.opsmanager.tc.data.util.UserFilterUpdateResponse;
import aff.confirm.opsmanager.tc.data.util.UserRoleRequest;
import aff.confirm.opsmanager.tc.data.util.UserRoleResponse;

@Stateless(name="OpsServerEnv",mappedName="OpsServerEnv")
public class OpsMgrUtilBean extends BaseOpsBean implements OpsMgrUtil {
	private static Logger log = Logger.getLogger( OpsMgrUtilBean.class);
	
	TCUtilProcessor processor;
	public OpsMgrUtilBean() throws NamingException {
		super();
		processor = new TCUtilProcessor(this.affinityConnection);
	}


	public Environment getAppServerEnv(String userName) {
		
		log.info("User(" + userName + ") getAppServEnv called");
		Environment env = new Environment();
		env.setResponseStatus(BaseResponse.SUCCESS);
		env.setAffinityDatabase(this.affinityDatabase);
		log.info("User(" + userName + ") getAppServEnv data returned");
		return env;
	}

	
	public UserFilterUpdateResponse updateUserFilter(
			UserFilterUpdateRequest request,String userName) {
		
		UserFilterUpdateResponse response = null;
		log.info("User(" + userName + ") updateUserFilter called");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			response= processor.updateUserFilter(request);
		}
		catch ( SQLException e){
			log.error("User(" + userName + ") updateUserFilter error : ", e);
			throw new EJBException(e);
		}
		catch (Exception e){
			log.error("User(" + userName + ") updateUserFilter error : ", e);
			response = new UserFilterUpdateResponse();
			OpsManagerUtil.populateErrorMessage(response,e);
		}
		log.info("User(" + userName + ") updateUserFilter returned");
		return response;

	}


	public UserFilterGetResponse getUserFilter(UserFilterGetRequest request,String userName) {
		
		UserFilterGetResponse response = null;
		log.info("User(" + userName + ") getUserFilter called");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			response= processor.getUserFilter(request);
		}
		catch ( SQLException e){
			log.error("User(" + userName + ") getUserFilter error : ", e);
			throw new EJBException(e);
		}
		catch (Exception e){
			log.error("User(" + userName + ") getUserFilter error : ", e);
			response = new UserFilterGetResponse();
			OpsManagerUtil.populateErrorMessage(response,e);
		}
		log.info("User(" + userName + ") getUserFilter returned");
		return response;
	}


	public UserRoleResponse getUserRoles(UserRoleRequest request,
			String userName) {

		UserRoleResponse response = null;
		log.info("User(" + userName + ") getUserRoles called with param value = " + request.getUserId());
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			response= processor.getUserRoles(request);
		}
		catch ( SQLException e){
			log.error("User(" + userName + ") getUserRoles error : ", e);
			throw new EJBException(e);
		}
		catch (Exception e){
			log.error("User(" + userName + ") getUserRoles error : ", e);
			response = new UserRoleResponse();
			OpsManagerUtil.populateErrorMessage(response,e);
		}
		log.info("User(" + userName + ") getUserRoles returned");
		return response;
	}
	@Override
	public void prepareForMethodCall() {
		processor.setDbConnection(this.affinityConnection);
	}


	public CdtyGrpCodeResponse getCdtyGrpCodes(String userName) {
		CdtyGrpCodeResponse response = null;
		log.info("User(" + userName + ") getCptyGrpCodes called ");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			response= processor.getCdtyGrpCodes();
		}
		catch ( SQLException e){
			log.error("User(" + userName + ") getCptyGrpCodes error : ", e);
			throw new EJBException(e);
		}
		catch (Exception e){
			log.error("User(" + userName + ") getCptyGrpCodes error : ", e);
			response = new CdtyGrpCodeResponse();
			OpsManagerUtil.populateErrorMessage(response,e);
		}
		log.info("User(" + userName + ") getCptyGrpCodes returned");
		return response;
	}


	public UserCompanyResponse getCompanyList(UserCompanyRequest request) {
		
		UserCompanyResponse response = new UserCompanyResponse();
		
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
		}
		log.info("User(" + request.getUserName() + ") getCompanyList called ");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, request.getUserName());
			response= processor.getUserCompanyList(request);
		}
		catch ( SQLException e){
			log.error("User(" + request.getUserName() + ") getCompanyList error : ", e);
			throw new EJBException(e);
		}
		catch (Exception e){
			log.error("User(" + request.getUserName() + ") getCompanyList error : ", e);
			response = new UserCompanyResponse();
			OpsManagerUtil.populateErrorMessage(response,e);
		}
		log.info("User(" + request.getUserName() + ") getCompanyList returned");
		return response;
		
	}
}
