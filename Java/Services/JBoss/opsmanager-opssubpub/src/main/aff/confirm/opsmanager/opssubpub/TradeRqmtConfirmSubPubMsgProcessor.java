package aff.confirm.opsmanager.opssubpub;

import aff.confirm.common.daoinbound.inbound.ejb3.VPcTradeSummaryDAOLocal;
import aff.confirm.common.daoinbound.inbound.ejb3.VTradeRqmtConfirmDAOLocal;
import aff.confirm.common.daoinbound.inbound.model.VPcTradeSummaryEntity;
import aff.confirm.common.daoinbound.inbound.model.VTradeRqmtConfirmEntity;
import aff.confirm.common.util.JndiUtil;
import aff.confirm.opsmanager.opssubpub.opstrackingmodel.EntityConverter;
import aff.confirm.opsmanager.opssubpub.opstrackingmodel.SummaryData;
import aff.confirm.opsmanager.opssubpub.opstrackingmodel.TradeRqmtConfirm;

import javax.jms.*;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.util.Calendar;
import java.util.Hashtable;
import java.util.List;
import org.jboss.logging.Logger;


/**
 * User: mthoresen
 * Date: Sep 20, 2012
 * Time: 8:21:10 AM
 */
public class TradeRqmtConfirmSubPubMsgProcessor extends BaseSubPubMsgProcessor{
    private static Logger log = Logger.getLogger( TradeRqmtConfirmSubPubMsgProcessor.class );
    private VTradeRqmtConfirmDAOLocal adoBean;
    private VPcTradeSummaryDAOLocal adoTradeSummaryBean;

    // publishing
    private Connection jmsTradeSummaryTopicConnection = null;
    private Session jmsTradeSummaryTopicSession;
    private String jmsTradeSummaryTopic;
    private MessageProducer tradeSummaryProducer;
    

    public TradeRqmtConfirmSubPubMsgProcessor(String jmsServer, String jmsUser, String jmsPwd, String jndiConnectionFactory, String providerContextFactory, String jmsQueue, String jmsTopic) {
        super(jmsServer, jmsUser, jmsPwd, jndiConnectionFactory, providerContextFactory, jmsQueue, jmsTopic);
    }

    public TradeRqmtConfirmSubPubMsgProcessor(String jmsServer, String jmsUser, String jmsPwd, String jndiConnectionFactory, String providerContextFactory, String jmsQueue, String jmsTopic, String jmsTradeSummaryTopic) {
        super(jmsServer, jmsUser, jmsPwd, jndiConnectionFactory, providerContextFactory, jmsQueue, jmsTopic);
        this.jmsTradeSummaryTopic = jmsTradeSummaryTopic;
        createTradeSummaryMsgProducer();
    }

    private void createTradeSummaryMsgProducer() {
        ConnectionFactory factory = null;
        Context jndiContext = null;
        try {
            Hashtable env = new Hashtable();
            env.put( Context.INITIAL_CONTEXT_FACTORY, providerContextFactory );
            env.put( Context.PROVIDER_URL, jmsServer );
            env.put( Context.SECURITY_PRINCIPAL, jmsUser );
            env.put( Context.SECURITY_CREDENTIALS, jmsPwd);
            jndiContext = new InitialContext( env );
            factory = JndiUtil.lookup(jndiContext,jndiConnectionFactory);

            // setup publisher
            this.jmsTradeSummaryTopicConnection = factory.createConnection(this.jmsUser, this.jmsPwd);
            this.jmsTradeSummaryTopicSession = this.jmsTradeSummaryTopicConnection.createSession(false, Session.AUTO_ACKNOWLEDGE);
            Topic topic = JndiUtil.lookup(jndiContext, "jms/topic/" + this.jmsTradeSummaryTopic);
            tradeSummaryProducer = this.jmsTradeSummaryTopicSession.createProducer(topic);
        } catch (NamingException | JMSException e) {
            log.error( "ERROR", e);
        }
    }

    @Override
    public void startListening() {
        super.startListening();
        try {
            jmsTradeSummaryTopicConnection.start();
        } catch (JMSException e) {
            log.error( "ERROR", e);
        }
    }

    @Override
    public void StopListening() {
        super.StopListening();
        try {
            jmsTradeSummaryTopicConnection.stop();
        } catch (JMSException e) {
            log.error( "ERROR", e);
        }
    }

