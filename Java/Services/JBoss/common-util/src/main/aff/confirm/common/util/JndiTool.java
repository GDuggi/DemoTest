package aff.confirm.common.util;

import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.io.Closeable;
import java.io.IOException;
import java.util.Hashtable;
import java.util.concurrent.Callable;

/**
 * <p/>
 * This class solves the problem of having the wrong ClassLoader when interacting with JBoss.
 * This happens when an MBean interacts with JBoss<br/>
 * <p/>
 * See more in <a href="https://access.redhat.com/solutions/782483">Red Hat Solution</a>
 *
 * @author rnell
 * @date 2015-07-01
 */
public class JndiTool implements Closeable {
    private static RetrySpec retrySpecDefault = new RetrySpec(1000, 10);

    private Context context;
    private RetrySpec retrySpec;

    /**
     * Constructor that uses the default InitialContext with no properties
     *
     * @throws NamingException
     */
    public JndiTool() throws NamingException {
        this(new Hashtable(), retrySpecDefault);
    }

    public JndiTool(RetrySpec retrySpec) throws NamingException {
        this(new Hashtable(), retrySpec);
    }

    public JndiTool(Hashtable env) throws NamingException {
        this(env, retrySpecDefault);
    }

    public JndiTool(Hashtable env, RetrySpec retrySpec) throws NamingException {
        this( newContext( env ), retrySpec );
    }

    public JndiTool(Context context, RetrySpec retrySpec) {
        this.context = context;
        this.retrySpec = retrySpec;
    }

    public <T> T lookup( final String s) throws NamingException {
        T lookup;
        RetryTool retry = new RetryTool(retrySpec);

        try (SetTccl tccl = new SetTccl()) {
            try {

                lookup = retry.exec(new Callable<T>() {
                    @Override
                    public T call() throws Exception {
                        return (T) context.lookup(s);
                    }
                });

            } catch (Exception e) {
                throw wrap(e);
            }

        } catch (IOException e) {
            throw wrap(e);
        }
        return lookup;
    }

    private Context getContext() throws NamingException {
        return context;
    }

    @Override
    public synchronized void close() throws IOException {
        if (context != null)
            try {
                context.close();
            } catch (NamingException e) {
                throw new IOException(e);
            }
    }

    private static NamingException wrap(Throwable t) {
        if (t instanceof NamingException) {
            return (NamingException) t;
        }
        NamingException ne = new NamingException();
        ne.initCause(t);
        return ne;
    }

    public static <T> T lu(String s) throws NamingException {
        return new JndiTool().lookup(s);
    }

    public static <T> T lu(Hashtable env, String s) throws NamingException {
        return new JndiTool(env).lookup(s);
    }

    public static <T> T lu(Context context, String s) throws NamingException {
        return lu(context, s, retrySpecDefault);
    }

    public static <T> T lu(Context context, String s, RetrySpec retrySpec) throws NamingException {
        return new JndiTool(context, retrySpec).lookup(s);
    }

    public static InitialContext newContext() throws NamingException {
        return newContext(new Hashtable());
    }

    public static InitialContext newContext(Hashtable env) throws NamingException {
        try (SetTccl ignored = new SetTccl()) {
            return new InitialContext(env);
        } catch (IOException e) {
            throw wrap(e);
        }
    }

}
