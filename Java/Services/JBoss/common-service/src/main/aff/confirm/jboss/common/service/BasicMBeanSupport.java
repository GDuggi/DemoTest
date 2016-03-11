package aff.confirm.jboss.common.service;

import aff.confirm.common.util.MBeanUtil;
import org.jboss.system.ServiceMBeanSupport;

import javax.management.ObjectName;

/**
 * @author rnell
 * @date 2015-07-02
 */
public class BasicMBeanSupport extends ServiceMBeanSupport {

    protected ObjectName objectName;
    private boolean useDefaultPropertiesFileName;
    private String alternatePropertiesFileName;

    private BasicMBeanSupport(String objectNameStr, boolean useDefaultPropertiesFileName, String alternatePropertiesFileName ) {
        this.useDefaultPropertiesFileName = useDefaultPropertiesFileName;
        this.alternatePropertiesFileName = alternatePropertiesFileName;
        try {
            objectName = new ObjectName( objectNameStr );
        } catch (Exception e) {
            throw new RuntimeException( e );
        }
        log.info("Constructor for " + objectName );
    }

    public BasicMBeanSupport(String objectNameStr, String propertiesFileName ) {
        this(objectNameStr, false, propertiesFileName);
    }

    public BasicMBeanSupport(String objectNameStr) {
        this(objectNameStr, true, null);
    }

    public void postConstruct() throws Exception {
        log.info("postConstruct");
        String propertyFileName = String.format( "%s.properties", this.getClass().getSimpleName() );

        String fileName =  useDefaultPropertiesFileName ? propertyFileName : alternatePropertiesFileName;

        if(fileName != null ) {
            MBeanUtil.loadProperties(this, fileName);
        }
        MBeanUtil.register(objectName, this);
        create();
        start();
    }

    public void preDestroy() throws Exception {
        log.info("preDestroy");
        MBeanUtil.unregister(objectName);
        stop();
        destroy();
    }

}
