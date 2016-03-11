package aff.confirm.opsmanager.opssubpub;

import aff.confirm.common.util.JndiUtil;
import org.jboss.logging.Logger;

import javax.jms.*;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.lang.reflect.Method;
import java.text.SimpleDateFormat;
import java.util.*;



/**
 * User: mthoresen
 * Date: Sep 20, 2012
 * Time: 8:22:33 AM
 */
public abstract class BaseSubPubMsgProcessor implements MessageListener, ExceptionListener {
    protected String jmsServer;
    protected String jmsUser;
    protected String jmsPwd;
    protected String jndiConnectionFactory;
    protected String providerContextFactory;
    protected String jmsQueue;
    protected Connection jmsConnection = null;
    protected Session jmsSession = null;
    protected Destination destination = null;
    protected MessageConsumer msgConsumer = null;
    protected Logger mylogger = Logger.getLogger(this.getClass().getSimpleName());
    // publishing
    protected JMSConnection publisher = null;
    protected Connection jmsTopicConnection = null;
    protected Session jmsTopicSession;
    protected String jmsTopic;
    protected MessageProducer producer;

    private static String _DATE_FORMAT = "MM/dd/yyyy HH:mm:ss";


    public BaseSubPubMsgProcessor(String jmsServer, String jmsUser, String jmsPwd, String jndiConnectionFactory, String providerContextFactory, String jmsQueue, String jmsTopic) {
        this.jmsServer = jmsServer;
        this.jmsUser = jmsUser;
        this.jmsPwd = jmsPwd;
        this.jndiConnectionFactory = jndiConnectionFactory;
        this.providerContextFactory = providerContextFactory;
        this.jmsQueue = jmsQueue;
        this.jmsTopic = jmsTopic;
        initProcessor();
    }

    private void initProcessor() {
        initJMS();
        InitialContext ctx;
        try {
            ctx = new InitialContext();
            initAdo(ctx);

        } catch (Exception e) {
            mylogger.error("Failed to complete context lookup(s)", e);
        }
    }

    protected abstract void initAdo(InitialContext ctx) throws Exception;

    protected void initJMS() {

        Hashtable env = new Hashtable();

        try {
            env.put(Context.INITIAL_CONTEXT_FACTORY, providerContextFactory);
            env.put(Context.PROVIDER_URL, jmsServer);
            env.put(Context.SECURITY_PRINCIPAL, jmsUser);
            env.put(Context.SECURITY_CREDENTIALS, jmsPwd);

            Context jndiContext = null;
            try {
                jndiContext = new InitialContext(env);
            } catch (NamingException e) {
                throw e;
            }

            ConnectionFactory factory = JndiUtil.lookup(jndiContext, jndiConnectionFactory);
            this.jmsConnection = factory.createConnection(this.jmsUser, this.jmsPwd);
            this.jmsConnection.setExceptionListener(this);
            this.jmsSession = this.jmsConnection.createSession(false, javax.jms.Session.AUTO_ACKNOWLEDGE);

            // Setup queue
            this.destination = this.jmsSession.createQueue(this.jmsQueue);
            this.msgConsumer = this.jmsSession.createConsumer(this.destination);
            this.msgConsumer.setMessageListener(this);

            // setup publisher
            this.jmsTopicConnection = factory.createConnection(this.jmsUser, this.jmsPwd);
            this.jmsTopicSession = this.jmsTopicConnection.createSession(false, Session.AUTO_ACKNOWLEDGE);
            Topic topic = JndiUtil.lookup(jndiContext, "jms/topic/" + this.jmsTopic);
            producer = this.jmsTopicSession.createProducer(topic);

        } catch (Exception e) {
            Logger.getLogger(BaseSubPubMsgProcessor.class).error("initJMS error using " + env.toString(), e);
        }
    }

    public void startListening() {
        // start listening
        try {
            this.jmsConnection.start();
            this.jmsTopicConnection.start();
            mylogger.info("BaseSubPubMsgProcessor: Listening to: " + this.jmsQueue);
            mylogger.info("BaseSubPubMsgProcessor: Listening to: " + this.jmsTopic);
        } catch (JMSException e) {
            mylogger.error("startListening", e );
        }
    }

    public void StopListening() {
        try {
            this.jmsConnection.stop();
            this.jmsTopicConnection.stop();
        } catch (JMSException e) {
            mylogger.error("stopListening", e );
        }
    }

