package aff.confirm.opsmanager.inbound.common;

import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.inbound.data.*;
import org.jboss.logging.Logger;

import java.sql.*;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;

public class InboundProcessor {
	private static Logger log = Logger.getLogger( InboundProcessor.class );

	private Connection affinityConnection;
	private static final String _SEMPRA_ACCESS = "S";
	private static final String _JPM_ACCESS = "J";
	private static final String _ALL_ACCESS = "B";
	
	
	private SimpleDateFormat sdfInbDoc = new SimpleDateFormat("M/d/yyyy h:m:s a");
	
	public InboundProcessor(){}
	
	public InboundProcessor(Connection connection){
		this.affinityConnection = connection;
	}
	public Connection getConnection() {
		return affinityConnection;
	}
	public void setConnection(Connection connection) {
		this.affinityConnection = connection;
	}

	public DocStatusResponse[] updateDocStatus(DocStatusRequest[] request) {
	
		DocStatusResponse[] response = null;
		
		String sp = " { call ops_tracking.pkg_inbound.p_update_doc_status(?,?)} ";

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		if (request == null){
			response = new DocStatusResponse[1];
			response[0] = new DocStatusResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
		}
		else {
				CallableStatement stmt = null;
				int i;
				response = new DocStatusResponse[request.length];
				for (i=0; i<request.length;++i) {
					response[i] = new DocStatusResponse();
					response[i].setRequest(request[i]);
				 }
				
			try {
				
				 stmt = affinityConnection.prepareCall(sp);
				 for (i=0; i<request.length;++i){
					 try {
						 stmt.setLong(1, request[i].getId());
						 stmt.setString(2,request[i].getStatusCode());
						 stmt.execute();
						 affinityConnection.commit();
						 response[i].setResponseStatus(BaseResponse.SUCCESS);
					 }
					 catch (Exception e){
						 Logger.getLogger(this.getClass()).error("User(" + userName + ") setDocumentDiscarded error : " , e );
						 response[i].setResponseStatus(BaseResponse.ERROR);
						 response[i].setResponseText(e.getMessage());
						 response[i].setResponseStackError(e.getStackTrace().toString());
					 }
				 }
				 
				
			} catch (SQLException e) {
				Logger.getLogger(this.getClass()).error("User(" + userName + ") setDocumentDiscarded error : " , e );
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

	
	
	public GetUserFlagResponse getUserFlags(GetUserFlagRequest request){
	
		
		PreparedStatement stmt = null;
		String sql = "select id,inbound_doc_id,inbound_user,flag_type,comments from ops_tracking.inbound_doc_user_flag " +
				" where inbound_user = ?";
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		GetUserFlagResponse response = new GetUserFlagResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		response.setRequest(request);
		ArrayList<GetUserFlagData> data = new  ArrayList<GetUserFlagData>();
		response.setUserFlagData(data);
		try {
			stmt = affinityConnection.prepareStatement(sql);
			stmt.setString(1, request.getUserName().toUpperCase());
			rs = stmt.executeQuery();
			while (rs.next()){
				GetUserFlagData gufd = new GetUserFlagData();
				gufd.setId(rs.getInt("id"));
				gufd.setInboundDocId(rs.getInt("inbound_doc_id"));
				gufd.setFlagType(rs.getString("flag_type"));
				gufd.setComments(rs.getString("comments"));
				data.add(gufd);
			}
			
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getUserFlags error : " , e );
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
	private String getCompanyFlag(String userName) throws SQLException{
		String sql = "select access_ind from ops_tracking.user_access where user_id = ? ";
		PreparedStatement ps = null;
		ResultSet rs = null;
		String accessInd = null;
		
		Logger.getLogger(this.getClass()).info("User(" + userName + ") Checking the Company indicator incoming user = " + userName);
		
		try {
			ps = this.affinityConnection.prepareStatement(sql);
			ps.setString(1, userName.toUpperCase());
			rs = ps.executeQuery();
			if (rs.next()){
				accessInd = rs.getString("access_ind");
			}
		}
		finally {
			try {
				if (rs != null){
					rs.close();
				}
				if ( ps != null){
					ps.close();
				}
			}
			catch (Exception e){

			}
		}
		return accessInd;
		
	}
	public UserFlagResponse[] updateUserFlag(UserFlagRequest[] request) {
		
		UserFlagResponse[] response = null;
		
		String sp = " { call ops_tracking.pkg_inbound_ext.p_update_user_flag(?,?,?,?,?)} ";

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		if (request == null){
			response = new UserFlagResponse[1];
			response[0] = new UserFlagResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
		}
		else {
				CallableStatement stmt = null;
				int i;
				response = new UserFlagResponse[request.length];
				for (i=0; i<request.length;++i) {
					response[i] = new UserFlagResponse();
					response[i].setRequest(request[i]);
				 }
				
			try {
				
				 stmt = affinityConnection.prepareCall(sp);
				 for (i=0; i<request.length;++i){
					 try {
						 stmt.setLong(1, request[i].getInboundDocId());
						 stmt.setString(2, request[i].getUserName());
						 stmt.setString(3, request[i].getFlagType());
						 stmt.setString(4, request[i].getComment());
						 if ( request[i].getUpdateDeleteFlag() == UserFlagRequest._update_user_flag.Delete) {
							 stmt.setString(5, "D");
						 }
						 else {
							 stmt.setString(5, "U");
						 }
						 stmt.execute();
						 affinityConnection.commit();
						 response[i].setResponseStatus(BaseResponse.SUCCESS);
					 }
					 catch (Exception e){
						 Logger.getLogger(this.getClass()).error("User(" + userName + ") updateUserFlag error : " , e );
						 response[i].setResponseStatus(BaseResponse.ERROR);
						 response[i].setResponseText(e.getMessage());
						 response[i].setResponseStackError(e.getStackTrace().toString());
					 }
				 }
				 
				
			} catch (SQLException e) {
				Logger.getLogger(this.getClass()).error("User(" + userName + ") updateUserFlag error : " , e );
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

	public InboundUpdateResponse[] updateInboundDoc(InboundUpdateRequest[] request){

		InboundUpdateResponse[] response = null;
		
		String sp = " { call ops_tracking.pkg_inbound.p_update_inbound_doc(?,?,?,?,?,?,?)} ";

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		if (request == null){
			response = new InboundUpdateResponse[1];
			response[0] = new InboundUpdateResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
		}
		else {
				CallableStatement stmt = null;
				int i;
				response = new InboundUpdateResponse[request.length];
				for (i=0; i<request.length;++i) {
					response[i] = new InboundUpdateResponse();
					response[i].setRequest(request[i]);
				 }
				
			try {
				
				 stmt = affinityConnection.prepareCall(sp);
				 for (i=0; i<request.length;++i){
					 try {
						 stmt.setLong(1, request[i].getId());
						 stmt.setString(2,request[i].getCallerRef());
						 stmt.setString(3,request[i].getSentTo());
						 stmt.setString(4,request[i].getFileName());
						 stmt.setString(5,request[i].getSender());
						 stmt.setString(6,request[i].getComment());
						 stmt.setString(7,request[i].getDocStatusCode() );
						 stmt.execute();
						 affinityConnection.commit();
						 response[i].setResponseStatus(BaseResponse.SUCCESS);
					 }
					 catch (Exception e){
						 Logger.getLogger(this.getClass()).error("User(" + userName + ") updateInboundDoc error : " , e );
						 response[i].setResponseStatus(BaseResponse.ERROR);
						 response[i].setResponseText(e.getMessage());
						 response[i].setResponseStackError(e.getStackTrace().toString());
					 }
				 }
				 
				
			} catch (SQLException e) {
				Logger.getLogger(this.getClass()).error("User(" + userName + ") updateInboundDoc error : " , e );
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
	
	public MapCallerReferenceResponse[] mapCallerRef(MapCallerReferenceRequest[] request){

		MapCallerReferenceResponse[] response = null;
		
		String sp = " { call ops_tracking.pkg_inbound_ext.p_map_caller_ref(?,?,?)} ";

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		if (request == null){
			response = new MapCallerReferenceResponse[1];
			response[0] = new MapCallerReferenceResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
		}
		else {
				CallableStatement stmt = null;
				int i;
				response = new MapCallerReferenceResponse[request.length];
				for (i=0; i<request.length;++i) {
					response[i] = new MapCallerReferenceResponse();
					response[i].setRequest(request[i]);
				 }
				
			try {
				
				 stmt = affinityConnection.prepareCall(sp);
				 for (i=0; i<request.length;++i){
					 try {
						 stmt.setString(1,request[i].getCallerRef());
						 stmt.setString(2,request[i].getCptySn());
						 stmt.setString(3,request[i].getRefType());
						 stmt.execute();
						 affinityConnection.commit();
						 response[i].setResponseStatus(BaseResponse.SUCCESS);
					 }
					 catch (Exception e){
						 Logger.getLogger(this.getClass()).error("User(" + userName + ") mapCallerRef error : " , e );
						 response[i].setResponseStatus(BaseResponse.ERROR);
						 response[i].setResponseText(e.getMessage());
						 response[i].setResponseStackError(e.getStackTrace().toString());
					 }
				 }
				 
				
			} catch (SQLException e) {
				Logger.getLogger(this.getClass()).error("User(" + userName + ") mapCallerRef error : " , e );
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
	
	public MapCallerReferenceResponse[] unMapCallerRef(MapCallerReferenceRequest[] request){

		MapCallerReferenceResponse[] response = null;
		
		String sp = " { call ops_tracking.pkg_inbound_ext.p_ummap_caller_ref(?)} ";

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		if (request == null){
			response = new MapCallerReferenceResponse[1];
			response[0] = new MapCallerReferenceResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
		}
		else {
				CallableStatement stmt = null;
				int i;
				response = new MapCallerReferenceResponse[request.length];
				for (i=0; i<request.length;++i) {
					response[i] = new MapCallerReferenceResponse();
					response[i].setRequest(request[i]);
				 }
				
			try {
				
				 stmt = affinityConnection.prepareCall(sp);
				 for (i=0; i<request.length;++i){
					 try {
						 stmt.setString(1,request[i].getCallerRef());
						 stmt.execute();
						 affinityConnection.commit();
						 response[i].setResponseStatus(BaseResponse.SUCCESS);
					 }
					 catch (Exception e){
						 Logger.getLogger(this.getClass()).error("User(" + userName + ") mapCallerRef error : " , e );
						 response[i].setResponseStatus(BaseResponse.ERROR);
						 response[i].setResponseText(e.getMessage());
						 response[i].setResponseStackError(e.getStackTrace().toString());
					 }
				 }
				 
				
			} catch (SQLException e) {
				Logger.getLogger(this.getClass()).error("User(" + userName + ") mapCallerRef error : " , e );
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
	
	public AssociatedDocumentResponse[] createAssociateDocument(AssociatedDocumentRequest[] request){
	

		String colSql = "insert into ops_tracking.associated_docs(id,inbound_docs_id,index_val," +
						"file_name,trade_id,doc_status_code,cdty_group_code,cpty_sn,broker_sn,doc_type_code," +
						"sec_validate_req_flag,trade_rqmt_id,associated_by,associated_dt,final_approved_by," +
						" final_approved_dt,disputed_by,disputed_dt,discarded_by,discarded_dt,vaulted_by," +
						"vaulted_dt) values (ops_tracking.seq_associated_docs.nextval,?,?,?," +
						"?,?,?,?,?,?,?," +
						"?,?,?,?,?," +
						"?,?,?,?,?,?)";
		
		
		AssociatedDocumentResponse[] response = null;
		

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		PreparedStatement stmt = null;
		if (request == null){
			response = new AssociatedDocumentResponse[1];
			response[0] = new AssociatedDocumentResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
		}
		else {
				
				int i;
				response = new AssociatedDocumentResponse[request.length];
				for (i=0; i<request.length;++i) {
					response[i] = new AssociatedDocumentResponse();
					response[i].setRequest(request[i]);
				 }
				
			try {
				
				 stmt = affinityConnection.prepareCall(colSql);
				 for (i=0; i<request.length;++i){
					 
					 try {
						 String docStatusCode = null;
						 String associatedBy = null;
						 Timestamp associatedDate = null;
						 String finalApprovedBy = null;
						 Timestamp finalApprovedDate = null;
						 String disputedBy = null;
						 Timestamp disputedDate = null;
						 String discardedBy = null;
						 Timestamp discardedDate = null;
						 String vaultedBy = null;
						 Timestamp vaultedDate = null;
						 switch (request[i].getDocStatusCode()) {
						 	case Associated:
						 		 docStatusCode = "ASSOCIATED";
						 		 associatedBy = request[i].getAssociatedBy();
						 		 associatedDate = new java.sql.Timestamp(request[i].getAssociatedDate().getTime());
						 		 break;
						 	case UnAssociated:
						 		docStatusCode = "UNASSOCIATED";
						 		associatedBy = request[i].getAssociatedBy();
						 //		 associatedDate = new java.sql.Timestamp(request[i].getAssociatedDate().getTime());
						 		 break;
						 	case PreApproved:
						 		docStatusCode = "PRE-APPROVED";
						 		 associatedBy = request[i].getAssociatedBy();
						 		 associatedDate = new java.sql.Timestamp(request[i].getAssociatedDate().getTime());
						 		break;
						 	case Approved:
						 		docStatusCode = "APPROVED";
						 		finalApprovedBy = request[i].getFinalApprovedBy();
						 		finalApprovedDate = new java.sql.Timestamp(request[i].getFinalApprovedDate().getTime());
						 		break;
						 	case Open:
						 		docStatusCode = "OPEN";
						 		break;
						 	case Closed:
						 		docStatusCode = "CLOSED";
						 		break;
						 	case Discarded:
						 		docStatusCode = "DISCARDED";
						 		discardedBy = request[i].getDiscardedBy();
						 		discardedDate = new java.sql.Timestamp(request[i].getDiscardedDate().getTime());
						 		break;
						 	case Disputed:
						 		docStatusCode = "DISPUTED";
						 		disputedBy = request[i].getDisputedBy();
						 		disputedDate = new java.sql.Timestamp(request[i].getDisputedDate().getTime());
						 		break;
						 	case Reserved:
						 		docStatusCode = "RESERVED";
						 		break;
						 	case Vaulted:
						 		docStatusCode = "VAULTED";
						 		vaultedBy = request[i].getVaultedBy();
						 		vaultedDate = new java.sql.Timestamp(request[i].getVaultedDate().getTime());
							 
						 }
						 
						 stmt.setLong(1, request[i].getInboundDocId());
						 stmt.setLong(2, request[i].getIndexVal());
						 stmt.setString(3, request[i].getFileName());
						 stmt.setLong(4, request[i].getTradeId());
						 stmt.setString(5, docStatusCode);
						 stmt.setString(6, request[i].getCdtyGroupCode());
						 stmt.setString(7, request[i].getCptySn());
						 stmt.setString(8, request[i].getBrokerSn());
						 stmt.setString(9, request[i].getDocTypeCode());
						 stmt.setString(10, request[i].getSecValidateReqFlag());
						 stmt.setLong(11, request[i].getTradeRqmtId());
						 stmt.setString(12, associatedBy);
						 stmt.setTimestamp(13, associatedDate);
						 stmt.setString(14, finalApprovedBy);
						 stmt.setTimestamp(15, finalApprovedDate);
						 stmt.setString(16, disputedBy);
						 stmt.setTimestamp(17, disputedDate);
						 stmt.setString(18, discardedBy);
						 stmt.setTimestamp(19, discardedDate);
						 stmt.setString(20, vaultedBy);
						 stmt.setTimestamp(21, vaultedDate);
						 stmt.executeUpdate();
						 affinityConnection.commit();
						 response[i].setResponseStatus(BaseResponse.SUCCESS);
					 }
					 catch (Exception e){
						 Logger.getLogger(this.getClass()).error("User(" + userName + ") mapCallerRef error : " , e );
						 response[i].setResponseStatus(BaseResponse.ERROR);
						 response[i].setResponseText(e.getMessage());
						 response[i].setResponseStackError(e.getStackTrace().toString());
					 }
				 }
				 
				
			} catch (SQLException e) {
				Logger.getLogger(this.getClass()).error("User(" + userName + ") mapCallerRef error : " , e );
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
	public AssociatedDocumentResponse[] updateAssociateDocument(AssociatedDocumentRequest[] request){
		

		String colSql = "update ops_tracking.associated_docs " +
						" set inbound_docs_id = ?," +
						" index_val = ?," +
						" file_name = ?, " + 
						" trade_id = ?, " +
						" doc_status_code = ?," +
						" cdty_group_code = ?, "+
						" cpty_sn = ?," +
						" broker_sn = ?," +
						" doc_type_code = ?," +
						" sec_validate_req_flag = ?," +
						" trade_rqmt_id = ?," +
						" associated_by = ?," +
						" associated_dt = ?," +
						" final_approved_by = ?," +
						" final_approved_dt = ?," +
						" disputed_by = ?," +
						" disputed_dt = ?," +
						" discarded_by = ?,"	+
						" discarded_dt = ?, " +
						" vaulted_by = ?," +
						" vaulted_dt = ? " +
						" where id = ?";
						
		
		
		AssociatedDocumentResponse[] response = null;
		

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		PreparedStatement stmt = null;
		if (request == null){
			response = new AssociatedDocumentResponse[1];
			response[0] = new AssociatedDocumentResponse();
			response[0].setResponseStatus(BaseResponse.ERROR);
			response[0].setResponseText("Parameter is null");
		}
		else {
				
				int i;
				response = new AssociatedDocumentResponse[request.length];
				for (i=0; i<request.length;++i) {
					response[i] = new AssociatedDocumentResponse();
					response[i].setRequest(request[i]);
				 }
				
			try {
				 stmt = affinityConnection.prepareCall(colSql);
				 for (i=0; i<request.length;++i){
					 
					 try {
						 String docStatusCode = null;
						 String associatedBy = null;
						 Timestamp associatedDate = null;
						 String finalApprovedBy = null;
						 Timestamp finalApprovedDate = null;
						 String disputedBy = null;
						 Timestamp disputedDate = null;
						 String discardedBy = null;
						 Timestamp discardedDate = null;
						 String vaultedBy = null;
						 Timestamp vaultedDate = null;
						 
						 associatedBy = request[i].getAssociatedBy();
						 if ( request[i].getAssociatedDate() != null ){
							 associatedDate = new java.sql.Timestamp(request[i].getAssociatedDate().getTime());
						 }
						 finalApprovedBy = request[i].getFinalApprovedBy();
						 if ( request[i].getFinalApprovedDate() != null) {
							 finalApprovedDate = new java.sql.Timestamp(request[i].getFinalApprovedDate().getTime());
						 }
				 		 discardedBy = request[i].getDiscardedBy();
				 		 if ( request[i].getDiscardedDate() != null) {
				 			 discardedDate = new java.sql.Timestamp(request[i].getDiscardedDate().getTime());
				 		 }	 
				 		 disputedBy = request[i].getDisputedBy();
				 		 if ( request[i].getDisputedDate() != null ) {
				 			 disputedDate = new java.sql.Timestamp(request[i].getDisputedDate().getTime());
				 		 }
				 		 vaultedBy = request[i].getVaultedBy();
				 		 
				 		 if ( request[i].getVaultedDate() != null) {
				 			 vaultedDate = new java.sql.Timestamp(request[i].getVaultedDate().getTime());
				 		 }
				 
				 		 switch (request[i].getDocStatusCode()) {
						 	case Associated:
						 		 docStatusCode = "ASSOCIATED";
						 		 break;
						 	case UnAssociated:
						 		docStatusCode = "UNASSOCIATED";
						 		 break;
						 	case PreApproved:
						 		docStatusCode = "PRE-APPROVED";
						 		break;
						 	case Approved:
						 		docStatusCode = "APPROVED";
						 		break;
						 	case Open:
						 		docStatusCode = "OPEN";
						 		break;
						 	case Closed:
						 		docStatusCode = "CLOSED";
						 		break;
						 	case Discarded:
						 		docStatusCode = "DISCARDED";
						 		break;
						 	case Disputed:
						 		docStatusCode = "DISPUTED";
						 		break;
						 	case Reserved:
						 		docStatusCode = "RESERVED";
						 		break;
						 	case Vaulted:
						 		docStatusCode = "VAULTED";
						 		
						 }
						 
						 stmt.setLong(1, request[i].getInboundDocId());
						 stmt.setLong(2, request[i].getIndexVal());
						 stmt.setString(3, request[i].getFileName());
						 stmt.setLong(4, request[i].getTradeId());
						 stmt.setString(5, docStatusCode);
						 stmt.setString(6, request[i].getCdtyGroupCode());
						 stmt.setString(7, request[i].getCptySn());
						 stmt.setString(8, request[i].getBrokerSn());
						 stmt.setString(9, request[i].getDocTypeCode());
						 stmt.setString(10, request[i].getSecValidateReqFlag());
						 stmt.setLong(11, request[i].getTradeRqmtId());
						 stmt.setString(12, associatedBy);
						 stmt.setTimestamp(13, associatedDate);
						 stmt.setString(14, finalApprovedBy);
						 stmt.setTimestamp(15, finalApprovedDate);
						 stmt.setString(16, disputedBy);
						 stmt.setTimestamp(17, disputedDate);
						 stmt.setString(18, discardedBy);
						 stmt.setTimestamp(19, discardedDate);
						 stmt.setString(20, vaultedBy);
						 stmt.setTimestamp(21, vaultedDate);
						 stmt.setLong(22, request[i].getId());
						 stmt.executeUpdate();
						 affinityConnection.commit();
						 response[i].setResponseStatus(BaseResponse.SUCCESS);
					 }
					 catch (Exception e){
						 Logger.getLogger(this.getClass()).error("User(" + userName + ") updateAssociateDocument error : " , e );
						 response[i].setResponseStatus(BaseResponse.ERROR);
						 response[i].setResponseText(e.getMessage());
						 response[i].setResponseStackError(e.getStackTrace().toString());
						 log.error( "ERROR", e );
					 }
				 }
				 
				
			} catch (SQLException e) {
				Logger.getLogger(this.getClass()).error("User(" + userName + ") updateAssociateDocument error : " , e );
				for (i=0; i<request.length;++i) {
					 response[i].setResponseStatus(BaseResponse.ERROR);
					 response[i].setResponseText(e.getMessage());
					 response[i].setResponseStackError(e.getStackTrace().toString());
				}
				log.error( "ERROR", e );
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
	public InboundDocCountResponse getInboundDocCount(InboundDocCountRequest request){
		
		
		PreparedStatement stmt = null;
		String allSql = "select count(*) from ops_tracking.associated_docs where inbound_docs_id = ?";
		String statusSql = "select count(*) from ops_tracking.associated_docs where inbound_docs_id = ? and doc_status_code = ?";
		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		InboundDocCountResponse response = new InboundDocCountResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		response.setRequest(request);
		try {
			if (request.getDocStatus() == null || "".equals(request.getDocStatus())) {
				stmt = affinityConnection.prepareStatement(allSql);
				stmt.setLong(1, request.getInboundDocId());
			}
			else {
				stmt = affinityConnection.prepareStatement(statusSql);
				stmt.setLong(1, request.getInboundDocId());
				stmt.setString(2, request.getDocStatus());
			}
			
			rs = stmt.executeQuery();
			if  (rs.next()){
				response.setCount(rs.getInt(1));
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getInboundDocCount error : " , e );
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
	
public InboundUnResolvedCountResponse getInboundUnResolvedDocCount(InboundUnResolvedCountRequest request){
		
		
		PreparedStatement stmt = null;
		String allSql = "select count(*) from ops_tracking.associated_docs where inbound_docs_id = ? and nvl(trade_id,0) =0";
		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		InboundUnResolvedCountResponse response = new InboundUnResolvedCountResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		response.setRequest(request);
		try {
			stmt = affinityConnection.prepareStatement(allSql);
			stmt.setLong(1, request.getInboundDocId());
			rs = stmt.executeQuery();
			if  (rs.next()){
				response.setCount(rs.getInt(1));
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getInboundUnResolvedDocCount error : " , e );
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

	
	public UpdateAssocDocResponse updateAssocStatus(UpdateAssocDocRequest request ){
	
		String docStatusCode = null;
		
		
		CallableStatement stmt = null;
		String sp = " { call ops_tracking.pkg_inbound.p_update_asso_status(?,?,?,?,?,?,?,?,?,?,?,?)} ";
		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		UpdateAssocDocResponse response = new UpdateAssocDocResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		response.setRequest(request);
		switch (request.getDocStatusCode()) {
	 		case Associated:
	 			docStatusCode = "ASSOCIATED";
	 			break;
	 		case UnAssociated:
	 			docStatusCode = "UNASSOCIATED";
	 			break;
	 		case PreApproved:
	 			docStatusCode = "PRE-APPROVED";
	 			break;
	 		case Approved:
	 			docStatusCode = "APPROVED";
	 			break;
	 		case Open:
	 			docStatusCode = "OPEN";
	 			break;
	 		case Closed:
	 			docStatusCode = "CLOSED";
	 			break;
	 		case Discarded:
	 			docStatusCode = "DISCARDED";
	 			break;
	 		case Disputed:
	 			docStatusCode = "DISPUTED";
	 			break;
	 		case Reserved:
	 			docStatusCode = "RESERVED";
	 			break;
	 		case Vaulted:
	 			docStatusCode = "VAULTED";
	 		
		}
		try {
			stmt = affinityConnection.prepareCall(sp);
			stmt.setLong(1, request.getInboundDocId());
			stmt.setString(2, request.getFileName());
			stmt.setLong(3, request.getTradeId());
			stmt.setString(4, docStatusCode);
			stmt.setString(5, request.getCdtyGrpCode());
			stmt.setString(6, request.getCptySn());
			stmt.setString(7, request.getBrokerSn());
			stmt.setLong(8, request.getRqmtId());
			stmt.setString(9, request.getRqmtStatus());
			stmt.setString(10, request.getRqmtType());
			stmt.setString(11, request.getSecondCheck());
			stmt.setInt(12, request.getIndexVal());
			stmt.execute();
			
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") updateAssocStatus error : " , e );
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
	
	public IndexCountResponse getCurrentIndexValue(IndexCountRequest request) {

		
		
		PreparedStatement stmt = null;
		String sql = " select nvl(max(index_val),0) from ops_tracking.associated_docs where inbound_docs_id = ? and doc_status_code not in ( 'UNASSOCIATED') ";
		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		IndexCountResponse response = new IndexCountResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		response.setRequest(request);
		try {
			stmt = affinityConnection.prepareCall(sql);
			stmt.setLong(1, request.getInboundDocId());
			rs = stmt.executeQuery();
			if (rs.next()){
				response.setCurrentIndexValue(rs.getInt(1));
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getCurrentIndexValue error : " , e );
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
	
	public InboundFaxNumberResponse getFaxNumber(String userName) {
		PreparedStatement stmt = null;
		String sql = "select faxno from ops_tracking.inbound_fax_nos where active_flag = 'Y' order by faxno asc";
		String filterSql = "select faxno from ops_tracking.inbound_fax_nos where active_flag = 'Y' and migrate_ind = ? order by faxno asc";
		ResultSet rs = null;
		
		InboundFaxNumberResponse response = new InboundFaxNumberResponse();
		ArrayList<String> faxNumbers= new ArrayList<String>();
		response.setFaxNumbers(faxNumbers);
		try {
			String accessInd = getCompanyFlag(userName);
			//if ( _SEMPRA_ACCESS.equalsIgnoreCase(accessInd) || _JPM_ACCESS.equalsIgnoreCase(accessInd)){
			if ( !_ALL_ACCESS.equalsIgnoreCase(accessInd)) {
				stmt = affinityConnection.prepareCall(filterSql);
				stmt.setString(1, accessInd);
			}
			else {
				stmt = affinityConnection.prepareCall(sql);
			}
			rs = stmt.executeQuery();
			while (rs.next()){
				faxNumbers.add(rs.getString("faxno"));
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getFaxNumber error : " , e );
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
	

	public AttrMapValueResponse getInbAttrMapValues(AttrMapValueRequest request ){
	
		PreparedStatement stmt = null;
		String sql = " select  id,mapped_value,descr from ops_tracking.inb_attrib_map_val where inb_attrib_code = ? and active_flag ='Y'";
		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		AttrMapValueResponse response = new AttrMapValueResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		Logger.getLogger(this.getClass()).info("getInbAttrMapValues : Incoming parameter attribute code " + request.getInboundAttributeCode());
		response.setRequest(request);
		ArrayList<AttrMapValueData> data = new ArrayList<AttrMapValueData>();
		response.setData(data);
		try {
			stmt = affinityConnection.prepareCall(sql);
			stmt.setString(1, request.getInboundAttributeCode());
			rs = stmt.executeQuery();
			while (rs.next()){
				AttrMapValueData attrMapValueData = new AttrMapValueData();
				attrMapValueData.setId(rs.getLong("id"));
				attrMapValueData.setDescr(rs.getString("descr"));
				attrMapValueData.setMappedValue(rs.getString("mapped_value"));
				data.add(attrMapValueData);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getInbAttrMapValues error : " , e );
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
	
	public AttrMapPhraseResponse getInbAttribValPhrases(AttrMapPhraseRequest request ){

		PreparedStatement stmt = null;
		String sql = "select mv.id as mapped_val_id, mv.descr, mp.phrase, mp.id as phrase_id " +
					" from ops_tracking.inb_attrib_map_val mv, ops_tracking.inb_attrib_map_phrase mp " +
					" where mv.active_flag = 'Y' and " +
					" mv.mapped_value = ? and " +
					" mp.inb_attrib_map_val_id = mv.id and " +
					" mp.active_flag = 'Y' " +
					" order by mp.inb_attrib_map_val_id, mp.id asc";

		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		AttrMapPhraseResponse response = new AttrMapPhraseResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		Logger.getLogger(this.getClass()).info("getInbAttribValPhrases : Incoming parameter map value  " + request.getInbAttribMapValue());
		response.setRequest(request);
		ArrayList<AttrMapPhraseData> data = new ArrayList<AttrMapPhraseData>();
		response.setData(data);
		try {
			stmt = affinityConnection.prepareCall(sql);
			stmt.setString(1, request.getInbAttribMapValue());
			rs = stmt.executeQuery();
			while (rs.next()){
				AttrMapPhraseData attrMapValueData = new AttrMapPhraseData();
				attrMapValueData.setPharseid(rs.getLong("pharse_id"));
				attrMapValueData.setInbAttribMapValueId(rs.getLong("mapped_val_id"));
				attrMapValueData.setPhrase(rs.getString("phrase"));
				attrMapValueData.setDescr(rs.getString("descr"));
				data.add(attrMapValueData);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getInbAttribValPhrases error : " , e );
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
	
	public UpdateMapPhraseResponse updateAttribValPhrases(UpdateMapPhraseRequest request ){

		PreparedStatement stmt = null;
		Statement seqStmt = null;
		
		String seqSql = "select ops_tracking.seq_inb_attrib_map_phrase.nextval from dual";
		String insertSql = "insert into ops_tracking.inb_attrib_map_phrase(id,inb_attrib_map_val_id,phrase,active_flag) values (?,?,?,?)";
		String updateSql = " update ops_tracking.inb_attrib_map_phrase " +
						   " set inb_attrib_map_val_id = ?, " +
						   " phrase = ? , " +
						   " active_flag = ? " +
						   " where id = ?";
		String deleteSql = "delete from ops_tracking.inb_attrib_map_phrase where id = ?";
		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		UpdateMapPhraseResponse response = new UpdateMapPhraseResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		Logger.getLogger(this.getClass()).info("updateAttribValPhrases : Incoming parameter map value id " + request.getId() + ";" + request.getInbAttribMapValId() + ";" + request.getPhrase() + ";" + request.getActiveFlag());
		response.setRequest(request);
		try {
			if (request.getId() > 0 ) {
				if ( "Y".equalsIgnoreCase(request.getActiveFlag()) ) {
					stmt = affinityConnection.prepareCall(updateSql);
					stmt.setLong(1, request.getInbAttribMapValId());
					stmt.setString(2, request.getPhrase());
					stmt.setString(3, request.getActiveFlag().toUpperCase());
					stmt.setLong(4, request.getId());
				}
				else {
					stmt = affinityConnection.prepareCall(deleteSql);
					stmt.setLong(1, request.getId());
				}
				stmt.executeUpdate();
			}
			else {
				
				seqStmt = affinityConnection.createStatement();
				rs = seqStmt.executeQuery(seqSql);
				if (rs.next()){
					request.setId(rs.getLong(1));
				}
				stmt = affinityConnection.prepareCall(insertSql);
				stmt.setLong(1, request.getId());
				stmt.setLong(2, request.getInbAttribMapValId());
				stmt.setString(3, request.getPhrase());
				stmt.setString(4, request.getActiveFlag());
				stmt.executeUpdate();
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") updateAttribValPhrases error : " , e );
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
	
	public InbDocStatusResponse updateInbDocStatus(InbDocStatusRequest request){
	
		InbDocStatusResponse response = new InbDocStatusResponse();
		
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		Logger.getLogger(this.getClass()).info("updateInbDocStatus Input Parameter InDoc Id = " + request.getInboundDocId() + "; Status = " + request.getDocStatus());
		
		String sql = "update ops_tracking.inbound_docs " +
					" set doc_status_code = ? " +
					" where id = ?";
		
		PreparedStatement stmt = null;
		response.setRequest(request);
		try {
			stmt = this.affinityConnection.prepareStatement(sql);
			stmt.setString(1, request.getDocStatus());
			stmt.setLong(2, request.getInboundDocId());
			stmt.executeUpdate();
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") updateInbDocStatus error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		}
		finally {
			try {
				if (stmt != null){
					stmt.close();
				}
			}
			catch (Exception e){
				
			}
		}
		return response;
	}
	
	public InbAttrMapValueUpdateResponse updateAttrMapValue(InbAttrMapValueUpdateRequest request){
	
		PreparedStatement stmt = null;
		Statement seqStmt = null;
		
		String seqSql = "select ops_tracking.seq_inb_attrib_map_val.nextval from dual";
		String insertSql = "insert into ops_tracking.inb_attrib_map_val(id,inb_attrib_code,mapped_value,descr,active_flag) values (?,?,?,?,?)";
		String updateSql = " update ops_tracking.inb_attrib_map_val " +
						   " set inb_attrib_code = ? ," +
						   " mapped_value = ?," +
						   " descr = ? " +
						   " active_flag = ? " +
						   " where id = ? ";

		String deleteSql = "delete from ops_tracking.inb_attrib_map_val where id = ?";
		
		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		InbAttrMapValueUpdateResponse response = new InbAttrMapValueUpdateResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		String paramValue = request.getId() + ";" + request.getInbAttribCode() + ";" + request.getMappedValue() + ";" + request.getDesc()+ ";" + request.getActiveFlag(); 
		Logger.getLogger(this.getClass()).info("updateAttrMapValue : Incoming parameter map value id " + paramValue);
		response.setRequest(request);
		try {
			if (request.getId() > 0 ) {
				if ("N".equalsIgnoreCase(request.getActiveFlag())) {
					stmt = affinityConnection.prepareStatement(deleteSql);
					stmt.setLong(1, request.getId());
					Logger.getLogger(this.getClass()).info("updateAttrMapValue : Deleting the row for the id = " + request.getId());
				}
				else {
					stmt = affinityConnection.prepareStatement(updateSql);
					stmt.setString(1, request.getInbAttribCode());
					stmt.setString(2, request.getMappedValue());
					stmt.setString(3, request.getDesc());
					stmt.setString(4, request.getActiveFlag());
					stmt.setLong(5, request.getId());
				}
				stmt.executeUpdate();
			}
			else {
				
				seqStmt = affinityConnection.createStatement();
				rs = seqStmt.executeQuery(seqSql);
				if (rs.next()){
					request.setId(rs.getLong(1));
				}
				stmt = affinityConnection.prepareStatement(insertSql);
				stmt.setLong(1, request.getId());
				stmt.setString(2, request.getInbAttribCode());
				stmt.setString(3, request.getMappedValue());
				stmt.setString(4, request.getDesc());
				stmt.setString(5, request.getActiveFlag());
				stmt.executeUpdate();
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") updateAttrMapValue error : " , e );
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
	public AttrMapPhraseResponse getInbAttribValPhrasesById(AttrMapPhraseRequest request ){

		PreparedStatement stmt = null;
		String sql = "select * from ops_tracking.inb_attrib_map_phrase where inb_attrib_map_val_id = ? and active_flag = 'Y'";
			
		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		AttrMapPhraseResponse response = new AttrMapPhraseResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		Logger.getLogger(this.getClass()).info("getInbAttribValPhrasesById : Incoming parameter map id  " + request.getInboundAttribMapValId());
		response.setRequest(request);
		ArrayList<AttrMapPhraseData> data = new ArrayList<AttrMapPhraseData>();
		response.setData(data);
		try {
			stmt = affinityConnection.prepareCall(sql);
			stmt.setLong(1, request.getInboundAttribMapValId());
			rs = stmt.executeQuery();
			while (rs.next()){
				AttrMapPhraseData attrMapValueData = new AttrMapPhraseData();
				attrMapValueData.setPharseid(rs.getLong("id"));
				attrMapValueData.setInbAttribMapValueId(rs.getLong("inb_attrib_map_val_id"));
				attrMapValueData.setPhrase(rs.getString("phrase"));
				data.add(attrMapValueData);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getInbAttribValPhrasesById error : " , e );
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
	
	public AttrMapValueResponse getInbAttrMapByMapValue(AttrMapValueRequest request ){
		
		PreparedStatement stmt = null;
		String sql = " select  id,mapped_value,descr,inb_attrib_code from ops_tracking.inb_attrib_map_val where mapped_value = ? and active_flag ='Y'";
		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		AttrMapValueResponse response = new AttrMapValueResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		Logger.getLogger(this.getClass()).info("getInbAttrMapByMapValue : Incoming parameter mapped value " + request.getMappedValue());
		response.setRequest(request);
		ArrayList<AttrMapValueData> data = new ArrayList<AttrMapValueData>();
		response.setData(data);
		try {
			stmt = affinityConnection.prepareCall(sql);
			stmt.setString(1, request.getMappedValue());
			rs = stmt.executeQuery();
			while (rs.next()){
				AttrMapValueData attrMapValueData = new AttrMapValueData();
				attrMapValueData.setId(rs.getLong("id"));
				attrMapValueData.setDescr(rs.getString("descr"));
				attrMapValueData.setMappedValue(rs.getString("mapped_value"));
				attrMapValueData.setAttribCode(rs.getString("inb_attrib_code"));
				data.add(attrMapValueData);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getInbAttrMapByMapValue error : " , e );
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

	public InboundDocResponse insertInboundDoc(InboundDocRequest request) {

		PreparedStatement stmt = null;
		Statement seqStmt = null;
		
		String seqSql = "select ops_tracking.seq_inbound_docs.nextval from dual";
		String insertSql = "insert into ops_tracking.inbound_docs(id,caller_ref,sent_to,rcvd_ts,file_name,sender,cmt,doc_status_code,has_auto_ascted_flag,mapped_cpty_sn) values " +
							" (?,?,?,?,?,?,?,?,?,?)";
		

		
		
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		InboundDocResponse response = new InboundDocResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		String paramValue = request.getId() + ";" + request.getCallerRef() + ";" + request.getSentTo() + ";" + request.getRcvdTimeStamp()+ ";" + request.getFileName() + ";" + request.getSender() + ";" + request.getCmt() + ";" + request.getDocStatusCode() + ";" + request.getHasAutoAssocFlag() + ";" + request.getMappedCptySn(); 
		Logger.getLogger(this.getClass()).info("insertInboundDoc : Incoming parameter inbound doc value= " + paramValue);
		response.setRequest(request);
		try {
				seqStmt = affinityConnection.createStatement();
				rs = seqStmt.executeQuery(seqSql);
				if (rs.next()){
					request.setId(rs.getLong(1));
				}
				stmt = affinityConnection.prepareCall(insertSql);
				stmt.setLong(1, request.getId());
				stmt.setString(2, request.getCallerRef());
				stmt.setString(3, request.getSentTo());
				stmt.setTimestamp(4, getTimeStamp(request.getRcvdTimeStamp()));
				stmt.setString(5, request.getFileName());
				stmt.setString(6, request.getSender());
				stmt.setString(7, request.getCmt());
				stmt.setString(8, request.getDocStatusCode());
				stmt.setString(9,request.getHasAutoAssocFlag());
				stmt.setString(10, request.getMappedCptySn());
				stmt.executeUpdate();
				response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") insertInboundDoc error : " , e );
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

	private Timestamp getTimeStamp(String rcvdTimeStamp) {
		Timestamp ts = null;
		
		if (rcvdTimeStamp != null){
			try {
				ts = new Timestamp(sdfInbDoc.parse(rcvdTimeStamp).getTime());
			} catch (ParseException e) {
				log.error( e );
			}
		}
		log.info(" The date & time = " + ts);
		return ts;
	}
	
	}
