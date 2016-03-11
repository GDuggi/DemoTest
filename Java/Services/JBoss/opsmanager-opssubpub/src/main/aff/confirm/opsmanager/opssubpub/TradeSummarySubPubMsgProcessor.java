package aff.confirm.opsmanager.opssubpub;

import aff.confirm.common.daoinbound.inbound.ejb3.VPcTradeRqmtDAOLocal;
import aff.confirm.common.daoinbound.inbound.ejb3.VPcTradeSummaryDAOLocal;
import aff.confirm.common.daoinbound.inbound.model.VPcTradeRqmtEntity;
import aff.confirm.common.daoinbound.inbound.model.VPcTradeSummaryEntity;
import aff.confirm.common.util.JndiUtil;
import aff.confirm.opsmanager.opssubpub.opstrackingmodel.EntityConverter;
import aff.confirm.opsmanager.opssubpub.opstrackingmodel.RqmtData;
import aff.confirm.opsmanager.opssubpub.opstrackingmodel.SummaryData;

import javax.jms.*;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.util.Calendar;
import java.util.Hashtable;
import java.util.List;

//import org.jboss.naming.remote.client.InitialContextFactory;



/**
 * User: mthoresen
 * Date: Sep 20, 2012
 * Time: 8:16:51 AM
 */
public class TradeSummarySubPubMsgProcessor extends BaseSubPubMsgProcessor{

    private VPcTradeSummaryDAOLocal adoBean;
    private VPcTradeRqmtDAOLocal adoRqmtBean;

    // publishing
    private Connection jmsRqmtTopicConnection = null;
    private Session jmsRqmtTopicSession;
    private String jmsRqmtTopic;
    private MessageProducer rqmtProducer;

    public TradeSummarySubPubMsgProcessor(String jmsServer, String jmsUser, String jmsPwd, String jndiConnectionFactory, String providerContextFactory, String jmsQueue, String jmsTopic) {
        super(jmsServer, jmsUser, jmsPwd, jndiConnectionFactory, providerContextFactory, jmsQueue, jmsTopic);
    }

    public TradeSummarySubPubMsgProcessor(String jmsServer, String jmsUser, String jmsPwd, String jndiConnectionFactory, String providerContextFactory, String jmsQueue, String jmsTopic, String jmsRqmtTopic) {
        super(jmsServer, jmsUser, jmsPwd, jndiConnectionFactory, providerContextFactory, jmsQueue, jmsTopic);
        this.jmsRqmtTopic = jmsRqmtTopic;
        createRqmtMsgProducer();
    }

    protected void initAdo(InitialContext ctx) throws Exception{
            adoBean = JndiUtil.lookup(ctx, "java:global/InboundDocsDAOLib/VPcTradeSummaryDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.VPcTradeSummaryDAOLocal");
            adoRqmtBean = JndiUtil.lookup(ctx, "java:global/InboundDocsDAOLib/VPcTradeRqmtDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.VPcTradeRqmtDAOLocal");
    }

    @Override
    public void startListening() {
        super.startListening();
        try {
            jmsRqmtTopicConnection.start();
        } catch (JMSException e) {
            mylogger.error("startListening: " , e );
        }
    }

    @Override
    public void StopListening() {
        super.StopListening();
        try {
            jmsRqmtTopicConnection.stop();
        } catch (JMSException e) {
            mylogger.error("StopListening: " , e );
        }
    }

    private void createRqmtMsgProducer() {
        ConnectionFactory factory = null;
        Context jndiContext = null;
        try {
            Hashtable env = new Hashtable();
            env.put( Context.INITIAL_CONTEXT_FACTORY, providerContextFactory );
            env.put( Context.PROVIDER_URL, jmsServer );
            env.put( Context.SECURITY_PRINCIPAL, jmsUser );
            env.put( Context.SECURITY_CREDENTIALS, jmsPwd);
            jndiContext = new InitialContext( env );
            factory = ( ConnectionFactory ) jndiContext.lookup( jndiConnectionFactory );

            // setup publisher
            this.jmsRqmtTopicConnection = factory.createConnection(this.jmsUser, this.jmsPwd);
            this.jmsRqmtTopicSession = this.jmsRqmtTopicConnection.createSession(false, Session.AUTO_ACKNOWLEDGE);
            Topic topic = JndiUtil.lookup(jndiContext, "jms/topic/" + this.jmsRqmtTopic);
            rqmtProducer = this.jmsRqmtTopicSession.createProducer(topic);

        } catch (NamingException e) {
            mylogger.error("createRqmtMsgProducer: " , e );
        } catch (JMSException e) {
            mylogger.error("createRqmtMsgProducer: " , e );
        }
    }

