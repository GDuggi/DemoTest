package aff.confirm.mbeans.servicebean;

import org.jboss.logging.Logger;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 11/21/12
 * Time: 1:35 PM
 */
public abstract class AffConfirmServiceMBeanSupport implements AffConfirmServiceMBean {
    protected  int state;
    protected  String stateString;
    protected Logger log = Logger.getLogger(this.getClass());

    @Override
    public void start() {
        try {
            this.startService();
            state = STARTED;
            stateString = "STARTED";
        }
        catch (Exception e) {
            state = FAILED;
            stateString = "FAILED";
        }
    }

    @Override
    public void stop()  {
        try {
            this.stopService();
            state = STOPPED;
            stateString = "STOPPED";
        }
        catch ( Exception e) {
            state = FAILED;
            stateString = "FAILED";
        }
    }

    public  String getName() {
        return  this.getClass().getSimpleName();
    }

    protected void startService() throws  Exception{
    }

    protected void stopService() throws Exception{
    }

    @Override
    public int getState() {
        return state;
    }

    @Override
    public String getStateString() {
        return stateString;
    }
}
