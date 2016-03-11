package aff.confirm.opsmanager.tc.common;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStreamReader;

import java.net.URL;
import java.net.URLConnection;
import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.sql.Clob;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import oracle.jdbc.OracleTypes;
import oracle.jdbc.driver.OracleConnection;
import org.jboss.logging.Logger;

import aff.confirm.opsmanager.common.data.*;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.tc.common.util.TCUtilProcessor;
import aff.confirm.opsmanager.tc.data.AssociatedDoc;
import aff.confirm.opsmanager.tc.data.FinalApproveRequest;
import aff.confirm.opsmanager.tc.data.FinalApproveResponse;
import aff.confirm.opsmanager.tc.data.DealSheetRequest;
import aff.confirm.opsmanager.tc.data.DealSheetResponse;
import aff.confirm.opsmanager.tc.data.GetTradeRequest;
import aff.confirm.opsmanager.tc.data.GetTradeResponse;
import aff.confirm.opsmanager.tc.data.InboundAttrib;
import aff.confirm.opsmanager.tc.data.InboundAttribMapRequest;
import aff.confirm.opsmanager.tc.data.InboundAttribMapResponse;
import aff.confirm.opsmanager.tc.data.InboundAttribResponse;
import aff.confirm.opsmanager.tc.data.JMSTradeProcessRequest;
import aff.confirm.opsmanager.tc.data.JMSTradeProcessResponse;
import aff.confirm.opsmanager.tc.data.ReopenRequest;
import aff.confirm.opsmanager.tc.data.ReopenResponse;
import aff.confirm.opsmanager.tc.data.TradeAuditData;
import aff.confirm.opsmanager.tc.data.TradeAuditRequest;
import aff.confirm.opsmanager.tc.data.TradeAuditResponse;
import aff.confirm.opsmanager.tc.data.TradeCommentRequest;
import aff.confirm.opsmanager.tc.data.TradeCommentResponse;
import aff.confirm.opsmanager.tc.data.TradeCompanyRequest;
import aff.confirm.opsmanager.tc.data.TradeCompanyResponse;
import aff.confirm.opsmanager.tc.data.TradeDataChange;
import aff.confirm.opsmanager.tc.data.TradeDataChangeRequest;
import aff.confirm.opsmanager.tc.data.TradeDataChangeResponse;
import aff.confirm.opsmanager.tc.data.TradeGroupRequest;
import aff.confirm.opsmanager.tc.data.TradeGroupResponse;
import aff.confirm.opsmanager.tc.data.TradeRqmt;
import aff.confirm.opsmanager.tc.data.TradeRqmtConfirm;
import aff.confirm.opsmanager.tc.data.TradeRqmtRequest;
import aff.confirm.opsmanager.tc.data.TradeRqmtResponse;
import aff.confirm.opsmanager.tc.data.TradeSummary;
import aff.confirm.opsmanager.tc.data.TradeUnGroupRequest;
import aff.confirm.opsmanager.tc.data.TradeUnGroupResponse;
import aff.confirm.opsmanager.tc.data.util.UserCompanyRequest;
import aff.confirm.opsmanager.tc.data.util.UserCompanyResponse;

public class TradeProcessor {
	private static Logger log = Logger.getLogger( TradeProcessor.class );
	private static final String _SEMPRA_TRADES_IND = "S";
	private static final String _ALL_COMPANY_IND = "B";
	Connection affinityConnection;
	private SimpleDateFormat sdf = new SimpleDateFormat("dd-MMM-yyyy");
	private SimpleDateFormat sdtf = new SimpleDateFormat("dd-MMM-yyyy hh:mm:ss aaa");
	
	public TradeProcessor(Connection connection){
		this.affinityConnection = connection;
	}
	
	public void setDbConnection(Connection connection){
		this.affinityConnection = connection;
	}
	
	public FinalApproveResponse[] finalApprove(FinalApproveRequest[] request,String userName) throws SQLException
	{
		FinalApproveResponse[] response = null;
		
		String sp = "{?=call ops_tracking.pkg_trade_appr.f_set_final_approval_flag(?,?,?,?)}";
		
		
		if (request == null){
			response = new FinalApproveResponse[1];
			response[0] = new FinalApproveResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
			return response;
		}
		CallableStatement stmt = null;
		int i;
		response = new FinalApproveResponse[request.length];
		for (i=0; i<request.length;++i) {
			 response[i] = new FinalApproveResponse(); 
			 response[i].setRequest(request[i]);
		 }
				 
		try {
				
			 stmt = affinityConnection.prepareCall(sp);
			 
			 stmt.registerOutParameter(1, OracleTypes.INTEGER);
			 for (i=0; i<request.length;++i){
				
				 try {
				 
					 stmt.setLong(2, request[i].getTradeId());
					 stmt.setString(3, request[i].getApprovalFlag());
					 stmt.setString(4, request[i].getOnlyIfReadyFlag());
					 stmt.setString(5, userName);
					 stmt.execute();
					 int result = stmt.getInt(1);
					 if (result == 0) {
						 response[i].setResponseStatus(BaseResponse.SUCCESS);
					 }
					 else {
						 String requestType = "Approved";
						 if (!"Y".equalsIgnoreCase(request[i].getApprovalFlag())) {
							 requestType = "Reopened";
						 }
						 response[i].setResponseStatus(BaseResponse.ERROR);
						 response[i].setResponseText("Not " + requestType );
					 }
					 log.info("Final Approved for = " + request[i].getTradeId());
					 affinityConnection.commit();
				 }
				 catch (Exception e) {
					 log.error("User(" + userName + ") finalApprove error : " , e );
					 response[i].setResponseStatus(BaseResponse.ERROR);
					 response[i].setResponseText(e.getMessage());
					 response[i].setResponseStackError(e.getStackTrace().toString());
				 }
			 }
				
		} catch (SQLException e) {
			log.error("User(" + userName + ") finalApprove error : " , e );
			for (i=0; i<request.length;++i) {
			 response[i].setResponseStatus(BaseResponse.ERROR);
			 response[i].setResponseText(e.getMessage());
			 response[i].setResponseStackError(e.getStackTrace().toString());
		  	 throw e;
			
			}
					
		}
		finally {
				
			if (stmt != null){
				try {
				stmt.close();
				} catch (SQLException e) {
				}
			}
		}	
		
		return response;
	}
	
