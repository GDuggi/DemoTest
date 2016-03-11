package aff.confirm.opsmanager.opssubpub;


import org.jboss.system.ServiceMBean;

/**
 * User: mthoresen
 * Date: Sep 19, 2012
 * Time: 11:18:31 AM
 */
public interface OpsJMSSubPubManagerServiceMBean extends ServiceMBean {
    String getJmsServer();
    void setJmsServer(String jmsServer);

    String getJmsUser();
    void setJmsUser(String jmsUser);

    String getJmsPwd();
    void setJmsPwd(String jmsPwd);

    String getJndiConnectionFactory();
    void setJndiConnectionFactory(String jndiConnectionFactory);

    String getProviderContextFactory();
    void setProviderContextFactory(String providerContextFactory);

    // JMS Queues
    String getTradeDataQueue();
    void setTradeDataQueue(String tradeDataQueue);

    // JMS Topic
    String getTradeDataTopic();
    void setTradeDataTopic(String tradeDataTopic);

    // JMS Topic
    String getTradeRqmtTopic();
    void setTradeRqmtTopic(String tradeRqmtTopic);

    // JMS Queues
    String getInbDocDataQueue();
    void setInbDocDataQueue(String inbDocDataQueue);

    // JMS Topic
    String getInbDocDataTopic();
    void setInbDocDataTopic(String inbDocDataTopic);

    // JMS Queues
    String getAssocDocDataQueue();
    void setAssocDocDataQueue(String assocDocDataQueue);

    // JMS Topic
    String getAssocDocDataTopic();
    void setAssocDocDataTopic(String assocDocDataTopic);

    // JMS Queues
    String getTradeRqmtConfirmDataQueue();
    void setTradeRqmtConfirmDataQueue(String tradeRqmtConfirmDataQueue);

    // JMS Topic
    String getTradeRqmtConfirmDataTopic();
    void setTradeRqmtConfirmDataTopic(String tradeRqmtConfirmDataTopic);
}
