package aff.confirm.opsmanager.confirm;

import aff.confirm.opsmanager.confirm.data.*;

import javax.ejb.Remote;

@Remote
public interface ConfirmationWrapper {

	RqmtConfirmUpdateResponse updateTradeConfirm(RqmtConfirmUpdateRequest request,String userName);
	ClauseHeaderResponse getConfirmClauseHeader(String userName);
	ClauseBodyResponse getConfirmClauseBody(String userName);
	InsertClauseResponse[] updateClause(InsertClauseRequest[] request, String userName);
	CptyAgreementResponse getCptyAgreements(CptyAgreementRequest request,String userName);
	CptyFaxNumberResponse getCptyFaxNumber(CptyFaxNumberRequest request,String userName);
	
	DesignationResponse getDesignationList(String userName);
	InfMgrFaxResponse getInfMgrFaxNumber(InfMgrFaxrequest request,String userName);
	TraderEmailAddressResponse getTraderEmailAddress(TraderEmailAddressRequest request,String userName);
	ContractResponse storeContract(ContractRequest request,String userName);
	
	ContractResponse getContract(ContractRequest request,String userName);
	TradeConfirmStatusResponse updateTradeConfirmStatus(TradeConfirmStatusRequest request,String userName);
	TradeRqmtConfirmDeleteResponse deleteTradeRqmtConfirm(TradeRqmtConfirmDeleteRequest request,String userName);
	SearchContractResponse getContractList(SearchContractRequest request, String userName);
	CptyInfoResponse getCptyInfo(CptyInfoRequest request,String userName);
	
	AgreementInfoResponse getCptyAgreementList(AgreementInfoRequest request,String userName);
	ContractFaxResponse getContractFaxList(ContractFaxRequest request,String userName);
	FaxLogSentResponse[] insertFaxLogSent(FaxLogSentRequest[] request,String userName);
	FaxStatusLogResponse getFaxGatewayStatusHistory(FaxStatusLogRequest request,String userName);
	
}