    public void onMessage(Message message) {
        try {
            TextMessage txtMsg = (TextMessage) message;
            String msgTxt = null;
            msgTxt = txtMsg.getText();
            mylogger.info("Processing Message: JMS Message Received: " + msgTxt);
            long startTime = System.currentTimeMillis();
            StringTokenizer st = null;
            st = new StringTokenizer(msgTxt, "|");
            List<String> msgList = new ArrayList<String>();
            while (st.hasMoreTokens()) {
                msgList.add(st.nextToken());
            }
            processMessage(msgList);

        } catch (JMSException e) {
            mylogger.error("onMessage", e);
        }
    }

    protected abstract void processMessage(List<String> msgList);

    public void onException(JMSException e) {
        Logger.getLogger(BaseSubPubMsgProcessor.class).error("processMessage" , e );
    }

    protected void sendMessage(OpsManagerMessage msg) {
        try {
            Message mm = getMessage(msg.getData());
            mm.setStringProperty("type", msg.getMessageType());
            producer.send(mm);
            mylogger.info("Processing Message: JMS Message Sent: " + msg.getMessageType());
        } catch (JMSException e) {
            Logger.getLogger(BaseSubPubMsgProcessor.class).error("sendMessage()" , e );
        }
    }

    protected void sendMessage(MessageProducer msgProducer, OpsManagerMessage msg) {
        try {
            Message mm = getMessage(msg.getData());
            mm.setStringProperty("type", msg.getMessageType());
            msgProducer.send(mm);
            mylogger.info("Processing Message: JMS Message Sent: " + msg.getMessageType());
        } catch (JMSException e) {
            Logger.getLogger(BaseSubPubMsgProcessor.class).error("sendMessage(,): " , e );
        }
    }

    private synchronized MapMessage getMessage(Object obj) {
        MapMessage msg = null;
        try {
            msg = this.jmsTopicSession.createMapMessage();
            Method[] methods = obj.getClass().getMethods();

            for (int i = 0; i < methods.length; ++i) {
                Method method = methods[i];
                if (method.getName().startsWith("get")) {
                    String methodName = method.getName().substring(3).toLowerCase();

                    // remove _ from the property

                    if ("_".equals(methodName.substring(0, 1))) {
                        methodName = methodName.substring(1);
                    }

                    Object methodValue = method.invoke(obj);
                    String msgValue = "";
                    if (methodValue != null) {
                        if (methodValue instanceof java.util.Date) {
                            msgValue = getDateFormat((java.util.Date) methodValue);
                        } else {
                            msgValue = methodValue.toString();
                        }
                    }
                    msg.setStringProperty(methodName, msgValue);
                }
            }

        } catch (Exception e) {
            mylogger.error("Error occurred in getMessage: " , e );
        }
        return msg;
    }

    private String getDateFormat(Date dt) {
        String returnValue = "";
        SimpleDateFormat sdf = new SimpleDateFormat(_DATE_FORMAT);
        returnValue = sdf.format(dt);
        return returnValue;
    }

    public static void main(String[] args) {
        String jmsServer = "tcp://sttibcoprodcn1:7222,tcp://sttibcoprodcn2:7224";
        String jmsUser = "sempra.ops.gs.service";
        String jmsPwd = "sempra";
        String jndiConnectionFactory = "HaConnectionFactory";
        String providerContextFactory = "com.tibco.tibjms.naming.TibjmsInitialContextFactory";
        String jmsQueue = "sempra.ops.opsTracking.activityAlert";
        String jmsTopic = "sempra.ops.opsTracking.summary.update";
        String jmsRqmtToic = "sempra.ops.opsTracking.rqmt.update";

        // SET STATIC VARIABLES FOR CLASS
        JMSConnection.setConnectionInfo(jmsServer, jmsUser, jmsPwd, jndiConnectionFactory, jmsServer);

        TradeSummarySubPubMsgProcessor subPubprocessor = new TradeSummarySubPubMsgProcessor(jmsServer, jmsUser, jmsPwd, jndiConnectionFactory, providerContextFactory, jmsQueue, jmsTopic, jmsRqmtToic);
        subPubprocessor.startListening();
        boolean processing = true;
        while (processing) {

        }
        subPubprocessor.StopListening();
    }
}
