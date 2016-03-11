package aff.confirm.opsmanager.confirm;

import aff.confirm.opsmanager.common.BaseOpsBean;
import aff.confirm.opsmanager.common.util.NotifyUtil;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.confirm.common.ConfirmProcessor;
import aff.confirm.opsmanager.confirm.common.ContractProcessor;
import aff.confirm.opsmanager.confirm.data.*;
import org.jboss.logging.Logger;

import javax.annotation.PostConstruct;
import javax.annotation.Resource;
import javax.ejb.EJBException;
import javax.ejb.Stateless;
import javax.naming.NamingException;
import java.sql.SQLException;
import java.util.HashMap;

@Stateless(name="Confirmation",mappedName="Confirmation")
public class ConfirmationBean extends BaseOpsBean implements Confirmation {
	Logger log = Logger.getLogger( ConfirmationBean.class  );
	
	private ConfirmProcessor processor;
	private ContractProcessor contractProcessor;
	
	@Resource(name="vaultWebServerUrl")
	private String vaultUrl;
	
	@Resource(name="loginUser")
	private String vaultUser;
	
	@Resource(name="loginPassword")
	private String vaultPassword;
	
	@Resource(name="dslName")
	private String vaultDslName;
	
	@Resource(name="repositoryName")
	private String vaultRepository;
	
	@Resource(name="repositoryNameJPM")
	private String vaultRepositoryJPM;
	
	@Resource(name="updateContractDb")
	private boolean updateContractDb;
	
	@Resource(name="getContractDb")
	private boolean getContractDb;
	
	@Resource(name="sempraCompanyId")
	private long sempraCompanyId;
	
	@Resource(name="creditStatusUrl")
	private String creditStatusUrl;
	
	@Resource(name="creditMarginUrl")
	private String creditMarginUrl;
	
	@Resource(name="creditMarginToken")
	private String creditMarginToken;
	
	@Resource(name="creditServiceUser")
	private String creditServiceUser;
	
	@Resource(name="traderWebUrl")
	private String traderWebUrl;
	
	@Resource(name="smtpHost")
	private String smtpHost;
	
	@Resource(name="smtpPort")
	private String smtpPort;
	
	@Resource(name="vaultFolders")
	private String contractVaults;
	
	private HashMap<String,String> vaultFolders = new HashMap<String,String>(); 
	
	public ConfirmationBean() throws NamingException{
		super();
        //Israel 6/11/14 -- Add support for EAP 6
		processor = new ConfirmProcessor(this.affinityConnection);
		contractProcessor = new ContractProcessor(this.affinityConnection);
	}
	
	@PostConstruct
	public void parseVaultInfo(){
		
		log.info(contractVaults);
		fillVaultFolders();
		
	}
	private void fillVaultFolders(){
		
		String[] vaultData = contractVaults.split(",");
		if (vaultData != null){
			for (int i =0;i<vaultData.length;++i){
				String oneFolder = vaultData[i];
				log.info("Vault Info= " + oneFolder);
				String[] folderInfo = oneFolder.split(":");
				vaultFolders.put(folderInfo[0].trim(),folderInfo[1].trim());
                log.info("Stored = " + folderInfo[0].trim() + ";" + folderInfo[1].trim());
			}
		}
	}
	public RqmtConfirmUpdateResponse updateTradeConfirm(
			RqmtConfirmUpdateRequest request, String userName) {
		
		log.info("User(" + userName + ") updateTradeConfirm called");
		
		RqmtConfirmUpdateResponse resp = new RqmtConfirmUpdateResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.updateTradeConfirm(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") updateTradeConfirm error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") updateTradeConfirm returned");
		return resp;
	}

	@Override
	public void prepareForMethodCall() {
		processor.setConnection(this.affinityConnection);
		contractProcessor.setConnection(this.affinityConnection);
	}


	public ClauseBodyResponse getConfirmClauseBody(String userName) {
		
		log.info("User(" + userName + ") getConfirmClauseBody called");
		
		ClauseBodyResponse resp = new ClauseBodyResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.getConfirmClauseBody();
		} catch (SQLException e) {
			log.error("User(" + userName + ") getConfirmClauseBody error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getConfirmClauseBody returned");
		return resp;
	}

	public ClauseHeaderResponse getConfirmClauseHeader(String userName) {
		
		log.info("User(" + userName + ") getConfirmClauseHeader called");
		
		ClauseHeaderResponse resp = new ClauseHeaderResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.getConfirmClauseHeader();
		} catch (SQLException e) {
			log.error("User(" + userName + ") getConfirmClauseHeader error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getConfirmClauseHeader returned");
		return resp;
	}

