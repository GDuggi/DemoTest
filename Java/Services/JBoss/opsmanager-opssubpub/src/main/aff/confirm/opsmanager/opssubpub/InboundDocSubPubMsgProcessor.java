package aff.confirm.opsmanager.opssubpub;

import aff.confirm.common.daoinbound.inbound.ejb3.VActiveAssociatedDocsDAOLocal;
import aff.confirm.common.daoinbound.inbound.ejb3.VInboundDocsDAOLocal;
import aff.confirm.common.daoinbound.inbound.model.VActiveAssociatedDocsEntity;
import aff.confirm.common.daoinbound.inbound.model.VInboundDocsEntity;
import aff.confirm.common.util.JndiUtil;
import aff.confirm.opsmanager.opssubpub.opstrackingmodel.AssociatedDoc;
import aff.confirm.opsmanager.opssubpub.opstrackingmodel.InboundDocsView;
import org.jboss.logging.Logger;

import javax.jms.*;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.util.Calendar;
import java.util.Hashtable;
import java.util.List;

/**
 * User: mthoresen
 * Date: Sep 20, 2012
 * Time: 8:20:41 AM
 */
public class InboundDocSubPubMsgProcessor extends BaseSubPubMsgProcessor{
    private static Logger log = Logger.getLogger(InboundDocSubPubMsgProcessor.class );
    private VInboundDocsDAOLocal adoBean;
    private VActiveAssociatedDocsDAOLocal adoAssocDocBean;

    // publishing - secondary AssocDoc topic
    private Connection jmsAssocDocTopicConnection = null;
    private Session jmsAssocDocTopicSession;
    private String jmsAssocDocTopic;
    private MessageProducer assocDocProducer;


    public InboundDocSubPubMsgProcessor(String jmsServer, String jmsUser, String jmsPwd, String jndiConnectionFactory, String providerContextFactory, String jmsQueue, String jmsTopic) {
        super(jmsServer, jmsUser, jmsPwd, jndiConnectionFactory, providerContextFactory, jmsQueue, jmsTopic);
    }

    public InboundDocSubPubMsgProcessor(String jmsServer, String jmsUser, String jmsPwd, String jndiConnectionFactory, String providerContextFactory, String jmsQueue, String jmsTopic, String jmsAssocDocTopic) {
        super(jmsServer, jmsUser, jmsPwd, jndiConnectionFactory, providerContextFactory, jmsQueue, jmsTopic);
        this.jmsAssocDocTopic = jmsAssocDocTopic;
        createAssocDocMsgProducer();
    }

