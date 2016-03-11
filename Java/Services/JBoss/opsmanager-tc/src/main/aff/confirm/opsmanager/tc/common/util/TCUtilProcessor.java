package aff.confirm.opsmanager.tc.common.util;

import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.tc.data.util.*;
import aff.confirm.opsmanager.tc.data.util.EFETConfirmRequest._Entity_Type;
import org.jboss.logging.Logger;

import java.sql.*;
import java.util.ArrayList;


public class TCUtilProcessor {
	private static Logger log = Logger.getLogger(TCUtilProcessor.class);
	private Connection affinityConnection;
	private static final String _BOTH_IND = "B";
	
	public void setDbConnection(Connection connection){
		this.affinityConnection = connection;
	}
	public TCUtilProcessor(Connection connection){
		this.affinityConnection = connection; 
	}
	
	private ArrayList<String> cdtyGrpData = null; 
	
	public BaseResponse processEConfirmRequest(int tradeId, String status) throws SQLException {
		
		BaseResponse resp = new BaseResponse();
		String sp = " { call econfirm.pkg_ext_notify.p_ok_to_resubmit(?,?) }";
		CallableStatement stmt;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		try {
			stmt = affinityConnection.prepareCall(sp);
			stmt.setInt(1, tradeId);
			stmt.setString(2, status);
			stmt.executeUpdate();
			resp.setResponseStatus(BaseResponse.SUCCESS);
					
		} catch (SQLException e) {
			log.error("User(" + userName + ") processEConfirmRequest error : " , e );
			resp = OpsManagerUtil.populateErrorMessage(resp,e);
			if (affinityConnection.isClosed()){
				throw e;
			}
		}
		
		return resp;
	}
	
