package aff.confirm.jboss.jms;

import org.dom4j.DocumentException;
import org.jboss.logging.Logger;

import javax.jms.*;
import javax.naming.NamingException;
import java.sql.SQLException;
import java.util.Hashtable;


public class TAMessageProcessor implements MessageListener {
    private static Logger log = Logger.getLogger( TAMessageProcessor.class );

    private static final String _EMP_ID = "empId";
//    private static final String _TRADE_SYS = "sourceSystemCode";
    private static final String _TRADE_SYS = "tradingSystem";
    private static final String _INST_CODE = "instCode";
    private static final String _DATABASE_ = "database";
    private static final String _STTL_CODE = "tradeSttlTypeCode";
    private static final String _USER_ID = "updateUserId";
    private static final String _TYPE = "type";
    private static final String _NOTIFY_CONTRACT = "notifyContract";

    private MessageFilter filter;
    private String hornetqServer;
    private String hornetqUser;
    private String hornetqPwd;
    private String hornetqRTQueueName;
    private String publishQueueName;
    private Connection connection;
    private Session session;
    private MessageConsumer  msgConsumer;
    private Hashtable elementMapping = new Hashtable();
    OpsMessagePublisher publisher ;
    //EmpDAO empDao;
    private boolean isStopped = true;
    private MessageFilter msgFilter;
    private String databaseName;


//    public TAMessageProcessor(String tibcoServer,String tibcoUser,String tibcoPwd,String tibcoQueueName,String publishQueueName,EmpDAO empDao,MessageFilter filter,String databaseName){
    public TAMessageProcessor(String pHornetqServer,String pHornetqUser,String pHornetqPwd,String pHornetqQueueName,String publishQueueName,MessageFilter filter,String databaseName){
        Logger.getLogger(TAMessageProcessor.class).info("TAMessageProcessor Create(): Started...");
        this.hornetqServer = pHornetqServer;
        Logger.getLogger(TAMessageProcessor.class).info("HornetqServer=" + pHornetqServer);
        this.hornetqUser = pHornetqUser;
        Logger.getLogger(TAMessageProcessor.class).info("HornetqUser=" + pHornetqUser);
        this.hornetqPwd  = pHornetqPwd;
        Logger.getLogger(TAMessageProcessor.class).info("HornetqPwd=" + pHornetqPwd);
        this.hornetqRTQueueName = pHornetqQueueName;
        Logger.getLogger(TAMessageProcessor.class).info("HornetqQueueName=" + pHornetqQueueName);
        this.publishQueueName =  publishQueueName;
        Logger.getLogger(TAMessageProcessor.class).info("HornepublishQueueName=" + publishQueueName);
        //this.empDao = empDao;
        this.msgFilter = filter;
//        Logger.getLogger(TAMessageProcessor.class).info("TAMessageProcessor Create: filter=" + filter.toString());
        this.databaseName = databaseName;
        Logger.getLogger(TAMessageProcessor.class).info("DatabaseName=" + databaseName);
        Logger.getLogger(TAMessageProcessor.class).info("TAMessageProcessor Create(): Complete.");
    }

    public void startProcessing(String mappingFileName,MessageListener listener) throws JMSException, NamingException, DocumentException {
        synchronized(this) {
            Logger.getLogger(TAMessageProcessor.class).info("Startup on queue: " + hornetqRTQueueName + " initiated...");
            elementMapping = XMLUtil.loadEMSElementMapping(mappingFileName);
            publisher = new OpsMessagePublisher(this.publishQueueName);
            connection = JMSHornetQUtil.getJMSQueueConnection(this.hornetqServer, this.hornetqUser, this.hornetqPwd);
            session = connection.createSession(false,Session.CLIENT_ACKNOWLEDGE);
          //  Queue queue = session.createQueue(tibcoRTQueueName);
            Queue queue = JMSHornetQUtil.getJMSQueue(hornetqRTQueueName,hornetqServer,hornetqUser,hornetqPwd);
            msgConsumer =session.createConsumer(queue);
            msgConsumer.setMessageListener(listener);
            connection.start();
            isStopped = false;
  //          System.out.println("Started listening the queue " + tibcoRTQueueName);
            Logger.getLogger(TAMessageProcessor.class).info("Startup on queue: " + hornetqRTQueueName  + " complete.");
        }


    }
    public void stopProcessing(){

        try {
            msgConsumer.close();
            session.close();
            connection.stop();
            connection.close();
            publisher.close();
            isStopped = true;
            Logger.getLogger(TAMessageProcessor.class).info("Stopped the messaging....");
//            System.out.println("Stopped the messaging....");
        } catch (JMSException e) {
            log.error("ERROR", e);
        }
        finally {
            publisher= null;
        }
    }

