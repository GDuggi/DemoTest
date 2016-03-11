package aff.confirm.opsmanager.tc.common;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.tc.data.DetermineActionRequest;
import aff.confirm.opsmanager.tc.data.DetermineActionResponse;
import aff.confirm.opsmanager.tc.data.TradeConfirmCreatorRequest;
import aff.confirm.opsmanager.tc.data.TradeConfirmCreatorResponse;

public class TradeRqmtProcessor {

	Connection affinityConnection;
	
	public TradeRqmtProcessor(Connection affConneciton){
		affinityConnection= affConneciton; 
	}
	
	public void setDbConnection(Connection connection) {
		this.affinityConnection = connection;
	}
	public void updateStatus(int tradeRqmtId, String status, String completedDt, String reference, String comment, String forceCmtToNull) throws SQLException{
	
		String sp = " { call ops_tracking.p_update_trade_rqmt(?,?,?,?,?,?)} ";
		
		CallableStatement stmt = affinityConnection.prepareCall(sp);
		stmt.setInt(1, tradeRqmtId);
		stmt.setString(2, status);
		stmt.setString(3, completedDt);
		stmt.setString(4, reference);
		stmt.setString(5, forceCmtToNull);
		stmt.execute();
		
	}
	
	public void updateFinalApproval(int tradeId, String finalApprovalStatus) throws SQLException{
	
		String sp = " { call ops_tracking.pkg_trade_summary.p_update_final_approval(?,?) }";
		
		CallableStatement stmt = affinityConnection.prepareCall(sp);
		stmt.setInt(1, tradeId);
		stmt.setString(2, finalApprovalStatus);
		stmt.execute();
		
	}
	public void updateTradeSummary(int tradeId,String finalApprovedFlag,String opsDetactFlag, String openRqmtFlag,String comment, String forceCmtNullFlag) throws SQLException{
			
		String sp = " { call ops_tracking.pkg_trade_summary.p_update_trade_summary(?,?,?,?,?,?) }";
		
		CallableStatement stmt = affinityConnection.prepareCall(sp);
		stmt.setInt(1, tradeId);
		stmt.setString(2, finalApprovedFlag);
		stmt.setString(3, opsDetactFlag);
		stmt.setString(4, openRqmtFlag);
		stmt.setString(5, comment);
		stmt.setString(6, forceCmtNullFlag);
		stmt.execute();
		
		
	}
	public void updateTradeSummaryComment(int tradeId,String comment) throws SQLException{
		
		String sp = " { call ops_tracking.pkg_trade_summary.p_update_trade_summary_cmt(?,?) }";
		
		CallableStatement stmt = affinityConnection.prepareCall(sp);
		stmt.setInt(1, tradeId);
		stmt.setString(2, comment);
		stmt.execute();
		
		
	}
	public DetermineActionResponse[] updateTradeActions(DetermineActionRequest[] request) {
		
		DetermineActionResponse[] response = null;
		
		if (request == null){
			response = new DetermineActionResponse[]{new DetermineActionResponse()};
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is empty");
			return response;
		}
		
		String sp = " { call ops_tracking.pkg_trade_summary.p_update_determine_actions(?,?) }";
		response = new DetermineActionResponse[request.length];
		for (int i=0;i<request.length; ++i) {
			DetermineActionResponse resp = new DetermineActionResponse();
			resp.setRequest(request[i]);
			response[i] = resp;
		}
		try {
			
			CallableStatement stmt = affinityConnection.prepareCall(sp);
			for (int i=0;i<request.length; ++i) {
				try {
					stmt.setLong(1,request[i].getTradeId());
					stmt.setString(2,request[i].getActionFlag() );
					stmt.execute();
					response[i].setResponseStatus(BaseResponse.SUCCESS);
				}
				catch (SQLException e){
					response[i].setResponseStatus(BaseResponse.ERROR);
					response[i].setResponseText(e.getMessage());
					response[i].setResponseStackError(e.getStackTrace().toString());
					
				}
				
			}
		}
		catch (SQLException e){
			for (int i=0;i<request.length; ++i) {
				response[i].setResponseStatus(BaseResponse.ERROR);
				response[i].setResponseText(e.getMessage());
				response[i].setResponseStackError(e.getStackTrace().toString());
			}
		}
		return response;
	}
	
		
	public void  updateWithRef(int tradeRqmtId,String status, String completedDt, String reference) throws SQLException{
		
		String sp = " { call ops_tracking.pkg_trade_summary.p_update_trade_rqmt(?,?,?,?) }";
		
		CallableStatement stmt = affinityConnection.prepareCall(sp);
		stmt.setInt(1, tradeRqmtId);
		stmt.setString(2, status);
		stmt.setString(3, completedDt);
		stmt.setString(4, reference);
		stmt.execute();
	}
	
	
	public void   updateWithComment(int tradeRqmtId,String status, String completedDt, String reference,String comment,String forceCmtToNull) throws SQLException{
		
		String sp = " { call ops_tracking.pkg_trade_summary.p_update_trade_rqmt(?,?,?,?,?,?) }";
		
		CallableStatement stmt = affinityConnection.prepareCall(sp);
		stmt.setInt(1, tradeRqmtId);
		stmt.setString(2, status);
		stmt.setString(3, completedDt);
		stmt.setString(4, reference);
		stmt.setString(5, comment);
		stmt.setString(6, forceCmtToNull);
		stmt.execute();
	}
	
