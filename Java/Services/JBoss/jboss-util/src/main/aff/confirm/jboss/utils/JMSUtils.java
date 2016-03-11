 package aff.confirm.jboss.utils;

 import aff.confirm.common.fileMaker.XMLMaker;
 import aff.confirm.common.util.JndiUtil;
 import aff.confirm.jboss.mail.MailNotifier;
 import org.jboss.logging.Logger;
 import org.w3c.dom.Document;
 import org.w3c.dom.Node;
 import org.w3c.dom.NodeList;
 import org.xml.sax.SAXException;

 import javax.jms.JMSException;
 import javax.jms.Message;
 import javax.jms.Session;
 import javax.naming.NamingException;
 import javax.xml.parsers.DocumentBuilder;
 import javax.xml.parsers.DocumentBuilderFactory;
 import javax.xml.parsers.ParserConfigurationException;
 import java.io.ByteArrayInputStream;
 import java.io.IOException;
 import java.io.InputStream;
 import java.sql.ResultSet;
 import java.sql.ResultSetMetaData;
 import java.text.SimpleDateFormat;
 import java.util.Enumeration;
 import java.util.LinkedList;
 import java.util.Locale;

 public class JMSUtils
 {
   public static SimpleDateFormat sdfDate = new SimpleDateFormat("yyyy-MM-dd", Locale.US);
   public static SimpleDateFormat sdfDateTime = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss", Locale.US);
   public static SimpleDateFormat sdfTime = new SimpleDateFormat("HH:mm:ss", Locale.US);
   public static String propSection = "[PROPERTIES]";
   public static String bodySection = "[BODY]";

   public static void resultSetToMessage(ResultSet rs, Message msg) throws Exception, JMSException {
     ResultSetMetaData rsmd = rs.getMetaData();
     for (int i = 1; i <= rsmd.getColumnCount(); i++) {
       int type = rsmd.getColumnType(i);
       switch (type) {
       case 91:
       case 93:
         if (rs.getTimestamp(i) != null)
           msg.setStringProperty(rsmd.getColumnName(i), sdfDateTime.format(rs.getTimestamp(i)));
         else
           msg.setStringProperty(rsmd.getColumnName(i), "");
         break;
       case 92:
         if (rs.getTime(i) != null)
           msg.setStringProperty(rsmd.getColumnName(i), sdfTime.format(rs.getTime(i)));
         else
           msg.setStringProperty(rsmd.getColumnName(i), "");
         break;
       case 2:
       case 8:
         msg.setDoubleProperty(rsmd.getColumnName(i), rs.getDouble(i));
         break;
       case -6:
       case 4:
       case 5:
         msg.setIntProperty(rsmd.getColumnName(i), rs.getInt(i));
         break;
       case -1:
       case 1:
       case 12:
         msg.setStringProperty(rsmd.getColumnName(i), nullCheck(rs.getString(i)));
         break;
       case 3:
         msg.setDoubleProperty(rsmd.getColumnName(i), rs.getDouble(i));
         break;
       case 6:
         msg.setFloatProperty(rsmd.getColumnName(i), rs.getFloat(i));
         break;
       case 0:
         msg.setStringProperty(rsmd.getColumnName(i), "");
         break;
       default:
         throw new Exception("Error. Data Type not implemented: " + type + ".");
       }
     }
   }

   private static String nullCheck(String source)
   {
     if (source == null) {
       return "";
     }
     return source;
   }

   public static void copyMessage(Message source, Message dest) throws JMSException {
     Enumeration props = source.getPropertyNames();
     while (props.hasMoreElements()) {
       String propName = props.nextElement().toString();
       dest.setStringProperty(propName, source.getStringProperty(propName));
     }
   }

   public static String messageToString(Message msg) {
     StringBuffer result = new StringBuffer();
     try {
       result.append(propSection + "\n");
       Enumeration props = msg.getPropertyNames();
       while (props.hasMoreElements()) {
         String propName = props.nextElement().toString();
         result.append(propName + "=" + msg.getStringProperty(propName) + "\n");
       }
       result.append(bodySection + "\n");
       result.append(msg.toString());
     } catch (Exception e) {
       Logger.getLogger(JMSUtils.class).error(e);
     }
     return result.toString();
   }

   public static String messageToXml(Message msg) throws ParserConfigurationException, IOException, SAXException, JMSException {
     XMLMaker xml = new XMLMaker("JMS_MESSAGES");
     return messageToXml(xml, msg);
   }

   public static String messagesToXml(LinkedList messageList) throws ParserConfigurationException, IOException, SAXException, JMSException {
     XMLMaker xml = new XMLMaker("JMS_MESSAGES");
     for (int i = 0; i < messageList.size(); i++) {
       Message msg = (Message)messageList.get(i);
       messageToXml(xml, msg);
     }
     return xml.getXML();
   }

   public static String messageToXml(XMLMaker xml, Message msg) throws JMSException, IOException {
     org.jdom.Element elemRoot = xml.addElement("JMS_MESSAGE", "");
     org.jdom.Element elemProp = xml.addElement(elemRoot, "PROPERTIES", "");
     Enumeration props = msg.getPropertyNames();
     while (props.hasMoreElements()) {
       String propName = props.nextElement().toString();
       xml.addElement(elemProp, propName, msg.getStringProperty(propName));
     }
     xml.addElement(elemRoot, "BODY", msg.toString());
     return xml.getXML();
   }

   public static LinkedList parseMessagesXml(Session qs, String xmlString) throws Exception, JMSException, IOException, ParserConfigurationException, SAXException {
     ByteArrayInputStream xmlStream = new ByteArrayInputStream(xmlString.getBytes());
     return parseMessagesXml(qs, xmlStream);
   }

   public static LinkedList parseMessagesXml(Session qs, InputStream xmlStream) throws Exception, IOException, ParserConfigurationException, SAXException
   {
     LinkedList result = new LinkedList();
     DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
     DocumentBuilder builder = factory.newDocumentBuilder();
     Document xmlDocument = builder.parse(xmlStream);
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
         if (msgNode.getNodeType() == 1) {
           Message msg = parseMessageNode(qs, (org.w3c.dom.Element)msgNode);
           result.add(msg);
         }
       }
     }
     return result;
   }

   private static Message parseMessageNode(Session qs, org.w3c.dom.Element msgNode) throws Exception {
     Message result = null;

     String msgNodeName = msgNode.getNodeName();
     if (!msgNodeName.equals("JMS_MESSAGE")) {
       throw new Exception("Parsing Error. Message node is not JMS_MESSAGE, " + msgNodeName);
     }
     result = qs.createMessage();
     NodeList msgNodeData = msgNode.getChildNodes();
     for (int i = 0; i < msgNodeData.getLength(); i++) {
       if (msgNodeData.item(i).getNodeType() == 1) {
         org.w3c.dom.Element propsNode = (org.w3c.dom.Element)msgNodeData.item(i);
         String propNodeName = propsNode.getNodeName();
         if (propNodeName.equals("PROPERTIES")) {
           NodeList propsNodeList = propsNode.getChildNodes();
           for (int k = 0; k < propsNodeList.getLength(); k++) {
             if (propsNodeList.item(k).getNodeType() == 1) {
               org.w3c.dom.Element propNode = (org.w3c.dom.Element)propsNodeList.item(k);
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

   public static void stringToMessage(String data, Message msg)
     throws Exception
   {
     StringBuffer dataBuf = new StringBuffer(data);
     try {
       msg.clearProperties();
       int propStart = dataBuf.indexOf(propSection);
       if (propStart < 0)
         throw new Exception("Parsing Error. Section [PROPERTIES] not found");
       propStart += propSection.length();
       int propEnd = dataBuf.indexOf(bodySection);
       if (propEnd < 0)
         throw new Exception("Parsing Error. Section [BODY] not found");
       int curPos = propStart + 1;
       while (curPos < propEnd) {
         String prop = dataBuf.substring(curPos, dataBuf.indexOf("\n", curPos));
         if (prop == null)
           throw new Exception("Parsing Error. line end not found");
         int assignPos = prop.indexOf("=");
         if (assignPos < 0)
           throw new Exception("Parsing Error. '=' not found in line " + prop);
         String propName = prop.substring(0, assignPos);
         if (propName == null)
           throw new Exception("Parsing Error. prop name not found in line " + prop);
         String propValue = prop.substring(assignPos + 1);
         if (propValue == null) {
           throw new Exception("Parsing Error. prop value not found in line " + prop);
         }
         msg.setStringProperty(propName, propValue);
         curPos = curPos + prop.length() + 1;
       }
     } catch (Exception e) {
       throw new Exception(e.getMessage());
     }
   }

   public static void notifyEmailGroup(String groupName, String subject, String content) throws NamingException {
     ((MailNotifier) JndiUtil.lookup("MailNotifier")).sendMailToGroup(subject, content, groupName);
   }
 }

