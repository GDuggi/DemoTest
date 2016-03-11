package aff.confirm.opsmanager.opssubpub;


import aff.confirm.jboss.common.service.BasicMBeanSupport;
import org.jboss.logging.Logger;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;


/**
 * User: mthoresen
 * Date: Sep 19, 2012
 * Time: 11:20:37 AM
 */
@Startup
@Singleton
public class OpsJMSSubPubManagerService extends BasicMBeanSupport implements OpsJMSSubPubManagerServiceMBean {

    private String jmsServer;
    private String jmsUser;
    private String jmsPwd;
    private String jndiConnectionFactory;
    private String providerContextFactory;
    private String tradeDataQueue;
    private String tradeDataTopic;
    private String tradeRqmtTopic;
    private String inbDocDataQueue;
    private String inbDocDataTopic;
    private String assocDocDataQueue;
    private String assocDocDataTopic;
    private String tradeRqmtConfirmDataQueue;
    private String tradeRqmtConfirmDataTopic;

    // SubPub processors
    TradeSummarySubPubMsgProcessor tradeSummarySubPubMsgProcessor;
    InboundDocSubPubMsgProcessor inboundDocSubPubMsgProcessor;
    AssocDocSubPubMsgProcessor assocDocSubPubMsgProcessor;
    TradeRqmtConfirmSubPubMsgProcessor tradeRqmtConfirmSubPubMsgProcessor;

    public OpsJMSSubPubManagerService() {
        super("affinity.confirm.jms:service=OpsJMSSubPubManagerService");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
       super.preDestroy();
    }

    public String getJmsServer() {
        return jmsServer;
    }

    public void setJmsServer(String jmsServer) {
        this.jmsServer = jmsServer;
    }

    public String getJmsUser() {
        return jmsUser;
    }

    public void setJmsUser(String jmsUser) {
        this.jmsUser = jmsUser;
    }

    public String getJmsPwd() {
        return jmsPwd;
    }

    public void setJmsPwd(String jmsPwd) {
        this.jmsPwd = jmsPwd;
    }

    public String getJndiConnectionFactory() {
        return jndiConnectionFactory;
    }

    public void setJndiConnectionFactory(String jndiConnectionFactory) {
        this.jndiConnectionFactory = jndiConnectionFactory;
    }

    public String getProviderContextFactory() {
        return providerContextFactory;
    }

    public void setProviderContextFactory(String providerContextFactory) {
        this.providerContextFactory = providerContextFactory;
    }

    public String getTradeDataQueue() {
        return this.tradeDataQueue;
    }

    public void setTradeDataQueue(String tradeDataQueue) {
        this.tradeDataQueue = tradeDataQueue;
    }

    public String getTradeDataTopic() {
        return this.tradeDataTopic;
    }

    public void setTradeDataTopic(String tradeDataTopic) {
        this.tradeDataTopic = tradeDataTopic;
    }

    public String getTradeRqmtTopic() {
        return this.tradeRqmtTopic;
    }

    public void setTradeRqmtTopic(String tradeRqmtTopic) {
        this.tradeRqmtTopic = tradeRqmtTopic;
    }

    public String getInbDocDataQueue() {
        return inbDocDataQueue;
    }// inbound doc Qs/Topics

    public void setInbDocDataQueue(String inbDocDataQueue) {
        this.inbDocDataQueue = inbDocDataQueue;
    }

    public String getInbDocDataTopic() {
        return inbDocDataTopic;
    }

    public void setInbDocDataTopic(String inbDocDataTopic) {
        this.inbDocDataTopic = inbDocDataTopic;
    }

    public String getAssocDocDataQueue() {
        return assocDocDataQueue;
    }

    public void setAssocDocDataQueue(String assocDocDataQueue) {
        this.assocDocDataQueue = assocDocDataQueue;
    }

    public String getAssocDocDataTopic() {
        return assocDocDataTopic;
    }

    public void setAssocDocDataTopic(String assocDocDataTopic) {
        this.assocDocDataTopic = assocDocDataTopic;
    }

    public String getTradeRqmtConfirmDataQueue() {
        return tradeRqmtConfirmDataQueue;
    }

    public void setTradeRqmtConfirmDataQueue(String tradeRqmtConfirmDataQueue) {
        this.tradeRqmtConfirmDataQueue = tradeRqmtConfirmDataQueue;
    }

    public String getTradeRqmtConfirmDataTopic() {
        return tradeRqmtConfirmDataTopic;
    }

    public void setTradeRqmtConfirmDataTopic(String tradeRqmtConfirmDataTopic) {
        this.tradeRqmtConfirmDataTopic = tradeRqmtConfirmDataTopic;
    }

    private void createTradeRqmtConfirmSubPubMsgProcessor() {
        this.tradeRqmtConfirmSubPubMsgProcessor = new  TradeRqmtConfirmSubPubMsgProcessor(
                this.jmsServer,
                this.jmsUser,
                this.jmsPwd,
                this.jndiConnectionFactory,
                this.providerContextFactory,
                this.tradeRqmtConfirmDataQueue,
                this.tradeRqmtConfirmDataTopic,
                this.tradeDataTopic);
        this.tradeRqmtConfirmSubPubMsgProcessor.startListening();
    }

    private void createAssocDocSubPubMsgProcessor() {
        this.assocDocSubPubMsgProcessor = new  AssocDocSubPubMsgProcessor(
                this.jmsServer,
                this.jmsUser,
                this.jmsPwd,
                this.jndiConnectionFactory,
                this.providerContextFactory,
                this.assocDocDataQueue,
                this.assocDocDataTopic);
        this.assocDocSubPubMsgProcessor.startListening();
    }

    private void createInboundDocSubPubMsgProcessor() {
        this.inboundDocSubPubMsgProcessor = new  InboundDocSubPubMsgProcessor(
                this.jmsServer,
                this.jmsUser,
                this.jmsPwd,
                this.jndiConnectionFactory,
                this.providerContextFactory,
                this.inbDocDataQueue,
                this.inbDocDataTopic,
                this.assocDocDataTopic);
        this.inboundDocSubPubMsgProcessor.startListening();
    }

    private void createTradeSummarySubPubMsgProcessor() {
        this.tradeSummarySubPubMsgProcessor = new  TradeSummarySubPubMsgProcessor(
                this.jmsServer,
                this.jmsUser,
                this.jmsPwd,
                this.jndiConnectionFactory,
                this.providerContextFactory,
                this.tradeDataQueue,
                this.tradeDataTopic,
                this.tradeRqmtTopic);
        this.tradeSummarySubPubMsgProcessor.startListening();
    }

    public void startService() throws Exception {
        Logger.getLogger(this.getClass()).info("Creating Trade Summary and RQMT Sub Pub Msg Processor");
        try{
            createTradeSummarySubPubMsgProcessor();
            createInboundDocSubPubMsgProcessor();
            createAssocDocSubPubMsgProcessor();
            createTradeRqmtConfirmSubPubMsgProcessor();
        }   catch (Exception e){
            Logger.getLogger(this.getClass()).error("start: " , e );
        }
    }

    public void stopService() {
        try{
            tradeSummarySubPubMsgProcessor.StopListening();
            inboundDocSubPubMsgProcessor.StopListening();
            assocDocSubPubMsgProcessor.StopListening();
            tradeRqmtConfirmSubPubMsgProcessor.StopListening();
        }   catch(Exception ex){
            Logger.getLogger(this.getClass()).error("stop: " + ex.getMessage());
        }
    }
}
