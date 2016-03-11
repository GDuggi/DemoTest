/**
 * User: islepini
 * Date: Jul 29, 2003
 * Time: 1:30:22 PM
 * To change this template use Options | File Templates.
 */
package aff.confirm.jboss.common.service.queueservice;

import aff.confirm.common.util.JndiUtil;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.Service;
import org.jboss.logging.Logger;

import javax.jms.*;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.util.Properties;

abstract public class QueueService extends Service {
    protected String queueName;
    private QueueConnection queueConnection;
    private QueueSession queueSession;
    private String sourceServerName;
    private QueueReceiver queueReceiver;
    QueueListener eventListener;
    private boolean isTransactional = false;

    public QueueService(String objectNameStr) {
        super(objectNameStr);
    }

    class QueueListener implements MessageListener,ExceptionListener {
        public void onMessage(Message message){
            synchronized(QueueService.this){
                String messageData = null;
                if(!QueueService.this.getStopingService()){
                    try{
                        QueueService.this.onMessage(message);
                        try {
                            message.acknowledge();
                        } catch (JMSException e1) {
                            log.error(e1);
                        }
                    } catch (StopServiceException e) {
                        log.error(e);
                        try {
                            if((getSqlConnection() != null) && (getSqlConnection().getAutoCommit() == false))
                               getSqlConnection().rollback();
                        } catch (Exception e1) {
                           log.error(e1);
                        }
                        messageData = messageToString(message);
                        notifyEmailGroupServiceStoped(messageData+"\n"+e.getMessage());
                        stop();

                    } catch (LogException e) {
                        log.error(e);
                        try {
                            if((getSqlConnection() != null) && (getSqlConnection().getAutoCommit() == false))
                               getSqlConnection().rollback();
                        } catch (Exception e1) {
                           log.error(e1);
                        }
                        messageData = messageToString(message);
                        String errorMessage = "";
                        notifyEmailGroupServiceFailedToProcess(errorMessage+messageData+"\n"+e.getMessage());
                    } finally{
                    }
                }else{

                }
            }
        }

        public void onException(JMSException e) {
            onConnectionException(e);
            notifyEmailGroupServiceStoped("Connection exception, "+e.getMessage());
            stop();
        }
    }


    protected void onMessage(Message message) throws StopServiceException,LogException {
        if(!started){
           started = true;
           try {
               Thread.sleep(20*1000);
           } catch (InterruptedException e) {
               Logger.getLogger(this.getClass()).error(e);
           }
       }
    }

    public boolean isTransactional()
    {
        return isTransactional;
    }

    public void setTransactional( boolean transactional )
    {
        isTransactional = transactional;
    }

    protected void onConnectionException(JMSException e){
    }

    public String getQueueName() {
        return queueName;
    }

    public void setQueueName(String queueName) {
        this.queueName = queueName;
    }

    public QueueConnection getQueueConnection() {
        return queueConnection;
    }

    public QueueSession getQueueSession() {
        return queueSession;
    }

    public String getSourceServerName() {
        return sourceServerName;
    }

    public void setSourceServerName(String sourceServerName) {
        this.sourceServerName = sourceServerName;
    }

    private void init() throws NamingException, JMSException {
        InitialContext ic = null;
        ConnectionFactory cf = null;
        try{
            if(sourceServerName != null){
                Properties env = new Properties();
                env.put(Context.PROVIDER_URL, sourceServerName+":1099");
                env.put(Context.URL_PKG_PREFIXES, "org.jboss.naming:org.jnp.interfaces");
                env.put(Context.INITIAL_CONTEXT_FACTORY,"org.jnp.interfaces.NamingContextFactory");
                ic = new InitialContext(env);
                cf = JndiUtil.lookup(ic,"UIL2ConnectionFactory");
            }else{
                ic = new InitialContext();
                cf = JndiUtil.lookup(ic,"java:/ConnectionFactory");
            }
            queueConnection = ((QueueConnectionFactory)cf).createQueueConnection();
            log.info(  "Creating session queue with trasaction flag : " + isTransactional);
            queueSession = queueConnection.createQueueSession(isTransactional,Session.CLIENT_ACKNOWLEDGE);
            Queue receiveQueue = JndiUtil.lookup(ic,"queue/"+queueName);
            queueReceiver = queueSession.createReceiver(receiveQueue);
            eventListener = new QueueListener();
            queueReceiver.setMessageListener(eventListener);
            queueConnection.setExceptionListener(eventListener);
        }finally{
            if(ic != null){
                ic.close();
                ic = null;
            }
        }
    }

    final protected void onInternalServiceStarting() throws Exception{
        init();
        onServiceStarting();
    }

    final protected void onInternalServiceStoping(){
        onServiceStoping();
        close();
    }

    abstract protected void onServiceStarting() throws Exception;

    abstract protected void onServiceStoping();

    protected void forceClose()
    {
        close();
    }

    protected void forceInit() throws JMSException, NamingException
    {
        init();
    }

    protected boolean isInitialized()
    {
        return queueReceiver != null;
    }

    private void close() {
        try{
            if(queueReceiver != null){
                queueReceiver.close();
                queueReceiver = null;
            }
        } catch (JMSException e) {
            log.error(e);
        }
        try {
            if(queueConnection != null){
                queueConnection.close();
                queueConnection = null;
            }
        } catch (JMSException e) {
            log.error(e);
        }

        if(eventListener != null)
            eventListener = null;

        if(queueSession != null)
            queueSession = null;
    }

    public void startProcessing() throws Exception {
        queueConnection.start();
    }





}
