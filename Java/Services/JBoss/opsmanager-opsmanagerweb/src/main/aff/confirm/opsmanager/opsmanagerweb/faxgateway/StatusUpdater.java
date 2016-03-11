package aff.confirm.opsmanager.opsmanagerweb.faxgateway;

import javax.ejb.Remote;

import aff.confirm.opsmanager.opsmanagerweb.data.FaxGatewayUpdateRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxGatewayUpdateResponse;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxReconcileRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxReconcileResponse;

@Remote
public interface StatusUpdater {

	FaxGatewayUpdateResponse updateStatus(FaxGatewayUpdateRequest request,String userName);
	FaxReconcileResponse  reconcileFaxServer(FaxReconcileRequest request,String userName);
	
	
}
