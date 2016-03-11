package aff.confirm.tradealertlog.mdb;


/*
* @author rescaraman 
* @since 2015-10-21
* copyright Amphora Inc. 2015
*/


import org.xml.sax.SAXException;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.annotation.Resource;
import javax.ejb.*;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageListener;
import javax.jms.TextMessage;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import javax.sql.DataSource;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.Properties;
import java.util.concurrent.LinkedBlockingQueue;
import java.util.logging.Logger;

@MessageDriven( name="TradeAlertLogMDB", activationConfig = {
        @ActivationConfigProperty( propertyName = "destinationType", propertyValue = "javax.jms.Queue" ),
        @ActivationConfigProperty( propertyName = "destination", propertyValue = "queue/confirmsMgr.tradeNotification.dbLogger" ),
        @ActivationConfigProperty( propertyName = "useDLQ", propertyValue = "false" ),
        @ActivationConfigProperty( propertyName = "maxSession", propertyValue = "1" ),
        @ActivationConfigProperty(propertyName = "acknowledgeMode", propertyValue = "Auto-acknowledge") } )
public class TradeAlertLogMDB implements MessageListener {

    static String SQL_INSERT = "INSERT INTO ALERT_MSG_LOG ( ID, ALERT_TIMESTAMP, MSG_TEXT) values (NEXT VALUE for SEQ_ALERT_MSG_LOG, getdate(), ? )";

    public final static Logger log = Logger.getLogger(TradeAlertLogMDB.class.toString());

    private LinkedBlockingQueue<String> failedQueue = new LinkedBlockingQueue<String>();

    String jmsServer = System.getProperty("aff.cnf.jms.server","localhost");
    String jmsUser = System.getProperty("aff.cnf.jms.user","sempra.ops.jboss");
    String jmsPassword = System.getProperty("aff.cnf.jms.password","sempra");
    String jmsFailedQueueName = System.getProperty("aff.cnf.jms.fail.queue.name","confirmsMgr.tradeNotification.dbLogger.failed");

    @Resource
    MessageDrivenContext ctx;

    @Resource(lookup = "java:jboss/datasources/Aff.SqlSvr.DS")
    private DataSource dataSource;

    @PostConstruct
    void startUp() throws Exception {
        log.info("TradeAlertLogMDB.start");
        if( dataSource == null )
            throw new IllegalStateException( "dataSource not injected");

        log.info("TradeAlertLogMDB.start done");
    }


    @Override
    public void onMessage(Message message) {
        log.info("TradeAlertLogMDB.onMessage invoked");

        try {

            //Step 1. Transfer data from HornetQ(if any) to list (draining the HornetQ)
            TradeAlertLogUtil.readFromFailedQueue(jmsServer, jmsUser, jmsPassword, jmsFailedQueueName, failedQueue);

            if (message instanceof TextMessage) {
                TextMessage textMessage = (TextMessage) message;
                String msg = textMessage.getText();
                log.info("msg=" + msg);

                //Step 2. append incoming msg to the end of the list, to preserve the order
                appendToQueue(msg); //

                //Step 3. process the list
                processQueue();

                if (failedQueue.size()==0 ) {
                    //Step 4. we need to drain all the msgs in the HornetQ (if there is because the list is now empty)
                    TradeAlertLogUtil.emptyFailedQueue(jmsServer, jmsUser, jmsPassword, jmsFailedQueueName);

                } else {
                    //Step. 4. if the list is not empty, send it to the HornetQ again
                    TradeAlertLogUtil.sendToFailedQueue(jmsServer, jmsUser, jmsPassword, jmsFailedQueueName, failedQueue);
                }
                int msgCtr = TradeAlertLogUtil.getQueueMessageCount(jmsServer, jmsFailedQueueName);
                log.info(">>> failedQueue (in memory) size = " + failedQueue.size() + ", hornet queue["+jmsFailedQueueName+"] message count = " + msgCtr );
            }
        } catch (JMSException e) {
            log.severe(e.getMessage());
        } finally {
        }
    }



    public boolean saveToDatabase(String message) {
        log.info("Saving message to database : " + message );
        Connection conn = null;
        PreparedStatement ps = null;
        try {
            conn = dataSource.getConnection();
            ps = conn.prepareStatement(SQL_INSERT);
            ps.setString(1, message);
            ps.execute();
            log.info("Successfully saved message : " + message) ;
        } catch (SQLException e) {
            log.severe("Error saving: "+ e.getMessage() );
            return false;
        } finally {
            try {
                if (ps!=null)
                    ps.close();
            } catch (SQLException e) {
                e.printStackTrace();
            }
            try {
                if (conn!=null)
                    conn.close();
            } catch (SQLException e) {
                e.printStackTrace();
            }
        }
        return true;
    }

    private void appendToQueue(String msg) {
        failedQueue.offer(msg);
    }

    private void processQueue() {
        if (failedQueue.size()==0)
            return;

        String message = failedQueue.peek();
        if (message!=null) {
            if (saveToDatabase(message)) {
                //was able to save now..
                try {
                    failedQueue.take();
                } catch (InterruptedException e) {
                    log.warning(e.getMessage());
                }
                //recursive
                processQueue();
            }
        }
    }

}
