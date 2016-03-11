/**
 * User: islepini
 * Date: Jun 27, 2003
 * Time: 2:05:51 PM
 * To change this template use Options | File Templates.
 */
package aff.confirm.jboss.queuemonitor;

public class MonitorQueue {
    private String name;
    private int treshHold;

    public boolean isFailed() {
        return failed;
    }

    public void setFailed(boolean failed) {
        this.failed = failed;
    }

    private boolean failed;

    public int getQueueDepth() {
        return queueDepth;
    }

    public void setQueueDepth(int queueDepth) {
        this.queueDepth = queueDepth;
    }

    private int queueDepth;
    public boolean isNotified() {
        return isNotified;
    }

    public void setNotified(boolean notified) {
        isNotified = notified;
    }

    private boolean isNotified;
    public MonitorQueue(String name, int treshHold) {
        this.name = name;
        this.treshHold = treshHold;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getTreshHold() {
        return treshHold;
    }

    public void setTreshHold(int treshHold) {
        this.treshHold = treshHold;
    }

    public String toString(){
        if(!failed)
            return name+" treshHold = "+treshHold+", depth: "+queueDepth;
        else
            return "failed to retieve data for queue: "+name;
    }
}
