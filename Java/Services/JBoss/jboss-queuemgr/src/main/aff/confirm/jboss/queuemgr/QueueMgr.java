/*
 * User: islepini
 * Date: Sep 27, 2002
 * Time: 10:17:44 AM
 * To change template for new interface use 
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.jboss.queuemgr;

import aff.confirm.common.fileMaker.XMLMaker;
import aff.confirm.common.util.JndiUtil;
import aff.confirm.jboss.common.service.BasicMBeanSupport;
import aff.confirm.jboss.common.util.DbInfoWrapper;
import aff.confirm.jboss.dbinfo.DBInfo;
import org.jboss.logging.Logger;
import org.jdom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.jms.*;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import java.io.*;
import java.sql.*;
import java.text.SimpleDateFormat;
import java.util.Enumeration;
import java.util.LinkedList;
import java.util.Locale;
import java.util.StringTokenizer;

@Startup
@Singleton
public class QueueMgr extends BasicMBeanSupport implements QueueMgrMBean {
    private static Logger log = Logger.getLogger(QueueMgr.class );

    private int qExpirationTime = 24 * 60 * 60 * 1000;
    SimpleDateFormat sdfDate = new SimpleDateFormat("yyyy-MM-dd", Locale.US);
    SimpleDateFormat sdfDateTime = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss", Locale.US);
    SimpleDateFormat sdfTime = new SimpleDateFormat("HH:mm:ss", Locale.US);
    String propSection = "[PROPERTIES]";
    String bodySection = "[BODY]";

    public QueueMgr() {
        super("affinity.utils:service=QueueMgr", null );
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    private QueueSession getQueueSession(InitialContext iniContext, QueueConnection qc) throws NamingException, JMSException {
        QueueConnectionFactory connectionFactory = JndiUtil.lookup(iniContext, "java:/ConnectionFactory");
        qc = connectionFactory.createQueueConnection();
        QueueSession qs = qc.createQueueSession(false, Session.CLIENT_ACKNOWLEDGE);
        qc.start();
        return qs;
    }

    private TopicSession getTopicSession(InitialContext iniContext, TopicConnection tc) throws NamingException, JMSException {
        TopicSession ts = null;
        TopicConnectionFactory connectionFactory = null;
        connectionFactory = JndiUtil.lookup(iniContext, "java:/ConnectionFactory");
        tc = connectionFactory.createTopicConnection();
        ts = tc.createTopicSession(false, Session.CLIENT_ACKNOWLEDGE);
        tc.start();
        return ts;
    }

    private QueueBrowser getBrowser(QueueConnection qc, String queueName, String filter) throws NamingException, JMSException {
        InitialContext iniContext = new InitialContext();
        QueueSession qs = getQueueSession(iniContext, qc);

        Queue queue = JndiUtil.lookup(iniContext, "queue/" + queueName);
        if (filter != null)
            filter = filter.trim();
        if ((filter == null) || (filter.length() == 0))
            return qs.createBrowser(queue);
        else
            return qs.createBrowser(queue, filter);
    }


    private QueueReceiver getReceiver(QueueConnection qc, String queueName, String filter) throws NamingException, JMSException {
        InitialContext iniContext = new InitialContext();

        QueueSession qs = getQueueSession(iniContext, qc);
        Queue queue = (Queue) iniContext.lookup("queue/" + queueName);
        if (filter != null)
            filter = filter.trim();
        if ((filter == null) || (filter.length() == 0))
            return qs.createReceiver(queue);
        else
            return qs.createReceiver(queue, filter);
    }

    public Logger getLog() {
        return log;
    }

    public String removeMessagesFromQueueUsingFilter(String queueName, String filterExpr) {
        String result = null;
        String filter = filterExpr;

        InitialContext ic = null;
        ConnectionFactory cf = null;
        QueueReceiver qr = null;
        QueueConnection qc = null;
        QueueSession qs;
        Queue queue;
//        QueueBrowser browser = null;
        int removedCount = 0;

        try {
            cf = JndiUtil.lookup("java:/ConnectionFactory");
            qc = ((QueueConnectionFactory) cf).createQueueConnection();
            qs = qc.createQueueSession(false, Session.AUTO_ACKNOWLEDGE);
            queue = JndiUtil.lookup("queue/" + queueName);
            qc.start();

            if (filter != null)
                filter = filter.trim();
            if ((filter == null) || (filter.length() == 0))
                qr = qs.createReceiver(queue);
            else
                qr = qs.createReceiver(queue, filter);

            Message msg = qr.receive(1000);
            while (msg != null) {
                removedCount++;
                msg = qr.receive(1000);
            }

            if (removedCount == 0)
                result = "no message found for filter " + filterExpr;
            else
                result = "removed " + removedCount + " for filter " + filterExpr;
        } catch (Exception e) {
            getLog().error(e);
            result = e.getMessage();
        } finally {
            if (qr != null)
                try {
                    qr.close();
                } catch (JMSException e) {
                    getLog().error(e);
                }
            if (qc != null)
                try {
                    qc.close();
                } catch (JMSException e) {
                    getLog().error(e);
                }
            if (ic != null) {
                try {
                    ic.close();
                } catch (NamingException e) {
                    log.error( "ERROR", e );
                }
                ic = null;
            }
        }
        return result;
    }

/*    public String removeMessagesFromQueueUsingFilter(String queueName, String filterExpr) {
        String result = null;
        QueueReceiver qr = null;
        QueueConnection qc = null;
        int removedCount = 0;
        try {
            qr = getReceiver(qc,queueName,filterExpr);
            Message msg = qr.receive(1000);
            while(msg != null){
                removedCount++;
                msg = qr.receive(1000);
            }
            if(removedCount == 0)
                result = "no message found for filter "+filterExpr;
            else
                result = "removed "+removedCount+" for filter "+filterExpr;
        } catch (Exception e) {
           getLog().error(e);
           result = e.getMessage();
        }finally{
            if (qr != null)
                try {
                    qr.close();
                } catch (JMSException e) {
                    getLog().error(e);
                }
            if (qc != null)
                try {
                    qc.close();
                } catch (JMSException e) {
                    getLog().error(e);
                }
        }
        return result;
    }*/

    public String publishIntoFileFromTradeAuditIDToTradeAuditID(String fileName, int startTradeAuditID, int endTradeAuditID) throws Exception, SQLException, JMSException {
        String result = "";
        QueueConnection qc = null;
        InitialContext ic = new InitialContext();
        try {
            File file = new File(fileName);
            if (!file.exists())
                file.createNewFile();

            QueueSession qs = getQueueSession(ic, qc);
            // DBInfo dbinfo = (DBInfo)ic.lookup("DBInfo");
            DbInfoWrapper dbinfo = new DbInfoWrapper("DBInfo");
            java.sql.Connection connection = DriverManager.getConnection(dbinfo.getDBUrl(), dbinfo.getDBUserName(), dbinfo.getDBPassword());
            LinkedList messageList = loadMessages(connection, dbinfo.getDatabaseName(), qs, startTradeAuditID, endTradeAuditID);
            String data = messagesToXml(messageList);
            saveFileData(fileName, data.getBytes());
            result = "Published " + messageList.size() + " messages into a file " + fileName;
        } catch (Exception e) {
            getLog().error(e);
            result = "Failed to publish messages to the file " + fileName + ", error " + e.getMessage();
        } finally {
            if (qc != null) {
                qc.close();
                qc = null;
            }
        }
        return result;
    }

    public String publishFromFileIntoQueue(String fileName, String queueName) {
        QueueConnection qc = null;
        String result = "";
        try {
            File file = new File(fileName);
            if (!file.exists())
                throw new Exception("File " + fileName + " not found");
            QueueSender qsend = null;
            InitialContext ic = new InitialContext();
            QueueSession qs = getQueueSession(ic, qc);
            Queue queue = JndiUtil.lookup(ic, "queue/" + queueName);
            qsend = qs.createSender(queue);
            LinkedList messageList = parseMessagesXml(qs, new FileInputStream(fileName));
            if (messageList == null)
                throw new Exception("No messages found in " + fileName);
            for (int i = 0; i < messageList.size(); i++) {
                Message message = (Message) messageList.get(i);
                qsend.send(message, DeliveryMode.PERSISTENT, 7, qExpirationTime);
            }
            result = "Published " + messageList.size() + " messages into a queue " + queueName;
        } catch (Exception e) {
            getLog().error(e);
            result = "Failed to publish message from file " + fileName + ", error " + e.getMessage();
        } finally {
            if (qc != null) {
                try {
                    qc.close();
                } catch (JMSException e) {
                    log.error(e);
                }
                qc = null;
            }
        }
        return result;
    }

    public String publishFromFileIntoTopic(String fileName, String topicName) {
        TopicConnection tc = null;
        String result = "";
        try {
            File file = new File(fileName);
            if (!file.exists())
                throw new Exception("File " + fileName + " not found");

            TopicPublisher tp = null;
            InitialContext ic = new InitialContext();
            TopicSession ts = getTopicSession(ic, tc);
            Topic topic = JndiUtil.lookup("topic/" + topicName);
            tp = ts.createPublisher(topic);
            LinkedList messageList = parseMessagesXml(ts, new FileInputStream(fileName));
            if (messageList == null)
                throw new Exception("No messages found in " + fileName);

            for (int i = 0; i < messageList.size(); i++) {
                Message message = (Message) messageList.get(i);
                tp.publish(message, DeliveryMode.PERSISTENT, 7, qExpirationTime);
            }
            result = "Published " + messageList.size() + "messages into a topic " + topicName;
        } catch (Exception e) {
            getLog().error(e);
            result = "Failed to publish message from file " + fileName + ", error " + e.getMessage();
        } finally {
            if (tc != null) {
                try {
                    tc.close();
                } catch (JMSException e) {
                    log.error(e);
                }
                tc = null;
            }
        }
        return result;
    }

    void saveFileData(String fileName, byte[] filedata) throws IOException {
        FileOutputStream fos = new FileOutputStream(fileName);
        fos.write(filedata);
        fos.close();
        fos = null;
    }

    public String publishIntoQueueFromTradeAuditIDToTradeAuditID(String queueName, int startTradeAuditID, int endTradeAuditID) throws Exception, NamingException, SQLException {
        String result = "";
        QueueSender qsend = null;
        QueueConnection qc = null;
        try {
            InitialContext ic = new InitialContext();
            QueueSession qs = getQueueSession(ic, qc);
            Queue queue = JndiUtil.lookup("queue/" + queueName);
            qsend = qs.createSender(queue);
            DBInfo dbinfo = JndiUtil.lookup("DBInfo");
            java.sql.Connection connection = DriverManager.getConnection(dbinfo.getDBUrl(), dbinfo.getDBUserName(), dbinfo.getDBPassword());
            LinkedList messageList = loadMessages(connection, dbinfo.getDatabaseName(), qs, startTradeAuditID, endTradeAuditID);
            for (int i = 0; i < messageList.size(); i++) {
                Message message = (Message) messageList.get(i);
                qsend.send(message, DeliveryMode.PERSISTENT, 7, qExpirationTime);
            }
            result = "Published " + messageList.size() + " messages";
        } catch (Exception e) {
            getLog().error(e);
            result = "Failed to publish messages to the queue " + queueName + ", error " + e.getMessage();
        } finally {
            if (qc != null) {
                qc.close();
                qc = null;
            }
        }
        return result;
    }

    public String publishIntoQueueTradeAuditIDCommaList(String queueName, String idList) throws Exception, NamingException, SQLException {
        if ((idList == null) || (idList.length() == 0))
            return "Error. Empty idList";
        String result = "";
        QueueSender qsend = null;
        QueueConnection qc = null;
        try {
            InitialContext ic = new InitialContext();
            QueueSession qs = getQueueSession(ic, qc);
            Queue queue = JndiUtil.lookup("queue/" + queueName);
            qsend = qs.createSender(queue);
            DBInfo dbinfo = JndiUtil.lookup("DBInfo");
            java.sql.Connection connection = DriverManager.getConnection(dbinfo.getDBUrl(), dbinfo.getDBUserName(), dbinfo.getDBPassword());
            StringTokenizer st = new StringTokenizer(idList, ",");
            int messageCounter = 0;
            while (st.hasMoreElements()) {
                String startID = (String) st.nextElement();
                String sql = "SELECT * FROM INFINITY_MGR.V_REALTIME_TRADE_AUDIT WHERE trade_audit_id = " + startID;
                LinkedList messageList = loadMessages(connection, dbinfo.getDatabaseName(), qs, sql);
                if (messageList.size() == 0)
                    result = result + ",Not found:" + startID + "\n";
                else if (messageList.size() == 1) {
                    Message message = (Message) messageList.get(0);
                    qsend.send(message, DeliveryMode.PERSISTENT, 7, qExpirationTime);
                    messageCounter++;
                } else
                    result = result + ",too many rows for:" + startID + ", not published\n";
            }
            result = "Published " + messageCounter + " messages," + result;
        } catch (Exception e) {
            getLog().error(e);
            result = "Failed to publish messages to the queue " + queueName + ", error " + e.getMessage();
        } finally {
            if (qc != null) {
                qc.close();
                qc = null;
            }
        }
        return result;
    }

    public String publishIntoQueueFromTradeAuditWhere(String queueName, String sqlExpr) throws Exception, NamingException, SQLException {
        String result = "";
        QueueSender qsend = null;
        QueueConnection qc = null;
        try {
            InitialContext ic = new InitialContext();
            QueueSession qs = getQueueSession(ic, qc);
            Queue queue = JndiUtil.lookup("queue/" + queueName);
            qsend = qs.createSender(queue);
            DbInfoWrapper dbinfo = new DbInfoWrapper("DBInfo");
            java.sql.Connection connection = DriverManager.getConnection(dbinfo.getDBUrl(), dbinfo.getDBUserName(), dbinfo.getDBPassword());
            String sql = "SELECT * FROM INFINITY_MGR.V_REALTIME_TRADE_AUDIT WHERE " + sqlExpr;
            log.info(sql);
            LinkedList messageList = loadMessages(connection, dbinfo.getDatabaseName(), qs, sql);
            for (int i = 0; i < messageList.size(); i++) {
                Message message = (Message) messageList.get(i);
                qsend.send(message, DeliveryMode.PERSISTENT, 7, qExpirationTime);
            }
            result = "Published " + messageList.size() + " messages";
        } catch (Exception e) {
            getLog().error(e);
            result = "Failed to publish messages to the queue " + queueName + ", error " + e.getMessage();
        } finally {
            if (qc != null) {
                qc.close();
                qc = null;
            }
        }
        return result;
    }

    public String publishIntoFileFromTradeAuditWhere(String fileName, String sqlExpr) throws Exception, SQLException, JMSException {
        String result = "";
        QueueConnection qc = null;
        InitialContext ic = new InitialContext();
        try {
            File file = new File(fileName);
            if (!file.exists())
                file.createNewFile();
            QueueSession qs = getQueueSession(ic, qc);
            //DBInfo dbinfo = (DBInfo)ic.lookup("DBInfo");
            DbInfoWrapper dbinfo = new DbInfoWrapper("DBInfo");
            java.sql.Connection connection = DriverManager.getConnection(dbinfo.getDBUrl(), dbinfo.getDBUserName(), dbinfo.getDBPassword());
            String sql = "SELECT * FROM INFINITY_MGR.V_REALTIME_TRADE_AUDIT WHERE " + sqlExpr;
            LinkedList messageList = loadMessages(connection, dbinfo.getDatabaseName(), qs, sql);
            String data = messagesToXml(messageList);
            saveFileData(fileName, data.getBytes());
            result = "Published " + messageList.size() + " messages";
        } catch (Exception e) {
            getLog().error(e);
            result = "Failed to publish messages to the file " + fileName + ", error " + e.getMessage();
        } finally {
            if (qc != null) {
                qc.close();
                qc = null;
            }
        }
        return result;
    }


    private LinkedList loadMessages(java.sql.Connection connection, String databaseName, QueueSession qs, int startTradeAuditID, int endTradeAuditID) throws Exception, NamingException {
        String sql = "SELECT * FROM INFINITY_MGR.V_REALTIME_TRADE_AUDIT WHERE trade_audit_id >= " + startTradeAuditID + " and trade_audit_id <= " + endTradeAuditID;
        return loadMessages(connection, databaseName, qs, sql);
    }


    private LinkedList loadMessages(java.sql.Connection connection, String databaseName, QueueSession qs, String sqlExpr) throws Exception, NamingException {
        LinkedList result = new LinkedList();
        Statement stmnt = null;
        ResultSet rs = null;
        try {
            stmnt = connection.createStatement();
            rs = stmnt.executeQuery(sqlExpr);
            while (rs.next()) {
                result.add(createMessage(qs, rs, databaseName));
            }
        } finally {
            if (rs != null) {
                rs.close();
                rs = null;
            }
            if (stmnt != null) {
                stmnt.close();
                stmnt = null;
            }
        }

        return result;

    }


    private Message createMessage(QueueSession qs, ResultSet rs, String databaseName) throws Exception {
        Message msg = qs.createMessage();
        msg.setStringProperty("TYPE", "TICKET");
        msg.setStringProperty("DATABASE", databaseName);
        resultSetToMessage(rs, msg);
        return msg;
    }

    public String writeQueueMessagesIntoFileUsingFilter(String queueName, String fileName, String filterExpr) {
        QueueConnection qc = null;
        QueueBrowser qb = null;

        try {
            File file = new File(fileName);
            if (!file.exists())
                file.createNewFile();
            qb = getBrowser(qc, queueName, filterExpr);
            LinkedList messageList = new LinkedList();
            Enumeration en = qb.getEnumeration();
            while (en.hasMoreElements()) {
                messageList.add(en.nextElement());
            }

            String data = messagesToXml(messageList);
            if (fileName.length() > 0)
                saveFileData(fileName, data.getBytes());

            if (messageList.size() == 0)
                return "No Messages found in the queue " + queueName + " filtered by " + filterExpr;
            else
                return data;

        } catch (Exception e) {
            getLog().error(e);
            return e.getMessage();
        } finally {
            if (qb != null)
                try {
                    qb.close();
                } catch (JMSException e) {
                    getLog().error(e);
                }
            if (qc != null)
                try {
                    qc.close();
                } catch (JMSException e) {
                    getLog().error(e);
                }
        }

    }

    private void resultSetToMessage(ResultSet rs, Message msg) throws Exception, JMSException {
        ResultSetMetaData rsmd = rs.getMetaData();
        for (int i = 1; i <= rsmd.getColumnCount(); i++) {
            int type = rsmd.getColumnType(i);
            switch (type) {
                case java.sql.Types.TIMESTAMP:
                case java.sql.Types.DATE:
                    if (rs.getTimestamp(i) != null)
                        msg.setStringProperty(rsmd.getColumnName(i), sdfDateTime.format(rs.getTimestamp(i)));
                    else
                        msg.setStringProperty(rsmd.getColumnName(i), "");
                    break;
                case java.sql.Types.TIME:
                    if (rs.getTime(i) != null)
                        msg.setStringProperty(rsmd.getColumnName(i), sdfTime.format(rs.getTime((i))));
                    else
                        msg.setStringProperty(rsmd.getColumnName(i), "");
                    break;
                case java.sql.Types.DOUBLE:
                case java.sql.Types.NUMERIC:
                    msg.setDoubleProperty(rsmd.getColumnName(i), rs.getDouble(i));
                    break;
                case java.sql.Types.INTEGER:
                case java.sql.Types.SMALLINT:
                case java.sql.Types.TINYINT:
                    msg.setIntProperty(rsmd.getColumnName(i), rs.getInt(i));
                    break;
                case java.sql.Types.CHAR:
                case java.sql.Types.LONGVARCHAR:
                case java.sql.Types.VARCHAR:
                    msg.setStringProperty(rsmd.getColumnName(i), nullCheck(rs.getString(i)));
                    break;
                case java.sql.Types.DECIMAL:
                    msg.setDoubleProperty(rsmd.getColumnName(i), rs.getDouble(i));
                    break;
                case java.sql.Types.FLOAT:
                    msg.setFloatProperty(rsmd.getColumnName(i), rs.getFloat(i));
                    break;
                case java.sql.Types.NULL:
                    msg.setStringProperty(rsmd.getColumnName(i), "");
                    break;
                default:
                    throw new Exception("Error. Data Type not implemented: " + type + ".");

            }
        }
    }

    static private String nullCheck(String source) {
        if (source == null)
            return "";
        else
            return source;
    }

    private String messagesToXml(LinkedList messageList) throws ParserConfigurationException, IOException, SAXException, JMSException {
        XMLMaker xml = new XMLMaker("JMS_MESSAGES");
        for (int i = 0; i < messageList.size(); i++) {
            Message msg = (Message) messageList.get(i);
            messageToXml(xml, msg);
        }
        return xml.getXML();
    }

    private String messageToXml(XMLMaker xml, Message msg) throws JMSException, IOException {
        Element elemRoot = xml.addElement("JMS_MESSAGE", "");
        Element elemProp = xml.addElement(elemRoot, "PROPERTIES", "");
        Enumeration props = msg.getPropertyNames();
        while (props.hasMoreElements()) {
            String propName = props.nextElement().toString();
            xml.addElement(elemProp, propName, msg.getStringProperty(propName));
        }
        xml.addElement(elemRoot, "BODY", msg.toString());
        return xml.getXML();
    }

    private LinkedList parseMessagesXml(Session qs, InputStream xmlStream) throws Exception, IOException, ParserConfigurationException, SAXException {
        LinkedList result = new LinkedList();
        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
        DocumentBuilder builder = factory.newDocumentBuilder();
        org.w3c.dom.Document xmlDocument = builder.parse(xmlStream);
        NodeList nodeList = xmlDocument.getChildNodes();
        if (nodeList == null)
            throw new Exception("Error parsing xml file");
        for (int i = 0; i < nodeList.getLength(); i++) {
            Node rootNode = nodeList.item(i);
            String rootName = rootNode.getNodeName();
            if (!rootName.equals("JMS_MESSAGES")) {
                throw new Exception("Parsing Error. Root node is not JMS_MESSAGES, " + rootName);
            }
            NodeList msgNodeList = rootNode.getChildNodes();
            for (int k = 0; k < msgNodeList.getLength(); k++) {
                Node msgNode = msgNodeList.item(k);
                if (msgNode.getNodeType() == Node.ELEMENT_NODE) {
                    Message msg = parseMessageNode(qs, (org.w3c.dom.Element) msgNode);
                    result.add(msg);
                }
            }
        }
        return result;
    }

    private Message parseMessageNode(Session qs, org.w3c.dom.Element msgNode) throws Exception {
        Message result = null;

        String msgNodeName = msgNode.getNodeName();
        if (!msgNodeName.equals("JMS_MESSAGE")) {
            throw new Exception("Parsing Error. Message node is not JMS_MESSAGE, " + msgNodeName);
        }
        result = qs.createMessage();
        NodeList msgNodeData = msgNode.getChildNodes();
        for (int i = 0; i < msgNodeData.getLength(); i++) {
            if (msgNodeData.item(i).getNodeType() == Node.ELEMENT_NODE) {
                org.w3c.dom.Element propsNode = (org.w3c.dom.Element) msgNodeData.item(i);
                String propNodeName = propsNode.getNodeName();
                if (propNodeName.equals("PROPERTIES")) {
                    NodeList propsNodeList = propsNode.getChildNodes();
                    for (int k = 0; k < propsNodeList.getLength(); k++) {
                        if (propsNodeList.item(k).getNodeType() == Node.ELEMENT_NODE) {
                            org.w3c.dom.Element propNode = (org.w3c.dom.Element) propsNodeList.item(k);
                            String value = "";
                            if (propNode.getFirstChild() != null)
                                value = propNode.getFirstChild().getNodeValue();
                            result.setStringProperty(propNode.getNodeName(), value);
                        }
                    }
                }
            }
        }
        return result;
    }

}
