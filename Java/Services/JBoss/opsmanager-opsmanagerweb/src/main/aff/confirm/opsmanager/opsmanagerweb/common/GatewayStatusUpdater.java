package aff.confirm.opsmanager.opsmanagerweb.common;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;


import org.jboss.logging.Logger;

import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxGatewayUpdateRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxGatewayUpdateResponse;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxReconcileData;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxReconcileRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxReconcileResponse;
import aff.confirm.opsmanager.opsmanagerweb.data.FaxGatewayUpdateRequest._Gateway_Status;


public class GatewayStatusUpdater {

	private Connection affinityConnection;
	
	public GatewayStatusUpdater(){}
	
	public GatewayStatusUpdater(Connection connection){
		this.affinityConnection = connection;
	}
	public Connection getConnection() {
		return affinityConnection;
	}

	public void setConnection(Connection connection) {
		this.affinityConnection = connection;
	}
	
	private SimpleDateFormat sdf = new SimpleDateFormat("M/d/yyyy'T'HH:mm");
	private SimpleDateFormat sdfReturn  = new SimpleDateFormat("dd-MMM-yyyy hh:mm a");
	
	public FaxGatewayUpdateResponse updateFaxGatewayResponse(FaxGatewayUpdateRequest request){
		
		CallableStatement stmt = null;
		String sql = "{ call PKG_RQMT_CONFIRM$P_UPDATE_FAX_GATEWAY_STATUS(?,?,?,?,?,?,?,?,?)}"; ;
		

		String userName = OpsManagerUtil.getUserName(affinityConnection);
	
		FaxGatewayUpdateResponse response = new FaxGatewayUpdateResponse();
		response.setRequest(request);
		try {
			stmt = affinityConnection.prepareCall(sql);
			stmt.setLong(1, request.getTradeId());
			stmt.setLong(2, request.getTradeRqmtId());
			stmt.setLong(3, request.getTradeRqmtConfirmId());
			String statusString = "S";
			if ( request.getStatus() == _Gateway_Status.Failed) {
				statusString = "F";
			}
			else if (request.getStatus() == _Gateway_Status.Queued) {
				statusString = "Q";
			}
			stmt.setString(4, statusString);
			stmt.setString(5, request.getRecipAddr());
			stmt.setString(6, request.getLabel());
			stmt.setString(7, request.getSender());
			stmt.setString(8, request.getFaxTelexInd());
			stmt.setString(9, request.getCmt());
		//	stmt.setLong(10, request.getAssociatedDocId());
			stmt.execute();
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
            Logger.getLogger(this.getClass()).error("User(" + userName + ") updateFaxGatewayResponse error : " + e.getMessage());
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		}
		finally {
            try {
				if (stmt != null) {
					stmt.close();
				}
			}
			catch (SQLException e) {}
		}
		
		return response;

	}
	
	public FaxReconcileResponse reconcileFaxServer(FaxReconcileRequest  request){

		PreparedStatement stmt = null;
		ResultSet rsCnt = null;
		ResultSet rs = null;
		PreparedStatement stmtMiss = null;
		String sql = "select count(*) from  ops_tracking.inb_faxination_log ifl, ops_tracking.inbound_fax_nos ifn where ifl.received_on >= ? and " +
					" ifl.received_on <= ?  and ifl.location = ? and ifl.sent_to = ifn.faxno ";
		String missingSql = "select ifl.* from ops_tracking.inb_faxination_log ifl, ops_tracking.inbound_docs id , ops_tracking.inbound_fax_nos ifn " +
							" where ifl.received_on >= ? and ifl.received_on <=? and location = ? " +
							" and ifl.sent_to = ifn.faxno " +
							" and ifl.job_reference = id.job_ref(+) and id.id is null " +
							" order by ifl.received_on ";
		

		String userName = OpsManagerUtil.getUserName(affinityConnection);
	
		
		FaxReconcileResponse response = new FaxReconcileResponse();
		ArrayList<FaxReconcileData> data = new ArrayList<FaxReconcileData>();
		if (request == null) {
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
				
		}
		response.setRequest(request);
		response.setData(data);
		try {
			Timestamp startTime = getDateTime(request.getStartDate());
			Timestamp endTime = getDateTime(request.getEndDate());
			stmt = affinityConnection.prepareStatement(sql);
			stmt.setTimestamp(1, startTime);
			stmt.setTimestamp(2, endTime);
			stmt.setString(3, request.getLocation().toLowerCase());
			rsCnt = stmt.executeQuery();
			if (rsCnt.next()){
				response.setFaxCount(rsCnt.getInt(1));
			}
			System.out.println("Total Count= " + response.getFaxCount());
			stmtMiss = affinityConnection.prepareStatement(missingSql);
			stmtMiss.setTimestamp(1, startTime);
			stmtMiss.setTimestamp(2, endTime);
			stmtMiss.setString(3, request.getLocation().toLowerCase());
			rs = stmtMiss.executeQuery();
			int missCount = 0;
			while ( rs.next()){
				FaxReconcileData fra = new FaxReconcileData();
				fra.setSender(rs.getString("sender"));
				fra.setJobReference(rs.getString("job_reference"));
				fra.setReceivedAt(formatDate(rs.getTimestamp("received_on")));
				fra.setFaxNumber(rs.getString("sent_to"));
				fra.setMessage(rs.getString("message"));
				data.add(fra);
				++missCount;
			}
			response.setOpsManagerCount(response.getFaxCount() - missCount);
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (ParseException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") reconcileFaxServer error : " + e.getMessage());
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") reconcileFaxServer error : " + e.getMessage());
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		}
		finally {
			try {
				if ( rsCnt != null) {
					rsCnt.close();
				}
				if (stmt != null) {
					stmt.close();
				}
				if (rs != null){
					rs.close();
				}
				if ( stmtMiss != null){
					stmtMiss.close();
				}
			}
			catch (SQLException e) {}
		}
		
		return response;

	
	}
	private String formatDate(Timestamp date) {
		return sdfReturn.format(date);
	}

	private Timestamp getDateTime(String dateStr) throws ParseException{
		Timestamp ts = null;
		
		if (dateStr != null){
			ts = new Timestamp(sdf.parse(dateStr).getTime());
		}
		System.out.println("Return date = " + ts);
		return ts;
	}
}