	public FinalApproveResponse[] finalApprove(FinalApproveRequest[] request) throws SQLException
	{
		FinalApproveResponse[] response = null;
		
		String sql = "insert into ops_tracking.q_final_approve(id,trade_id,approval_flag," +
					 "only_if_ready_flag) values( ops_tracking.seq_q_final_approve.nextval,?,?,?)";
		
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		if (request == null){
			response = new FinalApproveResponse[1];
			response[0] = new FinalApproveResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
			return response;
		}
		PreparedStatement stmt = null;
		int i;
		response = new FinalApproveResponse[request.length];
		for (i=0; i<request.length;++i) {
			 response[i] = new FinalApproveResponse(); 
			 response[i].setRequest(request[i]);
		 }
				 
		try {
				
			 stmt = affinityConnection.prepareStatement(sql);
			 for (i=0; i<request.length;++i){
				
				 try {
				 
					 stmt.setLong(1, request[i].getTradeId());
					 stmt.setString(2, request[i].getApprovalFlag());
					 stmt.setString(3, request[i].getOnlyIfReadyFlag());
					 stmt.executeUpdate();
					 response[i].setResponseStatus(BaseResponse.SUCCESS);
			//		 affinityConnection.commit();
				 }
				 catch (Exception e) {
					 log.error("User(" + userName + ") finalApprove error : " , e );
					 response[i].setResponseStatus(BaseResponse.ERROR);
					 response[i].setResponseText(e.getMessage());
					 response[i].setResponseStackError(e.getStackTrace().toString());
				 }
			 }
				
		} catch (SQLException e) {
			log.error("User(" + userName + ") finalApprove error : " , e );
			for (i=0; i<request.length;++i) {
			 response[i].setResponseStatus(BaseResponse.ERROR);
			 response[i].setResponseText(e.getMessage());
			 response[i].setResponseStackError(e.getStackTrace().toString());
		  	 throw e;
			
			}
					
		}
		finally {
				
			if (stmt != null){
				try {
				stmt.close();
				} catch (SQLException e) {
				}
			}
		}	
		
		return response;
	}
	
	public ReopenResponse[] reopen(ReopenRequest[] request)
	{
		ReopenResponse[] response = null;
		
		String sql = "insert into ops_tracking.q_restore_data ( id, trd_sys_code, trade_id ) " +
					" values (ops_tracking.seq_q_restore_data.nextval, ?, ?)";

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		if (request == null){
			response = new ReopenResponse[1];
			response[0] = new ReopenResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
		}
		else {
				PreparedStatement stmt = null;
				int i;
				response = new ReopenResponse[request.length];
				for (i=0; i<request.length;++i) {
					response[i] = new ReopenResponse();
					response[i].setRequest(request[i]);
				 }
				
			try {
				
				 stmt = affinityConnection.prepareStatement(sql);
				 for (i=0; i<request.length;++i){
					 try {
						 stmt.setString(1, request[i].getTradeSysCode());
						 stmt.setLong(2, request[i].getTradeId());
						 stmt.executeUpdate();
						 affinityConnection.commit();
						 response[i].setResponseStatus(BaseResponse.SUCCESS);
					 }
					 catch (Exception e){
						 log.error("User(" + userName + ") reopen error : " , e );
						 response[i].setResponseStatus(BaseResponse.ERROR);
						 response[i].setResponseText(e.getMessage());
						 response[i].setResponseStackError(e.getStackTrace().toString());
					 }
				 }
				 
				
			} catch (SQLException e) {
				log.error("User(" + userName + ") reopen error : " , e );
				for (i=0; i<request.length;++i) {
					 response[i].setResponseStatus(BaseResponse.ERROR);
					 response[i].setResponseText(e.getMessage());
					 response[i].setResponseStackError(e.getStackTrace().toString());
				}
				
			}
			finally {
				
				if (stmt != null){
					try {
						stmt.close();
					} catch (SQLException e) {
					}
				}
			}	
			
		}
		
		return response;
	}

	public TradeGroupResponse[] group(TradeGroupRequest[] request) throws SQLException{
		
		TradeGroupResponse[] response = null;
		String sql = "insert into ops_tracking.trade_group ( id, trade_id, xref ) " +
		" values (ops_tracking.seq_trade_group.nextval, ?, ?)";

		String userName = OpsManagerUtil.getUserName(affinityConnection);

		if (request == null){
			
			response = new TradeGroupResponse[1];
			response[0] = new TradeGroupResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
		}
		else {
			PreparedStatement stmt = null;
			int i;
			
			response = new TradeGroupResponse[request.length];
			for (i=0; i<request.length;++i) {
				response[i] = new TradeGroupResponse();
				response[i].setRequest(request[i]);
			 }
			
			try {
	
				stmt = affinityConnection.prepareStatement(sql);
				for (i=0; i<request.length;++i){
					try {
						stmt.setLong(1, request[i].getTradeId());
						stmt.setString(2, request[i].getXRef());
						stmt.executeUpdate();
						affinityConnection.commit();
						response[i].setResponseStatus(BaseResponse.SUCCESS);
					}
					catch (Exception e) {
						log.error("User(" + userName + ") group error : " , e );
						response[i].setResponseStatus(BaseResponse.ERROR);
						response[i].setResponseText(e.getMessage());
						response[i].setResponseStackError(e.getStackTrace().toString());
					}
				}
				
	
			} catch (SQLException e) {
				log.error("User(" + userName + ") group error : " , e );
				for (i=0; i<request.length;++i) {
					 response[i].setResponseStatus(BaseResponse.ERROR);
					 response[i].setResponseText(e.getMessage());
					 response[i].setResponseStackError(e.getStackTrace().toString());
				}
				throw e;
			}
			finally {
	
				if (stmt != null){
					try {
						stmt.close();
					} catch (SQLException e) {
					}
				}
			}	

		}
		return response;
	}
	
