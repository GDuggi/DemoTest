package aff.confirm.jboss.jms;


import aff.confirm.common.util.JndiUtil;
import org.jboss.logging.Logger;

import javax.jms.*;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Enumeration;
import java.util.Hashtable;

/**
 * User: srajaman
 * Date: Jul 7, 2008
 * Time: 3:29:34 PM
 */
public class OpsMessagePublisher {
    private static Logger log = Logger.getLogger(OpsMessagePublisher.class );
    private static SimpleDateFormat sdfCalender = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss");
    private QueueSender sender;
    QueueSession queueSession;
    QueueConnection queueConnection;

    private String queueName;

    public OpsMessagePublisher(String queueName) throws JMSException, NamingException {
        this.queueName = queueName;
        setupQueueConnection();
      //  setupTibcoConnection();

    }

    public void close(){
        try {
            sender.close();
            queueSession.close();
            queueConnection.close();
        } catch (JMSException e) {
            log.error(e);
        }
    }
    private void setupQueueConnection() throws NamingException, JMSException {
        InitialContext ic = new InitialContext();
        ConnectionFactory cf= null;
        int i =0;
        while ( i<10) {
            try {
                cf = JndiUtil.lookup("java:/ConnectionFactory");
                break;
            }
            catch (Exception e) {
                if (i>=9) {
                    throw new NamingException(e.getMessage());
                }
                try {
                    Thread.sleep(2 *1000);
                } catch (InterruptedException e1) {
                }
            }
            ++i;
        }
        queueConnection = ((QueueConnectionFactory)cf).createQueueConnection();
        queueSession = queueConnection.createQueueSession(false,Session.CLIENT_ACKNOWLEDGE);
        Queue senderQueue = JndiUtil.lookup("queue/"+queueName);
        sender = queueSession.createSender(senderQueue);
        Logger.getLogger(OpsMessagePublisher.class).info("Trade Publisher queue: " + senderQueue.getQueueName() + " connected successfully...");
    }

    public void sendMessage(Message msg) throws JMSException {
        sender.send(msg);
    }
    public void send(Hashtable tradeNotifyInfo) throws JMSException {

       Message msg = prepareMessage(tradeNotifyInfo);

        Enumeration enum1 = msg.getPropertyNames();
        String sendingMessage = "";
        while (enum1.hasMoreElements()){
            String propName = (String) enum1.nextElement();
            sendingMessage += "Property Name = " + propName + "; Value = "+ msg.getStringProperty(propName) + "\n";
        }
        Logger.getLogger(TAMessageProcessor.class).info("Sending message = " + sendingMessage);
       sender.send(msg);

    }

    private Message prepareMessage(Hashtable tradeNotifyInfo) throws JMSException {

        Message msg =  queueSession.createMessage();

        Enumeration keyList = tradeNotifyInfo.keys();
        while (keyList.hasMoreElements()){

            String key = (String) keyList.nextElement();
            ElementMappingInfo emi = (ElementMappingInfo) tradeNotifyInfo.get(key);
            String dataType = emi.getJmsPropertyType();
            String text = emi.getText();
            String propertyName = emi.getJmsPropertyName();
            String format = emi.getDataFormat();

            if ( ElementMappingInfo._STRING_TYPE.equalsIgnoreCase(dataType)) {
                msg.setStringProperty(propertyName,text);
            }
            else if( ElementMappingInfo._DATE_TYPE.equalsIgnoreCase(dataType)) {
               msg.setStringProperty(propertyName,getDateString(text,format));
            }
            else if( ElementMappingInfo._DOUBLE_TYPE.equalsIgnoreCase(dataType)) {
                msg.setDoubleProperty(propertyName,getDouble(text));
            }

        }
        return msg;

    }

    private String getDateString(String text, String format) {

       SimpleDateFormat sdf = new SimpleDateFormat(format);
       Date dt;
       String returnDateStr = "";
        try {
            dt = sdf.parse(text);
            returnDateStr = sdfCalender.format(dt);
        } catch (ParseException e) {
            log.error( "ERROR", e );
            e.printStackTrace();
        }
        return returnDateStr;

    }

    private double getDouble(String text){
        double returnValue = 0;
        try {
            returnValue = Double.parseDouble(text);
            if (Double.isNaN(returnValue)) {
                returnValue = 0;
            }
        }
        catch (Exception e){
            returnValue = 0;
        }
        return returnValue;

    }

}
