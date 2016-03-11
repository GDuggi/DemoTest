package aff.confirm.opsmanager.opsmanagerweb.faxgateway;

import java.sql.SQLException;

import javax.ejb.EJBException;
import javax.ejb.Stateless;
import javax.naming.NamingException;

import org.jboss.logging.Logger;

import aff.confirm.opsmanager.common.BaseOpsBean;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.opsmanagerweb.common.GatewayStatusUpdater;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxGatewayUpdateRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxGatewayUpdateResponse;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxReconcileRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxReconcileResponse;

@Stateless(name="FaxGatewayUpdater",mappedName="FaxGatewayUpdater")
public class StatusUpdaterBean extends BaseOpsBean implements StatusUpdater{

	private GatewayStatusUpdater gteWayUpdater;
	public StatusUpdaterBean() throws NamingException{
		super();
		gteWayUpdater = new GatewayStatusUpdater(this.affinityConnection);
	}
	
	@Override
	public void prepareForMethodCall() {
		gteWayUpdater.setConnection(this.affinityConnection);
	}

	public FaxGatewayUpdateResponse updateStatus(FaxGatewayUpdateRequest request, String userName) {

		Logger.getLogger(StatusUpdaterBean.class).info("User(" + userName + ") updateStatus called");
		
		FaxGatewayUpdateResponse resp = new FaxGatewayUpdateResponse();
		
			try {
				OpsManagerUtil.createProxy(this.affinityConnection, userName);
				resp =gteWayUpdater.updateFaxGatewayResponse(request);
			} catch (SQLException e) {
				Logger.getLogger(StatusUpdaterBean.class).error("User(" + userName + ") updateStatus error : " + e.getMessage());
				OpsManagerUtil.populateErrorMessage(resp, e);
				throw new EJBException(e);
			}
			Logger.getLogger(StatusUpdaterBean.class).info("User(" + userName + ") updateStatus returned");
			return resp;
		
	}

	public FaxReconcileResponse reconcileFaxServer(FaxReconcileRequest request,
			String userName) {
		
		Logger.getLogger(StatusUpdaterBean.class).info("User(" + userName + ") reconcileFaxServer called");
		
		FaxReconcileResponse resp = new FaxReconcileResponse();
		
			try {
				OpsManagerUtil.createProxy(this.affinityConnection, userName);
				resp =gteWayUpdater.reconcileFaxServer(request);
			} catch (SQLException e) {
				Logger.getLogger(StatusUpdaterBean.class).error("User(" + userName + ") reconcileFaxServer error : " + e.getMessage());
				OpsManagerUtil.populateErrorMessage(resp, e);
				throw new EJBException(e);
			}
			Logger.getLogger(StatusUpdaterBean.class).info("User(" + userName + ") reconcileFaxServer returned");
			return resp;
	}

}