	public void  updateRqmts(int tradeRqmtId, String status,String completedDate, String reference, String comment,String updateStatus, String updateCompletedDate, String updateRef,String updateComment) throws SQLException{
		
		String sp = " { call ops_tracking.pkg_trade_summary.p_update_mult_trade_rqmts(?,?,?,?,?,?,?,?,?) }";
		
		CallableStatement stmt = affinityConnection.prepareCall(sp);
		stmt.setInt(1, tradeRqmtId);
		stmt.setString(2, status);
		stmt.setString(3, completedDate);
		stmt.setString(4, reference);
		stmt.setString(5, comment);
		stmt.setString(6, updateStatus);
		stmt.setString(7, updateCompletedDate);
		stmt.setString(8, updateRef);
		stmt.setString(9, updateComment);
		stmt.execute();
	}
	
	public TradeConfirmCreatorResponse updateTradeConfirmCreator(TradeConfirmCreatorRequest request) throws SQLException{
		
		String sp = " { call ops_tracking.pkg_trade_rqmt.p_update_trade_confirm_creator(?,?) }";
		
		TradeConfirmCreatorResponse response = new TradeConfirmCreatorResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is empty");
			return response;
		}
		
		response.setRequest(request);
		CallableStatement stmt = null;
		try {
			
			stmt = affinityConnection.prepareCall(sp);
			stmt.setLong(1,request.getRqmtConfirmId());
			stmt.setString(2,request.getUserId());
			stmt.execute();
			response.setResponseStatus(BaseResponse.SUCCESS);
				
		}
		catch (SQLException e){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
			throw e;
		}
		finally {
			try {
				if ( stmt != null) {
					stmt.close();
				}
			}
			catch (Exception e){
				
			}
		}
		return response;
		
	}
	
public TradeConfirmCreatorResponse getTradeConfirmCreator(TradeConfirmCreatorRequest request) throws SQLException{
		
		String sql = "select  rqmt_confirm_id,user_id from ops_tracking.trade_rqmt_confirm_creator where rqmt_confirm_id = ?";
		
		TradeConfirmCreatorResponse response = new TradeConfirmCreatorResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is empty");
			return response;
		}
		
		response.setRequest(request);
		ResultSet rs = null;
		PreparedStatement stmt = null;
		try {
			
			stmt = affinityConnection.prepareCall(sql);
			stmt.setLong(1,request.getRqmtConfirmId());
			rs = stmt.executeQuery();
			if (rs.next()){
				response.setUserId(rs.getString("user_id"));
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
				
		}
		catch (SQLException e){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
			throw e;
		}
		finally {
			try {
				if (rs != null){
					rs.close();
				}
				if ( stmt != null) {
					stmt.close();
				}
			}
			catch (Exception e){
				
			}
		}
		return response;
		
	}

}
