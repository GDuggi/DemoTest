package aff.confirm.opsmanager.opsmanagerweb;

import javax.ejb.Remote;

import aff.confirm.opsmanager.opsmanagerweb.data.ContractDataRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.ContractDataResponse;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderGetRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderGetResponse;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderUpdateRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderUpdateResponse;

@Remote
public interface TraderUpdater {

	TraderGetResponse getPendingTrades(TraderGetRequest request,String userName,String pwd);
	TraderUpdateResponse updateRqmtStatus(TraderUpdateRequest request,String userName,String pwd);
	ContractDataResponse getContract(ContractDataRequest request,String userName);
	String checkLogin(String userName,String pwd);
	String getDbName();
}
