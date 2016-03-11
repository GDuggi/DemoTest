package aff.confirm.opsmanager.confirm;

import aff.confirm.opsmanager.confirm.data.*;
import org.jboss.ws.api.annotation.WebContext;

import javax.ejb.EJB;
import javax.ejb.Stateless;
import javax.jws.WebMethod;
import javax.jws.WebParam;
import javax.jws.WebService;

@Stateless(name="ConfirmationWrapperBean",mappedName="ConfirmationWrapperBean")
@WebService(serviceName="ConfirmationService",name="ConfirmationService",targetNamespace="http://rbssempra.com/confirm")
@WebContext(contextRoot="Confirm")
public class ConfirmationWrapperBean implements ConfirmationWrapper{

	@EJB private Confirmation confirm;
	
	@WebMethod(operationName="updateTradeConfirm")
	public RqmtConfirmUpdateResponse updateTradeConfirm(
			@WebParam(name="rqmtConfirmUpdateRequest") RqmtConfirmUpdateRequest request, 
			@WebParam(name="userName", header=true)String userName) {
		
		return confirm.updateTradeConfirm(request, userName);
	}


	@WebMethod(operationName="getConfirmClauseBody")
	public ClauseBodyResponse getConfirmClauseBody(
			@WebParam(name="userName", header=true) String userName) {
		return confirm.getConfirmClauseBody(userName);
	}

	@WebMethod(operationName="getConfirmClauseHeader")
	public ClauseHeaderResponse getConfirmClauseHeader(
			@WebParam(name="userName", header=true) String userName) {
		return confirm.getConfirmClauseHeader(userName);
	}

	@WebMethod(operationName="updateClause")
	public InsertClauseResponse[] updateClause(
			@WebParam(name="insertClauseRequest") InsertClauseRequest[] request,
			@WebParam(name="userName", header=true) String userName) {
		return confirm.updateClause(request, userName);
	}

	
	@WebMethod(operationName="getCptyAgreements")
	public CptyAgreementResponse getCptyAgreements(
			@WebParam(name="cptyAgreementRequest")  CptyAgreementRequest request, 
			@WebParam(name="userName", header=true)  String userName) {
		return confirm.getCptyAgreements(request, userName);
	}


	@WebMethod(operationName="getCptyFaxNumber")
	public CptyFaxNumberResponse getCptyFaxNumber(
			@WebParam(name="cptyFaxNumberRequest") CptyFaxNumberRequest request,
			@WebParam(name="userName", header=true) String userName) {
		return confirm.getCptyFaxNumber(request,userName);
	}


	@WebMethod(operationName="getDesignationList")
	public DesignationResponse getDesignationList(
			@WebParam(name="userName", header=true)String userName) {
		return confirm.getDesignationList(userName);
	}


	@WebMethod(operationName="getInfMgrFaxNumber")
	public InfMgrFaxResponse getInfMgrFaxNumber(
			@WebParam(name="infMgrFaxRequest") InfMgrFaxrequest request,
			@WebParam(name="userName", header=true) String userName) {
		return confirm.getInfMgrFaxNumber(request, userName);
	}


	@WebMethod(operationName="getTraderEmailAddress")
	public TraderEmailAddressResponse getTraderEmailAddress(
			@WebParam(name="traderEmailAddressRequest") TraderEmailAddressRequest request,
			@WebParam(name="userName", header=true) String userName) {
		return confirm.getTraderEmailAddress(request, userName);
	}


	@WebMethod(operationName="storeContract")
	public ContractResponse storeContract(
			@WebParam(name="vaultRequest") ContractRequest request,
			@WebParam(name="userName", header=true) String userName) {
		return confirm.storeContract(request, userName);
	}

	@WebMethod(operationName="getContract")
	public ContractResponse getContract(
			@WebParam(name="vaultRequest") ContractRequest request,
			@WebParam(name="userName", header=true) String userName) {
		return confirm.getContract(request, userName);
	}


	@WebMethod(operationName="updateTradeConfirmStatus")
	public TradeConfirmStatusResponse updateTradeConfirmStatus(
			@WebParam(name="statusRequest") TradeConfirmStatusRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		return confirm.updateTradeConfirmStatus(request, userName);
	}


	@WebMethod(operationName="deleteTradeRqmtConfirm")
	public TradeRqmtConfirmDeleteResponse deleteTradeRqmtConfirm(
			@WebParam(name="deleteRequest") TradeRqmtConfirmDeleteRequest request, 
			@WebParam(name="userName", header=true)String userName) {
		return confirm.deleteTradeRqmtConfirm(request, userName);
	}

	@WebMethod(operationName="getContractList")
	public SearchContractResponse getContractList(
			@WebParam(name="searchRequest") SearchContractRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		return confirm.getContractList(request, userName);
	}


	@WebMethod(operationName="getCptyInfo")	
	public CptyInfoResponse getCptyInfo(
			@WebParam(name="infoRequest")  CptyInfoRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		return confirm.getCptyInfo(request, userName);
	}

	@WebMethod(operationName="getCptyAgreementList")
	public AgreementInfoResponse getCptyAgreementList(
			@WebParam(name="agreementRequest")   AgreementInfoRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		return confirm.getCptyAgreementList(request, userName);
	}


	@WebMethod(operationName="getContractFaxList")
	public ContractFaxResponse getContractFaxList(
			@WebParam(name="contractFaxRequest")   ContractFaxRequest request,
			@WebParam(name="userName", header=true)  String userName) {
		return confirm.getContractFaxList(request, userName);
	}


	@WebMethod(operationName="insertFaxLogSent")
	public FaxLogSentResponse[] insertFaxLogSent(
			@WebParam(name="faxLogSentRequest")   FaxLogSentRequest[] request,
			@WebParam(name="userName", header=true) String userName) {
		return confirm.insertFaxLogSent(request, userName);
	}


	@WebMethod(operationName="getFaxGatewayStatusHistory")
	public FaxStatusLogResponse getFaxGatewayStatusHistory(
			@WebParam(name="faxLogStatusRequest") FaxStatusLogRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		return confirm.getFaxGatewayStatusHistory(request, userName);
	}

}