    protected void initAdo(InitialContext ctx)  throws  Exception{
        adoBean = JndiUtil.lookup(ctx, "java:global/InboundDocsDAOLib/VTradeRqmtConfirmDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.VTradeRqmtConfirmDAOLocal");
        adoTradeSummaryBean = JndiUtil.lookup(ctx,"java:global/InboundDocsDAOLib/VPcTradeSummaryDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.VPcTradeSummaryDAOLocal");
    }

    public void processMessage(List<String> msgList) {
        long id = 0;
        id = Long.parseLong(msgList.get(1));

        this.mylogger.info("Getting Entity for Trade Confirm ID: " + id);

        VTradeRqmtConfirmEntity entity = adoBean.findById(new Long(id),false);
        postTradeRqmtConfirmMessage(entity);

        // need to publish to trade summary topic. Reason:  RPLY_RDY_TO_SND_FLAG needs to be updated in client
        postSetReadyForReplyFlag(entity);
    }

    private void postSetReadyForReplyFlag(VTradeRqmtConfirmEntity entity) {
        String flag = "N";
        VTradeRqmtConfirmEntity data = null;
        try{
            VTradeRqmtConfirmEntity template = new VTradeRqmtConfirmEntity();
            template.setTradeId(new Long(entity.getTradeId()));
            template.setNextStatusCode("MGR");

            List<VTradeRqmtConfirmEntity> rqmtConfirmList = adoBean.findByExample(template, "");
            for (int i = 0; i < rqmtConfirmList.size(); i++)
            {
            	data = rqmtConfirmList.get(i);
				if((data.getConfirmLabel() == null) || ("".equals(data.getConfirmLabel()))){
					flag = "Y";
					break;
				}
            }

            VPcTradeSummaryEntity tradeSummaryBean = adoTradeSummaryBean.findById(new Long(entity.getTradeId()),false);
            publishTradeSummaryMessage(tradeSummaryBean);
        }
        catch (Exception e) {
            mylogger.getLogger("TradeRqmtConfirmListener").info("Exception SetReplyRdyToSendFlag: " , e );
        }
    }

    private void publishTradeSummaryMessage(VPcTradeSummaryEntity entity)
    {
        try
        {
            SummaryData summaryData = EntityConverter.createSummaryData(entity);
            if (entity == null)
            {
                return;
            }

            OpsManagerMessage message = new OpsManagerMessage();
            message.setData(summaryData);
            message.setMessageType("summary");
            sendMessage(tradeSummaryProducer, message);
        }
        catch(Exception ex)
        {
            mylogger.error("publishTradeSummaryMessage exception : "  + ex.getMessage(), ex);
        }
    }


    private void postTradeRqmtConfirmMessage(VTradeRqmtConfirmEntity entity) {
        try{
            TradeRqmtConfirm data = null;
            Calendar cal = Calendar.getInstance();

            if(entity != null){
                data = new TradeRqmtConfirm();

                data.set_id(entity.getId());
                data.set_confirmCmt(entity.getConfirmCmt());
                data.set_confirmLabel(entity.getConfirmLabel());
                data.set_faxTelexInd(entity.getFaxTelexInd());
                data.set_faxTelexNumber(entity.getFaxTelexNumber());
                data.set_finalApprovalFlag(entity.getFinalApprovalFlag());
                data.set_rqmtId(entity.getRqmtId());
                data.set_templateCategory(""); // Todo - see if this can be removed.
                data.set_templateId(0);
                data.set_templateName(entity.getTemplateName());
                data.set_templateTypeInd("");  // Todo - see if this can be removed.
                data.set_tradeId(entity.getTradeId());
                data.set_xmitAddr(entity.getXmitAddr());
                data.set_xmitCmt(entity.getXmitCmt());
                data.set_xmitStatusInd(entity.getXmitStatusInd());
                data.set_nextStatusCode(entity.getNextStatusCode());
                data.set_activeFlag(entity.getActiveFlag());
                data.set_preparerCanSendFlag(entity.getPreparerCanSendFlag());

                if(entity.getXmitTimestampGmt() != null){
                    cal.setTime(entity.getXmitTimestampGmt());
                    if(cal.get(Calendar.YEAR) > 1){
                        data.set_xmitTimeStampGmt(new java.util.Date(entity.getXmitTimestampGmt().getTime()));
                    }else data.set_xmitTimeStampGmt(null);
                }

                OpsManagerMessage message = new OpsManagerMessage();
                message.setData(data);
                message.setMessageType("confirm");
                this.sendMessage(message);
            }

        } catch(Exception ex){
            mylogger.error("postTradeRqmtConfirmMessage exception : "  + ex.getMessage(),ex);
        }
    }
}
