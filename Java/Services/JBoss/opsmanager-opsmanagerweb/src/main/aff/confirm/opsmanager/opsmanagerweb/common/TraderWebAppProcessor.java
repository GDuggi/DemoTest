package aff.confirm.opsmanager.opsmanagerweb.common;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;

import org.jboss.logging.Logger;

import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderGetData;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderGetRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderGetResponse;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderUpdateRequest;
import aff.confirm.opsmanager.opsmanagerweb.data.TraderUpdateResponse;

public class TraderWebAppProcessor {

	private Connection affinityConnection;

	public Connection getAffinityConnection() {
		return affinityConnection;
	}

	public void setAffinityConnection(Connection affinityConnection) {
		this.affinityConnection = affinityConnection;
	}
	
	
	public TraderGetResponse getTraderPendingTrades(TraderGetRequest request,String userName,String pwd) throws SQLException {
		
		String sql = "select /*+index(tr IND_TRDRQMT_RQMT) index(t IDX_TRADE_6) */ " +
					" td.trade_id,td.trade_dt,td.se_cpty_sn,td.cpty_sn, " +
					"td.cdty_grp_code,td.broker_sn,td.buy_sell_ind,td.sttl_type,td.qty_tot, " +
					"tr.id rqmt_id,trc.id rqmt_confirm_id " + 
					"from infinity_mgr.trade t," +
					"infinity_mgr.emp e,ops_tracking.trade_data td,"+
					"ops_tracking.trade_rqmt tr ," + 
					"ops_tracking.trade_rqmt_confirm trc " +
					"where t.TRADER_ID = e.ID " +
					"and e.ACTIVE_FLAG = 'Y' " + 
					"and t.EXP_DT = '31-dec-2299' " + 
					"and t.prmnt_id = td.trade_id " +
					"and t.prmnt_id = tr.trade_id " + 
					"and tr.id = trc.rqmt_id "  +
					"and tr.RQMT = 'XQCSP' " +
					"and tr.status = 'TRADER' " +
					"and trc.confirm_label = 'CONTRACT' " +
					"and trc.active_flag = 'Y'";
		
		if (!"Y".equalsIgnoreCase(request.getGetAllFlag())) { 
			sql = sql + "and e.FRST_NAME like ? " +
				"and e.LST_NAME like ? " ;
		}
		System.out.println("Trade SQL =" + sql);
		Connection connection = null;
		PreparedStatement stmt = null;
		ResultSet rs = null;
						

		connection = getDbConnection(userName,pwd);
		TraderGetResponse response = new TraderGetResponse();
		response.setRequest(request);
		ArrayList<TraderGetData> data = new ArrayList<TraderGetData>();
		response.setData(data);
		
		
		try {
			stmt =  connection.prepareStatement(sql);
			String traderName = request.getTraderName();
			String firstInitial =traderName.substring(0,1) + "%";
			String lastName = traderName.substring(1) + "%";
			Logger.getLogger(TraderWebAppProcessor.class).info("First Name= " + firstInitial + "; Last name =" + lastName);
			if (!"Y".equalsIgnoreCase(request.getGetAllFlag())) {
				stmt.setString(1, firstInitial);
				stmt.setString(2, lastName);
			}
			rs = stmt.executeQuery();
			
			while (rs.next()){
				TraderGetData tgd = new TraderGetData();
				tgd.setTradeId(rs.getLong("trade_id"));
				tgd.setTradeDate(rs.getDate("trade_dt"));
				tgd.setCptySn(rs.getString("cpty_sn"));
				tgd.setBrokerSn(rs.getString("broker_sn"));
				tgd.setSeCmpnySn(rs.getString("se_cpty_sn"));
				tgd.setBuySell(rs.getString("buy_sell_ind"));
				tgd.setCptyGroupCode(rs.getString("cdty_grp_code"));
				tgd.setSttlType(rs.getString("sttl_type"));
				tgd.setTotalQty(rs.getDouble("qty_tot"));
				tgd.setTradeRqmtId(rs.getLong("rqmt_id"));
				tgd.setTradeRqmtConfirmId(rs.getLong("rqmt_confirm_id"));
				data.add(tgd);
			}
			
			response.setResponseStatus(BaseResponse.SUCCESS);
			
		}
		catch (SQLException e){
			Logger.getLogger(this.getClass()).error("User(" + userName + ") getTraderPendingTrades error : " + e.getMessage());
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
				if ( connection != null) {
					connection.close();
				}
			}
			catch (SQLException e) {}
		}
		
		return response;
		
		
	}
	private Connection getDbConnection(String userName, String pwd) throws SQLException {
		Connection conn =null;
		String dbUrl = this.affinityConnection.getMetaData().getURL();
		conn = DriverManager.getConnection(dbUrl, userName, pwd);
		return conn;
	}

	public TraderUpdateResponse updateRqmtStatus(TraderUpdateRequest request,String userName,String pwd) throws SQLException{
			String sql = "update ops_tracking.trade_rqmt " +
						" set status = 'MGR' " +
						" where id = ?";
			PreparedStatement stmt = null;
			ResultSet rs = null;
			Connection connection = null;
			connection = getDbConnection(userName,pwd);
			
			TraderUpdateResponse response = new TraderUpdateResponse();
			response.setRequest(request);
			
			try {
				stmt =  connection.prepareStatement(sql);
				stmt.setLong(1, request.getRqmtId());
				stmt.executeUpdate();
				response.setResponseStatus(BaseResponse.SUCCESS);
			}
			catch (SQLException e){
				Logger.getLogger(this.getClass()).error("User(" + userName + ") updateRqmtStatus error : " + e.getMessage());
				response.setResponseStatus(BaseResponse.ERROR);
				response.setResponseText(e.getMessage());
				response.setResponseStackError(e.getStackTrace().toString());
			}
			finally {
				try {
					
					if (stmt != null) {
						stmt.close();
					}
					if (connection != null) {
						connection.close();
					}
				}
				catch (SQLException e) {}
			}

			return response;
		}
	
	public void checkLogin(String userName,String pwd) throws SQLException {
		Connection connection = null;
		try {
			String dbUrl = this.affinityConnection.getMetaData().getURL();
			connection = DriverManager.getConnection(dbUrl, userName, pwd);
			
		} catch (SQLException e) {
			throw e;
		}
		finally {
			if (connection != null) {
				try {
					connection.close();
				}
				catch (SQLException e){
					
				}
			}
		}
	}
	
	}
