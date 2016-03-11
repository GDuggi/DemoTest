package aff.confirm.mbeans.servicebean;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 11/21/12
 * Time: 1:15 PM
 */
public interface AffConfirmServiceMBean {

    /** The Service.stop has completed */
    public static final int STOPPED  = 0;
    /** The Service.stop has been invoked */
    public static final int STOPPING = 1;
    /** The Service.start has been invoked */
    public static final int STARTING = 2;
    /** The Service.start has completed */
    public static final int STARTED  = 3;
    /** There has been an error during some operation */
    public static final int FAILED  = 4;
    /** The Service.destroy has completed */
    public static final int DESTROYED = 5;
    /** The Service.create has completed */
    public static final int CREATED = 6;
    /** The MBean has been created but has not completed MBeanRegistration.postRegister */
    public static final int UNREGISTERED = 7;
    /** The MBean has been created and has completed MBeanRegistration.postRegister */
    public static final int REGISTERED = 8;

    void  start() ;
    void  stop() ;
    int getState() ;
    String getStateString();
    String getName();




}
