package aff.confirm.common.util;

import java.util.concurrent.Callable;
import org.jboss.logging.Logger;


/**
 * @author rnell
 * @date 2015-07-01
 */
public class RetryTool {
    private static final Logger log = Logger.getLogger( RetryTool.class );
    protected RetrySpec retrySpec;
    protected int count = 0;
    protected boolean success = false;

    public RetryTool(RetrySpec retrySpec) {
        this.retrySpec = retrySpec;
    }

    public <T> T exec( Callable<T> callable ) throws Exception {
        T result = null;
        do {
            try {
                log.debug("Trying");
                result = callable.call();
                log.debug("Success");

                success();
            } catch (Exception e) {
                log.debug("Failed with " + e.toString());
                if( fail() ) throw e;
            }

        } while( retry() );

        return result;
    }

    public int getCount() {
        return count;
    }

    /**
     *
     * @return true if caller should give up, else true and caller should try again
     */
    protected boolean fail()  {
        if( retrySpec == null )
            return true;

        count++;
        boolean rc = false;
        success = false;

        if( count < retrySpec.retryCount ) {
            try {
                Thread.sleep( retrySpec.retryInterval );
            } catch (InterruptedException e) {
                rc = true;
            }
        }
        else {
            rc = true;
        }

        return rc;
    }


    protected void success() {
        success=true;
    }


    protected boolean retry() {
        return !success;
    }
}
