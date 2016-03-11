package aff.confirm.opsmanager.tc.trade;


import java.sql.SQLException;

import javax.ejb.EJBException;
import javax.ejb.Stateless;
import javax.naming.NamingException;

import org.jboss.logging.Logger;

import aff.confirm.opsmanager.common.BaseOpsBean;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.tc.common.TradeProcessor;
import aff.confirm.opsmanager.tc.data.FinalApproveRequest;
import aff.confirm.opsmanager.tc.data.FinalApproveResponse;
import aff.confirm.opsmanager.tc.data.ReopenRequest;
import aff.confirm.opsmanager.tc.data.ReopenResponse;
import aff.confirm.opsmanager.tc.data.TradeGroupRequest;
import aff.confirm.opsmanager.tc.data.TradeGroupResponse;
import aff.confirm.opsmanager.tc.data.TradeUnGroupRequest;
import aff.confirm.opsmanager.tc.data.TradeUnGroupResponse;

@Stateless(name="TradeUpdater",mappedName="TradeUpdater")
public class TradeUpdaterBean extends BaseOpsBean implements TradeUpdater {

	TradeProcessor _processor = null;
	
	
	public TradeUpdaterBean() throws NamingException {
		super();
		
		
		_processor = new TradeProcessor(this.affinityConnection);
		
	}

	public FinalApproveResponse[] finalApprove(FinalApproveRequest[] request,String userName) {
		
		Logger.getLogger(this.getClass()).info("User(" + userName + ") finalApprove called");
		
		FinalApproveResponse[] resp = new FinalApproveResponse[1];
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			//resp =_processor.finalApprove(request);
			resp = _processor.finalApprove(request, userName);
		} catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") finalApprove error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0],e);
			try {
				if (this.affinityConnection.isClosed()) {
					throw new EJBException(e);
				}
			} catch (SQLException e1) {
				   throw new EJBException(e);
			}
		}
		Logger.getLogger(this.getClass()).info("User(" + userName + ") finalApprove returned");
		return resp;
	}

	public TradeGroupResponse[] groupTrades(TradeGroupRequest[] request,String userName) {

		Logger.getLogger(this.getClass()).info("User(" + userName + ") groupTrades called");
		
		TradeGroupResponse[] resp = new TradeGroupResponse[1];
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =_processor.group(request); 
		} catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") groupTrades error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0],e);
			try {
				if (this.affinityConnection.isClosed()) {
					throw new EJBException(e);
				}
			} catch (SQLException e1) {
				   throw new EJBException(e);
			}
		}
		Logger.getLogger(this.getClass()).info("User(" + userName + ") groupTrades returned");
		return resp;
	}

	public ReopenResponse[] reopen(ReopenRequest[] request,String userName) {

		Logger.getLogger(this.getClass()).info("User(" + userName + ") reopen called");
		
		ReopenResponse[] resp = new ReopenResponse[1];
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =_processor.reopen(request); 
		} catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") reopen error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0],e);
			try {
				if (this.affinityConnection.isClosed()) {
					throw new EJBException(e);
				}
			} catch (SQLException e1) {
				   throw new EJBException(e);
			}
		}
		Logger.getLogger(this.getClass()).info("User(" + userName + ") reopen returned");
		return resp;
	}

	public TradeUnGroupResponse ungroupTrades(TradeUnGroupRequest request,String userName) {
		
		Logger.getLogger(this.getClass()).info("User(" + userName + ") ungroupTrades called");
		
		TradeUnGroupResponse resp = new TradeUnGroupResponse();
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =_processor.ungroup(request); 
		} catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") ungroupTrades error : " , e );
			OpsManagerUtil.populateErrorMessage(resp,e);
			try {
				if (this.affinityConnection.isClosed()) {
					throw new EJBException(e);
				}
			} catch (SQLException e1) {
				   throw new EJBException(e);
			}
		}
		Logger.getLogger(this.getClass()).info("User(" + userName + ") ungroupTrades returned");
		return resp;
	}

	@Override
	public void prepareForMethodCall() {
		_processor.setDbConnection(this.affinityConnection);
	}

}
