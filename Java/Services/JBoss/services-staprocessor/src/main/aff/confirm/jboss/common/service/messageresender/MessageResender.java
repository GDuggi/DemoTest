/**
 * User: islepini
 * Date: Aug 19, 2003
 * Time: 4:17:10 PM
 * To change this template use Options | File Templates.
 */
package aff.confirm.jboss.common.service.messageresender;

import aff.confirm.common.util.JndiUtil;
import aff.confirm.jboss.common.Sender;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.queueservice.QueueService;

import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.jms.DeliveryMode;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.Queue;
import javax.naming.NamingException;
import java.sql.SQLException;
import java.util.LinkedList;
import java.util.StringTokenizer;

@Startup
@Singleton
public class MessageResender extends QueueService implements MessageResenderMBean {
    private LinkedList senders;
    private String senderQueueNames;

    public MessageResender() {
        super("affinity.cwf:service=STAProcessor");
    }

    public String getSenderQueueNames() {
        return senderQueueNames;
    }

    public void setSenderQueueNames(String senderQueueNames) {
        this.senderQueueNames = senderQueueNames;
    }


    protected void onServiceStarting() throws Exception {
        init();
    }

    protected void onServiceStoping() {
        try {
            close();
        } catch (Exception e) {
            log.error(e);
        }
    }


    public void init() throws Exception {
        createSenderQueues(senderQueueNames);
    }

    public void close() throws Exception {
        closeSenders();
    }

    private void createSenderQueues(String repQueues) throws Exception {
        senders = new LinkedList();
        StringTokenizer st = new StringTokenizer(repQueues, ",");
        while (st.hasMoreTokens()) {
            String queueName = "";
            try {
                queueName = st.nextToken();
                senders.add(createSender(queueName));
            } catch (Exception exc) {
                log.error("Failed on create sender on queue :" + queueName + exc);
            }
        }
        if (senders.size() < 1) {
            throw new Exception("No senders has been created");
        }
    }

    protected Sender createSender(String queueName) throws NamingException, SQLException, JMSException {
        Queue queue = JndiUtil.lookup("queue/" + queueName);
        log.info("creating sender on queue: " + queueName);
        return new Sender(null, getQueueSession(), queue);
    }

    private void closeSenders() {
        if (senders != null) {
            for (int i = 0; i < senders.size(); i++) {
                try {
                    ((Sender) senders.get(i)).close();
                } catch (Exception e) {
                    log.error(e);
                }
            }
            senders.clear();
        }
    }

    protected Message createMessage() throws JMSException {
        return getQueueSession().createMessage();
    }

    protected void onMessage(Message message) throws StopServiceException, LogException {
        try {
            String ticketID = df.format(message.getDoubleProperty("PRMNT_TRADE_ID"));
            log.info("Resending message for ticket " + ticketID);
            sendMessage(message);
        } catch (Exception e) {
            throw new StopServiceException(e.getMessage());
        }
    }

    protected void sendMessage(Message message) throws JMSException, StopServiceException {
        for (int i = 0; i < senders.size(); i++) {
            Message newMessage = createMessage();
            copyMessage(message, newMessage);
            ((Sender) senders.get(i)).send(newMessage, DeliveryMode.NON_PERSISTENT, 4, 0);
        }
    }

}
