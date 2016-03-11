package aff.confirm.opsmanager.tc.trade;

import javax.ejb.EJB;
import javax.ejb.Stateless;
import javax.jws.WebMethod;
import javax.jws.WebParam;
import javax.jws.WebService;

import org.jboss.ejb3.annotation.TransactionTimeout;
import org.jboss.ws.api.annotation.WebContext;





import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.tc.data.DetermineActionRequest;
import aff.confirm.opsmanager.tc.data.DetermineActionResponse;
import aff.confirm.opsmanager.tc.data.FinalApproveRequest;
import aff.confirm.opsmanager.tc.data.FinalApproveResponse;
import aff.confirm.opsmanager.tc.data.DealSheetRequest;
import aff.confirm.opsmanager.tc.data.DealSheetResponse;
import aff.confirm.opsmanager.tc.data.GetTradeRequest;
import aff.confirm.opsmanager.tc.data.GetTradeResponse;
import aff.confirm.opsmanager.tc.data.InboundAttribMapRequest;
import aff.confirm.opsmanager.tc.data.InboundAttribMapResponse;
import aff.confirm.opsmanager.tc.data.InboundAttribResponse;
import aff.confirm.opsmanager.tc.data.JMSTradeProcessRequest;
import aff.confirm.opsmanager.tc.data.JMSTradeProcessResponse;
import aff.confirm.opsmanager.tc.data.ReopenRequest;
import aff.confirm.opsmanager.tc.data.ReopenResponse;
import aff.confirm.opsmanager.tc.data.TradeCompanyRequest;
import aff.confirm.opsmanager.tc.data.TradeCompanyResponse;
import aff.confirm.opsmanager.tc.data.TradeConfirmCreatorRequest;
import aff.confirm.opsmanager.tc.data.TradeConfirmCreatorResponse;
import aff.confirm.opsmanager.tc.data.TradeAuditRequest;
import aff.confirm.opsmanager.tc.data.TradeAuditResponse;
import aff.confirm.opsmanager.tc.data.TradeCommentRequest;
import aff.confirm.opsmanager.tc.data.TradeCommentResponse;
import aff.confirm.opsmanager.tc.data.TradeDataChangeRequest;
import aff.confirm.opsmanager.tc.data.TradeDataChangeResponse;
import aff.confirm.opsmanager.tc.data.TradeGroupRequest;
import aff.confirm.opsmanager.tc.data.TradeGroupResponse;
import aff.confirm.opsmanager.tc.data.TradeRqmtRequest;
import aff.confirm.opsmanager.tc.data.TradeRqmtResponse;
import aff.confirm.opsmanager.tc.data.TradeUnGroupRequest;
import aff.confirm.opsmanager.tc.data.TradeUnGroupResponse;


@Stateless(name="TradeConfirmWrapperBean",mappedName="TradeConfirmWrapperBean")
@WebService(serviceName="TradeConfirmService",name="TradeConfirmWrapperBean",targetNamespace="http://rbssempra.com/confirm")
@WebContext(contextRoot="OpsManager")
public class TradeConfirmWrapperBean implements TradeConfirmWrapper {

	@EJB private OpsTrade opsTrade;
	@EJB private TradeUpdater tradeUpdater;
	
	
	@WebMethod(operationName="batchUpdateDetermineActions")
	public DetermineActionResponse[] batchUpdateDetermineActions(
			@WebParam(name="tradeActionList") DetermineActionRequest[] tradeActionList,
			@WebParam(name="userName",header=true) String userName) {
		
		return opsTrade.batchUpdateDetermineActions(tradeActionList, userName);
		
	}
	
	//@WebMethod(operationName="updateFinalApproval")
	public BaseResponse updateFinalApproval(
			@WebParam(name="tradeId") int tradeId, 
			@WebParam(name="approvalStatus") String approvalStatus,
			@WebParam(name="userName",header=true) String userName) {
		
		return opsTrade.updateFinalApproval(tradeId, approvalStatus, userName);
	}

	//@WebMethod(operationName="updateTradeSummary")
	public BaseResponse updateTradeSummary(
			@WebParam(name="tradeId") int tradeId,
			@WebParam(name="finalApprovedFlag") String finalApprovedFlag, 
			@WebParam(name="opsDetactFlag") String opsDetactFlag,
			@WebParam(name="openRqmtFlat") String openRqmtFlat, 
			@WebParam(name="comment") String comment, 
			@WebParam(name="forceCmtNullFlag")String forceCmtNullFlag,
			@WebParam(name="userName",header=true) String userName) {
		
		return opsTrade.updateTradeSummary(tradeId, finalApprovedFlag, opsDetactFlag, openRqmtFlat, comment, forceCmtNullFlag, userName);
		
	}

	//@WebMethod(operationName="updateTradeSummaryComment")
	public BaseResponse updateTradeSummaryComment(
			@WebParam(name="tradeId") int tradeId, 
			@WebParam(name="comment") String comment,
			@WebParam(name="userName",header=true)  String userName) {

		return opsTrade.updateTradeSummaryComment(tradeId, comment, userName);
		
	}

	// 45 min time out 
	@TransactionTimeout(2700) 
	@WebMethod(operationName="finalApproveTrades")
	public FinalApproveResponse[] finalApprove(
			@WebParam(name="tradeList") FinalApproveRequest[] tradeList,
			@WebParam(name="userName", header=true) String userName){
		
		return tradeUpdater.finalApprove(tradeList,userName);
		
	}


