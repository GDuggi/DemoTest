package aff.confirm.opsmanager.tc.trade;

import javax.ejb.Remote;

import aff.confirm.opsmanager.common.data.*;
import aff.confirm.opsmanager.tc.data.DetermineActionRequest;
import aff.confirm.opsmanager.tc.data.DetermineActionResponse;
import aff.confirm.opsmanager.tc.data.DealSheetRequest;
import aff.confirm.opsmanager.tc.data.DealSheetResponse;
import aff.confirm.opsmanager.tc.data.GetTradeRequest;
import aff.confirm.opsmanager.tc.data.GetTradeResponse;
import aff.confirm.opsmanager.tc.data.InboundAttribMapRequest;
import aff.confirm.opsmanager.tc.data.InboundAttribMapResponse;
import aff.confirm.opsmanager.tc.data.InboundAttribResponse;
import aff.confirm.opsmanager.tc.data.JMSTradeProcessRequest;
import aff.confirm.opsmanager.tc.data.JMSTradeProcessResponse;
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
import aff.confirm.opsmanager.tc.data.TradeRqmtRequest;
import aff.confirm.opsmanager.tc.data.TradeRqmtResponse;

@Remote
public interface OpsTrade {

	BaseResponse updateFinalApproval(int tradeId, String approvalStatus,String userName);
	BaseResponse updateTradeSummary(int tradeId,String finalApprovedFlag,String opsDetactFlag, String openRqmtFlat,String comment, String forceCmtNullFlag,String userName);
	BaseResponse updateTradeSummaryComment(int tradeId, String comment,String userName);
		
	DetermineActionResponse[] batchUpdateDetermineActions(DetermineActionRequest[] tradeActionList,String userName);
	
	TradeAuditResponse getTradeAudit(TradeAuditRequest request,String userName);
	TradeDataChangeResponse getTradeDataChange(TradeDataChangeRequest request, String userName);
	
	TradeRqmtResponse[] addRqmts(TradeRqmtRequest[] request,String userName);
	TradeRqmtResponse[] updateTradeRqmts( TradeRqmtRequest[] tradeRqmts,String userName);
	TradeCommentResponse[] updateTradeComments(TradeCommentRequest[] tradeComments,String userName);
	TradeRqmtResponse updateTradeRqmtStatus(TradeRqmtRequest request,String userName);
	
	GetTradeResponse getTrades(GetTradeRequest request, String userName);
	DealSheetResponse getDealSheet(DealSheetRequest request, String userName);
	JMSTradeProcessResponse replayTrade(JMSTradeProcessRequest request, String userName);
	
	InboundAttribResponse getInboundAttribList(String userName);
	InboundAttribMapResponse updateAttributeMap(InboundAttribMapRequest request,String userName );
	
	TradeConfirmCreatorResponse updateTradeConfirmCreator(TradeConfirmCreatorRequest request, String userName);
	TradeConfirmCreatorResponse getTradeConfirmCreator(TradeConfirmCreatorRequest request, String userName);
	
	TradeCompanyResponse getTradeCompany(TradeCompanyRequest request,String userName);
	
}