    public void onMessage(Message message) {
        try {
            processMessage(message);
        } catch (JMSException e) {
            log.error("ERROR", e);
        }
    }

    public void processMessage(Message message) throws JMSException {
        synchronized(this) {
            if (!isStopped) {
                TextMessage msg = (TextMessage) message;
                Logger.getLogger(TAMessageProcessor.class).info("Message Contents: " + msg.getText());
                Hashtable returnData = XMLUtil.parseMessage(msg.getText(),elementMapping);
                // apply the filter condition
                String auditId = ((ElementMappingInfo) returnData.get("tradeAuditId")).getText();
                Logger.getLogger(TAMessageProcessor.class).info("TradeAlert notification arrived for TradeAuditId: " + auditId);
                boolean isProcess =  ( msgFilter.getTradeAuditID() <= 0)  ||
                        ( auditId != null && Long.parseLong(auditId) >= msgFilter.getTradeAuditID());
                if (isProcess) {
                   FillComputedData(returnData);
                   publisher.send(returnData);
                   Logger.getLogger(TAMessageProcessor.class).info("Trade Alert notification processed for trade audit id = " + auditId);
                }
                else {
                      Logger.getLogger(TAMessageProcessor.class).warn("The TradeAuditId: " + auditId + " is NOT published due to the filter condition");
                  }
                message.acknowledge();
            }
        }
    }

    private void FillComputedData(Hashtable messageData) {
        ElementMappingInfo emi = (ElementMappingInfo) messageData.get(_DATABASE_);
        emi.setText(databaseName);

        emi = (ElementMappingInfo) messageData.get(_TRADE_SYS);
        emi.setText("AFF");

        emi = (ElementMappingInfo) messageData.get(_TYPE);
        emi.setText("TICKET");

        emi = (ElementMappingInfo) messageData.get(_STTL_CODE);
        String sttlValue = emi.getText();

        emi = (ElementMappingInfo) messageData.get(_INST_CODE);
        emi.setText(sttlValue);

        emi = (ElementMappingInfo) messageData.get(_USER_ID);
        String userId = emi.getText();
        double empId = 0; //empDao.getEmpId(userId);
        emi = (ElementMappingInfo) messageData.get(_EMP_ID);
        emi.setText(""+empId);

        // set y or n instead of yes or no
        emi = (ElementMappingInfo) messageData.get(_NOTIFY_CONTRACT);
        emi.setText(emi.getText().substring(0, 1));
    }

    public static void main(String[] arg) throws NamingException, DocumentException, SQLException {

        EmpDAO empDao = new EmpDAO("jdbc:oracle:thin:@yonoradb7:1521:PROD","jbossusr","jbossusr");
        MessageFilter msgFilter = new MessageFilter();
//        TAMessageProcessor processor = new TAMessageProcessor("stemsdev1","sempra.ops.jboss","sempra","sempra.ops.tradeNotification.tradeData","OPS_TRACKING_TRADE_ALERT",empDao,msgFilter,"SEMPRA.DEV");
        TAMessageProcessor processor = new TAMessageProcessor("stemsdev1","sempra.ops.jboss","sempra","sempra.ops.tradeNotification.tradeData","OPS_TRACKING_TRADE_ALERT",msgFilter,"SEMPRA.DEV");
        try {
            processor.startProcessing("h:\\java\\projects\\mapping.xml",processor);
        } catch (JMSException e) {
            e.printStackTrace();
        }
    }


}