    private void createAssocDocMsgProducer() {
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
            this.jmsAssocDocTopicConnection = factory.createConnection(this.jmsUser, this.jmsPwd);
            this.jmsAssocDocTopicSession = this.jmsAssocDocTopicConnection.createSession(false, Session.AUTO_ACKNOWLEDGE);
            Topic topic = JndiUtil.lookup(jndiContext,"jms/topic/"+this.jmsAssocDocTopic);
            assocDocProducer = this.jmsAssocDocTopicSession.createProducer(topic);

        } catch (Exception e) {
            log.error("ERROR", e);
        }
    }

    protected void initAdo(InitialContext ctx)  throws Exception{
        adoBean = JndiUtil.lookup(ctx,"java:global/InboundDocsDAOLib/VInboundDocsDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.VInboundDocsDAOLocal");
        adoAssocDocBean = JndiUtil.lookup(ctx,"java:global/InboundDocsDAOLib/VActiveAssociatedDocsDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.VActiveAssociatedDocsDAOLocal");
    }

    public void processMessage(List<String> msgList) {
        long id = Long.parseLong(msgList.get(1));
        String docStatusCode = msgList.get(3);

        VInboundDocsEntity entity = new VInboundDocsEntity();
        this.mylogger.info("Getting Entity for Inbound DocID: " + id);
        // TRADE SUMMARY DATA
        entity = adoBean.findById(new Long(id),false);
        postMessage(entity);
        // we need to update the SENT_TO value for all AssociatedDoc children for this InboundDoc
        List<VActiveAssociatedDocsEntity> assocDocList;
        VActiveAssociatedDocsEntity template = new VActiveAssociatedDocsEntity();
        template.setInboundDocsId(new Long(id));
        assocDocList = adoAssocDocBean.findByExample(template, "");
        processAssocDocMessage(assocDocList, entity);
    }

    @Override
    public void startListening() {
        super.startListening();
        try {
            jmsAssocDocTopicConnection.start();
        } catch (JMSException e) {
            log.error( "ERROR", e );
        }
    }

    @Override
    public void StopListening() {
        super.StopListening();
        try {
            jmsAssocDocTopicConnection.stop();
        } catch (JMSException e) {
            log.error( "ERROR", e );
        }
    }

    private void processAssocDocMessage(List<VActiveAssociatedDocsEntity> assocDocList, VInboundDocsEntity inbDoc) {
        AssociatedDoc data = null;
        Calendar cal = Calendar.getInstance();
        for (int i = 0; i < assocDocList.size(); i++) {
            VActiveAssociatedDocsEntity entity = assocDocList.get(i);
            data = new AssociatedDoc();

            data.set_id(entity.getId());
            data.set_inboundDocsId(entity.getInboundDocsId());
            data.set_indexVal(entity.getIndexVal());
            data.set_trdSysTicket(entity.getTrdSysTicket());
            data.set_trdSysCode(entity.getTrdSysCode());
            data.set_fileName(entity.getFileName());
            data.set_tradeId(entity.getTradeId());
            data.set_docStatusCode(entity.getDocStatusCode());

            data.set_associatedBy(entity.getAssociatedBy());
            if(entity.getAssociatedDt() != null){
                cal.setTime(entity.getAssociatedDt());
                if(cal.get(Calendar.YEAR) > 1){
                    data.set_associatedDt(new java.util.Date(entity.getAssociatedDt().getTime()));
                }else data.set_associatedDt(null);
            }

            data.set_finalApprovedBy(entity.getFinalApprovedBy());
            if(entity.getFinalApprovedDt() != null){
                cal.setTime(entity.getFinalApprovedDt());
                if(cal.get(Calendar.YEAR) > 1){
                    data.set_associatedDt(new java.util.Date(entity.getFinalApprovedDt().getTime()));
                }else data.set_finalApprovedDt(null);
            }

            data.set_disputedBy(entity.getDisputedBy());
            if(entity.getDisputedDt() != null){
                cal.setTime(entity.getDisputedDt());
                if(cal.get(Calendar.YEAR) > 1){
                    data.set_associatedDt(new java.util.Date(entity.getDisputedDt().getTime()));
                }else data.set_disputedDt(null);
            }

            data.set_discardedBy(entity.getDiscardedBy());
            if(entity.getDiscardedDt() != null){
                cal.setTime(entity.getDiscardedDt());
                if(cal.get(Calendar.YEAR) > 1){
                    data.set_associatedDt(new java.util.Date(entity.getDiscardedDt().getTime()));
                }else data.set_discardedDt(null);
            }

            data.set_vaultedBy(entity.getVaultedBy());
            if(entity.getVaultedDt() != null){
                cal.setTime(entity.getVaultedDt());
                if(cal.get(Calendar.YEAR) > 1){
                    data.set_associatedDt(new java.util.Date(entity.getVaultedDt().getTime()));
                }else data.set_vaultedDt(null);
            }

            data.set_cdtyGroupCode(entity.getCdtyGroupCode());
            data.set_cptyShortName(entity.getCptySn());
            data.set_brokerShortName(entity.getBrokerSn());
            data.set_docTypeCode(entity.getDocTypeCode());
            data.set_secondValidateReqFlag(entity.getSecValidateReqFlag());
            data.set_tradeRqmtId(entity.getTradeRqmtId());

            data.set_tradeFinalApprovalFlag(entity.getTradeFinalApprovalFlag());
            data.set_xmitStatusCode(entity.getXmitStatusCode());
            data.set_xmitValue(entity.getXmitValue());
            data.set_sentTo(inbDoc.getSentTo());
            
            OpsManagerMessage message = new OpsManagerMessage();
            message.setData(data);
            message.setMessageType("assoc");
            this.sendMessage(assocDocProducer,message);
        }
    }

    private void postMessage(VInboundDocsEntity entity) {
        InboundDocsView data = null;
        Calendar cal = Calendar.getInstance();
        try{
            if(entity != null){
                data = new InboundDocsView();
                data.set_id(entity.getId());
                data.set_unresolvedCount(entity.getUnresolvedcount());
                data.set_callerRef(entity.getCallerRef());
                data.set_sentTo(entity.getSentTo());
                if(entity.getRcvdTs() != null){
                    cal.setTime(entity.getRcvdTs());
                    if(cal.get(Calendar.YEAR) > 1){
                        data.set_rcvdTs(new java.util.Date(entity.getRcvdTs().getTime()));
                    }else data.set_rcvdTs(null);
                }
                data.set_fileName(entity.getFileName());
                data.set_sender(entity.getSender());
                data.set_cmt(entity.getCmt());
                data.set_docStatusCode(entity.getDocStatusCode());
                data.set_hasAutoAsctedFlag(entity.getHasAutoAsctedFlag());
                data.set_tradeIds(entity.getTradeids());
                data.set_mappedCptySn(entity.getMappedCptySn());

                data.set_procFlag(entity.getProcFlag());

                OpsManagerMessage message = new OpsManagerMessage();
                message.setData(data);
                message.setMessageType("inbound");
                this.sendMessage(message);
            }

        } catch(Exception ex){
            mylogger.error("postMessage exception : "  + ex.getMessage(),ex);
        }
    }
}
