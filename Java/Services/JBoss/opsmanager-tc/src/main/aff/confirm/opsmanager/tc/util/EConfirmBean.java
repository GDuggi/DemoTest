
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
import aff.confirm.opsmanager.tc.data.util.EConfirmMatchRequest;
import aff.confirm.opsmanager.tc.data.util.EConfirmMatchResponse;
import aff.confirm.opsmanager.tc.data.util.EConfirmRequest;
import aff.confirm.opsmanager.tc.data.util.EConfirmResponse;

@Stateless(name="EConfirm",mappedName="EConfirm")
public class EConfirmBean extends BaseOpsBean implements EConfirm {

	TCUtilProcessor processor;
	public EConfirmBean() throws NamingException {
		super();
		processor = new TCUtilProcessor(this.affinityConnection);
	}

	
	public BaseResponse resubmit( int tradeId,String status,String userName) throws EJBException  {

		Logger.getLogger(EConfirmBean.class).info("User(" + userName + ") resubmit called");
		BaseResponse response = null;
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			response= processor.processEConfirmRequest(tradeId, status);
		}
		catch (SQLException e){
			Logger.getLogger(EConfirmBean.class).error("User(" + userName + ") resubmit error : " , e  );
			response = OpsManagerUtil.populateErrorMessage(response,e);
			try {
				if (this.affinityConnection.isClosed()) {
					throw new EJBException(e);
				}
			} catch (SQLException e1) {
				   throw new EJBException(e);
			}
			
		}
		Logger.getLogger(EConfirmBean.class).info("User(" + userName + ") resubmit returned");
		return response;

	}


	public ArrayList<EConfirmResponse> batchSubmit(
			ArrayList<EConfirmRequest> tradeList, String userName) throws EJBException {
		
		ArrayList<EConfirmResponse> response = null;
		EConfirmResponse resp = null;
		Logger.getLogger(EConfirmBean.class).info("User(" + userName + ") batchSubmit called");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			response= processor.processEConfirmRequest(tradeList);
		}
		catch (SQLException e){
			resp = new EConfirmResponse();
			resp = (EConfirmResponse) OpsManagerUtil.populateErrorMessage(resp,e);
			try {
				if (this.affinityConnection.isClosed()) {
					throw new EJBException(e);
				}
			} catch (SQLException e1) {
				   throw new EJBException(e);
			}
			
		} catch (Exception e) {
			Logger.getLogger(EConfirmBean.class).error("User(" + userName + ") batchSubmit error : " , e );
			resp = new EConfirmResponse();
			resp = (EConfirmResponse) OpsManagerUtil.populateErrorMessage(resp,e);
			try {
				if (this.affinityConnection.isClosed()) {
					throw new EJBException(e);
				}
			} catch (SQLException e1) {
				   throw new EJBException(e);
			}
		}
		finally {
			if (resp != null)  { // there is an error
				response = new ArrayList<EConfirmResponse>();
				response.add(resp);
			}
		}
		Logger.getLogger(EConfirmBean.class).info("User(" + userName + ") batchSubmit returned");
		return response;
	}


	@Override
	public void prepareForMethodCall() {
		processor.setDbConnection(this.affinityConnection);
	}


	public ArrayList<EConfirmMatchResponse> batchMatch(
			ArrayList<EConfirmMatchRequest> tradeList, String userName) {
		
		ArrayList<EConfirmMatchResponse> response = null;
		EConfirmMatchResponse resp = null;
		Logger.getLogger(EConfirmBean.class).info("User(" + userName + ") batchMatch called");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			response= processor.matchEConfirm(tradeList);
		}
		catch (SQLException e){
			resp = new EConfirmMatchResponse();
			resp = (EConfirmMatchResponse) OpsManagerUtil.populateErrorMessage(resp,e);
			try {
				if (this.affinityConnection.isClosed()) {
					throw new EJBException(e);
				}
			} catch (SQLException e1) {
				   throw new EJBException(e);
			}
			
		} catch (Exception e) {
			Logger.getLogger(EConfirmBean.class).error("User(" + userName + ") batchMatch error : " , e );
			resp = new EConfirmMatchResponse();
			resp = (EConfirmMatchResponse) OpsManagerUtil.populateErrorMessage(resp,e);
			try {
				if (this.affinityConnection.isClosed()) {
					throw new EJBException(e);
				}
			} catch (SQLException e1) {
				   throw new EJBException(e);
			}
		}
		finally {
			if (resp != null)  { // there is an error
				response = new ArrayList<EConfirmMatchResponse>();
				response.add(resp);
			}
		}
		Logger.getLogger(EConfirmBean.class).info("User(" + userName + ") batchMatch returned");
		return response;
	}

}
