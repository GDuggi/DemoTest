package aff.confirm.tradealertlog.mdb;

/*
* @author rescaraman 
* @since 2015-11-13
* copyright Amphora Inc. 2015
*/

import javax.jms.*;
import javax.management.MBeanServer;
import javax.management.ObjectName;
import javax.management.remote.JMXConnector;
import javax.naming.Context;
import javax.naming.InitialContext;
import java.lang.management.ManagementFactory;
import java.util.HashMap;
import java.util.Properties;
import java.util.concurrent.LinkedBlockingQueue;
import java.util.logging.Logger;

public class TradeAlertLogUtil {

    public final static Logger log = Logger.getLogger(TradeAlertLogUtil.class.toString());

    public static int getQueueMessageCount(String jmsServer, String queueName) {
        HashMap<String, String[]> env = new HashMap<String, String[]>();
        String[] creds = new String[2];
        creds[0] = System.getProperty("aff.cnf.server.remoting.user.name","admin");
        creds[1] = System.getProperty("aff.cnf.server.remoting.user.password","Amphora-123");
        env.put(JMXConnector.CREDENTIALS, creds);
        String urlString = "service:jmx:remoting-jmx://" + jmsServer + ":9999";
        JMXConnector jmxConnector = null;
        try {
            MBeanServer connection = ManagementFactory.getPlatformMBeanServer();
            ObjectName objectName = new ObjectName("jboss.as:subsystem=messaging,hornetq-server=default,jms-queue=" + queueName);
            Long messageCount = (Long) connection.getAttribute(objectName, "messageCount");
            return messageCount.intValue();
        } catch (Throwable e) {
            log.severe(e.getMessage());
            e.printStackTrace();
            return -1;
        } finally {
            if (jmxConnector != null) {
                try {
                    jmxConnector.close();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }
    }

    public static void readFromFailedQueue(String jmsServer, String user, String password, String queueName, LinkedBlockingQueue<String> _failedQueue) {
        int msgCtr = getQueueMessageCount(jmsServer, queueName);
        //log.info("--->There are " + msgCtr + " from this queue ["+queueName+"]");

        while (msgCtr > 0) {
            String msg = _readFromQueue(jmsServer, user, password, queueName);
            //log.info("----->msg retrieved = " + msg );
            if (msg != null)
                _failedQueue.offer(msg);

            msgCtr = getQueueMessageCount(jmsServer, queueName);
        }
        //log.info("--->Now after read there are " + msgCtr + " elements.");
    }

    public static void emptyFailedQueue(String jmsServer, String user, String password, String queueName) {
        int msgCtr = getQueueMessageCount(jmsServer, queueName);
        //log.info("Before emptyFailedQueue are " + msgCtr + " from this queue ["+queueName+"] to empty");
        while (msgCtr > 0) {
            String msg = _readFromQueue(jmsServer, user, password, queueName);
            //log.info("msg retrieved = " + msg );
            msgCtr = getQueueMessageCount(jmsServer, queueName);
        }
        //log.info("After emptyFailedQueue are " + msgCtr + " from this queue ["+queueName+"] to empty");
    }


    private static String _readFromQueue(String jmsServer, String user, String password, String queueName) {

        Properties propert = new Properties();
        propert.put(Context.INITIAL_CONTEXT_FACTORY, "org.jboss.naming.remote.client.InitialContextFactory");
        propert.setProperty(Context.PROVIDER_URL, "remote://" + jmsServer + ":4447");
        propert.setProperty(Context.SECURITY_PRINCIPAL, user);
        propert.setProperty(Context.SECURITY_CREDENTIALS, password);

        QueueConnection queueConnection = null;
        QueueSession queueSession = null;
        QueueReceiver queueReceiver = null;
        InitialContext context = null;
        try {
            context = new InitialContext(propert);
            QueueConnectionFactory factory = (QueueConnectionFactory) context.lookup("jms/RemoteConnectionFactory");
            queueConnection = factory.createQueueConnection(user, password);
            queueSession = queueConnection.createQueueSession(false, Session.AUTO_ACKNOWLEDGE);
            Queue clusterQueue = queueSession.createQueue(queueName);
            queueReceiver = queueSession.createReceiver(clusterQueue);
            queueConnection.start();
            TextMessage msg = (TextMessage) queueReceiver.receive();
            //log.info("READ from (queue:" + queueName + ") ===>" + msg.getText());
            return msg.getText();
        } catch (Throwable e) {
            log.severe(e.getMessage());
            e.printStackTrace();
        } finally {
            try {
                if (queueReceiver != null)
                    queueReceiver.close();
            } catch (JMSException e) {
                e.printStackTrace();
            }
            try {
                if (queueSession != null)
                    queueSession.close();
            } catch (JMSException e) {
                e.printStackTrace();
            }
            try {
                if (queueConnection != null)
                    queueConnection.close();
            } catch (JMSException e) {
                e.printStackTrace();
            }
        }
        return null;
    }


    public static void sendToFailedQueue(String jmsServer, String user, String password, String queueName, LinkedBlockingQueue<String> _failedQueue) {

        //log.info("BEFORE: There are "+_failedQueue.size()+" messages to send to queue");

        if (_failedQueue.size() == 0) {
            log.info("Nothing to send to queue [" + queueName + "]");
            return;
        }

        //let messages live for a max of 24 hours (1 day) default
        long TIME_TO_LIVE = 1000 * 60 * 60 * Integer.parseInt(System.getProperty("aff.cnf.jms.message.failed.queue.expiration.hours", "24"));

        Connection jmsConnection = null;
        Session ticketSession = null;
        MessageProducer ticketProducer = null;

        //log.info("Created connection to HornetQ queue : " + queueName);
        try {
            Properties property = new Properties();
            property.put(Context.INITIAL_CONTEXT_FACTORY, "org.jboss.naming.remote.client.InitialContextFactory");
            property.setProperty(Context.PROVIDER_URL, "remote://" + jmsServer + ":4447");
            property.setProperty(Context.SECURITY_PRINCIPAL, user);
            property.setProperty(Context.SECURITY_CREDENTIALS, password);

            InitialContext context = new InitialContext(property);
            QueueConnectionFactory factory = (QueueConnectionFactory) context.lookup("jms/RemoteConnectionFactory");
            jmsConnection = factory.createConnection(user, password);
            ticketSession = jmsConnection.createSession(false, Session.AUTO_ACKNOWLEDGE);


            Destination destination = (Destination) context.lookup("jms/queue/" + queueName);
            ticketProducer = ticketSession.createProducer(destination);
            jmsConnection.start();


            while (true) {
                if (_failedQueue.peek() == null)
                    break;

                try {
                    String _data = _failedQueue.take();
                    Message message = ticketSession.createTextMessage(_data);
                    message.setJMSExpiration(TIME_TO_LIVE);
                    ticketProducer.send(message);
                } catch (Throwable e) {
                    e.printStackTrace();
                }
            }


        } catch (Throwable e) {
            e.printStackTrace();
        } finally {
            try {
                if (ticketProducer != null)
                    ticketProducer.close();
                if (ticketSession != null)
                    ticketSession.close();
                if (jmsConnection != null) {
                    jmsConnection.stop();
                    jmsConnection.close();
                }
            } catch (Throwable e) {
                e.printStackTrace();
            }
            //log.info("**** There are "+ getQueueMessageCount(jmsServer,user,password,queueName)+" messages in " + queueName );
        }
    }


}
