package aff.confirm.jboss.jms;


import aff.confirm.common.util.JndiUtil;
import org.jboss.logging.Logger;

import javax.jms.*;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.util.Hashtable;

/**
 * User: srajaman
 * Date: Jul 2, 2008
 * Time: 2:57:57 PM
 */
public class JMSHornetQUtil {
    final static String JNDI_FACTORY = "org.jboss.naming.remote.client.InitialContextFactory";

    public static InitialContext getInitialContext(String pFactory, String pProviderUrl,
                                                   String pUserId, String pPassword) throws NamingException {
        try {
            Class.forName("org.jboss.naming.remote.client.InitialContextFactory");
        } catch (ClassNotFoundException e) {
            Logger.getLogger(JMSHornetQUtil.class).info("ClassNotFoundException: " , e );
            throw new RuntimeException( e );
        }

        Logger.getLogger(JMSHornetQUtil.class).info(Context.INITIAL_CONTEXT_FACTORY + "=" + pFactory);
//        Logger.getLogger(JMSHornetQUtil.class).info(Context.PROVIDER_URL + "=" + pProviderUrl);
//        Logger.getLogger(JMSHornetQUtil.class).info(Context.SECURITY_PRINCIPAL + "=" + pUserId);
//        Logger.getLogger(JMSHornetQUtil.class).info(Context.SECURITY_CREDENTIALS + "=" + pPassword);

        Hashtable env = new Hashtable();
        env.put(Context.INITIAL_CONTEXT_FACTORY, pFactory);
        env.put(Context.PROVIDER_URL, pProviderUrl);
        env.put(Context.SECURITY_PRINCIPAL, pUserId);
        env.put(Context.SECURITY_CREDENTIALS, pPassword);
        return new InitialContext(env);
    }

    public static Connection getJMSConnection(String server, String userId, String pwd) throws JMSException, NamingException {

        Connection connection = null;

        InitialContext context;
        context = new InitialContext(getEnv(server, userId, pwd));
        ConnectionFactory factory = JndiUtil.lookup(context, "jms/RemoteConnectionFactory");
        connection = factory.createConnection(userId,pwd);
        Logger.getLogger(JMSHornetQUtil.class).info("Connected to the Hornet Q server = " + server + " successfully.....");
        return connection;
    }

    public static QueueConnection getJMSQueueConnection(String server, String userId, String pwd) throws JMSException, NamingException {
        QueueConnection connection = null;
        Logger.getLogger(JMSHornetQUtil.class).info("Creating initial context...");
        InitialContext context = getInitialContext(JNDI_FACTORY, server, userId, pwd);

        Logger.getLogger(JMSHornetQUtil.class).info("Looking up java:jms/RemoteConnectionFactory...");
        QueueConnectionFactory factory = JndiUtil.lookup(context,"java:jms/RemoteConnectionFactory");

        Logger.getLogger(JMSHornetQUtil.class).info("Creating QueueConnection...");
        connection = factory.createQueueConnection(userId,pwd);
        Logger.getLogger(JMSHornetQUtil.class).info("Connected to the HornetQ server = " + server + " successfully.....");
        return connection;
    }

    public static Queue getJMSQueue(String queueName,String server,String userId,String password) throws NamingException {
        Queue queue ;
        InitialContext context;

        context = new InitialContext(getEnv(server,userId,password));
        queue = JndiUtil.lookup(context,queueName);
        return queue;
    }

    private static Hashtable getEnv(String server, String userId,String pwd) {
        Hashtable env = new Hashtable();
        env.put(Context.INITIAL_CONTEXT_FACTORY, JNDI_FACTORY);
        env.put(Context.PROVIDER_URL, server  );
        env.put(Context.SECURITY_PRINCIPAL, userId);
        env.put(Context.SECURITY_CREDENTIALS, pwd);
        return env;

    }
}