    public void processMessage(List<String> msgList) {
        long tradeId = 0;
        long lastSeq = 0;
        lastSeq = Long.parseLong(msgList.get(3).toString());
        tradeId = Long.parseLong(msgList.get(2).toString());
        VPcTradeSummaryEntity entity = new VPcTradeSummaryEntity();
        mylogger.info("Getting Entity for Trade ID: " + tradeId);
        // TRADE SUMMARY DATA
        entity = adoBean.findById(new Long(tradeId),false);
        if (entity == null){
            mylogger.info("Entity object is NULL for Trade ID: " + tradeId);            
        }
        else{
            postTradeSummaryMessage(entity);
            // Need to publish to TradeRqmtTopic as well here...
            // TRADE RQMTS DATA
            List<VPcTradeRqmtEntity> rqmtList;
            VPcTradeRqmtEntity template = new VPcTradeRqmtEntity();
            template.setTradeId(new Long(tradeId));
            rqmtList = adoRqmtBean.findByExample(template, "");
            if(rqmtList == null || rqmtList.size() == 0){
                mylogger.info("Rqmt List Entity object is NULL for Trade ID: " + tradeId);            
            }
            else{
                processTradeRqmtMessage(rqmtList);
            }
        }
    }

    private void processTradeRqmtMessage(List<VPcTradeRqmtEntity> rqmtList) {
        RqmtData rqmtData = null;
        Calendar cal = Calendar.getInstance();
        try{
            for (int i = 0; i < rqmtList.size(); i++) {
                VPcTradeRqmtEntity vPcTradeRqmtEntity = rqmtList.get(i);
                rqmtData = new RqmtData();
                rqmtData.set_id(vPcTradeRqmtEntity.getId());
                rqmtData.set_tradeId(vPcTradeRqmtEntity.getTradeId());

                if (vPcTradeRqmtEntity.getRqmtTradeNotifyId() != null){
                    rqmtData.set_rqmtTradeNotifyId(vPcTradeRqmtEntity.getRqmtTradeNotifyId());
                }

                rqmtData.set_rqmt(vPcTradeRqmtEntity.getRqmt());
                rqmtData.set_trdSysTicket(vPcTradeRqmtEntity.getTrdSysTicket());
                rqmtData.set_trdSysCode(vPcTradeRqmtEntity.getTrdSysCode());
                rqmtData.set_status(vPcTradeRqmtEntity.getStatus());
                if(vPcTradeRqmtEntity.getCompletedDt() != null){
                    rqmtData.set_completedDt(new java.util.Date(vPcTradeRqmtEntity.getCompletedDt().getTime()));
                }
                if(vPcTradeRqmtEntity.getCompletedTimestampGmt() != null){
                    cal.setTime(vPcTradeRqmtEntity.getCompletedTimestampGmt());
                    if(cal.get(Calendar.YEAR) > 1){
                        rqmtData.set_completedTimestampGmt(new java.util.Date(vPcTradeRqmtEntity.getCompletedTimestampGmt().getTime()));
                    }else rqmtData.set_completedTimestampGmt(null);
                }
                rqmtData.set_reference(vPcTradeRqmtEntity.getReference());

                if(vPcTradeRqmtEntity.getCancelTradeNotifyId() != null){
                    rqmtData.set_cancelTradeNotifyId(vPcTradeRqmtEntity.getCancelTradeNotifyId());
                }
                rqmtData.set_cmt(vPcTradeRqmtEntity.getCmt());
                rqmtData.set_secondCheckFlag(vPcTradeRqmtEntity.getSecondCheckFlag());
                rqmtData.set_transactionSeq(vPcTradeRqmtEntity.getTransactionSeq());
                rqmtData.set_finalApprovalFlag(vPcTradeRqmtEntity.getFinalApprovalFlag());
                rqmtData.set_displayText(vPcTradeRqmtEntity.getDisplayText());
                rqmtData.set_category(vPcTradeRqmtEntity.getCategory());
                rqmtData.set_terminalFlag(vPcTradeRqmtEntity.getTerminalFlag());
                rqmtData.set_problemFlag(vPcTradeRqmtEntity.getProblemFlag());
                rqmtData.set_guiColorCode(vPcTradeRqmtEntity.getGuiColorCode());
                rqmtData.set_delphiConstant(vPcTradeRqmtEntity.getDelphiConstant());

                OpsManagerMessage message = new OpsManagerMessage();
                message.setData(rqmtData);
                message.setMessageType("rqmt");
                this.sendMessage(rqmtProducer,message);
            }
        } catch(Exception ex){
            mylogger.error("Exception in processTradeRqmtMessage: " + ex.getMessage());
        }
    }

    private void postTradeSummaryMessage(VPcTradeSummaryEntity entity)
    {
        try
        {
            SummaryData summaryData = EntityConverter.createSummaryData(entity);
            if (entity == null )
            {
                return;
            }

            OpsManagerMessage message = new OpsManagerMessage();
            message.setData(summaryData);
            message.setMessageType("summary");
            sendMessage(message);
        }
        catch(Exception ex)
        {
             mylogger.info("Exception in postTradeSummaryMessage.  Trade ID: " + entity.getTradeId() + ". " + ex.getMessage(), ex);
        }
    }
}