	public InsertClauseResponse[] updateClause(InsertClauseRequest[] request,
			String userName) {
		
		log.info("User(" + userName + ") updateClause called");
		
		InsertClauseResponse resp[] = new InsertClauseResponse[] {new InsertClauseResponse()};
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.updateClause(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") updateClause error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0], e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") updateClause returned");
		return resp;
	}

	public DocVaultResponse storeContract(DocVaultRequest request,String userName) {
		
		log.info("User(" + userName + ") storeContract called");
		
		DocVaultResponse resp= new DocVaultResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.storeContract(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") storeContract error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") storeContract returned");
		return resp;
	}

	public CptyAgreementResponse getCptyAgreements(
			CptyAgreementRequest request, String userName) {
		
		log.info("User(" + userName + ") getCptyAgreements called");
		
		CptyAgreementResponse resp= new CptyAgreementResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.getCptyAgreements(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getCptyAgreements error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getCptyAgreements returned");
		return resp;
	}

	public CptyFaxNumberResponse getCptyFaxNumber(CptyFaxNumberRequest request,
			String userName) {
		
		log.info("User(" + userName + ") getCptyFaxNumber called");
		
		CptyFaxNumberResponse resp= new CptyFaxNumberResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.getCptyFax(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getCptyFaxNumber error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getCptyFaxNumber returned");
		return resp;
	}

	public DesignationResponse getDesignationList(String userName) {
		
		log.info("User(" + userName + ") getDesignationList called");
		
		DesignationResponse resp= new DesignationResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.getDesignationList();
		} catch (SQLException e) {
			log.error("User(" + userName + ") getDesignationList error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getDesignationList returned");
		return resp;
	}

	public InfMgrFaxResponse getInfMgrFaxNumber(InfMgrFaxrequest request,
			String userName) {
		
		log.info("User(" + userName + ") getInfMgrFaxNumber called");
		
		InfMgrFaxResponse resp= new InfMgrFaxResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.getInfMgrFax(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getInfMgrFaxNumber error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getInfMgrFaxNumber returned");
		return resp;
	}

	public TraderEmailAddressResponse getTraderEmailAddress(
			TraderEmailAddressRequest request, String userName) {
		
		log.info("User(" + userName + ") getTraderEmailAddress called");
		
		TraderEmailAddressResponse resp= new TraderEmailAddressResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.getTraderEmailAddress(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getTraderEmailAddress error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getTraderEmailAddress returned");
		return resp;
	}

	public ContractResponse storeContract(ContractRequest request,
			String userName) {

		log.info("User(" + userName + ") storeContract called");
		
		ContractResponse resp= new ContractResponse();
		NotifyUtil.smtpHost = this.smtpHost;
		NotifyUtil.smtpPort = this.smtpPort;
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			request.setSeCompanyId(sempraCompanyId);
			boolean isMarginCheck = !creditServiceUser.equalsIgnoreCase(userName);
			resp =contractProcessor.storeContract(request,this.vaultUrl, this.vaultUser, this.vaultPassword, 
									userName, this.vaultRepository,this.vaultRepositoryJPM ,this.vaultDslName,
									this.creditMarginToken,this.creditStatusUrl,this.creditMarginUrl,this.traderWebUrl,this.affinityDatabase,isMarginCheck,this.vaultFolders);
		} catch (SQLException e) {
			log.error("User(" + userName + ") storeContract error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") storeContract returned");
		return resp;
	}

	public ContractResponse getContract(ContractRequest request,
			String userName) {
		
		log.info("User(" + userName + ") getContract called");
		
		ContractResponse resp= new ContractResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =contractProcessor.getContract(request, this.vaultUrl, this.vaultUser, this.vaultPassword, userName, this.vaultRepository, this.vaultRepositoryJPM ,this.vaultDslName,getContractDb,this.vaultFolders);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getContract error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getContract returned");
		return resp;
	}

	public TradeConfirmStatusResponse updateTradeConfirmStatus(
			TradeConfirmStatusRequest request, String userName) {
		
		log.info("User(" + userName + ") updateTradeConfirmStatus called");
		
		TradeConfirmStatusResponse resp= new TradeConfirmStatusResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =contractProcessor.updateTradeConfirmStatus(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") updateTradeConfirmStatus error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") updateTradeConfirmStatus returned");
		return resp;	
	}

	public ContractResponse getContractFromVault(ContractRequest request,
			String userName) {
		
		log.info("User(" + userName + ") getContractFromVault called");
		
		ContractResponse resp= new ContractResponse();
	
		try {
			resp =contractProcessor.getContractFromValult(request, this.vaultUrl, this.vaultUser, this.vaultPassword, userName, this.vaultRepository,this.vaultRepositoryJPM, this.vaultDslName,this.vaultFolders);
		} catch (Exception e) {
			log.error("User(" + userName + ") getContractFromVault error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getContractFromVault returned");
		return resp;
	}

	public ContractResponse storeContractInVault(ContractRequest request,
			String userName) {
		
		log.info("User(" + userName + ") storeContractInVault called");
		
		NotifyUtil.smtpHost = this.smtpHost;
		NotifyUtil.smtpPort = this.smtpPort;
		ContractResponse resp= new ContractResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			request.setSeCompanyId(sempraCompanyId);
			boolean isMarginCheck = !creditServiceUser.equalsIgnoreCase(userName);
			resp =contractProcessor.storeContract(request, this.vaultUrl, this.vaultUser, this.vaultPassword, 
								userName, this.vaultRepository,this.vaultRepositoryJPM ,this.vaultDslName,
								this.creditMarginToken,this.creditStatusUrl,this.creditStatusUrl,this.traderWebUrl,this.affinityDatabase,isMarginCheck,this.vaultFolders);
		} catch (SQLException e) {
			log.error("User(" + userName + ") storeContractInVault error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") storeContractInVault returned");
		return resp;
		
	}

	public TradeRqmtConfirmDeleteResponse deleteTradeRqmtConfirm(
			TradeRqmtConfirmDeleteRequest request, String userName) {
		
		log.info("User(" + userName + ") deleteTradeRqmtConfirm called");
		
		TradeRqmtConfirmDeleteResponse resp= new TradeRqmtConfirmDeleteResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp = processor.deleteTradeRqmtConfirm(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") deleteTradeRqmtConfirm error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") deleteTradeRqmtConfirm returned");
		return resp;
	}

	public SearchContractResponse getContractList(
			SearchContractRequest request, String userName) {
		
		log.info("User(" + userName + ") getContractList called");
		
		SearchContractResponse resp= new SearchContractResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =contractProcessor.getContractList(request,userName);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getContractList error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getContractList returned");
		return resp;
	}

	public CptyInfoResponse getCptyInfo(CptyInfoRequest request, String userName) {
		
		log.info("User(" + userName + ") getCptyInfo called");
		
		CptyInfoResponse resp= new CptyInfoResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp = processor.getCptyInfo(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getCptyInfo error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getCptyInfo returned");
		return resp;
	}

	public AgreementInfoResponse getCptyAgreementList(
			AgreementInfoRequest request, String userName) {

		log.info("User(" + userName + ") getCptyAgreementList called");
		
		AgreementInfoResponse resp= new AgreementInfoResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp = processor.getCptyAgreementList(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getCptyAgreementList error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getCptyAgreementList returned");
		return resp;
	}

	public ContractFaxResponse getContractFaxList(ContractFaxRequest request,
			String userName) {

		log.info("User(" + userName + ") getContractFaxList called");
		
		ContractFaxResponse resp= new ContractFaxResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp = processor.getContractFaxList(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getContractFaxList error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getContractFaxList returned");
		return resp;
	}

	public FaxLogSentResponse[] insertFaxLogSent(FaxLogSentRequest[] request,
			String userName) {
		
		log.info("User(" + userName + ") insertFaxLogSent called");
		
		FaxLogSentResponse resp[] = new FaxLogSentResponse[] {new FaxLogSentResponse()};
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.insertFaxLogSent(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") insertFaxLogSent error : " , e );
			OpsManagerUtil.populateErrorMessage(resp[0], e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") insertFaxLogSent returned");
		return resp;
	}

	public FaxStatusLogResponse getFaxGatewayStatusHistory(
			FaxStatusLogRequest request, String userName) {
		
		log.info("User(" + userName + ") getFaxGatewayStatusHistory called");
		
		FaxStatusLogResponse resp= new FaxStatusLogResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp = processor.getFaxGatewayStatusHistory(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getFaxGatewayStatusHistory error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getFaxGatewayStatusHistory returned");
		return resp;
	}

}
