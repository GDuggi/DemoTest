package aff.confirm.opsmanager.tc.util;

import java.sql.SQLException;
import java.util.ArrayList;

import javax.ejb.EJBException;
import javax.ejb.Stateless;
import javax.naming.NamingException;

import org.jboss.logging.Logger;

import aff.confirm.opsmanager.common.BaseOpsBean;
import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.tc.common.util.TCUtilProcessor;
import aff.confirm.opsmanager.tc.data.util.ECMBoxSubmitRequest;
import aff.confirm.opsmanager.tc.data.util.ECMBoxSubmitResponse;
import aff.confirm.opsmanager.tc.data.util.EFETConfirmRequest;
import aff.confirm.opsmanager.tc.data.util.EFETConfirmResponse;

@Stateless(name="EFETConfirm",mappedName="EFETConfirm")
public class EFETConfirmBean extends BaseOpsBean implements EFETConfirm {

	TCUtilProcessor processor;
	public EFETConfirmBean() throws NamingException {
		super();
		processor = new TCUtilProcessor(this.affinityConnection);
	}

	
	public BaseResponse resubmit(
			int tradeId, 
			String status, 
			String userName) throws EJBException {
		
		Logger.getLogger(EFETConfirmBean.class).info("User(" + userName + ") resubmit called");
		BaseResponse response = null;
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			response= processor.processEEFTRequest(tradeId, status);
			
		}
		catch (SQLException e){
			Logger.getLogger(EFETConfirmBean.class).error("User(" + userName + ") resubmit error : " , e );
			response = OpsManagerUtil.populateErrorMessage(response,e);
			throw new EJBException(e);
		}
		Logger.getLogger(EFETConfirmBean.class).info("User(" + userName + ") resubmit returned");
		return response;
	}


	public ArrayList<EFETConfirmResponse> batchSubmit(
			ArrayList<EFETConfirmRequest> tradeList, String userName) throws EJBException  {
		
		ArrayList<EFETConfirmResponse> response = null;
		EFETConfirmResponse resp = null;
		
		Logger.getLogger(EFETConfirmBean.class).info("User(" + userName + ") batchSubmit called");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			response= processor.processEEFTRequest(tradeList);
		}
		catch (SQLException e){
			Logger.getLogger(EFETConfirmBean.class).error("User(" + userName + ") batchsubmit error : " , e );
			throw new EJBException(e);
			
		} catch (Exception e) {
			Logger.getLogger(EFETConfirmBean.class).error("User(" + userName + ") batchsubmit error : " , e );
			resp = new EFETConfirmResponse();
			resp = (EFETConfirmResponse) OpsManagerUtil.populateErrorMessage(resp,e);
		}
		Logger.getLogger(EFETConfirmBean.class).info("User(" + userName + ") resubmit returned");
		return response;
	}


	public ECMBoxSubmitResponse submitToECMBox(ECMBoxSubmitRequest request,
			String userName) {
		
		ECMBoxSubmitResponse response = null;
		Logger.getLogger(EFETConfirmBean.class).info("User(" + userName + ") submitToECMBox called");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			response= processor.submitToBox(request);
		}
		catch ( SQLException e){
			Logger.getLogger(EFETConfirmBean.class).error("User(" + userName + ") submitToECMBox error : " , e );
			throw new EJBException(e);
		}
		catch (Exception e){
			Logger.getLogger(EFETConfirmBean.class).error("User(" + userName + ") submitToECMBox error : " , e );
			response = new ECMBoxSubmitResponse();
			OpsManagerUtil.populateErrorMessage(response,e);
		}
		Logger.getLogger(EFETConfirmBean.class).info("User(" + userName + ") submitToECMBox returned");
		return response;
	}
	@Override
	public void prepareForMethodCall() {
		processor.setDbConnection(this.affinityConnection);
	}
}