	public ArrayList<EConfirmResponse> processEConfirmRequest(ArrayList<EConfirmRequest> tradeList) throws Exception{
		
		ArrayList<EConfirmResponse> respList = new ArrayList<EConfirmResponse>();
		if (tradeList == null) {
			throw new Exception("The incoming parameter is empty");
		}
		
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		EConfirmResponse resp = null;
		int length = tradeList.size();
		String sp = " { call econfirm.pkg_ext_notify.p_ok_to_resubmit(?,?) }";
		CallableStatement stmt;
		try {
			stmt = affinityConnection.prepareCall(sp);
			for ( int i = 0; i<length; i++){
				try {
					stmt.setLong(1, tradeList.get(i).getTradeId());
					stmt.setString(2, tradeList.get(i).getStatus());
					int recUpdated = stmt.executeUpdate();
					resp = new EConfirmResponse();
					resp.setTradeId(tradeList.get(i).getTradeId());
					resp.setRequest(tradeList.get(i));
					if ( recUpdated > 0) {
						resp.setResponseStatus(BaseResponse.SUCCESS);
					}
					else {
						resp.setResponseStatus(BaseResponse.WARN);
						resp.setResponseText("No rows updated in DB");
					}
					respList.add(resp);
				}
				catch (SQLException e) {
					Logger.getLogger(this.getClass()).error("User(" + userName + ") processEConfirmRequest error : " , e );
					resp = new EConfirmResponse();
					resp.setRequest(tradeList.get(i));
					resp.setTradeId(tradeList.get(i).getTradeId());
					resp = (EConfirmResponse) OpsManagerUtil.populateErrorMessage(resp,e);
					respList.add(resp);
					if (affinityConnection.isClosed()){
						throw e;
					}
				} 
			}			
			} catch (SQLException e) {
				Logger.getLogger(this.getClass()).error("User(" + userName + ") processEConfirmRequest error : " , e );
				resp = new EConfirmResponse();
				resp = (EConfirmResponse) OpsManagerUtil.populateErrorMessage(resp,e);
				respList.add(resp);
				if (affinityConnection.isClosed()){
					throw e;
				}
		}
		return respList;
		
	}
	public BaseResponse processEEFTRequest(int tradeId, String status) throws SQLException {
		
		BaseResponse resp = new BaseResponse();
		String sp = " { call efet.pkg_ext_notify.p_ok_to_resubmit(?,?) }";
		CallableStatement stmt;
		
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		try {
			stmt = affinityConnection.prepareCall(sp);
			stmt.setInt(1, tradeId);
			stmt.setString(2, status);
			stmt.executeUpdate();
			resp.setResponseStatus(BaseResponse.SUCCESS);
					
		} catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") processEEFTRequest error : " , e );
			resp = OpsManagerUtil.populateErrorMessage(resp,e);
			if (affinityConnection.isClosed()){
				throw e;
			}
		}
		
		return resp;
	}
	
	public ArrayList<EFETConfirmResponse> processEEFTRequest(ArrayList<EFETConfirmRequest> tradeList) throws SQLException,Exception{
		
		ArrayList<EFETConfirmResponse> respList = new ArrayList<EFETConfirmResponse>();
		if (tradeList == null) {
			throw new Exception("The incoming parameter is empty");
		}
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		EFETConfirmResponse resp = null;
		int length = tradeList.size();
		String sp = " { call efet.pkg_ext_notify.p_ok_to_resubmit(?,?,?) }";
		CallableStatement stmt;
		try {
			stmt = affinityConnection.prepareCall(sp);
			for ( int i = 0; i<length; i++){
				try {
					stmt.setLong(1, tradeList.get(i).getTradeId());
					String entityType = "A";
					if ( tradeList.get(i).getEntityType() == _Entity_Type.Cpty) {
						entityType  = "C";
					}
					else if ( tradeList.get(i).getEntityType() == _Entity_Type.Broker){
						entityType  = "B";
					}
					stmt.setString(2, entityType);
					stmt.setString(3, tradeList.get(i).getStatus());
					int recUpdated = stmt.executeUpdate();
					resp = new EFETConfirmResponse();
					resp.setTradeId(tradeList.get(i).getTradeId());
					resp.setRequest(tradeList.get(i));
					if ( recUpdated > 0) {
						resp.setResponseStatus(BaseResponse.SUCCESS);
					}
					else {
						resp.setResponseStatus(BaseResponse.WARN);
						resp.setResponseText("No rows updated in DB");
					}
					respList.add(resp);
				}
				catch (SQLException e) {
					Logger.getLogger(this.getClass()).error("User(" + userName + ") processEEFTRequest error : " , e );
					resp = new EFETConfirmResponse();
					resp.setRequest(tradeList.get(i));
					resp.setTradeId(tradeList.get(i).getTradeId());
					resp = (EFETConfirmResponse) OpsManagerUtil.populateErrorMessage(resp,e);
					respList.add(resp);
					if (affinityConnection.isClosed()){
						throw e;
					}
				} 
			}			
			} catch (SQLException e) {
				Logger.getLogger(this.getClass()).error("User(" + userName + ") processEEFTRequest error : " , e );
				resp = new EFETConfirmResponse();
				resp = (EFETConfirmResponse) OpsManagerUtil.populateErrorMessage(resp,e);
				respList.add(resp);
				if (affinityConnection.isClosed()){
					throw e;
				}
		}
		return respList;
		
	}

	public ArrayList<EConfirmMatchResponse> matchEConfirm(ArrayList<EConfirmMatchRequest> tradeList) throws Exception {

		ArrayList<EConfirmMatchResponse> respList = new ArrayList<EConfirmMatchResponse>();
		if (tradeList == null) {
			throw new Exception("The incoming parameter is empty");
		}
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		EConfirmMatchResponse resp = null;
		int length = tradeList.size();
		String sp = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_matched(?, ?, ?) }";
		CallableStatement stmt;
		String sp2 = "{call econfirm.PKG_EXT_NOTIFY.p_update_ec_trade_summary(?, ?, ?,?) }";
		CallableStatement stmt2;
		try {
			stmt = affinityConnection.prepareCall(sp);
			stmt2 =affinityConnection.prepareCall(sp2); 
			for ( int i = 0; i<length; i++){
				try {
					resp = new EConfirmMatchResponse();
					resp.setRequest(tradeList.get(i));
					stmt.setLong(1, tradeList.get(i).getTradeId());
					stmt.setString(2, tradeList.get(i).getCptyRefId());
					stmt.setString(3, tradeList.get(i).getStatusDateStr());
					int recUpdated = stmt.executeUpdate();
					
					stmt2.setLong(1, tradeList.get(i).getTradeId());
					stmt2.setString(2,"MATCHED" );
					stmt2.setString(3, "N");
					stmt2.setString(4, tradeList.get(i).getCptyRefId());
					recUpdated = stmt2.executeUpdate();
					if ( recUpdated > 0) {
						resp.setResponseStatus(BaseResponse.SUCCESS);
					}
					else {
						resp.setResponseStatus(BaseResponse.WARN);
						resp.setResponseText("No rows updated in DB");
					}
					respList.add(resp);
				}
				catch (SQLException e) {
					Logger.getLogger(this.getClass()).error("User(" + userName + ") processEEFTRequest error : " , e );
					resp = new EConfirmMatchResponse();
					resp.setRequest(tradeList.get(i));
					resp = (EConfirmMatchResponse) OpsManagerUtil.populateErrorMessage(resp,e);
					respList.add(resp);
					if (affinityConnection.isClosed()){
						throw e;
					}
				} 
			}			
			} catch (SQLException e) {
				Logger.getLogger(this.getClass()).error("User(" + userName + ") processEEFTRequest error : " , e );
				resp = new EConfirmMatchResponse();
				resp = (EConfirmMatchResponse) OpsManagerUtil.populateErrorMessage(resp,e);
				respList.add(resp);
				if (affinityConnection.isClosed()){
					throw e;
				}
		}
		return respList;
		
	}
	public ECMBoxSubmitResponse submitToBox(ECMBoxSubmitRequest request) throws SQLException{
	
		
		ECMBoxSubmitResponse resp = new ECMBoxSubmitResponse();
		String sql = " insert into ops_tracking.process_control(id,process_mast_code) " +
					" values(ops_tracking.seq_process_control.nextval,?)";
		PreparedStatement stmt;
		resp.setRequest(request);
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		try {
			stmt = affinityConnection.prepareStatement(sql);
			stmt.setString(1, request.getProcessMasterCode());
			stmt.executeUpdate();
			resp.setResponseStatus(BaseResponse.SUCCESS);
			
					
		} catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") processEConfirmRequest error : " , e );
			OpsManagerUtil.populateErrorMessage(resp,e);
			throw e;
		}
		
		return resp;
	}
	
	public UserFilterUpdateResponse updateUserFilter(UserFilterUpdateRequest request) throws SQLException{
		
		UserFilterUpdateResponse response = new UserFilterUpdateResponse();
		String insSQL = "Insert into ops_tracking.user_filters_opsmgr(user_id,descr,filter_expr,id) values " +
						"(?,?,?,?)";
		String updateSQL = "Update ops_tracking.user_filters_opsmgr " +
						   " set user_id = ? ," +
						   " descr =  ? ," +
						   " filter_expr = ? " +
						   "where id = ?";
		String deleteSQL = "Delete from ops_tracking.user_filters_opsmgr where id = ?";
		
		response.setRequest(request);
		PreparedStatement stmt = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		try {
			if (request.getRequestType() == UserFilterUpdateRequest._request_type.Delete ) {
				stmt = affinityConnection.prepareStatement(deleteSQL);
				stmt.setInt(1, request.getFilterId());
			}
			else { 
				if (request.getRequestType() == UserFilterUpdateRequest._request_type.Update) {
					stmt = affinityConnection.prepareStatement(updateSQL);
				}
				else {
					Statement idStmt = affinityConnection.createStatement(); 
					ResultSet rs = idStmt.executeQuery("select ops_tracking.seq_user_filters_opsmgr.nextval from dual");
					rs.next();
					request.setFilterId(rs.getInt(1));
					idStmt.close();
					stmt = affinityConnection.prepareStatement(insSQL);
					
				}
				stmt.setString(1, request.getUserId());
				stmt.setString(2, request.getDescription());
				stmt.setString(3, request.getFilterExpr());
				stmt.setInt(4, request.getFilterId());
			}
			stmt.executeUpdate();
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") updateUserFilter error : " , e );
			throw e;
		}
						   
						   
		return response;
		
	}
	
	public UserFilterGetResponse getUserFilter(UserFilterGetRequest request) throws SQLException{
		
		
		UserFilterGetResponse response = new UserFilterGetResponse();
		String sql = "select id,user_id,descr,filter_expr from  ops_tracking.v_user_filters where user_id = ?";
		
		response.setRequest(request);
		ArrayList<UserFilterData> filterData = new ArrayList<UserFilterData>();
		response.setUserFilters(filterData);
		PreparedStatement stmt = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		try {
				stmt = affinityConnection.prepareStatement(sql);
				stmt.setString(1, request.getUserId());
				ResultSet rs = stmt.executeQuery();
				while (rs.next()){
					UserFilterData ufd = new UserFilterData();
					ufd.setFilterId(rs.getInt("id"));
					ufd.setUserId(rs.getString("user_id"));
					ufd.setDescription(rs.getString("descr"));
					ufd.setFilterExpr(rs.getString("filter_expr"));
					filterData.add(ufd);
				}
				response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") updateUserFilter error : " , e );
			throw e;
		}
		finally {
			if (stmt != null) {
				try {
					stmt.close();
				}
				catch (Exception e) {}
			}
		}
						   
		return response;
	}
	
	public UserRoleResponse getUserRoles(UserRoleRequest request) throws SQLException{

		UserRoleResponse response = new UserRoleResponse();
		String sql = "select user_id,role_code,descr from  ops_tracking.v_active_user_role where user_id = ? ";
		
		response.setRequest(request);
		ArrayList<UserRoleData> roleData = new ArrayList<UserRoleData>();
		response.setRoleList(roleData);
		PreparedStatement stmt = null;
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		try {
				stmt = affinityConnection.prepareStatement(sql);
				stmt.setString(1, request.getUserId());
				 rs = stmt.executeQuery();
				while (rs.next()){
					UserRoleData urd = new UserRoleData();
					urd.setUserId(rs.getString("user_id"));
					urd.setRole(rs.getString("role_code"));
					urd.setDescription(rs.getString("descr"));
					roleData.add(urd);
				}
				response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getUserRoles error : " , e );
			throw e;
		}
		finally {
			if (stmt != null) {
				try {
					if (rs != null) {
						rs.close();
					}
					stmt.close();
					
				}
				catch (Exception e) {}
			}
		}
						   
		return response;
		
	}
	
	
	public CdtyGrpCodeResponse getCdtyGrpCodes() throws SQLException{

		String sql = "Select distinct cdty_grp_code from ops_tracking.trade_data  where cdty_grp_code is not null order by cdty_grp_code";
		CdtyGrpCodeResponse response = new CdtyGrpCodeResponse();
		
		if ( cdtyGrpData != null) {
			Logger.getLogger(this.getClass()).info("getCdtyGrpCodes data returned from cache");
			response.setCdtyGrpCodes(cdtyGrpData);
			return response;
		}
		cdtyGrpData = new ArrayList<String>();
		response.setCdtyGrpCodes(cdtyGrpData);
		PreparedStatement stmt = null;
		ResultSet rs = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		try {
				stmt = affinityConnection.prepareStatement(sql);
				rs = stmt.executeQuery();
				while (rs.next()){
					cdtyGrpData.add(rs.getString("cdty_grp_code"));
				}
				response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getCdtyGrpCodes error : " , e );
			throw e;
		}
		finally {
			try {
				if (rs != null ) {
					rs.close();
				}
				if (stmt != null) {
					stmt.close();
				}
			}
			catch (Exception e) {}
		}
						   
		return response;

	}
	
	public UserCompanyResponse getUserCompanyList(UserCompanyRequest request) throws SQLException {
		String compSql = "select access_ind from ops_tracking.user_access where user_id = ? and active_flag = 'Y'";
		String sql = "select u.company_sn from ops_tracking.user_company u, ops_tracking.company_mast c " +
					" where u.company_sn = c.company_sn and c.active_flag = 'Y' " +
					" and u.user_id = ? and u.active_flag = 'Y'";

		UserCompanyResponse response = new UserCompanyResponse();
		response.setRequest(request);
		 
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
		}
		ArrayList<String> list = new ArrayList<String>();
		ArrayList<String> migrateList = new ArrayList<String>();
		response.setCompanyList(list);
		response.setMigrateIndicator(migrateList);
		PreparedStatement stmt = null;
		PreparedStatement stmt2 = null;
		ResultSet rs = null;
		ResultSet rs2 = null;
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		try {
            stmt2 = affinityConnection.prepareStatement(compSql);
            stmt2.setString(1, request.getUserName().toUpperCase());
            rs2 = stmt2.executeQuery();
            if (rs2.next()) {
                response.setAccessIndicator(rs2.getString("access_ind"));
                response.setMigrateIndicator(getMigrateList(rs2.getString("access_ind")));
            }
            stmt = affinityConnection.prepareStatement(sql);
            stmt.setString(1, request.getUserName().toUpperCase());
            rs = stmt.executeQuery();
            while (rs.next()){
                list.add(rs.getString("company_sn"));
            }
            response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e) {
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getUserCompanyList error : " , e );
			throw e;
		}
		finally {
			try {
				if (rs2 != null){
					rs2.close();
				}
				if (rs != null ) {
					rs.close();
				}
				if (stmt2 != null){
					stmt2.close();
				}
				if (stmt != null) {
					stmt.close();
				}
			}
			catch (Exception e) {
				log.error( "ERROR", e);
			}
		}

		
		return response;
	}
	
	public ArrayList<String> getMigrateList(String accessInd) throws SQLException{
	
		ArrayList<String> migrateList = new ArrayList<String>();
		String sql = "select * from ops_tracking.company_migrate where access_ind = ?";
		PreparedStatement ps = null;
		ResultSet rsAccess = null;
		
		try {
			if (!_BOTH_IND.equalsIgnoreCase(accessInd)){
				ps = this.affinityConnection.prepareStatement(sql);
				ps.setString(1, accessInd);
				rsAccess = ps.executeQuery();
				while (rsAccess.next()){
					migrateList.add(rsAccess.getString("migrate_ind"));
				}
			
			}
		}
		catch (SQLException e){
			log.error("getMigrateList error : " , e );
			throw e;
		}
		finally {
			if (rsAccess != null){
				rsAccess.close();
			}
			if ( ps != null){
				ps.close();
			}
		}
		return migrateList;
		
	}
	

}
