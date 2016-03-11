package aff.confirm.common.util;


import org.jboss.logging.Logger;

import javax.management.MBeanServer;
import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.lang.management.ManagementFactory;
import java.lang.reflect.InvocationTargetException;
import java.util.Properties;

/**
 * @author rnell
 * @date 2014-10-29
 * @copyright Amphora Inc. 2014
 */
public class MBeanUtil {
    private static final Logger log = Logger.getLogger(MBeanUtil.class);

    public static final void register(ObjectName on, Object o) {
        MBeanServer mbeanServer = ManagementFactory.getPlatformMBeanServer();
        unregister(on);
        try {
            mbeanServer.registerMBean(o, on);
        } catch (Exception e) {
            log.error("Failed to register MBean: " + on, e);
            throw new RuntimeException(e);
        }
    }

    public static final boolean unregister(ObjectName on) {
        if( on == null )
            throw new IllegalArgumentException( "Unexpected null argument");

        MBeanServer mbeanServer = ManagementFactory.getPlatformMBeanServer();
        boolean wasRegistered = false;
        if (mbeanServer.isRegistered(on)) {
            try {
                mbeanServer.unregisterMBean(on);
            } catch (Exception e) {
                log.error("Failed to unregisterMBean : " + on, e);
                throw new RuntimeException(e);
            }
            wasRegistered = true;
        }
        return wasRegistered;
    }

    public static final ObjectName makeName(String root, String type, String name, String service) throws MalformedObjectNameException {
        return new ObjectName(String.format("%s:type=%s,service=%s,name=%s", root,type, service, name));
    }


    public static void loadProperties( Object bean, String fileName ) throws IOException, InvocationTargetException, IllegalAccessException {
        String configDirName = System.getProperty("jboss.server.config.dir");

        File file = new File( configDirName, fileName );
        Logger.getLogger( bean.getClass() ).info( "Loading property file " + file.getAbsolutePath() );
        Properties p = new Properties();
        try {
            p.load( new FileReader( file ));
        } catch (IOException e) {
            throw new IOException( "Failed to load property file " + file.getAbsolutePath(), e );
        }

        try {
            BeanUtil.setProperties( p, bean );
        } catch (Exception e) {
            throw new IOException( "Failed to set properties of bean class " + bean.getClass().getName() + " from file " + file.getAbsolutePath(), e );
        }

    }

}
