package aff.confirm.opsmanager.tc.trade;

import javax.ejb.Remote;

import aff.confirm.opsmanager.tc.data.FinalApproveRequest;
import aff.confirm.opsmanager.tc.data.FinalApproveResponse;
import aff.confirm.opsmanager.tc.data.ReopenRequest;
import aff.confirm.opsmanager.tc.data.ReopenResponse;
import aff.confirm.opsmanager.tc.data.TradeGroupRequest;
import aff.confirm.opsmanager.tc.data.TradeGroupResponse;
import aff.confirm.opsmanager.tc.data.TradeUnGroupRequest;
import aff.confirm.opsmanager.tc.data.TradeUnGroupResponse;

@Remote
public interface TradeUpdater {

	FinalApproveResponse[] finalApprove(FinalApproveRequest[] request,String userName);
	ReopenResponse[] reopen(ReopenRequest[] request,String userName);
	
	TradeGroupResponse[] groupTrades(TradeGroupRequest[] request,String userName);
	
	TradeUnGroupResponse ungroupTrades(TradeUnGroupRequest request,String userName);
	
}
