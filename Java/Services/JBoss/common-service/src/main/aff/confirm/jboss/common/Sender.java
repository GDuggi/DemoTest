/**
 * User: islepini
 * Date: Jul 15, 2003
 * Time: 8:28:31 AM
 * To change this template use Options | File Templates.
 */
package aff.confirm.jboss.common;

import aff.confirm.jboss.common.exceptions.StopServiceException;
import org.jboss.logging.Logger;

import javax.jms.*;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;

public class Sender {
    private QueueSender sender;
    private QueueSession queueSession;
    public Sender(Connection sqlConnection, QueueSession queueSession, Queue queue) throws JMSException {
        this.sender = queueSession.createSender(queue);
        this.sqlConnection = sqlConnection;
        this.queueSession = queueSession;
    }

    private Connection sqlConnection;

    public void close() throws JMSException {
        sender.close();
    }

    public void send(Message newMessage) throws StopServiceException {
        send(newMessage,DeliveryMode.NON_PERSISTENT,4,0);
    }


    public void send(Message newMessage, int nonPersistent, int i, int qExpirationTime) throws StopServiceException {
        PreparedStatement stmnt = null;
        try {
            sender.send(newMessage, nonPersistent, i,  qExpirationTime);
/*
            String sql = "INSERT INTO JBOSSUSR.Q_"+((Queue)newMessage.getJMSDestination()).getQueueName().toUpperCase()+
            "(MESSAGE) VALUES()";
            stmnt = sqlConnection.prepareStatement(sql);
            String data = JMSUtils.messageToXml(newMessage);
            stmnt.setString(1,data);
            stmnt.execute();
 */
    //        queueSession.commit();
        } catch (Exception e) {
    /*        try {
                queueSession.rollback();

            } catch (JMSException e1) {

            } */
            Logger.getLogger(Sender.class).error(e);
            throw new StopServiceException(e.getMessage());
        }finally{
            if(stmnt != null)
                try {
                    stmnt.close();
                } catch (SQLException e) {
                    Logger.getLogger(Sender.class).error(e);
                }
        }
    }
}
