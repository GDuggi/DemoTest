package aff.confirm.opsmanager.confirm.common;

import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.confirm.data.*;
import org.jboss.logging.Logger;

import java.sql.*;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;


public class ConfirmProcessor {
	private static Logger log = Logger.getLogger( ConfirmProcessor.class );
	private SimpleDateFormat sdtf = new SimpleDateFormat("dd-MMM-yyyy hh:mm:ss aaa");
	
	private Connection affinityConnection;
	
	public ConfirmProcessor(){}
	
	public ConfirmProcessor(Connection connection){
		this.affinityConnection = connection;
	}
	public Connection getConnection() {
		return affinityConnection;
	}

	public void setConnection(Connection connection) {
		this.affinityConnection = connection;
	}
	
	private SimpleDateFormat sdf = new SimpleDateFormat("dd-MMM-yyyy");
	
	public RqmtConfirmUpdateResponse updateTradeConfirm(RqmtConfirmUpdateRequest request){
		CallableStatement stmt = null;
		Statement seqStmt = null;
		ResultSet rs = null;
		String seqSql = "select ops_tracking.seq_trade_rqmt_confirm.nextval from dual";
		String insertSp = "{ call ops_tracking.pkg_rqmt_confirm.p_insert_rqmt_confirm(?,?,?,?,?,?,?,?,?)}";
		String updateSp = "{ call ops_tracking.pkg_rqmt_confirm.p_update_rqmt_confirm(?,?,?,?,?,?,?,?,?,?)}";
		
		String userName = OpsManagerUtil.getUserName(affinityConnection);
		
		RqmtConfirmUpdateResponse response = new RqmtConfirmUpdateResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		response.setRequest(request);
		try {

			boolean isUpdate = false;
			int paramIndex = 0;
			if (request.getId() <= 0) {
				// checking the trade and rqmt id is in sync
				if ( getTradeRqmtCount(request.getTradeId(), request.getRqmtId()) <= 0) {
					String err = "Error updating Trade Confirm : The trade summary grid value (" + request.getTradeId() + ") and " +
								" rqmt grid value(" + request.getRqmtId() + ") is not in sync. Please contact confirmsupport with this message.";
					throw new SQLException(err);
				}
				stmt = affinityConnection.prepareCall(insertSp);
				seqStmt = affinityConnection.createStatement();
				rs = seqStmt.executeQuery(seqSql);
				rs.next();
				long id = rs.getLong(1);
				request.setId(id);
			}
			else {
				// checking the trade and rqmt id is in sync
				if ( getTradeRqmtConfirmCount(request.getTradeId(), request.getRqmtId(),request.getId()) <= 0) {
					String err = "Error updating Trade Confirm : The trade summary grid value (" + request.getTradeId() + "), " +
								" rqmt grid value(" + request.getRqmtId() + ") and confirm value(" + request.getId() + ") is not in sync. Please contact confirmsupport with this message.";
					throw new SQLException(err);
				}
				stmt = affinityConnection.prepareCall(updateSp);
				isUpdate = true;
			}
			
			stmt.setLong(++paramIndex, request.getId());
			stmt.setLong(++paramIndex, request.getRqmtId());
			stmt.setLong(++paramIndex, request.getTradeId());
			if ( request.getTemplateId() > 0) {
				stmt.setLong(++paramIndex, request.getTemplateId());
			}
			else {
				stmt.setNull(++paramIndex, java.sql.Types.NUMERIC);
			}
			stmt.setString(++paramIndex, request.getConfirmCmt());
			String transMode = request.getFaxTelexInd();
			stmt.setString(++paramIndex, transMode);
			stmt.setString(++paramIndex, request.getFaxTelexNumber());
			stmt.setString(++paramIndex, request.getConfirmLabel());
			stmt.setString(++paramIndex, request.getNextStatusCode());

			//Israel 1/29/15 - Update now has one more param than insert
			if (isUpdate)
			{
				String activeFlag = "Y";
				if (request.getConfirmLabel().equalsIgnoreCase("CONTRACT") &&
						request.getConfirmCmt().equalsIgnoreCase("RESET TO NEW"))
					activeFlag = "N";
				stmt.setString(++paramIndex, activeFlag);
			}

			stmt.execute();
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			log.error("User(" + userName + ") updateTradeConfirm error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		}
		finally {
			try {
				if (seqStmt != null){
					seqStmt.close();
				}
				if (stmt != null) {
					stmt.close();
				}
			}
			catch (SQLException e) {}
		}
		
		return response;
	}
	
	private int getTradeRqmtCount(long tradeId, long rqmtId) throws SQLException {

		PreparedStatement stmt = null;
		ResultSet rs = null;
		String sql = "select id from ops_tracking.trade_rqmt where trade_id = ? and id = ?";      
		int cnt = 0;
		log.info("Validating Trade Rqmt for trade id = " + tradeId + " rqmt id = " + rqmtId );
		try {
			stmt = affinityConnection.prepareStatement(sql);
			stmt.setLong(1, tradeId);
			stmt.setLong(2, rqmtId);
			rs = stmt.executeQuery();
			if (rs.next()){
				cnt = 1;
			}
			
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
		
		return cnt;

	}
	
	private int getTradeRqmtConfirmCount(long tradeId, long rqmtId,long tradeRqmtConfirmId) throws SQLException {

		PreparedStatement stmt = null;
		ResultSet rs = null;
		String sql = "select id from ops_tracking.trade_rqmt_confirm where trade_id = ? and rqmt_id = ? and id = ? and active_flag = ?";
		int cnt = 0;
		log.info("Validating Trade Rqmt Confirm for trade id = " + tradeId
				+ " rqmt id = " + rqmtId + " confirm id = " + tradeRqmtConfirmId);
		try {
			stmt = affinityConnection.prepareStatement(sql);
			stmt.setLong(1, tradeId);
			stmt.setLong(2, rqmtId);
			stmt.setLong(3, tradeRqmtConfirmId);
			//If you hard-code the value in the above statement it forces a regeneration on the DB server each time it is called.
			stmt.setString(4, "Y");
			rs = stmt.executeQuery();
			if (rs.next()){
				cnt = 1;
			}
			
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
		
		return cnt;

	}
	
	public ClauseHeaderResponse getConfirmClauseHeader(){
		
		PreparedStatement stmt = null;
		ResultSet rs = null;
		String sql = "select * from infinity_mgr.vcnfrmcls where active_flag = 'Y'" ;      


		String userName = OpsManagerUtil.getUserName(affinityConnection);
	
		ClauseHeaderResponse response = new ClauseHeaderResponse();
		ArrayList<ClauseHeaderData> data = new ArrayList<ClauseHeaderData>();
		response.setData(data);
		try {
			stmt = affinityConnection.prepareStatement(sql);
			rs = stmt.executeQuery();
			while (rs.next()){
				ClauseHeaderData cd = new ClauseHeaderData();
				cd.setLutId(rs.getLong("lut_id"));
				cd.setQuickCode(rs.getString("quick_code"));
				cd.setShortName(rs.getString("short_name"));
				cd.setPrmntConfirmClauseId(rs.getLong("prmnt_confirm_clause_id"));
				cd.setCategory(rs.getString("category"));
				cd.setTokenString(rs.getString("token_string"));
				cd.setComment(rs.getString("cmt"));
				data.add(cd);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
			
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getConfirmClauseHeader error : " , e );
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
	
	
	public ClauseBodyResponse getConfirmClauseBody(){
		
		PreparedStatement stmt = null;
		ResultSet rs = null;
		String sql = "select * from infinity_mgr.vcnfrmclsbd where active_flag = 'Y'" ;      


		String userName = OpsManagerUtil.getUserName(affinityConnection);
	
		ClauseBodyResponse response = new ClauseBodyResponse();
		ArrayList<ClauseBodyData> data = new ArrayList<ClauseBodyData>();
		response.setData(data);
		try {
			stmt = affinityConnection.prepareStatement(sql);
			rs = stmt.executeQuery();
			while (rs.next()){
				ClauseBodyData cd = new ClauseBodyData();
				cd.setLutId(rs.getLong("lut_id"));
				cd.setPrmntConfirmClauseId(rs.getLong("prmnt_confirm_clause_id"));
				cd.setSegment(rs.getInt("segment"));
				cd.setBody(rs.getString("body"));
				data.add(cd);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getConfirmClauseBody error : " , e );
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

	public InsertClauseResponse[] updateClause(InsertClauseRequest[] request) {
		
		
		InsertClauseResponse[] response = null;
		String sql = "insert into confirm.clauses_used (id,category,clause,user_id,cr_timestamp)  " +
          	" values (confirm.seq_clauses_used.nextval,?,?,?,Sysdate )";

		String userName = OpsManagerUtil.getUserName(affinityConnection);

		if (request == null){
			return response;
		}
		
		PreparedStatement stmt = null;
		int i;
		
		response = new InsertClauseResponse[request.length];
		for (i=0; i<request.length;++i) {
			response[i] = new InsertClauseResponse();
			response[i].setRequest(request[i]);
		 }
			
		try {
	
			stmt = affinityConnection.prepareStatement(sql);
			for (i=0; i<request.length;++i){
				try {
					stmt.setString(1, request[i].getCategory());
					stmt.setString(2, request[i].getUserId());
					stmt.setString(3, request[i].getUserId());
					stmt.executeUpdate();
					affinityConnection.commit();
					response[i].setResponseStatus(BaseResponse.SUCCESS);
				}
				catch (Exception e){
					log.error("User(" + userName + ") updateClause error : " , e );
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

	public DocVaultResponse storeContract(DocVaultRequest request){
		return null;
	}
	
	public CptyAgreementResponse getCptyAgreements(CptyAgreementRequest request){
		
		String cptyAgreement =null;
		CallableStatement stmt = null;
		String sql = "{ call ? := cpty.pkg_contracts.f_get_cpty_agreement(?,?)}"; ;      


		String userName = OpsManagerUtil.getUserName(affinityConnection);
	
		CptyAgreementResponse response = new CptyAgreementResponse();
		response.setRequest(request);
		try {
			stmt = affinityConnection.prepareCall(sql);
			stmt.registerOutParameter(1, java.sql.Types.VARCHAR);
			stmt.setString(2, request.getCptyShortName());
			stmt.setDate(3, getDate(request.getTradeDate()));
			stmt.execute();
			cptyAgreement = stmt.getString(1);
			response.setAgreementData(cptyAgreement);
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getCptyAgreements error : " , e );
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

	private java.sql.Date getDate(String tradeDate) {
		SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy");
		try {
			Date dt = sdf.parse(tradeDate);
			return new java.sql.Date(dt.getTime());
		} catch (ParseException e) {
			log.error( e );
		}
		return null;
	}
	
	public CptyFaxNumberResponse getCptyFax(CptyFaxNumberRequest request){
		

		PreparedStatement stmt = null;
		ResultSet rs = null;
		String sql = "select phone_type_code,decode(phone_type_code,'FAX','F','TELEX','T','EMAIL','E','PHONE','P','?') fax_telex_ind," +
					 "country_phone_code, area_code, local_number from cpty.phone_number where id IN cpty.pkg_contracts.f_get_contract_phone_number_id(?,?,?)";       

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		CptyFaxNumberResponse response = new CptyFaxNumberResponse();
		response.setRequest(request);
		try {
			stmt =  affinityConnection.prepareStatement(sql);
			stmt.setString(1, request.getCptySn());
			stmt.setString(2,request.getCdtyCode());
			stmt.setString(3, request.getInstrumentType());

		//	stmt.setObject(4,null,OracleTypes.CURSOR);			
			
			rs = stmt.executeQuery();
			if (rs.next()){
				response.setPhoneTypeCode(rs.getString("phone_type_code"));
				response.setPhoneTypeInd(rs.getString("fax_telex_ind"));
				response.setCountryCode(rs.getString("country_phone_code"));
				response.setAreaCode(rs.getString("area_code"));
				response.setLocalNumber(rs.getString("local_number"));
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
			
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getCptyFax error", e);
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
	
	public DesignationResponse getDesignationList() {

		PreparedStatement stmt = null;
		ResultSet rs = null;
		String sql = "select code, descr from cpty.dsgntn where dsgntn_type_code = 'PHONE' " +
					 " and code like 'CF%' and active_flag = 'Y' order by code";       

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		DesignationResponse response = new DesignationResponse();
		ArrayList<DesignationData> data = new ArrayList<DesignationData>();
		response.setData(data);
		try {
			stmt =  affinityConnection.prepareStatement(sql);
			
		//	stmt.setObject(4,null,OracleTypes.CURSOR);			
			
			rs = stmt.executeQuery();
			while (rs.next()){
				DesignationData dd = new DesignationData();
				dd.setCode(rs.getString("code"));
				dd.setDescription(rs.getString("descr"));
				data.add(dd);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
			
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getDesignationList error : " , e );
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
	
	public InfMgrFaxResponse getInfMgrFax(InfMgrFaxrequest request) {

		String faxNumber =null;
		CallableStatement stmt = null;
		String sql = "{ call ? := cpty.pkg_contracts.f_get_old_fax_no(?)}"; ;      


		String userName = OpsManagerUtil.getUserName(affinityConnection);
	
		InfMgrFaxResponse response = new InfMgrFaxResponse();
		response.setRequest(request);
		try {
			stmt = affinityConnection.prepareCall(sql);
			stmt.registerOutParameter(1, java.sql.Types.VARCHAR);
			stmt.setString(2, request.getCptySn());
			stmt.execute();
			faxNumber = stmt.getString(1);
			response.setFaxNumber(faxNumber);
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getInfMgrFax error : " , e );
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
	
	public TraderEmailAddressResponse getTraderEmailAddress(TraderEmailAddressRequest request){

		PreparedStatement stmt = null;
		ResultSet rs = null;
		String sql = "select e.email_addr from infinity_mgr.trade t, infinity_mgr.emp e " +
					" where t.TRADER_ID = e.ID 	and e.ACTIVE_FLAG = 'Y' and t.EXP_DT = '31-dec-2299' " + 
					" and t.prmnt_id = ?";       
						

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		TraderEmailAddressResponse response = new TraderEmailAddressResponse();
		try {
			stmt =  affinityConnection.prepareStatement(sql);
			stmt.setLong(1,request.getTradeId() );
			rs = stmt.executeQuery();
			if (rs.next()){
				response.setEmailAddress(rs.getString("email_addr"));
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
			
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getDesignationList error : " , e );
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
	
	public TradeRqmtConfirmDeleteResponse deleteTradeRqmtConfirm(TradeRqmtConfirmDeleteRequest request){

		PreparedStatement stmt = null;
		String sql = "delete from ops_tracking.trade_rqmt_confirm where id = ?";       
						

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		TradeRqmtConfirmDeleteResponse response = new TradeRqmtConfirmDeleteResponse();
		response.setRequest(request);
		try {
			stmt =  affinityConnection.prepareStatement(sql);
			stmt.setLong(1,request.getTradeRqmtConfirmId());
			stmt.executeUpdate();
			
		}
		catch (SQLException e){
			log.error("User(" + userName + ") deleteTradeRqmtConfirm error : " , e );
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
	
	public CptyInfoResponse getCptyInfo(CptyInfoRequest request){
		
		
		PreparedStatement stmt = null;
		PreparedStatement stmt2 = null;
		
		ResultSet rs = null;
		ResultSet rs2 = null;
		
		String sql = "select short_name,legal_name,str_addr_1,str_addr_2, " +
					"str_addr_3,city,state_prov_code, country_code,postal_code  " +
					" from cpty.v_cpty_address where short_name=?";
		String phoneSql = "select phone_type_code,country_phone_code,area_code,local_number " +
						" from cpty.v_cpty_phone_fax where short_name = ?";
						

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		CptyInfoResponse response = new CptyInfoResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		
		response.setRequest(request);
		try {
			stmt =  affinityConnection.prepareStatement(sql);
			stmt.setString(1,request.getCptySn());
			rs  = stmt.executeQuery();
			if (rs.next()){
				response.setCptySn(rs.getString("short_name"));
				response.setLegalName(rs.getString("legal_name"));
				response.setAddress1(rs.getString("str_addr_1"));
				response.setAddress2(rs.getString("str_addr_2"));
				response.setAddress3(rs.getString("str_addr_3"));
				response.setCity(rs.getString("city"));
				response.setState(rs.getString("state_prov_code"));
				response.setCountry(rs.getString("country_code"));
				response.setZip(rs.getString("postal_code"));
				
			}
			stmt2 = affinityConnection.prepareStatement(phoneSql);
			stmt2.setString(1, request.getCptySn());
			rs2 = stmt2.executeQuery();
			while ( rs2.next()){
				if ("FAX".equalsIgnoreCase(rs2.getString("phone_type_code"))){
					response.setFaxCountryCode(rs2.getString("country_phone_code"));
					response.setFaxAreaCode(rs2.getString("area_code"));
					response.setFaxNumber(rs2.getString("local_number"));
				}
				else if ("PHONE".equalsIgnoreCase(rs2.getString("phone_type_code"))) {
					response.setPhoneCountryCode(rs2.getString("country_phone_code"));
					response.setPhoneAreaCode(rs2.getString("area_code"));
					response.setPhoneNumber(rs2.getString("local_number"));
				}
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getCptyInfo error : " , e );
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
				if (rs2 != null){
					rs2.close();
				}
				if (stmt2 != null) {
					stmt2.close();
				}
			}
			catch (SQLException e) {}
		}
		
		return response;
	
	}
	
	public AgreementInfoResponse getCptyAgreementList(AgreementInfoRequest request){
	
		PreparedStatement stmt = null;
		
		ResultSet rs = null;
		
		String sql = "select agrmnt_type_code,status_ind,date_signed,termination_dt,se_agrmnt_contact_name," +
					 "cmt,id,se_cpty_id,cpty_id,se_cpty_sn,cpty_sn " +
					 "from cpty.v_cpty_agreements " +
					 "where cpty_sn = ?";
						

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		AgreementInfoResponse response = new AgreementInfoResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		
		response.setRequest(request);
		ArrayList<AgreementData> data = new ArrayList<AgreementData>();
		response.setData(data);
		try {
			stmt =  affinityConnection.prepareStatement(sql);
			stmt.setString(1,request.getCptySn());
			rs  = stmt.executeQuery();
			while (rs.next()){
				AgreementData ad = new AgreementData();
				ad.setAgreementTypeCode(rs.getString("agrmnt_type_code"));
				ad.setStatusInd(rs.getString("status_ind"));
				ad.setDateSigned(getDateString(rs.getDate("date_signed")));
				ad.setTerminationDate(getDateString(rs.getDate("termination_dt")));
				ad.setContactName(rs.getString("se_agrmnt_contact_name"));
				ad.setComment(rs.getString("cmt"));
				ad.setAgreementId(rs.getLong("id"));
				ad.setSempraCompanyId(rs.getLong("se_cpty_id"));
				ad.setCptyId(rs.getLong("cpty_id"));
				ad.setSempraCompanySn(rs.getString("se_cpty_sn"));
				ad.setCptySn(rs.getString("cpty_sn"));
				data.add(ad);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getCptyAgreements error : " , e );
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
	private String getDateString(Date dt){
		String returnStr = "";
		if (dt != null) {
			returnStr =sdf.format(dt); 
		}
		return returnStr;
	}
	public ContractFaxResponse getContractFaxList(ContractFaxRequest request){
		
		PreparedStatement stmt = null;
		
		ResultSet rs = null;
		
		String sql = "select cpty_id,short_name,phone_id,phone_type_code,active_flag, " +
					"country_phone_code,area_code,local_number,dsgntn_code,dsgactive_flag,descr " +
					"from cpty.v_cpty_contract_fax " +
					"where short_name = ?";
						

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		ContractFaxResponse response = new ContractFaxResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		
		response.setRequest(request);
		ArrayList<ContractFaxData> data = new ArrayList<ContractFaxData>();
		response.setData(data);
		try {
			stmt =  affinityConnection.prepareStatement(sql);
			stmt.setString(1,request.getCptySn());
			rs  = stmt.executeQuery();
			while (rs.next()){
				ContractFaxData cfd = new ContractFaxData();
				cfd.setCptyId(rs.getLong("cpty_id"));
				cfd.setShortName(rs.getString("short_name"));
				cfd.setPhoneId(rs.getLong("phone_id"));
				cfd.setPhoneTypeCode(rs.getString("phone_type_code"));
				cfd.setActiveFlag(rs.getString("active_flag"));
				cfd.setCountryPhoneCode(rs.getString("country_phone_code"));
				cfd.setAreaCode(rs.getString("area_code"));
				cfd.setLocalNumber(rs.getString("local_number"));
				cfd.setDesignationCode(rs.getString("dsgntn_code"));
				cfd.setDsgActiveFlag(rs.getString("dsgactive_flag"));
				cfd.setDescription(rs.getString("descr"));
				data.add(cfd);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getContractFaxList error : " , e );
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


	public FaxLogSentResponse[] insertFaxLogSent(FaxLogSentRequest[] request) {
		
		
		FaxLogSentResponse[] response = null;
		String sql = "{ call ops_tracking.pkg_rqmt_confirm.p_insert_fax_log_sent(?,?,?,?,?,?,?) }";

		String userName = OpsManagerUtil.getUserName(affinityConnection);

		if (request == null){
			return response;
		}
		
		CallableStatement stmt = null;
		int i;
		
		response = new FaxLogSentResponse[request.length];
		for (i=0; i<request.length;++i) {
			response[i] = new FaxLogSentResponse();
			response[i].setRequest(request[i]);
		 }
			
		try {
	
			stmt = affinityConnection.prepareCall(sql);
			for (i=0; i<request.length;++i){
				try {
					stmt.setLong(1, request[i].getTradeId());
					String docType = "CNF";
					if ( request[i].getDocType() == FaxLogSentRequest._DOC_TYPE.INB) {
						docType = "INB";
					}
					stmt.setString(2, docType);
					stmt.setString(3, request[i].getSender());
					String telexType = "EMAIL";
					if ( request[i].getFaxTelexCode() == FaxLogSentRequest._FAX_TELEX_CODE.FAX ) {
						telexType = "FAX";
					}
					else if ( request[i].getFaxTelexCode() == FaxLogSentRequest._FAX_TELEX_CODE.TELEX ){
						telexType = "TELEX";
					}
					stmt.setString(4, telexType);
					stmt.setString(5, request[i].getFaxTelexNumber());
					stmt.setString(6, request[i].getDocTypeRef());
					stmt.setLong(7, request[i].getAssociatedDocId());
					
					stmt.executeUpdate();
					affinityConnection.commit();
					response[i].setResponseStatus(BaseResponse.SUCCESS);
				}
				catch (Exception e){
					log.error("User(" + userName + ") insertFaxLogSent error : " , e );
					response[i].setResponseStatus(BaseResponse.ERROR);
					response[i].setResponseText(e.getMessage());
					response[i].setResponseStackError(e.getStackTrace().toString());
				}
			}
		} catch (SQLException e) {
			log.error("User(" + userName + ") insertFaxLogSent error : " , e );
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
		
		return response;


	}


	public FaxStatusLogResponse getFaxGatewayStatusHistory(FaxStatusLogRequest request) {
		
		PreparedStatement stmt = null;
		
		ResultSet rs = null;
		
		String sql = "select id,trade_id,trade_rqmt_confirm_id,sender,crtd_ts_gmt," +
					 "fax_telex_ind,fax_telex_number,cmt,fax_status " +
					 "from  ops_tracking.fax_log_status " +
					 "where trade_rqmt_confirm_id = ? order by id desc";
						

		String userName = OpsManagerUtil.getUserName(affinityConnection);
		FaxStatusLogResponse response = new FaxStatusLogResponse();
		if (request == null){
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		
		response.setRequest(request);
		ArrayList<FaxStatusLogData> data = new ArrayList<FaxStatusLogData>();
		response.setData(data);
		try {
			stmt =  affinityConnection.prepareStatement(sql);
			stmt.setLong(1,request.getTradeRqmtConfirmId());
			rs  = stmt.executeQuery();
			while (rs.next()){
				FaxStatusLogData fsld = new FaxStatusLogData();
				fsld.setId(rs.getLong("id"));
				fsld.setTradeId(rs.getLong("trade_id"));
				fsld.setComment(rs.getString("cmt"));
				fsld.setFaxTelexInd(rs.getString("fax_telex_ind"));
				fsld.setFaxTelexNumber(rs.getString("fax_telex_number"));
				fsld.setSender(rs.getString("sender"));
				fsld.setStatus(rs.getString("fax_status"));
				fsld.setStatusUpdateDateTime(getTimeFormat(new Date(rs.getTimestamp("crtd_ts_gmt").getTime())));
				fsld.setTradeRqmtConfirmId(rs.getLong("trade_rqmt_confirm_id"));
				data.add(fsld);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e){
			log.error("User(" + userName + ") getFaxGatewayStatus error : " , e );
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

	private String getTimeFormat(Date date) {
		String returnStr = "";
		
		if ( date != null){
			returnStr= sdtf.format(date);
		}
		return returnStr;
	}
}
