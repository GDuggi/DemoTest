package aff.confirm.opsmanager.confirm.common;

import aff.confirm.common.vaultlib.util.VaultInfo;
import aff.confirm.common.vaultlib.util.VaultStatusInfo;
import aff.confirm.common.vaultlib.util.VaultWrapper;
import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.confirm.data.*;
import org.jboss.logging.Logger;
import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
import javax.xml.rpc.ServiceException;
import java.io.IOException;
import java.net.InetAddress;
import java.net.UnknownHostException;
import java.sql.*;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;

public class ContractProcessor {
	private static Logger log = Logger.getLogger( ConfirmProcessor.class );

	private Connection affinityConnection;
	private static final String _SEMPRA_ACCESS = "S";
	private static final String _JPM_ACCESS = "J";
	private static final String _ALL_ACCESS = "B";
	
	
	public ContractProcessor(){}
	
	public ContractProcessor(Connection connection){
		this.affinityConnection = connection;
	}
	public Connection getConnection() {
		return affinityConnection;
	}

	public void setConnection(Connection connection) {
		this.affinityConnection = connection;
	}
	public  ContractResponse storeContract(ContractRequest request,
											String vaultWSUrl, 
											String userName, 
											String password,
											String onBehalfUserName,
											String docRepositorySempra,
											String docRepositoryJPM,
											String dslName,
											String marginToken,
											String creditStatusUrl,
											String creditMarginUr,
											String traderWebUrl,
											String databaseName,
											boolean marginCheck,
											HashMap<String,String> vaultFolders) throws SQLException  {
		
		ContractResponse response = new ContractResponse();
		
		if (request == null) {
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		VaultWrapper vw = null;
		/*
		if (storeDb){
			response = storeContractInDb(request);
		}
		*/
		log.info("Vault Param --> User name " + userName + "; password ->" + password);
		try {
			
			String accessInd = getTradeCompany(request.getTradeId(),request.getTradeRqmtConfirmId());
			// determine the vault location
			String docRepository = docRepositorySempra;
			// 2/8/2011, implemented multiple folders. 
			/*
			if (_JPM_ACCESS.equalsIgnoreCase(accessInd)) {
				docRepository = docRepositoryJPM;
			}*/
			docRepository = vaultFolders.get(accessInd);
			log.info("storeContract(): Vault Folder Name = " + docRepository + " for trade id = " + request.getTradeId() + "; confirm id= " + request.getTradeRqmtConfirmId());
			
			ContractResponse currentContract = getContractFromValult(request, vaultWSUrl,  userName, password, onBehalfUserName, docRepositorySempra,docRepositoryJPM, dslName,vaultFolders);
			
			String currentText =currentContract.getContract(); 
			
			if (currentText == null){
				currentText = "";
			}
			boolean marginTokenReplaced = false;
			
			TradeRqmtDAO tradeRqmtDAO = new TradeRqmtDAO(this.affinityConnection);
			CreditMarginProcessor cmr = new CreditMarginProcessor();
			TradeRqmtRec rec = null;
			boolean marginReplaced = false;
			
			if (marginCheck) { // check whether we need to margin not required for credit margin service
			// check for the trade is approved by the credit
				rec = tradeRqmtDAO.getTradeConfirmId(request.getTradeId(),request.getTradeRqmtConfirmId());
				if ( rec.getTradeRqmtConfirmId() > 0) {
					boolean creditMsgFound = checkCreditMarginMsg(request.getTradeId());
					String replacedMargin = cmr.checkAndReplaceMarginToken(request.getContract(), marginToken, request.getTradeSysCode(), request.getTradeId(), creditStatusUrl, creditMarginUr, rec,creditMsgFound);
					if (replacedMargin != null) {  // margin token exists and the trade is approved, margin replaced.
						request.setContract(replacedMargin);
						marginReplaced = true;
					}
				}
				log.info("Margin Check is done and margin Replaced value " + marginReplaced);
			}	
			
			
			if ("Y".equalsIgnoreCase(request.getSignedFlag()) || !currentText.equalsIgnoreCase(request.getContract())) {
				
				vw = new VaultWrapper(vaultWSUrl,userName,password);
				VaultInfo vi = new VaultInfo();
				vi.setUserName(onBehalfUserName);
				vi.setDocRepository(docRepository);
				vi.setDslName(dslName);
				String fieldValues = getFieldValues(request);
				vi.setFieldNames(VaultInfo._CONTRACT_FIELD_NAMES);
				vi.setFieldValues(fieldValues);
				vi.setData(request.getContract().getBytes());
				log.info("Field Names = " + VaultInfo._CONTRACT_FIELD_NAMES);
				log.info("Field Values = " + fieldValues);
			 	VaultStatusInfo vsi =vw.storeDocument(vi);
				if ( vsi.getResultCode() <0 ) {
					response.setResponseStatus(BaseResponse.ERROR);
					response.setResponseText(vsi.getResultValue());
				}
				else {
					if (marginReplaced) { // if margin is replaced, then update the requirment
						cmr.updateRqtmStatus(tradeRqmtDAO, rec, traderWebUrl, databaseName);
					}
					response.setResponseStatus(BaseResponse.SUCCESS);
				}
				
			}
			else {
				log.info("User(" + userName + ") storeContract is not saved because same contract exists");
				response.setResponseStatus(BaseResponse.SUCCESS);
			}
		} catch (ServiceException e) {
			log.error("User(" + userName + ") storeContract error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		} catch (SAXException e) {
			log.error("User(" + userName + ") storeContract error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		} catch (IOException e) {
			log.error("User(" + userName + ") storeContract error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		} catch (ParserConfigurationException e) {
			log.error("User(" + userName + ") storeContract error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		} catch (SQLException e) {
			log.error("User(" + userName + ") storeContract error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
			throw e;
		}
		
		return response;
		
	}

	private boolean checkCreditMarginMsg(long tradeId) {
		boolean creditMsg = false;
		String sql = "select id from ops_tracking.credit_margin_log where trade_id = ?";
		PreparedStatement ps = null;
		ResultSet rs = null;
		
		try {
			ps = this.affinityConnection.prepareCall(sql);
			ps.setLong(1, tradeId);
			rs = ps.executeQuery();
			if (rs.next()) {
				creditMsg = true;
			}
		}
		catch (SQLException e){
			log.error("User(" +  ") checkCreditMarginMsg error : " , e );
		}
		finally {
			try {
				if ( rs != null) {
					rs.close();
				}
				if ( ps != null) {
					ps.close();
				}
			}
			catch (SQLException e) {
				
			}
		}
		
		return creditMsg;
	}

	public ContractResponse storeContractInDb(ContractRequest request) {
		
		ContractResponse response = new ContractResponse();
		java.sql.Date busnDate  = getBusinessDate();
		try {
			createSession(busnDate,request);
			insertTradeConfirmDb(busnDate,request);
			
		} catch (UnknownHostException e) {
			log.error( "ERROR", e );
		} catch (SQLException e) {
			log.error("ERROR", e);
		}
		
		return response;
		
	}
	
	private void insertTradeConfirmDb(java.sql.Date busnDate,ContractRequest request) throws SQLException {
		
		String nextIdSql = "select infinity_mgr.seq_trdcnfrm.nextval from dual";
		
		String sql = "insert into infinity_mgr.trade_confirm(id,prmnt_trade_id," +
					"confirm_blotasci_id,prmnt_confirm_ad_hoc_id,blotter_no," +
					"confirm_status_ind,busn_dt,action,cpty_prod_area_id,cpty_sn," +
					"trade_dt,trade_version,no_confirm_reason,prmnt_id) values(?," +
					"?,?,null,?,?,?,?,?,?,?,?,?,?)";
		
		String bodySql = "insert into infinity_mgr.trade_confirm_body(prmnt_trade_confirm_id,segment," +
						"active_flag,body) values (?,?,'Y',?)";
		
		PreparedStatement mainStmt = null;
		PreparedStatement bodyStmt = null;
		Statement idStmt = null;
		ResultSet rs  = null;
		long nextCnfmId = 0;
		
		try {
			mainStmt = this.affinityConnection.prepareCall(sql);
			bodyStmt = this.affinityConnection.prepareStatement(bodySql);
			idStmt = this.affinityConnection.createStatement();
			rs = idStmt.executeQuery(nextIdSql);
			if (rs.next()){
				nextCnfmId = rs.getLong(1);
			}
			mainStmt.setLong(1, nextCnfmId);
			mainStmt.setLong(2, request.getTradeId());
			mainStmt.setLong(3, request.getBlotAsciId());
			mainStmt.setString(4, request.getBlotterNo());
			mainStmt.setString(5, request.getConfirmStatusInd());
			mainStmt.setDate(6, busnDate);
			mainStmt.setString(7, request.getAction());
			mainStmt.setLong(8, request.getCptyProdAreaId());
			mainStmt.setString(9, request.getCptySn());
			mainStmt.setDate(10, convertDate(request.getTradeDate()));
			mainStmt.setInt(11, request.getTradeVersion());
			mainStmt.setString(12, request.getNoConfirmReason());
			mainStmt.setLong(13, nextCnfmId);
			mainStmt.executeUpdate();
			log.info("Id = " + nextCnfmId);
			
			int i =1;
			String contractData = request.getContract();
			if ( contractData != null ) {
				while (contractData.length() > 0 ) {
					int length = contractData.length();
					if (length > 2000) {
					length = 2000;
					}
					String updateString = contractData.substring(0, length);
					contractData = contractData.substring(length);
					bodyStmt.setLong(1, nextCnfmId);
					bodyStmt.setInt(2, i);
					bodyStmt.setString(3, updateString);
					bodyStmt.executeUpdate();
					++i;
		
				}
				
			}
			this.affinityConnection.commit();
		}
		finally {
			
		}
		
	}
	
	private java.sql.Date convertDate(String tradeDate) {
		
		java.sql.Date returnDt = null;
		SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy");
		try {
			java.util.Date dt = sdf.parse(tradeDate);
			returnDt = new java.sql.Date(dt.getTime());
			 
		} catch (ParseException e) {
		} 
		return returnDt;
	}

	private void createSession(java.sql.Date busnDate,ContractRequest request) throws SQLException, UnknownHostException {
		
		String sp = " { call infinity_mgr.pkg_misc.populate_session(?,?,?,?,?) }";
		CallableStatement stmt = null;
		String status = null;
		try {
			java.net.InetAddress host = InetAddress.getLocalHost();
			String hostName = host.getHostName();
			stmt = this.affinityConnection.prepareCall(sp);
			stmt.setString(1, getBusDateFormat(busnDate));
			log.info(getBusDateFormat(busnDate));
			stmt.setString(2, hostName);
			stmt.setString(3, "ENRGY");
			stmt.setLong(4, request.getSeCompanyId());
			stmt.registerOutParameter(5, java.sql.Types.VARCHAR);
			stmt.execute();
			status = stmt.getString(5);
			log.info("Session Return Status = " + status);
		}
		finally {
			
		}
		
	}

	private String getBusDateFormat(java.sql.Date busnDate) {
		SimpleDateFormat sdf  = new SimpleDateFormat("MM/dd/yyyy");
		return sdf.format(busnDate);
	}

	private java.sql.Date getBusinessDate() {
		
		String sql = "select max(busn_dt) from infinity_mgr.close_dt where stat_ind = 'C'";
		Statement stmt = null;
		ResultSet rs = null;
		java.sql.Date busnDate = null;
		
		try {
			stmt = this.affinityConnection.createStatement();
			rs = stmt.executeQuery(sql);
			if (rs.next()) {
				busnDate = rs.getDate(1);
			}
		}
		catch (SQLException e){
			log.error("User(" +  ") getBusinessDate error : " , e );
		}
		finally {
			try {
				if ( rs != null) {
					rs.close();
				}
				if ( stmt != null) {
					stmt.close();
				}
			}
			catch (SQLException e) {
				
			}
		}
		return busnDate;
	}

	private String getFieldValues(ContractRequest request) {
		
		String fieldValues = "";
		
		fieldValues = fieldValues + ((request.getTradeSysCode() == null)? "":request.getTradeSysCode()) + "|";
		fieldValues = fieldValues + ((request.getTradeId() <= 0)? 0:request.getTradeId()) + "|";
		fieldValues = fieldValues + ((request.getTemplateId() <= 0)? 0:request.getTemplateId()) + "|";
		fieldValues = fieldValues + ((request.getSempraCptySn() == null)? "":request.getSempraCptySn()) + "|";
		fieldValues = fieldValues + ((request.getCptySn() == null)? "":request.getCptySn()) + "|";
		fieldValues = fieldValues + ((request.getDateSent() == null)? "":request.getDateSent()) + "|";
		fieldValues = fieldValues + ((request.getSignedFlag() == null)? "":request.getSignedFlag()) + "|";
		fieldValues = fieldValues + ((request.getRqmtId() <= 0)? 0:request.getRqmtId()) + "|";
		fieldValues = fieldValues + ((request.getTradeRqmtConfirmId() <= 0)? 0:request.getTradeRqmtConfirmId()) + "|";
		fieldValues = fieldValues + ((request.getTradeDate() == null)? "":request.getTradeDate()) + "|";
		fieldValues = fieldValues + ((request.getCdtyCode() == null)? "":request.getCdtyCode()) + "|";
		fieldValues = fieldValues + ((request.getCdtyGroupCode() == null)? "":request.getCdtyGroupCode())  + "|";
		fieldValues = fieldValues + ((request.getSettlementType() == null)? "":getSettleType(request.getSettlementType()))  ;
		
		
		return fieldValues;
	}
	
	private String getSettleType(String settlementType) {
		String returnSttl = settlementType;
		if ("FINANCIAL".equalsIgnoreCase(settlementType)) {
			returnSttl = "FNCL";
		}
		else if ("PHYSICAL".equalsIgnoreCase(settlementType) ){
			returnSttl = "PHYS";
		}
		if 	( returnSttl != null && returnSttl.length() > 4) {
			returnSttl = returnSttl.substring(0,4);
		}
		return returnSttl;
	}

	public  ContractResponse getContract(ContractRequest request,
						String vaultWSUrl, 
						String userName, 
						String password,
						String onBehalfUserName,
						String docRepositorySempra,
						String docRepositoryJPM,
						String dslName, 
						boolean fromDb,
						HashMap<String,String> vaultFolders
						)  {
	
		ContractResponse response = new ContractResponse();
		 
		
		if (request == null) {
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		response.setRequest(request);
		try {
			String userCompanyFlag = getCompanyFlag(onBehalfUserName);
			boolean isValid = true;
			// traders will not have the, set them to view the trades.
			if ( userCompanyFlag != null){  
				// check the user has access to get the contract
				isValid= isValidTradeForUser(userCompanyFlag,request.getTradeId(),request.getTradeRqmtConfirmId(),onBehalfUserName);			}
			
			if (isValid) {
				if ( fromDb) {
					response = getContractFromDb(request);
				}
				else {
					if (request.getTradeRqmtConfirmId() > 0) { 
						response = getContractFromValult(request, vaultWSUrl, userName, password, onBehalfUserName, docRepositorySempra,docRepositoryJPM, dslName,vaultFolders);
					}
					if ( !BaseResponse.SUCCESS.equalsIgnoreCase(response.getResponseStatus())) {
						response = getContractFromDb(request);
					}
				}
			}
			else {
				response.setResponseStatus(BaseResponse.ERROR);
				response.setResponseText("Invalid Trade Id...");
			}
		}
		catch (SQLException e) {	
			log.error("User(" + "" + ") getContract error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());	
		}
		
		return response;
	}
	
	private boolean isValidTradeForUser(String userCompanyFlag,long tradeId,long tradeRqmtConfirmId,String userName) throws SQLException{
		boolean validTrade= true;
		//String sql = "Select cm.access_ind from ops_tracking.v_pc_trade_summary pc, ops_tracking.company_migrate cm where pc.migrate_ind = cm.migrate_ind and trade_id = ? " +
		//		"and pc.migrate_ind = cm.migrate_ind and se_cpty_sn in (select company_sn from ops_tracking.user_company where active_flag ='Y' and user_id = '" + userName.toUpperCase() + "')" ;

		//Israel 1/30/15 -- reformatted for readability
		String sql =
				"SELECT cm.access_ind " +
						"  FROM ops_tracking.v_pc_trade_summary pc, ops_tracking.company_migrate cm " +
						" WHERE     pc.migrate_ind = cm.migrate_ind " +
						"       AND trade_id = ? " +
						"       AND pc.migrate_ind = cm.migrate_ind " +
						"       AND se_cpty_sn IN " +
						"              (SELECT company_sn " +
						"                 FROM ops_tracking.user_company " +
						"                WHERE active_flag = 'Y' " +
						"                     AND user_id = '" + userName.toUpperCase() + "')";



//		String tradeRqmtSql = "select cm.access_ind from ops_tracking.v_pc_trade_summary pc,ops_tracking.trade_rqmt_confirm trf,ops_tracking.company_migrate cm " +
//					" where pc.trade_id = trf.trade_id and pc.migrate_ind = cm.migrate_ind and trf.active_flag = 'Y' and trf.id = ? " +
//					"and pc.se_cpty_sn in (select company_sn from ops_tracking.user_company where active_flag ='Y' and user_id = '" + userName.toUpperCase() + "')" ;

		//Israel 1/30/15 -- added trf.active_flag and reformatted for readability
		String tradeRqmtSql =
				"SELECT cm.access_ind " +
						"  FROM ops_tracking.v_pc_trade_summary pc, " +
						"       ops_tracking.trade_rqmt_confirm trf, " +
						"       ops_tracking.company_migrate cm " +
						" WHERE     pc.trade_id = trf.trade_id " +
						"       AND pc.migrate_ind = cm.migrate_ind " +
						"       AND trf.active_flag = 'Y' " +
						"       AND trf.id = ? " +
						"       AND pc.se_cpty_sn IN " +
						"              (SELECT company_sn " +
						"                 FROM ops_tracking.user_company " +
						"                WHERE active_flag = 'Y' " +
						"                      AND user_id = '" + userName.toUpperCase() + "') ";

		PreparedStatement ps = null;
		ResultSet rs = null;
		
		 //if ( _SEMPRA_ACCESS.equalsIgnoreCase(userCompanyFlag) || _JPM_ACCESS.equalsIgnoreCase(userCompanyFlag)){
		if ( !_ALL_ACCESS.equalsIgnoreCase(userCompanyFlag)) {
			try {
				if (tradeId > 0 )
				{
					ps = this.affinityConnection.prepareStatement(sql);
					ps.setLong(1, tradeId);
				}
				else if ( tradeRqmtConfirmId > 0){
					ps = this.affinityConnection.prepareStatement(tradeRqmtSql);
					ps.setLong(1, tradeRqmtConfirmId);
				}
				else {
					return validTrade;
				}
				validTrade = false;
				rs = ps.executeQuery();
				while (rs.next()){
					log.info("Database value =" + rs.getString("access_ind"));
					validTrade = userCompanyFlag.equalsIgnoreCase(rs.getString("access_ind"));
					if (validTrade) {
						break;
					}
				}
				
			}
			finally {
				try {
					if (rs != null){
						rs.close();
					}
					if (ps != null){
						ps.close();
					}
				}
				catch (Exception e){
					
				}
				
			}
		}
		log.info("Return Valid Trade = " + validTrade);
		return validTrade;
	}
	
	private String getCompanyFlag(String userName) throws SQLException{
		String sql = "select access_ind from ops_tracking.user_access where user_id = ? ";
		PreparedStatement ps = null;
		ResultSet rs = null;
		String accessInd = null;
		
		log.info("User(" + userName + ") Checking the Company indictator incoming user = " + userName);
		
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
	public  ContractResponse getContractFromValult(ContractRequest request,
										String vaultWSUrl, 
										String userName, 
										String password,
										String onBehalfUserName,
										String docRepositorySempra,
										String docRepositoryJPM,
										String dslName,
										HashMap<String,String> vaultFolders
										)  {
		
		ContractResponse response = new ContractResponse();
		
		log.info("User(" + userName + ") Getting contract from vault.");
		
		if (request == null) {
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		VaultWrapper vw = null;
		response.setRequest(request);
		try {
			 long tradeId = request.getTradeId();
			 String accessInd = getTradeCompany(request.getTradeId(),request.getTradeRqmtConfirmId());
			 // determine the vault location
			 String docRepository = docRepositorySempra;
				// 2/8/2011, implemented multiple folders. 
				/*
				if (_JPM_ACCESS.equalsIgnoreCase(accessInd)) {
					docRepository = docRepositoryJPM;
				}*/
			 docRepository = vaultFolders.get(accessInd);
			
			 log.info("getContractFromValult(): Vault Folder Name = " + docRepository + " for trade id = " + request.getTradeId() + "; confirm id= " + request.getTradeRqmtConfirmId());
			 
			 vw = new VaultWrapper(vaultWSUrl,userName,password);
			 VaultInfo vi = new VaultInfo();
			 vi.setUserName(onBehalfUserName);
			 vi.setDocRepository(docRepository);
			 vi.setDslName(dslName);
			 String fieldValues = getSearchFieldValues(request);
			 vi.setFieldNames(VaultInfo._SEARCH_CONTRACT_FIELD_NAMES);
			 vi.setFieldValues(fieldValues);
			 VaultStatusInfo vsi =vw.retrieveDocument(vi);
			 if ( vsi.getResultCode() <0 ) {
				 response.setResponseStatus(BaseResponse.ERROR);
				 response.setResponseText(vsi.getResultValue());
			 }
			 else {
				 response.setResponseStatus(BaseResponse.SUCCESS);
				 String s  = new String(vsi.getData(),"utf-8");
				 response.setContract(s);
			 }
			 
		} catch (ServiceException e) {
			log.error("User(" + userName + ") getContract error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		} catch (SAXException e) {
			log.error("User(" + userName + ") getContract error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		} catch (IOException e) {
			log.error("User(" + userName + ") getContract error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		} catch (Exception e) {
			log.error("User(" + userName + ") getContract error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
			response.setResponseStackError(e.getStackTrace().toString());
		}
		
		return response;
		
	}
	public  ContractResponse getContractFromDb(ContractRequest request) {
		
		ContractResponse response = new ContractResponse();
		String prmntIdSql = "select distinct prmnt_id from infinity_mgr.trade_confirm " +
							"where prmnt_trade_id = ? AND " +
							"prmnt_id in (select distinct prmnt_trade_confirm_id from infinity_mgr.trade_confirm_body) ";

		String contractSql = "select segment,body from infinity_mgr.trade_confirm_body where prmnt_trade_confirm_id = ? " +
							" and active_flag = 'Y' and exp_dt = to_date('12/31/2299','mm/dd/yyyy') order by segment";

		log.info("Getting Contract from Database.....");
		PreparedStatement prmntIdStmt = null;
		PreparedStatement contractStmt = null;
		ResultSet rsId = null;
		ResultSet rsContract = null;
		long prmntId = request.getPrmntConfirmId();
		String contractData = "";
		response.setRequest(request);
		try {
			/* if prmnt id is passed, use it otherwise
			 *  get it from the table
			 */
			Logger.getLogger(ContractProcessor.class).info("Incoming Prmnt Id = " + prmntId);
			if ( prmntId <= 0 && request.getTradeId() > 0) {  
				prmntIdStmt = this.affinityConnection.prepareStatement(prmntIdSql);
				prmntIdStmt.setLong(1, request.getTradeId());
				Logger.getLogger(ContractProcessor.class).info("Incoming Trade Id = " + request.getTradeId());
				rsId = prmntIdStmt.executeQuery();
				if (rsId.next()){
					prmntId = rsId.getLong("prmnt_id");
					Logger.getLogger(ContractProcessor.class).info("Getting prmnt id from Db, Prmnt Id = " + prmntId);
				}	
			}
			if (prmntId > 0) {
				contractStmt = this.affinityConnection.prepareStatement(contractSql);
				contractStmt.setLong(1, prmntId);
				rsContract = contractStmt.executeQuery();
				while (rsContract.next()){
					String contractSegmentdata = rsContract.getString("body");
					if (contractSegmentdata != null) {
						contractData += contractSegmentdata;
					}
				}
				contractData = contractData.replace('\u0000','\t');
				response.setContract(contractData);
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
			
		} catch (SQLException e) {
			log.error("User(" + "" + ") getContract error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
		}
		catch (Exception e){
			log.error( "ERROR", e );
		}
		finally {
				try {
					if ( rsId != null) {
						rsId.close();
					}
					if ( rsContract != null){
						rsContract.close();
					}
					if (prmntIdStmt != null){
						prmntIdStmt.close();
					}
					if ( contractStmt != null){
						contractStmt.close();
					}
			}
			catch (SQLException e) {}
		}
		Logger.getLogger(ContractProcessor.class).info("Contract Db Returned...");
		return response;
		
	}
	private String getSearchFieldValues(ContractRequest request) {
		
		String fieldValues = "";
		fieldValues = fieldValues + ((request.getTradeRqmtConfirmId() <= 0)? 0:request.getTradeRqmtConfirmId());
		return fieldValues;
	}
	
	public TradeConfirmStatusResponse updateTradeConfirmStatus(TradeConfirmStatusRequest request){

		
		String prmntIdSql = "select distinct prmnt_id from infinity_mgr.trade_confirm " +
		"where prmnt_trade_id = ? AND " +
		"prmnt_id in (select distinct prmnt_trade_confirm_id from infinity_mgr.trade_confirm_body) ";

		String updateSql = "update infinity_mgr.trade_confirm " +
						 " set confirm_status_ind = ?, " +
						 " prmnt_confirm_template_id = ?, "+
						 " no_confirm_reason = ?, " +
						 " cmt = ?, " +
						 " fax_telex_ind = ?," +
						 " new_template_id = ?, "+ 
						 " fax_telex_number = ?, " +
						 " transmit_timestamp_gmt = sysdate " +
						 " where prmnt_id = ? and " +
						 " exp_dt = to_date('12/31/2299','mm/dd/yyyy')";
		

		TradeConfirmStatusResponse response = new TradeConfirmStatusResponse();
		
		PreparedStatement prmntIdStmt = null;
		PreparedStatement updateStmt = null;
		ResultSet rsId = null;
		ResultSet rsContract = null;
		long prmntId = 0;
		String contractData = "";
		response.setRequest(request);
		try {

			prmntIdStmt = this.affinityConnection.prepareStatement(prmntIdSql);
			prmntIdStmt.setLong(1, request.getTradeId());
			rsId = prmntIdStmt.executeQuery();
			if (rsId.next()){
				prmntId = rsId.getLong("prmnt_id");
				updateStmt = this.affinityConnection.prepareStatement(updateSql);
				updateStmt.setString(1, request.getStatusInd());
				updateStmt.setLong(2, request.getTemplateId());
				updateStmt.setString(3, request.getNoConfirmReason());
				updateStmt.setString(4, request.getComment());
				updateStmt.setString(5, request.getFaxTelexInd());
				if (request.getNewTemplateId() > 0) {
					updateStmt.setLong(6, request.getNewTemplateId());
				}
				else {
					updateStmt.setNull(6, java.sql.Types.NUMERIC);
				}
				updateStmt.setString(7, request.getFaxTelexNumber());
				updateStmt.setLong(8, prmntId);
				updateStmt.executeUpdate();
			}
			response.setResponseStatus(BaseResponse.SUCCESS);

		} catch (SQLException e) {
			log.error("User(" + "" + ") updateTradeConfirmStatus error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
		}
	
		finally {
			try {
				if ( rsId != null) {
					rsId.close();
				}
				if ( rsContract != null){
					rsContract.close();
				}
				if (prmntIdStmt != null){
					prmntIdStmt.close();
				}
				if ( updateStmt != null){
					updateStmt.close();
				}
			}
			catch (SQLException e) {}
		}
		return response;
		
	}

	public SearchContractResponse getContractList(SearchContractRequest request,String userName) {

		
		String affSql =  "select tc.trade_dt,tc.prmnt_id, t.name " +
						 "from confirm.template t,infinity_mgr.trade_confirm tc " +
						 "where tc.new_template_id = t.id (+) " +
						 "and tc.prmnt_trade_id = ? " +
						 " and tc.exp_dt = '31-DEC-2299' " +
						 " and tc.confirm_status_ind = 'S' ";
		String jmsSql = "select tc.trade_dt,tc.prmnt_id, t.name  " + 
						"from infinity_mgr.trade_confirm tc , confirm.template t " +
						"where tc.BLOTTER_NO = ? " +		 
						"and tc.new_template_id = t.id (+)  " +
						" and tc.exp_dt = '31-DEC-2299' " +
						" and tc.confirm_status_ind = 'S' ";
		String rqmtSql = "select td.trade_dt,trc.id,t.name " +
						 "from ops_tracking.trade_data td, ops_tracking.trade_rqmt tr, ops_tracking.trade_rqmt_confirm trc,confirm.template t " +
						 "where td.trade_id = ? " + 
						 "and tr.trade_id = td.trade_id " +  
						 "and   tr.rqmt = 'XQCSP' " + 
						 "and   tr.status IN ('SENT')" +
						 "and   tr.id = trc.rqmt_id " + 
						 "and  trc.template_id = t.id (+) " +
						 "and trc.active_flag = 'Y' ";
		
		SearchContractResponse response = new SearchContractResponse();
		if (request == null) {
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText("Parameter is null");
			return response;
		}
		response.setRequest(request);
		ArrayList<SearchContractData> data = new ArrayList<SearchContractData>();
		response.setData(data);
		PreparedStatement stmt = null;
		PreparedStatement stmt2 = null;
		ResultSet rsOldContract  = null;
		ResultSet rsNewContract = null;
		
		try {
			String userCompanyFlag = getCompanyFlag(userName);
			if ( userCompanyFlag == null){
				response.setResponseStatus(BaseResponse.ERROR);
				response.setResponseText("Company access setup is not done.");
				return response;
			}
			// check the user has access to get the contract
			boolean isValid = isValidTradeForUser(userCompanyFlag,request.getTradeId(),0,userName);
			if (isValid) { 
				if ("JMS".equalsIgnoreCase(request.getTradeSystemCode())) {
					stmt = this.affinityConnection.prepareStatement(jmsSql);
					stmt.setString(1,request.getTradeSystemCode() + request.getTradeId());
				}
				else {
					stmt = this.affinityConnection.prepareStatement(affSql);
					stmt.setLong(1, request.getTradeId());
				}
				rsOldContract = stmt.executeQuery();
				while ( rsOldContract.next()){
					SearchContractData scd = new SearchContractData();
					scd.setTradeConfirmId(rsOldContract.getLong("prmnt_id"));
					scd.setTemplateName(rsOldContract.getString("name"));
					scd.setTradeDate(getDateString(rsOldContract.getDate("trade_dt")));
					data.add(scd);
				}
				stmt2  = this.affinityConnection.prepareStatement(rqmtSql);
				stmt2.setLong(1, request.getTradeId());
				rsNewContract = stmt2.executeQuery();
				while ( rsNewContract.next()){
					SearchContractData scd = new SearchContractData();
					scd.setTradeRqmtConfirmId(rsNewContract.getLong("id"));
					scd.setTemplateName(rsNewContract.getString("name"));
					scd.setTradeDate(getDateString(rsNewContract.getDate("trade_dt")));
					data.add(scd);
				}
			}
			response.setResponseStatus(BaseResponse.SUCCESS);
		}
		catch (SQLException e) {
			log.error("User(" + "" + ") getContractList error : " , e );
			response.setResponseStatus(BaseResponse.ERROR);
			response.setResponseText(e.getMessage());
		}
		finally {
			try {
				if (rsOldContract != null){
					rsOldContract.close();
				}
				if (stmt != null){
					stmt.close();
				}
				if (rsNewContract != null){
					rsNewContract.close();
				}
				if (stmt2 != null){
					stmt2.close();
				}
			}
			catch (Exception e){
				
			}
		}
		return response;
	}
	
	private String getDateString(Date dt){
		SimpleDateFormat sdf = new SimpleDateFormat("dd-MMM-yyyy");
		return sdf.format(dt);
	}
	private String getTradeCompany(long tradeId,long tradeRqmtConfirmId) throws SQLException{
		
		String returnInd = _SEMPRA_ACCESS;
		//String sql = "select cm.access_ind from ops_tracking.v_pc_trade_summary pc, ops_tracking.company_migrate cm " +
		//" where pc.trade_id = ? and pc.migrate_ind = cm.migrate_ind";

		//Israel 1/30/15 -- Reformatted
		String sql =
				"SELECT cm.access_ind " +
						"  FROM ops_tracking.v_pc_trade_summary pc, ops_tracking.company_migrate cm " +
						" WHERE pc.trade_id = ? AND pc.migrate_ind = cm.migrate_ind ";

		//String tradeRqmtConfirmSql = "select cm.access_ind from ops_tracking.v_pc_trade_summary pc, ops_tracking.company_migrate cm , ops_tracking.trade_rqmt_confirm trc " +
		//" where trc.id = ? and pc.trade_id = trc.trade_id and pc.migrate_ind = cm.migrate_ind and trc.active_flag = 'Y' ";

		//Israel 1/30/15 -- Added trc.active_flag and reformatted
		String tradeRqmtConfirmSql =
				"SELECT cm.access_ind " +
						"  FROM ops_tracking.v_pc_trade_summary pc, " +
						"       ops_tracking.company_migrate cm, " +
						"       ops_tracking.trade_rqmt_confirm trc " +
						" WHERE     trc.id = ? " +
						"       AND pc.trade_id = trc.trade_id " +
						"       AND pc.migrate_ind = cm.migrate_ind " +
						"       AND trc.active_flag = 'Y' ";

		PreparedStatement stmt = null;
		try {
			if (tradeId > 0) {
				stmt = this.affinityConnection.prepareStatement(sql);
				stmt.setLong(1, tradeId);
			}
			else {
				stmt = this.affinityConnection.prepareStatement(tradeRqmtConfirmSql);
				stmt.setLong(1, tradeRqmtConfirmId);
			}
			ResultSet rs = stmt.executeQuery();
			if (rs.next()){
				returnInd = rs.getString("access_ind");
			}
		}
		catch (SQLException e) {
			log.error(" getTradeCompany error : " , e );
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
		return returnInd;

	}
}