	public TradeUnGroupResponse ungroup(TradeUnGroupRequest request) throws SQLException{
		
		TradeUnGroupResponse response = new TradeUnGroupResponse();
		String sql = "delete from ops_tracking.trade_group where trade_id in ";

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		response.setRequest(request);
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
		}
		else {
			sql =  sql + "(" + request.getTradeIdList() + ")";
			Statement stmt = null;
			try {
				stmt = affinityConnection.createStatement();
				stmt.executeUpdate(sql);
				affinityConnection.commit();
				response.setResponseStatus(BaseResponse.SUCCESS);
			} catch (SQLException e) {
				log.error("User(" + userName + ") ungroup error : " , e );
				response.setResponseStatus(BaseResponse.ERROR);
				response.setResponseText(e.getMessage());
				response.setResponseStackError(e.getStackTrace().toString());
				throw e;
				
			}
			finally {
	
				if (stmt != null){
					try {
						stmt.close();
					} catch (SQLException e) {
					}
				}
			}	

		}
		return response;
	}
	
	public TradeAuditResponse getTradeAudit(TradeAuditRequest request) throws SQLException{
		
		TradeAuditResponse response = new TradeAuditResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		ArrayList<TradeAuditData> auditData = new ArrayList<TradeAuditData>();
		response.setAuditData(auditData);
		String sql = "select trade_id, trade_rqmt_id,operation,rqmt,status,\"machine name\" machine_name ," +
					"userid,timestamp,completed_dt from ops_tracking.v_trade_audit where trade_id = ? order by timestamp ";
		
		PreparedStatement stmt = null;
		try {
			stmt = this.affinityConnection.prepareStatement(sql);
			stmt.setLong(1, request.getTradeId());
			ResultSet rs = stmt.executeQuery();
			
			while (rs.next()){
				
				TradeAuditData data = new TradeAuditData();
				data.setTradeId(rs.getLong("trade_id"));
				data.setTradeRqmtId(rs.getLong("trade_rqmt_id"));
				data.setOperation(rs.getString("operation"));
				data.setStatus(rs.getString("status"));
				data.setMachineName(rs.getString("machine_name"));
				data.setUserId(rs.getString("userid"));
				data.setTimeStamp( getTimeFormat(new Date(rs.getTimestamp("timestamp").getTime())));
				data.setCompletedDate(getDateFormat(rs.getDate("completed_dt")));		
				data.setRqmt(rs.getString("rqmt"));
				auditData.add(data);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e) {
			log.error("User(" + userName + ") getTradeAudit error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
			throw e;
			
		}
		finally {
			if (stmt != null){
				try { 
					stmt.close();
				}
				catch (SQLException e) {
					
				}
			}
		}
		
		return response;
		
	}
	
	private String getDateFormat(Date date) {
		String returnStr = "";
		
		if ( date != null){
			returnStr= sdf.format(date);
		}
		return returnStr;
	}

	private String getTimeFormat(Date date) {
		String returnStr = "";
		
		if ( date != null){
			returnStr= sdtf.format(date);
		}
		return returnStr;
	}

	public TradeDataChangeResponse getTradeCorrection(TradeDataChangeRequest request) throws SQLException{
		
		TradeDataChangeResponse response = new TradeDataChangeResponse();
		
		response.setRequest(request);
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
		}
		
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		ArrayList<TradeDataChange> correctionData = new ArrayList<TradeDataChange>();
		response.setTradeDataChange(correctionData);
		String sql = "select id,trade_id, upd_busn_dt,col_name,old_value,new_value,crtd_ts_gmt " +
				//	" user_name,audit_type_code,odb_include_flag,odb_cancel_correct_exclude_id " +
					"from ops_tracking.v_trade_data_chg  where  trade_id = ? and  col_name <> 'BROKER_CHG' " +
					" order by id";
		
		PreparedStatement stmt = null;
		try {
			stmt = this.affinityConnection.prepareStatement(sql);
			stmt.setLong(1, request.getTradeId());
			ResultSet rs = stmt.executeQuery();

			while (rs.next()){

				TradeDataChange data = new TradeDataChange();
				data.setTradeId(rs.getLong("trade_id"));
				data.setId(rs.getLong("id"));
				data.setUpdateBusnDate(getDateFormat(rs.getDate("upd_busn_dt")));
				data.setColumnName(rs.getString("col_name"));
				data.setOldValue(rs.getString("old_value"));
				data.setNewValue(rs.getString("new_value"));
				data.setCreateDateTime(getTimeFormat(new Date(rs.getTimestamp("crtd_ts_gmt").getTime())));
				correctionData.add(data);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e) {
			log.error("User(" + userName + ") getTradeCorrection error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
			throw e;
		}
		finally {
			if (stmt != null){
				try { 
					stmt.close();
				}
				catch (SQLException e) {

				}
			}
		}
		
		return response;
	}
	
	public TradeCommentResponse[] updateTradeComments(TradeCommentRequest[] request) throws SQLException{

		TradeCommentResponse[] response = null;
		String sql = "{ call ops_tracking.pkg_trade_summary .p_update_trade_summary_cmt(?,?) }";

		String userName = OpsManagerUtil.getUserName(affinityConnection);

		if (request == null){
			return response;
		}
		
		CallableStatement stmt = null;
		int i;
		
		response = new TradeCommentResponse[request.length];
		for (i=0; i<request.length;++i) {
			response[i] = new TradeCommentResponse();
			response[i].setRequest(request[i]);
		 }
			
		try {
	
			stmt = affinityConnection.prepareCall(sql);
			for (i=0; i<request.length;++i){
				try {
					stmt.setLong(1, request[i].getTradeId());
					stmt.setString(2, request[i].getComment());
					stmt.executeUpdate();
					affinityConnection.commit();
					response[i].setResponseStatus(BaseResponse.SUCCESS);
				}
				catch (Exception e){
					log.error("User(" + userName + ") group error : " , e );
					response[i].setResponseStatus(BaseResponse.ERROR);
					response[i].setResponseText(e.getMessage());
					response[i].setResponseStackError(e.getStackTrace().toString());
				}
			}
		} catch (SQLException e) {
			log.error("User(" + userName + ") group error : " , e );
			for (i=0; i<request.length;++i) {
				 response[i].setResponseStatus(BaseResponse.ERROR);
				 response[i].setResponseText(e.getMessage());
				 response[i].setResponseStackError(e.getStackTrace().toString());
			}
			throw e;
		}
		finally {
			if (stmt != null){
				try {
					stmt.close();
				} catch (SQLException e) {
				}
			}
		}
		
		return response;

		
	}

	public TradeRqmtResponse[] updateRqmts(TradeRqmtRequest[] request) throws SQLException{
		TradeRqmtResponse[] response = null;
		String sql = "{ call ops_tracking.pkg_trade_rqmt.p_update_trade_rqmt(?,?,?,?,?,?) }";
		
		String checkSql = "select id from ops_tracking.trade_rqmt where trade_id = ? and id = ?";

		String userName = OpsManagerUtil.getUserName(affinityConnection);

		if (request == null){
			return response;
		}
		CallableStatement stmt = null;
		PreparedStatement ps = null;
		ResultSet rs = null;
		int i;
		
		response = new TradeRqmtResponse[request.length];
		for (i=0; i<request.length;++i) {
			response[i] = new TradeRqmtResponse();
			response[i].setRequest(request[i]);
		 }
			
		try {
	
			stmt = affinityConnection.prepareCall(sql);
			ps = affinityConnection.prepareStatement(checkSql);
			for (i=0; i<request.length;++i){
				try {
					
					// check the trade id and rqmt id are in sync
					log.info("Validating the trade rqmt table for trade id = " + request[i].getTradeId() + " and rqmt id = " + request[i].getRqmtId());
					ps.setLong(1, request[i].getTradeId());
					ps.setLong(2, request[i].getRqmtId());
					rs = ps.executeQuery();
					if ( !rs.next()) {  // trade id and rqmt id do not match, do not update and throw error
						response[i].setResponseStatus(BaseResponse.ERROR);
						String errorMsg = "Error updating rqmt: The trade summary grid value (" + request[i].getTradeId() + ") and " +
								" rqmt grid value(" + request[i].getRqmtId() + ") is not in sync. Please contact confirmsupport with this message.";	
						response[i].setResponseText(errorMsg);
						log.error(errorMsg);
						rs.close();
						continue;
					}
					rs.close();
					stmt.setLong(1, request[i].getRqmtId());
					if (request[i].getStatusDate() == null) {
						stmt.setDate(2, null);
					}
					else {
						stmt.setDate(2, new java.sql.Date(request[i].getStatusDate().getTime()));
					}
					stmt.setString(3, request[i].getSecondCheck());
					stmt.setString(4, request[i].getStatus());
					stmt.setString(5, request[i].getReference());
					stmt.setString(6, request[i].getComment());
					stmt.executeUpdate();
					affinityConnection.commit();
					response[i].setResponseStatus(BaseResponse.SUCCESS);
				}
				catch (Exception e){
					log.error("User(" + userName + ") group error : " , e );
					response[i].setResponseStatus(BaseResponse.ERROR);
					response[i].setResponseText(e.getMessage());
					response[i].setResponseStackError(e.getStackTrace().toString());
				}
			}
		} catch (SQLException e) {
			log.error("User(" + userName + ") group error : " , e );
			for (i=0; i<request.length;++i) {
				 response[i].setResponseStatus(BaseResponse.ERROR);
				 response[i].setResponseText(e.getMessage());
				 response[i].setResponseStackError(e.getStackTrace().toString());
			}
			throw e;
		}
		finally {
			if (stmt != null){
				try {
					stmt.close();
				} catch (SQLException e) {
				}
			}
			if ( ps != null) {
				try {
					ps.close();
				} catch (SQLException e) {
				}
			}
		}
		
		return response;
		
	}

	public TradeRqmtResponse[] addRqmts(TradeRqmtRequest[] request) throws SQLException{
		
		TradeRqmtResponse[] response = null;
		String sql = "{ call ops_tracking.pkg_trade_rqmt.p_add_trade_rqmt(?,?,?,?) }";

		String userName = OpsManagerUtil.getUserName(affinityConnection);

		if (request == null){
			
			response = new TradeRqmtResponse[1];
			response[0] = new TradeRqmtResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
			return response;
		}
		CallableStatement stmt = null;
		int i;
		
		response = new TradeRqmtResponse[request.length];
		for (i=0; i<request.length;++i) {
			response[i] = new TradeRqmtResponse();
			response[i].setRequest(request[i]);
		 }
			
		try {
	
			stmt = affinityConnection.prepareCall(sql);
			for (i=0; i<request.length;++i){
				try {
					stmt.setLong(1, request[i].getTradeId());
					stmt.setString(2, request[i].getRqmtCode());
					stmt.setString(3, request[i].getReference());
					stmt.setString(4, request[i].getComment());
					stmt.executeUpdate();
					affinityConnection.commit();
					response[i].setResponseStatus(BaseResponse.SUCCESS);
				}
				catch (Exception e){
					log.error("User(" + userName + ") group error : " , e );
					response[i].setResponseStatus(BaseResponse.ERROR);
					response[i].setResponseText(e.getMessage());
					response[i].setResponseStackError(e.getStackTrace().toString());
				}
			}
		} catch (SQLException e) {
			log.error("User(" + userName + ") group error : " , e );
			for (i=0; i<request.length;++i) {
				 response[i].setResponseStatus(BaseResponse.ERROR);
				 response[i].setResponseText(e.getMessage());
				 response[i].setResponseStackError(e.getStackTrace().toString());
			}
			throw e;
		}
		finally {
			if (stmt != null){
				try {
					stmt.close();
				} catch (SQLException e) {
				}
			}
		}
		
		return response;
		
	}

	public GetTradeResponse getTrades(GetTradeRequest request,int maxRows,String userName) throws Exception{
		
		GetTradeResponse resp = new GetTradeResponse();
		resp.setRequest(request);
		ArrayList<TradeSummary> summaryList = new ArrayList<TradeSummary>();
		resp.setTrades(summaryList);
		ResultSet rs;
		log.info("Query started at = " + new Date());
		log.info("Max Size = " + maxRows);
		try {
			rs = getTradesData(request,maxRows,userName);
			int i =0;
			while (rs.next()){
				++i;
				TradeSummary summary = new TradeSummary();
				summary.setArchiveFlag(rs.getString("archive_flag"));
				summary.set_book(rs.getString("book"));
				summary.set_bkrDbUpd(rs.getLong("bkr_db_upd"));
				summary.set_bkrMeth(rs.getString("bkr_meth"));
				summary.set_brokerPrice(rs.getString("broker_price"));
				summary.set_bkrRqmt(rs.getString("bkr_rqmt"));
				summary.set_brokerSn(rs.getString("broker_sn"));
				summary.set_bkrStatus(rs.getString("bkr_status"));
				summary.set_buySellInd(rs.getString("buy_sell_ind"));
				summary.set_cdtyCode(rs.getString("cdty_code"));
				summary.set_cdtyGrpCode(rs.getString("cdty_grp_code"));
				summary.set_cmt(rs.getString("cmt"));
				summary.set_comm(rs.getString("comm"));
				summary.set_cptyDbUpd(rs.getLong("cpty_db_upd"));
				summary.set_cptyMeth(rs.getString("cpty_meth"));
				summary.set_cptyRqmt(rs.getString("cpty_rqmt"));
				summary.set_cptySn(rs.getString("cpty_sn"));
				summary.set_cptyStatus(rs.getString("cpty_status"));
				summary.set_currentBusnDt(rs.getDate("current_busn_dt"));
				summary.set_efsCptySn(rs.getString("efs_cpty_sn"));
				summary.set_efsFlag(rs.getString("efs_flag"));
				summary.set_endDt(rs.getDate("end_dt"));
				summary.set_finalApprovalTimestampGmt(rs.getDate("final_approval_timestamp_gmt"));
				summary.set_finalApprovalFlag(rs.getString("final_approval_flag"));
				summary.set_id(rs.getLong("id"));
				summary.set_inceptionDt(rs.getDate("inception_dt"));
				summary.set_lastUpdateTimestampGmt(rs.getDate("last_update_timestamp_gmt"));
				summary.set_lastTrdEditTimestampGmt(rs.getDate("last_trd_edit_timestamp_gmt"));
				summary.set_locationSn(rs.getString("location_sn"));
				summary.set_noconfMeth(rs.getString("noconf_meth"));
				summary.set_noconfRqmt(rs.getString("noconf_rqmt"));
				summary.set_noconfStatus(rs.getString("noconf_status"));
				summary.set_noconfDbUpd(rs.getLong("noconf_db_upd"));
				summary.set_opsDetActFlag(rs.getString("ops_det_act_flag"));
				summary.set_optnPremPrice(rs.getString("optn_prem_price"));
				summary.set_optnPutCallInd(rs.getString("optn_put_call_ind"));
				summary.set_optnStrikePrice(rs.getString("optn_strike_price"));
				summary.set_payPrice(rs.getString("pay_price"));
				summary.set_plAmt(rs.getString("pl_amt"));
				summary.set_priceDesc(rs.getString("price_desc"));
				summary.set_priority(rs.getString("priority"));
				summary.set_qty(rs.getFloat("qty"));
				summary.set_qtyTot(rs.getFloat("qty_tot"));
				summary.set_setcDbUpd(rs.getLong("setc_db_upd"));
				summary.set_setcMeth(rs.getString("setc_meth"));
				summary.set_setcRqmt(rs.getString("setc_rqmt"));
				summary.set_seCptySn(rs.getString("se_cpty_sn"));
				summary.set_setcStatus(rs.getString("setc_status"));
				summary.set_recentInd(rs.getLong("recent_ind"));
				summary.set_recPrice(rs.getString("rec_price"));
				summary.set_refSn(rs.getString("ref_sn"));
				summary.set_startDt(rs.getDate("start_dt"));
				summary.set_tradeDt(rs.getDate("trade_dt"));
				summary.set_tradeId(rs.getLong("trade_id"));
				summary.set_tradeStatCode(rs.getString("trade_stat_code"));
				summary.set_sttlType(rs.getString("sttl_type"));
				summary.set_tradeTypeCode(rs.getString("trade_type_code"));
				summary.setTrdSysCode(rs.getString("trd_sys_code"));
				summary.set_transactionSeq(rs.getLong("transaction_seq"));
				summary.set_uomDurCode(rs.getString("uom_dur_code"));
				summary.set_verblMeth(rs.getString("verbl_meth"));
				summary.set_verblRqmt(rs.getString("verbl_rqmt"));
				summary.set_verblStatus(rs.getString("verbl_status"));
				summary.set_verblDbUpd(rs.getLong("verbl_db_upd"));
				summary.set_version(rs.getLong("version"));
				summary.set_xref(rs.getString("xref"));
				summary.setGroupXRef(rs.getString("group_xref"));
				summary.set_cptyLn(rs.getString("cpty_ln"));
				summary.set_readyForReplyFlag(rs.getString("rply_rdy_to_snd_flag"));
				summary.set_migrateInd(rs.getString("migrate_ind"));
				summary.set_analystName(rs.getString("analyst_name"));
				summary.set_isTestBook(rs.getString("is_test_book"));
				summary.set_additionalConfirmSent(rs.getString("additional_confirm_sent"));
				summaryList.add(summary);				
				
			}
			log.info("Number of rows = " + i);
			rs.close();
			rs.getStatement().close();
			populateTradeRqmts(resp);
			populateTradeConfirms(resp);
			populateAssociatedDocs(resp);
			resp.setResponseStatus(BaseResponse.SUCCESS);
		} catch (SQLException e) {
			log.error("User(" + userName + ") GetTrades error : " , e );
			throw e;
		}
		log.info("Query Ended at = " + new Date());
		return resp;
	}
	
	private void populateTradeRqmts(GetTradeResponse resp) throws SQLException {

		ArrayList<TradeRqmt> rqmts = new ArrayList<TradeRqmt>();
		resp.setRqmts(rqmts);
		Statement stmt = null;
		ResultSet rs = null;
		if (resp.getTrades().size() <= 0) {
			return;
		}
		
		try {
			int i =0;
			while ( i < resp.getTrades().size() ) {
			
				String sql = "select * from ops_tracking.v_pc_trade_rqmt where trade_id in (";
				String condition = ""; 
				while (i<resp.getTrades().size()){
					condition += resp.getTrades().get(i).get_tradeId();
					++i;
					if ( i % 900 == 0) {
						break;
					}	
					if  (!(i == resp.getTrades().size())){
						condition += ",";
					}
					
				}
				sql += condition + ")";
				Logger.getLogger(TradeProcessor.class).info(sql);
				stmt= this.affinityConnection.createStatement();
				rs = stmt.executeQuery(sql);
				while (rs.next()){
					TradeRqmt tr = new TradeRqmt();
					tr.set_cancelTradeNotifyId(rs.getLong("cancel_trade_notify_id"));
					tr.set_category(rs.getString("category"));
					tr.set_guiColorCode(rs.getString("gui_color_code"));
					tr.set_cmt(rs.getString("cmt"));
					tr.set_completedDt(rs.getDate("completed_dt"));
					tr.set_completedTimestampGmt(rs.getDate("completed_timestamp_gmt"));
					tr.set_delphiConstant(rs.getString("delphi_constant"));
					tr.set_displayText(rs.getString("display_text"));
					tr.set_prelimAppr(rs.getString("prelim_appr"));
					tr.set_problemFlag(rs.getString("problem_flag"));
					tr.set_reference(rs.getString("reference"));
					tr.set_rqmt(rs.getString("rqmt"));
					tr.set_id(rs.getLong("id"));
					tr.set_rqmtTradeNotifyId(rs.getLong("rqmt_trade_notify_id"));
					tr.set_secondCheckFlag(rs.getString("second_check_flag"));
					tr.set_status(rs.getString("status"));
					tr.set_terminalFlag(rs.getString("terminal_flag"));
					tr.set_tradeId(rs.getLong("trade_id"));
					tr.set_transactionSeq(rs.getLong("transaction_seq"));
					tr.set_finalApprovalFlag(rs.getString("final_approval_flag"));
					rqmts.add(tr);
				}
				rs.close();
				stmt.close();
			}
		}
		catch (SQLException e) {
			try {
				if (rs != null) {
					rs.close();
				}
			}
			catch (Exception ef){
				
			}
			try {
				if ( stmt != null) {
					stmt.close();
				}
			}
			catch (Exception ef){
				
			}
			throw e;
		}
		
	}
	
	private void populateTradeConfirms(GetTradeResponse resp) throws SQLException {

		ArrayList<TradeRqmtConfirm> confirms = new ArrayList<TradeRqmtConfirm>();
		resp.setConfirms(confirms);
		Statement stmt = null;
		ResultSet rs = null;
		
		if (resp.getTrades().size() <= 0) {
			return;
		}
		
		try {
			int i =0;
			while ( i < resp.getTrades().size() ) {
			
				String sql = "select * from ops_tracking.v_trade_rqmt_confirm where trade_id in (";
				String condition = ""; 
				while (i<resp.getTrades().size()){
					condition += resp.getTrades().get(i).get_tradeId();
					++i;
					if ( i % 900 == 0) {
						break;
					}	
					if  (!(i == resp.getTrades().size())){
						condition += ",";
					}
				
			
				}
				sql += condition + ")";
				Logger.getLogger(TradeProcessor.class).info(sql);
				stmt = this.affinityConnection.createStatement();
				rs = stmt.executeQuery(sql);
				while (rs.next()){
					TradeRqmtConfirm trc = new TradeRqmtConfirm();
					trc.set_id(rs.getLong("id"));
					trc.set_rqmtId(rs.getLong("rqmt_id"));
					trc.set_tradeId(rs.getLong("trade_id"));
					trc.set_templateId(rs.getLong("template_id"));
					trc.set_nextStatusCode(rs.getString("next_status_code"));
					trc.set_confirmLabel(rs.getString("confirm_label"));
					trc.set_confirmCmt(rs.getString("confirm_cmt"));
					trc.set_xmitStatusInd(rs.getString("xmit_status_ind"));
					trc.set_xmitAddr(rs.getString("xmit_addr"));
					trc.set_xmitTimeStampGmt(rs.getDate("xmit_timestamp_gmt"));
					trc.set_templateName(rs.getString("template_name"));
					trc.set_templateCategory(rs.getString("template_category"));
					trc.set_templateTypeInd(rs.getString("template_type_ind"));
					trc.set_finalApprovalFlag(rs.getString("final_approval_flag"));
					trc.set_activeFlag("active_flag");
			
					confirms.add(trc);
				}
				rs.close();
				stmt.close();
		
			}
		}
		catch (SQLException e) {
			try {
				if (rs != null) {
					rs.close();
				}
			}
			catch (Exception ef){
				
			}
			try {
				if ( stmt != null) {
					stmt.close();
				}
			}
			catch (Exception ef){
				
			}
			throw e;
		}
		
	}

	private void populateAssociatedDocs(GetTradeResponse resp) throws SQLException {

		ArrayList<AssociatedDoc> assoDocs = new ArrayList<AssociatedDoc>();
		resp.setAssociatedDocs(assoDocs);
		Statement stmt = null;
		ResultSet rs = null;
		
		if (resp.getTrades().size() <= 0) {
			return;
		}
		
		try {
			int i =0;
			while ( i < resp.getTrades().size() ) {
			
				String sql = "select  /*+index(ts bi_finalapprflag) */ " +
							" ad.id  , ad.inbound_docs_id  , ad.index_val , ad.file_name ," +
							" ad.trade_id , ad.doc_status_code , ad.associated_by , ad.associated_dt , " +
							" ad.final_approved_by , ad.final_approved_dt , ad.disputed_by , ad.disputed_dt , " +
							" ad.discarded_by , ad.discarded_dt , ad.vaulted_by , ad.vaulted_dt , ad.cdty_group_code , " +
							" ad.cpty_sn  , ad.broker_sn , ad.doc_type_code , ad.sec_validate_req_flag , ad.trade_rqmt_id , " + 
							" ad.xmit_status_code , ad.xmit_value , ib.sent_to " +
							" from  ops_tracking.associated_docs ad, ops_tracking.inbound_docs ib " +
							" Where ad.INBOUND_DOCS_ID = ib.ID and ad.TRADE_ID IN ( ";
				String condition = ""; 
				while (i<resp.getTrades().size()){
					condition += resp.getTrades().get(i).get_tradeId();
					++i;
					if ( i % 900 == 0) {
						break;
					}	
					if  (!(i == resp.getTrades().size())){
						condition += ",";
					}
				
			
				}
				sql += condition + ")";
				Logger.getLogger(TradeProcessor.class).info(sql);
				stmt = this.affinityConnection.createStatement();
				rs = stmt.executeQuery(sql);
				while (rs.next()){
					AssociatedDoc ac = new AssociatedDoc();
					ac.set_id(rs.getLong("id"));
					ac.set_inboundDocsId(rs.getLong("inbound_docs_id"));
					ac.set_indexVal(rs.getInt("index_val"));
					ac.set_fileName(rs.getString("file_name"));
					ac.set_tradeId(rs.getLong("trade_id"));
					ac.set_docStatusCode(rs.getString("doc_status_code"));
					ac.set_associatedBy(rs.getString("associated_by"));
					ac.set_associatedDt(rs.getDate("associated_dt"));
					ac.set_finalApprovedBy(rs.getString("associated_by"));
					ac.set_finalApprovedDt(rs.getDate("final_approved_dt"));
					ac.set_disputedBy(rs.getString("disputed_by"));
					ac.set_disputedDt(rs.getDate("disputed_dt"));
					ac.set_discardedBy(rs.getString("discarded_by"));
					ac.set_discardedDt(rs.getDate("discarded_dt"));
					ac.set_vaultedBy(rs.getString("vaulted_by"));
					ac.set_vaultedDt(rs.getDate("vaulted_dt"));
					ac.set_cdtyGroupCode(rs.getString("cdty_group_code"));
					ac.set_cptyShortName(rs.getString("cpty_sn"));
					ac.set_brokerShortName(rs.getString("broker_sn"));
					ac.set_docTypeCode(rs.getString("doc_type_code"));
					ac.set_secondValidateReqFlag(rs.getString("sec_validate_req_flag"));
					ac.set_tradeRqmtId(rs.getLong("trade_rqmt_id"));
					ac.set_xmitStatusCode(rs.getString("xmit_status_code"));
					ac.set_xmitValue(rs.getString("xmit_value"));
					ac.set_sentTo(rs.getString("sent_to"));
					
					assoDocs.add(ac);
				}
				
		
			}
		}
		finally  {
			try {
				if (rs != null) {
					rs.close();
				}
			}
			catch (Exception ef){
				
			}
			try {
				if ( stmt != null) {
					stmt.close();
				}
			}
			catch (Exception ef){
				
			}
			
		}
		
	}

	private ResultSet getTradesData(GetTradeRequest request,int maxSize,String userName) throws Exception{
		
		ResultSet rs  = null;
		ResultSet rsCount = null;
		String sqlCount = "select count(*) cnt from ops_tracking.v_pc_trade_summary ";
		String sql = "select * from ops_tracking.v_pc_trade_summary ";
		String condition = "";
		
		String[]  securityInfo = getUserCompanyList(userName);
		if ( request.getTradingSystem()  != null && !"".equalsIgnoreCase(request.getTradingSystem())) {
			condition  +=  " trd_sys_code = ? ";  
		}
		if ( request.getRbsSempraSn() != null && !"".equalsIgnoreCase(request.getRbsSempraSn())) {
			condition  +=  ("".equalsIgnoreCase(condition) ? "" : " and ")  +  " se_cpty_sn = ? ";  
		}
		else {  // no company filter, so put the user company filter.
			condition  +=  ("".equalsIgnoreCase(condition) ? "" : " and ")  +  " se_cpty_sn in ( " + securityInfo[1] + ") ";
		}
		if ( request.getCptySn() != null && !"".equalsIgnoreCase(request.getCptySn())) {
			condition  += ( "".equalsIgnoreCase(condition) ? "" : " and " ) +  " cpty_sn = ? ";  
		}
		if ( request.getCdtyCode() != null && !"".equalsIgnoreCase(request.getCdtyCode())) {
			condition  +=  ( "".equalsIgnoreCase(condition) ? "" : " and " ) +  " cdty_code = ? ";  
		}
		if ( request.getTradeStartDate() != null ) {
			condition  +=  ( "".equalsIgnoreCase(condition) ? "" : " and " ) +  " trade_dt >= ? ";
			Logger.getLogger(TradeProcessor.class).info("Start Date = " + request.getTradeStartDate());
		}
		if ( request.getTradeEndDate() != null ) {
			condition  +=  ( "".equalsIgnoreCase(condition) ? "" : " and " ) +  " trade_dt <= ? ";
			Logger.getLogger(TradeProcessor.class).info("Start Date = " + request.getTradeEndDate());
		}
		
		if ( request.getTradeIdList() != null && !"".equalsIgnoreCase(request.getTradeIdList())) {
			condition  +=  ( "".equalsIgnoreCase(condition) ? "" : " and " ) +  " trade_id in ( " + request.getTradeIdList() + ") ";  
		}
		
		if (!_ALL_COMPANY_IND.equalsIgnoreCase(securityInfo[0])) {
			condition  +=  ( "".equalsIgnoreCase(condition) ? "" : " and " ) +  " migrate_ind in ( " + securityInfo[0] + ") ";
		}

		if (!"".equalsIgnoreCase(condition)){
			sql += " where " + condition;
			sqlCount += " where " + condition;
		}
		
		
		PreparedStatement stmt = null;
		PreparedStatement stmtCount = null;
		int paramIndex = 1;
		Logger.getLogger(TradeProcessor.class).info(sql);
		stmt = this.affinityConnection.prepareStatement(sql);
		stmtCount = this.affinityConnection.prepareStatement(sqlCount);
		if ( request.getTradingSystem()  != null && !"".equalsIgnoreCase(request.getTradingSystem())) {
			stmtCount.setString(paramIndex, request.getTradingSystem());
			stmt.setString(paramIndex++, request.getTradingSystem());  
		}
		if ( request.getRbsSempraSn() != null && !"".equalsIgnoreCase(request.getRbsSempraSn())) {
			stmtCount.setString(paramIndex, request.getRbsSempraSn());
			stmt.setString(paramIndex++, request.getRbsSempraSn());  
		}
		if ( request.getCptySn() != null && !"".equalsIgnoreCase(request.getCptySn())) {
			stmtCount.setString(paramIndex, request.getCptySn());
			stmt.setString(paramIndex++, request.getCptySn());  
		}
		if ( request.getCdtyCode() != null && !"".equalsIgnoreCase(request.getCdtyCode())) {
			stmtCount.setString(paramIndex, request.getCdtyCode());
			stmt.setString(paramIndex++, request.getCdtyCode());  
		}
		if ( request.getTradeStartDate() != null && !"".equals(request.getTradeStartDate()) ) {
			stmtCount.setDate(paramIndex,  java.sql.Date.valueOf(request.getTradeStartDate()));
			stmt.setDate(paramIndex++,   java.sql.Date.valueOf(request.getTradeStartDate()));  
		}
		if ( request.getTradeEndDate() != null && !"".equals(request.getTradeEndDate()) ) {
			stmtCount.setDate(paramIndex, java.sql.Date.valueOf(request.getTradeEndDate()));
			stmt.setDate(paramIndex++,  java.sql.Date.valueOf(request.getTradeEndDate()));  
		}
			
		rsCount = stmtCount.executeQuery();
		rsCount.next();
		if ( rsCount.getInt(1) > maxSize) {
			throw new Exception("Too many rows returned, narrow down your search."); 
		}
		rs = stmt.executeQuery();
		
		return rs;	
	}
	
	private String[] getUserCompanyList(String userName) throws SQLException{
		String[] returnData  = new String[2];
		String companyList = "";
		String accessIndicator = "";
		
		TCUtilProcessor utilProcessor = new TCUtilProcessor((OracleConnection) this.affinityConnection);
		UserCompanyRequest request = new UserCompanyRequest();
		request.setUserName(userName);
		UserCompanyResponse response =  utilProcessor.getUserCompanyList(request);
		accessIndicator = response.getAccessIndicator() ;
		if ( !_ALL_COMPANY_IND.equalsIgnoreCase(accessIndicator)){
			ArrayList<String> migrationIndList = utilProcessor.getMigrateList(accessIndicator);
			if ( migrationIndList == null || migrationIndList.size() == 0)
				{
					 accessIndicator = "'NA'";
				}
			else
			{
				accessIndicator = "";
				for ( int i=0;i<migrationIndList.size();++i){
					if (i !=0 ) {
						accessIndicator += ",";
					}
					accessIndicator += "'" + migrationIndList.get(i) + "'";
				}
			}
		}
		/*
		if (_SEMPRA_TRADES_IND.equalsIgnoreCase(accessIndicator)){
			accessIndicator = "'" + accessIndicator + "','R','D'";
		}
		else {
			accessIndicator = "'" + accessIndicator + "'"; 
		}
		*/
		returnData[0] = accessIndicator;
		ArrayList<String> company = response.getCompanyList();
		for ( int i=0;i<company.size();++i){
			if ( i>0){
				companyList += " , ";
			}
			companyList +=  "'" + company.get(i).replaceAll("'", "''") + "'";
		}
		if ("".equals(companyList)){
			companyList = "NONE";
		}
		returnData[1] = companyList;
		return returnData;
		
	}
	
	public TradeRqmtResponse updateRqmt(TradeRqmtRequest request) throws SQLException{
		TradeRqmtResponse response = new TradeRqmtResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is empty");
			return response;
		}
		
		response.setRequest(request);
		String sp = " { call ops_tracking.pkg_trade_rqmt.p_update_trade_rqmt(?,?,?) }";
		
		try {
			
			CallableStatement stmt = affinityConnection.prepareCall(sp);
			stmt.setLong(1,request.getRqmtId());
			stmt.setString(2,request.getStatus() );
			stmt.setString(3,getFormattedDate(request.getStatusDate()) );
			stmt.execute();
			response.setResponseStatus(BaseResponse.SUCCESS);
				
		}
		catch (SQLException e){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		}
		return response;
	}

	private String getFormattedDate(Date statusDate) {
		String dateStr = "";
		if ( statusDate != null) {
			SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy");
			dateStr = sdf.format(statusDate);
		}
		return dateStr;
	}

	public DealSheetResponse getDealSheet(DealSheetRequest request,String affinityURL,String jmsURL) {
		DealSheetResponse response = new DealSheetResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is empty");
			return response;
		}
		Logger.getLogger(TradeProcessor.class).info("Deal Sheet Request Trade Id = " + request.getTradingSystem() + " " + request.getTradeId());
		try {
			String html = null;
			if ( request.getTradingSystem() == DealSheetRequest._trading_system.JMS) {
				html =	getJMSDealSheet(request,jmsURL);
			}
			else {
				html = getAffDealSheet(request,affinityURL);
			}
			response.setDealSheetHtml(html);
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e) {
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		} catch (IOException e) {
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		}
		
		return response;
	}

	private String getAffDealSheet(DealSheetRequest request, String affinityURL) throws IOException {
		Logger.getLogger(TradeProcessor.class).info("Affinity URL = " + affinityURL);
		String urlAddr = affinityURL + "?TICKET=" + request.getTradeId();
		URL url = new URL(urlAddr);
		URLConnection uc = url.openConnection();
		BufferedReader br = null;
		br = new BufferedReader( new InputStreamReader(uc.getInputStream()));
		StringBuffer sb = new StringBuffer();
		String inputLine = null;
		while  ((inputLine = br.readLine()) != null) {
			sb.append(inputLine + "\n");
		}
		br.close();
		return sb.toString();
		
	}

	private String getJMSDealSheet(DealSheetRequest request,String jmsURL) throws IOException,SQLException {
		String returnHtml = null;
		returnHtml = getJMSDealSheetFromDB(request); // check from dataase
		if ( returnHtml != null) {  // available in the database 
			return returnHtml;
		}
		Logger.getLogger(TradeProcessor.class).info("JMS URL = " + jmsURL);
		String urlAddr = jmsURL + "?TradeNum=" + request.getTradeId();
		URL url = new URL(urlAddr);
		URLConnection uc = url.openConnection();
		BufferedReader br = null;
		br = new BufferedReader( new InputStreamReader(uc.getInputStream()));
		StringBuffer sb = new StringBuffer();
		String inputLine = null;
		while  ((inputLine = br.readLine()) != null) {
			sb.append(inputLine + "\n");
		}
		br.close();
		return sb.toString();
	}

	private String getJMSDealSheetFromDB(DealSheetRequest request) throws SQLException {
		String sql = "select file_data from jbossusr.jms_dealsheet where trade_id = ? order by version desc";
		PreparedStatement stmt = null;
		ResultSet rs = null;
		Clob dealSheet = null;
		String dealSheetHtml = "";
		try {
			stmt = this.affinityConnection.prepareStatement(sql);
			stmt.setLong(1,request.getTradeId());
			rs =stmt.executeQuery();
			if (rs.next()){
				dealSheet = rs.getClob("file_data");
				if (rs.wasNull()) {
				//	dealSheetHtml = "<html><body><b>Deal Sheet is not available for the trade ID "+request.getTradeId() + "." + "</b></body></html>";
					dealSheetHtml = null;
				}
				else {
					dealSheetHtml =dealSheet.getSubString(1, (int) dealSheet.length());
				}
			}
			else {
				dealSheetHtml = "<html><body><b>Trade ID "+request.getTradeId() + " does not exist." + "</b></body></html>";
			}
		}
		finally {
			try {
				if (rs != null) {
					rs.close();
				}
				if (stmt != null) {
					stmt.close();
				}
			}
			catch (SQLException e) {
			}
		}
		return dealSheetHtml;
	}
	
	public JMSTradeProcessResponse replayTrade(JMSTradeProcessRequest request, String workDir, String outputDir){
		JMSTradeProcessResponse response = new JMSTradeProcessResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is empty");
			return response;
		}
		response.setRequest(request);
		String sql = "select file_data from jbossusr.jms_dealsheet where trade_id = ? order by version desc";
		PreparedStatement stmt = null;
		ResultSet rs = null;
		Clob dealSheet = null;
		String dealSheetHtml = "";
		try {
			stmt = this.affinityConnection.prepareStatement(sql);
			stmt.setLong(1,request.getJmsTradeId());
			rs =stmt.executeQuery();
			if (rs.next()){
				dealSheet = rs.getClob("file_data");
				if (rs.wasNull()) {
					dealSheetHtml = "<html><body><b>Deal Sheet is not available." + "</b></body></html>";
				}
				else {
					dealSheetHtml =dealSheet.getSubString(1, (int) dealSheet.length());
				}
				dropFiles(request, dealSheetHtml,workDir,  outputDir);
				response.setResponseStatus(BaseResponse.SUCCESS);
			}
			else {
				response.setResponseStatus(BaseResponse.ERROR);
				response.setResponseText("The trade id does not exist.");
			}
		} catch (SQLException e) {
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
			log.error("replayTrade error : " , e );
		} catch (Exception e) {
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
			log.error("replayTrade error : " , e );
		}
		finally {
			try {
				if (rs != null) {
					rs.close();
				}
				if (stmt != null) {
					stmt.close();
				}
			}
			catch (SQLException e) {
			}
		}
	
		
		return response;
		
	}

	private void dropFiles(JMSTradeProcessRequest request,
			String dealSheetHtml, String workDir, String outputDir) throws Exception {
		
		SimpleDateFormat sdf = new SimpleDateFormat("yyyyMMdd-HHmmss");
		String dateNow = sdf.format(new Date());
		String xmlFileName = request.getJmsTradeId() + "-" + dateNow + "-UPD.XML";
		String htmlFileName = request.getJmsTradeId() + "-" + dateNow + "-UPD.HTML";
		String xmlData = "<Trade TradeNum=\"" + request.getJmsTradeId() + "\"><Operation>UPDATE</Operation></Trade>";
		File xmlFile = new File(workDir + "\\" + xmlFileName);
		File htmlFile = new File(workDir + "\\" + htmlFileName);
		File destXmlFile  = new File(outputDir + "\\" + xmlFileName);
		File destHtmlFile = new File(outputDir + "\\" + htmlFileName);
		try {
			
			BufferedWriter bwXml = new BufferedWriter(new FileWriter(xmlFile));
			bwXml.write(xmlData);
			bwXml.close();
			
			BufferedWriter bwHtml = new BufferedWriter(new FileWriter(htmlFile));
			bwHtml.write(dealSheetHtml);
			bwHtml.close();
			if (destXmlFile.exists()) {
				destXmlFile.delete();
			}
			if ( destHtmlFile.exists()) {
				destHtmlFile.delete();
			}
			xmlFile.renameTo(destXmlFile);
			htmlFile.renameTo(destHtmlFile);
			
		}
		catch (Exception e){
			
			if (destXmlFile.exists()) {
				destXmlFile.delete();
			}
			if ( destHtmlFile.exists()) {
				destHtmlFile.delete();
			}
			throw e;
		}
		
		
	}

    public InboundAttribResponse getInboundAttribList() {
		PreparedStatement stmt = null;
		String sql = "select code,descr from ops_tracking.inbound_attrib order by code ";
		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		InboundAttribResponse response = new InboundAttribResponse();
		ArrayList<InboundAttrib> attribList= new ArrayList<InboundAttrib>();
		response.setData(attribList);
		try {
			stmt = affinityConnection.prepareCall(sql);
			rs = stmt.executeQuery();
			while (rs.next()){
			    InboundAttrib attr = new InboundAttrib();
			    attr.setCode(rs.getString("code"));
			    attr.setDescription(rs.getString("descr"));
			    attribList.add(attr);
				
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getInboundAttribList error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		}
		finally {
			try {
				if (rs != null){
					rs.close();
				}
				if (stmt != null) {
					stmt.close();
				}
			}
			catch (SQLException e) {}
		}
		
		return response;

	}

	public InboundAttribMapResponse updateAttributeMap(InboundAttribMapRequest request ){
		CallableStatement stmt = null;
		String sp = " { call ops_tracking.pkg_inbound_ext.p_update_inbound_attrib_map(?,?,?)}";
		
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		InboundAttribMapResponse response = new InboundAttribMapResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		response.setRequest(request);
		try {
			stmt = affinityConnection.prepareCall(sp);
			stmt.setString(1, request.getAttribCode());
			stmt.setString(2, request.getPhrase());
			stmt.setString(3, request.getOurValue());
			stmt.execute();
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			log.error("User(" + userName + ") updateAttributeMap error : " , e );
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
	
	public TradeCompanyResponse getBookingCompany(TradeCompanyRequest request) throws SQLException{
		TradeCompanyResponse response = new TradeCompanyResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		String sql = "select cm.access_ind from ops_tracking.v_pc_trade_summary pc, ops_tracking.company_migrate cm " +
					" where pc.trade_id = ? and pc.migrate_ind = cm.migrate_ind";
			
		
		PreparedStatement stmt = null;
		try {
			stmt = this.affinityConnection.prepareStatement(sql);
			stmt.setLong(1, request.getTradeId());
			ResultSet rs = stmt.executeQuery();
			
			if (rs.next()){
				response.setCompanyID(rs.getString("access_ind"));
				
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e) {
			log.error("User(" + userName + ") getBookingCompany error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
			throw e;
			
		}
		finally {
			if (stmt != null){
				try { 
					stmt.close();
				}
				catch (SQLException e) {
					
				}
			}
		}
		
		return response;

		
	}
}
