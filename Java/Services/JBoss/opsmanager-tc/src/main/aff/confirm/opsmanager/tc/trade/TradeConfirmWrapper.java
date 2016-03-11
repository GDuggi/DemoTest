package aff.confirm.opsmanager.tc.trade;

import javax.ejb.Remote;

import aff.confirm.opsmanager.common.data.*;
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

@Remote
public interface TradeConfirmWrapper {
	BaseResponse updateFinalApproval(int tradeId, String approvalStatus,String userName);
	BaseResponse updateTradeSummary(int tradeId,String finalApprovedFlag,String opsDetactFlag, String openRqmtFlat,String comment, String forceCmtNullFlag,String userName);
	BaseResponse updateTradeSummaryComment(int tradeId, String comment,String userName);
	
	
	FinalApproveResponse[] finalApprove(FinalApproveRequest[] request,String userName);
	ReopenResponse[] reopen(ReopenRequest[] request,String userName);
	
	TradeGroupResponse[] groupTrades(TradeGroupRequest[] request,String userName);
	TradeUnGroupResponse ungroupTrades(TradeUnGroupRequest request,String userName);
	
	TradeAuditResponse getTradeAudit(TradeAuditRequest request,String userName);
	TradeDataChangeResponse getTradeDataChange(TradeDataChangeRequest request, String userName);
	
	DetermineActionResponse[] batchUpdateDetermineActions(DetermineActionRequest[] request,String userName);
	
	GetTradeResponse getTrades(GetTradeRequest request, String userName);
	
	TradeRqmtResponse[] addRqmts(TradeRqmtRequest[] request,String userName);
	TradeRqmtResponse[] updateTradeRqmts(TradeRqmtRequest[] tradeRqmtRequest,String userName);
	TradeRqmtResponse updateTradeRqmtStatus(TradeRqmtRequest tradeRqmtRequest,String userName);	
	TradeCommentResponse[] updateTradeComments(TradeCommentRequest[] tradeCommentRequest ,String userName);
	
	DealSheetResponse getDealSheetData(DealSheetRequest request, String userName);
	JMSTradeProcessResponse replayTrade(JMSTradeProcessRequest request, String userName);
	InboundAttribResponse getInboundAttribList(String userName);
	InboundAttribMapResponse updateAttributeMap(InboundAttribMapRequest request,String userName );
	
	TradeConfirmCreatorResponse updateTradeConfirmCreator(TradeConfirmCreatorRequest request, String userName);
	TradeConfirmCreatorResponse getTradeConfirmCreator(TradeConfirmCreatorRequest request, String userName);
	TradeCompanyResponse getTradeCompany(TradeCompanyRequest request,String userName);
	
}
