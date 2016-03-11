package aff.confirm.opsmanager.creditmargin.common;


import aff.confirm.common.util.JndiUtil;
import org.jboss.logging.Logger;

import javax.jms.Connection;
import javax.jms.ConnectionFactory;
import javax.jms.JMSException;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.util.Hashtable;

/**
 * User: srajaman
 * Date: Dec 3, 2008
 * Time: 1:3    8:19 PM
 */
public class JMSUtil {
    private static Logger log = Logger.getLogger( JMSUtil.class.getName() );

    public static Connection getQueueConnection(String jndiServer, String userId, String pwd) throws JMSException, NamingException {
            ConnectionFactory factory = getConnectionFactory(jndiServer,userId,pwd);
            Connection connection = factory.createConnection(userId,pwd);
            Logger.getLogger(JMSUtil.class).info("Connected to the JMS server = " + jndiServer + " successfully.....");
            log.info("Connected to HornetQ...");
            return connection;
        }

        private static ConnectionFactory getConnectionFactory(String jndiServer, String userId,String pwd) throws NamingException {
            Hashtable env = new Hashtable();
            env.put(Context.INITIAL_CONTEXT_FACTORY, "org.jboss.naming.remote.client.InitialContextFactory");
            env.put(Context.PROVIDER_URL, jndiServer  );
            env.put(Context.SECURITY_PRINCIPAL, userId);
            env.put(Context.SECURITY_CREDENTIALS, pwd);
            InitialContext ic  = new InitialContext(env);
            ConnectionFactory factory = JndiUtil.lookup(ic,"jms/RemoteConnectionFactory");
            return factory;
        }
}