	@WebMethod(operationName="groupTrades")
	public TradeGroupResponse[] groupTrades(
			@WebParam(name="tradeList") TradeGroupRequest[] request,
			@WebParam(name="userName", header=true) String userName) {
		
		return tradeUpdater.groupTrades(request, userName);
		
	}

	@WebMethod(operationName="reopenTrades")
	public ReopenResponse[] reopen(
			@WebParam(name="tradeList")  ReopenRequest[] request, 
			@WebParam(name="userName", header=true) String userName) {
		
		return tradeUpdater.reopen(request, userName);
	}

	
	@WebMethod(operationName="ungroupTrades")
	public TradeUnGroupResponse ungroupTrades(
			@WebParam(name="tradeList")  TradeUnGroupRequest request,
			@WebParam(name="userName", header=true) String userName) {
		
		return tradeUpdater.ungroupTrades(request, userName);
		
	}

	@WebMethod(operationName="getTradeAudit")
	public TradeAuditResponse getTradeAudit(
			@WebParam(name="tradeAuditRequest") TradeAuditRequest request,
			@WebParam(name="userName", header=true) String userName) {

		return opsTrade.getTradeAudit(request, userName);
	}

	@WebMethod(operationName="getTradeDataChange")
	public TradeDataChangeResponse getTradeDataChange(
			@WebParam(name="tradeDataChangeRequest") TradeDataChangeRequest request, 
			@WebParam(name="userName", header=true) String userName) {

		return opsTrade.getTradeDataChange(request, userName);
	}

	@WebMethod(operationName="addRqmts")
	public TradeRqmtResponse[] addRqmts(
			@WebParam(name="tradeRqmtRequest") TradeRqmtRequest[] request,
			@WebParam(name="userName", header=true) String userName) {
		
		return opsTrade.addRqmts(request, userName);
		
	}

	@WebMethod(operationName="updateTradeRqmts")
	public TradeRqmtResponse[] updateTradeRqmts(
			@WebParam(name="tradeRqmtRequest")  TradeRqmtRequest[] tradeRqmtRequest, 
			@WebParam(name="userName", header=true) String userName) {
		
		return opsTrade.updateTradeRqmts( tradeRqmtRequest, userName);
		
	}
	
	@WebMethod(operationName="updateTradeComments")
	public TradeCommentResponse[] updateTradeComments(
			@WebParam(name="tradeCommentRequest")  TradeCommentRequest[] tradeCommentRequest,
			@WebParam(name="userName", header=true) String userName) {
		
		return opsTrade.updateTradeComments(tradeCommentRequest,  userName);
		
	}

	@WebMethod(operationName="getTrades")
	public GetTradeResponse getTrades(
			@WebParam(name="tradeRequest") GetTradeRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		return opsTrade.getTrades(request, userName);
	}

	
	@WebMethod(operationName="updateTradeRqmtStatus")
	public TradeRqmtResponse updateTradeRqmtStatus(
			@WebParam(name="tradeRqmtRequest") TradeRqmtRequest tradeRqmtRequest, 
			@WebParam(name="userName", header=true) String userName) {
		return opsTrade.updateTradeRqmtStatus(tradeRqmtRequest, userName);
	}

	@WebMethod(operationName="getDealSheet")
	public DealSheetResponse getDealSheetData(
			@WebParam(name="dealSheetRequest") DealSheetRequest request,
			@WebParam(name="userName", header=true) String userName) {
		
		return opsTrade.getDealSheet(request, userName);
	}

	@WebMethod(operationName="replayTrade")
	public JMSTradeProcessResponse replayTrade(
			@WebParam(name="jmsTradeRequest") JMSTradeProcessRequest request,
			@WebParam(name="userName", header=true) String userName) {
		return opsTrade.replayTrade(request, userName);
	}
	
	@WebMethod(operationName="getInboundAttribList")
	public InboundAttribResponse getInboundAttribList(
			@WebParam(name="userName", header=true) String userName) {
		return opsTrade.getInboundAttribList(userName);
	}

	@WebMethod(operationName="updateAttributeMap")
	public InboundAttribMapResponse updateAttributeMap(
			@WebParam(name="attribRequest") InboundAttribMapRequest request, 
			@WebParam(name="userName", header=true) String userName) {
		return opsTrade.updateAttributeMap(request, userName);
	}

	@WebMethod(operationName="updateTradeConfirmCreator")
	public TradeConfirmCreatorResponse updateTradeConfirmCreator(
			@WebParam(name="tradeConfirmCreatorRequest")  TradeConfirmCreatorRequest request,
			@WebParam(name="userName", header=true)  String userName) {
		return opsTrade.updateTradeConfirmCreator(request, userName);
	}

	@WebMethod(operationName="getTradeConfirmCreator")
	public TradeConfirmCreatorResponse getTradeConfirmCreator(
			@WebParam(name="tradeConfirmCreatorRequest") TradeConfirmCreatorRequest request,
			@WebParam(name="userName", header=true) String userName) {
		return opsTrade.getTradeConfirmCreator(request, userName);
	}

	@WebMethod(operationName="getTradeCompany")
	public TradeCompanyResponse getTradeCompany(
			@WebParam(name="tradeCompanyRequest") TradeCompanyRequest request,
			@WebParam(name="userName", header=true) String userName) {

		return opsTrade.getTradeCompany(request, userName);
	}

		
}
