package aff.confirm.common.util;

import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;

/**
 * @author rnell
 * @date 2015-06-29
 */
public class JndiUtil {
    public static int DEFAULT_RETRY_COUNT = 10;
    public static long DEFAULT_RETRY_INTERVAL = 1000;
    public static InitialContext ic = null;
    private long retryInterval;
    private Context context;
    private int retryCount = -1;


    public JndiUtil(Context context, long retryInterval, int retryCount) {
        this.context = context;
        this.retryCount = retryCount;
        this.retryInterval = retryInterval;
    }

    public JndiUtil(long retryInterval, int retryCount) {
        this( null, retryInterval, retryCount);
    }

    public <T> T lu(Context ctx, String name) throws NamingException {
        return lookup( ctx, name, retryInterval, retryCount );
    }

    public <T> T lu(String name) throws NamingException, InterruptedException {
        return lookup( context, name, retryInterval, retryCount );
    }

    public static <T> T lookup(String name) throws NamingException {
            return lookup(null, name, DEFAULT_RETRY_INTERVAL, DEFAULT_RETRY_COUNT);
    }

    public static <T> T lookup(Context ctx, String name) throws NamingException {
        return lookup(ctx, name, DEFAULT_RETRY_INTERVAL, DEFAULT_RETRY_COUNT);
    }

    public static <T> T lookup(Context ctx, String name, long intervalTime, int intervalCount) throws NamingException {
        T t;

        // Use default context if not set
        if( ctx == null ) {
            // Create default context if null
            if( ic == null ) {
                synchronized ( JndiUtil.class ) {
                    if( ic == null ) {
                        ic = new InitialContext();
                    }
                }
            }
            ctx = ic;
        }

        int i = 0;
        while (true) {
            try {
                t = (T) ctx.lookup(name);
                break;
            } catch (NamingException e) {
                if (i >= intervalCount) {
                    throw e;
                }
                try {
                    Thread.sleep(intervalTime);
                } catch (InterruptedException e1) {
                    throw new RuntimeException( e1 );
                }
            }
            ++i;
        }

        return t;
    }

}
