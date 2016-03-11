package aff.confirm.opsmanager.opssubpub;

import aff.confirm.common.util.JndiUtil;

import javax.jms.*;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Hashtable;



public class JMSConnection {
	private static org.jboss.logging.Logger log = org.jboss.logging.Logger.getLogger(JMSConnection.class);

	private static String tibcoServerName;
	private static String tibcoUserId;
	private static String tibcoPwd;
	private static String factoryName;
	private static String jndiServer;

	private static String _DATE_FORMAT = "MM/dd/yyyy HH:mm:ss";

	private String topicName;
	private Session session;
	private MessageProducer producer;
	Connection topicConnection;

	public JMSConnection(String topicName) throws JMSException, NamingException{
		this.topicName = topicName;
	//	String url = "tcp://localhost:7222";

		Hashtable hs = getTibcoEnv();
		InitialContext ic = new InitialContext(hs);
		log.info("Using HornetQ ConnectionFactory......");
		ConnectionFactory factory = JndiUtil.lookup(ic, "jms/RemoteConnectionFactory");
		topicConnection = factory.createConnection(tibcoUserId,tibcoPwd);
		session = topicConnection.createSession(false, Session.AUTO_ACKNOWLEDGE);
		Topic topic = JndiUtil.lookup(ic, "jms/topic/"+topicName);
		producer = session.createProducer(topic);



	}
	private Hashtable<String,String> getTibcoEnv(){

		Hashtable<String, String> env= new Hashtable<String, String>();

		env.put(Context.INITIAL_CONTEXT_FACTORY,"org.jboss.naming.remote.client.InitialContextFactory");
		env.put(Context.PROVIDER_URL,jndiServer);
		env.put(Context.SECURITY_PRINCIPAL, tibcoUserId);
		env.put(Context.SECURITY_CREDENTIALS, tibcoPwd);

		return env;

	}
	public static void setConnectionInfo(String serverName,String userId, String pwd,String tibcofactoryName,String tibcoJndi){
		tibcoServerName = serverName;
		tibcoUserId = userId;
		tibcoPwd = pwd;
		factoryName = tibcofactoryName;
		jndiServer = tibcoJndi;
	}

	public void sendMessage(OpsManagerMessage msg){

		try {
			Message mm = getMessage(msg.getData());
			mm.setStringProperty("type", msg.getMessageType());
			producer.send(mm);


		} catch (JMSException e) {
			log.error("Topic Name: (" + topicName + ")", e);
		}

	}
	private synchronized Message getMessage(Object obj){

		Message msg = null;
		try {
			msg =  session.createMessage();
			Method[] methods = obj.getClass().getMethods();

			for (int i=0;i<methods.length;++i){
				Method method = methods[i];
				if ( method.getName().startsWith("get")) {
					String methodName = method.getName().substring(3).toLowerCase();

					// remove _ from the property

					if ("_".equals(methodName.substring(0, 1))){
						methodName = methodName.substring(1);
					}

					Object methodValue  =  method.invoke(obj);
					String msgValue = "";
					if (methodValue != null){
						if (methodValue  instanceof java.util.Date ){
							msgValue = getDateFormat((java.util.Date) methodValue);
						}
						else {
							msgValue = methodValue.toString();
						}
					}
					msg.setStringProperty(methodName, msgValue);
//					System.out.println("Property Name= " + methodName + "=== Value=" + msgValue);
				}
			}

		} catch (JMSException e) {
			log.error("Topic Name: (" + topicName + ")", e);
		} catch (IllegalAccessException e) {
			log.error("Topic Name: (" + topicName + ")", e);
		} catch (InvocationTargetException e) {
			log.error("Topic Name: (" + topicName + ")", e);
		}
		return msg;
	}


	private String getAnnotationValue(Method method) {
		String attrName = null;

	//	FilterAttribute fa = method.getAnnotation(FilterAttribute.class);
		Object fa = null;
		if ( fa != null) {
		//	attrName=fa.value();
			log.info("Attribute name = " + attrName);
		}
		return attrName;
	}

	private String getDateFormat(Date dt) {
		String returnValue ="";
		SimpleDateFormat sdf = new SimpleDateFormat(_DATE_FORMAT);
		returnValue = sdf.format(dt);
		return returnValue;
	}
	public void stop() {
		try {
			if (topicConnection != null) {
				topicConnection.close();
				log.info("Topic Name: (" + topicName + ") Connection closed...");
			}
		} catch (JMSException e) {
			log.error( e );
		}
	}
}
