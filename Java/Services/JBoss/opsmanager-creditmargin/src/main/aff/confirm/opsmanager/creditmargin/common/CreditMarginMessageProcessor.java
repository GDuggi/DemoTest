package aff.confirm.opsmanager.creditmargin.common;


import org.jboss.logging.Logger;

import javax.jms.*;
import javax.naming.NamingException;
import javax.swing.text.BadLocationException;
import java.io.IOException;
import java.sql.SQLException;


/**
 * User: srajaman
 * Date: Dec 3, 2008
 * Time: 12:59:30 PM
 */
public class CreditMarginMessageProcessor {
    private static Logger log = Logger.getLogger( CreditMarginMessageProcessor.class );
    private String tibcoServer;
    private String tibcoUser;
    private String tibcoPwd;
    private String tibcoQueueName;

    private Connection connection;
    private Session session;
    private MessageConsumer  msgConsumer;

    private boolean isStopped = true;
    private String databaseName;
    private CreditMarginProcessor creditMarginProcessor;

    public CreditMarginMessageProcessor(String tibcoServer,String tibcoUser,String tibcoPwd,String tibcoQueueName,String databaseName,
                                        CreditMarginProcessor marginProcessor){
        this.tibcoServer = tibcoServer;
        this.tibcoUser = tibcoUser;
        this.tibcoPwd  = tibcoPwd;
        this.tibcoQueueName = tibcoQueueName;
        this.databaseName = databaseName;
        this.creditMarginProcessor = marginProcessor;
    }

    public void startProcessing(MessageListener listener) throws JMSException, NamingException {
        synchronized(this) {
            connection = JMSUtil.getQueueConnection(this.tibcoServer, this.tibcoUser, this.tibcoPwd);
            session = connection.createSession(false, Session.CLIENT_ACKNOWLEDGE);
            Queue queue = session.createQueue(tibcoQueueName);
            msgConsumer =session.createConsumer(queue);
            msgConsumer.setMessageListener(listener);
            connection.start();
            isStopped = false;
            log.info("Started listening the queue " + tibcoQueueName);
            Logger.getLogger(CreditMarginMessageProcessor.class).info("Started listening the queue " + tibcoQueueName);
        }
    }

    public void stopProcessing(){
        try {
            msgConsumer.close();
            session.close();
            connection.stop();
            connection.close();
            isStopped = true;
            Logger.getLogger(CreditMarginMessageProcessor.class).info("Stopped the messaging....");
        } catch (JMSException e) {
            log.error( "ERROR", e );
        }
        finally {
        }
    }

    public void processMessage(Message message) throws JMSException, NamingException, IOException, BadLocationException, SQLException {

        long tradeId=0;
        String tradingSystem = "AFF";
        int version = 0;

        TextMessage msg = (TextMessage) message;
        String tradeInfo = msg.getText();
        Logger.getLogger(CreditMarginMessageProcessor.class).info("Incoming message =" + tradeInfo);
        String[] tradeData = tradeInfo.split("\\|");
        tradingSystem = tradeData[0];
        tradeId = Long.parseLong(tradeData[1]);
        version = Integer.parseInt(tradeData[2]);
        creditMarginProcessor.processCreditMarginNotification(tradingSystem,tradeId, version,tradeInfo);
        Logger.getLogger(CreditMarginMessageProcessor.class).info("Message =" + tradeInfo + " processed successfully");
        message.acknowledge();

    }

}
