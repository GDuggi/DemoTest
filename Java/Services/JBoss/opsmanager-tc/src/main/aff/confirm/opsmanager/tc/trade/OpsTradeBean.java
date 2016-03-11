package aff.confirm.opsmanager.tc.trade;

import java.sql.SQLException;

import javax.annotation.Resource;
import javax.ejb.EJBException;
import javax.ejb.Stateless;
import javax.naming.NamingException;


import aff.confirm.opsmanager.common.BaseOpsBean;
import aff.confirm.opsmanager.common.data.BaseResponse;
import aff.confirm.opsmanager.common.util.OpsManagerUtil;
import aff.confirm.opsmanager.tc.common.TradeProcessor;
import aff.confirm.opsmanager.tc.common.TradeRqmtProcessor;
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
import org.jboss.logging.Logger;

@Stateless(name="OpsTrade",mappedName="OpsTrade")
public class OpsTradeBean extends BaseOpsBean implements OpsTrade {
	private static Logger log = Logger.getLogger(OpsTradeBean.class);
	TradeRqmtProcessor processor ;
	TradeProcessor tradeProcessor ;
	
	@Resource(name="maxGetTrades")
	private int maxGetTrades;
	
	@Resource(name="affinityURL")
	private String affDealSheetURL;
	
	@Resource(name="jmsURL")
	private String jmsDealSheetURL;
	
	@Resource(name="jmsReplyWorkDir")
	private String jmsWorkDir;
	
	@Resource(name="jmsReplyDropDir")
	private String jmsDropDir;
	
	public OpsTradeBean() throws NamingException {
		super();
		processor = new TradeRqmtProcessor(affinityConnection);
		tradeProcessor = new TradeProcessor(affinityConnection);
		
	}

	public DetermineActionResponse[] batchUpdateDetermineActions(DetermineActionRequest[] tradeActionList,
			String userName) {

		log.info("User(" + userName + ") batchUpdateDetermineActions called");
		
		DetermineActionResponse[] resp = new DetermineActionResponse[]{ new DetermineActionResponse()};
		
			try {
				OpsManagerUtil.createProxy(this.affinityConnection, userName);
				resp =processor.updateTradeActions(tradeActionList);
			} catch (SQLException e) {
				log.error("User(" + userName + ") batchUpdateDetermineActions error : " , e );
				OpsManagerUtil.populateErrorMessage(resp[1], e);
				throw new EJBException(e);
			}
			log.info("User(" + userName + ") batchUpdateDetermineActions returned");
			return resp;
		
			
	}

	public BaseResponse updateFinalApproval(int tradeId, String approvalStatus,
			String userName) {
	
		BaseResponse resp = new BaseResponse();
		log.info("User(" + userName + ") updateFinalApproval called");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			processor.updateFinalApproval(tradeId, approvalStatus);
			resp.setResponseStatus(BaseResponse.SUCCESS);
		} catch (SQLException e) {
			log.error("User(" + userName + ") updateFinalApproval error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") updateFinalApproval returned");
		return resp;

	}

	public BaseResponse updateTradeSummary(int tradeId,
			String finalApprovedFlag, String opsDetactFlag,
			String openRqmtFlag, String comment, String forceCmtNullFlag,
			String userName) {

		BaseResponse resp = new BaseResponse();
		log.info("User(" + userName + ") updateTradeSummary called");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			processor.updateTradeSummary(tradeId, finalApprovedFlag, opsDetactFlag, openRqmtFlag, comment, forceCmtNullFlag);
			resp.setResponseStatus(BaseResponse.SUCCESS);
		} catch (SQLException e) {
			log.error("User(" + userName + ") updateTradeSummary error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") updateTradeSummary returned");
		return resp;
		
	}

