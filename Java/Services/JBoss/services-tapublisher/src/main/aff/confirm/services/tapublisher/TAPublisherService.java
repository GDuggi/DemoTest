package aff.confirm.services.tapublisher;

import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.util.DbInfoWrapper;
import org.jboss.logging.Logger;
import org.dom4j.DocumentException;
import aff.confirm.jboss.jms.TAMessageProcessor;
import aff.confirm.jboss.jms.MessageFilter;
import aff.confirm.jboss.jms.NotifyUtil;
//import aff.confirm.jboss.jms.EmpDAO;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.naming.NamingException;
import javax.jms.JMSException;
import javax.jms.MessageListener;
import javax.jms.Message;
import javax.jms.TextMessage;
import java.sql.SQLException;

/**
 * User: srajaman
 * Date: Jul 10, 2008
 * Time: 5:17:39 PM
 */
@Startup
@Singleton
public class TAPublisherService extends aff.confirm.jboss.common.service.Service implements TAPublisherServiceMBean,MessageListener  {
    private static final String _PUBLISHER_MAPPING_XML =   System.getProperty("jboss.server.config.dir") + "/affinity/Publisher_Mapping.xml";

    private String jmsServer;
    private String jmsUser;
    private String jmsPwd;
    private String tradeQueueName;
    private String jbossOpsQueue;
    private String smtpHost;
    private String smtpPort;
    private TAMessageProcessor processor ;
    private String affinityDisplayName;
    private MessageFilter msgFilter  = new MessageFilter();
    private String notifyAddr;

    public TAPublisherService() {
        super("affinity.confirm:service=TAPublisher");
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

    public void setJmsServer(String serverName) {
        this.jmsServer = serverName;
    }

    public String getUser() {
        return jmsUser;
    }

    public void setUser(String userId) {
        jmsUser = userId;
    }

    public String getPwd() {
        return jmsPwd;
    }

    public void setPwd(String pwd) {
           jmsPwd = pwd;
    }

    public String getReceiverQueue() {
        return tradeQueueName;
    }

    public void setReceiverQueue(String receiverQueue) {
        tradeQueueName = receiverQueue;
    }

    public String getSenderQueue() {
        return jbossOpsQueue;
    }

    public void setSenderQueue(String senderQueue) {
        jbossOpsQueue = senderQueue;
    }

    public void setSmtpHost(String host) {
        smtpHost = host;
    }

    public String getSmtpHost() {
        return smtpHost;
    }

    public void setSmtpPort(String port) {
        smtpPort = port;
    }

    public String getSmtpPort() {
        return smtpPort;
    }

    public void setNextAuditId(long auditId) {
        msgFilter.setTradeAuditID(auditId);
    }

    public long getNextAuditId() {
        return msgFilter.getTradeAuditID();
    }

    public String getNotifyAddr() {
        return notifyAddr;
    }

    public void setNotifyAddr(String addr) {
        this.notifyAddr = addr;
    }

    private void init() throws StopServiceException {
        //EmpDAO empDao = null;
        try {
            log.info("TA Publisher init() is starting....");
            NotifyUtil.notifyAddr = this.notifyAddr;
            NotifyUtil.smtpHost = this.smtpHost;
            NotifyUtil.smtpPort = this.smtpPort;
//            empDao = getEmpDAO();
//            processor  = new TAMessageProcessor(jmsServer, jmsUser, jmsPwd, tradeQueueName,jbossOpsQueue,empDao,msgFilter,affinityDisplayName);
            affinityDisplayName = getDatabaseName();
            log.info("affinityDisplayName: "+affinityDisplayName);
            processor  = new TAMessageProcessor(jmsServer, jmsUser, jmsPwd, tradeQueueName,jbossOpsQueue,msgFilter,affinityDisplayName);
            processor.startProcessing(_PUBLISHER_MAPPING_XML, this);
            NotifyUtil.sendMail("TAPublisher Startup", "The TAPublisher has been started successfully.");
            log.info("TAPublisher init() is complete.");
        } catch (NamingException e) {
            log.error("NamingException during startup :" , e );
            NotifyUtil.sendMail("TAPublisher Startup Error ","The TAPublisher has NOT been started : " + e.getMessage());
            throw new StopServiceException(e.getMessage());
        } /*catch (SQLException e) {
             log.error("Error during startup :" , e );
            NotifyUtil.sendMail("TA Processor Startup Error","The TA Processor has NOT been started : " + e.getMessage());
            throw new StopServiceException(e.getMessage());
        }*/ catch (JMSException e) {
             log.error("JMSException during startup :" , e );
            NotifyUtil.sendMail("TAPublisher Startup Error","The TAPublisher has NOT been started : " + e.getMessage());
            throw new StopServiceException(e.getMessage());
        } catch (DocumentException e) {
            log.error("DocumentException during startup :" , e );
            NotifyUtil.sendMail("TAPublisher Startup Error","The TAPublisher has NOT been started : " + e.getMessage());
            throw new StopServiceException(e.getMessage());
        }

    }

    private String getDatabaseName() {
        String dbName = "";
        DbInfoWrapper dbinfo = new DbInfoWrapper(getDbInfoName());
        dbName = dbinfo.getDatabaseName();
        return dbName;
    }

    public void startProcessing() throws Exception {
    }

    protected void onInternalServiceStarting() throws Exception {
        init();
    }

    protected void onInternalServiceStoping() {
        log.info("Stopping is called..");
        try {
            processor.stopProcessing();
        }
        catch (Exception e){
            log.error(e.getMessage());
        }
        finally {
            processor = null;
        }
    }

    public void onMessage(Message message) {
        try {
            processor.processMessage(message);
        } catch (JMSException e) {
            TextMessage msg = (TextMessage) message;
            String body = null;
            try {
                body = " Error processing the message : " + "\n" +
                            msg.getText() + "\n" + "\n" + " Error " + e.getMessage();
            } catch (JMSException e1) {
            }
            NotifyUtil.sendMail("TAPublisher Message processing error: Service Stopped",body);
            log.error(e.getMessage());
            onInternalServiceStoping();
        }
    }
}
