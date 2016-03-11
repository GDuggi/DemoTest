package aff.confirm.opsmanager.opsmanagerweb;

import java.sql.SQLException;

import javax.ejb.EJB;
import javax.ejb.EJBException;
import javax.ejb.Stateless;
import javax.naming.NamingException;

import org.jboss.logging.Logger;

import aff.confirm.opsmanager.common.BaseOpsBean;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.confirm.Confirmation;
import aff.confirm.opsmanager.opsmanagerweb.common.TraderWebAppProcessor;
import aff.confirm.opsmanager.opsmanagerweb.data.ContractDataRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.ContractDataResponse;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderGetRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderGetResponse;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderUpdateRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderUpdateResponse;

import aff.confirm.opsmanager.confirm.data.ContractRequest;
import aff.confirm.opsmanager.confirm.data.ContractResponse;
import aff.confirm.opsmanager.tc.data.util.Environment;
import aff.confirm.opsmanager.tc.util.OpsMgrUtil;

@Stateless(name="TraderUpdater",mappedName="TraderUpdater")
public class TraderUpdaterBean extends BaseOpsBean implements TraderUpdater{

	TraderWebAppProcessor processor;
	
	@EJB(mappedName ="java:global/cnf-TransmissionGatewayCallbacks/OpsManagerCnf/Confirmation" )
    private Confirmation confirm;
	@EJB (mappedName ="java:global/cnf-TransmissionGatewayCallbacks/OpsManagerTC/OpsServerEnv" )
    private OpsMgrUtil envUtil;


	public TraderUpdaterBean() throws NamingException{
		super();
		processor = new TraderWebAppProcessor();
	}
	
	@Override
	public void prepareForMethodCall() {
		processor.setAffinityConnection(this.affinityConnection);
		
	}

	
	public TraderGetResponse getPendingTrades(TraderGetRequest request,
			String userName,String pwd) {
		
		Logger.getLogger(TraderUpdaterBean.class).info("User(" + userName + ") getPendingTrades called");
		
		TraderGetResponse resp= new TraderGetResponse();
	
		try {
			resp =processor.getTraderPendingTrades(request,userName,pwd);
		} catch (Exception e) {
			Logger.getLogger(TraderUpdaterBean.class).error("User(" + userName + ") getPendingTrades error : " + e.getMessage());
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(TraderUpdaterBean.class).info("User(" + userName + ") getPendingTrades returned");
		return resp;
	}

	public TraderUpdateResponse updateRqmtStatus(TraderUpdateRequest request,
			String userName,String pwd) {

		Logger.getLogger(TraderUpdaterBean.class).info("User(" + userName + ") updateRqmtStatus called");
		
		TraderUpdateResponse resp= new TraderUpdateResponse();
	
		try {
			resp =processor.updateRqmtStatus(request,userName,pwd);
		} catch (Exception e) {
			Logger.getLogger(TraderUpdaterBean.class).error("User(" + userName + ") updateRqmtStatus error : " + e.getMessage());
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(TraderUpdaterBean.class).info("User(" + userName + ") updateRqmtStatus returned");
		return resp;
	}

	public ContractDataResponse getContract(ContractDataRequest request,
			String userName) {
		
		Logger.getLogger(TraderUpdaterBean.class).info("User(" + userName + ") getContract called");
		
		ContractDataResponse resp= new ContractDataResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			ContractRequest cr = new ContractRequest();
			cr.setTradeId(request.getTradeId());
			cr.setTradeRqmtConfirmId(request.getTradeConfirmId());
			ContractResponse response =confirm.getContractFromVault(cr, userName);
			resp.setRequest(request);
			resp.setContractData(response.getContract());
			resp.setResponseStatus(resp.getResponseStatus());
			resp.setResponseText(response.getResponseText());
			resp.setResponseStackError(response.getResponseStackError());
			
		} catch (SQLException e) {
			Logger.getLogger(TraderUpdaterBean.class).error("User(" + userName + ") getContract error : " + e.getMessage());
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		Logger.getLogger(TraderUpdaterBean.class).info("User(" + userName + ") getContract returned");
		return resp;
	}

	public String checkLogin(String userName, String pwd) {
		
		Logger.getLogger(TraderUpdaterBean.class).info("User(" + userName + ") checkLogin called");
		String returnString = "OK";
		
		try {
			processor.checkLogin(userName, pwd);
			
		} catch (SQLException e) {
			Logger.getLogger(TraderUpdaterBean.class).error("User(" + userName + ") checkLogin error : " + e.getMessage());
			returnString = e.getMessage();
			
		}
		Logger.getLogger(TraderUpdaterBean.class).info("User(" + userName + ") checkLogin returned");
		return returnString;
	
	}

	public String getDbName() {
		
		Logger.getLogger(TraderUpdaterBean.class).info("getDbName called");
		String returnString = "No Name";
		
		try {
			Environment env = envUtil.getAppServerEnv("OpsWeb");
			returnString = env.getAffinityDatabase();
			
		} catch (Exception e) {
			Logger.getLogger(TraderUpdaterBean.class).error("getDbName error : " + e.getMessage());
			returnString = "ERROR";
			
		}
		Logger.getLogger(TraderUpdaterBean.class).info("getDbName returned");
		return returnString;
		
	}

	
}
