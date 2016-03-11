package aff.confirm.common.util;

import java.io.Closeable;
import java.io.IOException;

/**
 * Sets the Thread Context ClassLoader (TCCL)<br/><br/>
 * The EE spec does not define what classloader the TCCL should be set to when invoking MBeans
 * because MBeans are not EE managed components. It does define this for EJBs, Servlets etc,
 * so you can rely on the TCCL already set when using these.
 * The JMX spec only specifies classloaders to use when creating MBeans and deserializing parameters
 * and return values 1. It doesn't specify anything about TCCL and thus JBoss does not change
 * the TCCL when invoking MBeans.<br/>
 *
 * See more in <a href="https://access.redhat.com/solutions/782483">Red Hat Solution</a>
 *
 * @author rnell
 * @date 2015-07-01
 *
 */
public class SetTccl implements Closeable {

    private final ClassLoader originalClassLoader;

    public SetTccl() {
        originalClassLoader = Thread.currentThread().getContextClassLoader();
        Thread.currentThread().setContextClassLoader ( this.getClass().getClassLoader() ); //TCCL Classloader
    }

    @Override
    public void close() throws IOException {
        Thread.currentThread().setContextClassLoader ( originalClassLoader );
    }

}