	public BaseResponse updateTradeSummaryComment(int tradeId, String comment,
			String userName) {

		BaseResponse resp = new BaseResponse();
		log.info("User(" + userName + ") updateTradeSummaryComment called");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			processor.updateTradeSummaryComment(tradeId, comment);
			resp.setResponseStatus(BaseResponse.SUCCESS);
		} catch (SQLException e) {
			log.error("User(" + userName + ") updateTradeSummaryComment error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		
		log.info("User(" + userName + ") updateTradeSummaryComment returned");
		return resp;
	}

	public TradeAuditResponse getTradeAudit(TradeAuditRequest request,String userName) {

		TradeAuditResponse resp = new TradeAuditResponse();
		log.info("User(" + userName + ") getTradeAudit called");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =tradeProcessor.getTradeAudit(request);
		} catch (SQLException e) {
			log.info("User(" + userName + ") getTradeAudit error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getTradeAudit returned");
		return resp;
	}

	public TradeDataChangeResponse getTradeDataChange(
			TradeDataChangeRequest request, String userName) {
		
		TradeDataChangeResponse resp = new TradeDataChangeResponse();
		log.info("User(" + userName + ") getTradeDataChange called");
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =tradeProcessor.getTradeCorrection(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getTradeDataChange error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getTradeDataChange returned");
		return resp;
	}

	public TradeRqmtResponse[] addRqmts(TradeRqmtRequest[] request,String userName) {
		
		log.info("User(" + userName + ") addRqmts called");
		
		TradeRqmtResponse[] resp = new TradeRqmtResponse[]{ new TradeRqmtResponse()};
		
			try {
				OpsManagerUtil.createProxy(this.affinityConnection, userName);
				resp =tradeProcessor.addRqmts(request);
			} catch (SQLException e) {
				log.error("User(" + userName + ") addRqmts error : " , e );
				OpsManagerUtil.populateErrorMessage(resp[0], e);
				throw new EJBException(e);
			}
			log.info("User(" + userName + ") addRqmts returned");
			return resp;
	
	}

	public TradeRqmtResponse[] updateTradeRqmts(
			 TradeRqmtRequest[] tradeRqmts,String userName) {
		
		log.info("User(" + userName + ") updateTradeRqmts called");
		
		TradeRqmtResponse[] resp = new TradeRqmtResponse[]{ new TradeRqmtResponse()};
		
			try {
				OpsManagerUtil.createProxy(this.affinityConnection, userName);
				 resp = tradeProcessor.updateRqmts(tradeRqmts);
			} catch (SQLException e) {
				log.error("User(" + userName + ") updateTradeRqmts error : " , e );
				OpsManagerUtil.populateErrorMessage(resp[0], e);
				throw new EJBException(e);
			}
			log.info("User(" + userName + ") updateTradeRqmts returned");
			return resp;

	}

	
	public TradeCommentResponse[] updateTradeComments(
			TradeCommentRequest[] tradeComments, String userName) {
		
		log.info("User(" + userName + ") updateTradeComments called");
		
		TradeCommentResponse[] resp = new TradeCommentResponse[] { new TradeCommentResponse() };
		
			try {
				OpsManagerUtil.createProxy(this.affinityConnection, userName);
				resp = tradeProcessor.updateTradeComments(tradeComments);
			} catch (SQLException e) {
				log.error( "User(" + userName + ") updateTradeComments error : ", e);
				OpsManagerUtil.populateErrorMessage(resp[0], e);
				throw new EJBException(e);
			}
			log.info("User(" + userName + ") updateTradeComments returned");
			return resp;

	}

	public GetTradeResponse getTrades(GetTradeRequest request,
			String userName) {
		
			log.info("User(" + userName + ") getTrades called");
			GetTradeResponse resp = new GetTradeResponse();
		
			try {
				OpsManagerUtil.createProxy(this.affinityConnection, userName);
				resp = tradeProcessor.getTrades(request,maxGetTrades,userName);
			} catch (SQLException e) {
				log.error("User(" + userName + ") getTrades error : " , e );
				OpsManagerUtil.populateErrorMessage(resp, e);
				throw new EJBException(e);
			} catch (Exception e) {
				log.error("User(" + userName + ") getTrades error : " , e );
				OpsManagerUtil.populateErrorMessage(resp, e);
			}
			log.info("User(" + userName + ") getTrades returned");
			return resp;

	}

	@Override
	public void prepareForMethodCall() {
		tradeProcessor.setDbConnection(this.affinityConnection);
		processor.setDbConnection(this.affinityConnection);
	}

	public TradeRqmtResponse updateTradeRqmtStatus(TradeRqmtRequest request,
			String userName) {

		log.info("User(" + userName + ") updateTradeRqmtStatus called");
		TradeRqmtResponse resp = new TradeRqmtResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp = tradeProcessor.updateRqmt(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") updateTradeRqmtStatus error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		} catch (Exception e) {
			log.error("User(" + userName + ") updateTradeRqmtStatus error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
		}
		log.info("User(" + userName + ") updateTradeRqmtStatus returned");
		return resp;
	}

	public DealSheetResponse getDealSheet(
			DealSheetRequest request,
			String userName) {

		log.info("User(" + userName + ") getDealSheet called");
		DealSheetResponse resp = new DealSheetResponse();
		
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp = tradeProcessor.getDealSheet(request,affDealSheetURL,jmsDealSheetURL);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getDealSheet error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		} catch (Exception e) {
			log.error("User(" + userName + ") getDealSheet error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
		}
		log.info("User(" + userName + ") getDealSheet returned");
		return resp;
		
	}

	public JMSTradeProcessResponse replayTrade(JMSTradeProcessRequest request,
			String userName) {
		
		log.info("User(" + userName + ") replyTrade called");
		JMSTradeProcessResponse resp = new JMSTradeProcessResponse();
		
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp = tradeProcessor.replayTrade(request,this.jmsWorkDir,this.jmsDropDir);
		} catch (SQLException e) {
			log.error("User(" + userName + ") replyTrade error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		} catch (Exception e) {
			log.error("User(" + userName + ") replyTrade error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
		}
		log.info("User(" + userName + ") replyTrade returned");
		return resp;
	}

public InboundAttribResponse getInboundAttribList(String userName) {
		
		log.info("User(" + userName + ") getInboundAttribList called");
		
		InboundAttribResponse resp = new InboundAttribResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =tradeProcessor.getInboundAttribList();
		} catch (SQLException e) {
			log.error("User(" + userName + ") getInboundAttribList error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getInboundAttribList returned");
		return resp;	
	}

	public InboundAttribMapResponse updateAttributeMap(
			InboundAttribMapRequest request, String userName) {
		
		log.info("User(" + userName + ") updateAttributeMap called");
		
		InboundAttribMapResponse resp = new InboundAttribMapResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =tradeProcessor.updateAttributeMap(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") updateAttributeMap error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") updateAttributeMap returned");
		return resp;	
	}

	public TradeConfirmCreatorResponse updateTradeConfirmCreator(TradeConfirmCreatorRequest request,
			String userName) {

		
		log.info("User(" + userName + ") update2ndCheck called");
		
		TradeConfirmCreatorResponse resp = new TradeConfirmCreatorResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.updateTradeConfirmCreator(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") update2ndCheck error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") update2ndCheck returned");
		return resp;
		
	}

	public TradeConfirmCreatorResponse getTradeConfirmCreator(TradeConfirmCreatorRequest request,
			String userName) {

		
		log.info("User(" + userName + ") get2ndCheck called");
		
		TradeConfirmCreatorResponse resp = new TradeConfirmCreatorResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp =processor.getTradeConfirmCreator(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") get2ndCheck error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") get2ndCheck returned");
		return resp;
		
	}

	public TradeCompanyResponse getTradeCompany(TradeCompanyRequest request,String userName) {
		
		log.info("User(" + userName + ") getTradeCompany called");
		
		TradeCompanyResponse resp = new TradeCompanyResponse();
	
		try {
			OpsManagerUtil.createProxy(this.affinityConnection, userName);
			resp = tradeProcessor.getBookingCompany(request);
		} catch (SQLException e) {
			log.error("User(" + userName + ") getTradeCompany error : " , e );
			OpsManagerUtil.populateErrorMessage(resp, e);
			throw new EJBException(e);
		}
		log.info("User(" + userName + ") getTradeCompany returned");
		return resp;
	
	}

}
